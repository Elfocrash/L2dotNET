namespace L2dotNET.Network.serverpackets
{
    class RestartResponse : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x5f);
            WriteInt(0x01);
        }
    }
}