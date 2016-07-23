using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Tables.Admin_Bypass;

namespace L2dotNET.GameService.Tables.Ndextend
{
    class NDTeleport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NDTeleport));
        public SortedList<int, ABTeleportNpc> Npcs = new SortedList<int, ABTeleportNpc>();

        public NDTeleport()
        {
            Reload();
        }

        private void Reload()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\nd_teleports.xml"));
            XElement ex = xml.Element("list");
            if (ex != null)
            {
                foreach (XElement m in ex.Elements())
                {
                    if (m.Name != "npc")
                        continue;

                    ABTeleportNpc npc = new ABTeleportNpc
                                        {
                                            Id = int.Parse(m.Attribute("id").Value)
                                        };

                    foreach (XElement x in m.Elements())
                    {
                        if (x.Name != "group")
                            continue;

                        ABTeleportGroup ab = new ABTeleportGroup
                                             {
                                                 Id = int.Parse(x.Attribute("id").Value)
                                             };

                        foreach (XElement e in x.Elements())
                        {
                            if (e.Name != "e")
                                continue;

                            ABTeleportEntry ae = new ABTeleportEntry
                                                 {
                                                     Name = e.Attribute("name").Value,
                                                     X = int.Parse(e.Attribute("x").Value),
                                                     Y = int.Parse(e.Attribute("y").Value),
                                                     Z = int.Parse(e.Attribute("z").Value),
                                                     Id = ab.Teles.Count
                                                 };

                            if (e.Attribute("cost") != null)
                                ae.Cost = int.Parse(e.Attribute("cost").Value);
                            if (e.Attribute("itemId") != null)
                                ae.ItemId = int.Parse(e.Attribute("itemId").Value);

                            ab.Teles.Add(ae.Id, ae);
                        }

                        npc.Groups.Add(ab.Id, ab);
                    }

                    if (Npcs.ContainsKey(npc.Id))
                        Log.Error($"NpcData(Teleporter) dublicate npc str {npc.Id}");
                    else
                        Npcs.Add(npc.Id, npc);
                }
            }

            Log.Info($"NpcData(Teleporter): loaded {Npcs.Count} npcs.");
        }
    }
}