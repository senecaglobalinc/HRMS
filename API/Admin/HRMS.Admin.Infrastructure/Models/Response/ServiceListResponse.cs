using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Models.Response
{
    /// <summary>
    /// Response for list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceListResponse<T> : BaseServiceResponse
    {
        /// <summary>
        /// Items
        /// </summary>
        public List<T> Items { get; set; }                                                                                                                              
    }
}
