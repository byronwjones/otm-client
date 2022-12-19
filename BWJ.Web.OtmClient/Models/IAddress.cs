using System;

namespace BWJ.Web.OTM.Models
{
    public interface IAddress
    {
        int AddressId { get; }
        bool AddressIsConfirmed { get; }
        string AddressNumber { get; }
        AddressType AddressType { get; }
        string AdminNotes { get; }
        string Apartment { get; }
        string Business_ComplexName { get; }
        string CellPhone { get; }
        string City { get; }
        string CrossStreet { get; }
        DateTime DateAdded { get; }
        DateTime? DateLastWorked { get; }
        DateTime? DateLetterSent { get; }
        string Email { get; }
        bool HasLockedGate_Dog { get; }
        string HouseholderName { get; }
        bool IsDoNotCall { get; }
        string Jurisdiction { get; }
        Language Language { get; }
        double? Latitude { get; }
        string LetterSubject_Description { get; }
        double? Longitude { get; }
        string MapGrid { get; }
        int? MapPageNumber { get; }
        string Notes { get; }
        string PostalCode { get; }
        string Religion { get; }
        string Source { get; }
        string StreetCardinalDirectionName { get; }
        string StreetName { get; }
        string Telephone { get; }
        string TelephoneOther { get; }
        int TerritoryId { get; }
        int? VisitingPublisherId { get; }
    }
}