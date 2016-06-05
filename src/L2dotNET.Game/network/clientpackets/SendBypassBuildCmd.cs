using L2dotNET.GameService.Handlers;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.l2recv
{
    class SendBypassBuildCmd : GameServerNetworkRequest
    {
        public SendBypassBuildCmd(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _alias;

        public override void read()
        {
            _alias = readS();
            _alias = _alias.Trim();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            AdminCommandHandler.Instance.request(player, _alias);
        }
    }
}