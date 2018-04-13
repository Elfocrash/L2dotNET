using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestDispel : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _ownerId;
        private readonly int _skillId;
        private readonly int _skillLv;

        public RequestDispel(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _ownerId = packet.ReadInt();
            _skillId = packet.ReadInt();
            _skillLv = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (_ownerId != player.ObjId)
            {
                player.SendActionFailed();
                return;
            }
            
        }
    }
}