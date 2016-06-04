using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2send
{
    class QuestList : GameServerNetworkPacket
    {
        private List<QuestInfo> list;
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
