using System.Collections.Generic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSendManorList : GameServerNetworkPacket
    {
        private readonly List<string> _list;

        public ExSendManorList(List<string> list)
        {
            _list = list;
        }

        protected internal override void Write()
        {
            WriteC(0xFE);
            WriteH(0x1B);
            WriteD(_list.Count);

            int id = 1;
            foreach (string manor in _list)
            {
                WriteD(id);
                id++;
                WriteS(manor);
            }
        }
    }
}