using System.Collections.Generic;

namespace BWJ.Web.OTM.Models.Request.Territory
{
    internal class CheckInTerritoryRequest
    {
        public HashSet<int> MyTerID { get; } = new HashSet<int>();

        public int? newuserid { get; set; }

        public string ChkIn { get; } = "Force Check In";
    }
}
