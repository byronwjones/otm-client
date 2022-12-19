using AngleSharp.Dom;
using BWJ.Net.Http.RequestBuilder;
using BWJ.Net.Http.RequestBuilderExtensions.Html;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Internal.Models;
using BWJ.Web.OTM.Models;
using BWJ.Web.OTM.Models.Request.Territory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic
{
    public interface ITerritoryTopic
    {
        Task CheckInTerritories(string session, IEnumerable<int> territoryAssignmentIds, int? reassignTo = null);
        Task CheckInTerritory(string session, int territoryAssignmentId, int? reassignTo = null);
        Task CheckOutTerritory(string session, int territoryId, int recipientId);
        Task<Stream> GetTerritoryDocument(string session, int territoryAssignmentId);
        Task<IEnumerable<ITerritory>> GetTerritoryInfo(string session);
    }

    public sealed class TerritoryTopic : ITerritoryTopic
    {
        private readonly OtmHttpClient _client;

        internal TerritoryTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task CheckOutTerritory(string session, int territoryId, int recipientId)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }
            if (territoryId < 1)
            {
                throw new ArgumentException(nameof(territoryId));
            }
            if (recipientId < 1)
            {
                throw new ArgumentException(nameof(recipientId));
            }

            await _client
                .Get("ListStand")
                .IncludeQuery(new CheckOutTerritoryQuery { TerrID = territoryId, userid = recipientId })
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendAsync();
        }

        public async Task CheckInTerritories(string session, IEnumerable<int> territoryAssignmentIds, int? reassignTo = null)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }
            if (territoryAssignmentIds is null ||
                territoryAssignmentIds.Any() == false ||
                territoryAssignmentIds.Any(id => id < 1))
            {
                throw new ArgumentException(nameof(territoryAssignmentIds));
            }

            var request = new CheckInTerritoryRequest { newuserid = reassignTo };
            foreach (var id in territoryAssignmentIds)
            {
                request.MyTerID.Add(id);
            }

            await _client
                .Post("MyTer")
                .Form(request)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendAsync();
        }

        public async Task CheckInTerritory(string session, int territoryAssignmentId, int? reassignTo = null)
            => await CheckInTerritories(session, new List<int> { territoryAssignmentId }, reassignTo);

        public async Task<Stream> GetTerritoryDocument(string session, int territoryAssignmentId)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }
            if (territoryAssignmentId < 1)
            {
                throw new ArgumentException(nameof(territoryAssignmentId));
            }

            return await _client
                .Get("PrintPreviewSimpleR2")
                .IncludeQuery(new DownloadTerritoryQuery { TerritoryAssignmentId = territoryAssignmentId })
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForStreamAsync();
        }

        public async Task<IEnumerable<ITerritory>> GetTerritoryInfo(string session)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }

            var territories = await GetTerritoryListInfo(session);
            var assignments = await GetTerritoryFolderInfo(session);

            return territories
                .Select(t =>
                {
                    t.AssignmentInfo = assignments.FirstOrDefault(a => a.Description.Contains($"{t.Name} - {t.Description}"));
                    return t;
                });
        }

        private async Task<List<TerritoryAssignment>> GetTerritoryFolderInfo(string session)
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
        private async Task<List<Territory>> GetTerritoryListInfo(string session)
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

        private static TerritoryAssignment ToTerritoryFolderInfo(IElement e)
        {
            var info = new TerritoryAssignment();
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
                info.DateAssigned = Utils.GetDate(cells[4], "Date created cell").GetValueOrDefault();
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
        private static Territory ToTerritoryListInfo(IElement e)
        {
            var info = new Territory();
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
                info.LastWorked = Utils.GetDate(cells[5], "Last worked date", optional: true);
                info.LastCheckIn = Utils.GetDate(cells[6], "Last check in date", optional: true);
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
        private static void SetCompletedPercentages(IElement e, TerritoryAssignment info)
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
    }
}
