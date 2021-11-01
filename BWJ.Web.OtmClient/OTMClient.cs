using BWJ.Web.OTM.Http;
using BWJ.Web.OTM.Topic;
using BWJ.Web.OTM.Topic.Tools;

namespace BWJ.Web.OTM
{
    public sealed class OTMClient
    {
        private readonly OtmHttpClient client = new OtmHttpClient();
        public OTMClient()
        {
            Authentication = new AuthenticationTopic(client);
            Tools = new ToolsTopic(client);
            Territory = new TerritoryTopic(client);
        }

        public AuthenticationTopic Authentication { get; }
        public ToolsTopic Tools { get; }
        public TerritoryTopic Territory { get; }
    }
}
