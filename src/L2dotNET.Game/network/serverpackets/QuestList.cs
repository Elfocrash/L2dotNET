using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class QuestList : GameServerNetworkPacket
    {
        private readonly List<QuestInfo> list;

        public QuestList(L2Player player)
        {
            list = player.getAllActiveQuests();
        }

        protected internal override void write()
        {
            writeC(0x86);
            writeH((short)list.Count);

            foreach (QuestInfo qi in list)
            {
                writeD(qi.id);
                writeD(qi.stage);
            }

            writeB(new byte[128]);
        }
    }
}