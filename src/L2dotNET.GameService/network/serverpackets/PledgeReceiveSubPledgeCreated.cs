using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeReceiveSubPledgeCreated : GameServerNetworkPacket
    {
        private readonly e_ClanSub sub;

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