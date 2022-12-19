using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Topic;
using BWJ.Web.OTM.Topic.Tools;

namespace BWJ.Web.OTM
{
    public sealed class OtmClient
    {
        private readonly OtmHttpClient client = new OtmHttpClient();
        public OtmClient()
        {
            Authentication = new AuthenticationTopic(client);
            Tools = new ToolsTopic(client);
            Territory = new TerritoryTopic(client);
        }

        public IAuthenticationTopic Authentication { get; }
        public IToolsTopic Tools { get; }
        public ITerritoryTopic Territory { get; }
    }
}
