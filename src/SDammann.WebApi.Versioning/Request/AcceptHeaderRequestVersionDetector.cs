namespace SDammann.WebApi.Versioning.Request {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;

    /// <summary>
    /// Represents an <see cref="IRequestVersionDetector"/> implementation that parses an API version out of an accept header
    /// </summary>
    public abstract class AcceptHeaderRequestVersionDetector : IRequestVersionDetector {
        /// <summary>
        /// Gets the <see cref="ApiVersion"/> requested from the request. Returns null if no API version could be detected.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public ApiVersion GetVersion(HttpRequestMessage requestMessage) {
            if (requestMessage == null) {
                throw new ArgumentNullException("requestMessage");
            }

            // get "accept" HTTP header value
            var acceptHeader = requestMessage.Headers.Accept;

            return this.GetVersionFromHeader(acceptHeader);
        }

        /// <summary>
        /// Returns the API version from the collection with accept header values. Derived classes may override this.
        /// </summary>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        protected virtual ApiVersion GetVersionFromHeader(IEnumerable<MediaTypeWithQualityHeaderValue> acceptHeader)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (MediaTypeWithQualityHeaderValue headerValue in acceptHeader.OrderByDescending(x => x.Quality)) {
                ApiVersion version = this.GetVersionFromSingleHeader(headerValue);

                if (version != null)
                {
                    return version;
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts an API version from a single request header
        /// </summary>
        /// <param name="headerValue"></param>
        /// <returns></returns>
        protected abstract ApiVersion GetVersionFromSingleHeader(MediaTypeWithQualityHeaderValue headerValue);
    }
}