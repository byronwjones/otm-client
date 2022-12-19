using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Models.Request.Authentication;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic
{
    public interface IAuthenticationTopic
    {
        Task<string> SignInUser(string username, string password);
    }

    internal class AuthenticationTopic : IAuthenticationTopic
    {
        private readonly OtmHttpClient _client;

        internal AuthenticationTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task<string> SignInUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(nameof(username));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            var response = await _client
                .Post("login")
                .Form(new LoginRequest
                {
                    username = username,
                    password = password
                })
                .AcceptStatusCodes(HttpStatusCode.Redirect, HttpStatusCode.OK)
                .SendAsync();

            // we only get a redirect when login succeeded
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var sessionCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                if (sessionCookie is null)
                {
                    throw new SessionCookieHeaderNotFoundException();
                }
                if (Regex.IsMatch(sessionCookie, @"^PHPSESSID=[a-z0-9]+;") == false)
                {
                    throw new SessionCookieUnexpectedFormatException();
                }

                return ParseSessionCookie(sessionCookie);
            }
            else
            {
                return null; // login failed
            }
        }

        private string ParseSessionCookie(string cookie)
        {
            return cookie.Remove(cookie.IndexOf(';'))
                .Split('=')[1];
        }
    }
}
