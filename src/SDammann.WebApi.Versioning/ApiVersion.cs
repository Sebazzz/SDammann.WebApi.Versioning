namespace SDammann.WebApi.Versioning {
    using System;

    /// <summary>
    /// Represents an API version. This can be an integer, but also be a complex string or double.
    /// </summary>
    /// <remarks>
    /// Derived implementations are expected to be immmutable and implement 'GetHashCode' correctly
    /// </remarks>
    public abstract class ApiVersion : IEquatable<ApiVersion> {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public abstract bool Equals(ApiVersion other);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public abstract override string ToString();

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public abstract override int GetHashCode();

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public sealed override bool Equals(object obj) {
            ApiVersion other = obj as ApiVersion;
            if (other != null)
            {
                return this.Equals(other);
            }

            return false;
        }
    }
}