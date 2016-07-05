using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestRecordInfo : GameServerNetworkRequest
    {
        public RequestRecordInfo(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            player.SendPacket(new UserInfo(player));
            player.SendPacket(new ExBrExtraUserInfo(player.ObjId, player.AbnormalBitMaskEvent));

            foreach (L2Object obj in player.KnownObjects.Values)
            {
                player.OnAddObject(obj, null, "Player " + player.Name + " recording replay with your character.");
            }
        }
    }
}