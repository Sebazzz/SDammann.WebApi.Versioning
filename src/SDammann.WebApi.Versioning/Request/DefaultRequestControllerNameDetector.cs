namespace SDammann.WebApi.Versioning.Request {
    /// <summary>
    /// Default implementation of <see cref="IRequestControllerNameDetector"/> which detects the controller name using the default 'controller' route key
    /// </summary>
    public sealed class DefaultRequestControllerNameDetector : RouteKeyRequestControllerNameDetector {
        private const string DefaultRouteKey = "controller";

        /// <summary>
        /// Gets the route key thats used for determine the controller name
        /// </summary>
        protected override string RouteKey {
            get { return DefaultRouteKey; }
        }
    }
}