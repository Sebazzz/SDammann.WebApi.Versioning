namespace VersioningTestApp {
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Routing;
    using SDammann.WebApi.Versioning;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // enable API versioning
            GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerSelector), new RouteVersionedControllerSelector(GlobalConfiguration.Configuration));
        }
    }
}