namespace SDammann.WebApi.Versioning.Tests.Integration {
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Versioning.Request;

    [TestClass]
    public class DefaultSettingsIntegrationTest {
        [ClassInitialize]
        public static void Setup(TestContext ctx) {
            IntegrationTestManager.Startup(c => {
                ApiVersioning.Configure(c)
                         .ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();
            });
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
        public void HelloApiVersion3_ReturnsErrorDoesntExist_WhenInvoked() {
            using (HttpClient client = IntegrationTestManager.GetClient()) {
                // arrange
                const string address = "/api/v3/hello";
                
                // act
                HttpResponseMessage responseMessage = client.GetAsync(address).GetAwaiter().GetResult();
                string result = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // assert
                Assert.IsTrue(Regex.IsMatch(result, "The API '([A-z0-9. ]+)' doesn't exist"),
                    "Expected a message like \"The API 'xxx' doesn't exist\"");
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