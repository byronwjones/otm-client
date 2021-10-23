using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Http;
using BWJ.Web.OTM.Models.Request.Authentication;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic
{
    public sealed class AuthenticationTopic
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
                .SendAsync();

            var codeFamily = ((int)response.StatusCode / 100) * 100;
            // we only get a redirect when login succeeded
            if (codeFamily == 300)
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
            else if (codeFamily == 200)
            {
                return null; // login failed
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new StatusCodeUnexpectedException(response.StatusCode, response.RequestMessage.RequestUri.ToString(), body);
            }
        }

        private string ParseSessionCookie(string cookie)
        {
            return cookie.Remove(cookie.IndexOf(';'))
                .Split('=')[1];
        }
    }
}
