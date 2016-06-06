using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using L2dotNET.GameService.Model.Structures;

namespace L2dotNET.GameService.Tables
{
    class StructureTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StructureTable));
        private static volatile StructureTable instance;
        private static readonly object syncRoot = new object();

        public static StructureTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new StructureTable();
                        }
                    }
                }

                return instance;
            }
        }

        public StructureTable() { }

        public SortedList<int, HideoutTemplate> structures = new SortedList<int, HideoutTemplate>();
        public SortedList<int, StructureSpawn> spawns = new SortedList<int, StructureSpawn>();
        public SortedList<int, Hideout> hideouts = new SortedList<int, Hideout>();

        public StructureSpawn GetSpawn(int id)
        {
            if (spawns.ContainsKey(id))
                return spawns[id];

            return null;
        }

        public void Initialize()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\structure_spawn.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');
                    StructureSpawn spawn = new StructureSpawn();
                    spawn.npcId = Convert.ToInt32(pt[0]);

                    for (byte ord = 1; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1);
                        value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "spawn":
                                spawn.SetLocation(value.Split(' '));
                                break;
                            case "resp":
                                spawn.respawnSec = Convert.ToInt32(value);
                                break;
                        }
                    }

                    spawns.Add(spawn.npcId, spawn);
                }
            }

            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\structures.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    HideoutTemplate template = null;
                    switch (pt[1])
                    {
                        case "hideout":
                            template = new Hideout();
                            break;
                    }

                    if (template != null)
                    {
                        template.ID = Convert.ToInt32(pt[0]);

                        for (byte ord = 2; ord < pt.Length; ord++)
                        {
                            string parameter = pt[ord];
                            string value = parameter.Substring(parameter.IndexOf('{') + 1);
                            value = value.Remove(value.Length - 1);

                            switch (parameter.Split('{')[0].ToLower())
                            {
                                case "npc":
                                {
                                    foreach (string str in value.Split(' '))
                                        template.SetNpc(Convert.ToInt32(str));
                                }
                                    break;
                                case "door":
                                {
                                    foreach (string str in value.Split(' '))
                                        template.SetDoor(Convert.ToInt32(str));
                                }
                                    break;
                                case "spawn":
                                    template.SetOwnerRespawn(value.Split(' '));
                                    break;
                                case "outside":
                                    template.SetOutsideRespawn(value.Split(' '));
                                    break;
                                case "banish":
                                    template.SetBanishRespawn(value.Split(' '));
                                    break;
                                case "zone":
                                {
                                    foreach (string str in value.Split(';'))
                                    {
                                        template.SetZoneLoc(str.Split(' '));
                                    }
                                }
                                    break;
                            }
                        }

                        structures.Add(template.ID, template);
                    }
                }
            }

            log.Info("Structs: loaded " + structures.Count + " templates.");
            log.Info("Hideouts: " + hideouts.Count + ".");

            foreach (HideoutTemplate st in structures.Values)
            {
                st.init();
            }
        }
    }
}