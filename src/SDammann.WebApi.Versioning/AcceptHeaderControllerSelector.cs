namespace SDammann.WebApi.Versioning {
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Web.Http;

    /// <summary>
    /// Version controller that enables version by MIME type in the accept header
    /// </summary>
    public sealed class AcceptHeaderVersionedControllerSelector : AcceptHeaderVersionedControllerSelectorBase {
        /// <summary>
        /// Specifies the media type to accept. Set this in your Application_Start or before.
        /// </summary>
        public static string AcceptMediaType = "application/json";

        /// <summary>
        /// Initializes the <see cref="AcceptHeaderVersionedControllerSelector"/> instance
        /// </summary>
        /// <param name="configuration"></param>
        public AcceptHeaderVersionedControllerSelector(HttpConfiguration configuration) : base(configuration) { }

        protected override int? GetVersion(MediaTypeWithQualityHeaderValue mimeType) {
            if (!String.Equals(mimeType.MediaType, AcceptMediaType, StringComparison.InvariantCultureIgnoreCase)) {
                return null;
            }

            // get version
            NameValueHeaderValue versionParameter =
                mimeType.Parameters.FirstOrDefault(parameter => parameter.Name == "version");

            if (versionParameter == null) {
                return null;
            }

            // parse version
            int version;
            if (!Int32.TryParse(versionParameter.Value, NumberStyles.None, CultureInfo.InvariantCulture, out version)) {
                return null;
            }

            return version;
        }
    }
}