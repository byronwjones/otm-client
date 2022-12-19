using System;

namespace BWJ.Web.OTM.Models
{
    public interface ITerritoryAssignment
    {
        int AssignmentId { get; }
        string AssignedTo { get; }
        DateTime DateAssigned { get; }
        double PercentCompletedExcludingNotAtHomes { get; }
        double PercentCompletedIncludingNotAtHomes { get; }
        string TerritoryType { get; }
        TerritoryRouteState RouteState { get; }
        string Notes { get; }
    }
}
