namespace SDammann.WebApi.Versioning.Internal {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Discovery;

    /// <summary>
    ///     Manages a cache of <see cref="System.Web.Http.Controllers.IHttpController" /> types detected in the system.
    /// </summary>
    internal sealed class HttpControllerTypeCache {
        private readonly Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>> _cache;
        private readonly HttpConfiguration _configuration;

        public HttpControllerTypeCache(HttpConfiguration configuration) {
            if (configuration == null) {
                throw new ArgumentException("configuration");
            }

            this._configuration = configuration;
            this._cache = new Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>>(this.InitializeCache);
        }

        internal Dictionary<ControllerIdentification, ILookup<string, Type>> Cache {
            get { return this._cache.Value; }
        }

        public ICollection<Type> GetControllerTypes(ControllerIdentification controllerId) {
            if (controllerId == null) {
                throw new ArgumentNullException("controllerId");
            }

            var matchingTypes = new HashSet<Type>();

            ILookup<string, Type> namespaceLookup;
            if (this._cache.Value.TryGetValue(controllerId, out namespaceLookup)) {
                foreach (var namespaceGroup in namespaceLookup) {
                    matchingTypes.UnionWith(namespaceGroup);
                }
            }

            return matchingTypes;
        }

        private Dictionary<ControllerIdentification, ILookup<string, Type>> InitializeCache() {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver =
                this._configuration.Services.GetHttpControllerTypeResolver();
            IControllerIdentificationDetector controllerIdentificationDetector =
                this._configuration.DependencyResolver.GetControllerIdentificationDetector();

            // group controllers by name
            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            IEnumerable<IGrouping<ControllerIdentification, Type>> groupedByName =
                controllerTypes.Select(
                    x => new {ClrType = x, Id = controllerIdentificationDetector.GetIdentification(x)})
                    .GroupBy(x => x.Id, x => x.ClrType);

            return groupedByName.ToDictionary(
                g => g.Key,
                g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                ControllerIdentification.Comparer);
        }
    }
}