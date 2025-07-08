using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class ProfessionalDetail : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// ProgramTitle
        /// </summary>
        public string ProgramTitle { get; set; }

        /// <summary>
        /// ProgramType
        /// </summary>
        public string ProgramType { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// institution
        /// </summary>
        public string institution { get; set; }

        /// <summary>
        /// specialization
        /// </summary>
        public string specialization { get; set; }

        /// <summary>
        /// CurrentValidity
        /// </summary>
        public string CurrentValidity { get; set; }


    }
}
