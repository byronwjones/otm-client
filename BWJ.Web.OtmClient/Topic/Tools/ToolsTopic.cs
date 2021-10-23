using BWJ.Web.OTM.Http;

namespace BWJ.Web.OTM.Topic.Tools
{
    public sealed class ToolsTopic
    {
        private readonly OtmHttpClient _client;

        internal ToolsTopic(OtmHttpClient client)
        {
            _client = client;
            Admin = new AdminTopic(_client);
        }

        public AdminTopic Admin { get; }
    }
}
