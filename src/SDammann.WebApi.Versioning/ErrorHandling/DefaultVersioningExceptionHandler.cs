namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.ExceptionHandling;
    using Request;

    /// <summary>
    /// Represents a class that can handle exceptions thrown by the versioning system
    /// </summary>
    public class DefaultVersioningExceptionHandler : IVersioningExceptionHandler {
        /// <summary>
        /// Handles the specified exception by rethrowing it
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public virtual HttpResponseMessage HandleException(ExceptionContext context, BaseApiException ex) {
            HttpResponseMessage result = null;

            result = result ?? HandleExceptionInternal(context, ex as ApiControllerNotFoundException);
            result = result ?? HandleExceptionInternal(context, ex as AmbigiousApiRequestException);
            result = result ?? HandleExceptionInternal(context, ex as ApiVersionFormatException);
            result = result ?? HandleExceptionInternal(context, ex as ApiVersionNotDeterminedException);

            return result;
        }

        private static HttpResponseMessage HandleExceptionInternal(ExceptionContext context, ApiVersionNotDeterminedException exception) {
            if (exception == null) {
                return null;
            }

            return context.Request.CreateResponse(HttpStatusCode.BadRequest, exception.Message);
        }

        private static HttpResponseMessage HandleExceptionInternal(ExceptionContext context, ApiVersionFormatException exception) {
            if (exception == null) {
                return null;
            }

            return context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ExceptionResources.CannotDetermineRequestVersion, exception);
        }

        private static HttpResponseMessage HandleExceptionInternal(ExceptionContext context, AmbigiousApiRequestException exception) {
            if (exception == null) {
                return null;
            }

            // multiple matching types
            return context.Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message);
        }

        private static HttpResponseMessage HandleExceptionInternal(ExceptionContext context, ApiControllerNotFoundException exception) {
            if (exception == null) {
                return null;
            }

            return context.Request.CreateResponse(HttpStatusCode.NotFound, String.Format(ExceptionResources.ApiDoesntExist, exception.ControllerIdentification));
        }
    }
}