namespace SDammann.WebApi.Versioning.Tests.Request {
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Versioning.Request;

    [TestClass]
    public class RouteKeyRequestControllerNameDetectorTests {
        private const string RouteContextKey = "MS_HttpRouteData"; // taken from Web API source code

        [TestMethod]
        public void DefaultRouteKeyRequestControllerNameDetector_ReturnsControllerName_FromRequest() {
            // given
            const string controllerName = "CatClawler";

            IRequestControllerNameDetector nameDetector = new DefaultRequestControllerNameDetector();
            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Properties[RouteContextKey] = GetMockingRouteData(new Dictionary<string, object>() {
                                                                                                       {
                                                                                                           "controller",
                                                                                                           controllerName
                                                                                                       }
                                                                                                   });

            // when
            string detectedName = nameDetector.GetControllerName(msg);

            // then
            Assert.AreEqual(controllerName, detectedName, true);
        }

        private static IHttpRouteData GetMockingRouteData(Dictionary<string, object> routeData) {
            var stub = Substitute.For<IHttpRouteData>();
            stub.Values.Returns(routeData);
            return stub;
        }
    }
}