namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Request;

    /// <summary>
    /// Represents a class that can handle exceptions thrown by the versioning system
    /// </summary>
    public sealed class VersionExceptionFilter : IVersionExceptionFilter {
        /// <summary>
        /// Handles the specified exception by rethrowing it
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        public void HandleException(HttpControllerContext context, BaseApiException ex) {
            HandleExceptionInternal(context, ex as ApiControllerNotFoundException);
            HandleExceptionInternal(context, ex as AmbigiousApiRequestException);
            HandleExceptionInternal(context, ex as ApiVersionFormatException);
            HandleExceptionInternal(context, ex as ApiVersionNotDeterminedException);
        }

        private static void HandleExceptionInternal(HttpControllerContext context, ApiVersionNotDeterminedException exception) {
            if (exception == null) {
                return;
            }

            throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.BadRequest, exception.Message));
        }

        private static void HandleExceptionInternal(HttpControllerContext context, ApiVersionFormatException exception) {
            if (exception == null) {
                return;
            }

            throw new HttpResponseException(context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ExceptionResources.CannotDetermineRequestVersion, exception));
        }

        private static void HandleExceptionInternal(HttpControllerContext context, AmbigiousApiRequestException exception) {
            if (exception == null) {
                return;
            }

            // multiple matching types
            throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message));
        }

        private static void HandleExceptionInternal(HttpControllerContext context, ApiControllerNotFoundException exception) {
            if (exception == null) {
                return;
            }

            throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.NotFound, "The API '" + exception.ControllerIdentification + "' doesn't exist"));
        }
    }
}