using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestRecordInfo : GameServerNetworkRequest
    {
        public RequestRecordInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            player.sendPacket(new UserInfo(player));
            player.sendPacket(new ExBrExtraUserInfo(player.ObjID, player.AbnormalBitMaskEvent));

            foreach (L2Object obj in player.knownObjects.Values)
            {
                player.onAddObject(obj, null, "Player "+player.Name+" recording replay with your character.");
            }

        }
    }
}
