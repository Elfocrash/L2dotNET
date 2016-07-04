using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class ObserverReturn : GameServerNetworkRequest
    {
        public ObserverReturn(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // not actions
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.SendPacket(new ObservationReturn(player.Obsx, player.Obsy, player.Obsz));
        }
    }
}