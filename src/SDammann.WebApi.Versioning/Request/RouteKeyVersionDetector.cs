namespace SDammann.WebApi.Versioning.Request {
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Web.Http.Routing;

    /// <summary>
    /// Implements a <see cref="IRequestVersionDetector"/> which implements version detection by route key
    /// </summary>
    public abstract class RouteKeyVersionDetector : IRequestVersionDetector {
        /// <summary>
        /// Gets the routing key used for determining API versions
        /// </summary>
        protected abstract string RouteKey { get; }

        /// <summary>
        /// Gets the <see cref="ApiVersion"/> requested from the request. Returns null if no API version could be detected.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public ApiVersion GetVersion(HttpRequestMessage requestMessage) {
            if (requestMessage == null) {
                throw new ArgumentNullException("requestMessage");
            }

            IHttpRouteData routeData = requestMessage.GetRouteData();
            if (routeData == null) {
                return default(ApiVersion);
            }

            return this.GetControllerVersionFromRouteData(routeData);
        }

        /// <summary>
        /// Gets the requested controller version from routing data
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        protected virtual ApiVersion GetControllerVersionFromRouteData(IHttpRouteData routeData) {
            string rawVersionNumber = this.GetStringRouteValue(routeData, this.RouteKey);

            if (rawVersionNumber == null) {
                return null;
            }

            Version versionNumber = ParseVersionNumber(rawVersionNumber);

            return new SemVerApiVersion(versionNumber);
        }

        /// <summary>
        /// Parses the version number from the specified string or returns <c>null</c> if the version number cannot be parsed
        /// </summary>
        /// <param name="rawVersionNumber"></param>
        /// <returns></returns>
        protected static Version ParseVersionNumber(string rawVersionNumber) {
            // System.Version does not parse a single integer, so we should do it ourselves
            Version version = null;
            if (rawVersionNumber.IndexOf('.') == -1)
            {
                int singleVersionNumber;
                if (Int32.TryParse(rawVersionNumber, NumberStyles.None, CultureInfo.InvariantCulture, out singleVersionNumber))
                {
                    version = new Version(singleVersionNumber, 0);
                }
            }

            // parse via default path
            if (version == null && !Version.TryParse(rawVersionNumber, out version)) {
                const string msg = "Cannot parse '{0}' as a version number";
                throw new ApiVersionFormatException(String.Format(msg, rawVersionNumber));
            }

            return version;
        }

        /// <summary>
        /// Parses a raw version number from the routing key
        /// </summary>
        /// <returns></returns>
        protected string GetStringRouteValue(IHttpRouteData routeData, string routeKey) {
            // Look up controller in route data
            object controllerVersion;
            if (!routeData.Values.TryGetValue(routeKey, out controllerVersion)) {
                const string msg = "Cannot retrieve the version number from the routing data. This probably means you haven't included a '{0}' key in your route configuration.";
                throw new InvalidOperationException(String.Format(msg, this.RouteKey));
            }

            if (controllerVersion == null) {
                return null;
            }

            // convert instead of casting - allows all default values
            string rawVersionNumber = Convert.ToString(controllerVersion, CultureInfo.InvariantCulture);
            return rawVersionNumber;
        }
    }
}