namespace SDammann.WebApi.Versioning {
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents an API version using semantic versioning (x.x.x.x)
    /// </summary>
    public class SemVerApiVersion : ApiVersion {
        private readonly Version _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SemVerApiVersion(Version version) {
            if (version == null) {
                throw new ArgumentNullException("version");
            }

            this._version = version;
        }

        /// <summary>
        /// Gets the four digit (or less) version
        /// </summary>
        public Version Version {
            [DebuggerStepThrough] get { return this._version; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public override bool Equals(ApiVersion other) {
            return other is SemVerApiVersion && ((SemVerApiVersion) other)._version == this._version;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() {
            return this._version.ToString();
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            return this._version.GetHashCode();
        }

        /// <summary>
        /// Compares the current <see cref="ApiVersion"/> with another object of the same type. The default version does not allow sorting and returns zero.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override int CompareToVersion(ApiVersion other) {
            return this.Version.CompareTo(((SemVerApiVersion) other).Version);
        }
    }
}