using BWJ.Web.OTM.Models;
using System;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class Address : IAddress
    {
        public int AddressId { get; set; }
        public int TerritoryId { get; set; }
        public string Business_ComplexName { get; set; }
        public string HouseholderName { get; set; }
        public Language Language { get; set; }
        public AddressType AddressType { get; set; }
        public string AddressNumber { get; set; }
        public string StreetCardinalDirectionName { get; set; }
        public string StreetName { get; set; }
        public string CrossStreet { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Jurisdiction { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string TelephoneOther { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Religion { get; set; }
        public bool AddressIsConfirmed { get; set; }
        public bool IsDoNotCall { get; set; }
        public bool HasLockedGate_Dog { get; set; }
        public DateTime? DateLetterSent { get; set; }
        public string LetterSubject_Description { get; set; }
        public string Notes { get; set; }
        public string AdminNotes { get; set; }
        public int? VisitingPublisherId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? MapPageNumber { get; set; }
        public string MapGrid { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateLastWorked { get; set; }
        public string Source { get; set; }
    }
}
