namespace SDammann.WebApi.Versioning.Tests.Integration {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Dispatcher;
    using Microsoft.Owin.Hosting;
    using Owin;
    using TinyIoC;
    using Versioning.Discovery;
    using Versioning.Request;

    internal static class IntegrationTestManager {
        public const string BaseAddress = "http://localhost:8943/";

        private static IDisposable _HostingInstance;
        internal static Action<HttpConfiguration> ConfigurationCallback;
        public static TinyIoCContainer DependencyContainer;


        public static HttpClient GetClient() {
            if (_HostingInstance == null) {
                throw new InvalidOperationException("Hosting has not started up yet");
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress, UriKind.Absolute);

            return client;
        }

        public static void Startup(Action<HttpConfiguration> configurationCallback) {
            Shutdown();

            DependencyContainer = new TinyIoCContainer();

            ConfigurationCallback = configurationCallback;
            _HostingInstance = WebApp.Start<WebApiConfig>(BaseAddress);
        }

        public static void Shutdown() {
            if (_HostingInstance != null) {
                _HostingInstance.Dispose();
                _HostingInstance = null;
            }

            if (DependencyContainer != null) {
                DependencyContainer.Dispose();
            }
        }


        public class WebApiConfig {
            // This code configures Web API. The Startup class is specified as a type
            // parameter in the WebApp.Start method.
            public void Configuration(IAppBuilder appBuilder)
            {
                // Configure Web API for self-host. 
                HttpConfiguration config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/v{version}/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.Services.Replace(typeof(IHttpControllerSelector), new VersionedApiControllerSelector(config));
                config.DependencyResolver = new DependencyResolver();

                if (ConfigurationCallback != null) {
                    ConfigurationCallback.Invoke(config);
                }

                DependencyContainer.Register((c, np) => new DefaultControllerIdentificationDetector(config));
                DependencyContainer.Register((c, np) => new DefaultRequestControllerIdentificationDetector(config));

                appBuilder.UseWebApi(config);
            }

            private sealed class DependencyResolver : IDependencyResolver {
                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose() {
                    // no-op
                }

                /// <summary>
                /// Retrieves a service from the scope.
                /// </summary>
                /// <returns>
                /// The retrieved service.
                /// </returns>
                /// <param name="serviceType">The service to be retrieved.</param>
                public object GetService(Type serviceType) {
                    try {
                        return DependencyContainer.Resolve(serviceType);
                    }
                    catch (TinyIoCResolutionException ex) {
                        Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                        return null;
                    }
                }

                /// <summary>
                /// Retrieves a collection of services from the scope.
                /// </summary>
                /// <returns>
                /// The retrieved collection of services.
                /// </returns>
                /// <param name="serviceType">The collection of services to be retrieved.</param>
                public IEnumerable<object> GetServices(Type serviceType) {
                    try {
                        return DependencyContainer.ResolveAll(serviceType);
                    }
                    catch (TinyIoCResolutionException ex) {
                        Debug.WriteLine("Exception in resolving {0}: {1}", serviceType, ex.Message);
                        return null;
                    }
                }

                /// <summary>
                /// Starts a resolution scope. 
                /// </summary>
                /// <returns>
                /// The dependency scope.
                /// </returns>
                public IDependencyScope BeginScope() {
                    // no -op
                    return new DependencyResolver();
                }
            }
        } 
    }
}