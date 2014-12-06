namespace SDammann.WebApi.Versioning {
    using System;

    /// <summary>
    /// Represents an <see cref="ApiVersion"/> that is not defined (no api version)
    /// </summary>
    public sealed class UndefinedApiVersion : ApiVersion {
        private UndefinedApiVersion() {}

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public override bool Equals(ApiVersion other) {
            return ReferenceEquals(other, this);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() {
            return String.Empty;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            return 0;
        }

        /// <summary>
        /// Gets the instance of <see cref="UndefinedApiVersion"/>
        /// </summary>
        public static readonly ApiVersion Instance = new UndefinedApiVersion();
    }
}