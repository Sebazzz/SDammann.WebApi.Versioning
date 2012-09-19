ASP.NET Web API Versioning module
==================================

Author: Sebastiaan Dammann
Website: http://damsteen.nl/
Project website: https://github.com/Sebazzz/SDammann.WebApi.Versioning
License: https://github.com/Sebazzz/SDammann.WebApi.Versioning/blob/master/LICENSE

-----------------------------------

Put something like this in your Global.asax Application_Start:

			// enable API versioning
            GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerSelector),
                                                           new XXXX(GlobalConfiguration.Configuration));


Replace XXX with the name of an IHttpControllerSelector implementation from the SDammann.WebApi.Versioning 
namespace. The implementations that are included are listed below.

The fully qualified name of the API controllers must include one section that is equal to VersionN, where
N is an version number. For example: TestWebsite.Api.Version1.HelloController is an controller with name
"Hello" for version 1 of the API. This convention can be changed, see below.

------------------------------------------
Changing the API controller convention
------------------------------------------

It is possible to override the default convention used for detecting a Controller's version. 
This convention can be changed by assigning a different value to the VersionedControllerSelector.VersionPrefix property (new since 1.2).

Make sure to do this very early in your application, for example in the Application_Start method in Global.asax.

------------------------------------------
IHttpControllerSelector implementations
------------------------------------------

SDammann.WebApi.Versioning.RouteVersionedControllerSelector:
	Supports selecting an API controller by the "version" string in the request URI. Requires the routing
	table to be modified to include an "version" key. You can use something like this:

	            routes.MapHttpRoute(
                                name: "DefaultApi",
                                routeTemplate: "api/v{version}/{controller}/{id}",
                                defaults: new {id = RouteParameter.Optional}
                    );



SDammann.WebApi.Versioning.VersionHeaderVersionedControllerSelector:
	Supports selecting an API controller by the "X-Api-Version" HTTP header. The integer value of the
	HTTP header determines the API version to use. 



SDammann.WebApi.Versioning.AcceptHeaderVersionedControllerSelector:
	This in an abstract class that supports versioning by the "Accept" HTTP header and enables callers
	to use an Accept header value like 'application/vnd.company.myapp-v3+json' to select the API
	version. Some REST-evangelists think this is the best way to implement API versioning.

	Derived classes need to implement one method to return the API version from the media type string
	in the Accept header: GetVersion
