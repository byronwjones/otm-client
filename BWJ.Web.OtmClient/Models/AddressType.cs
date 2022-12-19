using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Attributes;

namespace BWJ.Web.OTM.Models
{
    public enum AddressType
    {
        [WebOption("", "")]
        Indeterminate,

        [WebOption("R", "Residential")]
        Residential,
        [WebOption("B", "Business")]
        Business,
        [WebOption("L", "Letter Writing")]
        LetterWriting,
        [WebOption("T", "Telephone")]
        Telephone,
    }

    public static class AddressTypeExtensions
    {
        public static string ToDescriptiveString(this AddressType addressType)
            => Utils.EnumToDescriptiveString(addressType);

        public static string ToOptionValue(this AddressType addressType)
            => Utils.EnumToOptionValue(addressType);
    }

    public static class AddressTypeUtils
    {
        public static AddressType FromValue(string value)
            => Utils.OptionValueToEnumValue<AddressType>(value);
    }
}
