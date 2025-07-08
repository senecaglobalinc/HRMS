using HRMS.Admin.Entities;
using System.Collections.Generic;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class MenuData
    {
        public int MenuId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public string Parameter { get; set; }
        public string NodeId { get; set; }
        public string Style { get; set; }
        public List<MenuData> Categories { get; set; }
        public IEnumerable<Role> MenuRoles { get; set; }
        public bool? IsActive { get; set; }

        public MenuData()
        {
            Categories = new List<MenuData>();
            MenuRoles = new List<Role>();
        }
    }
}
