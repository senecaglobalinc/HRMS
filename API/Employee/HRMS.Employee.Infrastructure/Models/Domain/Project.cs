using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Domain
{
    public class Project
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// ProjectCode
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// ClientId
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// ProjectTypeId
        /// </summary>
        public int? ProjectTypeId { get; set; }

        /// <summary>
        /// ProjectStateId
        /// </summary>
        public int ProjectStateId { get; set; }

        /// <summary>
        /// ActualStartDate
        /// </summary>
        public DateTime? ActualStartDate { get; set; }

        /// <summary>
        /// ActualEndDate
        /// </summary>
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// PracticeAreaId
        /// </summary>
        public int PracticeAreaId { get; set; }

        /// <summary>
        /// DomainId
        /// </summary>
        public int? DomainId { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        public bool? IsActive { get; set; }

    }

    public class TalentPoolDetails {

        public int projectId;
        public int EmployeeId;
        public DateTime ReleaseDate;
    }
}
