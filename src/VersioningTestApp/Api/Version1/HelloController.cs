namespace VersioningTestApp.Api.Version1 {
    using System.Web.Http;

    /// <summary>
    /// Initial limited hello implementation.
    /// </summary>
    /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
    public sealed class HelloController : ApiController {
        /// <summary>
        /// Gets an fine greeting from the controller
        /// </summary>
        /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
        /// <returns></returns>
        public Message Get() {
            return new Message("Hello World from API version 1!", "Hello World");
        }

        /// <summary>
        /// Deletes some message
        /// </summary>
        /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
        /// <param name="messageId"></param>
        public void Delete(string messageId)
        {
            // this should show up as a delete.
        }
    }
}