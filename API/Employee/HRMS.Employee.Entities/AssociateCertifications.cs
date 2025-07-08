using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class AssociateCertification : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// CertificationId
        /// </summary>
        public int CertificationId { get; set; }

        /// <summary>
        /// ValidFrom
        /// </summary>
        public string ValidFrom { get; set; }

        /// <summary>
        /// Institution
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// Specialization
        /// </summary>
        public string Specialization { get; set; }

        /// <summary>
        /// ValidUpto
        /// </summary>
        public string ValidUpto { get; set; }

        /// <summary>
        /// SkillGroupId
        /// </summary>
        public int SkillGroupId { get; set; }
    }
}
