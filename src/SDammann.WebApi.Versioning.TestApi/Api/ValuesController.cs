namespace SDammann.WebApi.Versioning.TestApi.Api {
    using System.Collections.Generic;
    using System.Web.Http;

    /// <summary>
    ///     Test documentation for the 'api/values' controller
    /// </summary>
    [Authorize]
    public class ValuesController : ApiController {
        /// <summary>
        ///     Test documentation for api 'api/values' (GET)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get() {
            return new[] {"value1", "value2"};
        }

        /// <summary>
        ///     Test documentation for api 'api/values/5' (GET)
        /// </summary>
        /// <returns></returns>
        public string Get(int id) {
            return "value";
        }

        /// <summary>
        ///     Test documentation for api 'api/values' (POST)
        /// </summary>
        /// <returns></returns>
        public void Post([FromBody] string value) {}

        /// <summary>
        ///     Test documentation for api 'api/values/5' (PUT)
        /// </summary>
        /// <returns></returns>
        public void Put(int id, [FromBody] string value) {}

        /// <summary>
        ///     Test documentation for api 'api/values/5' (DELETE)
        /// </summary>
        /// <returns></returns>
        public void Delete(int id) {}
    }
}