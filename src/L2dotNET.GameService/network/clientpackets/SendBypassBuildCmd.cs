using L2dotNET.GameService.Handlers;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class SendBypassBuildCmd : GameServerNetworkRequest
    {
        public SendBypassBuildCmd(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private string _alias;

        public override void Read()
        {
            _alias = ReadS();
            _alias = _alias.Trim();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            AdminCommandHandler.Instance.Request(player, _alias);
        }
    }
}