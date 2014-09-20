namespace SDammann.WebApi.Versioning.Request {
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Web.Http;
    using Configuration;
    using Internal;

    /// <summary>
    /// Default implementation of <see cref="IRequestControllerIdentificationDetector"/> uses the <see cref="IRequestControllerNameDetector"/> and <see cref="IRequestVersionDetector"/>
    /// as configured in <see cref="ApiVersioningConfiguration"/>
    /// </summary>

    public class DefaultRequestControllerIdentificationDetector : IRequestControllerIdentificationDetector
    {
        private readonly HttpConfiguration _configuration;

        private readonly Lazy<IRequestControllerNameDetector> _controllerNameDetectorInstance;
        private readonly Lazy<IRequestVersionDetector> _controllerVersionDetectorInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DefaultRequestControllerIdentificationDetector(HttpConfiguration configuration)
        {
            this._configuration = configuration;

            this._controllerNameDetectorInstance = new Lazy<IRequestControllerNameDetector>(() => this._configuration.Services.GetRequestControllerNameDetector());
            this._controllerVersionDetectorInstance = new Lazy<IRequestVersionDetector>(() => this._configuration.Services.GetRequestControllerVersionDetector());
        }

        /// <summary>
        /// Gets an <see cref="ControllerIdentification"/> for the specified type. 
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <returns></returns>
        public ControllerIdentification GetIdentification(HttpRequestMessage requestMessage)
        {
            string name = GetControllerName(requestMessage);
            ApiVersion version = GetControllerVersion(requestMessage);

            if (name != null)
            {
                return new ControllerIdentification(name, version);
            }

            return null;
        }

        /// <summary>
        /// Gets the API version of the controller using the default configured instance
        /// </summary>
        /// <returns></returns>
        protected ApiVersion GetControllerVersion(HttpRequestMessage requestMessage)
        {
            IRequestVersionDetector instance = this._controllerVersionDetectorInstance.Value;
            Debug.Assert(instance != null);

            return instance.GetVersion(requestMessage);
        }

        /// <summary>
        /// Gets the API name of the controller using the default configured instance
        /// </summary>
        /// <returns></returns>
        protected string GetControllerName(HttpRequestMessage requestMessage)
        {
            IRequestControllerNameDetector instance = this._controllerNameDetectorInstance.Value;
            Debug.Assert(instance != null);

            return instance.GetControllerName(requestMessage);
        }
    }
}