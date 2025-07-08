using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Constants
{
    public static class ServicePaths
    {
        #region OrgEndPoint
        public static class OrgEndPoint
        {
            public const string GETALLDEPARTMENTS = "Department/GetAll";
            public const string GetUserRoles = "UserRole/GetUserRoles/";
            public const string GETUSERDEPARTMENT = "Department/GetUserDepartmentDetails";
            public const string GETALLGRADES = "Grade/GetAll";
            public const string GetAllFinancialYears = "FinancialYear/GetAll";
            public const string GetAllRoleTypes = "RoleType/GetAll";
            public const string SENDEMAIL = "NotificationManager/SendEmail";
            public const string GETNOTIFICATIONBYNOTIFICATIONTYPEANDCATEGORY = "NotificationConfiguration/GetByNotificationTypeAndCategory?notificationTypeId=";
            public const string GetNotificationConfiguration = "NotificationConfiguration/GetNotificationConfiguration?notificationCode=";
            public const string GetById = "Department/GetById/";
            public const string GetRoleTypesAndDepartments = "RoleType/GetRoleTypesAndDepartments";
            public const string GETFINANCIALYEARBYID = "FinancialYear/GetById/";
        }
        #endregion

        #region EmployeeEndPoint
        public static class EmployeeEndPoint
        {
            public const string GETEMPLOYEENAMES = "Employee/GetEmployeeNames";
            public const string GETEMPLOYEEWORKEMAILADDRESS = "Employee/GetEmployeeWorkEmailAddress/";
            public const string GETEMPLOYEESBYROLE = "Employee/GetEmployeesByRole";
        }
    }
        #endregion
    }
