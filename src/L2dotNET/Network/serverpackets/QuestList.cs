using L2dotNET.Models.player;

namespace L2dotNET.Network.serverpackets
{
    class QuestList : GameserverPacket
    {
       
        public QuestList(L2Player player)
        {

        }

        public override void Write()
        {
            WriteByte(0x86);
            //WriteShort((short)_list.Count);

            //foreach (QuestInfo qi in _list)
            //{
            //    WriteInt(qi.Id);
            //    WriteInt(qi.Stage);
            //}

            //WriteBytesArray(new byte[128]);
        }
    }
}