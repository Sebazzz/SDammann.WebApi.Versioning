namespace SDammann.WebApi.Versioning.Request {
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Web.Http.Routing;

    /// <summary>
    /// Implementation of <see cref="IRequestControllerNameDetector"/> which detects the controller name based on a Route value
    /// </summary>
    public abstract class RouteKeyRequestControllerNameDetector : IRequestControllerNameDetector {
        /// <summary>
        /// Gets the route key thats used for determine the controller name
        /// </summary>
        protected abstract string RouteKey { get; }

        /// <summary>
        /// Gets a name for the controller from the specified request message. Returns null if no controller name could be detected.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public string GetControllerName(HttpRequestMessage requestMessage) {
            if (requestMessage == null) {
                throw new ArgumentNullException("requestMessage");
            }

            IHttpRouteData routeData = requestMessage.GetRouteData();
            if (routeData == null)
            {
                return default(String);
            }

            return this.GetControllerNameFromRouteData(routeData);
        }

        /// <summary>
        /// Gets the controller name from routing data
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        protected virtual string GetControllerNameFromRouteData(IHttpRouteData routeData) {
            // Look up controller in route data
            object controllerName;
            routeData.Values.TryGetValue(this.RouteKey, out controllerName);

            return Convert.ToString(controllerName, CultureInfo.InvariantCulture);
        }
    }
}