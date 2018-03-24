using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class MultiSellChoose : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _listId;
        private readonly int _entryId;
        private readonly int _amount;
        private readonly short _enchant;
        private readonly int _unk2;
        private readonly int _unk3;

        public MultiSellChoose(Packet packet, GameClient client)
        {
            _client = client;
            _listId = packet.ReadInt();
            _entryId = packet.ReadInt();
            _amount = packet.ReadInt();
            if (_amount < 0)
                _amount = 1;
            _enchant = packet.ReadShort();
            _unk2 = packet.ReadInt();
            _unk3 = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            
        }
    }
}