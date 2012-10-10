using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace VersioningTestApp.Models
{
    public class DocumentationModel
    {
        public DocumentationModel(IApiExplorer explorer)
        {
            if (explorer == null)
                throw new ArgumentNullException("explorer");
            this.Explorer = explorer;
        }

        public IApiExplorer Explorer { get; set; }
    }
}