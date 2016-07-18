using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExChangeNicknameNColor : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0x83);
        }
    }
}