using System;

namespace BWJ.Web.OTM.Models.Request.Territory
{
    internal class CheckOutTerritoryQuery
    {
        public int TerrID { get; set; }
        public int userid { get; set; }
        public int chkout { get; } = 1;
        public int confirm { get; } = 1;
        public double sid { get; } = (new Random()).NextDouble();
    }
}
