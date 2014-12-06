namespace SDammann.WebApi.Versioning.Discovery {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Default implementation of attribute version detection using the <see cref="ApiVersionAttribute"/>
    /// </summary>
    public class DefaultAttributeControllerVersionDetector : AttributeControllerVersionDetector {
        /// <summary>
        /// Gets an API version from attributes
        /// </summary>
        /// <returns></returns>
        protected override ApiVersion GetVersionFromAttributes(IEnumerable<Attribute> controllerAttributes) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (Attribute attribute in controllerAttributes) {
                ApiVersionAttribute apiVersionAttribute = attribute as ApiVersionAttribute;
                if (apiVersionAttribute != null) {
                    SemVerApiVersion version = new SemVerApiVersion(apiVersionAttribute.Version);
                    return version;
                }
            }

            return UndefinedApiVersion.Instance;
        }
    }
}