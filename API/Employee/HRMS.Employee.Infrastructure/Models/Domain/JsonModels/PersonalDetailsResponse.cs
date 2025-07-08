using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Employee.Entities;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class PersonalDetailsResponse
    {
        public PersonalInformationResponse PersonalDetail { get; set; }
        public List<FamilyDetailsResponse> Family { get; set; }
        public List<AssociateCertificationsResponse> AssociateCertification { get; set; }
        public List<AssociatesMembershipResponse> AssociateMembership { get; set; }
        public List<EducationDetailsResponse> Education { get; set; }
        public List<EmployeeProjectResponse> Projects { get; set; }
        public List<EmployeeSkillResponse> Skills { get; set; }
        public List<PreviousEmploymentDetailsResponse> PreviousEmployeeDetails { get; set; }
        public List<ProfessionalReferencesResponse> ProfessionalReference { get; set; }
        public List<EmergencyContactDetailsResponse> EmergencyContact { get; set; }
    }
}
