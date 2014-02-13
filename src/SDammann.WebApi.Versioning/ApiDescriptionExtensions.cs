using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace SDammann.WebApi.Versioning
{
    internal static class ApiDescriptionExtensions
    {
        public static void SetResponseDescription(this ApiDescription apiDescription, ResponseDescription responseDescription)
        {
            var property = GetPropertyFrom(apiDescription);
            SetPropertyTo(apiDescription, responseDescription, property);
        }

        private static System.Reflection.PropertyInfo GetPropertyFrom(ApiDescription apiDescription)
        {
            if (apiDescription == null)
                throw new ArgumentNullException("apiDescription");
            return apiDescription.GetType().GetProperty("ResponseDescription");
        }

        private static void SetPropertyTo(ApiDescription apiDescription, ResponseDescription responseDescription, System.Reflection.PropertyInfo property)
        {
            if (property != null)
                property.SetValue(apiDescription, responseDescription);
        }
    }
}
