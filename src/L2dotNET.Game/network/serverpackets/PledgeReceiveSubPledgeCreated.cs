using L2dotNET.Game.model.communities;

namespace L2dotNET.Game.network.l2send
{
    class PledgeReceiveSubPledgeCreated : GameServerNetworkPacket
    {
        private e_ClanSub sub;
        public PledgeReceiveSubPledgeCreated(e_ClanSub sub)
        {
            this.sub = sub;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x40);

            writeD(0x01);
            writeD((short)sub.Type);
            writeS(sub.Name);
            writeS(sub.LeaderName);
        }
    }
}
