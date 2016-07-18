namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExChangeNicknameNColor : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0x83);
        }
    }
}