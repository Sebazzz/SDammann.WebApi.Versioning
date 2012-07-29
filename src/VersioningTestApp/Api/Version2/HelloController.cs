namespace VersioningTestApp.Api.Version2 {
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;


    public sealed class HelloController : ApiController {
        public Message Get (string languageCode) {
            if (String.IsNullOrWhiteSpace(languageCode)) {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return new Message("Hello World from API version 2! Language code = " + languageCode, "Hello World");
        }
    }
}