namespace SDammann.WebApi.Versioning {
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents an unique identification for a controller 
    /// </summary>
    /// <remarks>
    /// This class is immutable and implements <see cref="GetHashCode"/>. Derived classes should follow the same rules.
    /// </remarks>
    [Serializable]
    public class ControllerIdentification : IEquatable<ControllerIdentification> {
        private readonly string _name;
        private readonly ApiVersion _version;

        /// <summary>
        /// Gets a short unqualified name for the controller
        /// </summary>
        public string Name {
            get { return this._name; }
        }

        /// <summary>
        /// Gets the API version associated with the controller
        /// </summary>
        public ApiVersion Version {
            get { return this._version; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ControllerIdentification(string name, ApiVersion version) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            if (version == null) {
                throw new ArgumentNullException("version");
            }

            this._name = name;
            this._version = version;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals(ControllerIdentification other) {
            bool isOtherEqual = other != null;
            isOtherEqual = isOtherEqual && String.Equals(other.Name, this.Name, StringComparison.OrdinalIgnoreCase);

            if (this._version != null) {
                isOtherEqual = isOtherEqual && this._version.Equals(other.Version);
            }

            return isOtherEqual;
        }


        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public sealed override int GetHashCode() {
            unchecked {
                int result = this._name.GetHashCode();
                    result = this._version.GetHashCode() >> 3 ^ result;

                return result;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public sealed override bool Equals(object obj) {
            ControllerIdentification other = obj as ControllerIdentification;
            if (other != null) {
                return this.Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() {
            StringBuilder stringRepBuilder = new StringBuilder(this._name);
            stringRepBuilder.Append(" ");

            if (!(this._version is UndefinedApiVersion)) {
                stringRepBuilder.Append("v");
                stringRepBuilder.Append(this._version);
            }else {
                stringRepBuilder.Append("(undefined version)");
            }
            
            return stringRepBuilder.ToString();
        }

        /// <summary>
        /// Gets an <see cref="IEqualityComparer{T}"/> for this class
        /// </summary>
        public static readonly IEqualityComparer<ControllerIdentification> Comparer = new ControllerIdentificationComparer();

        private sealed class ControllerIdentificationComparer : IEqualityComparer<ControllerIdentification> {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            public bool Equals(ControllerIdentification x, ControllerIdentification y) {
                if (x == null || y == null) {
                    return x == null && y == null;
                }

                return x.Equals(y);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            public int GetHashCode(ControllerIdentification obj) {
                if (obj == null) {
                    throw new ArgumentNullException("obj");
                }

                return obj.GetHashCode();
            }
        }
    }
}