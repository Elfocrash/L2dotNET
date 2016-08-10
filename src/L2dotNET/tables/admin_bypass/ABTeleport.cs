using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using log4net;
using L2dotNET.model.player;

namespace L2dotNET.tables.admin_bypass
{
    public class ABTeleport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ABTeleport));
        public SortedList<int, ABTeleportGroup> Groups = new SortedList<int, ABTeleportGroup>();

        public ABTeleport()
        {
            Reload();
        }

        public void Reload()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\admin\abteleport.xml"));
            XElement ex = xml.Element("list");
            if (ex != null)
            {
                foreach (XElement m in ex.Elements())
                {
                    if (m.Name != "group")
                        continue;

                    ABTeleportGroup ab = new ABTeleportGroup
                    {
                        Id = int.Parse(m.Attribute("id").Value),
                        Str = m.Attribute("str").Value,
                        Name = m.Attribute("name").Value,
                        Level = int.Parse(m.Attribute("level").Value)
                    };

                    foreach (ABTeleportEntry ae in m.Elements().Where(e => e.Name == "entry").Select(e => new ABTeleportEntry
                    {
                        Name = e.Attribute("name").Value,
                        X = int.Parse(e.Attribute("x").Value),
                        Y = int.Parse(e.Attribute("y").Value),
                        Z = int.Parse(e.Attribute("z").Value),
                        Id = ab.Teles.Count
                    }))
                        ab.Teles.Add(ae.Id, ae);

                    Groups.Add(ab.Id, ab);
                }
            }

            Log.Info($"AdminPlugin(Teleport): loaded {Groups.Count} groups.");
        }

        public void ShowGroup(L2Player player, int groupId)
        {
            if (!Groups.ContainsKey(groupId))
            {
                player.SendMessage($"teleport group #{groupId} was not found.");
                player.SendActionFailed();
                return;
            }

            ABTeleportGroup gr = Groups[groupId];
            StringBuilder sb = new StringBuilder($"<button value=\"Back\" action=\"bypass -h admin?ask=3&reply=0\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"><center><font color=\"Blue\">Region : </font><font color=\"LEVEL\">{gr.Name}</font><br>");
            foreach (ABTeleportEntry e in gr.Teles.Values)
                sb.Append($"<button value=\"{e.Name}\" action=\"bypass -h admin?ask=2&reply={e.Id}\" width=150 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"><br1>");

            sb.Append("</center>");

            player.ViewingAdminTeleportGroup = gr.Id;
            player.ShowHtmAdmin(sb.ToString(), true);
        }

        public void ShowGroupList(L2Player player)
        {
            StringBuilder sb = new StringBuilder("<center>");
            sb.Append("<font color=\"LEVEL\">Move to the given coordinates</font>");
            sb.Append("<table width=260><tr><td width=15>X:</td><td><edit var=\"char_cord_x\" width=55></td><td width=15>Y:</td><td><edit var=\"char_cord_y\" width=55></td><td width=15>Z:</td><td><edit var=\"char_cord_z\" width=55></td><td><button value=\"Go\" action=\"bypass -h admin?tp $char_cord_x $char_cord_y $char_cord_z\" width=50 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td></tr></table><br1>");
            sb.Append("<font color=\"333333\" align=\"center\">_______________________________________</font><br1>");
            sb.Append("<font color=\"LEVEL\">Choise region teleport</font>");
            sb.Append("<table width=270>");
            sb.Append("<tr>");
            int count = 0;
            foreach (ABTeleportGroup gr in Groups.Values)
            {
                count++;
                sb.Append($"<td><button value=\"{gr.Name}\" action=\"bypass -h admin?ask=1&reply={gr.Id}\" width=135 height=20 back=\"L2UI_ct1.button_df\" fore=\"L2UI_ct1.button_df\"></td>");
                if (count != 2)
                    continue;

                sb.Append("</tr>");
                sb.Append("<tr>");
                count = 0;
            }

            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<font color=\"333333\" align=\"center\">_______________________________________</font>");
            sb.Append("</center>");

            player.ShowHtmAdmin(sb.ToString(), true);
        }

        public void Use(L2Player player, int reply)
        {
            if ((player.ViewingAdminTeleportGroup != -1) && Groups.ContainsKey(player.ViewingAdminTeleportGroup))
                return;

            player.SendMessage($"teleport group #{player.ViewingAdminTeleportGroup} was not found.");
            player.SendActionFailed();
            //return;

            //ab_teleport_group gr = _groups[player.ViewingAdminTeleportGroup];
            //ab_teleport_entry e = gr._teles[reply];
        }
    }
}