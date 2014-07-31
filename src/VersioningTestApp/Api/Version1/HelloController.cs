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

        public string Post(string message)
        {
            return message;
        }
    }

    [RoutePrefix("api/v{version}/AttributeHello")]
    public sealed class AttributeHelloController : ApiController
    {

        public Message Get()
        {
            return new Message("Hello World from API version 1 routed via attributes!", "Hello Attribute World");
        }
        
        [Route("{title}/{message}")]
        public Message Get(string message, string title)
        {
            return new Message(string.Format("Echo message: {0}", message), string.Format("Hello Attribute World - {0}", title));
        }

    }
}