namespace SDammann.WebApi.Versioning.Request {
    using System.Net.Http;

    /// <summary>
    /// Defines the interface for classes which detect controller names from requests
    /// </summary>
    public interface IRequestControllerNameDetector {
        /// <summary>
        /// Gets a name for the controller from the specified request message. Returns null if no controller name could be detected.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        string GetControllerName(HttpRequestMessage requestMessage);
    }
}