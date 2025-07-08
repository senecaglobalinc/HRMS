using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class EducationDetailsResponse
    {
        /// <summary>
        /// PersonalInfoId
        /// </summary>
        public Guid PersonalInfoId { get; set; }

        /// <summary>
        /// EducationalQualification
        /// </summary>
        public string EducationalQualification { get; set; }

        /// <summary>
        /// AcademicCompletedYear
        /// </summary>
        public DateTime? AcademicCompletedYear { get; set; }

        /// <summary>
        /// Institution
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// Specialization
        /// </summary>
        public string Specialization { get; set; }

        /// <summary>
        /// ProgramType
        /// </summary>
        public string ProgramType { get; set; }

        /// <summary>
        /// Grade
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Marks
        /// </summary>
        public string Marks { get; set; }

        /// <summary>
        /// AcademicYearFrom
        /// </summary>
        public string AcademicYearFrom { get; set; }

        /// <summary>
        /// AcademicYearTo
        /// </summary>
        public string AcademicYearTo { get; set; }

        /// <summary>
        /// ProgramTypeId
        /// </summary>
        public int? ProgramTypeId { get; set; }

        public bool IsActive { get; set; }

    }
}
