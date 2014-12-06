using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDammann.WebApi.Versioning.TestApi.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return this.RedirectToAction("Index", "Help", new {area = ""});
        }
    }
}
