namespace SDammann.WebApi.Versioning.Discovery {
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Represents an <see cref="IControllerVersionDetector"/> implementation that detects API version by namespace. See remarks.
    /// </summary>
    /// <remarks>
    /// For example. MyCompany.WebApi.Version1 controllers will yield version '1.0.0.0', same applies for Version1_0_0_0.
    /// 
    /// </remarks>
    public abstract class NamespaceControllerVersionDetector : IControllerVersionDetector {
        /// <summary>
        /// Gets the versioning prefix in the namespace that will be used for determining the version
        /// </summary>
        protected abstract string VersionPrefix { get; }

        /// <summary>
        /// Gets the API version associated with a controller type. Implementors return null if no version could be detected.
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public ApiVersion GetVersion(Type controllerType) {
            string namespaceName = controllerType.Namespace;

            // the namespace may be null or empty, as a namespace is not required in the CLR
            if (String.IsNullOrEmpty(namespaceName)) {
                return UndefinedApiVersion.Instance;
            }

            return this.GetVersionForNamespace(controllerType, namespaceName.Split('.'));
        }

        /// <summary>
        /// Gets the API version for the specified namespace name
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="namespaceParts"></param>
        /// <returns></returns>
        protected virtual ApiVersion GetVersionForNamespace(Type controllerType, string[] namespaceParts) {
            string prefix = this.VersionPrefix;

            // ReSharper disable once LoopCanBeConvertedToQuery -- Don't linqify this, this yields unneeded allocations and thus GCs
            foreach (string namespacePart in this.GetPossibleVersionNamespaceParts(namespaceParts, prefix)) {
                ApiVersion apiVersion = this.GetApiVersion(namespacePart);

                if (apiVersion != null) {
                    return apiVersion;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the api version from the specified namespace part (without <see cref="VersionPrefix"/>)
        /// </summary>
        /// <param name="namespacePartWithoutPrefix"></param>
        /// <returns></returns>
        protected virtual ApiVersion GetApiVersion(string namespacePartWithoutPrefix) {
            string apiVersionAsParsable = namespacePartWithoutPrefix.Replace('_', '.');

            // System.Version does not parse a single integer, so we should do it ourselves
            Version version = null;
            if (apiVersionAsParsable.IndexOf('.') == -1) {
                int singleVersionNumber;
                if (Int32.TryParse(apiVersionAsParsable, NumberStyles.None, CultureInfo.InvariantCulture, out singleVersionNumber)) {
                        version = new Version(singleVersionNumber, 0);
                }
            }

            // parse via default path
            if (version == null && !Version.TryParse(apiVersionAsParsable, out version)) {
                return UndefinedApiVersion.Instance;
            }

            return new SemVerApiVersion(version);
        }

        /// <summary>
        /// Gets version namespace parts which /may/ be a version indicator
        /// </summary>
        /// <param name="namespaceParts"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected virtual List<String> GetPossibleVersionNamespaceParts(string[] namespaceParts, string prefix) {
            List<string> parts = new List<string>(1 /* we assume only one element will fill the list*/);
            
            // try detecting version number from the part closest to the end of the namespace
            for (int index = namespaceParts.Length-1; index >= 0; index--) {
                string part = namespaceParts[index];
                if (part.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && part.Length > prefix.Length) {
                    parts.Add(part.Substring(prefix.Length));
                }
            }

            return parts;
        }
    }
}