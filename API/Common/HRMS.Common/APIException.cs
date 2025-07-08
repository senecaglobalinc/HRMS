using System;
using System.Runtime.Serialization;

namespace HRMS.Common
{
    [Serializable]
    public class APIException : Exception
    {
        public APIException()
        : base() { }

        public APIException(string message)
        : base(message) { }

        public APIException(string format, params object[] args)
        : base(string.Format(format, args)) { }

        public APIException(string message, Exception innerException)
        : base(message, innerException) { }

        public APIException(string format, Exception innerException, params object[] args)
        : base(string.Format(format, args), innerException) { }

        public APIException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
    }
}
