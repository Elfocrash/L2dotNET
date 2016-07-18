using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestSocialAction : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _actionId;

        public RequestSocialAction(Packet packet, GameClient client)
        {
            _client = client;
            _actionId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            if (player == null)
                return;

            if ((_actionId < 2) || (_actionId > 13))
                return;

            player.BroadcastPacket(new SocialAction(player.ObjId, _actionId));
        }
    }
}