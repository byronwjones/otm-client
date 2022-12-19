using System;

namespace BWJ.Web.OTM.Models
{
    public interface IAddressSearchResult
    {
        string Address { get; }
        int AddressId { get; }
        bool AddressIsConfirmed { get; }
        AddressType AddressType { get; }
        string Apartment { get; }
        string CheckedOutBy { get; }
        string City { get; }
        bool HasLockedGate_Dog { get; }
        string HouseholderName { get; }
        bool IsDoNotCall { get; }
        string Jurisdiction_PostalCode { get; }
        Language Language { get; }
        DateTime? LastWorked { get; }
        string Notes { get; }
        string Telephone { get; }
        string TerritoryName { get; }
        string VisitingPublisher { get; }
    }
}
