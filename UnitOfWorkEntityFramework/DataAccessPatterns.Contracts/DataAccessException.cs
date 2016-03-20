using System;
using System.Runtime.Serialization;

namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Represents a data access exception.
    /// </summary>
    public class DataAccessException : Exception
    {
        public DataAccessException()
        { }

        public DataAccessException(string message) : base(message)
        { }

        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        { }

        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
