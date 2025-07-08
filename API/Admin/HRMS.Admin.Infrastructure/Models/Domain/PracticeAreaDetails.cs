using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Domain
{
    public class PracticeAreaDetails
    {
        public int PracticeAreaId { get; set; }

        public string PracticeAreaCode { get; set; }

        public string PracticeAreaDescription { get; set; }

        public int? PracticeAreaHeadId { get; set; }

        public string PracticeAreaHeadName { get; set; }
    }
}
