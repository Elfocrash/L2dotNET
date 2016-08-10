using System.Collections.Generic;
using L2dotNET.model.player;
using L2dotNET.model.quests;

namespace L2dotNET.Network.serverpackets
{
    class QuestList : GameserverPacket
    {
        private readonly List<QuestInfo> _list;

        public QuestList(L2Player player)
        {
            _list = player.GetAllActiveQuests();
        }

        public override void Write()
        {
            WriteByte(0x86);
            WriteShort((short)_list.Count);

            foreach (QuestInfo qi in _list)
            {
                WriteInt(qi.Id);
                WriteInt(qi.Stage);
            }

            WriteBytesArray(new byte[128]);
        }
    }
}