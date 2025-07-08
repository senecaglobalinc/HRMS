using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class MenuRoleDetails
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Menus> MenuList { get; set; }

    }

    public class Menus
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
}
