using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Constants
{
    public static class ServicePaths
    {
        #region OrgEndPoint
        public static class OrgEndPoint
        {
            public const string GETSTATUSBYCATEGORYANDSTATUSCODE = "Status/GetByCategoryAndStatusCode/";
            public const string GETSTATUSBYIDS = "Client/GetByIds?clientIds=";
            public const string GETALLPRACTICEAREA = "PracticeArea/GetAll?isActive=";
            public const string GETPRACTICEAREABYID = "PracticeArea/GetById?practiceAreaId=";
            public const string GETPRACTICEAREABYIDS = "PracticeArea/GetByIds?practiceAreaIds=";
            public const string GETPROJECTTYPEBYID = "ProjectType/GetById?ProjectTypeId=";
            public const string GETALLPROJECTTYPES = "ProjectType/GetAll?isActive=";
            public const string GETPROJECTTYPEBYIDS = "ProjectType/GetByIds?projectTypeIds=";
            public const string GetALLUSERS = "User/GetAllUsers";
			public const string GETUSERBYID = "User/GetByUserId?userId=";
            public const string GETCLIENTS = "Client/GetAll";
            public const string GETCLIENTBYIDS = "Client/GetByIds?clientIds=";
            public const string GETCLIENTBYID = "Client/GetById/";
            public const string GETDOMAINBYID = "Domain/GetByDomainId/";
            public const string GetAllDepartment = "Department/GetAll?isActive=";
            public const string GETDEPARTMENTBYID = "Department/GetById/";
            public const string GETDEPARTMENTBYCODE = "Department/GetByDepartmentCode/";
            public const string GETSTATUSESBYCATEGORYNAME = "Status/GetAllByCategoryName?category=";
            public const string GETSTATUSESBYCATEGORY = "NightlyJob/GetAllByCategoryName?category=";
            public const string GETCATEGORYBYNAME = "CategoryMaster/GetByCategoryName?categoryName=";
            public const string GETUSERBYEMAIL = "User/GetByEmail?email=";
            public const string GETSTATUSBYCATEGORYIDANDSTATUSCODE = "Status/GetByCategoryIdAndStatusCode/";
            public const string GETNOTIFICATIONTYPEBYCODE = "NotificationType/GetByNotificationCode/";
            public const string GETBYNOTIFICATIONTYPEANCATEGORYID = "NotificationConfiguration/GetByNotificationTypeAndCategory?notificationTypeId=";
            public const string GETPROGRAMMANAGERS = "UserRole/GetProgramManagers/";
            public const string GETNOTIFICATIONCONFIGURATION = "NotificationConfiguration/GetByNotificationTypeAndCategory?notificationTypeId=";
            public const string SENDEMAIL = "NotificationManager/SendEmail/";
            public const string UPDATEREPORTINGMANAGERID = "EmployeePersonalDetails/UpdateReportingManagerId/";
            public const string UPDATEEMPLOYEE = "EmployeePersonalDetails/UpdateExternal";
            public const string GETROLESBYDEPARTMENTID = "FunctionalRole/GetByDepartmentID?departmentId=";
            public const string GETROLENAMES = "FunctionalRole/GetRoleNames";
            public const string GETALLROLEMASTERS = "FunctionalRole/GetAll";
            public const string GETALLROLES = "UserRole/GetAllRoles?isActive=";
            public const string GETALLUSERROLES = "UserRole/GetAllUserRoles?isActive=";
            public const string GETALLUSERS = "User/GetUsers";
            public const string GETCLOSUREACTIVITIESBYDEPARTMENT = "Activity/GetClosureActivitiesByDepartment";
            public const string GETASSOCIATEALLOCATIONMASTERS = "Report/GetAssociateAllocationMasters";
            public const string GETALLDEPARTMENTSWITHDLS = "Department/GetAllDepartmentsWithDLs";
        }
        #endregion

        #region EmployeeEndPoint
        public static class EmployeeEndPoint
        {
            public const string GETALL = "Employee/GetAll?isActive=";
            public const string GETEMPLOYEEBYID = "Employee/GetById/";
            public const string GETEMPLOYEEBYIDS = "Employee/GetByIds?employeeIds=";
			public const string GETEMPLOYEEBYUSERID = "Employee/GetByUserId/";
            public const string GETEMPLOYEEBYUSERNAME = "Employee/GetEmployeeByUserName/";
            public const string GETACTIVEEMPLOYEEBYID = "Employee/GetActiveEmployeeById/";
            public const string GETASSOCIATESFORDROPDOWN = "Employee/GetAssociatesForDropdown/";
            public const string GetResignedAssociateByID = "AssociateExit/GetResignedAssociateByID/";
            public const string GETALLEXITASSOCIATES = "AssociateExit/GetAll";
        }
        #endregion

    }
}
