using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class CharFlyToLocation : GameserverPacket
    {
        private readonly L2Character _character;
        private readonly int _flyType;

        public CharFlyToLocation(L2Character character, FlyType flyType)
        {
            _character = character;
            _flyType = (int)flyType;
        }

        public override void Write()
        {
            WriteByte(0xC5);

            WriteInt(_character.ObjectId);

            WriteInt(_character.CharMovement.DestinationX);
            WriteInt(_character.CharMovement.DestinationY);
            WriteInt(_character.CharMovement.DestinationZ);

            WriteInt(_character.X);
            WriteInt(_character.Y);
            WriteInt(_character.Z);

            WriteInt(_flyType);
        }
    }
}