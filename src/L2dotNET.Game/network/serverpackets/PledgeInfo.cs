using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.GameService.network.l2send
{
    class PledgeInfo : GameServerNetworkPacket
    {
        private int _id;
        private string _name;
        private string _ally;
        public PledgeInfo(int id, string name, string ally)
        {
            _id = id;
            _name = name;
            _ally = ally;
        }

        protected internal override void write()
        {
            writeC(0x89);
            writeD(_id);
            writeS(_name);
            writeS(_ally);
        }
    }
}
