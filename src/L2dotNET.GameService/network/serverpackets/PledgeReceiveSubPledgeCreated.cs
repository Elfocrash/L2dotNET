using L2dotNET.GameService.Model.Communities;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PledgeReceiveSubPledgeCreated : GameServerNetworkPacket
    {
        private readonly EClanSub _sub;

        public PledgeReceiveSubPledgeCreated(EClanSub sub)
        {
            this._sub = sub;
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x40);

            WriteD(0x01);
            WriteD((short)_sub.Type);
            WriteS(_sub.Name);
            WriteS(_sub.LeaderName);
        }
    }
}