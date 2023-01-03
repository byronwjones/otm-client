namespace BWJ.Web.OTM.Models
{
    public interface IOrganizationUser
    {
        string Email { get; set; }
        string FullName { get; set; }
        Language Language { get; set; }
        int? MaxNumberOfTerritoriesAllowed { get; set; }
        string PhoneNumber { get; set; }
        bool? PublicWitnessingApproved { get; set; }
        string UserId { get; set; }
        string UserName { get; set; }
        OtmUserType UserType { get; set; }
    }
}