using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EmergencyContactData
    {
        public int ID { get; set; }
        public bool? isPrimary { get; set; }
        public string contactType { get; set; }
        public int employeeID { get; set; }
        public string contactName { get; set; }
        public string relationship { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string telephoneNo { get; set; }
        public string mobileNo { get; set; }
        public string emailAddress { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
        public string EncryptedMobileNo { get; set; }
        public object EncryptedTelePhoneNo { get; set; }
    }
}
