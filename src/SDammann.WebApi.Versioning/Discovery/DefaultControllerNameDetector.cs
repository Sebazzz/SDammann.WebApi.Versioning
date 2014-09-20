namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    /// Represents a Web API versioning implementation for controller type names, which requires controllers to have the suffix 'Controller'
    /// </summary>
    public sealed class DefaultControllerNameDetector : TypeNameControllerNameDetector {
        private const string DefaultSuffix = "Controller";

        /// <summary>
        /// Gets the required suffix for a controller <see cref="Type"/>s name
        /// </summary>
        protected override string ControllerSuffix {
            get { return DefaultSuffix; }
        }
    }
}