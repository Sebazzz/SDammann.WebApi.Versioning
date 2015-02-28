namespace SDammann.WebApi.Versioning.ErrorHandling {
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using Internal;

    /// <summary>
    /// Represents the default exception filter, delegating to <see cref="IVersionExceptionFilter"/>
    /// </summary>
    public class DefaultExceptionFilter : IExceptionFilter {
        private readonly Task _nop = Task.FromResult(0);

        /// <summary>
        /// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        /// <returns>
        /// true if more than one instance is allowed to be specified; otherwise, false. The default is false.
        /// </returns>
        public bool AllowMultiple {
            get { return false; }
        }

        /// <summary>
        /// Executes an asynchronous exception filter.
        /// </summary>
        /// <returns>
        /// An asynchronous exception filter.
        /// </returns>
        /// <param name="actionExecutedContext">The action executed context.</param><param name="cancellationToken">The cancellation token.</param>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken) {
            HttpConfiguration httpConfig = actionExecutedContext.ActionContext.ControllerContext.Configuration;

            BaseApiException exceptionThrown = actionExecutedContext.Exception as BaseApiException;
            if (exceptionThrown != null) {
                IVersionExceptionFilter exceptionFilter = httpConfig.DependencyResolver.GetVersionExceptionFilter();
                exceptionFilter.HandleException(actionExecutedContext.ActionContext.ControllerContext, exceptionThrown);
            }

            return this._nop;
        }
    }
}