using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Player;
using L2dotNET.Utility;

namespace L2dotNET.Network.serverpackets
{
    class ShowBoard : GameserverPacket
    {
        private readonly string _id;
        private readonly string _htmlCode;
        private readonly List<string> _arg;
        private const short BbsMax = 8180;

        public ShowBoard(string htm, string id)
        {
            _id = id;
            _htmlCode = htm;
        }

        public ShowBoard(List<string> arg)
        {
            _id = "1002";
            _htmlCode = null;
            _arg = arg;
        }

        public static void SeparateAndSend(string html, L2Player player)
        {
            if (html.Length < BbsMax)
            {
                player.SendPacketAsync(new ShowBoard(html, "101"));
                player.SendPacketAsync(new ShowBoard(null, "102"));
                player.SendPacketAsync(new ShowBoard(null, "103"));
            }
            else
            {
                if (html.Length < (BbsMax * 2))
                {
                    player.SendPacketAsync(new ShowBoard(html.Remove(BbsMax), "101"));
                    player.SendPacketAsync(new ShowBoard(html.Substring(BbsMax), "102"));
                    player.SendPacketAsync(new ShowBoard(null, "103"));
                }
                else
                {
                    if (html.Length >= (BbsMax * 3))
                        return;

                    player.SendPacketAsync(new ShowBoard(html.Remove(BbsMax), "101"));
                    player.SendPacketAsync(new ShowBoard(html.Substring(BbsMax).Remove(BbsMax), "102"));
                    player.SendPacketAsync(new ShowBoard(html.Substring(BbsMax * 2), "103"));
                }
            }
        }

        public override void Write()
        {
            WriteByte(0x6e);
            WriteByte(0x01); // c4 1 to show community 00 to hide
            WriteString("bypass _bbshome"); // top
            WriteString("bypass _bbsgetfav"); // favorite
            WriteString("bypass _bbsloc"); // region
            WriteString("bypass _bbsclan"); // clan
            WriteString("bypass _bbsmemo"); // memo
            WriteString("bypass _maillist_0_1_0_"); // mail
            WriteString("bypass _friendlist_0_"); // friends
            WriteString("bypass bbs_add_fav"); // add fav.

            string st = $"{_id}\u0008";
            if (!_id.EqualsIgnoreCase("1002"))
                st += _htmlCode;
            else
                st += string.Join(string.Empty, _arg.Select(s => $"{s} \u0008").ToArray());

            WriteString(st);
        }
    }
}