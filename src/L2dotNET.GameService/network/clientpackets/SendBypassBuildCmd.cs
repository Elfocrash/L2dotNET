using L2dotNET.GameService.Handlers;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class SendBypassBuildCmd : GameServerNetworkRequest
    {
        public SendBypassBuildCmd(GameClient client, byte[] data)
        {
            makeme(client, data);
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