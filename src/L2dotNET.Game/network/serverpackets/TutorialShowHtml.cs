using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class TutorialShowHtml : GameServerNetworkPacket
    {
        private string Content;
        public TutorialShowHtml(L2Player player, string file, bool admin)
        {
            render(player, file, "", admin);
        }

        public TutorialShowHtml(L2Player player, string file, string back, bool admin)
        {
            render(player, file, back, admin);
        }

        public TutorialShowHtml(L2Player player, string text, string back, bool plain, bool admin)
        {
            renderPlain(player, text, back, admin);
        }

        public TutorialShowHtml(L2Player player, string text, bool plain, bool admin)
        {
            renderPlain(player, text, "", admin);
        }

        private void renderPlain(L2Player player, string text, string back, bool admin)
        {
            if (admin)
            {
                if (back.Equals(""))
                    back = "link main.htm";

                Content = "<html><title>Admin Menu</title><table width=270><tr><td width=45><td width=45><button value=\"Back\" action=\"" + back + "\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td>><td width=180><center><td width=45><button value=\"Main\" action=\"link main.htm\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></center></td><td width=45><button value=\"Close\" action=\"link admin_close\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td></tr></table>";
            }
            else
                Content = "<html><body>" + text + "</body></html>";
        }

        private void render(L2Player player, string file, string back, bool admin)
        {
            Content = admin ? HtmCache.Instance.getHtmAdmin(player._locale, file) : HtmCache.Instance.getHtm(player._locale, file);

            if (admin)
            {
                if (back.Equals(""))
                    back = "link main.htm";

                replace("<html>", "<html><title>Admin Menu</title><table width=270><tr><td width=45><td width=45><button value=\"Back\" action=\"" + back + "\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td>><td width=180><center><td width=45><button value=\"Main\" action=\"link main.htm\" width=45 height=21 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></center></td><td width=45><button value=\"Close\" action=\"link admin_close\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td></tr></table>");
            }
        }

        public void replace(string p, object t)
        {
            Content = Content.Replace(p, t.ToString());
        }

        protected internal override void write()
        {
            writeC(0xa0);
            writeS(Content);
        }
    }
}
