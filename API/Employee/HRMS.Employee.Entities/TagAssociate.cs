using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Entities
{
    public class TagAssociate : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TagAssociateListName
        /// </summary>
        public string TagAssociateListName { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// ManagerId
        /// </summary>
        public int ManagerId { get; set; }       
    }
}
