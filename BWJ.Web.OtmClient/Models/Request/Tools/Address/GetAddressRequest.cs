using System.Collections.Generic;

namespace BWJ.Web.OTM.Models.Request.Tools.Address
{
    internal class GetAddressRequest
    {
        public GetAddressRequest(int addressId)
        {
            TerAddrRec = new int[] { addressId };
        }

        public string edit { get; } = "Edit";

        public IEnumerable<int> TerAddrRec { get; }
    }
}
