using AngleSharp.Dom;
using BWJ.Net.Http.RequestBuilderExtensions.Html;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Http;
using BWJ.Web.OTM.Models;
using BWJ.Web.OTM.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic
{
    public sealed class TerritoryTopic
    {
        private readonly OtmHttpClient _client;

        internal TerritoryTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<TerritoryInfo>> GetTerritoryInfo(string session)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }

            var territories = await GetTerritoryListInfo(session);
            var assignments = await GetTerritoryFolderInfo(session);

            return territories
                .Select(t => new TerritoryInfo(
                    t,
                    assignments.FirstOrDefault(a => a.Description.Contains($"{t.Name} - {t.Description}"))
                    ));
        }

        private async Task<List<TerritoryFolderInfo>> GetTerritoryFolderInfo(string session)
        {
            var html = await _client
                .Get("MyTer")
                .IncludeQuery(new { showallmyter = "1", sort = "1" })
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var data = html.QuerySelectorAll("#mainform2 > table tbody tr");
            try
            {
                return data.Select(d => ToTerritoryFolderInfo(d)).ToList();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse checked out territories list", ex);
            }
        }
        private async Task<List<TerritoryListInfo>> GetTerritoryListInfo(string session)
        {
            var html = await _client
                .Get("GetStandard")
                .IncludeQuery(new { code = "A" })
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var data = html.QuerySelectorAll("#boxleft table tbody tr");
            try
            {
                return data.Select(d => ToTerritoryListInfo(d)).ToList();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse standard territory list", ex);
            }
        }

        private static TerritoryFolderInfo ToTerritoryFolderInfo(IElement e)
        {
            var info = new TerritoryFolderInfo();
            try
            {
                var cells = e.QuerySelectorAll("td");
                if (cells.Length != 14)
                {
                    throw new HtmlParsingException($"Expected 14 cells per row, got {cells.Length}");
                }

                info.AssignmentId = GetTerritoryAssignmentId(cells[0]);
                info.AssignedTo = GetText(cells[1], "assigned publisher cell");
                info.Description = GetText(cells[2], "territory description cell");
                SetCompletedPercentages(cells[3], info);
                info.DateAssigned = GetDate(cells[4], "Date created cell") ??
                    throw new HtmlParsingException("Expected a date in date created cell, but it was empty");
                info.TerritoryType = GetText(cells[5], "territory type cell");
                info.RouteState = GetRouteState(cells[9]);
                info.Notes = cells[13].TextContent?.Trim();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse checked out territories list row", ex);
            }

            return info;
        }
        private static TerritoryListInfo ToTerritoryListInfo(IElement e)
        {
            var info = new TerritoryListInfo();
            try
            {
                var cells = e.QuerySelectorAll("td");
                if (cells.Length != 7)
                {
                    throw new HtmlParsingException($"Expected 7 cells per row, got {cells.Length}");
                }

                var descLink = cells[0].QuerySelector("a");
                if (descLink is null)
                {
                    throw new HtmlParsingException("Expected link in territory name cell");
                }
                info.Name = descLink.InnerHtml;
                info.Id = GetTerritoryId(descLink);
                info.Description = cells[1].InnerHtml?.Trim() ?? string.Empty;
                info.AvailableAddressCount = GetInt(cells[2], "Number available addresses");
                info.AddressCount = GetInt(cells[3], "Number of addresses");
                info.ConfirmedAddressCount = GetInt(cells[4], "Number of confirmed addresses");
                info.LastWorked = GetDate(cells[5], "Last worked date");
                info.LastCheckIn = GetDate(cells[6], "Last check in date");
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse territory list row", ex);
            }

            return info;
        }

        private static TerritoryRouteState GetRouteState(IElement e)
        {
            var content = e.TextContent;
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new HtmlParsingException($"Expected text in route loaded cell");
            }

            if (content.Contains("No"))
            {
                return TerritoryRouteState.NotLoaded;
            }
            if (content.Contains("Yes") && content.Contains("Update Route"))
            {
                return TerritoryRouteState.RequiresUpdate;
            }
            if (content.Contains("Yes"))
            {
                return TerritoryRouteState.Loaded;
            }

            return TerritoryRouteState.Indeterminate;
        }
        private static void SetCompletedPercentages(IElement e, TerritoryFolderInfo info)
        {
            var strPercentages = GetText(e, "percentage completed cell");
            if (false == Regex.IsMatch(strPercentages, @"^[0-9]{1,3}\s*%\s*/\s*[0-9]{1,3}\s*%$"))
            {
                throw new HtmlParsingException($"Completed percentages data in an unexpected format. Expected '[number]% / [number]%', got '{strPercentages}'");
            }

            var pcts = strPercentages.Replace("%", string.Empty).Split('/');
            info.PercentCompletedExcludingNotAtHomes = Convert.ToDouble(pcts[0]);
            info.PercentCompletedIncludingNotAtHomes = Convert.ToDouble(pcts[1]);
        }
        private static int GetTerritoryAssignmentId(IElement e)
        {
            var checkbox = e.QuerySelector("input");
            if (checkbox is null)
            {
                throw new HtmlParsingException("Expected input element in first cell");
            }

            var strId = checkbox.GetAttribute("value");
            if (string.IsNullOrEmpty(strId))
            {
                throw new HtmlParsingException("Expected 'value' attribute on input element in first cell");
            }

            int value;
            if (false == int.TryParse(strId, out value))
            {
                throw new HtmlParsingException($"Input element in first cell's 'value' attribute (territory assignment ID) is in an unexpected format. Expected an integer, got '{strId}'");
            }

            return value;
        }
        private static int GetTerritoryId(IElement e)
        {
            var js = e.GetAttribute("onclick");
            if (string.IsNullOrEmpty(js))
            {
                throw new HtmlParsingException("Expected 'onclick' attribute on territory name link element");
            }

            var jsFunction = js.Split(';')[0].Trim();
            if (false == Regex.IsMatch(js, @"^getTerList\([0-9]+,[0-9]+,[0-9]+,[0-9]+\)"))
            {
                throw new HtmlParsingException($"Territory name link 'onclick' attribute value in an unexpected format. Expected 'getTerList([number],[number],[number],[number])', got '{jsFunction}'");
            }

            var strId = jsFunction
                .Split(',')[0]
                .Substring(jsFunction.IndexOf('(') + 1);

            return Convert.ToInt32(strId);
        }
        private static string GetText(IElement e, string dataDescription)
        {
            var text = WebUtility.HtmlDecode(e.TextContent)?.Trim();
            if (string.IsNullOrEmpty(text))
            {
                throw new HtmlParsingException($"Expected text for element {dataDescription}");
            }

            return text;
        }
        private static int GetInt(IElement e, string dataDescription)
        {
            int value;
            if (false == int.TryParse(WebUtility.HtmlDecode(e.TextContent), out value))
            {
                throw new HtmlParsingException($"{dataDescription} is in an unexpected format. Expected an integer, got '{e.InnerHtml}'");
            }

            return value;
        }
        private static DateTime? GetDate(IElement e, string dataDescription)
        {
            var text = WebUtility.HtmlDecode(e.TextContent)?.Trim();
            if (string.IsNullOrWhiteSpace(text)) { return null; }

            if (false == Regex.IsMatch(text, @"^[0-9]{1,2}/[0-9]{1,2}/([0-9]{2}|[0-9]{4})$"))
            {
                throw new HtmlParsingException($"{dataDescription} is in an unexpected format. Expected 'mm/dd/yy' or 'mm/dd/yyyy', got '{text}'");
            }

            var dateParts = text.Split('/');
            var year = Convert.ToInt32(dateParts[2]);
            return new DateTime(
                year: year < 100 ? (2000 + year) : year,
                month: Convert.ToInt32(dateParts[0]),
                day: Convert.ToInt32(dateParts[1]));
        }
    }
}
