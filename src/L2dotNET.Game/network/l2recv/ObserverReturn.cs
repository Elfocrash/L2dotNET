using L2dotNET.Game.network.l2send;
using System;

namespace L2dotNET.Game.network.l2recv
{
    class ObserverReturn : GameServerNetworkRequest
    {
        public ObserverReturn(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // not actions
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendPacket(new ObservationReturn(player._obsx, player._obsy, player._obsz));
        }
    }
}
