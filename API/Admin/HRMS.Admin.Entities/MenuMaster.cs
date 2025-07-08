using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Entities
{
    public class MenuMaster : BaseEntity
    {
        public int MenuId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int DisplayOrder { get; set; }
        public int ParentId { get; set; }    
        public string Parameter { get; set; }
        public string NodeId { get; set; }
        public string Style { get; set; }
    }
}
