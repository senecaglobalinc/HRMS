using HRMS.KRA.Infrastructure.Response;

namespace HRMS.KRA.Infrastructure.Models.Response
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
