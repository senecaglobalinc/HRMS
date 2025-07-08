using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Response
{
    public class ServiceResponse<T>:BaseServiceResponse
    {
        public T Item { get; set; }
    }
}
