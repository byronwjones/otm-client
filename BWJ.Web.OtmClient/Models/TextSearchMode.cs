using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Attributes;

namespace BWJ.Web.OTM.Models
{
    public enum TextSearchMode
    {
        [WebOption("LIKE", "Contains Search Phrase")]
        ContainsSearchPhrase,
        [WebOption("NOT LIKE", "Does Not Contain Search Phrase")]
        DoesNotContainSearchPhrase
    }

    public static class TextSearchModeExtensions
    {
        public static string ToDescriptiveString(this TextSearchMode textSearchMode)
            => Utils.EnumToDescriptiveString(textSearchMode);

        public static string ToOptionValue(this TextSearchMode textSearchMode)
            => Utils.EnumToOptionValue(textSearchMode);
    }
}
