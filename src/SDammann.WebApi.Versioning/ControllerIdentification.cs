namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Generic;
    using System.Globalization;


    /// <summary>
    /// Represents a controller name with an associated version
    /// </summary>
    public struct ControllerIdentification : IEquatable<ControllerIdentification> {
        private static readonly Lazy<IEqualityComparer<ControllerIdentification>> ComparerInstance = new Lazy<IEqualityComparer<ControllerIdentification>>(() => new ControllerNameComparer());

        /// <summary>
        /// Gets an comparer for comparing <see cref="ControllerIdentification"/> instances
        /// </summary>
        public static IEqualityComparer<ControllerIdentification> Comparer {
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
        public string Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerIdentification"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        public ControllerIdentification(string name, string version)
                : this() {
            this.Name = name;
            this.Version = version;
        }

        public bool Equals(ControllerIdentification other) {
            return StringComparer.InvariantCultureIgnoreCase.Equals(other.Name, this.Name) &&
                   other.Version == this.Version;
        }

        public override bool Equals(object obj) {
            if (obj is ControllerIdentification) {
                ControllerIdentification cn = (ControllerIdentification)obj;
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

            return string.Format("{0}{1}.{2}", VersionedControllerSelector.VersionPrefix, this.Version.Replace(".", "_"), this.Name);
        }

        private class ControllerNameComparer : IEqualityComparer<ControllerIdentification> {
            public bool Equals(ControllerIdentification x, ControllerIdentification y) {
                return x.Equals(y);
            }

            public int GetHashCode(ControllerIdentification obj) {
                return obj.GetHashCode();
            }
        }
    }
}