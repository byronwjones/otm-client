using BWJ.Web.OTM.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BWJ.Web.OTM.Models.Request.Tools.Address
{
    public sealed class CreateAddressRequest
    {
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
        public bool IsSexOffender { get; set; }
        public string Notes { get; set; }
        public bool UpdateGeocode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? MapPageNumber { get; set; }
        public string MapGrid { get; set; }
        public string AddressSource { get; set; }
    }

    internal sealed class InternalCreateAddressRequest
    {
        public InternalCreateAddressRequest(CreateAddressRequest request)
        {
            TerID = request.TerritoryId;
            BusRes = request.AddressType.ToOptionValue();
            Name = request.HouseholderName;
            Addr = request.AddressNumber;
            Dir = request.StreetCardinalDirectionName;
            Street = request.StreetName;
            Apt = request.Apartment;
            City = request.City;
            State = request.Jurisdiction;
            Zip = request.PostalCode;
            Phone = request.Telephone;
            PhoneCell = request.CellPhone;
            PhoneOther = request.TelephoneOther;
            Email1 = request.Email;
            BusOrComplexName = request.Business_ComplexName;
            AddrSource = request.AddressSource;
            Confirmed = request.AddressIsConfirmed.ToBooleanNumber();
            Religion = request.Religion;
            CrosStreet = request.CrossStreet;
            DNC = request.IsDoNotCall.ToBooleanNumber();
            LGD = request.HasLockedGate_Dog.ToBooleanNumber();
            SexOffndr = request.IsSexOffender.ToBooleanNumber();
            TBMPageNum = request.MapPageNumber?.ToString();
            TBMGrid = request.MapGrid;
            Lat = request.Latitude?.ToString("F5");
            Lng = request.Longitude?.ToString("F5");
            Notes = request.Notes;
            Lang = request.Language.ToDescriptiveString();
        }

        public int TerID { get; }
        public string BusRes { get; }
        public string Name { get; }
        public string Addr { get; }
        public string Dir { get; }
        public string Street { get; }
        public string Apt { get; }
        public string City { get; }
        public string State { get; }
        public string Zip { get; }
        public string Phone { get; }
        public string PhoneCell { get; }
        public string PhoneOther { get; }
        public string Email1 { get; }
        public string BusOrComplexName { get; }
        public string AddrSource { get; }
        public int Confirmed { get; }
        public string Religion { get; }
        public string CrosStreet { get; }
        public int DNC { get; }
        public int LGD { get; }
        public int SexOffndr { get; }
        public string TBMPageNum { get; }
        public string TBMGrid { get; }
        public string Lat { get; }
        public string Lng { get; }
        public string Notes { get; }
        public string Lang { get; }
        public string save { get; } = "save";
    }
}
