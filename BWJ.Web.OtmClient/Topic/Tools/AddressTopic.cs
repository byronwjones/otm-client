using AngleSharp.Dom;
using BWJ.Net.Http.RequestBuilderExtensions.Html;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Internal.Models;
using BWJ.Web.OTM.Models;
using BWJ.Web.OTM.Models.Request.Tools.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic.Tools
{
    public interface IAddressTopic
    {
        Task<int> CreateAddress(CreateAddressRequest request, string session);
        Task<IAddress> GetAddress(int addressId, string session);
        Task<IEnumerable<IAddressSearchResult>> SearchAddresses(SearchAddressRequest request, string session);
        Task UpdateAddress(IEnumerable<UpdateAddressRequest> request, string session);
        Task UpdateAddress(UpdateAddressRequest request, string session);
    }

    internal class AddressTopic : IAddressTopic
    {
        private readonly OtmHttpClient _client;

        internal AddressTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task<IAddress> GetAddress(int addressId, string session)
        {
            var post = new GetAddressRequest(addressId);

            var html = await _client
                .Post("AddrSearch3")
                .UrlEncodedForm(post)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var formSelector = "form#mainform2 .well";
            var data = html.QuerySelector(formSelector);

            var address = GetAddressFromForm(data, addressId) as Address;

            // search for the address retrieved
            var telephone = string.IsNullOrWhiteSpace(address.Telephone) == false ? address.Telephone :
                string.IsNullOrWhiteSpace(address.TelephoneOther) == false ? address.TelephoneOther :
                string.IsNullOrWhiteSpace(address.CellPhone) == false ? address.CellPhone :
                null;
            var searchMatch = (await SearchAddresses(new SearchAddressRequest { 
                AddressNumber = address.AddressNumber,
                StreetName = address.StreetName,
                Telephone = telephone,
            }, session))
            .FirstOrDefault(x => x.AddressId == addressId);
            address.DateLastWorked = searchMatch?.LastWorked;

            return address;
        }

        public async Task<int> CreateAddress(CreateAddressRequest request, string session)
        {
            var religion = request.Religion;
            var tempId = Guid.NewGuid().ToString("N");
            request.Religion = tempId;

            var post = new InternalCreateAddressRequest(request);

            await _client
                .Post("AdminSingleAddr")
                .UrlEncodedForm(post)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendAsync();

            var idSearchResults = await SearchAddresses(new SearchAddressRequest { Religion = tempId }, session);
            if (idSearchResults.Any() == false)
            {
                throw new Exception("Address creation unsuccessful -- confirmation lookup failed");
            }
            var addressId = idSearchResults.First().AddressId;

            var updateRequest = ToUpdateAddressRequest(addressId, request);
            updateRequest.Religion = religion;
            await UpdateAddress(updateRequest, session);

            return addressId;
        }

        public async Task<IEnumerable<IAddressSearchResult>> SearchAddresses(SearchAddressRequest request, string session)
        {
            var post = new InternalSearchAddressRequest(request);

            var html = await _client
                .Post("AddrSearch2")
                .UrlEncodedForm(post)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var tableRowSelector = "form#mainform2 table#rowclick1 > tbody > tr";
            var data = html.QuerySelectorAll(tableRowSelector);
            try
            {
                return data.Select(d => GetSearchResultTableRow(d)).ToList();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse address search results list", ex);
            }
        }

        public async Task UpdateAddress(IEnumerable<UpdateAddressRequest> request, string session)
        {
            var post = new InternalUpdateAddressRequest(request);

            await _client
                .Post("AddrSearch3")
                .UrlEncodedForm(post)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.Redirect)
                .SendAsync();
        }

        public async Task UpdateAddress(UpdateAddressRequest request, string session)
            => await UpdateAddress(new UpdateAddressRequest[] { request }, session);

        private static IAddress GetAddressFromForm(IElement formElement, int addressId)
        {
            Func<string, string> getVal = (name) => Utils.GetValueByName(formElement, $"{name}[{addressId}]");
            Func<string, IElement> getElement = (name) => Utils.GetChildElementByName(formElement, $"{name}[{addressId}]");
            Func<string, string> getSelection = (name) => Utils.GetSelectedValue(getElement(name));

            var address = new Address { AddressId = addressId };
            ApplyAddressMetadata(formElement, address);

            var territory = getElement("TerID");
            address.TerritoryId = Convert.ToInt32(Utils.GetSelectedValue(territory));

            address.Business_ComplexName = getVal("BusOrComplexName");
            address.HouseholderName = getVal("Name");

            var language = getSelection("Lang");
            address.Language = LanguageUtils.FromDescription(language);

            var addressType = getSelection("BusRes");
            address.AddressType = AddressTypeUtils.FromValue(addressType);

            address.AddressNumber = getVal("Addr");
            address.StreetCardinalDirectionName = getVal("Dir");
            address.StreetName = getVal("Street");
            address.Apartment = getVal("Apt");
            address.City = getVal("City");
            address.Jurisdiction = getVal("State");
            address.PostalCode = getVal("Zip");

            address.Telephone = getVal("Phone");
            address.CellPhone = getVal("PhoneCell");
            address.TelephoneOther = getVal("PhoneOther");

            address.Email = getVal("Email1");
            address.Religion = getVal("Religion");
            address.CrossStreet = getVal("CrosStreet");

            address.AddressIsConfirmed = getSelection("Confirmed") == "1";
            address.IsDoNotCall = getSelection("DNC") == "1";
            address.HasLockedGate_Dog = getSelection("LGD") == "1";

            address.DateLetterSent = Utils.GetDate(getVal("LetterDateSent"), "Date Letter Sent", optional: true);
            address.LetterSubject_Description = getVal("LetterInfo");

            address.Notes = Utils.GetText(getElement("Notes"), "Notes", optional: true);
            address.AdminNotes = getVal("NotesOther");

            var visitingPublisherId = getSelection("RVBBSUserID") ?? string.Empty;
            address.VisitingPublisherId = Regex.IsMatch(visitingPublisherId, @"^[0-9]+$") ?
                Convert.ToInt32(visitingPublisherId) : null;

            {
                var txtLatitude = getVal("Latitude");
                if (string.IsNullOrWhiteSpace(txtLatitude) == false)
                {
                    address.Latitude = double.TryParse(txtLatitude, out var latitude) ? latitude :
                        throw new HtmlParsingException("Expected numerical value in the Latitude field");
                }
            }
            {
                var txtLongitude = getVal("Longitude");
                if (string.IsNullOrWhiteSpace(txtLongitude) == false)
                {
                    address.Longitude = double.TryParse(txtLongitude, out var longitude) ? longitude :
                        throw new HtmlParsingException("Expected numerical value in the Longitude field");
                }
            }

            {
                var txtPageNumber = getVal("TBMPageNum");
                if (string.IsNullOrWhiteSpace(txtPageNumber) == false)
                {
                    address.MapPageNumber = int.TryParse(txtPageNumber, out var pageNumber) ? pageNumber :
                        throw new HtmlParsingException("Expected numerical value in the Map Page Number field");
                }
            }
            address.MapGrid = getVal("TBMGrid");

            return address;
        }

        private static void ApplyAddressMetadata(IElement formElement, Address address)
        {
            var innerFormSelector = ".row > .col-sm-9";
            var innerForm = formElement.QuerySelector(innerFormSelector);
            if (innerForm is null)
            {
                throw new HtmlParsingException($"Expected selection {innerForm} within the form element");
            }
            var labelEndRegex = @"<\/\s*label[^>]*>";
            var metadataHtml = innerForm.InnerHtml;
            var snippetStart = Regex.Match(metadataHtml, labelEndRegex);
            if (snippetStart.Success == false)
            {
                throw new HtmlParsingException("Expected at least one <label /> element within the form content area");
            }
            var snippetEnd = Regex.Match(metadataHtml, @"<\s*div[^>]+>");
            if (snippetEnd.Success == false)
            {
                throw new HtmlParsingException("Expected at least one <div /> element within the form content area");
            }
            var startIndex = snippetStart.Index + snippetStart.Value.Length;
            var metadataSnippet = metadataHtml.Substring(startIndex, snippetEnd.Index - startIndex);
            metadataSnippet = WebUtility.HtmlDecode(metadataSnippet);
            var dateMatch = Regex.Match(metadataSnippet, @"[^<]*");
            var dateValue = dateMatch.Success ? dateMatch.Value.Trim() : string.Empty;
            address.DateAdded = Utils.GetDate(dateValue, "Date Added") ?? default;
            snippetEnd = Regex.Match(metadataSnippet, labelEndRegex);
            if (snippetEnd.Success == false)
            {
                throw new HtmlParsingException("Expected to encounter a second label element closing tag within the form content area");
            }
            address.Source = metadataSnippet.Substring(snippetEnd.Index + snippetEnd.Value.Length).Trim();
        }

        private static int GetAddressIdFromTable(IElement cellElement)
        {
            var strAddressId = Utils.GetValueById(cellElement, "TerAddrRec[]");
            if (string.IsNullOrEmpty(strAddressId))
            {
                throw new HtmlParsingException("Did not find value for address ID");
            }
            if (int.TryParse(strAddressId, out var addressId))
            {
                return addressId;
            }
            else
            {
                throw new HtmlParsingException($"Encountered invalid address ID value '{strAddressId}'");
            }
        }

        private static IAddressSearchResult GetSearchResultTableRow(IElement e)
        {
            var address = new AddressSearchResult();
            try
            {
                var cells = e.QuerySelectorAll("td");
                if (cells.Length != 15)
                {
                    throw new HtmlParsingException($"Expected 15 cells per row, got {cells.Length}");
                }

                address.AddressId = GetAddressIdFromTable(cells[0]);
                address.TerritoryName = Utils.GetText(cells[1], "Territory Name cell");
                address.HouseholderName = Utils.GetText(cells[2], "Householder Name cell", optional: true);
                var arrAddress = Utils.GetText(cells[3], "Address cell", optional: true).Split('-');
                address.Address = arrAddress[0].Trim();
                if (arrAddress.Length > 1 && string.IsNullOrWhiteSpace(arrAddress[1]) == false)
                {
                    address.Apartment = arrAddress[1].Trim();
                }
                var cityStateZip = Utils.GetText(cells[4], "City/Jurisdiction/Postal Code cell", optional: true);
                if (cityStateZip.Contains(","))
                {
                    var divideAt = cityStateZip.LastIndexOf(',');
                    address.City = cityStateZip.Substring(0, divideAt).Trim();
                    address.Jurisdiction_PostalCode = cityStateZip.Substring(divideAt + 1).Trim();
                }
                else
                {
                    address.City = cityStateZip.Trim();
                }
                address.VisitingPublisher = Utils.GetText(cells[5], "Visiting Publisher Name cell", optional: true);
                address.AddressType = AddressTypeUtils.FromValue(Utils.GetText(cells[6], "Address Type cell"));
                address.Telephone = Utils.GetText(cells[7], "Telephone cell", optional: true);
                address.LastWorked = Utils.GetDate(cells[8], "Date Last Worked cell", optional: true);
                address.CheckedOutBy = Utils.GetText(cells[9], "Checked Out By cell", optional: true);
                address.IsDoNotCall = Utils.GetBoolean(cells[10], "Do Not Call cell").GetValueOrDefault();
                address.AddressIsConfirmed = Utils.GetBoolean(cells[11], "Do Not Call cell").GetValueOrDefault();
                address.Language = LanguageUtils.FromDescription(Utils.GetText(cells[12], "Language cell"));
                address.Notes = Utils.GetText(cells[13], "Notes cell", optional: true);
                address.HasLockedGate_Dog = Utils.GetBoolean(cells[14], "Locked Gate / Dog  cell").GetValueOrDefault();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse address search result row", ex);
            }

            return address;
        }

        private static UpdateAddressRequest ToUpdateAddressRequest(int addressId, CreateAddressRequest r)
            => new UpdateAddressRequest
            {
                AddressId = addressId,
                AddressIsConfirmed = r.AddressIsConfirmed,
                AddressNumber = r.AddressNumber,
                AddressType = r.AddressType,
                Apartment = r.Apartment,
                Business_ComplexName = r.Business_ComplexName,
                CellPhone = r.CellPhone,
                City = r.City,
                CrossStreet = r.CrossStreet,
                HasLockedGate_Dog = r.HasLockedGate_Dog,
                HouseholderName = r.HouseholderName,
                Email = r.Email,
                IsDoNotCall = r.IsDoNotCall,
                Jurisdiction = r.Jurisdiction,
                Language = r.Language,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                MapGrid = r.MapGrid,
                MapPageNumber = r.MapPageNumber,
                Notes = r.Notes,
                PostalCode = r.PostalCode,
                Religion = r.Religion,
                StreetCardinalDirectionName = r.StreetCardinalDirectionName,
                StreetName = r.StreetName,
                Telephone = r.Telephone,
                TelephoneOther = r.TelephoneOther,
                TerritoryId = r.TerritoryId,
            };
    }
}
