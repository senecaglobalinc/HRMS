using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure
{
    public class KRAMailSubjects
    {
        public string SendToHOD { get; set; }
        public string SendToOpHead { get; set; }
        public string SendToCEO { get; set; }
        public string ApprovedByCEO { get; set; }
    }
}
