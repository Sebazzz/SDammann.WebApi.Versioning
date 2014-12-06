using System;

namespace SDammann.WebApi.Versioning.TestApi.Areas.HelpPage
{
    /// <summary>
    /// This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
        public InvalidSample(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public override bool Equals(object obj)
        {
            InvalidSample other = obj as InvalidSample;
            return other != null && this.ErrorMessage == other.ErrorMessage;
        }

        public override int GetHashCode()
        {
            return this.ErrorMessage.GetHashCode();
        }

        public override string ToString()
        {
            return this.ErrorMessage;
        }
    }
}