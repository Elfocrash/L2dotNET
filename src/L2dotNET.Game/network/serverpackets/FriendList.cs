
using System.Collections.Generic;

namespace L2dotNET.Game.network.l2send
{
    class FriendList : GameServerNetworkPacket
    {

        protected internal override void write()
        {
            writeC(0xfa);
            writeH(0x00);
            writeH(0x00);
        }
    }

}
