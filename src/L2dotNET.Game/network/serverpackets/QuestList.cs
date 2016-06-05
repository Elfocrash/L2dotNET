using System.Collections.Generic;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.quests;

namespace L2dotNET.GameService.network.serverpackets
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