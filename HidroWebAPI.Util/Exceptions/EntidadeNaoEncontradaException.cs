using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HidroWebAPI.Util.Exceptions
{
    [Serializable]
    public class EntidadeNaoEncontradaException : Exception
    {
        public EntidadeNaoEncontradaException()
        {
        }

        public EntidadeNaoEncontradaException(string message) : base(message)
        {
        }

        public EntidadeNaoEncontradaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntidadeNaoEncontradaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
