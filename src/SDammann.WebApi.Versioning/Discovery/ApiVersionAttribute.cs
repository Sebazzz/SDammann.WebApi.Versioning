namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    /// This attribute can be used to decorate controllers to specify and API version
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class ApiVersionAttribute : Attribute {
        private readonly int _majorVersion;
        private readonly int _minorVersion;
        private readonly int _buildVersion = -1;
        private readonly int _revisionVersion = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ApiVersionAttribute(int majorVersion, int minorVersion) {
            this._majorVersion = majorVersion;
            this._minorVersion = minorVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ApiVersionAttribute(int majorVersion, int minorVersion, int buildVersion) {
            this._majorVersion = majorVersion;
            this._minorVersion = minorVersion;
            this._buildVersion = buildVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ApiVersionAttribute(int majorVersion, int minorVersion, int buildVersion, int revisionVersion) {
            this._majorVersion = majorVersion;
            this._minorVersion = minorVersion;
            this._buildVersion = buildVersion;
            this._revisionVersion = revisionVersion;
        }

        /// <summary>
        /// Gets a <see cref="Version"/> instance based on the version information contained in this attribute
        /// </summary>
        public Version Version {
            get {
                return new Version(this._majorVersion, this._minorVersion, this._buildVersion, this._revisionVersion);
            }
        }
        
    }
}