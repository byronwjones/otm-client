using BWJ.Web.OTM.Internal;
using System.ComponentModel;

namespace BWJ.Web.OTM.Models
{
    public enum OrganizationType
    {
        [Description("")]
        Unknown,

        [Description("Congregation")]
        Congregation,
        [Description("Group")]
        Group,
        [Description("Pre-Group")]
        PreGroup,
    }

    public static class OrganizationTypeExtensions
    {
        public static string ToDescriptiveString(this OrganizationType type)
            => Utils.EnumToDescriptiveString(type);
    }

    public static class OrganizationTypeUtils
    {
        public static OrganizationType FromValue(string value)
        {
            switch (value)
            {
                case "Congregation":
                    return OrganizationType.Congregation;
                case "Group":
                    return OrganizationType.Group;
                case "Pre-Group":
                    return OrganizationType.PreGroup;
                default:
                    return OrganizationType.Unknown;
            }
        }
    }
}
