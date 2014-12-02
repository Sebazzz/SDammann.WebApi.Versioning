namespace SDammann.WebApi.Versioning.Tests.Integration {
    using System.Net.Http;
    using Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Versioning.Request;

    [TestClass]
    public class DefaultSettingsIntegrationTest {
        [ClassInitialize]
        public static void Setup(TestContext ctx) {
            IntegrationTestManager.Startup();

            ApiVersioning.Configure()
                         .ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();
        }

        [ClassCleanup]
        public static void Shutdown() {
            IntegrationTestManager.Shutdown();
        }

        [TestMethod]
        public void HelloApiVersion1_ReturnsExpectedString_WhenInvoked() {
            using (HttpClient client = IntegrationTestManager.GetClient()) {
                // arrange
                const string address = "/api/v1/hello";
                
                // act
                HttpResponseMessage responseMessage = client.GetAsync(address).GetAwaiter().GetResult();
                string result = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // assert
                Assert.AreEqual("\"Version1.0\"", result);
            }
        }

        [TestMethod]
        public void HelloApiVersion2_ReturnsExpectedString_WhenInvoked()
        {
            using (HttpClient client = IntegrationTestManager.GetClient())
            {
                // arrange
                const string address = "/api/v2.5/hello";

                // act
                HttpResponseMessage responseMessage = client.GetAsync(address).GetAwaiter().GetResult();
                string result = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // assert
                Assert.AreEqual("\"Version2.5\"", result);
            }
        }
    }
}