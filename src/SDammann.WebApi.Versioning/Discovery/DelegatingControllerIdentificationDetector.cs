namespace SDammann.WebApi.Versioning.Discovery {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal class DelegatingControllerIdentificationDetector : IControllerIdentificationDetector {
        private readonly IEnumerable<IControllerIdentificationDetector> _detectors;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DelegatingControllerIdentificationDetector(IEnumerable<IControllerIdentificationDetector> detectors) {
            this._detectors = detectors;
        }

        /// <summary>
        /// Gets an <see cref="ControllerIdentification"/> for the specified type. 
        /// </summary>
        /// <remarks>
        /// Implementors should implement this  as a high-performance method (at least on the negative path) 
        /// because it will be called for all types  in the referenced assemblies during the application initialization phase.
        /// </remarks>
        /// <param name="controllerType">.NET CLR type for controller</param>
        /// <returns></returns>
        public ControllerIdentification GetIdentification(Type controllerType) {
            foreach (IControllerIdentificationDetector controllerIdentificationDetector in this._detectors) {
                var result = controllerIdentificationDetector.GetIdentification(controllerType);

                if (result != null) {
                    Debug.Assert(!String.IsNullOrEmpty(result.Name));

                    return result;
                }
            }

            throw ControllerCouldNotBeIdentified(controllerType);
        }

        private static InvalidOperationException ControllerCouldNotBeIdentified(Type controllerType) {
            return new InvalidOperationException(String.Format(ExceptionResources.ControllerCouldNotBeIdentified, controllerType));
        }
    }
}