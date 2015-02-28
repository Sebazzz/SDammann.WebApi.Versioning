namespace SDammann.WebApi.Versioning.Request {
    using System;
    using System.Runtime.Serialization;
    using ErrorHandling;

    /// <summary>
    /// The exception that is thrown when an API version cannot be parsed
    /// </summary>
    [Serializable]
    public class ApiVersionFormatException : BaseApiException {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.FormatException"/> class.
        /// </summary>
        public ApiVersionFormatException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.FormatException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public ApiVersionFormatException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.FormatException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
        public ApiVersionFormatException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.FormatException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data. </param><param name="context">The contextual information about the source or destination. </param>
        protected ApiVersionFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}