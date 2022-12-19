using System;

namespace BWJ.Web.OTM.Models.Request.Tools.Users
{
    internal class IsUsernameUniqueQuery
    {
        public string q { get; set; }
        public string id { get; } = "-1";
        public double sid { get; } = (new Random()).NextDouble();
    }
}
