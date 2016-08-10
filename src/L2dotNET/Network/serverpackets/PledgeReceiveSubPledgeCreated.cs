using L2dotNET.model.communities;

namespace L2dotNET.Network.serverpackets
{
    class PledgeReceiveSubPledgeCreated : GameserverPacket
    {
        private readonly EClanSub _sub;

        public PledgeReceiveSubPledgeCreated(EClanSub sub)
        {
            _sub = sub;
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x40);

            WriteInt(0x01);
            WriteInt((short)_sub.Type);
            WriteString(_sub.Name);
            WriteString(_sub.LeaderName);
        }
    }
}