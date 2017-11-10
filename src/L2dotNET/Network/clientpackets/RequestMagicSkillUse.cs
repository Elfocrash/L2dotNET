using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets
{
    class RequestMagicSkillUse : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _magicId;
        private readonly bool _ctrlPressed;
        private readonly bool _shiftPressed;

        public RequestMagicSkillUse(Packet packet, GameClient client)
        {
            _client = client;
            _magicId = packet.ReadInt(); // Identifier of the used skill
            _ctrlPressed = packet.ReadInt() != 0; // True if it's a ForceAttack : Ctrl pressed
            _shiftPressed = packet.ReadByte() != 0; // True if Shift pressed
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}