using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace SDammann.WebApi.Versioning
{
    public static class ApiDescriptionExtensions
    {
        public static void SetResponseDescription(this ApiDescription apiDescription, ResponseDescription responseDescription)
        {
            apiDescription.GetType().GetProperty("ResponseDescription").SetValue(apiDescription, responseDescription);
        }
    }
}
