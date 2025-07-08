using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
   public class WorkFromHomeAttendanceService : IWorkFromHomeAttendanceService
    {
        #region Global Varibles
        private readonly EmployeeDBContext _employeeDBContext;   
        private readonly IOrganizationService m_OrgService;      
        private readonly ILogger<AttendanceReportService> _logger;
        private readonly EmailConfigurations m_EmailConfigurations;
        DateTime CurrentDate = Common.Utility.GetDateTimeInIST();        
        #endregion

        #region Constructor
        public WorkFromHomeAttendanceService(EmployeeDBContext employeeDBContext,
                                  ILogger<AttendanceReportService> logger, IOptions<EmailConfigurations> emailConfigurations, IOrganizationService organizationService)
        {
            _employeeDBContext = employeeDBContext;
            _logger = logger;
            m_EmailConfigurations = emailConfigurations.Value;
            m_OrgService = organizationService;
        }
        #endregion
        
        #region SaveAttendanceDetais
        public async Task<ServiceResponse<bool>> SaveAttendanceDetais(BioMetricAttendance bioMetricAttendance)
        {
            var response = new ServiceResponse<bool>();
            int isSaved = 0;
            try
            {
                var isAttendanceExist = _employeeDBContext.BioMetricAttendenceDetail.Where(employee => employee.AsscociateId == bioMetricAttendance.AsscociateId && employee.Punch1_Date==CurrentDate.Date).FirstOrDefault();
                if (isAttendanceExist == null)
                {
                  
                    
                    bioMetricAttendance.Punch1_Date = CurrentDate;
                    bioMetricAttendance.InTime = Common.Utility.GetDateTimeInIST().ToString("HH:mm");
                    bioMetricAttendance.SUMMARY = GetSummary(bioMetricAttendance.InTime, null);
                    bioMetricAttendance.PunchInfoLog = bioMetricAttendance.SignedStatus == Convert.ToInt32(AttendanceSigningCode.SignedIn) ?  "Signed In : "+ bioMetricAttendance.InTime +"$Location : "+ bioMetricAttendance.Location : bioMetricAttendance.PunchInfoLog + "," + "Signed Out : ";
                    _employeeDBContext.BioMetricAttendenceDetail.Add(bioMetricAttendance);
                     isSaved = await _employeeDBContext.SaveChangesAsync();
                }
                else
                {
                    isAttendanceExist.Location = bioMetricAttendance.Location;
                    isAttendanceExist.Punch2_Date = CurrentDate;
                    isAttendanceExist.OutTime = Common.Utility.GetDateTimeInIST().ToString("HH:mm");
                    isAttendanceExist.SignedStatus = bioMetricAttendance.SignedStatus;
                    isAttendanceExist.Remarks = bioMetricAttendance.Remarks;
                    isAttendanceExist.SUMMARY = GetSummary(TimeSpan.Parse(isAttendanceExist.InTime).ToString(@"hh\:mm"), TimeSpan.Parse(isAttendanceExist.OutTime).ToString(@"hh\:mm"));
                    if (isAttendanceExist.PunchInfoLog == null)
                    {
                        isAttendanceExist.PunchInfoLog = bioMetricAttendance.SignedStatus == Convert.ToInt32(AttendanceSigningCode.SignedIn) ?  "Signed In : " + isAttendanceExist.OutTime + "$Location : "+isAttendanceExist.Location :  "Signed Out : " + isAttendanceExist.OutTime + "$Location : "+isAttendanceExist.Location;
                    }
                    else
                    {

                        isAttendanceExist.PunchInfoLog = bioMetricAttendance.SignedStatus == Convert.ToInt32(AttendanceSigningCode.SignedIn) ? isAttendanceExist.PunchInfoLog + "," + "Signed In : " + isAttendanceExist.OutTime + "$Location : "+isAttendanceExist.Location : isAttendanceExist.PunchInfoLog + "," + "Signed Out : " + isAttendanceExist.OutTime + "$Location : "+isAttendanceExist.Location;
                    }
                    isAttendanceExist.WorkTime_HHMM = GetTotalWorkHourse(isAttendanceExist.InTime,isAttendanceExist.OutTime);
                    _employeeDBContext.BioMetricAttendenceDetail.Update(isAttendanceExist);
                    await _employeeDBContext.SaveChangesAsync();
                }
           
                if(isSaved>0 && bioMetricAttendance.Location=="WFO")
                {
                    var associateDetails = _employeeDBContext.Employees.Where(emp => emp.EmployeeCode.ToLower() == bioMetricAttendance.AsscociateId.ToLower() && emp.IsActive == true).FirstOrDefault();
                    var associateRM_Details = _employeeDBContext.Employees.Where(emp => emp.EmployeeId == associateDetails.ReportingManager && emp.IsActive == true).FirstOrDefault();
                    SendNotificationToRMWhenWFOSignIn(associateDetails, associateRM_Details, bioMetricAttendance);
                }
                response.IsSuccessful = true;
                response.Item = true;
            }
            catch(Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;
        }
        #endregion

        #region GetAttendanceDetais
        public async Task<ServiceResponse<BioMetricAttendance>> GetAttendanceDetais(string employeeCode)
        {
            var response = new ServiceResponse<BioMetricAttendance>();

            try
            {
                var workFromHomeAttendace = await _employeeDBContext.BioMetricAttendenceDetail.Where(employee => employee.AsscociateId == employeeCode && employee.Punch1_Date == CurrentDate.Date && (employee.Location.ToLower() == "wfh" || employee.Location.ToLower()== "on duty" || employee.Location.ToLower()=="cl" || employee.Location.ToLower()=="wfo")).FirstOrDefaultAsync();
                if (workFromHomeAttendace==null)
                {
                    response.IsSuccessful = false;
                    response.Message = "No Data Found";
                    return response;
                        
                }
                    response.IsSuccessful = true;
                response.Item = workFromHomeAttendace;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;

        }
        #endregion

        #region GetloginStatus
        public async Task<ServiceResponse<int?>> GetloginStatus(string employeeCode)
        {
            var response = new ServiceResponse<int?>();
            int? loginStatus =null;
            try
            {
                var workFromHomeAttendace = await _employeeDBContext.BioMetricAttendenceDetail.Where(employee => employee.AsscociateId == employeeCode && employee.Punch1_Date == CurrentDate.Date).FirstOrDefaultAsync();
               if(workFromHomeAttendace==null)
                {
                    loginStatus =Convert.ToInt32(AttendanceSigningCode.Initial);
                }
                else if (workFromHomeAttendace.SignedStatus==Convert.ToInt32(AttendanceSigningCode.SignedIn))
                {
                    loginStatus=Convert.ToInt32(AttendanceSigningCode.SignedIn);
                }
                else if (workFromHomeAttendace.SignedStatus == Convert.ToInt32(AttendanceSigningCode.SignedOut))
                {
                    loginStatus =Convert.ToInt32(AttendanceSigningCode.SignedOut);
                }
                response.IsSuccessful = true;
                response.Item = loginStatus;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Message = e.Message;
            }
            return response;

        }
        #endregion

        #region Private methods
        private string GetTotalWorkHourse(string InTime, string OutTime)
        {
            string TotalTime = string.Empty;
            if (string.IsNullOrWhiteSpace(OutTime) == false && string.IsNullOrWhiteSpace(InTime) == false)
            {
                var inTime = InTime.Split(':');
                var outTime = OutTime.Split(':');


                TotalTime = inTime.Count() > 1 && outTime.Count() > 1 ?
                                (new TimeSpan(Convert.ToInt32(outTime[0]), Convert.ToInt32(outTime[1]), 0) -
                                 new TimeSpan(Convert.ToInt32(inTime[0]), Convert.ToInt32(inTime[1]), 0)).ToString(@"hh\:mm") : "";
            }
            return TotalTime;
        }

        private string GetSummary(string Intime, string OutTime)
        {
            string summary = string.Empty;
            //var ExactInTime = "09:10";
            //var ExactOutTime = "18:00";

            //if (string.IsNullOrWhiteSpace(OutTime) == false && string.IsNullOrWhiteSpace(Intime) == false)
            //{
            //    var inTime = DateTime.ParseExact(Intime, "HH:mm", CultureInfo.InvariantCulture);
            //    var outTime = DateTime.ParseExact(OutTime, "HH:mm", CultureInfo.InvariantCulture);
            //    var exactInTime = DateTime.ParseExact(ExactInTime, "HH:mm", CultureInfo.InvariantCulture);
            //    var exactOutTime = DateTime.ParseExact(ExactOutTime, "HH:mm", CultureInfo.InvariantCulture);
            //    summary = OutTime.Count() > 1 && Intime.Count() > 1 && exactInTime >= inTime && exactOutTime <= outTime ? "Present" : "Early-Out";
            //    summary = OutTime.Count() > 1 && Intime.Count() > 1 && exactInTime < inTime && exactOutTime <= outTime ? "Late-In" : "Early-Out";
            //}          
            //else 
            //{
            //    var inTime = DateTime.ParseExact(Intime, "HH:mm", CultureInfo.InvariantCulture);
            //    var exactTime = DateTime.ParseExact(ExactInTime, "HH:mm", CultureInfo.InvariantCulture);
            //    summary = Intime.Count() > 1 && inTime !=null ?  "Punches Not In Pair":"Late-In";
            //}

            summary = Intime != null && OutTime == null ? "Punches Not In Pair" : "Signed-Out";
            return summary;
        }

        private void SendNotificationToRMWhenWFOSignIn(Entities.Employee employeeDetails,Entities.Employee RMdetails,BioMetricAttendance bioMetricAttendance)
        {
            string associateName = employeeDetails.FirstName + " " + employeeDetails.LastName;
            var filePath = Utility.GetNotificationTemplatePath(NotificationTemplatePaths.currentDirectory,NotificationTemplatePaths.subDirectories_Associate_WFO_Signin);
            StreamReader stream = new StreamReader(filePath);
            string MailText = stream.ReadToEnd();
            stream.Close();
            MailText = MailText.Replace("{ReportingManager}", RMdetails.FirstName +" "+RMdetails.LastName);
            MailText = MailText.Replace("{EmployeeName}", associateName);
            MailText = MailText.Replace("{Date}", bioMetricAttendance.Punch1_Date.ToString());
            MailText = MailText.Replace("{Time}", bioMetricAttendance.InTime);
            MailText = MailText.Replace("{Location}", "Work from office");
            NotificationDetail notificationDetail = new NotificationDetail();
           
                notificationDetail.Subject = associateName+" has successfully marked his WFO attendance for the day.";
            notificationDetail.EmailBody = MailText;
            notificationDetail.FromEmail = m_EmailConfigurations.FromEmail;
            notificationDetail.ToEmail = $"{RMdetails.WorkEmailAddress}";
            m_OrgService.SendEmail(notificationDetail);
        }
        #endregion
    }
}
