using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShowBoard : GameServerNetworkPacket
    {
        private readonly string _id;
        private readonly string _htmlCode;
        private readonly List<string> _arg;
        private const short BbsMax = 8180;

        public ShowBoard(string htm, string id)
        {
            this._id = id;
            _htmlCode = htm;
        }

        public ShowBoard(List<string> arg)
        {
            _id = "1002";
            _htmlCode = null;
            this._arg = arg;
        }

        public static void SeparateAndSend(string html, L2Player player)
        {
            if (html.Length < BbsMax)
            {
                player.SendPacket(new ShowBoard(html, "101"));
                player.SendPacket(new ShowBoard(null, "102"));
                player.SendPacket(new ShowBoard(null, "103"));
            }
            else if (html.Length < BbsMax * 2)
            {
                player.SendPacket(new ShowBoard(html.Remove(BbsMax), "101"));
                player.SendPacket(new ShowBoard(html.Substring(BbsMax), "102"));
                player.SendPacket(new ShowBoard(null, "103"));
            }
            else if (html.Length < BbsMax * 3)
            {
                player.SendPacket(new ShowBoard(html.Remove(BbsMax), "101"));
                player.SendPacket(new ShowBoard(html.Substring(BbsMax).Remove(BbsMax), "102"));
                player.SendPacket(new ShowBoard(html.Substring(BbsMax * 2), "103"));
            }
        }

        protected internal override void Write()
        {
            WriteC(0x6e);
            WriteC(0x01); // c4 1 to show community 00 to hide
            WriteS("bypass _bbshome"); // top
            WriteS("bypass _bbsgetfav"); // favorite
            WriteS("bypass _bbsloc"); // region
            WriteS("bypass _bbsclan"); // clan
            WriteS("bypass _bbsmemo"); // memo
            WriteS("bypass _maillist_0_1_0_"); // mail
            WriteS("bypass _friendlist_0_"); // friends
            WriteS("bypass bbs_add_fav"); // add fav.

            string st = _id + "\u0008";
            if (!_id.EqualsIgnoreCase("1002"))
                st += _htmlCode;
            else
                foreach (string s in _arg)
                    st += s + " \u0008";

            WriteS(st);
        }
    }
}