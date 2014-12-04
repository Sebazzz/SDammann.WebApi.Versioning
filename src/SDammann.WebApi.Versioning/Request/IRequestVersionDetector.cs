namespace SDammann.WebApi.Versioning.Request {
    using System.Net.Http;

    /// <summary>
    /// Defines the interface for classes which detect API versions from requests
    /// </summary>
    public interface IRequestVersionDetector {
        /// <summary>
        /// Gets the <see cref="ApiVersion"/> requested from the request. Returns null if no API version could be detected, may throw a <see cref="ApiVersionFormatException"/> if the API version cannot be parsed
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        /// <exception cref="ApiVersionFormatException">Thrown when the API version cannot be parsed because of an invalid format</exception>
        ApiVersion GetVersion(HttpRequestMessage requestMessage);
    }
}