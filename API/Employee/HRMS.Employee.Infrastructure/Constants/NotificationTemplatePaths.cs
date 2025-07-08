using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HRMS.Employee.Infrastructure.Constants
{
    public class NotificationTemplatePaths
    {
        #region Notification Template Paths

        public static string currentDirectory = Directory.GetCurrentDirectory();        
        public static List<string> subDirectories_Associate_Abscond = new List<string> { "NotificationTemplate", "AssociateAbscond.html" };
        public static List<string> subDirectories_AssociateExit_GenericMail = new List<string> { "NotificationTemplate", "AssociateExit_GenericMail.html" };
        public static List<string> subDirectories_AssociateExit_ResignationStatus = new List<string> { "NotificationTemplate", "AssociateExit_ResignationStatus.html" };
        public static List<string> subDirectories_AssociateExit_ResignationStatusWithReason = new List<string> { "NotificationTemplate", "AssociateExit_ResignationStatusWithReason.html" };
        public static List<string> subDirectories_ITReturnsNotification = new List<string> { "NotificationTemplate", "ITReturnsNotification.html" };
        public static List<string> subDirectories_Regularization_Notification = new List<string> { "NotificationTemplate", "RegularizationNotification.html" };
        public static List<string> subDirectories_Employee_Skill_Submit = new List<string> { "NotificationTemplate", "EmployeeSkillSubmit_Template.html" };
        public static List<string> subDirectories_Associate_Release_Request = new List<string> { "NotificationTemplate", "AssociateReleaseRequestNotification.html" };
        public static List<string> subDirectories_Associate_WFO_Signin = new List<string> { "NotificationTemplate", "AssociateWFOSignInNotification.html" };
        #endregion
    }
}
