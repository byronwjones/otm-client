using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BWJ.Web.OTM.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode statusCode, string location, string message = null, string responseBody = null) :
            base(string.IsNullOrWhiteSpace(message) ? Decamelize(statusCode.ToString()) : message)
        {
            StatusCode = statusCode;
            Location = location;
            ResponseBody = responseBody;
        }

        public HttpStatusCode StatusCode { get; }
        public string Location { get; }
        public string ResponseBody { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Location))
            {
                sb.AppendLine($"Location: {Location}");
            }

            sb.AppendLine($"Status Code: {(int)StatusCode} ({StatusCode})");

            if (!string.IsNullOrWhiteSpace(ResponseBody))
            {
                sb.AppendLine($"Response Body: {ResponseBody}");
            }

            sb.AppendLine(base.ToString());

            return sb.ToString();
        }

        private static string Decamelize(string s)
        {
            var sb = new StringBuilder();
            var firstLetter = true;
            foreach (var c in s)
            {
                var letter = c;
                if (char.IsUpper(letter) && firstLetter == false)
                {
                    sb.Append(' ');
                    letter = char.ToLower(c);
                }

                sb.Append(letter);
                firstLetter = false;
            }

            return sb.ToString();
        }
    }
}
