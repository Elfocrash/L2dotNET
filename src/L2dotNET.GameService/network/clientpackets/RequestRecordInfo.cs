using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestRecordInfo : PacketBase
    {
        private readonly GameClient _client;

        public RequestRecordInfo(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SendPacket(new UserInfo(player));
            player.SendPacket(new ExBrExtraUserInfo(player.ObjId, player.AbnormalBitMaskEvent));

            foreach (L2Object obj in player.KnownObjects.Values)
                player.OnAddObject(obj, null, "Player " + player.Name + " recording replay with your character.");
        }
    }
}