namespace L2dotNET.Network.serverpackets
{
    class AgitDecoInfo : GameserverPacket
    {
        public AgitDecoInfo()
        {
        }

        public override void Write()
        {
            WriteByte(0xf7);
        }
    }
}