using BWJ.Web.OTM.Internal.Http;

namespace BWJ.Web.OTM.Topic.Tools
{
    public interface IToolsTopic
    {
        IAddressTopic Address { get; }
        IAdminTopic Admin { get; }
        IGroupTopic Group { get; }
        IUserTopic Users { get; }
    }

    internal class ToolsTopic : IToolsTopic
    {
        private readonly OtmHttpClient _client;

        internal ToolsTopic(OtmHttpClient client)
        {
            _client = client;
            Admin = new AdminTopic(_client);
            Group = new GroupTopic(_client);
            Users = new UserTopic(_client, Group);
            Address = new AddressTopic(_client);
        }

        public IAdminTopic Admin { get; }
        public IUserTopic Users { get; }
        public IGroupTopic Group { get; }
        public IAddressTopic Address { get; }
    }
}
