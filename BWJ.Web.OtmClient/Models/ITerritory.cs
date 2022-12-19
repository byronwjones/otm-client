using System;

namespace BWJ.Web.OTM.Models
{
    public interface ITerritory
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        int AvailableAddressCount { get; }
        int AddressCount { get; }
        int ConfirmedAddressCount { get; }
        DateTime? LastWorked { get; }
        DateTime? LastCheckIn { get; }

        ITerritoryAssignment AssignmentInfo { get; }
    }
}
