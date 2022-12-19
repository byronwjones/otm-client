using BWJ.Web.OTM.Models;
using System.Collections.Generic;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class Organization : IOrganization
    {
        public int OrganizationId { get; set; }
        public OrganizationType OrganizationType { get; set; }
        public string Description { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string OrganizationLanguage { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CityAndJurisdiction { get; set; }
        public Country Country { get; set; }
        public string BigMapBy { get; set; }
        public string CustomColumn1 { get; set; }
        public string CustomColumn2 { get; set; }
        public IEnumerable<Language> AdditionalOrganizationLanguages { get; set; } = new List<Language>();
    }
}
