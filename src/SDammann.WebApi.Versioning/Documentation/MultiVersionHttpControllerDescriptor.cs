namespace SDammann.WebApi.Versioning.Documentation {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Represents a <see cref="HttpControllerDescriptor"/> for multiple API versions. For backwards compat. this also represents the latest version of the API.
    /// </summary>
    public class MultiVersionHttpControllerDescriptor : HttpControllerDescriptor {
        private readonly SortedDictionary<ApiVersion, HttpControllerDescriptor> _descriptorsPerVersion;
        private readonly IReadOnlyDictionary<ApiVersion, HttpControllerDescriptor> _readOnlyDescriptorsPerVersion; 

        /// <summary>
        /// Gets <see cref="HttpControllerDescriptor"/> per API version
        /// </summary>
        public IReadOnlyDictionary<ApiVersion, HttpControllerDescriptor> DescriptorsPerVersion {
            [DebuggerStepThrough] get { return this._readOnlyDescriptorsPerVersion; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor"/> class.
        /// </summary>
        public MultiVersionHttpControllerDescriptor(HttpControllerDescriptor originalDescriptor) : base(originalDescriptor.Configuration, originalDescriptor.ControllerName, originalDescriptor.ControllerType) {
            this._descriptorsPerVersion = new SortedDictionary<ApiVersion, HttpControllerDescriptor>();
            this._readOnlyDescriptorsPerVersion = new ReadOnlyDictionary<ApiVersion, HttpControllerDescriptor>(this._descriptorsPerVersion);
        }

        /// <summary>
        /// Registers a pair of API version and a <see cref="HttpControllerDescriptor"/>
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <param name="controllerDescriptor"></param>
        internal void AddVersion(ApiVersion apiVersion, HttpControllerDescriptor controllerDescriptor) {
            if (apiVersion == null) {
                throw new ArgumentNullException("apiVersion");
            }
            if (controllerDescriptor == null) {
                throw new ArgumentNullException("controllerDescriptor");
            }

            try {
                this._descriptorsPerVersion.Add(apiVersion, controllerDescriptor);
            }
            catch (ArgumentException ex) {
                throw new ArgumentException(ExceptionResources.ApiVersionAlreadyAdded, "apiVersion", ex);
            }
        }
    }
}