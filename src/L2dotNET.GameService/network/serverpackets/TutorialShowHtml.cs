using L2dotNET.GameService.Model.Player;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class TutorialShowHtml : GameServerNetworkPacket
    {
        private string _content;

        public TutorialShowHtml(L2Player player, string file, bool admin)
        {
            Render(player, file, "", admin);
        }

        public TutorialShowHtml(L2Player player, string file, string back, bool admin)
        {
            Render(player, file, back, admin);
        }

        public TutorialShowHtml(L2Player player, string text, string back, bool plain, bool admin)
        {
            RenderPlain(player, text, back, admin);
        }

        public TutorialShowHtml(L2Player player, string text, bool plain, bool admin)
        {
            RenderPlain(player, text, "", admin);
        }

        private void RenderPlain(L2Player player, string text, string back, bool admin)
        {
            if (admin)
            {
                if (back.EqualsIgnoreCase(""))
                {
                    back = "link main.htm";
                }

                _content = "<html><title>Admin Menu</title><table width=270><tr><td width=45><td width=45><button value=\"Back\" action=\"" + back + "\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td>><td width=180><center><td width=45><button value=\"Main\" action=\"link main.htm\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></center></td><td width=45><button value=\"Close\" action=\"link admin_close\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td></tr></table>";
            }
            else
            {
                _content = "<html><body>" + text + "</body></html>";
            }
        }

        private void Render(L2Player player, string file, string back, bool admin)
        {
            //Content = admin ? HtmCache.Instance.getHtmAdmin(player._locale, file) : HtmCache.Instance.getHtm(player._locale, file);

            //if (admin)
            //{
            //    if (back.EqualsIgnoreCase(""))
            //        back = "link main.htm";

            //    replace("<html>", "<html><title>Admin Menu</title><table width=270><tr><td width=45><td width=45><button value=\"Back\" action=\"" + back + "\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td>><td width=180><center><td width=45><button value=\"Main\" action=\"link main.htm\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></center></td><td width=45><button value=\"Close\" action=\"link admin_close\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td></tr></table>");
            //}
        }

        public void Replace(string p, object t)
        {
            _content = _content.Replace(p, t.ToString());
        }

        protected internal override void Write()
        {
            WriteC(0xa0);
            WriteS(_content);
        }
    }
}