namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System.Net.Http;
    using System.Web.Http.ExceptionHandling;

    /// <summary>
    /// Represents the interface for a handler that can handle exceptions thrown by the versioning system
    /// </summary>
    public interface IVersioningExceptionHandler {
        /// <summary>
        /// Handles the specified exception by rethrowing it
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        HttpResponseMessage HandleException(ExceptionContext context, BaseApiException ex);
    }
}