namespace SDammann.WebApi.Versioning.Discovery {
    using System;

    /// <summary>
    /// Defines the interface for classes that can detect a unique controller identification for a controller
    /// </summary>
    public interface IControllerIdentificationDetector {
        /// <summary>
        /// Gets an <see cref="ControllerIdentification"/> for the specified type. 
        /// </summary>
        /// <remarks>
        /// Implementors should implement this as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType">.NET CLR type for controller</param>
        /// <returns></returns>
        ControllerIdentification GetIdentification(Type controllerType);
    }
}