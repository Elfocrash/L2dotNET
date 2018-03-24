namespace L2dotNET.Network.serverpackets
{
    class MultiSellListEx : GameserverPacket
    {
       
        public override void Write()
        {
            WriteByte(0xd0);
        }
    }
}