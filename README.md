SDammann.WebApi.Versioning
==========================

Versioning support for ASP.NET Web Api. Associated blog post: http://damsteen.nl/blog/implementing-versioning-in-asp.net-web-api

There is also a [Nuget package](https://nuget.org/packages/SDammann.WebApi.Versioning) available.

New testing version
--------------------------
There is a new rewritten version being developed. Please check the   [vnext](https://github.com/Sebazzz/SDammann.WebApi.Versioning/tree/vnext) branch. The documentation has not been finalized yet nor a prerelease package has been pushed, but based on [this file](https://github.com/Sebazzz/SDammann.WebApi.Versioning/blob/vnext/src/SDammann.WebApi.Versioning.Tests/Integration/IntegrationTestManager.cs#L54) you should be able to get it working with your project.

Using the library
--------------------------
Put something like this in your Global.asax `Application_Start`:

	// enable API versioning
	GlobalConfiguration.Configuration.Services.Replace(typeof (IHttpControllerSelector),
												   new XXXX(GlobalConfiguration.Configuration));


Replace XXX with the name of an `IHttpControllerSelector` implementation from the SDammann.WebApi.Versioning 
namespace. The implementations that are included are listed below.

The fully qualified name of the API controllers must include one section that is equal to VersionN, where
N is an version number. For example: `TestWebsite.Api.Version1.HelloController` is an controller with name
"Hello" for version 1 of the API. This convention can be changed, see below.

Changing the API controller convention
------------------------------------------

It is possible to override the default convention used for detecting a Controller's version. 
This convention can be changed by assigning a different value to the `VersionedControllerSelector.VersionPrefix` property (new since 1.2).

Make sure to do this very early in your application, for example in the `Application_Start` method in Global.asax.

IHttpControllerSelector implementations
------------------------------------------

`SDammann.WebApi.Versioning.RouteVersionedControllerSelector`:
	Supports selecting an API controller by the "version" string in the request URI. Requires the routing
	table to be modified to include an "version" key. You can use something like this:

	routes.MapHttpRoute(
					name: "DefaultApi",
					routeTemplate: "api/v{version}/{controller}/{id}",
					defaults: new {id = RouteParameter.Optional}
		);

`SDammann.WebApi.Versioning.VersionHeaderVersionedControllerSelector`:
	Supports selecting an API controller by the "X-Api-Version" HTTP header. The integer value of the
	HTTP header determines the API version to use. 

`SDammann.WebApi.Versioning.AcceptHeaderVersionedControllerSelectorBase`:
	This in an abstract class that supports versioning by the "Accept" HTTP header and enables callers
	to use an Accept header value like 'application/vnd.company.myapp-v3+json' to select the API
	version. Some REST-evangelists think this is the best way to implement API versioning.
	Derived classes need to implement one method to return the API version from the media type string
	in the Accept header: GetVersion
	
`SDammann.WebApi.Versioning.AcceptHeaderControllerSelector`:
	Implementation of the `AcceptHeaderVersionedControllerSelector` class. Requires the Accept header to
	be in the format of '<mime-type>; version=<number>'. For example: 'application/json; version=1'.

	
License
-----------------------------------------
Apache 2.0 license. License details are available in the LICENSE file.


