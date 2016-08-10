namespace L2dotNET.Network.serverpackets
{
    class FriendList : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xfa);
            WriteShort(0x00);
            WriteShort(0x00);
        }
    }
}