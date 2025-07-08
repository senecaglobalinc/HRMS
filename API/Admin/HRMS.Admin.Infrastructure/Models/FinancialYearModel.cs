using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models
{
    public class FinancialYearModel
    {
        public int Id { get; set; }
        public string FinancialYearName { get; set; }
        public bool IsActive { get; set; }
    }
}
