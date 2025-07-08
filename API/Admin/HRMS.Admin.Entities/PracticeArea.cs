using System.Collections.Generic;

namespace HRMS.Admin.Entities
{
    public class PracticeArea : BaseEntity
    {
        /// <summary>
        /// PracticeAreaId
        /// </summary>
        public int PracticeAreaId { get; set; }

        /// <summary>
        /// PracticeAreaCode
        /// </summary>
        public string PracticeAreaCode { get; set; }

        /// <summary>
        /// PracticeAreaDescription
        /// </summary>
        public string PracticeAreaDescription { get; set; }

        /// <summary>
        /// PracticeAreaHeadId
        /// </summary>
        public int? PracticeAreaHeadId { get; set; }
    }
}
