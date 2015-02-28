namespace SDammann.WebApi.Versioning.TestApi {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using Areas.HelpPage;
    using Configuration;
    using Discovery;
    using Documentation;
    using Request;
    using TinyIoC;

    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            config.SetDocumentationProvider(
                new XmlDocumentationProvider(
                    HostingEnvironment.MapPath("~/bin/" + typeof (WebApiConfig).Assembly.GetName().Name + ".xml")));

            var dependencyContainer = new TinyIoCContainer();

            // API versioning
            config.Services.Replace(typeof (IHttpControllerSelector), new VersionedApiControllerSelector(config));
            config.Services.Replace(typeof(IApiExplorer), new VersionedApiExplorer(config));
            config.DependencyResolver = new DependencyResolver(dependencyContainer);

            dependencyContainer.Register((c, np) => new DefaultControllerIdentificationDetector(config));
            dependencyContainer.Register((c, np) => new DefaultRequestControllerIdentificationDetector(config));

            ApiVersioning.Configure(config)
                         .ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/v{version}/{controller}/{id}", new {id = RouteParameter.Optional});
        }


        private sealed class DependencyResolver : IDependencyResolver {
            private readonly TinyIoCContainer _dependencyContainer;

            public DependencyResolver(TinyIoCContainer dependencyContainer) {
                this._dependencyContainer = dependencyContainer;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() {
                // no-op
            }

            /// <summary>
            ///     Retrieves a service from the scope.
            /// </summary>
            /// <returns>
            ///     The retrieved service.
            /// </returns>
            /// <param name="serviceType">The service to be retrieved.</param>
            public object GetService(Type serviceType) {
                try {
                    return this._dependencyContainer.Resolve(serviceType);
                }
                catch (TinyIoCResolutionException ex) {
                    Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                    return null;
                }
            }

            /// <summary>
            ///     Retrieves a collection of services from the scope.
            /// </summary>
            /// <returns>
            ///     The retrieved collection of services.
            /// </returns>
            /// <param name="serviceType">The collection of services to be retrieved.</param>
            public IEnumerable<object> GetServices(Type serviceType) {
                try {
                    return this._dependencyContainer.ResolveAll(serviceType);
                }
                catch (TinyIoCResolutionException ex) {
                    Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                    return null;
                }
            }

            /// <summary>
            ///     Starts a resolution scope.
            /// </summary>
            /// <returns>
            ///     The dependency scope.
            /// </returns>
            public IDependencyScope BeginScope() {
                // no -op
                return new DependencyResolver(this._dependencyContainer);
            }
        }
    }
}