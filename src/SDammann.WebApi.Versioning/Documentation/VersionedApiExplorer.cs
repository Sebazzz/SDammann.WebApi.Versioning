namespace SDammann.WebApi.Versioning.Documentation {
    using System;
    using System.Collections.ObjectModel;
    using System.Web.Http;
    using System.Web.Http.Description;

    /// <summary>
    ///     Represents an <see cref="IApiExplorer" /> that supports API versioning
    /// </summary>
    public class VersionedApiExplorer : IApiExplorer {
        private const string ActionVariableName = "action";
        private const string ControllerVariableName = "controller";
        private readonly Lazy<Collection<ApiDescription>> _apiDescriptions;
        private readonly HttpConfiguration _configuration;
        private readonly ApiExplorer _defaultApiExplorer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public VersionedApiExplorer(HttpConfiguration configuration) {
            this._configuration = configuration;
            this._defaultApiExplorer = new ApiExplorer(this._configuration);
            this._apiDescriptions = new Lazy<Collection<ApiDescription>>(() => this._defaultApiExplorer.ApiDescriptions);
        }

        /// <summary>
        ///     Gets the API descriptions. The descriptions are initialized on the first access.
        /// </summary>
        public Collection<ApiDescription> ApiDescriptions {
            get { return this._apiDescriptions.Value; }
        }
    }
}