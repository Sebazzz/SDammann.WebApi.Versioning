namespace SDammann.WebApi.Versioning.Tests.Integration.Controllers.Version1 {
    using System.Web.Http;

    public sealed class HelloController : ApiController {

        public string Get() {
            return "Version1.0";
        }
    }
}