namespace L2dotNET.Network.serverpackets
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