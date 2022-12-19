using BWJ.Web.OTM.Models;

namespace BWJ.Web.OTM.Internal.Models
{
    internal class OtmUser : IOtmUser
    {
		public string UserId { get; set; }
		public OtmUserType UserType { get; set; } 
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName
		{
			get
			{
				if(!string.IsNullOrWhiteSpace(fullName))
                {
					return fullName;
                }
				else if(!string.IsNullOrWhiteSpace(FirstName) && 
					!string.IsNullOrWhiteSpace(LastName))
                {
					return $"{FirstName} {LastName}";
                }
				else if(!string.IsNullOrWhiteSpace(FirstName))
                {
					return FirstName;
                }
				else
                {
					return LastName;
                }
			}

            set
            {
				fullName = value;
            }
		}
		public string Email { get; set; }
		public string UserName { get; set; }
		public string PhoneNumber { get; set; }
		public Language Language { get; set; }
		public int? MaxNumberOfTerritoriesAllowed { get; set; }
		public bool? PublicWitnessingApproved { get; set; }

		private string fullName = null;
	}
}
