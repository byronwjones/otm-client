using BWJ.Web.OTM.Models.Internal;
using System;

namespace BWJ.Web.OTM.Models
{
    public class TerritoryAssignmentInfo
    {
        internal TerritoryAssignmentInfo(TerritoryFolderInfo fi)
        {
            AssignmentId = fi.AssignmentId;
            AssignedTo = fi.AssignedTo;
            DateAssigned = fi.DateAssigned;
            PercentCompletedExcludingNotAtHomes = fi.PercentCompletedExcludingNotAtHomes;
            PercentCompletedIncludingNotAtHomes = fi.PercentCompletedIncludingNotAtHomes;
            TerritoryType = fi.TerritoryType;
            RouteState = fi.RouteState;
            Notes = fi.Notes;
        }

        public int AssignmentId { get; }
        public string AssignedTo { get; }
        public DateTime DateAssigned { get; }
        public double PercentCompletedExcludingNotAtHomes { get; }
        public double PercentCompletedIncludingNotAtHomes { get; }
        public string TerritoryType { get; }
        public TerritoryRouteState RouteState { get; }
        public string Notes { get; }
    }
}
