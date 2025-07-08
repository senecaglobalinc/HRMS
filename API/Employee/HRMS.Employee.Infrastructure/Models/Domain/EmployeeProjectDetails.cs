namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmployeeProjectDetails
    {
        public int ID { get; set; }
        public int? empID { get; set; }
        public string organizationName { get; set; }
        public string projectName { get; set; }
        public int? duration { get; set; }
        public int? RoleMasterId { get; set; }
        public string roleName { get; set; }
        public string keyAchievement { get; set; }
        public int DomainID { get; set; }
        public string domainName { get; set; }

    }
}
