using AutoMapper;
using HRMS.KRA.Database;
using HRMS.KRA.Entities;
using HRMS.KRA.Infrastructure;
using HRMS.KRA.Infrastructure.Models;
using HRMS.KRA.Types;
using HRMS.KRA.Types.External;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.KRA.Service
{
    public class KRAService : IKRAService
    {
        #region Global Variables
        private readonly ILogger<KRAService> m_Logger;
        private readonly KRAContext m_KRAContext;
        private readonly IMapper m_Mapper;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IOrganizationService m_OrganizationService;
        private readonly IConfiguration m_configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        #endregion

        #region Constructor
        public KRAService(ILogger<KRAService> logger, KRAContext kraContext, IEmployeeService employeeService,
            IOrganizationService organizationService, IConfiguration configuration, IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            m_Logger = logger;
            m_KRAContext = kraContext;
            m_EmployeeService = employeeService;
            m_OrganizationService = organizationService;
            m_configuration = configuration;
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
            //CreateMapper
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<AspectMaster, AspectMaster>();
            //});

            //m_Mapper = config.CreateMapper();
        }
        #endregion

        #region GetAll Operators
        /// <summary>
        /// This method fetches all KRA Operator List.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Operator>> GetOperatorsAsync()
        {
            m_Logger.LogInformation("KRAService: Calling \"GetAll Operators\" method.");
            return await m_KRAContext.Operators.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAll TargetPeriods
        /// <summary>
        /// This method fetches all KRA Target PeriodList.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TargetPeriod>> GetTargetPeriodsAsync()
        {
            m_Logger.LogInformation("KRAService: Calling \"GetAll TargetPeriod\" method.");
            return await m_KRAContext.TargetPeriods.AsNoTracking().ToListAsync();
        }
        #endregion

        #region CreateKRADefinition
        /// <summary>
        /// Create KRA Definitions
        /// </summary>
        /// <param name="kraDefinitionIn"></param>
        /// <returns></returns>
        //public async Task<ServiceResponseMessage> CreateKRADefinition(KRADefinition kraDefinitionIn)
        //{
        //    var response = new ServiceResponseMessage();
        //    response.IsSuccessful = false;
        //    try
        //    {
        //        int maxScale = m_KRAContext.KRAScaleMasters.Where(x => x.ScaleID == kraDefinitionIn.ScaleID).Select(x => x.MaximumScale).FirstOrDefault();
        //        if (kraDefinitionIn.TargetValue > maxScale)
        //        {
        //            response.Message = "target value cannot be greater than maximum scale";
        //            return response;
        //        }
        //        m_KRAContext.kRADefinitions.Add(kraDefinitionIn);
        //        response.IsSuccessful = await m_KRAContext.SaveChangesAsync() > 0 ? true : false;

        //        if (!response.IsSuccessful)
        //        {
        //            response.Message = "KRADefinition not created.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccessful = false;
        //        response.Message = "Unable to save KRADefinition.";

        //        m_Logger.LogError("Error Occured in Save() method in CreateKRADefinition" + ex.StackTrace);
        //    }
        //    return response;
        //}
        #endregion

        #region Generate KRA PDF      

        #region GenerateKRAPdf 
        /// <summary>
        /// GetKRAPdfData  Generate KRA Pdf For All Associates
        /// </summary>
        public async Task<AssociateKRA> GetKRAPdfData(string employeeCode, int financialYearId, int? departmentId = null, int? roleId = null)
        {
            AssociateKRA associateKRA = new AssociateKRA();
            // Get FinancialYear By Id
            var financialYear = await m_OrganizationService.GetFinancialYearByIdAsync(financialYearId);
            if (financialYear != null)
            {
                associateKRA.FinancialYear = financialYear.FinancialYearName;
            }

            // Get Employees By Role    
            var associates = await m_EmployeeService.GetEmployeesByRole(employeeCode, departmentId, roleId);
            if (associates.IsSuccessful)
            {
                associateKRA.AssociateRoleTypes = associates.Items;
            }

            // Get Grade Role Types
            /*var gradeRoles = await m_OrganizationService.GetAllGradeRoleTypesAsync();

            foreach (AssociateRoleType associate in associateKRA.AssociateRoleTypes)
            {
                if (associate != null)
                {
                    associate.GradeRoleTypeId = gradeRoles.Items.Where(d => d.GradeId == associate.GradeId && d.DepartmentId == associate.DepartmentId && d.RoleTypeId == associate.EmployeeRoleId)
                        .Select(c => c.GradeRoleTypeId).FirstOrDefault();
                }
            }*/

            // Get Role Types
            var roles = await m_OrganizationService.GetAllRoleTypesAsync();

            // Get KRAs By Role
            if (!string.IsNullOrWhiteSpace(employeeCode))
            {
                if (associateKRA.AssociateRoleTypes != null && associateKRA.AssociateRoleTypes.Count > 0)
                {
                    int roleTypeId = 0;
                    //roleTypeId = gradeRoles.Items.Where(g => g.DepartmentId == associateKRA.AssociateRoleTypes[0].DepartmentId
                    //&& g.GradeId == associateKRA.AssociateRoleTypes[0].GradeId
                    //&& g.RoleTypeId == associateKRA.AssociateRoleTypes[0].EmployeeRoleId)
                    //    .Select(r => r.GradeRoleTypeId).FirstOrDefault();
                    roleTypeId = associateKRA.AssociateRoleTypes[0].EmployeeRoleId;

                    associateKRA.KRADefinitions = (from d in m_KRAContext.Definitions
                                                   join aspList in m_KRAContext.Aspects on d.AspectId equals aspList.AspectId into aspectData
                                                   from asp in aspectData.DefaultIfEmpty()
                                                   join opList in m_KRAContext.Operators on d.OperatorId equals opList.OperatorId into opData
                                                   from op in opData.DefaultIfEmpty()
                                                   join mtList in m_KRAContext.MeasurementTypes on d.MeasurementTypeId equals mtList.MeasurementTypeId into mtype
                                                   from mt in mtype.DefaultIfEmpty()
                                                   join tpList in m_KRAContext.TargetPeriods on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                   from tp in tperiod.DefaultIfEmpty()
                                                   join scList in m_KRAContext.Scales on d.ScaleId equals scList.ScaleId into scale
                                                   from sc in scale.DefaultIfEmpty()
                                                   where d.FinancialYearId == financialYearId
                                                     && d.RoleTypeId == roleTypeId
                                                   select new KRADefinition
                                                   {
                                                       DepartmentId = 0,
                                                       RoleTypeId = d.RoleTypeId,
                                                       KRAAspectName = asp.AspectName,
                                                       KRAAspectMetric = d.Metric,
                                                       KRATargetPeriod = tp.TargetPeriodValue,
                                                       KRATargetValue = d.TargetValue,
                                                       KRAMeasurementType = mt.MeasurementTypeName,
                                                       KRATargetText = "",
                                                       Operator = op.OperatorValue
                                                   }).ToList<KRADefinition>();

                }
            }
            else if (roleId.HasValue)
            {

                associateKRA.KRADefinitions = (from d in m_KRAContext.Definitions
                                               join aspList in m_KRAContext.Aspects on d.AspectId equals aspList.AspectId into aspectData
                                               from asp in aspectData.DefaultIfEmpty()
                                               join opList in m_KRAContext.Operators on d.OperatorId equals opList.OperatorId into opData
                                               from op in opData.DefaultIfEmpty()
                                               join mtList in m_KRAContext.MeasurementTypes on d.MeasurementTypeId equals mtList.MeasurementTypeId into mtype
                                               from mt in mtype.DefaultIfEmpty()
                                               join tpList in m_KRAContext.TargetPeriods on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                               from tp in tperiod.DefaultIfEmpty()
                                               join scList in m_KRAContext.Scales on d.ScaleId equals scList.ScaleId into scale
                                               from sc in scale.DefaultIfEmpty()
                                               where d.FinancialYearId == financialYearId
                                                 && d.RoleTypeId == roleId
                                               select new KRADefinition
                                               {
                                                   DepartmentId = 0,
                                                   RoleTypeId = d.RoleTypeId,
                                                   KRAAspectName = asp.AspectName,
                                                   KRAAspectMetric = d.Metric,
                                                   KRATargetPeriod = tp.TargetPeriodValue,
                                                   KRATargetValue = d.TargetValue,
                                                   KRAMeasurementType = mt.MeasurementTypeName,
                                                   KRATargetText = "",
                                                   Operator = op.OperatorValue
                                               }).ToList<KRADefinition>();
            }
            else if (departmentId.HasValue && !roleId.HasValue)
            {
                if (roles.IsSuccessful)
                {
                    //List<int> roleTypes = gradeRoles.Items.Where(c => c.DepartmentId == departmentId.Value).Select(g => g.GradeRoleTypeId).Distinct().ToList();
                    List<int> roleTypes = roles.Items.Where(c => c.DepartmentId == departmentId.Value).Select(g => g.RoleTypeId).Distinct().ToList();

                    if (roleTypes != null && roleTypes.Count > 0)
                    {
                        associateKRA.KRADefinitions = (from d in m_KRAContext.Definitions
                                                       join aspList in m_KRAContext.Aspects on d.AspectId equals aspList.AspectId into aspectData
                                                       from asp in aspectData.DefaultIfEmpty()
                                                       join opList in m_KRAContext.Operators on d.OperatorId equals opList.OperatorId into opData
                                                       from op in opData.DefaultIfEmpty()
                                                       join mtList in m_KRAContext.MeasurementTypes on d.MeasurementTypeId equals mtList.MeasurementTypeId into mtype
                                                       from mt in mtype.DefaultIfEmpty()
                                                       join tpList in m_KRAContext.TargetPeriods on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                                       from tp in tperiod.DefaultIfEmpty()
                                                       join scList in m_KRAContext.Scales on d.ScaleId equals scList.ScaleId into scale
                                                       from sc in scale.DefaultIfEmpty()
                                                       where d.FinancialYearId == financialYearId
                                                       && roleTypes.Contains(d.RoleTypeId)
                                                       select new KRADefinition
                                                       {
                                                           DepartmentId = 0,
                                                           RoleTypeId = d.RoleTypeId,
                                                           KRAAspectName = asp.AspectName,
                                                           KRAAspectMetric = d.Metric,
                                                           KRATargetPeriod = tp.TargetPeriodValue,
                                                           KRATargetValue = d.TargetValue,
                                                           KRAMeasurementType = mt.MeasurementTypeName,
                                                           KRATargetText = "",
                                                           Operator = op.OperatorValue
                                                       }).ToList<KRADefinition>();
                    }
                }
            }
            else
            {
                associateKRA.KRADefinitions = (from d in m_KRAContext.Definitions
                                               join aspList in m_KRAContext.Aspects on d.AspectId equals aspList.AspectId into aspectData
                                               from asp in aspectData.DefaultIfEmpty()
                                               join opList in m_KRAContext.Operators on d.OperatorId equals opList.OperatorId into opData
                                               from op in opData.DefaultIfEmpty()
                                               join mtList in m_KRAContext.MeasurementTypes on d.MeasurementTypeId equals mtList.MeasurementTypeId into mtype
                                               from mt in mtype.DefaultIfEmpty()
                                               join tpList in m_KRAContext.TargetPeriods on d.TargetPeriodId equals tpList.TargetPeriodId into tperiod
                                               from tp in tperiod.DefaultIfEmpty()
                                               join scList in m_KRAContext.Scales on d.ScaleId equals scList.ScaleId into scale
                                               from sc in scale.DefaultIfEmpty()
                                               where d.FinancialYearId == financialYearId
                                               select new KRADefinition
                                               {
                                                   DepartmentId = 0,
                                                   RoleTypeId = d.RoleTypeId,
                                                   KRAAspectName = asp.AspectName,
                                                   KRAAspectMetric = d.Metric,
                                                   KRATargetPeriod = tp.TargetPeriodValue,
                                                   KRATargetValue = d.TargetValue,
                                                   KRAMeasurementType = mt.MeasurementTypeName,
                                                   KRATargetText = "",
                                                   Operator = op.OperatorValue
                                               }).ToList<KRADefinition>();
            }

            return associateKRA;
        }
        #endregion

        #region Generate PDF files
        /// <summary>
        /// Generate PDF files
        /// </summary>
        public void GeneratePdfFiles(AssociateKRA associateKRA, string webRootPath)
        {
            try
            {
                string filepath = m_MiscellaneousSettings.RepositoryPath;
                string previousFinancialYear = GetPreviousFinancialYear(associateKRA.FinancialYear);
                string section1 = $@"We thank you for your commitment and contributions in the year {previousFinancialYear} for ensuring business continuity and successfully delivering services to our clients.

In the current business scenario, it is critical to serve clients effectively and be efficient in all aspects of business operations for continuity and realization of future growth. Accordingly, we have defined performance key result areas (KRAs) of associates in alignment with the goals and targets for client delivery. We promote high-performance, innovation and improvement, entrepreneurship, learning and development, and care for the overall wellbeing and growth of our associates.";

                string section2 = $@"You would have received valuable feedback during your development review about your achievement against the KRAs set for the year {previousFinancialYear}. We request you to implement suitable developmental action plan during the year {associateKRA.FinancialYear} to address the identified development needs.";

                string section3 = $@"Please discuss your KRAs with Reporting Manager to prepare and implement a development plan and work systematically to achieve them by contributing effectively to client delivery.  Also focus on holistic development of your competence to be productive and undertake role responsibilities competitively.";

                bool isExists = Directory.Exists(filepath);

                if (isExists == false)
                    return;

                if (associateKRA.KRADefinitions != null && associateKRA.KRADefinitions.Count > 0)
                {
                    foreach (AssociateRoleType associate in associateKRA.AssociateRoleTypes)
                    {
                        var kras = associateKRA.KRADefinitions
                                      .Where(k => k.RoleTypeId == associate.EmployeeRoleId)
                                      .Select(o => new { o.KRAAspectName })
                                      .Distinct().ToList();


                        if (kras == null || kras.Count == 0)
                            continue;

                        // Employee Folder
                        bool isEmployeeFolderExists = Directory.Exists(filepath + "/" + associate.EmployeeCode);

                        if (isEmployeeFolderExists == false)
                            Directory.CreateDirectory(filepath + "/" + associate.EmployeeCode);

                        // KRA Folder
                        bool isKRAFolderExists = Directory.Exists(filepath + "/" + associate.EmployeeCode + "/KRA");

                        if (isKRAFolderExists == false)
                            Directory.CreateDirectory(filepath + "/" + associate.EmployeeCode + "/KRA");

                        string fileName = associate.EmployeeCode + "_" + associateKRA.FinancialYear + "_" + associate.EmployeeRole + "_KRA.pdf";
                        var filePath = filepath + "/" + associate.EmployeeCode + "/KRA/" + fileName;

                        KRAPdf krapdf = new KRAPdf()
                        {
                            FinancialYear = associateKRA.FinancialYear,
                            EmployeeCode = associate.EmployeeCode,
                            EmployeeEmail = associate.EmployeeEmail,
                            FileName = fileName,
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            CreatedBy = "",
                            ModifiedBy = ""
                        };
                        m_KRAContext.Add(krapdf);
                        bool isCreated = m_KRAContext.SaveChanges() > 0 ? true : false;

                        using (FileStream memoryStream = new FileStream(filePath, FileMode.Create))
                        {
                            Paragraph para = null;
                            Phrase pharse = null;
                            PdfPTable table = null;
                            PdfPCell cell = null;
                            Document document = new Document(PageSize.A4, 60, 60, 40, 80); // PageSize.A4, left, right, top , bottom        
                            Font contentFont = new Font(FontFactory.GetFont("Arial", 11, new BaseColor(32, 32, 32)));
                            Font tableFont = new Font(FontFactory.GetFont("Arial", 11, new BaseColor(32, 32, 32)));
                            Font contentFontBold = new Font(FontFactory.GetFont("Arial", 11, Font.BOLD));
                            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                            // calling PDFFooter class to Include in document
                            writer.PageEvent = new Footer(webRootPath);
                            document.Open();

                            // Date
                            para = new Paragraph(DateTime.Now.ToString("dd-MMMM-yyyy"), contentFont);
                            para.Font = new Font(Font.FontFamily.HELVETICA);
                            para.SpacingAfter = 15;
                            document.Add(para);

                            // AssociateName
                            para = new Paragraph(0, "Dear " + associate.EmployeeName + ",", contentFont);
                            para.SpacingAfter = 15;
                            document.Add(para);

                            para = new Paragraph(section1, contentFont);
                            para.Alignment = Element.ALIGN_JUSTIFIED;
                            para.SpacingAfter = 15;
                            document.Add(para);

                            para = new Paragraph(section2,contentFont);
                            para.Alignment = Element.ALIGN_JUSTIFIED;
                            para.Font = contentFont;
                            para.SpacingAfter = 15;
                            document.Add(para);

                            para = new Paragraph();
                            para.Alignment = Element.ALIGN_JUSTIFIED;
                            para.Font = contentFont;
                            para.SpacingAfter = 15;
                            document.Add(para);

                            // Financial Year
                            para = new Paragraph(0, @"The following KRAs are assigned to you for the year " + associateKRA.FinancialYear + " to demonstrate your work", contentFont);
                            para.SpacingAfter = 15;
                            document.Add(para);
                            para = new Paragraph(0, @"performance and competency development.", contentFont);
                            para.SpacingAfter = 15;
                            document.Add(para);

                            para = new Paragraph();
                            para.Font = contentFont;
                            para.SpacingAfter = 15;
                            document.Add(para);

                            // Associate Role
                            //para = new Paragraph(0, "Role Type: " + associate.EmployeeRole, contentFontBold);

                            document.NewPage();
                            table = new PdfPTable(2);
                            table.TotalWidth = 500f;
                            float[] widths = new float[] { 250f, 250f };
                            table.SetWidths(widths);
                            pharse = new Phrase();
                            cell = PhraseCell(new Paragraph("Grade: " + associate.GradeName,contentFont));
                            cell.Border = 0;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.PaddingLeft = 15f;
                            table.AddCell(cell);
                            cell = PhraseCell(new Paragraph("Role Type: "+ associate.EmployeeRole,contentFont));
                            cell.Border = 0;
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell);
                            table.LockedWidth = true;
                            //para = new Paragraph(0, "Role Type: " + associate.GradeName);
                            //para.SpacingAfter = 15;
                            //document.Add(para);
                            document.Add(table);



                            table = new PdfPTable(3);
                            
                            table.HorizontalAlignment = 0;
                            table.TotalWidth = 500f;
                            table.LockedWidth = true;
                            widths = new float[] { 120f, 300f, 80f };
                            table.SetWidths(widths);

                            // KRA Table Header
                            pharse = new Phrase();
                            cell = PhraseCell(new Paragraph("KRA Aspect", FontFactory.GetFont("Arial",11, Font.BOLD, new BaseColor(255,255,255))));
                            cell.BackgroundColor = new BaseColor(167, 168, 108);
                            table.AddCell(cell);

                            cell = PhraseCell(new Paragraph("Metric", FontFactory.GetFont("Arial", 11, Font.BOLD, new BaseColor(255, 255, 255))));
                            cell.BackgroundColor = new BaseColor(167, 168, 108);
                            table.AddCell(cell);

                            cell = PhraseCell(new Paragraph("Target", FontFactory.GetFont("Arial", 11, Font.BOLD, new BaseColor(255, 255, 255))));
                            cell.BackgroundColor = new BaseColor(167, 168, 108);
                            table.AddCell(cell);
                            

                            string KRAtareget = string.Empty;


                            // KRA Table Data
                            foreach (var aspect in kras)
                            {
                                PdfPCell aspectCell = null;
                                aspectCell = PhraseCell(new Paragraph(aspect.KRAAspectName, tableFont));
                                aspectCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                aspectCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                int count = associateKRA.KRADefinitions.FindAll(k => k.KRAAspectName == aspect.KRAAspectName &&
                                k.RoleTypeId == associate.EmployeeRoleId).Count;
                                aspectCell.Rowspan = count;
                                table.AddCell(aspectCell);

                                // inner loop to print 
                                var metricsAndTargets = associateKRA.KRADefinitions.FindAll(k => k.KRAAspectName == aspect.KRAAspectName && k.RoleTypeId == associate.EmployeeRoleId);
                                foreach (var kra in metricsAndTargets)
                                {
                                    cell = PhraseCell(new Paragraph(kra.KRAAspectMetric, tableFont));
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    table.AddCell(cell);

                                    if (kra.KRAMeasurementType == "Scale" || kra.KRAMeasurementType == "Number")
                                    {
                                        string target = kra.KRATargetValue.ToString();
                                        int endIndex = 0, value = (target.IndexOf('.')) + 1;
                                        endIndex = ((target[value] - '0') > 0) ? 2 : 0;

                                        if (kra.KRATargetText != null)
                                        {
                                            KRAtareget = kra.Operator + kra.KRATargetValue.ToString() + " " + kra.KRATargetText + " (" + kra.KRATargetPeriod + ")";
                                        }
                                        else
                                            KRAtareget = kra.Operator + kra.KRATargetValue.ToString() + " (" + kra.KRATargetPeriod + ")";

                                    }
                                    else if (kra.KRAMeasurementType == "Percentage")
                                    {
                                        if (kra.KRATargetText != null)
                                            KRAtareget = kra.Operator + kra.KRATargetValue.ToString() + "% " + kra.KRATargetText + " (" + kra.KRATargetPeriod + ")";
                                        else
                                            KRAtareget = kra.Operator + kra.KRATargetValue.ToString() + "%  (" + kra.KRATargetPeriod + ")";
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(kra.KRATargetText))
                                            KRAtareget = kra.KRATargetValue.ToString();
                                        KRAtareget = kra.KRATargetValue.ToString();
                                    }
                                    cell = PhraseCell(new Paragraph(KRAtareget, tableFont));
                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                    table.AddCell(cell);

                                }
                            }

                            document.Add(table);
                            document.NewPage();
                            para = new Paragraph();
                            para.Font = contentFont;
                            para.SpacingBefore = 15;
                            document.Add(para);
                            para = new Paragraph(section3, contentFont);
                            para.SpacingBefore = 15;
                            para.Alignment = Element.ALIGN_JUSTIFIED;
                            document.Add(para);

                            para = new Paragraph("With best wishes,", contentFont);
                            para.SpacingBefore = 10;
                            para.SpacingAfter = 6;
                            document.Add(para);

                            // Department Head Signature
                            string imagePath = Path.Combine(webRootPath, @"Images/" + associate.DepartmentName + "_sign.png");
                            Image image = Image.GetInstance(imagePath);
                            if (image == null)
                            {
                                imagePath = Path.Combine(webRootPath, @"Images/departmenthead_sign.png");
                                image = Image.GetInstance(imagePath);
                            }
                            image.Alignment = Element.ALIGN_LEFT;
                            image.ScaleToFit(100f, 100f);
                            document.Add(image);

                            para = new Paragraph("Yours sincerely,", contentFont);
                            para.SpacingAfter = 10;
                            document.Add(para);

                            // Department Head Name
                            para = new Paragraph(associate.DepartmentHeadName, contentFont); //+ " <br/>" + "Head, Technology & Delivery" + "</p>");
                            para.Alignment = Element.ALIGN_LEFT;
                            document.Add(para);

                            // Department Name
                            para = new Paragraph("Head, " + associate.DepartmentName, contentFont); //+ " <br/>" + "Head, Technology & Delivery" + "</p>");
                            para.Alignment = Element.ALIGN_LEFT;
                            document.Add(para);

                            if (document.IsOpen())
                                document.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PhraseCell
        /// <summary>
        /// PhraseCell
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        private static PdfPCell PhraseCell(Phrase phrase, int align = Element.ALIGN_CENTER)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingLeft = 7f;
            cell.PaddingRight = 7f;
            cell.PaddingBottom = 7f;
            cell.PaddingTop = 7f;
            return cell;
        }
        #endregion

        private string GetPreviousFinancialYear(string financialYear)
        {
            string result = financialYear;

            if (financialYear.Length > 9)
            {
                result = result.Replace(financialYear.Substring(0, 4), Convert.ToString(Convert.ToInt32(financialYear.Substring(0, 4)) - 1));
                result = result.Replace(financialYear.Substring(financialYear.Length - 4, 4), Convert.ToString(Convert.ToInt32(financialYear.Substring(financialYear.Length - 4, 4)) - 1));
            }

            return result;
        }

        #endregion

        #region Get KRAPdfs
        /// <summary>
        /// This method fetches all KRAPdf records.
        /// </summary>
        /// <returns>List<KRAPdf></returns>
        public async Task<List<KRAPdf>> GetKRAPdfsAsync(bool IsActive = true)
        {
            m_Logger.LogInformation("KRAPdfService: Calling \"GetAll\" method.");

            return await m_KRAContext.KRAPdfs.Where(c => c.IsActive == IsActive)
                          .ToListAsync();
        }
        #endregion

        #region Delete KRAPdf
        /// <summary>
        /// This method deletes KRAPdf record.
        /// </summary>
        /// <param name="kraPdfId"></param>
        /// <returns>dynamic</returns>
        public async Task<bool> DeleteKRAPdfAsync(Guid kraPdfId)
        {
            m_Logger.LogInformation("KRAPdfService: Calling \"Delete\" method");
            var kraPdf = m_KRAContext.KRAPdfs.Find(kraPdfId);

            if (kraPdf is null)
            {
                return (false);
            }

            m_Logger.LogInformation("KRAPdfService: Calling SaveChanges method on DB Context.");
            m_KRAContext.KRAPdfs.Remove(kraPdf);
            var isUpdated = await m_KRAContext.SaveChangesAsync() > 0;

            return (isUpdated);
        }
        #endregion
    }
}
