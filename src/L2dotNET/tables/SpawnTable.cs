using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using log4net;
using L2dotNET.model.npcs;
using L2dotNET.world;

namespace L2dotNET.tables
{
    public class SpawnTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SpawnTable));
        private static volatile SpawnTable _instance;
        private static readonly object SyncRoot = new object();

        public static SpawnTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new SpawnTable();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            foreach (string path in Directory.EnumerateFiles(@"scripts\spawn\", "*.xml"))
                Read(path);

            Log.Info($"SpawnTable: Created {Territorries.Count} territories with {_npcs} monsters.");
        }

        public readonly SortedList<string, L2Territory> Territorries = new SortedList<string, L2Territory>();
        public readonly List<L2Spawn> Spawns = new List<L2Spawn>();

        private long _npcs;

        public void Read(string path)
        {
            XElement xml = XElement.Parse(File.ReadAllText(path));
            XElement ex = xml.Element("list");
            if (ex == null)
                return;

            foreach (XElement m in ex.Elements())
            {
                if (m.Name == "territory")
                {
                    L2Territory zone = new L2Territory
                    {
                        Name = m.Attribute("name").Value,
                        Controller = m.Attribute("controller").Value,
                        StartActive = bool.Parse(m.Attribute("start_active").Value)
                    };

                    foreach (XElement stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "npc":
                                int cnt = Convert.ToInt32(stp.Attribute("count").Value);
                                string pos = null;
                                if (stp.Attribute("pos") != null)
                                    pos = stp.Attribute("pos").Value;
                                zone.AddNpc(Convert.ToInt32(stp.Attribute("id").Value), cnt, stp.Attribute("respawn").Value, pos);
                                _npcs += cnt;
                                break;
                            case "zone":
                                zone.AddPoint(stp.Attribute("loc").Value.Split(' '));
                                break;
                        }
                    }

                    zone.InitZone(); //создаем зону
                    if (Territorries.ContainsKey(zone.Name))
                        Log.Info($"duplicate zone name {zone.Name}");
                    else
                        Territorries.Add(zone.Name, zone);
                }
                else
                {
                    if (m.Name != "spawn")
                        continue;

                    foreach (XElement stp in m.Elements())
                    {
                        switch (stp.Name.LocalName)
                        {
                            case "npc":
                            {
                                string respawn = stp.Attribute("respawn").Value;
                                long value = Convert.ToInt32(respawn.Remove(respawn.Length - 1));
                                if (respawn.Contains("s"))
                                    value *= 1000;
                                else
                                {
                                    if (respawn.Contains("m"))
                                        value *= 60000;
                                    else
                                    {
                                        if (respawn.Contains("h"))
                                            value *= 3600000;
                                        else
                                        {
                                            if (respawn.Contains("d"))
                                                value *= 86400000;
                                        }
                                    }
                                }

                                Spawns.Add(new L2Spawn(Convert.ToInt32(stp.Attribute("id").Value), value, stp.Attribute("pos").Value.Split(' ')));
                            }
                                _npcs++;
                                break;
                        }
                    }
                }
            }
        }

        private const bool Nospawn = true;

        public void Spawn()
        {
            Log.Info("NpcServer spawn init.");
            //if (nospawn)
            //{
            //    log.Info("NpcServer spawn done (blocked).");
            //    return;
            //}

            long sp = 0;
            foreach (L2Territory t in Territorries.Values)
            {
                sp += t.Spawns.Count;
                t.Spawn();
            }

            sp += Spawns.Count;

            Spawns.ForEach(s => s.Init());

            Log.Info($"NpcServer spawn done, #{sp} npcs.");
        }

        public void SunRise(bool y)
        {
            foreach (L2Territory t in Territorries.Values)
                t.SunRise(y);

            Spawns.ForEach(s => s.SunRise(y));
        }

        public L2Object SpawnOne(int id, int x, int y, int z, int h)
        {
            //NpcTemplate template = new NpcTemplate(new StatsSet()); //NpcTable.Instance.GetNpcTemplate(id);

            L2Warrior o = new L2Warrior
            {
                X = x,
                Y = y,
                Z = z,
                Heading = h,
                SpawnX = x,
                SpawnY = y,
                SpawnZ = z
            };
            //o.setTemplate(template);
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
            //        o = new L2Npc();
            //        ((L2Npc)o).setTemplate(template);
            //        break;
            //}

            L2World.Instance.AddObject(o);
            o.OnSpawn();

            return o;
        }
    }
}