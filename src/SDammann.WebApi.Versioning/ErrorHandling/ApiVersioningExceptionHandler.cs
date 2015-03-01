namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Results;
    using Internal;

    /// <summary>
    /// Represents the default exception handler
    /// </summary>
    public sealed class ApiVersioningExceptionHandler : ExceptionHandler {
        /// <summary>
        /// When overridden in a derived class, handles the exception synchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context) {
            HttpConfiguration httpConfig = context.ExceptionContext.RequestContext.Configuration;

            BaseApiException exceptionThrown = context.Exception as BaseApiException;
            if (exceptionThrown != null) {
                IVersioningExceptionHandler exceptionHandler = httpConfig.DependencyResolver.GetVersionExceptionFilter();
                HttpResponseMessage result = exceptionHandler.HandleException(context.ExceptionContext, exceptionThrown);

                if (result != null) {
                    context.Result = new ResponseMessageResult(result);
                }
            }
        }

        /// <summary>
        /// Determines whether the exception should be handled.
        /// </summary>
        /// <returns>
        /// true if the exception should be handled; otherwise, false.
        /// </returns>
        /// <param name="context">The exception handler context.</param>
        public override bool ShouldHandle(ExceptionHandlerContext context) {
            return context.Exception is BaseApiException;
        }
    }
}