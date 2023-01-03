using BWJ.Web.OTM.Models;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class OtmUser : IOtmUser
    {
		public string UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string PhoneNumber { get; set; }
		public Language Language { get; set; }
	}
}
