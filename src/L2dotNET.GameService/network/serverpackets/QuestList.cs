using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class QuestList : GameServerNetworkPacket
    {
        private readonly List<QuestInfo> _list;

        public QuestList(L2Player player)
        {
            _list = player.GetAllActiveQuests();
        }

        protected internal override void Write()
        {
            WriteC(0x86);
            WriteH((short)_list.Count);

            foreach (QuestInfo qi in _list)
            {
                WriteD(qi.Id);
                WriteD(qi.Stage);
            }

            WriteB(new byte[128]);
        }
    }
}