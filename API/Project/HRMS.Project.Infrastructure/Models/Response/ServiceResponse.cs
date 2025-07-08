using HRMS.Project.Entities;

using HRMS.Project.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Project.Infrastructure.Models.Response
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

        public static implicit operator ServiceResponse<T>(Addendum v)
        {
            throw new NotImplementedException();
        }
    }
}
