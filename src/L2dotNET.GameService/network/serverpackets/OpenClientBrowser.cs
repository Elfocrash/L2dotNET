using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class OpenClientBrowser : GameServerNetworkPacket
    {

        public OpenClientBrowser()
        {
        }

        protected internal override void write()
        {
            writeC(0x70);
            writeS("http://www.google.com");
        }
    }
}
