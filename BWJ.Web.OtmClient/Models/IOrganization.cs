using System.Collections.Generic;

namespace BWJ.Web.OTM.Models
{
    public interface IOrganization
    {
        IEnumerable<Language> AdditionalOrganizationLanguages { get; }
        string Address { get; }
        string BigMapBy { get; }
        string CityAndJurisdiction { get; }
        string ContactEmail { get; }
        string ContactPerson { get; }
        string ContactPhoneNumber { get; }
        Country Country { get; }
        string CustomColumn1 { get; }
        string CustomColumn2 { get; }
        string Description { get; }
        decimal Latitude { get; }
        decimal Longitude { get; }
        int OrganizationId { get; }
        string OrganizationLanguage { get; }
        OrganizationType OrganizationType { get; }
    }
}