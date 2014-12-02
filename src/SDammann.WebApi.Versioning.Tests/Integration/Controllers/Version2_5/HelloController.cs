namespace SDammann.WebApi.Versioning.Tests.Integration.Controllers.Version2_5 {
    using System.Web.Http;

    public sealed class HelloController : ApiController {
        public string Get() {
            return "Version2.5";
        }
    }
}