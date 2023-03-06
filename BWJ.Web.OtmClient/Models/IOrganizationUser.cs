namespace BWJ.Web.OTM.Models
{
    public interface IOrganizationUser
    {
        string Email { get; }
        string FullName { get; }
        Language Language { get; }
        int? MaxNumberOfTerritoriesAllowed { get; }
        string PhoneNumber { get; }
        bool? PublicWitnessingApproved { get; }
        string UserId { get; }
        string UserName { get; }
        OtmUserType UserType { get; }
    }
}