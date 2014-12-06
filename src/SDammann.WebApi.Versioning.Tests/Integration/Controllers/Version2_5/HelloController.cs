#if WWW
namespace SDammann.WebApi.Versioning.TestApi.Api.Version2_5 {
#else
namespace SDammann.WebApi.Versioning.Tests.Integration.Controllers.Version2_5 {
#endif
    using System.Web.Http;

    /// <summary>
    /// Hello controller version 2.5 description
    /// </summary>
    /// <remarks>
    /// Hello controller version 2.5 remarks
    /// </remarks>
    public sealed class HelloController : ApiController {
        /// <summary>
        /// Hello controller version 2.5 'GET' method description
        /// </summary>
        /// <returns>Description of return value</returns>
        public string Get() {
            return "Version2.5";
        }
    }
}