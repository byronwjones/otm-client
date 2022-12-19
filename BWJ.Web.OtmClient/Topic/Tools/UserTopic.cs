using AngleSharp.Dom;
using BWJ.Net.Http.RequestBuilder;
using BWJ.Net.Http.RequestBuilderExtensions.Html;
using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Internal.Models;
using BWJ.Web.OTM.Models;
using BWJ.Web.OTM.Models.Request.Tools.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic.Tools
{
    public interface IUserTopic
    {
        Task CreateUser(CreateUserRequest request, string session);
        Task<IOtmUser> GetSignedInUser(string session);
        Task<IEnumerable<IOtmUser>> GetUsers(string session);
        Task<bool> IsUsernameUnique(string username, string session);
    }

    internal class UserTopic : IUserTopic
    {
        private readonly OtmHttpClient _client;
        private readonly IGroupTopic _groupTopic;

        internal UserTopic(OtmHttpClient client, IGroupTopic groupTopic)
        {
            _client = client;
            _groupTopic = groupTopic;
        }

        public async Task<IOtmUser> GetSignedInUser(string session)
        {
            var html = await _client
                .Get("UserPref")
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var formSelector = ".container-fluid form.form-horizontal";
            var data = html.QuerySelector(formSelector);
            if (data is null)
            {
                throw new HtmlParsingException($"Expected form element with selector '{formSelector}'");
            }

            return ToUser(data);
        }

        public async Task<IEnumerable<IOtmUser>> GetUsers(string session)
        {
            var html = await _client
                .Get("Users")
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForHtmlAsync();

            var tableRowSelector = "#rowclick1 > tbody > tr";
            var data = html.QuerySelectorAll(tableRowSelector);
            try
            {
                return data.Select(d => GetUserFromTableRow(d)).ToList();
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse OTM users list", ex);
            }
        }

        public async Task CreateUser(CreateUserRequest request, string session)
        {
            var usernameUnique = await IsUsernameUnique(request.UserName, session);
            if(usernameUnique == false)
            {
                throw new InvalidOperationException("Username must be unique");
            }

            var post = new InternalCreateUserRequest(request);
            post.GroupID = (await _groupTopic.GetSignedInOrganization(session)).OrganizationId;

            await _client
                .Post("Users")
                .UrlEncodedForm(post)
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendAsync();

            var users = await GetUsers(session);
            if(users.Any(u => u.UserName == request.UserName) == false)
            {
                throw new HttpException(HttpStatusCode.BadRequest, "Users.php", "User was not created on group account");
            }
        }

        public async Task<bool> IsUsernameUnique(string username, string session)
        {
            var response = (await _client
                .Get("checkuser")
                .IncludeQuery(new IsUsernameUniqueQuery { q = username })
                .IncludeSession(session)
                .AcceptStatusCodes(HttpStatusCode.OK)
                .SendForTextAsync())
                ?.ToLower() ?? string.Empty;

            if ((new string[] { "yes", "no" }).Contains(response) == false)
            {
                throw new Exception($"Unrecognized response '{response}' returned from server");
            }

            return response == "yes"; // 'no' means no user by this name exists
        }

        private static IOtmUser GetUserFromTableRow(IElement e)
        {
            var user = new OtmUser();
            try
            {
                var cells = e.QuerySelectorAll("td");
                if (cells.Length != 10)
                {
                    throw new HtmlParsingException($"Expected 10 cells per row, got {cells.Length}");
                }

                user.UserId = GetUserIdFromTable(cells[0]);
                user.UserType = GetUserTypeFromTable(cells[1]);
                user.FullName = Utils.GetText(cells[2], "Full Name cell");
                user.Email = Utils.GetText(cells[3], "Email cell");
                user.PhoneNumber = Utils.GetText(cells[4], "Phone Number cell", optional: true);
                user.UserName = Utils.GetText(cells[7], "User Name cell");
                if (int.TryParse(Utils.GetText(cells[8], "Max Number of Territories Allowed to Check Out cell"), out var maxTerr))
                {
                    user.MaxNumberOfTerritoriesAllowed = maxTerr;
                }
                else
                {
                    throw new HtmlParsingException("Expected number in Max Number Allowed to Check Out cell");
                }
                var publicWitnessingApproved = Utils.GetText(cells[9], "Public Witnessing cell").ToLower();
                user.PublicWitnessingApproved = publicWitnessingApproved == "yes" ? true :
                    publicWitnessingApproved == "no" ? false : throw new HtmlParsingException("Unexpected value for public witnessing approved");
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse checked out territories list row", ex);
            }

            return user;
        }

        private static OtmUserType GetUserTypeFromTable(IElement cellElement)
        {
            var txtType = Utils.GetText(cellElement, "User Type cell");
            switch (txtType)
            {
                case "General":
                    return OtmUserType.General;
                case "Publisher":
                    return OtmUserType.Publisher;
                case "Group Admin":
                    return OtmUserType.GroupAdmin;
                case "Read-Only":
                    return OtmUserType.ReadOnly;
                case "Mobile-Only":
                    return OtmUserType.MobileOnly;
                default:
                    throw new HtmlParsingException($"Unknown user type '{txtType}' encountered");
            }
        }

        private static string GetUserIdFromTable(IElement cellElement)
        {
            var controlName = "UserID";
            var control = cellElement.QuerySelector($"input[name='{controlName}']");
            if (control is null)
            {
                throw new HtmlParsingException($"Expected input element with name '{controlName}'");
            }

            var userId = WebUtility.HtmlDecode(control.GetAttribute("value"));
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new HtmlParsingException($"Expected input with name '{controlName}' to contain a value");
            }

            return userId;
        }

        private static IOtmUser ToUser(IElement e)
        {
            var user = new OtmUser();
            try
            {
                user.UserId = Utils.GetValueById(e, "UserID");
                user.FirstName = Utils.GetValueById(e, "Firstname");
                user.LastName = Utils.GetValueById(e, "Lastname");
                user.Email = Utils.GetValueById(e, "EmailAddress");
                user.UserName = Utils.GetValueById(e, "Username");
                user.PhoneNumber = Utils.GetValueById(e, "PhoneNum");
                user.Language = GetLanguage(e);
            }
            catch (Exception ex)
            {
                throw new HtmlParsingException("Failed to parse user information", ex);
            }

            return user;
        }

        private static Language GetLanguage(IElement formElement)
        {
            var txtLanguage = Utils.GetSelectedValue(formElement, "UserLangID");
            var language = LanguageUtils.FromValue(txtLanguage);
            if (language == Language.UnsupportedLanguage)
            {
                throw new HtmlParsingException($"Encountered unsupported language '{txtLanguage}'");
            }

            return language;
        }
    }
}
