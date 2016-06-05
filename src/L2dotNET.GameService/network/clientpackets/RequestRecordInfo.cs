using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Clientpackets
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
                player.onAddObject(obj, null, "Player " + player.Name + " recording replay with your character.");
            }
        }
    }
}