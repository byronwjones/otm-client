using System;

namespace BWJ.Web.OTM.Exceptions
{
    public class HtmlParsingException : Exception
    {
        public HtmlParsingException(string message) : base(message) { }
        public HtmlParsingException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
