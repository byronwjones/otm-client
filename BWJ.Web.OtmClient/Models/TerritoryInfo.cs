using BWJ.Web.OTM.Models.Internal;
using System;

namespace BWJ.Web.OTM.Models
{
    public class TerritoryInfo
    {
        internal TerritoryInfo(TerritoryListInfo li, TerritoryFolderInfo fi)
        {
            Id = li.Id;
            Name = li.Name;
            Description = li.Description;
            AvailableAddressCount = li.AvailableAddressCount;
            AddressCount = li.AddressCount;
            ConfirmedAddressCount = li.ConfirmedAddressCount;
            LastWorked = li.LastWorked;
            LastCheckIn = li.LastCheckIn;
            AssignmentInfo = fi != null ? new TerritoryAssignmentInfo(fi) : null;
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int AvailableAddressCount { get; }
        public int AddressCount { get; }
        public int ConfirmedAddressCount { get; }
        public DateTime? LastWorked { get; }
        public DateTime? LastCheckIn { get; }

        public TerritoryAssignmentInfo AssignmentInfo { get; }
    }
}
