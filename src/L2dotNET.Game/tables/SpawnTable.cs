using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.tables
{
    public class SpawnTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SpawnTable));
        private static volatile SpawnTable instance;
        private static object syncRoot = new object();

        public static SpawnTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SpawnTable();
                        }
                    }
                }

                return instance;
            }
        }

        public void Initialize()
        {
            foreach (string path in Directory.EnumerateFiles(@"scripts\spawn\", "*.xml"))
                Read(path);

            log.Info("SpawnTable: Created " + territorries.Count + " territories with " + npcs + " monsters.");
        }

        public readonly SortedList<string, L2Territory> territorries = new SortedList<string, L2Territory>();
        public readonly List<L2Spawn> Spawns = new List<L2Spawn>();
        public SpawnTable()
        {

        }

        private long npcs = 0;
        public void Read(string path)
        {
            XElement xml = XElement.Parse(File.ReadAllText(path));
            XElement ex = xml.Element("list");
            foreach (var m in ex.Elements())
            {
                if (m.Name == "territory")
                {
                    L2Territory zone = new L2Territory();
                    zone.name = m.Attribute("name").Value;
                    zone.controller = m.Attribute("controller").Value;
                    zone.start_active = bool.Parse(m.Attribute("start_active").Value);

                    foreach (var stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "npc":
                                int cnt = Convert.ToInt32(stp.Attribute("count").Value);
                                string pos = null;
                                if (stp.Attribute("pos") != null)
                                    pos = stp.Attribute("pos").Value;
                                zone.AddNpc(Convert.ToInt32(stp.Attribute("id").Value), cnt, stp.Attribute("respawn").Value, pos);
                                npcs += cnt;
                                break;
                            case "zone":
                                zone.AddPoint(stp.Attribute("loc").Value.Split(' '));
                                break;
                        }
                    }

                    zone.InitZone(); //создаем зону
                    if (territorries.ContainsKey(zone.name))
                        log.Info($"duplicate zone name { zone.name }");
                    else
                        territorries.Add(zone.name, zone);
                }
                else if (m.Name == "spawn")
                {
                    foreach (var stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "npc":
                                {
                                    string respawn = stp.Attribute("respawn").Value;
                                    long value = Convert.ToInt32(respawn.Remove(respawn.Length - 1));
                                    if (respawn.Contains("s"))
                                        value *= 1000;
                                    else if (respawn.Contains("m"))
                                        value *= 60000;
                                    else if (respawn.Contains("h"))
                                        value *= 3600000;
                                    else if (respawn.Contains("d"))
                                        value *= 86400000;

                                    Spawns.Add(new L2Spawn(Convert.ToInt32(stp.Attribute("id").Value), value, stp.Attribute("pos").Value.Split(' ')));
                                }
                                npcs++;
                                break;

                        }
                    }
                }
            }
        }

        bool nospawn = true;
        public void Spawn()
        {
            log.Info("NpcServer spawn init.");
            if (nospawn)
            {
                log.Info("NpcServer spawn done (blocked).");
                return;
            }
            long sp = 0;
            foreach (L2Territory t in territorries.Values)
            {
                sp += t.spawns.Count;
                t.Spawn();
            }

            sp += Spawns.Count;
            foreach (L2Spawn s in Spawns)
                s.init();

            log.Info("NpcServer spawn done, #"+sp+" npcs.");
        }

        public void SunRise(bool y)
        {
            foreach (L2Territory t in territorries.Values)
                t.SunRise(y);

            foreach (L2Spawn s in Spawns)
                s.SunRise(y);
        }

        public L2Object SpawnOne(int id, int x, int y, int z, int h)
        {
            NpcTemplate template = NpcTable.Instance.GetNpcTemplate(id);

            L2Warrior o = new L2Warrior();
            o.setTemplate(template);
            //switch (template._type)
            //{
            //    case NpcTemplate.L2NpcType.warrior:
            //    case NpcTemplate.L2NpcType.zzoldagu:
            //    case NpcTemplate.L2NpcType.herb_warrior:
            //    case NpcTemplate.L2NpcType.boss:
            //        o = new L2Warrior();
            //        ((L2Warrior)o).setTemplate(template);
            //        break;

            //    default:
            //        o = new L2Citizen();
            //        ((L2Citizen)o).setTemplate(template);
            //        break;
            //}
            o.X = x;
            o.Y = y;
            o.Z = z;
            o.Heading = h;

            o.SpawnX = x;
            o.SpawnY = y;
            o.SpawnZ = z;

            L2World.Instance.RealiseEntry(o, null, true);
            o.onSpawn();

            return o;
        }
    }
}
