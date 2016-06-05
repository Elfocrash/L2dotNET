using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.tables.admin_bypass;

namespace L2dotNET.GameService.tables.ndextend
{
    class NDTeleport
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NDTeleport));
        public SortedList<int, ab_teleport_npc> npcs = new SortedList<int, ab_teleport_npc>();

        public NDTeleport()
        {
            reload();
        }

        private void reload()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\nd_teleports.xml"));
            XElement ex = xml.Element("list");
            foreach (var m in ex.Elements())
            {
                if (m.Name == "npc")
                {
                    ab_teleport_npc npc = new ab_teleport_npc();
                    npc.id = int.Parse(m.Attribute("id").Value);

                    foreach (var x in m.Elements())
                    {
                        if (x.Name == "group")
                        {
                            ab_teleport_group ab = new ab_teleport_group();
                            ab.id = int.Parse(x.Attribute("id").Value);

                            foreach (var e in x.Elements())
                            {
                                if (e.Name == "e")
                                {
                                    ab_teleport_entry ae = new ab_teleport_entry();
                                    ae.name = e.Attribute("name").Value;
                                    ae.x = int.Parse(e.Attribute("x").Value);
                                    ae.y = int.Parse(e.Attribute("y").Value);
                                    ae.z = int.Parse(e.Attribute("z").Value);
                                    ae.id = ab._teles.Count;

                                    if (e.Attribute("cost") != null)
                                        ae.cost = long.Parse(e.Attribute("cost").Value);
                                    if (e.Attribute("itemId") != null)
                                        ae.itemId = int.Parse(e.Attribute("itemId").Value);

                                    ab._teles.Add(ae.id, ae);
                                }
                            }

                            npc.groups.Add(ab.id, ab);
                        }
                    }
                    if (npcs.ContainsKey(npc.id))
                        log.Error($"NpcData(Teleporter) dublicate npc str {npc.id}");
                    else
                        npcs.Add(npc.id, npc);
                }
            }

            log.Info("NpcData(Teleporter): loaded " + npcs.Count + " npcs.");
        }
    }

    public class ab_teleport_npc
    {
        public int id;
        public SortedList<int, ab_teleport_group> groups = new SortedList<int, ab_teleport_group>();
    }
}