namespace BWJ.Web.OTM.Models
{
    public enum OtmUserType
    {
        Unspecified,
        General,
        Publisher,
        GroupAdmin,
        ReadOnly,
        MobileOnly
    }

    public static class OtmUserTypeExtensions
    {
        public static string ToDescriptiveString(this OtmUserType type)
        {
            switch (type)
            {
                case OtmUserType.General:
                    return "General";
                case OtmUserType.Publisher:
                    return "Publisher";
                case OtmUserType.GroupAdmin:
                    return "Group Admin";
                case OtmUserType.ReadOnly:
                    return "Read-Only";
                case OtmUserType.MobileOnly:
                    return "MobileOnly";
                case OtmUserType.Unspecified:
                default:
                    return string.Empty;
            }
        }
    }
}
