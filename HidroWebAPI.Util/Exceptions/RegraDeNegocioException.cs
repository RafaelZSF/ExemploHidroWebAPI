using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HidroWebAPI.Util.Exceptions
{
    [Serializable]
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException()
        {
        }

        public RegraDeNegocioException(string message) : base(message)
        {
        }

        public RegraDeNegocioException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RegraDeNegocioException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
