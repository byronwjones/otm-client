using BWJ.Web.OTM.Models;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class OrganizationUser : IOrganizationUser
    {
        public string UserId { get; set; }
        public OtmUserType UserType { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public Language Language { get; set; }
        public int? MaxNumberOfTerritoriesAllowed { get; set; }
        public bool? PublicWitnessingApproved { get; set; }
    }
}
