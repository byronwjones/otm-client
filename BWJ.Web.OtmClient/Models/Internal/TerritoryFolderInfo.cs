using System;

namespace BWJ.Web.OTM.Models.Internal
{
    internal class TerritoryFolderInfo
    {
        public int AssignmentId { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public DateTime DateAssigned { get; set; }
        public double PercentCompletedExcludingNotAtHomes { get; set; }
        public double PercentCompletedIncludingNotAtHomes { get; set; }
        public string TerritoryType { get; set; }
        public TerritoryRouteState RouteState { get; set; }
        public string Notes { get; set; }
    }
}
