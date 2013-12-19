namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;


    /// <summary>
    ///   Represents an <see cref="IHttpControllerSelector" /> implementation that supports versioning and selects an controller based on versioning by convention (namespace.Api.Version1.xxxController). The controller to invoke is determined by the number in the "X-Api-Version" HTTP header.
    /// </summary>
    public sealed class VersionHeaderVersionedControllerSelector : VersionedControllerSelector {
        /// <summary>
        ///   Defines the name of the HTTP header that selects the API version
        /// </summary>
        public const string ApiVersionHeaderName = "X-Api-Version";

        /// <summary>
        ///   Initializes a new instance of the <see cref="VersionHeaderVersionedControllerSelector" /> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public VersionHeaderVersionedControllerSelector (HttpConfiguration configuration) : base(configuration) {
        }

        protected override ControllerIdentification GetControllerIdentificationFromRequest (HttpRequestMessage request) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            // get the version number from the HTTP header
            IEnumerable<string> values;
            string apiVersion = null;
            if (request.Headers.TryGetValues(ApiVersionHeaderName, out values)) {
                foreach (string value in values) {
                    if (!String.IsNullOrWhiteSpace(value)) {
                        apiVersion = value;
                        break;
                    }
                }
            }

            string controllerName = this.GetControllerNameFromRequest(request);

            return new ControllerIdentification(controllerName, apiVersion);
        }
    }
}