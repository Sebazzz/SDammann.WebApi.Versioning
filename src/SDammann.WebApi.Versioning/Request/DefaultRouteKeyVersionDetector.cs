namespace SDammann.WebApi.Versioning.Request {
    /// <summary>
    /// Implements a <see cref="RouteKeyVersionDetector"/> which uses a default '
    /// </summary>
    public sealed class DefaultRouteKeyVersionDetector : RouteKeyVersionDetector {
        private const string DefaultRouteKey = "version";

        /// <summary>
        /// Gets the routing key used for determining API versions
        /// </summary>
        protected override string RouteKey {
            get { return DefaultRouteKey; }
        }
    }
}