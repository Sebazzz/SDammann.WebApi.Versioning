#if WWW
namespace SDammann.WebApi.Versioning.TestApi.Api.Version2_5 {
#else
namespace SDammann.WebApi.Versioning.Tests.Integration.Controllers.Version2_5 {
#endif
    using System.Web.Http;

    public sealed class HelloController : ApiController {
        public string Get() {
            return "Version2.5";
        }
    }
}