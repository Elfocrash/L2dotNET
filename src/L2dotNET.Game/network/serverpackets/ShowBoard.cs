using System.Collections.Generic;

namespace L2dotNET.Game.network.l2send
{
    class ShowBoard : GameServerNetworkPacket
    {
        private string id;
        private string htmlCode;
        private List<string> arg;
        private static short BBS_MAX = 8180;
        public ShowBoard(string htm, string id)
        {
            this.id = id;
            this.htmlCode = htm;
        }

        public ShowBoard(List<string> arg)
        {
            this.id = "1002";
            this.htmlCode = null;
            this.arg = arg;
        }

        public static void separateAndSend(string html, L2Player player)
        {
            if (html.Length < BBS_MAX)
            {
                player.sendPacket(new ShowBoard(html, "101"));
                player.sendPacket(new ShowBoard(null, "102"));
                player.sendPacket(new ShowBoard(null, "103"));
            }
            else if (html.Length < BBS_MAX * 2)
            {
                player.sendPacket(new ShowBoard(html.Remove(BBS_MAX), "101"));
                player.sendPacket(new ShowBoard(html.Substring(BBS_MAX), "102"));
                player.sendPacket(new ShowBoard(null, "103"));
            }
            else if (html.Length < BBS_MAX * 3)
            {
                player.sendPacket(new ShowBoard(html.Remove(BBS_MAX), "101"));
                player.sendPacket(new ShowBoard(html.Substring(BBS_MAX).Remove(BBS_MAX), "102"));
                player.sendPacket(new ShowBoard(html.Substring(BBS_MAX * 2), "103"));
            }
        }

        protected internal override void write()
        {
            writeC(0x6e);
            writeC(0x01); // c4 1 to show community 00 to hide
            writeS("bypass _bbshome"); // top
            writeS("bypass _bbsgetfav"); // favorite
            writeS("bypass _bbsloc"); // region
            writeS("bypass _bbsclan"); // clan
            writeS("bypass _bbsmemo"); // memo
            writeS("bypass _maillist_0_1_0_"); // mail
            writeS("bypass _friendlist_0_"); // friends
            writeS("bypass bbs_add_fav"); // add fav.

            string st = id + "\u0008";
            if (!id.Equals("1002"))
                st += htmlCode;
            else
                foreach (string s in arg)
                    st += s + " \u0008";
            writeS(st);
        }
    }
}
