namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ContactDetails
    {
        public int ID { get; set; }
        public string addressType { get; set; }
        public string currentAddCity { get; set; }
        public string currentAddCountry { get; set; }
        public string currentAddState { get; set; }
        public string currentAddZip { get; set; }
        public string permanentAddCity { get; set; }
        public string permanentAddCountry { get; set; }
        public string permanentAddState { get; set; }
        public string permanentAddZip { get; set; }
        public string currentAddress1 { get; set; }
        public string currentAddress2 { get; set; }
        public string permanentAddress1 { get; set; }
        public string permanentAddress2 { get; set; }

    }
}
