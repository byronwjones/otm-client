using AngleSharp.Dom;
using BWJ.Net.Http.RequestBuilderExtensions.Html;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Caching;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Internal.Models;
using BWJ.Web.OTM.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic.Tools
{
    public interface IGroupTopic
    {
        Task<IOrganization> GetSignedInOrganization(string session, bool bypassCache = false);
    }

    internal class GroupTopic : IGroupTopic
    {
        private readonly OtmHttpClient _client;

        internal GroupTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task<IOrganization> GetSignedInOrganization(string session, bool bypassCache = false)
        {
            if (bypassCache == false)
            {
                var cachedOrganization = cache[session];
                if (cachedOrganization is not null)
                {
                    return cachedOrganization;
                }
            }

            var html = await _client
                .Get("GroupPref")
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var formSelector = "#mainform2";
            var form = html.QuerySelector(formSelector);
            if (form is null)
            {
                throw new HtmlParsingException($"Expected form element with selector '{formSelector}'");
            }

            var organization = ToOrganization(form);
            cache[session] = organization;
            return organization;
        }

        private static IOrganization ToOrganization(IElement e)
        {
            var org = new Organization();
            try
            {
                org.OrganizationId = GetOrganizationId(e);

                var txtOrganizationType = Utils.GetSelectedValue(e, "GroupType");
                org.OrganizationType = OrganizationTypeUtils.FromValue(txtOrganizationType);
                if (org.OrganizationType == OrganizationType.Unknown)
                {
                    throw new HtmlParsingException($"Encountered unknown organization type value '{txtOrganizationType}'");
                }

                org.Description = Utils.GetValueById(e, "GroupDescr");
                org.ContactPerson = Utils.GetValueById(e, "GroupContact");
                org.ContactPhoneNumber = Utils.GetValueById(e, "GroupPhoneNum");
                org.ContactEmail = Utils.GetValueById(e, "GroupEmail");
                org.OrganizationLanguage = Utils.GetValueById(e, "GroupLang");
                org.Address = Utils.GetValueById(e, "GroupKHAddr");
                org.Latitude = GetDecimalValue(e, "GroupKHLat", "latitude");
                org.Longitude = GetDecimalValue(e, "GroupKHLong", "longitude");
                org.CityAndJurisdiction = Utils.GetValueById(e, "GroupMainCityState");

                var txtCountry = Utils.GetSelectedValue(e, "GroupLocal");
                org.Country = CountryUtils.FromValue(txtCountry);
                if (org.Country == Country.Unknown)
                {
                    throw new HtmlParsingException($"Encountered unknown country value '{txtCountry}'");
                }

                org.BigMapBy = Utils.GetSelectedValue(e, "BigMapByZipCity");
                org.CustomColumn1 = Utils.GetValueById(e, "GroupCustomCol1CS");
                org.CustomColumn2 = Utils.GetValueById(e, "GroupCustomCol2Rel");

                var additionalLanguages = new List<Language>();
                var additionalLanguagesElement = Utils.GetChildElementById(e, "CongLangs[]");
                var selectedLanguages = additionalLanguagesElement.QuerySelectorAll("option");
                foreach (var languageElement in selectedLanguages)
                {
                    var txtLanguage = Utils.GetValue(languageElement);
                    var language = LanguageUtils.FromValue(txtLanguage);
                    if (language == Language.UnsupportedLanguage)
                    {
                        throw new HtmlParsingException($"Encountered unsupported language '{txtLanguage}'");
                    }

                    additionalLanguages.Add(language);
                }
                org.AdditionalOrganizationLanguages = additionalLanguages;
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse organization information", ex);
            }

            return org;
        }

        private static decimal GetDecimalValue(IElement form, string childElementId, string valueDescription)
        {
            var txtValue = Utils.GetValueById(form, childElementId);
            if (decimal.TryParse(txtValue, out var number))
            {
                return number;
            }
            else
            {
                throw new HtmlParsingException($"Value for {valueDescription} '{txtValue}' cannot be converted into a number");
            }
        }

        private static int GetOrganizationId(IElement formElement)
        {
            var legend = formElement.QuerySelector("legend");
            if (legend is null)
            {
                throw new HtmlParsingException($"Expected <legend> element");
            }
            var legendContent = WebUtility.HtmlDecode(legend.TextContent) ?? string.Empty;
            var match = Regex.Match(legendContent, @"[0-9]+\s*\)\s*$");
            if (match.Success == false)
            {
                throw new HtmlParsingException($"Expected the first <legend> element within the form content to contain the organization ID within parenthesis");
            }

            return Convert.ToInt32(match.Value.Replace(")", string.Empty));
        }

        private ResponseCache<IOrganization> cache = new ResponseCache<IOrganization>(10);
    }
}
