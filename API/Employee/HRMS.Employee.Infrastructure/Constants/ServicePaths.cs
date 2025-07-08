using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace HRMS.Employee.Infrastructure.Constants
{
    public static class ServicePaths
    {
        #region OrgEndPoint
        public static class OrgEndPoint
        {
            public const string GETALLDEPARTMENTS = "Department/GetAll";
            public const string GETDEPARTMENTBYCODE = "Department/GetByDepartmentCode/";
            public const string GETDEPARTMENTBYCODES = "Department/GetByDepartmentCodes?departmentCodes=";
            public const string GETDEPARTMENTBYID = "Department/GetById/";
            public const string GETALLDESIGNATIONS = "Designation/GetAll";
            public const string GETDESIGNATIONBYID = "Designation/GetById/";
            public const string GETDESIGNATIONBYCODE = "Designation/GetByCode/";
            public const string SENDEMAIL = "NotificationManager/SendEmail";
            public const string GETPRACTICEAREABYCODE = "PracticeArea/GetByPracticeAreaCode/";
            public const string GETALLGRADES = "Grade/GetAll";
            public const string GETALLDOMAINS = "Domain/GetAll";
            public const string GETALLPRACTICEAREAS = "PracticeArea/GetAll";
            public const string GETALLSTATUSES = "Status/GetAll";
            public const string GETSTATUSBYCATEGORYANDSTATUSCODE = "Status/GetByCategoryAndStatusCode/";
            public const string GETSTATUSBYID = "Status/GetStatusById/";
            public const string GETSTATUSBYCODE = "Status/GetStatusByCode/";
            public const string GETALLUSERS = "User/GetAllUsers";
            public const string GETACTIVEUSERSBYID = "User/GetActiveUserByUserId/";
            public const string GETALLUSERROLES = "UserRole/GetAllUserRoles";
            public const string GETALLROLES = "UserRole/GetAllRoles";
            public const string UPDATEUSER = "User/UpdateUser/";
            public const string GETROLEBYROLENAME = "UserRole/GetRoleByRoleName/";
            public const string GETSKILLSBYSKILLGROUPID = "Skill/GetBySkillGroupId?skillGroupIds=";
            public const string GETSKILLSBYSKILLID = "Skill/GetById?skillIds=";
            public const string GETALLSKILLS = "Skill/GetAll?isActive=";
            public const string GETPROFICIENCEYLEVELS = "ProficiencyLevel/GetAll?isActive=";
            public const string GETCOMPETENCYAREAS = "CompetencyArea/GetAll?isActive=";
            public const string GETPROFICIENCEYLEVELSBYPROFICIENCEYLEVELID = "ProficiencyLevel/GetById?proficiencyLevelIds=";
            public const string GETNOTIFICATIONBYNOTIFICATIONTYPEANDCATEGORY = "NotificationConfiguration/GetByNotificationTypeAndCategory?notificationTypeId=";
            public const string GETEXITACTIVITIESBYDEPARTMENT = "Activity/GetExitActivitiesByDepartment";
            public const string GETSTATUSESBYCATEGORYNAME = "Status/GetAllByCategoryName?category=";
            public const string GETEXITTYPEBYID = "ExitType";
            public const string GETALLEXITTYPES = "ExitType/GetExitTypesForDropdown";
            public const string GETALLEXITREASONS = "Reason/GetReasonsForDropdown";
            public const string REMOVEUSERROLEONEXIT = "UserRole/RemoveUserRoleOnExit/";
            public const string GETDEPARTMENTDLBYDEPTID = "Department/GetDepartmentDLByDeptId/";
            public const string GETUSERBYROLES = "User/GetUsersByRoles/";
            public const string GETALLROLETYPES = "RoleType/GetAll";
            public const string GETROLETYPEBYID = "RoleType/GetById?gradeRoleTypeId=";
            public const string GETROLETYPESFORDROPDOWN = "RoleType/GetRoleTypesForDropdown";
            public const string GETALLDEPARTMENTSFORJOB = "NightlyJob/GetAllDepartments";
            public const string GETALLPRACTICEAREASFORJOB = "NightlyJob/GetAllPracticeAreas";
            public const string GETALLDEPARTMENTSWITHDLS = "Department/GetAllDepartmentsWithDLs";
            public const string GetAllFinancialYears = "FinancialYear/GetAll";
            public const string GETALLHOLIDAYS = "Holiday/GetAll";
            public const string GETCLIENTBYID = "Client/GetById/";
            public const string GETAllCLIENTS = "Client/GetAll";
            public const string GETAllROLEMASTER = "FunctionalRole/GetAll";
        }
        #endregion

        #region ProjectEndPoint
        public static class ProjectEndPoint
        {
            public const string GETASSOCIATEALLOCATIONS = "AssociateAllocation/GetAll";
            public const string GETALLASSOCIATEALLOCATIONS = "NightlyJob/GetAllAssociateAllocations";
            public const string GETASSOCIATEALLOCATIONSBYID = "AssociateAllocation/GetAllocationsById/";
            public const string GETASSOCIATEALLOCATIONSBYEMPLOYEEID = "AssociateAllocation/GetByEmployeeId/";
            public const string GETASSOCIATEALLOCATIONSBYLEADID = "AssociateAllocation/GetByLeadId/";
            public const string GETPROJECTBYID = "Project/GetById/";
            public const string GETALLPROJECTS = "Project/GetAll";
            public const string GETPROJECTS = "NightlyJob/GetAllProjects";
            public const string GETPROJECTMANAGERS = "ProjectManager/GetAll";
            public const string GETPROJECTMANAGERSBYID = "ProjectManager/GetProjectManagerById/";
            public const string GETACTIVEPROJECTMANAGERS = "ProjectManager/GetActiveProjectManagers";
            public const string GETALLOCATIONPERCENTAGE = "AllocationPercentage/GetAll/";
            public const string GETPROJECTMANAGERSBYEMPLOYEEID = "ProjectManager/GetByEmployeeId/";
            public const string GETASSOCIATEALLOCATIONSBYIDS = "AssociateAllocation/GetAllocationsByEmpIds?employeeIds=";
            public const string GETPROJECTMANAGERSBYEMPLOYEEIDS = "ProjectManager/GetProjectManagerByEmployeeId?employeeIds=";
            public const string ALLOCATEASSOCIATETOTALENTPOOL = "AssociateAllocation/AllocateAssociateToTalentPool";
            public const string GETEMPLOYEEBYPROJECTID = "Report/GetEmployeeByProjectId?projectId=";
            public const string GETRESOURCEBYPROJECT = "Report/GetResourceByProject?projectId=";
            public const string GETSKILLSEARCHALLOCATIONS = "Report/GetSkillSearchAllocations";
            public const string GETSKILLSEARCHASSOCIATEALLOCATIONS = "AssociateAllocation/GetSkillSearchAssociateAllocations?employeeIds=";
            public const string GETPROJECTSBYIDS = "Project/GetProjectsByIds/";
            public const string UPDATETALENTPOOL = "AssociateAllocation/ReleaseFromTalentPool";
            public const string GETEMPLOYEESBYEMPLOYEEIDANDROLE = "AssociateAllocation/GetAssociatesToRelease/";
            public const string UPDATETP_PROJECTINALLOCATIONS = "AssociateAllocation/UpdatePracticeAreaOfTalentPoolProject/";
            public const string GETPROJECTLEADDATA = "ProjectManager/GetProjectLeadData/";
            public const string GETPROJECTRMDATA = "ProjectManager/GetProjectRMData/";
            public const string GETPROJECTMANAGERFROMALLOCATIONS = "ProjectManager/GetProjectManagerFromAllocations/";
            public const string GETALLTALENTPOOLDATA = "TalentPool/GetAll";
            public const string GETPMBYPRACTICEAREAID = "ProjectManager/GetPMByPracticeAreaId/";
            public const string RELEASEALLOCATIONS = "AssociateAllocation/ReleaseFromAllocations/";
            public const string RELEASEONEXIT = "AssociateAllocation/ReleaseOnExit/";
            public const string GETALLOCATIONSBYPROJECT = "Report/GetUtilizationReportAllocations/";
            public const string GETACTIVEALLOCATIONS = "AssociateAllocation/GetActiveAllocations/";
            public const string GETMANAGERSDETAILSBYALLOCATIONID = "AssociateAllocation/GetById/";
            public const string GETPRACTIVEAREAMANAGERS = "AssociateAllocation/GetCompetencyAreaManagersDetails/";
            public const string GETALLOCATIONBYEMPIDS = "AssociateAllocation/GetAllocationsByEmpIds?employeeIds=";
            public const string GETALLACTIVEALLOCATIONDETAILS = "AssociateAllocation/GetAllAllocationDetails";
            public const string GETPROJECTBYEMPIDANDROLE = "HRMSExternal/GetProjectsByEmpIdAndRole/";
        }
        #endregion

        #region KRAEndPoint
        public static class KRAEndPoint
        {
            public const string GETROLETYPEBYGRADEID = "RoleType/GetRoleTypesByGradeId/";
        }
        #endregion
    }
}


