# SDammann.WebApi.Versioning

Easy, well-tested, and extendable ASP.NET Web API versioning.

There is also a [Nuget package](https://nuget.org/packages/SDammann.WebApi.Versioning) available.

## Using the library
### Setting up
Register the `DefaultControllerIdentificationDetector` and `DefaultRequestControllerIdentificationDetector` class with your dependency container.

Next, choose a versioning scheme and register it with the library. For example, to use versioning by API route (this will use urls like `/api/v1.2/product/`) include the following in your Web API configuration delegate:

    ApiVersioning.Configure(config)
                 .ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();

 The code assumes the `config` variable is of type `HttpConfiguration`. Make sure to register this class with your dependency container.

### Choosing an API versioning scheme
Currently the library supports two API versioning schemes.

#### API versions by URL / routing. 
This will allow the API version to be chosen by your URL routing scheme. Example:

	config.Routes.MapHttpRoute(
		name: "DefaultApi",
		routeTemplate: "api/v{version}/{controller}/{id}",
		defaults: new { id = RouteParameter.Optional }
	);

    ApiVersioning.Configure(config)
                 .ConfigureRequestVersionDetector<DefaultRouteKeyVersionDetector>();

#### API versions by HTTP header
A scheme preferred by many REST purists. This allows the API version to be chosen by using the MIME type in the Accept header of the request. In order to achieve this, inherit from `AcceptHeaderRequestVersionDetector` and implement the `GetVersionFromSingleHeader` method.

Next, register your implementation in the Web API configuration delegate:

    ApiVersioning.Configure(config)
                 .ConfigureRequestVersionDetector<YourCustomRoutingDetector>();

Don't forget to set-up your custom MIME-type in [content negotiation](http://www.asp.net/web-api/overview/formats-and-model-binding/content-negotiation) so ASP.NET Web API will output the correct response format.

### Concepts
The library distinguishes several responsibilities in API versioning:

- Controller naming / versioning
- External request naming / versioning
- The definition of a version itself

Controller name and version detection are implemented using the `IControllerNameDetector` and `IControllerVersionDetector` interfaces. The default `DefaultControllerNameDetector` implementation uses the ASP.NET Web API conventions, like requiring your controller class name to end with `Controller`. The default `DefaultControllerVersionDetector` expects you to put your controllers in 'versioned' namespaces. For instance, controllers in namespace `MyApi.Version1_1` will point to version 1.1 of the API. There is also an implementation available that allows you to use attributes instead (`DefaultAttributeControllerVersionDetector`).

Request controller naming and versioning is implemented using the `IRequestControllerNameDetector` and `IRequestVersionDetector` interfaces. The default `DefaultRequestControllerNameDetector` implementation uses the ASP.NET Web API conventions. There is no default `IRequestVersionDetector` configured. The library forces you to choose an API versioning scheme. 

An API version itself is abstracted away as an `ApiVersion`. This is an abstract class that allows the concept of versioning itself to be customizable. For example, the `SemVerApiVersion` uses a one to four numbers to be used for API versions. One could easily extend this to support letters, for example as designation for 'beta' and 'alpha' by inheriting from `SemVerApiVersion` and then implement `IControllerVersionDetector` and `IRequestVersionDetector` or extend the existing implementations.

## Advanced concepts
### Custom exception handling
By default the library will properly handle exceptions that occur during controller selection itself. In some cases you may want to override or modify the error messages returned to the client by this library. You may do this in two ways:

1. Implementing a global exception handler and handle any `BaseApiException` derived exception yourself, as per [ASP.NET Web API documentation](http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling). Note that the library registers it's own exception handler, but you may wish to use your own. In that case you can always delegate any exception handling to `IVersioningExceptionHandler`.

2. Implement a custom `IVersioningExceptionHandler`. This exception handler is called by the library when a API versioning error occurs. You can implement your own custom response here. You can also inherit or delegate to `DefaultVersioningExceptionHandler` if you can't or won't handle an exception in certain cases.

### Extending the identification of a controller
By default, a Web API controller is identified by its version and name (in the form of a `ControllerIdentification` instance). It is possible to extend or modify this behavior. This is done by inheriting from the `ControllerIdentification` class and implementing custom versions of the `IControllerIdentificationDetector` and `IRequestControllerIdentificationDetector` interfaces or extending the default implementations. 

Note this is usually not necessary, but if you'd like to go beyond names and versions you can use this approach.

## Contact
Any questions or remarks? Drop me a twitter message (@sebazzz91) or put an issue in the issue tracker.

## Older version
The legacy older version (2.x) is available at the [2.x branch](https://github.com/Sebazzz/SDammann.WebApi.Versioning/tree/2.x).

	
License
-----------------------------------------
Apache 2.0 license. License details are available in the LICENSE file.


