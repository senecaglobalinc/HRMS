using HRMS.Common;
using System;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class RelationDetails
    {
        public int ID { get; set; }
        public string EncryptedName { get; set; }
        public string name
        {
            get { return Utility.DecryptStringAES(EncryptedName); }
            set { EncryptedName = Utility.EncryptStringAES(value); }
        }
        public int? empID { get; set; }
        public string relationship { get; set; }
        public DateTime? dob { get; set; }
        public string occupation { get; set; }
        public string birthDate { get; set; }
    }
}
