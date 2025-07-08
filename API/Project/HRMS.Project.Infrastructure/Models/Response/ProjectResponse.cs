using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
{
    public class ProjectResponse
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string PracticeAreaCode { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectTypeDescription { get; set; }
        public string StatusCode { get; set; }
        public int StatusId { get; set; }
        public int ProjectStateId { get; set; }
        public string ProjectState { get; set; }
        public string ManagerName { get; set; }
        public int ManagerId { get; set; }
        public int PracticeAreaId { get; set; }
        public int ProjectTypeId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public bool? IsActive { get; set; }
        public int ProgramManagerId { get; set; }
        public string UserRole { get; set; }
        public string ProgramManager { get; set; }
        public int? ReportingManagerId { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PracticeArea { get; set; }
    }

    public class GenericType
    {
        public int Id { get; set; }        
        public string Name { get; set; }
    }
   }
