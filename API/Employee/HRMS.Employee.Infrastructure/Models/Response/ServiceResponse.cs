using HRMS.Employee.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Response
{
    /// <summary>
    /// Response for service
    /// </summary>
    public class ServiceResponse<T> : BaseServiceResponse
    {
        /// <summary>
        /// Item
        /// </summary>
        public T Item { get; set; }
    }
}
