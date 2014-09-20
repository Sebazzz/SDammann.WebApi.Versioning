namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    ///     Defines an interface for classes which can detect a <see cref="ApiVersion" /> associated with a controller
    /// </summary>
    public interface IControllerVersionDetector {
        /// <summary>
        /// Gets the API version associated with a controller type. Implementors return null if no version could be detected.
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        ApiVersion GetVersion(Type controllerType);
    }
}