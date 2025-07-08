namespace HRMS.Report.Infrastructure.Constants
{
    public static class ServicePaths
    {
        #region OrgEndPoint
        public static class OrgEndPoint
        {
            public const string GETFINANCEREPORTMASTERS = "Report/GetFinanceReportMasters";
            public const string GETUTILIZATIONREPORTMASTERSNIGHTJOB = "Nightlyjob/GetUtilizationReportMasters";
            public const string GETUTILIZATIONREPORTMASTERS = "Report/GetUtilizationReportMasters";
            public const string GETDOMAINREPORTMASTERS = "Report/GetDomainReportMasters";
            public const string GETTALENTPOOLREPORTMASTERS = "Report/GetTalentPoolReportMasters";
            public const string GETSKILLSEARCHMASTERS = "Report/GetSkillSearchMasters";
            public const string GETSERVICETYPEMASTERS = "ProjectType/GetAll?isActive=true";
            public const string GETPROJECTTYPEBYCODE = "ProjectType/GetProjectTypeByCode";
           
        }
        #endregion

        #region EmployeeEndPoint
        public static class EmployeeEndPoint
        {
            public const string GETUTILIZATIONREPORTASSOCIATESNIGHTJOB = "Nightlyjob/GetUtilizationReportAssociates";
            public const string GETUTILIZATIONREPORTASSOCIATES = "Report/GetUtilizationReportAssociates";
            public const string GETFINANCEREPORTASSOCIATES = "Report/GetFinanceReportAssociates";
            public const string GETDOMAINWISERESOURCECOUNT = "Report/GetDomainWiseResourceCount";
            public const string GETSERVICETYPECOUNT = "Report/GetServiceTypeResourceCount";
            public const string GETDOMAINREPORTASSOCIATES = "Report/GetDomainReportAssociates?employeeIds=";
            public const string GETTALENTPOOLREPORTASSOCIATES = "Report/GetTalentPoolReportAssociates?projectId=";
            public const string GETSKILLSEARCHASSOCIATES = "Report/GetSkillSearchAssociates";
            public const string GETACTIVEASSOCIATES = "Report/GetActiveAssociates";
            public const string GETPARKINGSLOTREPORT = "Report/GetParkingSlotReport";
        }
        #endregion

        #region ProjectEndPoint
        public static class ProjectEndPoint
        {
            public const string GETUTILIZATIONREPORTALLOCATIONSNIGHTJOB = "Nightlyjob/GetUtilizationReportAllocations";
            public const string GETUTILIZATIONREPORTALLOCATIONS = "Report/GetUtilizationReportAllocations";
            public const string GETFINANCEREPORTALLOCATIONS = "Report/GetFinanceReportAllocations";
            public const string GETDOMAINWISERESOURCECOUNT = "Report/GetDomainWiseResourceCount";
            public const string GETTALENTPOOLWISERESOURCECOUNT = "Report/GetTalentPoolWiseResourceCount?projectTypeIds=";
            public const string GETSERVICETYPECOUNT = "Report/GetServiceTypeProjectCount";
            public const string GETALLPROJECTS = "Report/GetAllProjects";
            public const string GETCRITICALRESOURCEREPORT = "Report/GetCriticalResourceReport";
            public const string GETNONCRITICALRESOURCEREPORT = "Report/GetNonCriticalResourceReport/";
            public const string GETNONCRITICALRESOURCEBILLINGREPORT = "Report/GetNonCriticalResourceBillingReport/";
        }
        #endregion
    }
}
