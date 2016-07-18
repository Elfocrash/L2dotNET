using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class ObserverReturn : PacketBase
    {
        private readonly GameClient _client;

        public ObserverReturn(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SendPacket(new ObservationReturn(player.Obsx, player.Obsy, player.Obsz));
        }
    }
}