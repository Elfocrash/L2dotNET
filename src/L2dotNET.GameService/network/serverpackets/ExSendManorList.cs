using System.Collections.Generic;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExSendManorList : GameserverPacket
    {
        private readonly List<string> _list;

        public ExSendManorList(List<string> list)
        {
            _list = list;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0x1B);
            WriteInt(_list.Count);

            int id = 1;
            foreach (string manor in _list)
            {
                WriteInt(id);
                id++;
                WriteString(manor);
            }
        }
    }
}