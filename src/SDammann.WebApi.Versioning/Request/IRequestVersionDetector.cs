namespace SDammann.WebApi.Versioning.Request {
    using System.Net.Http;

    /// <summary>
    /// Defines the interface for classes which detect API versions from requests
    /// </summary>
    public interface IRequestVersionDetector {
        /// <summary>
        /// Gets the <see cref="ApiVersion"/> requested from the request. Returns null if no API version could be detected.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        ApiVersion GetVersion(HttpRequestMessage requestMessage);
    }
}