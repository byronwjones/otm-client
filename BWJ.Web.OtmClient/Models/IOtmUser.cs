namespace BWJ.Web.OTM.Models
{
    public interface IOtmUser
    {
		string UserId { get; }
		string FirstName { get; }
		string LastName { get; }
		string Email { get; }
		string UserName { get; }
		string PhoneNumber { get; }
		Language Language { get; }
	}
}
