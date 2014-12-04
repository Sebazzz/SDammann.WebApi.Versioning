#if WWW
namespace SDammann.WebApi.Versioning.TestApi.Api.Version1 {
#else
namespace SDammann.WebApi.Versioning.Tests.Integration.Controllers.Version1 {
#endif
    using System.Web.Http;

    public sealed class HelloController : ApiController {

        public string Get() {
            return "Version1.0";
        }
    }
}