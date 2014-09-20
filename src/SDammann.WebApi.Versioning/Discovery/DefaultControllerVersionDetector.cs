namespace SDammann.WebApi.Versioning.Discovery {
    /// <summary>
    /// Represents a <see cref="NamespaceControllerVersionDetector"/> that requires API version to be in a namespace part that is prefixed with 'Version'
    /// </summary>
    public sealed class DefaultControllerVersionDetector : NamespaceControllerVersionDetector {
        private const string DefaultVersionPrefix = "Version";

        /// <summary>
        /// Gets the versioning prefix in the namespace that will be used for determining the version
        /// </summary>
        protected override string VersionPrefix {
            get { return DefaultVersionPrefix; }
        }
    }
}