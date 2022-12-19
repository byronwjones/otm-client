using BWJ.Web.OTM.Internal;
using System;

namespace BWJ.Web.OTM.Models.Request.Tools.Address
{
    public class SearchAddressRequest
    {
        public int? TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public string HouseholderName { get; set; }
        public Language? Language { get; set; }
        public AddressType? AddressType { get; set; }
        public string AddressNumber { get; set; }
        public string StreetName { get; set; }
        public string CrossStreet { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Religion { get; set; }
        public BooleanSearchFilter DoNotCallFiltering { get; set; }
        public BooleanSearchFilter LockedGate_DogFiltering { get; set; }
        //public BooleanSearchFilter WorkedAddressFiltering { get; set; }
        //public BooleanSearchFilter ConfirmedAddressFiltering { get; set; }
        public BooleanSearchFilter ContainsPhysicalAddressFiltering { get; set; }
        public DateTimeSearchRange WorkedOnSearchRange { get; set; }
        public DateTimeSearchRange LetterSentSearchRange { get; set; }
        public TextSearchMode NotesSearchPhraseMode { get; set; }
        public string Notes { get; set; }
        public TextSearchMode AdminNotesSearchPhraseMode { get; set; }
        public string AdminNotes { get; set; }
        public int? VisitingPublisherId { get; set; }
        public int? MapPageNumber { get; set; }
        public string MapGrid { get; set; }
    }

    internal class InternalSearchAddressRequest
    {
        public InternalSearchAddressRequest(SearchAddressRequest r)
        {
            Func<object, string> s = Utils.ToFormString;
            var dateFormat = "M/d/yyyy";

            coords = string.Empty;
            ternum = s(r.TerritoryId);
            tername = s(r.TerritoryName);
            contact = s(r.HouseholderName);
            Lang = r.Language?.ToDescriptiveString() ?? "ALLLANG";
            BusRes = r.AddressType?.ToOptionValue() ?? string.Empty;
            housenum = s(r.AddressNumber);
            street = s(r.StreetName);
            crosStreet = s(r.CrossStreet);
            city = s(r.City);
            zip = s(r.PostalCode);
            phone = s(r.Telephone);
            religion = s(r.Religion);
            DNC = r.DoNotCallFiltering.ToOptionValue();
            LGDog = r.LockedGate_DogFiltering.ToOptionValue();
            //notworked = r.WorkedAddressFiltering == BooleanSearchFilter.ExcludeAll ? "on" : string.Empty;
            //worked = r.WorkedAddressFiltering == BooleanSearchFilter.IncludeOnly ? "on" : string.Empty;
            //confirmed = r.ConfirmedAddressFiltering == BooleanSearchFilter.IncludeOnly ? "on" : string.Empty;
            //unconfirmed = r.ConfirmedAddressFiltering == BooleanSearchFilter.ExcludeAll ? "on" : string.Empty;
            stempty = r.ContainsPhysicalAddressFiltering.ToOptionValue();
            if(r.WorkedOnSearchRange is not null)
            {
                startdate = r.WorkedOnSearchRange.Min.ToString(dateFormat);
                enddate = r.WorkedOnSearchRange.Max.ToString(dateFormat);
            }
            else
            {
                startdate = enddate = string.Empty;
            }
            if (r.LetterSentSearchRange is not null)
            {
                ltrstartdate = r.LetterSentSearchRange.Min.ToString(dateFormat);
                ltrenddate = r.LetterSentSearchRange.Max.ToString(dateFormat);
            }
            else
            {
                ltrstartdate = ltrenddate = string.Empty;
            }
            noteflag = r.NotesSearchPhraseMode.ToOptionValue();
            notes = s(r.Notes);
            noteoflag = r.AdminNotesSearchPhraseMode.ToOptionValue();
            notesother = s(r.AdminNotes);
            rvuser = s(r.VisitingPublisherId);
            mappage = s(r.MapPageNumber);
            mapgrid = s(r.MapGrid);
            sort = "Address";
            Search = "Search";
            MapStat = "Map Not Loaded";
        }

        public string coords { get; }
        public string ternum { get; }
        public string tername { get; }
        public string mappage { get; }
        public string mapgrid { get; }
        public string housenum { get; }
        public string street { get; }
        public string city { get; }
        public string zip { get; }
        public string Lang { get; }
        public string BusRes { get; }
        public string contact { get; }
        public string crosStreet { get; }
        public string noteflag { get; }
        public string notes { get; }
        public string religion { get; }
        public string noteoflag { get; }
        public string notesother { get; }
        public string phone { get; }
        public string startdate { get; }
        public string enddate { get; }
        public string ltrstartdate { get; }
        public string ltrenddate { get; }
        public string LGDog { get; }
        public string DNC { get; }
        public string stempty { get; }
        public string rvuser { get; }
        //public string notworked { get; }
        //public string worked { get; }
        //public string confirmed { get; }
        //public string unconfirmed { get; }
        public string sort { get; }
        public string Search { get; }
        public string MapStat { get; }
    }
}
