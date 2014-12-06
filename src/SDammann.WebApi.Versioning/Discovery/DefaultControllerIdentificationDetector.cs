namespace SDammann.WebApi.Versioning.Discovery {
    using System;
    using System.Diagnostics;
    using System.Web.Http;
    using Configuration;
    using Internal;

    /// <summary>
    /// Default implementation of <see cref="IControllerIdentificationDetector"/> uses the <see cref="IControllerNameDetector"/> and <see cref="IControllerVersionDetector"/>
    /// as configured in <see cref="ApiVersioningConfiguration"/>
    /// </summary>
 
    public class DefaultControllerIdentificationDetector : IControllerIdentificationDetector {
        private readonly HttpConfiguration _configuration;

        private readonly Lazy<IControllerNameDetector> _controllerNameDetectorInstance;
        private readonly Lazy<IControllerVersionDetector> _controllerVersionDetectorInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DefaultControllerIdentificationDetector(HttpConfiguration configuration) {
            this._configuration = configuration;

            this._controllerNameDetectorInstance = new Lazy<IControllerNameDetector>(() => this._configuration.DependencyResolver.GetControllerNameDetector());
            this._controllerVersionDetectorInstance = new Lazy<IControllerVersionDetector>(() => this._configuration.DependencyResolver.GetControllerVersionDetector());
        }

        /// <summary>
        /// Gets an <see cref="ControllerIdentification"/> for the specified type. 
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType">.NET CLR type for controller</param>
        /// <returns></returns>
        public ControllerIdentification GetIdentification(Type controllerType) {
            string name = this.GetControllerName(controllerType);
            ApiVersion version = this.GetControllerVersion(controllerType);

            if (name != null) {
                return new ControllerIdentification(name, version);
            }

            return null;
        }

        /// <summary>
        /// Gets the API version of the controller using the default configured instance
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected ApiVersion GetControllerVersion(Type controllerType) {
            IControllerVersionDetector instance = this._controllerVersionDetectorInstance.Value;
            Debug.Assert(instance != null);

            return instance.GetVersion(controllerType);
        }

        /// <summary>
        /// Gets the API name of the controller using the default configured instance
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected string GetControllerName(Type controllerType) {
            IControllerNameDetector instance = this._controllerNameDetectorInstance.Value;
            Debug.Assert(instance != null);

            return instance.GetControllerName(controllerType);
        }
    }
}