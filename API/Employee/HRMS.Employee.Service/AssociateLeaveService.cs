using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    /// <summary>
    /// Service handling associate leave data upload and template retrieval.
    /// </summary>
    public class AssociateLeaveService : IAssociateLeaveService
    {
        #region  Global Varibles
        private readonly ILogger<AssociateLeaveService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IOrganizationService m_organizationService;
        private readonly EmailConfigurations m_EmailConfigurations;


        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="AssociateLeaveService"/>.
        /// </summary>
        /// <param name="employeeDBContext">Employee database context.</param>
        /// <param name="logger">Application logger.</param>
        /// <param name="organizationService">Service used to send notifications.</param>
        /// <param name="emailConfigurations">Email configuration options.</param>
        public AssociateLeaveService(EmployeeDBContext employeeDBContext, ILogger<AssociateLeaveService> logger, IOrganizationService organizationService,
            IOptions<EmailConfigurations> emailConfigurations)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            m_organizationService = organizationService;
            m_EmailConfigurations = emailConfigurations.Value;


        }
        #endregion

        #region Uploadleavedata
        /// <summary>
        /// Uploads associate leave information from an Excel file.
        /// </summary>
        /// <param name="file">Excel file containing leave data.</param>
        /// <returns>Result indicating success or failure.</returns>
        public async Task<ServiceResponse<bool>> UploadLeaveData(IFormFile file)
        {

            var response = new ServiceResponse<bool>();
           
            List<HeaderNames> headerNames = new List<HeaderNames>();
            string message = string.Empty;
            string columnName = string.Empty;
            string columnInput = string.Empty;
            using (var dbContext = m_EmployeeContext.Database.BeginTransaction())
            {
                try
                {
                    if (file != null)
                    {
                        var extention = Path.GetExtension(file.FileName);

                        #region Validation
                        if (extention != ".xlsx")
                        {
                            response.IsSuccessful = false;
                            response.Message = "Please provide .xlsx file format";
                            return response;
                        }
                        Stream stream = file.OpenReadStream();
                        using (ExcelPackage package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets["Report"];
                            if (worksheet == null)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Invalid file";
                                return response;
                            }
                            //Validate given headers with ExcelHeadersName
                            message = ValidateHeaders(worksheet);
                            if (message != "")
                            {
                                message = message + "Please Verify with ExcelTemplate";
                                SendNotification(message);
                                response.IsSuccessful = false;
                                response.Message = "Column names are not valid";
                                return response;
                            }
                            //delete empty rows from excel sheet
                            worksheet = RemoveEmptyRowsFromExcel(worksheet);
                            int rowCount = worksheet.Dimension.Rows;
                            int ColCount = worksheet.Dimension.End.Column;
                            var headersCount = GetHeaderNames().Count();
                            if (ColCount != headersCount)
                            {
                                response.IsSuccessful = false;
                                response.Message = "Invalid file";
                                return response;
                            }
                            if (rowCount == 1)
                            {
                                response.IsSuccessful = false;
                                response.Message = "No data to process";
                                return response;
                            }
                            #endregion

                            // format the excel data as list
                            var result= FormatFileData(worksheet, rowCount, ColCount);
                            List<AssociateLeave> associateLeaves = new List<AssociateLeave>();
                            associateLeaves = result.Item1;
                            if (associateLeaves.Count == 0)
                            {
                                columnName = result.Item2;
                                columnInput = result.Item3;
                            }
                            DateTime? earliestFromDate = associateLeaves.Min(x => x.FromDate);
                            DateTime? startOfMonth = new DateTime(earliestFromDate.Value.Year, earliestFromDate.Value.Month, 1);
                            DateTime currentDate = DateTime.Now.Date;
                            var employeeCodes = associateLeaves
                                    .Where(x => x.FromDate.Date >= startOfMonth && x.FromDate.Date <= currentDate)
                                    .Select(x => x.EmployeeCode)
                                    .Distinct()
                                    .ToList();

                            var leavesToRemove = m_EmployeeContext.AssociateLeave
                                    .Where(x => employeeCodes.Contains(x.EmployeeCode)
                                                && x.FromDate >= startOfMonth
                                                && x.FromDate <= currentDate)
                                    .ToList();

                            // delete all data from associateleave

                            m_EmployeeContext.AssociateLeave.RemoveRange(leavesToRemove);

                            //insert list of leave associates
                            foreach (var associate in  associateLeaves)
                            {
                                m_EmployeeContext.AssociateLeave.Add(associate);
                            }

                            await m_EmployeeContext.SaveChangesAsync();
                            response.IsSuccessful = true;
                            await dbContext.CommitAsync();
                        }
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "File not found";
                    }
                }

                catch (Exception e)
                {
                    dbContext.Rollback();
                    string errorMessage = string.Empty;
                    if (columnName != "" && columnInput != "")
                    {
                        errorMessage = " Input is not valid. at cell-column name:" + columnName + ", " + columnInput;
                    }
                    else
                    {
                        errorMessage = e.Message;
                    }
                    SendNotification(errorMessage);
                    response.IsSuccessful = false;
                    response.Message = "Processing is failed";
                    m_Logger.LogError("Error occured while adding leave data into AssociaeLeave table ", e.StackTrace);
                }
            }
            return response;
        }
        #endregion

        #region GetTemplateFile
        /// <summary>
        /// Returns the Excel template used for uploading leave data.
        /// </summary>
        /// <returns>Template file details.</returns>
        public FileDetail GetTemplateFile()
        {
            string content = string.Empty;
            FileDetail file = new FileDetail();
            try
            {
                string filePath = Environment.CurrentDirectory + "/Templates/LeaveDataTemplate.xlsx";
                if (System.IO.File.Exists(filePath))
                {
                    string fileName = Path.GetFileName(filePath);
                    using (Stream stream = File.OpenRead(filePath))
                    {
                        using (var binaryReader = new BinaryReader(stream))
                        {
                            var fileContent = binaryReader.ReadBytes((int)stream.Length);
                            file.FileName = fileName;
                            file.FileType = @"application/xlsx";
                            file.Content = Convert.ToBase64String(fileContent);
                            file.FileContent = fileContent;

                        }
                    }
                }
                            

            }
            catch (Exception e)
            {
                m_Logger.LogError("Error occured while adding leave data into AssociaeLeave table ", e.StackTrace);
            }

            return file;
        }
        #endregion

        #region Private methods
        private (List<AssociateLeave>, string, string) FormatFileData(ExcelWorksheet worksheet, int rowCount, int ColCount)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable;
            string columnName = string.Empty;
            StringBuilder associateNames = new StringBuilder();
            string columnInput = string.Empty;
            dataTable = new DataTable(worksheet.Name);
            for (int row = 1; row <= rowCount; row++)
            {
                if (row == 1)
                {
                    for (int column = 1; column <= ColCount; column++)
                    {
                        string header = worksheet.Cells[row, column].Value.ToString().Trim();

                        dataTable.Columns.Add(header.Replace(" ", ""));
                    }

                }
                else
                {
                    DataRow dr = dataTable.NewRow();
                    for (int column = 0; column < dataTable.Columns.Count; column++)
                    {
                        columnName = worksheet.Cells[1, column + 1].Value.ToString();
                        columnInput = "row no:" + row;

                        dr[column] = worksheet.Cells[row, column + 1].Value != null ? worksheet.Cells[row, column + 1].Value : "";

                        if (dr[column] != "" && (columnName.ToLower() == "from" || columnName.ToLower() == "to"))
                        {
                            double d = double.Parse(dr[column].ToString());
                            DateTime date = DateTime.FromOADate(d);
                            dr[column] = Common.Utility.GetDateTimeInIST(date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //dr[column] = GetDateTimeInIST(date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); 
                        }

                    }
                    dataTable.Rows.Add(dr);
                }
            }

            List<AssociateLeaveDetailsFromExcel> associateLeaveDetails = ConvertDataTable<AssociateLeaveDetailsFromExcel>(dataTable);
            var lkValues = m_EmployeeContext.lkValue.ToList();
            List<AssociateLeave> associateLeaves = new List<AssociateLeave>();
            foreach (var associate in associateLeaveDetails)
            {
                try
                {
                    bool isDataAvailable = associate.EmployeeNo == "" && associate.Name == "" ? false : true;
                    if (isDataAvailable)
                    {
                        //var lkValues = m_EmployeeContext.lkValue.ToList();

                        AssociateLeave associateLeave = new AssociateLeave()
                        {
                            AssociateName = associate.Name,
                            EmployeeCode = associate.EmployeeNo,
                            ManagerCode = associate.ManagerNo,
                            ManagerName = associate.ManagerName,
                            LeaveType = associate.LeaveType,
                            LeaveTypeId = lkValues.Where(x => x.ValueName.ToLower() == associate.LeaveType.ToLower()).FirstOrDefault().ValueKey,
                            FromDate = Common.Utility.GetDateTimeInIST(DateTime.ParseExact(associate.From, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                            ToDate = Common.Utility.GetDateTimeInIST(DateTime.ParseExact(associate.To, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                            //FromDate = GetDateTimeInIST(DateTime.ParseExact(associate.From, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                            //ToDate =  GetDateTimeInIST(DateTime.ParseExact(associate.To, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                            Session1Id = lkValues.Where(x => x.ValueName.ToLower() == associate.Session1.ToLower()).FirstOrDefault().ValueKey,
                            Session2Id = lkValues.Where(x => x.ValueName.ToLower() == associate.Session2.ToLower()).FirstOrDefault().ValueKey,
                            Session1Name = associate.Session1,
                            Session2Name = associate.Session2,
                            NumberOfDays = Convert.ToDecimal(associate.Days)
                        };
                        associateLeaves.Add(associateLeave);
                    }
                }
                catch (Exception ex)
                {
                    if (associate != null && associate.Name != null)
                    {
                        associateNames.Append($"{associate.Name},");
                        m_Logger.LogError($"error for the associate:{associate.Name}");
                    }
                }
            }
            if (associateNames.Length > 1)
            {
                SendNotification(associateNames.ToString());
            }
            return (associateLeaves, columnName, columnInput);
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                        break;
                    }
                    else
                        continue;
                }
            }
            return obj;
        }


        private void SendNotification(string message)
        {
            var DepartmentDl = m_organizationService.GetAllDepartmentsWithDLs().Result;
            string HREmailAddress = string.Empty;
            if (DepartmentDl.Items != null)
            {
                HREmailAddress = DepartmentDl.Items.Where(x => x.DepartmentCode == "HR").FirstOrDefault().DepartmentDLAddress;
            }
            NotificationDetail notificationDetail = new NotificationDetail();
            notificationDetail.EmailBody = "<html><h3>Hi Team, </h3><p>Leave attendance process has failed due to this issue :" + message + " </p><p>Thanks!</p></html>";
            notificationDetail.Subject = "Leave attendance process has failed";
            notificationDetail.ToEmail = HREmailAddress;
            notificationDetail.CcEmail = m_EmailConfigurations.CcEmail;
            notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;
            m_organizationService.SendEmail(notificationDetail);
        }
        

        private List<HeaderNames> GetHeaderNames()
        {
            string jsonPath = Path.Combine(Environment.CurrentDirectory, "Templates", "ExcelHeadersForLeave.json"); //Linux
            //jsonPath = Environment.CurrentDirectory + "\\Templates\\ExcelHeadersForLeave.json"; //Windows
            using StreamReader reader = new StreamReader(jsonPath);
            var json = reader.ReadToEnd();
            var jarray = JArray.Parse(json);
            List<HeaderNames> headersName = new List<HeaderNames>();
            foreach (var item in jarray)
            {
                HeaderNames name = item.ToObject<HeaderNames>();
                headersName.Add(name);
            }
            return headersName;
        }
        private string ValidateHeaders( ExcelWorksheet worksheet)
        {
           var headerNames= GetHeaderNames();
            int row = 1;
            string message = string.Empty;
            var ColCount = worksheet.Dimension.Columns;

            for (int column = 1; column <= ColCount; column++)
            {
                // check column headers are matched with defined headerNames
                string headerName = worksheet.Cells[row, column].Value.ToString().Trim();
                var isHeaderExist = headerNames.Where(header => header.Name.ToString() == headerName).FirstOrDefault();
                if (isHeaderExist == null)
                {
                    message += (message != "" ? "\n " : "") + " '" + headerName + "'" + " column name is not valid. ";
                }
            }
            return message;
        }

        private ExcelWorksheet RemoveEmptyRowsFromExcel(ExcelWorksheet worksheet)
        {
            while (IsLastRowEmpty(worksheet))
                worksheet.DeleteRow(worksheet.Dimension.End.Row);
            return worksheet;
        }
        private bool IsLastRowEmpty(ExcelWorksheet worksheet)
            {
             var empties = new List<bool>();

            for (int column = 1; column <= worksheet.Dimension.End.Column; column++)
            {
                var rowEmpty = worksheet.Cells[worksheet.Dimension.End.Row, column].Value == null ? true : false;
                empties.Add(rowEmpty);
            }
            return empties.All(e => e);
            }

        #endregion
    }
}
