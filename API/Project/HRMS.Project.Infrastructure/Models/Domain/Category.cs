using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Category
    {
        public int CategoryMasterId { get; set; }
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
