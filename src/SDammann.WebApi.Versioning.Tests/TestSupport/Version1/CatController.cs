namespace SDammann.WebApi.Versioning.Tests.TestSupport.Version1 {
    using System.Web.Http;
    using Versioning.Discovery;

    internal class CatController : ApiController {
        
    }

    namespace Version {
        [ApiVersion(6,5,3,2)]
        internal class CatControllerInVersionNamespace : ApiController
        {

        }
    }

    [ApiVersion(3,5)]
    internal class CatCtrl : CatController {
        
    }

    internal class Controller : ApiController {
        
    }


    namespace Version2_5 {
        internal class VersionTwoDotFiveController : ApiController {
            
        }
    }
}

internal class ControllerWithNoNamespace {
    
}

namespace SDammann.WebApi.Versioning.Tests.TestSupport {
    internal class ControllerWithNoVersionNamespace {
        
    }

}