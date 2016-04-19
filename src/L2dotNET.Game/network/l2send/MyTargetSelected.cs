using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class MyTargetSelected : GameServerNetworkPacket
    {
        private int _targetId;
        private short _color;

        public MyTargetSelected(int target, int color)
        {
            _targetId = target;
            _color = (short)color;
        }

        protected internal override void write()
        {
            writeC(0xb9);
            writeD(_targetId);
            writeH(_color);
            writeD(0x00);
        }
    }
}
