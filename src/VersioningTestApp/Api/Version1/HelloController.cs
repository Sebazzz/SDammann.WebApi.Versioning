namespace VersioningTestApp.Api.Version1 {
    using System.Web.Http;

    public sealed class HelloController : ApiController {
        public Message Get() {
            return new Message("Hello World from API version 1!", "Hello World");
        }
    }
}