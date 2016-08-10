namespace L2dotNET.Network.serverpackets
{
    class ExShowOwnthingPos : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x93);

            WriteInt(0);
            WriteInt(0);
        }
    }
}