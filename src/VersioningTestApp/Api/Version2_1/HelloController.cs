namespace VersioningTestApp.Api.Version2_1 {
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Hello version 2.1!
    /// </summary>
    /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
    public sealed class HelloController : ApiController {
        /// <summary>
        /// Gets the message with the specified language code
        /// </summary>
        /// <param name="languageCode">Language code. Mandatory.</param>
        /// <returns></returns>
        /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
        public Message Get(string languageCode) {
            if (String.IsNullOrWhiteSpace(languageCode)) {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return new Message("Hello World from API version 2.1! Language code = " + languageCode, "Hello World");
        }

        public string Post(string message, string otherthing)
        {
            return message;
        }
    }
}