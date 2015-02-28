namespace SDammann.WebApi.Versioning.Configuration {
    using System;
    using System.Web.Http;

    /// <summary>
    /// Helper class for configuring API versioning
    /// </summary>
    public static class ApiVersioning {
        private static ApiVersioningConfiguration _Config;
        internal static ApiVersioningConfiguration Configuration {
            get {
                if (_Config == null) {
                    throw new InvalidOperationException("API versioning has not been configured. Please configure API versioning using the ApiVersioning.Configure method");
                }

                return _Config;
            }
        }

        /// <summary>
        /// Configure API versioning
        /// </summary>
        /// <returns></returns>
        public static ApiVersioningConfiguration Configure(HttpConfiguration configuration) {
            return _Config ?? (_Config = new ApiVersioningConfiguration(configuration));
        }
    }
}