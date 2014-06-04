using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VersioningTestApp.Api;

namespace VersioningTestApp.Controllers
{
    public class HelloController : ApiController
    {
        /// <summary>
        /// Gets an fine greeting from the controller
        /// </summary>
        /// <remarks>Bogus documentation that will turn up in the generate documentation pages.</remarks>
        /// <returns></returns>
        public Message Get()
        {
            return new Message("Hello World from General API without version!", "Hello World");
        }
    }
}
