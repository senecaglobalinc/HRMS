using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ProfessionalDetails
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string ProgramTitle { get; set; }
        public string ValidFrom { get; set; }
        public string Institution { get; set; }
        public string Specialization { get; set; }
        public string ValidUpto { get; set; }
        public int CertificationId { get; set; }
        public int SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public string SkillName { get; set; }
        public int ProgramType { get; set; }
        
    }
}
