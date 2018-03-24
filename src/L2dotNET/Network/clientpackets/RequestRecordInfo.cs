using L2dotNET.Models;
using L2dotNET.Models.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.Network.clientpackets
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
                player.OnAddObject(obj, null, $"Player {player.Name} recording replay with your character.");
        }
    }
}