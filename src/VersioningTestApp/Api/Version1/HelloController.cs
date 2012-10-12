namespace VersioningTestApp.Api.Version1 {
    using System.Web.Http;

    public sealed class HelloController : ApiController {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public Message Get() {
            return new Message("Hello World from API version 1!", "Hello World");
        }
        public void Delete(string messageId)
        {
            // this should show up as a delete.
        }
    }
}