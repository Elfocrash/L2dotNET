namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExChangeNicknameNColor : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0x83);
        }
    }
}