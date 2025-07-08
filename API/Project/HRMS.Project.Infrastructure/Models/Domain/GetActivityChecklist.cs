using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class GetActivityChecklist
    {
        public int ActivityId { get; set; }
        public string Value { get; set; }
        public string Remarks { get; set; }
    }
}
