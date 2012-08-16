namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;


    /// <summary>
    ///   Manages a cache of <see cref="System.Web.Http.Controllers.IHttpController" /> types detected in the system.
    /// </summary>
    internal sealed class HttpControllerTypeCache {
        private readonly Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>> _cache;
        private readonly HttpConfiguration _configuration;

        internal Dictionary<ControllerIdentification, ILookup<string, Type>> Cache {
            get { return this._cache.Value; }
        }

        public HttpControllerTypeCache(HttpConfiguration configuration) {
            if (configuration == null) {
                throw new ArgumentNullException("configuration");
            }

            this._configuration = configuration;
            this._cache = new Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>>(this.InitializeCache);
        }

        public ICollection<Type> GetControllerTypes(ControllerIdentification controllerName) {
            if (String.IsNullOrEmpty(controllerName.Name)) {
                throw new ArgumentNullException("controllerName");
            }

            var matchingTypes = new HashSet<Type>();

            ILookup<string, Type> namespaceLookup;
            if (this._cache.Value.TryGetValue(controllerName, out namespaceLookup)) {
                foreach (var namespaceGroup in namespaceLookup) {
                    matchingTypes.UnionWith(namespaceGroup);
                }
            }

            return matchingTypes;
        }

        private Dictionary<ControllerIdentification, ILookup<string, Type>> InitializeCache() {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = this._configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            IEnumerable<IGrouping<ControllerIdentification, Type>> groupedByName = controllerTypes.GroupBy(
                                                                                                 this.GetControllerName,
                                                                                                 ControllerIdentification.Comparer);

            return groupedByName.ToDictionary(
                                              g => g.Key,
                                              g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
                                              ControllerIdentification.Comparer);
        }

        private ControllerIdentification GetControllerName(Type type) {
            string fullName = type.FullName;
            Debug.Assert(fullName != null);

            fullName = fullName.Substring(0, fullName.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

            // split by dot and find version
            string[] nameSplit = fullName.Split('.');

            string name = nameSplit[nameSplit.Length - 1]; // same as Type.Name
            int? version = null;

            for (int i = nameSplit.Length - 2; i >= 0; i--) {
                string namePart = nameSplit[i];
                if (!namePart.StartsWith(VersionedControllerSelector.VersionPrefix, StringComparison.InvariantCultureIgnoreCase)) {
                    continue;
                }

                string versionNumberStr = namePart.Substring(VersionedControllerSelector.VersionPrefix.Length);
                int versionNumber;
                if (Int32.TryParse(versionNumberStr, out versionNumber)) {
                    // OK :D we have a version
                    version = versionNumber;
                    break;
                }
            }

            return new ControllerIdentification(name, version);
        }
    }
}