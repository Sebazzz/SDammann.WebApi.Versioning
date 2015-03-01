namespace SDammann.WebApi.Versioning.Configuration {
    using System;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.ExceptionHandling;
    using Discovery;
    using ErrorHandling;
    using Request;

    /// <summary>
    /// Configuration for API versioning
    /// </summary>
    public sealed class ApiVersioningConfiguration {
        internal Type RequestVersionDetector;
        internal Type RequestControllerNameDetectorType = typeof(DefaultRequestControllerNameDetector);
        internal Type RequestControllerIdentificationDetectorType = typeof(DefaultRequestControllerIdentificationDetector);
        internal Type ControllerNameDetectorType = typeof(DefaultControllerNameDetector);
        internal Type ControllerVersionDetectorType = typeof(DefaultControllerVersionDetector);
        internal Type ControllerIdentificationDetectorType = typeof(DefaultControllerIdentificationDetector);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        internal ApiVersioningConfiguration(HttpConfiguration configuration) {
            ConfigureDefaults(configuration);
        }

        private static void ConfigureDefaults(HttpConfiguration configuration) {
            configuration.Services.Replace(typeof(IHttpControllerSelector), new VersionedApiControllerSelector(configuration));
            configuration.Services.Replace(typeof(IExceptionHandler), new ApiVersioningExceptionHandler());
        }

        /// <summary>
        /// Configures an type for version detecting on the API request
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestVersionDetector<TVersionDetector>() where TVersionDetector : IRequestVersionDetector {
            return this.ConfigureRequestVersionDetector(typeof (TVersionDetector));
        }

        /// <summary>
        /// Configures an type for version detecting on the API request
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestVersionDetector(Type type) {
            this.RequestVersionDetector = type;
            return this;
        }

        /// <summary>
        /// Configures an type for version detecting on the API request
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestControllerNameDetector<TControllerNameDetector>() where TControllerNameDetector : IRequestControllerNameDetector {
            return this.ConfigureRequestControllerNameDetector(typeof(TControllerNameDetector));
        }

        /// <summary>
        /// Configures an type for version detecting on the API request
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestControllerNameDetector(Type type) {
            this.RequestControllerNameDetectorType = type;
            return this;
        }

        /// <summary>
        /// Configures an type for detecting the name of a controller
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerNameDetector<TControllerNameDetector>() where TControllerNameDetector : IControllerNameDetector {
            return this.ConfigureControllerNameDetector(typeof(TControllerNameDetector));
        }

        /// <summary>
        /// Configures an type for detecting the name of a controller
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerNameDetector(Type type) {
            this.ControllerNameDetectorType = type;
            return this;
        }

        /// <summary>
        /// Configures an type for detecting the complete identification (name plus version) of a controller. Overriding this
        /// default behavior is usually not needed as detection of version and name is seperated in two other services.
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerIdentificationDetector<TControllerIdentificationDetector>() where TControllerIdentificationDetector : IControllerIdentificationDetector
        {
            return this.ConfigureControllerIdentificationDetector(typeof(TControllerIdentificationDetector));
        }

        /// <summary>
        /// Configures an type for detecting the complete identification (name plus version) of a controller. Overriding this
        /// default behavior is usually not needed as detection of version and name is seperated in two other services.
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerIdentificationDetector(Type type)
        {
            this.ControllerIdentificationDetectorType = type;
            return this;
        }

        /// <summary>
        /// Configures an type for detecting the complete identification (name plus version) of a controller. Overriding this
        /// default behavior is usually not needed as detection of version and name is seperated in two other services.
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestControllerIdentificationDetector<TControllerIdentificationDetector>() where TControllerIdentificationDetector : IRequestControllerIdentificationDetector
        {
            return this.ConfigureRequestControllerIdentificationDetector(typeof(TControllerIdentificationDetector));
        }

        /// <summary>
        /// Configures an type for detecting the complete identification (name plus version) of a controller. Overriding this
        /// default behavior is usually not needed as detection of version and name is seperated in two other services.
        /// </summary>
        public ApiVersioningConfiguration ConfigureRequestControllerIdentificationDetector(Type type)
        {
            this.RequestControllerIdentificationDetectorType = type;
            return this;
        }


        /// <summary>
        /// Configures an type for detecting the version of a controller
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerVersionDetector<TControllerVersionDetector>() where TControllerVersionDetector : IControllerVersionDetector {
            return this.ConfigureControllerNameDetector(typeof(TControllerVersionDetector));
        }

        /// <summary>
        /// Configures an type for detecting the version of a controller
        /// </summary>
        public ApiVersioningConfiguration ConfigureControllerVersionDetector(Type type) {
            this.ControllerVersionDetectorType = type;
            return this;
        }
    }
}