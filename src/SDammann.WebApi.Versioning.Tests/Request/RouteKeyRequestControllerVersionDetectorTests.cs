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

        [TestMethod]
        public void DefaultRouteKeyRequestControllerVersionDetector_ReturnsNullVersion_FromNullVersionRequest()
        {
            // given
            const string controllerVersion = "3.93";

            IRequestVersionDetector nameDetector = new DefaultRouteKeyVersionDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>() {
                                                                                                       {
                                                                                                           "version",
                                                                                                           null
                                                                                                       }
                                                                                                   });

            // when
            ApiVersion semVerApiVersion = nameDetector.GetVersion(msg);

            // then
            Assert.IsNull(semVerApiVersion, "Expected version number to be null since it is null in the route also");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Expected an InvalidOperationException to be thrown")]
        public void DefaultRouteKeyRequestControllerVersionDetector_ThrowsException_WhenNoVersionInApiRoute()
        {
            // given
            IRequestVersionDetector nameDetector = new DefaultRouteKeyVersionDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>());

            // when
            nameDetector.GetVersion(msg);

            // then
            Assert.Inconclusive();
        }

        [TestMethod]
        [ExpectedException(typeof(ApiVersionFormatException), "Expected an FormatException to be thrown")]
        public void DefaultRouteKeyRequestControllerVersionDetector_ThrowsException_WhenApiVersionUnparsable()
        {
            // given
            const string controllerVersion = "3.unparsable";

            IRequestVersionDetector nameDetector = new DefaultRouteKeyVersionDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>(){
                                                                                                       {
                                                                                                           "version",
                                                                                                           controllerVersion
                                                                                                       }
                                                                                                   });

            // when
            nameDetector.GetVersion(msg);

            // then
            Assert.Inconclusive();
        }

        private static IHttpRouteData GetMockingRouteData(Dictionary<string, object> routeData) {
            var stub = Substitute.For<IHttpRouteData>();
            stub.Values.Returns(routeData);
            return stub;
        }
    }
}