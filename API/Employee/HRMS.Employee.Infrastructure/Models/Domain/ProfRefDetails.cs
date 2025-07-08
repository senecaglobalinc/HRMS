namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public partial class ProfRefDetails
    {
        public int ID { get; set; }
        public int? empID { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public string officeEmailAddress { get; set; }
        public string mobileNo { get; set; }
    }
}
