using System.Collections.Generic;
using L2dotNET.Game.managers;
using L2dotNET.Game.tools;

namespace L2dotNET.Game.network.l2send
{
    class ExShowSentPostList : GameServerNetworkPacket
    {
        private int id;
        private List<MailMessage> list;

        public ExShowSentPostList(int p, List<MailMessage> list)
        {
            this.id = p;
            this.list = list;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xac);
            writeD(Utilz.CurrentSeconds());
            writeD(list.Count);

            foreach (MailMessage mm in list)
            {
                writeD(mm.MailID);
                writeS(mm.Title);
                writeS(mm.ReceiverName);
                writeD(mm.Trade);
                writeD(mm.getExpirationSeconds());
                writeD(mm.NotOpend);
                writeD(0x01);
                writeD(mm.WithItem);
            }
        }
    }
}
