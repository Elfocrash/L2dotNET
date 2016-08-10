namespace L2dotNET.Network.serverpackets
{
    class CharCreateOk : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x19);
            WriteInt(0x01);
        }
    }
}