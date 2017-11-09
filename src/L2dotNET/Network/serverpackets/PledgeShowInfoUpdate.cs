namespace L2dotNET.Network.serverpackets
{
    class PledgeShowInfoUpdate : GameserverPacket
    {
        
        public PledgeShowInfoUpdate()
        {
        }

        public override void Write()
        {
            WriteByte(0x8e);
        }
    }
}