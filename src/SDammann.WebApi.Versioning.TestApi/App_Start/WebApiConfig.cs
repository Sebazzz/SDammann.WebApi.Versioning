namespace SDammann.WebApi.Versioning.TestApi {
    using System.Web.Hosting;
    using System.Web.Http;
    using Areas.HelpPage;

    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            config.SetDocumentationProvider(
                new XmlDocumentationProvider(
                    HostingEnvironment.MapPath("~/bin/" + typeof (WebApiConfig).Assembly.GetName().Name + ".xml")));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );
        }
    }
}