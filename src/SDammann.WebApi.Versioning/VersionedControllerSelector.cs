namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;


    /// <summary>
    ///   Represents an <see cref="IHttpControllerSelector" /> implementation that supports versioning and selects an controller based on versioning by convention (namespace.Api.Version1.xxxController).
    ///   How the actual controller to be invoked is determined, is up to the derived class to implement.
    /// </summary>
    public abstract class VersionedControllerSelector : IHttpControllerSelector {
        protected const string ControllerKey = "controller";
        public static readonly string ControllerSuffix = "Controller";
        public static readonly string VersionPrefix = "Version";

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly HttpControllerTypeCache _controllerTypeCache;

        /// <summary>
        ///   Initializes a new instance of the <see cref="System.Web.Http.Dispatcher.DefaultHttpControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public VersionedControllerSelector(HttpConfiguration configuration) {
            if (configuration == null) {
                throw new ArgumentNullException("configuration");
            }

            this._controllerInfoCache =
                    new Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>>(this.InitializeControllerInfoCache);
            this._configuration = configuration;
            this._controllerTypeCache = new HttpControllerTypeCache(this._configuration);
        }


        #region IHttpControllerSelector Members

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
                Justification = "Caller is responsible for disposing of response instance.")]
        public HttpControllerDescriptor SelectController(HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            ControllerIdentification controllerName = this.GetControllerIdentificationFromRequest(request);
            if (String.IsNullOrEmpty(controllerName.Name)) {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }

            HttpControllerDescriptor controllerDescriptor;
            if (this._controllerInfoCache.Value.TryGetValue(controllerName, out controllerDescriptor)) {
                return controllerDescriptor;
            }

            ICollection<Type> matchingTypes = this._controllerTypeCache.GetControllerTypes(controllerName);

            // ControllerInfoCache is already initialized.
            Contract.Assert(matchingTypes.Count != 1);

            if (matchingTypes.Count == 0) {
                // no matching types
                throw new HttpResponseException(request.CreateResponse(
                                                                       HttpStatusCode.NotFound,
                                                                       "The API '" + controllerName + "' doesn't exist"));
            }

            // multiple matching types
            throw new HttpResponseException(request.CreateResponse(
                                                                   HttpStatusCode.InternalServerError,
                                                                   CreateAmbiguousControllerExceptionMessage(request.GetRouteData().Route,
                                                                                                             controllerName.Name,
                                                                                                             matchingTypes)));
        }

        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping() {
            return this._controllerInfoCache.Value.ToDictionary(c => VersionPrefix + c.Key.Version + "." + c.Key.Name, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        #endregion


        /// <summary>
        /// Gets the name of the controller from the request route date
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string GetControllerNameFromRequest(HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null) {
                return default(String);
            }

            // Look up controller in route data
            object controllerName;
            routeData.Values.TryGetValue(ControllerKey, out controllerName);

            return controllerName.ToString();
        }

        protected abstract ControllerIdentification GetControllerIdentificationFromRequest (HttpRequestMessage request);

        private static string CreateAmbiguousControllerExceptionMessage(IHttpRoute route, string controllerName,
                                                                         IEnumerable<Type> matchingTypes) {
            Contract.Assert(route != null);
            Contract.Assert(controllerName != null);
            Contract.Assert(matchingTypes != null);

            // Generate an exception containing all the controller types
            StringBuilder typeList = new StringBuilder();
            foreach (Type matchedType in matchingTypes) {
                typeList.AppendLine();
                typeList.Append(matchedType.FullName);
            }

            return String.Format("Multiple possibilities for {0}, using route template {1}. The following types were selected: {2}.",
                                 controllerName,
                                 route.RouteTemplate,
                                 typeList);
        }

        private ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor> InitializeControllerInfoCache() {
            var result = new ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>(ControllerIdentification.Comparer);
            var duplicateControllers = new HashSet<ControllerIdentification>();
            Dictionary<ControllerIdentification, ILookup<string, Type>> controllerTypeGroups = this._controllerTypeCache.Cache;

            foreach (KeyValuePair<ControllerIdentification, ILookup<string, Type>> controllerTypeGroup in controllerTypeGroups) {
                ControllerIdentification controllerName = controllerTypeGroup.Key;

                foreach (IGrouping<string, Type> controllerTypesGroupedByNs in controllerTypeGroup.Value) {
                    foreach (Type controllerType in controllerTypesGroupedByNs) {
                        if (result.Keys.Contains(controllerName)) {
                            duplicateControllers.Add(controllerName);
                            break;
                        } else {
                            result.TryAdd(controllerName,
                                          new HttpControllerDescriptor(this._configuration, controllerName.Name, controllerType));
                        }
                    }
                }
            }

            foreach (ControllerIdentification duplicateController in duplicateControllers) {
                HttpControllerDescriptor descriptor;
                result.TryRemove(duplicateController, out descriptor);
            }

            return result;
        }
    }
}
