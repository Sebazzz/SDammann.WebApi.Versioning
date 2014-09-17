namespace SDammann.WebApi.Versioning {
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using System.Linq;
    using System.Web.Http.Controllers;


    /// <summary>
    ///   Represents an <see cref="IHttpControllerSelector" /> implementation that supports versioning and selects an controller based on versioning by convention (namespace.Api.Version1.xxxController). The controller to invoke is determined by the "version" key in the routing dictionary.
    /// </summary>
    public sealed class RouteVersionedControllerSelector : VersionedControllerSelector {
        private const string VersionKey = "version";

        /// <summary>
        ///   Initializes a new instance of the <see cref="RouteVersionedControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public RouteVersionedControllerSelector (HttpConfiguration configuration) : base(configuration) {
        }

        protected override ControllerIdentification GetControllerIdentificationFromRequest (HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null) {
                return default(ControllerIdentification);
            }

            // Look up controller in route data
            string controllerName;
            var subRoute = (routeData.GetSubRoutes() ?? Enumerable.Empty<IHttpRouteData>()).FirstOrDefault();
            if (subRoute == null)
                controllerName = this.GetControllerNameFromRequest(request);
            else
                controllerName = getControllerNameFromSubRouteData(subRoute);

            // Also try the version if possible
            object apiVersionObj;
            string apiVersion = null;
            int version;

            if (subRoute != null && subRoute.Values.TryGetValue(VersionKey, out apiVersionObj)
                && !String.IsNullOrWhiteSpace(apiVersionObj as string))
            {
                apiVersion = apiVersionObj as string;
            }
            else if (routeData.Values.TryGetValue(VersionKey, out apiVersionObj) &&
                     !String.IsNullOrWhiteSpace(apiVersionObj as string))
            {
                apiVersion = apiVersionObj as string;
            }

            return new ControllerIdentification(controllerName, apiVersion);
        }

        private string getControllerNameFromSubRouteData(IHttpRouteData pRouteData)
        {
            var descriptors = pRouteData.Route.DataTokens["actions"] as HttpActionDescriptor[];
            if (descriptors == null || descriptors.Length == 0)
                return string.Empty;

            return descriptors[0].ControllerDescriptor.ControllerName;
        }
    }
}