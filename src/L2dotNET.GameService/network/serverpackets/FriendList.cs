namespace L2dotNET.GameService.Network.Serverpackets
{
    class FriendList : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0xfa);
            WriteShort(0x00);
            WriteShort(0x00);
        }
    }
}