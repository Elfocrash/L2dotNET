using System.Collections.Generic;

namespace L2dotNET.Network.serverpackets
{
    class PledgeShowMemberListAll : GameserverPacket
    {
        public PledgeShowMemberListAll()
        {
        }

        public override void Write()
        {
            WriteByte(0x5a);
        }
    }
}