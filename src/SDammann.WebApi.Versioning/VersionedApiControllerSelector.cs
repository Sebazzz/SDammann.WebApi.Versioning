namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using Documentation;
    using ErrorHandling;
    using Internal;
    using Request;

    /// <summary>
    ///     An <see cref="IHttpControllerSelector" /> implementation that offers API versioning
    /// </summary>
    public class VersionedApiControllerSelector : IHttpControllerSelector {
        private readonly HttpConfiguration _configuration;

        private readonly Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly Lazy<IRequestControllerIdentificationDetector> _requestControllerIdentificationDetector;

        private readonly HttpControllerTypeCache _controllerTypeCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Web.Http.Dispatcher.DefaultHttpControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public VersionedApiControllerSelector(HttpConfiguration configuration) {
            if (configuration == null) {
                throw new ArgumentNullException("configuration");
            }

            this._controllerInfoCache =
                new Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>>(this.InitializeControllerInfoCache);
            this._requestControllerIdentificationDetector = new Lazy<IRequestControllerIdentificationDetector>(() => this._configuration.DependencyResolver.GetRequestControllerIdentificationDetector());
            this._configuration = configuration;
            this._controllerTypeCache = new HttpControllerTypeCache(this._configuration);
        }


        /// <summary>
        ///     Selects a <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> for the given
        ///     <see cref="T:System.Net.Http.HttpRequestMessage" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> instance.
        /// </returns>
        /// <param name="request">The request message.</param>
        public HttpControllerDescriptor SelectController(HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            return this.OnSelectController(request);
        }

        /// <summary>
        /// Select a controller. The <paramref name="request"/> parameter is guaranteed not to be <c>null</c>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual HttpControllerDescriptor OnSelectController(HttpRequestMessage request) {
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
                throw ApiControllerNotFoundException.Create(controllerName);
            }

            // multiple matching types
            throw AmbigiousApiRequestException.Create(controllerName, request.GetRouteData().Route, matchingTypes);
        }

        /// <summary>
        /// Gets the <see cref="ControllerIdentification"/> from the request. This method is guaranteed not to return <c>null</c>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request) {
            IRequestControllerIdentificationDetector controllerIdentificationDetector = this._requestControllerIdentificationDetector.Value;
            ControllerIdentification id;

            try {
                id = controllerIdentificationDetector.GetIdentification(request);
            } catch (ApiVersionFormatException ex) {
                Debug.WriteLine(ex);
                throw; // rethrow - handled in an exception filter
            }

            if (id == null) {
                throw ApiVersionNotDeterminedException.Create();
            }

            return id;
        }


        /// <summary>
        ///     Returns a map, keyed by controller string, of all
        ///     <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector can select.  This is
        ///     primarily called by <see cref="T:System.Web.Http.Description.IApiExplorer" /> to discover all the possible
        ///     controllers in the system.
        /// </summary>
        /// <returns>
        ///     A map of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector can select, or
        ///     null if the selector does not have a well-defined mapping of
        ///     <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" />.
        /// </returns>
        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping() {
            Dictionary<string, HttpControllerDescriptor> dict = new Dictionary<string, HttpControllerDescriptor>(this._controllerInfoCache.Value.Select(x => x.Key.Name).Distinct().Count());

            foreach (var controllersByName in this._controllerInfoCache.Value.GroupBy(x => x.Key.Name)) {
                MultiVersionHttpControllerDescriptor current = null;

                foreach (KeyValuePair<ControllerIdentification, HttpControllerDescriptor> controllerWithVersion in controllersByName.OrderByDescending(x => x.Key.Version)) {
                    if (current == null) {
                        current = new MultiVersionHttpControllerDescriptor(controllerWithVersion.Value);
                    }
                    
                    current.AddVersion(controllerWithVersion.Key.Version, controllerWithVersion.Value);
                }

                Debug.Assert(current != null, "This cannot be run as a grouping contains at least one controller...");
                dict[controllersByName.Key] = current;
            }

            return dict;
        }

        private ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor> InitializeControllerInfoCache() {
            // let's find and cache the found controllers
            var result = new ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>(ControllerIdentification.Comparer);
            var duplicateControllers = new HashSet<ControllerIdentification>();
            Dictionary<ControllerIdentification, ILookup<string, Type>> controllerTypeGroups =this._controllerTypeCache.Cache;

            foreach (var controllerTypeGroup in controllerTypeGroups) {
                ControllerIdentification controllerName = controllerTypeGroup.Key;

                foreach (var controllerTypesGroupedByNs in controllerTypeGroup.Value) {
                    foreach (Type controllerType in controllerTypesGroupedByNs) {
                        if (result.Keys.Contains(controllerName)) {
                            duplicateControllers.Add(controllerName);
                            break;
                        }
                        result.TryAdd(controllerName,
                            new HttpControllerDescriptor(this._configuration, controllerName.Name, controllerType));
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