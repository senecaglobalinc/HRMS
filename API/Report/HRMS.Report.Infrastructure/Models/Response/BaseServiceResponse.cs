using System.Collections.Generic;

namespace HRMS.Report.Infrastructure.Models.Response
{
    public class BaseServiceResponse
    {
        public List<ServiceResponseMessage> DetailMessages { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
