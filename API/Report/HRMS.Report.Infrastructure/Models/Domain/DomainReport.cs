namespace HRMS.Report.Infrastructure.Models.Domain
{
    public class DomainReportCount
    {
        public int ResourceCount { get; set; }
        public int DomainID { get; set; }
        public string DomainName { get; set; }
    }

    public class DomainDataCount
    {
        public int DomainID { get; set; }
        public int EmployeeID { get; set; }
    }

    public class ServiceTypeReportCount
    {
        public int? ResourceCount { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeDescription { get; set; }
        public string ServiceTypeCode { get; set; }
    }
    public class ServiceTypeCount
    {
        public int ServiceTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class DomainReportEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public int DesignationId { get; set; }
        public int GradeId { get; set; }
    }

    public class DomainReport
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
    }

    public class ServiceTypeReportEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public int DesignationId { get; set; }
        public int GradeId { get; set; }
    }

    public class ServiceTypeReport
    {
        public string EmployeeCode { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Experience { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public string ProjectName { get; set; }
    }
}
