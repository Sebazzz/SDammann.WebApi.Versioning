namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System;
    using System.Runtime.Serialization;
    using Request;

    /// <summary>
    /// Represents the base class for all exceptions thrown in the API versioning system
    /// </summary>
    /// <remarks>
    /// The following exceptions are derived from this exception:
    /// <see cref="ApiControllerNotFoundException"/>
    /// <see cref="AmbigiousApiRequestException"/>
    /// <see cref="ApiVersionNotDeterminedException"/>
    /// <see cref="ApiVersionFormatException"/>
    /// </remarks>
    [Serializable]
    public abstract class BaseApiException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        protected BaseApiException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        protected BaseApiException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        protected BaseApiException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected BaseApiException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}