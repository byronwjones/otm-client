using System.Net;

namespace BWJ.Web.OTM.Exceptions
{
    public class StatusCodeUnexpectedException : HttpException
    {
        public StatusCodeUnexpectedException(HttpStatusCode statusCode, string location, string responseBody) :
            base(statusCode, location, responseBody: responseBody)
        { }
    }
}
