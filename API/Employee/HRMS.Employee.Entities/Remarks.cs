using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class Remarks:BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// AssociateExitId
        /// </summary>
        public int AssociateExitId { get; set; }
        /// <summary>
        /// RoleId
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Comment { get; set; }
        public virtual AssociateExit AssociateExit { get; set; }
    }
}
