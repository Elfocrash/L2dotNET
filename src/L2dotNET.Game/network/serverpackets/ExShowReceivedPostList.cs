using L2dotNET.Game.managers;
using System.Collections.Generic;
using L2dotNET.Game.tools;

namespace L2dotNET.Game.network.l2send
{
    class ExShowReceivedPostList : GameServerNetworkPacket
    {
        private List<MailMessage> list;

        public ExShowReceivedPostList(List<MailMessage> list)
        {
            this.list = list;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0xaa);
            writeD(Utilz.CurrentSeconds());
            writeD(list.Count);

            foreach (MailMessage Mail in list)
            {
                writeD(Mail.MailID);
                writeS(Mail.Title);
                writeS(Mail.SenderName);
                writeD(Mail.Trade);
                writeD(Mail.getExpirationSeconds());
                writeD(Mail.NotOpend);
                writeD(Mail.ReturnAble);
                writeD(Mail.WithItem);
                writeD(Mail.SentBySystem);
                writeD(Mail._news); //??
                writeD(0);
            }
        }
    }
}
