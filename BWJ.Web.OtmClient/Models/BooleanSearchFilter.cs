using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Attributes;

namespace BWJ.Web.OTM.Models
{
    public enum BooleanSearchFilter
    {
        [WebOption("", "No Filtering")]
        NoFiltering,

        [WebOption("1", "Include Only")]
        IncludeOnly,

        [WebOption("0", "Exclude All")]
        ExcludeAll,
    }

    public static class BooleanSearchFilterExtensions
    {
        public static string ToDescriptiveString(this BooleanSearchFilter searchScope)
            => Utils.EnumToDescriptiveString(searchScope);

        public static string ToOptionValue(this BooleanSearchFilter searchScope)
            => Utils.EnumToOptionValue(searchScope);
    }
}
