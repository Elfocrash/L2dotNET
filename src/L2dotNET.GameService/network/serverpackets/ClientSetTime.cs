using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.GameService.Controllers;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ClientSetTime : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0xEC);
            WriteD(GameTime.Instance.Time);
            WriteD(6);
        }
    }
}
