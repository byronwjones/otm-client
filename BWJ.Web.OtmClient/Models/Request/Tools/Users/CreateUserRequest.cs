namespace BWJ.Web.OTM.Models.Request.Tools.Users
{
    public sealed class CreateUserRequest
    {
        public OtmUserType UserType { get; set; } = OtmUserType.General;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool RequirePasswordChange { get; set; }
        public int TerritoryCheckoutMax { get; set; }
        public int UserLanguageId { get; set; }
    }

    internal class InternalCreateUserRequest
    {
        public InternalCreateUserRequest(CreateUserRequest request)
        {
            UserType = request.UserType;
            Firstname = request.FirstName;
            Lastname = request.LastName;
            EmailAddress = request.EmailAddress;
            Username = request.UserName;
            Password = request.Password;
            PhoneNum = request.PhoneNumber;
            ChangePassword = request.RequirePasswordChange ? 1 : 0;
            MaxNumChkOut = request.TerritoryCheckoutMax;
            UserLangID = request.UserLanguageId;
        }

        public int UserID { get => -1; }
        public OtmUserType UserType { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string EmailAddress { get; }
        public string Username { get; }
        public string Password { get; }
        public string VerifyPassword { get => Password; }
        public string PhoneNum { get; }
        public int PWApproved { get => 0; }
        public int GroupID { get; set; }
        public int ChangePassword { get; }
        public int MaxNumChkOut { get; }
        public int UserLangID { get; }
        public string AddNew { get => "Add"; }
    }
}
