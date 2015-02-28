namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Represents the interface for a handler that can handle exceptions thrown by the versioning system
    /// </summary>
    public interface IVersionExceptionFilter {
        /// <summary>
        /// Handles the specified exception by rethrowing it
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        void HandleException(HttpControllerContext context, BaseApiException ex);
    }
}