namespace SDammann.WebApi.Versioning {
    using System;

    /// <summary>
    /// Represents an API version. This can be an integer, but also be a complex string or double.
    /// </summary>
    /// <remarks>
    /// Derived implementations are expected to be immmutable and implement 'GetHashCode' correctly
    /// </remarks>
    public abstract class ApiVersion : IEquatable<ApiVersion>, IComparable<ApiVersion> {
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public abstract bool Equals(ApiVersion other);

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(ApiVersion other) {
            if (other == null || (other is UndefinedApiVersion && !(this is UndefinedApiVersion))) {
                return 1;
            }

            if (this.CanCompareTo(other)) {
                return this.CompareToVersion(other);
            }

            return 0;
        }

        /// <summary>
        /// Returns a value if the <see cref="ApiVersion"/> can be compared to the current. The default version returns <c>true</c> if the types are exactly equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual bool CanCompareTo(ApiVersion other) {
            return other.GetType() == this.GetType();
        }

        /// <summary>
        /// Compares the current <see cref="ApiVersion"/> with another object of the same type. The default version does not allow sorting and returns zero.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other"></param>
        /// <returns></returns>
        protected virtual int CompareToVersion(ApiVersion other) {
            return 0;
        }

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