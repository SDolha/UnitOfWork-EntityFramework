using System;
using System.Runtime.Serialization;

namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Represents a data access exception.
    /// </summary>
    [Serializable]
    public class DataAccessException : Exception
    {
        /// <summary>
        /// Initializes a data access exception object.
        /// </summary>
        public DataAccessException()
        { }

        /// <summary>
        /// Initializes a data access exception object with a message.
        /// </summary>
        public DataAccessException(string message) : base(message)
        { }

        /// <summary>
        /// Initializes a data access exception object with a message and an inner exception.
        /// </summary>
        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a data access exception object for serialization support.
        /// </summary>
        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
