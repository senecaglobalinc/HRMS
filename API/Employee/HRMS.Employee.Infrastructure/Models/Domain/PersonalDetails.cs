using HRMS.Common;
using System;
using System.Collections.Generic;
namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class PersonalDetails
    {
        public int ID { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string empName { get; set; }
        public string lastName { get; set; }
        public int empID { get; set; }
        public string EncryptedPhoneNumber { get; set; }
        public string phoneNumber
        {
            get { return Utility.DecryptStringAES(EncryptedPhoneNumber); }
            set { EncryptedPhoneNumber = Utility.EncryptStringAES(value); }
        }
        public string EncryptedMobileNo { get; set; }
        public string mobileNo
        {
            get { return Utility.DecryptStringAES(EncryptedMobileNo); }
            set { EncryptedMobileNo = Utility.EncryptStringAES(value); }
        }
        public string personalEmail { get; set; }
        public DateTime? dob { get; set; }
        public DateTime? doj { get; set; }
        public string maritalStatus { get; set; }
        public string gender { get; set; }
        public string EncryptedPanNumber { get; set; }
        public DateTime? bgvStartDate { get; set; }
        public DateTime? bgvCompletedDate { get; set; }
        public string panNumber
        {
            get { return Utility.DecryptStringAES(EncryptedPanNumber); }
            set { EncryptedPanNumber = Utility.EncryptStringAES(value); }
        }
        public string EncryptedPassportNumber { get; set; }
        public string passportNumber
        {
            get { return Utility.DecryptStringAES(EncryptedPassportNumber); }
            set { EncryptedPassportNumber = Utility.EncryptStringAES(value); }
        }
        public string passportIssuingOffice { get; set; }

        public string passportValidDate { get; set; }
        public string EncryptedAadharNumber { get; set; }
        public string aadharNumber
        {
            get { return Utility.DecryptStringAES(EncryptedAadharNumber); }
            set { EncryptedAadharNumber = Utility.EncryptStringAES(value); }
        }

        public string EncryptedPFNumber { get; set; }
        public string pfNumber
        {
            get { return Utility.DecryptStringAES(EncryptedPFNumber); }
            set { EncryptedPFNumber = Utility.EncryptStringAES(value); }
        }

        public string EncryptedUANNumber { get; set; }
        public string uanNumber
        {
            get { return Utility.DecryptStringAES(EncryptedUANNumber); }
            set { EncryptedUANNumber = Utility.EncryptStringAES(value); }
        }
        public string bloodGroup { get; set; }
        public string nationality { get; set; }
        public string empCode { get; set; }
        public string workEmailID { get; set; }
        public int? bgvStatusID { get; set; }
        public ContactDetails contacts { get; set; }
        public string bgvStatus { get; set; }
        public string Birthdate { get; set; }
        public string employmentType { get; set; }
        public IEnumerable<ContactDetails> contactDetails { get; set; }
        public string Experience { get; set; }
        public DateTime? joiningDate { get; set; }
        public int ReportingManagerId { get; set; }
        public string hrAdvisor { get; set; }
        public int? designationID { get; set; }
        public string designation { get; set; }
        public int? deptID { get; set; }
        public string department { get; set; }
        public int? gradeID { get; set; }
        public int? technologyID { get; set; }
        public string GradeName { get; set; }
        public string Technology { get; set; }
        public string ReportingManager { get; set; }
        public DateTime? EmploymentStartDate { get; set; }
        public DateTime? EmplStartDate { get; set; }
        public int? CareerBreak { get; set; }
        public int? KRARoleId { get; set; }
    }
}
