using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class MenuRole : BaseEntity
    {
        public int MenuRoleId { get; set; }
        public int MenuId { get; set; }
        public int? RoleId { get; set; }
    }
}
