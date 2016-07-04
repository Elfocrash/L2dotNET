using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.Tables.Admin_Bypass;

namespace L2dotNET.GameService.Tables.Ndextend
{
    class NdTeleport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NdTeleport));
        public SortedList<int, AbTeleportNpc> Npcs = new SortedList<int, AbTeleportNpc>();

        public NdTeleport()
        {
            Reload();
        }

        private void Reload()
        {
            XElement xml = XElement.Parse(File.ReadAllText(@"scripts\nd_teleports.xml"));
            XElement ex = xml.Element("list");
            if (ex != null)
                foreach (XElement m in ex.Elements())
                    if (m.Name == "npc")
                    {
                        AbTeleportNpc npc = new AbTeleportNpc();
                        npc.Id = int.Parse(m.Attribute("id").Value);

                        foreach (XElement x in m.Elements())
                            if (x.Name == "group")
                            {
                                AbTeleportGroup ab = new AbTeleportGroup();
                                ab.Id = int.Parse(x.Attribute("id").Value);

                                foreach (XElement e in x.Elements())
                                    if (e.Name == "e")
                                    {
                                        AbTeleportEntry ae = new AbTeleportEntry();
                                        ae.Name = e.Attribute("name").Value;
                                        ae.X = int.Parse(e.Attribute("x").Value);
                                        ae.Y = int.Parse(e.Attribute("y").Value);
                                        ae.Z = int.Parse(e.Attribute("z").Value);
                                        ae.Id = ab.Teles.Count;

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

            Log.Info("NpcData(Teleporter): loaded " + Npcs.Count + " npcs.");
        }
    }
}