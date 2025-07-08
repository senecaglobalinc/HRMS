using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class ProgramManagersData
    {
        public List<ProgramManagerDTO> ProgramManagers { get; set; }
    }
    public class ProgramManagerDTO
    {
        public int ApproverId { get; set; }
        public string ApproverCode { get; set; }
        public string ApproverName { get; set; }
    }
}
