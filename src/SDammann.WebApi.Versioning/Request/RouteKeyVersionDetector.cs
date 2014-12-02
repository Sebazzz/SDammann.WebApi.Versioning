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

            Version versionNumber = ParseVersionNumber(rawVersionNumber);

            return new SemVerApiVersion(versionNumber);
        }

        /// <summary>
        /// Parses the version number from the specified string
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
            if (version == null && !Version.TryParse(rawVersionNumber, out version))
            {
                return null;
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
            routeData.Values.TryGetValue(routeKey, out controllerVersion);

            string rawVersionNumber = Convert.ToString(controllerVersion, CultureInfo.InvariantCulture);
            return rawVersionNumber;
        }
    }
}