using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BWJ.Web.OTM.Models
{
    public enum Country
	{
		[WebOption("", "")]
		Unknown,

		[WebOption("USA", "USA")]
		USA,
		[WebOption("CANADA", "Canada")]
		Canada,
		[WebOption("AUSTRALIA", "Australia")]
		Australia,
		[WebOption("EUROPE", "Europe")]
		Europe,
		[WebOption("ITALY", "Italy")]
		Italy,
		[WebOption("NEWZEALAND", "New Zealand")]
		New_Zealand,
		[WebOption("CHILE", "Chile")]
		Chile,
		[WebOption("ARGENTINA", "Argentina")]
		Argentina,
		[WebOption("BRAZIL", "Brazil")]
		Brazil,
		[WebOption("ALBANIA", "Albania")]
		Albania,
		[WebOption("DOMINICANREPUB", "Dominican Republic")]
		Dominican_Republic,
	}

	public static class CountryUtils
	{
		public static Country FromValue(string value)
			=> Utils.OptionValueToEnumValue<Country>(value);
	}
}
