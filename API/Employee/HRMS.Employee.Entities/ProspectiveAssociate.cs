using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class ProspectiveAssociate : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// DesignationId
        /// </summary>
        public Nullable<int> DesignationId { get; set; }

        /// <summary>
        /// GradeId
        /// </summary>
        public Nullable<int> GradeId { get; set; }

        /// <summary>
        /// Technology
        /// </summary>
        public string Technology { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public Nullable<int> DepartmentId { get; set; }

        /// <summary>
        /// HRAdvisorName
        /// </summary>
        public string HRAdvisorName { get; set; }

        /// <summary>
        /// JoiningStatusId
        /// </summary>
        public Nullable<int> JoiningStatusId { get; set; }

        /// <summary>
        /// JoinDate
        /// </summary>
        public Nullable<System.DateTime> JoinDate { get; set; }

        /// <summary>
        /// EmploymentType
        /// </summary>
        public string EmploymentType { get; set; }

        /// <summary>
        /// MaritalStatus
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// BGVStatusId
        /// </summary>
        public Nullable<int> BGVStatusId { get; set; }

        /// <summary>
        /// TechnologyID
        /// </summary>
        public Nullable<int> TechnologyID { get; set; }

        /// <summary>
        /// EmployeeID
        /// </summary>
        public Nullable<int> EmployeeID { get; set; }

        /// <summary>
        /// RecruitedBy
        /// </summary>
        public string RecruitedBy { get; set; }

        /// <summary>
        /// StatusID
        /// </summary>
        public Nullable<int> StatusID { get; set; }

        /// <summary>
        /// ReasonForDropOut
        /// </summary>
        public string ReasonForDropOut { get; set; }

        /// <summary>
        /// ManagerId
        /// </summary>
        public Nullable<int> ManagerId { get; set; }

        /// <summary>
        /// PersonalEmailAddress
        /// </summary>
        public string PersonalEmailAddress { get; set; }

        /// <summary>
        /// MobileNo
        /// </summary>
        public string MobileNo { get; set; }

    }
}
