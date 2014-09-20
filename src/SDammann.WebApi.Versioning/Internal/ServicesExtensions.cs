namespace SDammann.WebApi.Versioning.Internal {
    using System;
    using System.Web.Http.Controllers;
    using Configuration;
    using Discovery;
    using Request;

    internal static class ServicesExtensions {
        public static IControllerIdentificationDetector GetControllerIdentificationDetector(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServiceOrThrow<IControllerIdentificationDetector>(ApiVersioning.Configuration.ControllerIdentificationDetectorType);
        }

        public static IControllerNameDetector GetControllerNameDetector(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServiceOrThrow<IControllerNameDetector>(ApiVersioning.Configuration.ControllerNameDetectorType);
        }

        public static IControllerVersionDetector GetControllerVersionDetector(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServiceOrThrow<IControllerVersionDetector>(ApiVersioning.Configuration.ControllerVersionDetectorType);
        }

        public static IRequestControllerNameDetector GetRequestControllerNameDetector(this ServicesContainer servicesContainer) {
            return servicesContainer.GetServiceOrThrow<IRequestControllerNameDetector>(ApiVersioning.Configuration.RequestControllerNameDetectorType);
        }

        public static IRequestVersionDetector GetRequestControllerVersionDetector(this ServicesContainer servicesContainer){
            return servicesContainer.GetServiceOrThrow<IRequestVersionDetector>(ApiVersioning.Configuration.RequestVersionDetector);
        }

        private static TService GetServiceOrThrow<TService>(this ServicesContainer servicesContainer, Type clrType) where TService : class {
            clrType = clrType ?? typeof (TService);

            var service = servicesContainer.GetService(clrType) as TService;
            if (service == null) {
                throw new InvalidOperationException(String.Format(ExceptionResources.DependencyContainerNotConfigured, clrType.FullName));
            }

            return service;
        }
    }
}