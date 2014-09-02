namespace VersioningTestApp {
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Routing;
    using SDammann.WebApi.Versioning;
    using WebApi.DocumentationController.DocumentationProviders;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            // enable API versioning
            GlobalConfiguration.Configuration.Services.Replace(typeof(IApiExplorer), new VersionedApiExplorer(GlobalConfiguration.Configuration));
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IDocumentationProvider), new XmlCommentDocumentationProvider());
            GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerSelector), new RouteVersionedControllerSelector(GlobalConfiguration.Configuration));
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AcceptHeaderVersionedControllerSelector(GlobalConfiguration.Configuration));
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}