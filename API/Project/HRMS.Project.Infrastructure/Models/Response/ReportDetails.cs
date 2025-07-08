using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{    
    public class ReportDetails
    {
        public int RecordType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum RecordTypeEnum
    {
        Associate = 1,
        Manager = 2,
        Client = 3,
        Skill = 4,
        Department = 5,
        Designation = 6,
        Grade = 7,
        PracticeArea = 8,
        Domain = 9,
        ProjectType = 10,
        GradeRoleTypes = 11,
        FinancialYears = 12,
        RoleTypes = 13,
        ProjectStatus = 14
    }
}
