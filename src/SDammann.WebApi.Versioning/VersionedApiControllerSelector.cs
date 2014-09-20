namespace SDammann.WebApi.Versioning {
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    /// <summary>
    /// An <see cref="IHttpControllerSelector"/> implementation that offers API versioning
    /// </summary>
    public class VersionedApiControllerSelector : IHttpControllerSelector {
        /// <summary>
        /// Selects a <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> for the given <see cref="T:System.Net.Http.HttpRequestMessage"/>. 
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> instance.
        /// </returns>
        /// <param name="request">The request message.</param>
        public HttpControllerDescriptor SelectController(HttpRequestMessage request) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns a map, keyed by controller string, of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> that the selector can select.  This is primarily called by <see cref="T:System.Web.Http.Description.IApiExplorer"/> to discover all the possible controllers in the system. 
        /// </summary>
        /// <returns>
        /// A map of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> that the selector can select, or null if the selector does not have a well-defined mapping of <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/>.
        /// </returns>
        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping() {
            return null; // currently not implemented so return a sane default value
        }
    }
}