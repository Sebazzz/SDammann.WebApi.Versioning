using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VersioningTestApp.Models;
using System.Web.Http;
using SDammann.WebApi.Versioning;

namespace VersioningTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new DocumentationModel(new VersionedApiExplorer(GlobalConfiguration.Configuration)));
        }

    }
}
