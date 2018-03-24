using L2dotNET.Handlers;
using L2dotNET.Models.player;

namespace L2dotNET.Network.clientpackets
{
    class SendBypassBuildCmd : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _alias;

        public SendBypassBuildCmd(Packet packet, GameClient client)
        {
            _client = client;
            _alias = packet.ReadString().Trim();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            AdminCommandHandler.Instance.Request(player, _alias);
        }
    }
}