namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    /// Defines an interface for classes which implement detect of controller names
    /// </summary>
    public interface IControllerNameDetector {
        /// <summary>
        /// Gets an unqualified name for the specified controller type
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        string GetControllerName(Type controllerType);
    }
}