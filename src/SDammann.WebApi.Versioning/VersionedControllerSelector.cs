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
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using Resources;
    using Util;


    /// <summary>
    ///   Represents an <see cref="IHttpControllerSelector" /> implementation that supports versioning and selects an controller based on versioning by convention (namespace.Api.Version1.xxxController).
    ///   How the actual controller to be invoked is determined, is up to the derived class to implement.
    /// </summary>
    public abstract class VersionedControllerSelector : IHttpControllerSelector {
        private static LockValue<string> _VersionPrefix = "Version";
        private static LockValue<string> _ControllerSuffix = DefaultHttpControllerSelector.ControllerSuffix;

        protected const string ControllerKey = "controller";

        /// <summary>
        /// Gets the suffix in the Controller <see cref="Type"/>s <see cref="Type.Name"/>
        /// </summary>
        public static string ControllerSuffix {
            get { return _ControllerSuffix; }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                if (String.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentException(String.Format(ExceptionStrings.CannotSetEmptyValue, "ControllerSuffix"), "value");
                }

                if (_ControllerSuffix.IsLocked) {
                    throw new InvalidOperationException(String.Format(ExceptionStrings.ControllerDiscoveryProcessAlreadyRun, "ControllerSuffix"));
                }

                _ControllerSuffix = value;
            }
        }

        /// <summary>
        /// Gets the prefix used for identifying a controller version in a <see cref="Type"/>.<see cref="Type.FullName"/>. Examples and usage in remarks.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///     Make sure to set this property in the Application_Start method.
        /// </para>
        /// 
        ///  <para>
        ///     For example, when this is set to "V", a controller in the namespace of Company.V1.ProductController will identify the ProductController as being version 1, but will not identify 
        ///     Company.Version1.ProductController as being a version 1 controller.
        /// </para>
        /// </remarks>
        public static string VersionPrefix {
            get { return _VersionPrefix; }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                if (String.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentException(String.Format(ExceptionStrings.CannotSetEmptyValue, "VersionPrefix"), "value");
                }

                if (_VersionPrefix.IsLocked) {
                    throw new InvalidOperationException(String.Format(ExceptionStrings.ControllerDiscoveryProcessAlreadyRun, "VersionPrefix"));
                }

                _VersionPrefix = value;
            }
        }

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly HttpControllerTypeCache _controllerTypeCache;

        /// <summary>
        ///   Initializes a new instance of the <see cref="System.Web.Http.Dispatcher.DefaultHttpControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        protected VersionedControllerSelector(HttpConfiguration configuration) {
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

            if (controllerName.Version == null) controllerName.Version = this.GetVersionRouteDefaults(request);
            
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

        private string GetVersionRouteDefaults(HttpRequestMessage request)
        {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            object data;

            if (request.GetRouteData().Values.TryGetValue("version", out data))
            {
                return data as string;
            }
            return null;
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

            object controllerName;
            var subRoute = (routeData.GetSubRoutes() ?? Enumerable.Empty<IHttpRouteData>()).FirstOrDefault();
            if (subRoute == null)
                routeData.Values.TryGetValue(ControllerKey, out controllerName);
            else
                controllerName = GetControllerNameFromSubRouteData(subRoute);

            return controllerName.ToString();
        }

        private string GetControllerNameFromSubRouteData(IHttpRouteData pRouteData)
        {
            var descriptors = pRouteData.Route.DataTokens["actions"] as HttpActionDescriptor[];
            if (descriptors == null || descriptors.Length == 0)
                return string.Empty;

            return descriptors[0].ControllerDescriptor.ControllerName;
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
            // lock dependend properties
            _VersionPrefix.Lock();


            // let's find and cache the found controllers
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

        /// <summary>
        /// Determines whether part of a namespace string is a valid version number
        /// </summary>
        /// <param name="namespacePart">Part of a namespace string to check</param>
        /// <returns>True if the namespace part is a valid version number, false otherwise</returns>
        internal static bool IsVersionNumber(string namespacePart) {
            return Regex.IsMatch(namespacePart, String.Format(@"{0}[0-9]+(_[0-9]+)*", VersionPrefix));
        }

        /// <summary>
        /// Converts part of a namespace string to a valid version number
        /// </summary>
        /// <param name="namespacePart">Part of a namespace string to convert</param>
        /// <returns>A string containing a valid version number. e.g. for the namespace part Version2_1 the string 2.1 would be returned</returns>
        internal static string ToVersionNumber(string namespacePart) {
            // we have a version, strip the prefix and convert version separators to ensure a valid namespace
            return namespacePart.Substring(VersionPrefix.Length).Replace("_", ".");
        }
    }
}
