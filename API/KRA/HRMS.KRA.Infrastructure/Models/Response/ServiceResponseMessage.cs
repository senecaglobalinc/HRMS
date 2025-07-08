using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Response
{
    /// <summary>
    /// ServiceResponseMessage class
    /// </summary>
    public class ServiceResponseMessage : BaseServiceResponse
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
        public MessageTypes MessageType { get; set; }

        public enum MessageTypes
        {
            Success = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }
    }
}
