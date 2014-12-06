using System;

namespace SDammann.WebApi.Versioning.TestApi.Areas.HelpPage
{
    /// <summary>
    /// This represents an image sample on the help page. There's a display template named ImageSample associated with this class.
    /// </summary>
    public class ImageSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSample"/> class.
        /// </summary>
        /// <param name="src">The URL of an image.</param>
        public ImageSample(string src)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            this.Src = src;
        }

        public string Src { get; private set; }

        public override bool Equals(object obj)
        {
            ImageSample other = obj as ImageSample;
            return other != null && this.Src == other.Src;
        }

        public override int GetHashCode()
        {
            return this.Src.GetHashCode();
        }

        public override string ToString()
        {
            return this.Src;
        }
    }
}