using System;

namespace BWJ.Web.OTM.Internal.Attributes
{
    internal class WebOptionAttribute : Attribute
    {
        public WebOptionAttribute(string value, string description)
        {
            Value = value;
            Description = description;
        }

        public string Value { get; }
        public string Description { get; }
    }
}
