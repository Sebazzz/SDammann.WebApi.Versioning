namespace SDammann.WebApi.Versioning.Documentation {
    using System;
    using System.Collections.ObjectModel;
    using System.Web.Http;
    using System.Web.Http.Description;

    /// <summary>
    /// Represents an <see cref="IApiExplorer"/> that supports API versioning
    /// </summary>
    public class VersionedApiExplorer : IApiExplorer {
        private readonly ApiExplorer _defaultApiExplorer;
        private readonly HttpConfiguration _configuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public VersionedApiExplorer(HttpConfiguration configuration) {
            this._configuration = configuration;
            this._defaultApiExplorer = new ApiExplorer(this._configuration);
        }

        /// <summary>
        /// Gets the API descriptions. The descriptions are initialized on the first access. 
        /// </summary>
        public Collection<ApiDescription> ApiDescriptions {
            get {
                return this._defaultApiExplorer.ApiDescriptions;
            }
        }
    }
}