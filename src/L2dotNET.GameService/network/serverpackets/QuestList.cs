using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class QuestList : GameserverPacket
    {
        private readonly List<QuestInfo> _list;

        public QuestList(L2Player player)
        {
            _list = player.GetAllActiveQuests();
        }

        protected internal override void Write()
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