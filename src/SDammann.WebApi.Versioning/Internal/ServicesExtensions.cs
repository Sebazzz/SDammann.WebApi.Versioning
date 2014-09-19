namespace SDammann.WebApi.Versioning.Internal {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Controllers;
    using Discovery;
    using Request;

    internal static class ServicesExtensions {
        public static IEnumerable<IControllerIdentificationDetector> GetControllerIdentificationDetectors(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServicesOrThrow<IControllerIdentificationDetector>();
        }

        public static IEnumerable<IRequestControllerNameDetector> GetRequestControllerNameDetectors(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServicesOrThrow<IRequestControllerNameDetector>();
        }

        public static IEnumerable<IRequestVersionDetector> GetRequestVersionDetectors(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServicesOrThrow<IRequestVersionDetector>();
        }

        private static TService GetServiceOrThrow<TService>(this ServicesContainer servicesContainer)where TService : class {
            Type clrType = typeof (TService);

            var service = servicesContainer.GetService(clrType) as TService;
            if (service == null) {
                throw new InvalidOperationException(String.Format(ExceptionResources.DependencyContainerNotConfigured,
                    clrType.FullName));
            }

            return service;
        }

        private static IEnumerable<TService> GetServicesOrThrow<TService>(this ServicesContainer servicesContainer)where TService : class {
            Type clrType = typeof (TService);

            List<TService> services = servicesContainer.GetServices(clrType).OfType<TService>().ToList();
            if (services.Count == 0) {
                throw new InvalidOperationException(String.Format(ExceptionResources.DependencyContainerNotConfigured,
                    clrType.FullName));
            }

            return services;
        }
    }
}