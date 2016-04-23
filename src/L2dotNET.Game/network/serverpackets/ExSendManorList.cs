using System.Collections.Generic;

namespace L2dotNET.Game.network.l2send
{
    class ExSendManorList : GameServerNetworkPacket
    {
        private List<string> _list;
        public ExSendManorList(List<string> list)
        {
            _list = list;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0x1B);
            writeD(_list.Count);

            int id = 1;
            foreach (string manor in _list)
            {
                writeD(id); id++;
                writeS(manor);
            }
        }
    }
}
