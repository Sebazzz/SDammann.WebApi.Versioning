namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    /// Represents an implementation of <see cref="IControllerNameDetector"/> which uses the type name with a specified suffix 
    /// </summary>
    public abstract class TypeNameControllerNameDetector : IControllerNameDetector {
        /// <summary>
        /// Gets the required suffix for a controller <see cref="Type"/>s name
        /// </summary>
        protected abstract string ControllerSuffix { get; }

        /// <summary>
        /// Gets an unqualified name for the specified controller type
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public string GetControllerName(Type controllerType) {
            string suffix = this.ControllerSuffix;
            
            string typeName = controllerType.Name;
            int suffixIndex = typeName.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);

            // we assert that the suffix index is not the type name itself, so for example just 'Controller' won't match
            if (suffixIndex < 1) {
                return null; // unknown, we cannot match
            }

            return typeName.Substring(0, suffixIndex);
        }
    }
}