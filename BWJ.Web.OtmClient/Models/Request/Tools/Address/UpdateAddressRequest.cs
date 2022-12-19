using BWJ.Web.OTM.Internal;
using System;
using System.Collections.Generic;

namespace BWJ.Web.OTM.Models.Request.Tools.Address
{
    public sealed class UpdateAddressRequest
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
        public bool UpdateGeocode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? MapPageNumber { get; set; }
        public string MapGrid { get; set; }
    }

    internal sealed class InternalUpdateAddressRequest
    {
        public InternalUpdateAddressRequest(UpdateAddressRequest request)
        {
            Add(request);
        }

        public InternalUpdateAddressRequest(IEnumerable<UpdateAddressRequest> requests)
        {
            foreach(var request in requests)
            {
                Add(request);
            }
        }

        public List<int> TerAddrRec { get; } = new List<int>();
        public Dictionary<int, int> TerID { get; } = new Dictionary<int, int>();
        public Dictionary<int, string> BusOrComplexName { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Name { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Lang { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> BusRes { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Addr { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Dir { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Street { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Apt { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> City { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> State { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Zip { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Phone { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> PhoneCell { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> PhoneOther { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Email1 { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Religion { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> CrosStreet { get; } = new Dictionary<int, string>();
        public Dictionary<int, int> Confirmed { get; } = new Dictionary<int, int>();
        public Dictionary<int, int> DNC { get; } = new Dictionary<int, int>();
        public Dictionary<int, int> LGD { get; } = new Dictionary<int, int>();
        public Dictionary<int, string> LetterDateSent { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> LetterInfo { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Notes { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> NotesOther { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> RVBBSUserID { get; } = new Dictionary<int, string>();
        public Dictionary<int, int> UpdGeo { get; } = new Dictionary<int, int>();
        public Dictionary<int, string> Latitude { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> Longitude { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> TBMPageNum { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> TBMGrid { get; } = new Dictionary<int, string>();
        public string save { get; } = "save";

        private void Add(UpdateAddressRequest request)
        {
            if(request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if(TerAddrRec.Contains(request.AddressId))
            {
                throw new ArgumentException($"Address with ID {request.AddressId} already added to this request");
            }

            var id = request.AddressId;
            TerAddrRec.Add(id);
            TerID.Add(id, request.TerritoryId);
            BusOrComplexName.Add(id, s(request.Business_ComplexName));
            Name.Add(id, s(request.HouseholderName));
            Lang.Add(id, s(request.Language.ToDescriptiveString()));
            BusRes.Add(id, s(request.AddressType.ToOptionValue()));
            Addr.Add(id, s(request.AddressNumber));
            Dir.Add(id, s(request.StreetCardinalDirectionName));
            Street.Add(id, s(request.StreetName));
            Apt.Add(id, s(request.Apartment));
            City.Add(id, s(request.City));
            State.Add(id, s(request.Jurisdiction));
            Zip.Add(id, s(request.PostalCode));
            Phone.Add(id, s(request.Telephone));
            PhoneOther.Add(id, s(request.TelephoneOther));
            PhoneCell.Add(id, s(request.CellPhone));
            Email1.Add(id, s(request.Email));
            Religion.Add(id, s(request.Religion));
            CrosStreet.Add(id, s(request.CrossStreet));
            Confirmed.Add(id, request.AddressIsConfirmed ? 1 : 0);
            DNC.Add(id, request.IsDoNotCall ? 1 : 0);
            LGD.Add(id, request.HasLockedGate_Dog ? 1 : 0);
            LetterDateSent.Add(id, s(request.DateLetterSent?.ToString("MM/dd/yyyy")));
            LetterInfo.Add(id, s(request.LetterSubject_Description));
            Notes.Add(id, s(request.Notes));
            NotesOther.Add(id, s(request.AdminNotes));
            RVBBSUserID.Add(id, s(request.VisitingPublisherId is null ? "NULL" : request.VisitingPublisherId));
            UpdGeo.Add(id, request.UpdateGeocode ? 1 : 0);
            Latitude.Add(id, s(request.Latitude));
            Longitude.Add(id, s(request.Longitude));
            TBMPageNum.Add(id, s(request.MapPageNumber));
            TBMGrid.Add(id, s(request.MapGrid));
        }

        private static string s(object o) => Utils.ToFormString(o);
    }
}
