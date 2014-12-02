namespace SDammann.WebApi.Versioning.Tests.Request {
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Versioning.Request;

    [TestClass]
    public class RouteKeyRequestControllerVersionDetectorTests {
        private const string RouteContextKey = "MS_HttpRouteData"; // taken from Web API source code

        [TestMethod]
        public void DefaultRouteKeyRequestControllerVersionDetector_ReturnsControllerVersion_FromSingleVersionRequest()
        {
            // given
            const string controllerVersion = "3";

            IRequestVersionDetector nameDetector = new DefaultRouteKeyVersionDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>() {
                                                                                                       {
                                                                                                           "version",
                                                                                                           controllerVersion
                                                                                                       }
                                                                                                   });

            // when
            SemVerApiVersion semVerApiVersion = nameDetector.GetVersion(msg) as SemVerApiVersion;

            // then
            Assert.IsNotNull(semVerApiVersion, "Expected version number to be detected");
            Assert.AreEqual(new Version(3, 0), semVerApiVersion.Version);
        }

        [TestMethod]
        public void DefaultRouteKeyRequestControllerVersionDetector_ReturnsControllerVersion_FromDoubleVersionRequest()
        {
            // given
            const string controllerVersion = "3.93";

            IRequestVersionDetector nameDetector = new DefaultRouteKeyVersionDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>() {
                                                                                                       {
                                                                                                           "version",
                                                                                                           controllerVersion
                                                                                                       }
                                                                                                   });

            // when
            SemVerApiVersion semVerApiVersion = nameDetector.GetVersion(msg) as SemVerApiVersion;

            // then
            Assert.IsNotNull(semVerApiVersion, "Expected version number to be detected");
            Assert.AreEqual(new Version(3, 93), semVerApiVersion.Version);
        }

        private static IHttpRouteData GetMockingRouteData(Dictionary<string, object> routeData) {
            var stub = Substitute.For<IHttpRouteData>();
            stub.Values.Returns(routeData);
            return stub;
        }
    }
}