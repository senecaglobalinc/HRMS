using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Response
{
    /// <summary>
    /// BaseServiceResponse class
    /// </summary>
    public class BaseServiceResponse
    {
        public List<ServiceResponseMessage> DetailMessages { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
