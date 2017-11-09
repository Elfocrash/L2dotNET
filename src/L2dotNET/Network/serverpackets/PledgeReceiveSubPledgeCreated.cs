namespace L2dotNET.Network.serverpackets
{
    class PledgeReceiveSubPledgeCreated : GameserverPacket
    {
        
        public PledgeReceiveSubPledgeCreated()
        {
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x40);
        }
    }
}