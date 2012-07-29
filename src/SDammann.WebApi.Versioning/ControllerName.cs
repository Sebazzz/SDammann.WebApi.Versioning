namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Generic;
    using System.Globalization;


    /// <summary>
    /// Represents a controller name with an associated version
    /// </summary>
    public struct ControllerName : IEquatable<ControllerName> {
        private static readonly Lazy<IEqualityComparer<ControllerName>> ComparerInstance = new Lazy<IEqualityComparer<ControllerName>>(() => new ControllerNameComparer());

        /// <summary>
        /// Gets an comparer for comparing <see cref="ControllerName"/> instances
        /// </summary>
        public static IEqualityComparer<ControllerName> Comparer {
            get { return ComparerInstance.Value; }
        }

        /// <summary>
        /// Gets or sets the name of the controller
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated version
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerName"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        public ControllerName(string name, int? version)
                : this() {
            this.Name = name;
            this.Version = version;
        }

        public bool Equals(ControllerName other) {
            return StringComparer.InvariantCultureIgnoreCase.Equals(other.Name, this.Name) &&
                   other.Version == this.Version;
        }

        public override bool Equals(object obj) {
            if (obj is ControllerName) {
                ControllerName cn = (ControllerName)obj;
                return this.Equals(cn);
            }

            return false;
        }

        public override int GetHashCode() {
            return this.ToString().ToUpperInvariant().GetHashCode();
        }

        public override string ToString() {
            if (this.Version == null) {
                return this.Name;
            }

            return VersionedControllerSelector.VersionPrefix + this.Version.Value.ToString(CultureInfo.InvariantCulture) + "." + this.Name;
        }

        private class ControllerNameComparer : IEqualityComparer<ControllerName> {
            public bool Equals(ControllerName x, ControllerName y) {
                return x.Equals(y);
            }

            public int GetHashCode(ControllerName obj) {
                return obj.GetHashCode();
            }
        }
    }
}