using Newtonsoft.Json;

namespace BWJ.Web.OTM.Models.Request.Territory
{
    internal class DownloadTerritoryQuery
    {
        [JsonProperty("MyTerID")]
        public int TerritoryAssignmentId { get; set; }
        public int Sort { get; } = -1;
        public int First { get; } = 1;
    }
}
