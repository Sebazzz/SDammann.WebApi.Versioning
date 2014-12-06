namespace SDammann.WebApi.Versioning.Documentation {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.ModelBinding.Binders;
    using System.Web.Http.Routing;
    using System.Web.Http.Services;

    /// <summary>
    /// Represents an <see cref="IApiExplorer"/> that supports API versioning
    /// </summary>
    public class VersionedApiExplorer : IApiExplorer {
        private readonly ApiExplorer _defaultApiExplorer;
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<Collection<ApiDescription>> _apiDescriptions;
        private const string ActionVariableName = "action";
        private const string ControllerVariableName = "controller";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public VersionedApiExplorer(HttpConfiguration configuration) {
            this._configuration = configuration;
            this._defaultApiExplorer = new ApiExplorer(this._configuration);
            this._apiDescriptions = new Lazy<Collection<ApiDescription>>(() => this._defaultApiExplorer.ApiDescriptions);
        }

        /// <summary>
        /// Gets the API descriptions. The descriptions are initialized on the first access. 
        /// </summary>
        public Collection<ApiDescription> ApiDescriptions {
            get { return this._apiDescriptions.Value; }
        }
    }

}