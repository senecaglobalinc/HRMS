using System;
using System.Collections.Generic;

namespace HRMS.Employee.Entities
{
    public class AssociateExitActivity : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int AssociateExitActivityId { get; set; }

        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int? AssociateExitId { get; set; }

        /// <summary>
        /// AssociateAbscondId
        /// </summary>
        public int? AssociateAbscondId { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }

        
        /// <summary>
        /// NoDues
        /// </summary>
        public bool NoDues { get; set; }

        /// <summary>
        /// DueAmount
        /// </summary>
        public Decimal? DueAmount { get; set; }

        /// <summary>
        /// AssetsNotHanded
        /// </summary>
        public string AssetsNotHanded { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        public virtual AssociateExit AssociateExit { get; set; }

        public virtual AssociateAbscond AssociateAbscond { get; set; }

        public virtual ICollection<AssociateExitActivityDetail> AssociateExitActivityDetail { get; set; }
    }
}
