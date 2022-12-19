using BWJ.Web.OTM.Models;
using System;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class AddressSearchResult : IAddressSearchResult
    {
        public int AddressId { get; set; }
        public string TerritoryName { get; set; }
        public string HouseholderName { get; set; }
        public Language Language { get; set; }
        public AddressType AddressType { get; set; }
        public string Address { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Jurisdiction_PostalCode { get; set; }
        public string Telephone { get; set; }
        public bool AddressIsConfirmed { get; set; }
        public bool IsDoNotCall { get; set; }
        public bool HasLockedGate_Dog { get; set; }
        public string Notes { get; set; }
        public string CheckedOutBy { get; set; }
        public string VisitingPublisher { get; set; }
        public DateTime? LastWorked { get; set; }
    }
}
