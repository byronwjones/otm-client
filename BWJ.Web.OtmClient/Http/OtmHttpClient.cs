using BWJ.Net.Http;
using BWJ.Net.Http.RequestBuilder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BWJ.Web.OTM.Http
{
    internal class OtmHttpClient
    {
        public OtmHttpClient()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false,
                AllowAutoRedirect = false
            };

            var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri("https://onlineterritorymanager.com");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            httpClient.DefaultRequestHeaders.Add("Origin", "https://onlineterritorymanager.com");
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(".NET Standard", "2.0"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(System.Net.Http.HttpClient)"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("OTM Http Client", "0.1.0"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(+https://github.com/byronwjones/otm-client)"));

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/avif"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/apng"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/signed-exchange", 0.9));

            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("en-US"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("en", 0.9));

            client = new FluentHttpClient(httpClient);
        }

        public HttpRequestWithContentBuilder Post(string endpoint)
        {
            endpoint = NormalizeEndpoint(endpoint);
            return client.Post(endpoint);
        }

        public HttpRequestWithQueryBuilder Get(string endpoint)
        {
            endpoint = NormalizeEndpoint(endpoint);
            return client.Get(endpoint);
        }

        private string NormalizeEndpoint(string endpoint) => endpoint.IndexOf('.') == -1 ? endpoint + ".php" : endpoint;

        private readonly FluentHttpClient client;
    }
}
