namespace SDammann.WebApi.Versioning.Discovery {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// Detects the API version of a <see cref="ApiController"/> based on the attributes the controller has
    /// </summary>
    public abstract class AttributeControllerVersionDetector : IControllerVersionDetector {
        /// <summary>
        /// Gets the API version associated with a controller type. Implementors return null if no version could be detected.
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public ApiVersion GetVersion(Type controllerType) {
            IEnumerable<Attribute> controllerAttributes = controllerType.GetCustomAttributes(false).OfType<Attribute>();

            return this.GetVersionFromAttributes(controllerAttributes);
        }

        /// <summary>
        /// Gets an API version from attributes
        /// </summary>
        /// <returns></returns>
        protected abstract ApiVersion GetVersionFromAttributes(IEnumerable<Attribute> controllerAttributes);
    }
}