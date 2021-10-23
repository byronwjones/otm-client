using BWJ.Net.Http.RequestBuilder;

namespace BWJ.Web.OTM.Http
{
    public static class HttpRequestBuilderExtensions
    {
        public static HttpRequestBuilder IncludeSession(this HttpRequestBuilder builder, string session)
        {
            if (!string.IsNullOrWhiteSpace(session))
            {
                builder.IncludeHeader("Cookie", $"PHPSESSID={session}");
            }

            return builder;
        }
    }
}
