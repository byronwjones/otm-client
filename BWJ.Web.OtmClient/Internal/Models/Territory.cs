using BWJ.Web.OTM.Models;
using System;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class Territory : ITerritory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AvailableAddressCount { get; set; }
        public int AddressCount { get; set; }
        public int ConfirmedAddressCount { get; set; }
        public DateTime? LastWorked { get; set; }
        public DateTime? LastCheckIn { get; set; }

        public ITerritoryAssignment AssignmentInfo { get; set; }
    }
}
