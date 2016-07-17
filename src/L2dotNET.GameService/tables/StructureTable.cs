using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Tables
{
    class StructureTable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StructureTable));
        private static volatile StructureTable _instance;
        private static readonly object SyncRoot = new object();

        public static StructureTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new StructureTable();
                }

                return _instance;
            }
        }

        public SortedList<int, HideoutTemplate> Structures = new SortedList<int, HideoutTemplate>();
        public SortedList<int, StructureSpawn> Spawns = new SortedList<int, StructureSpawn>();
        public SortedList<int, Hideout> Hideouts = new SortedList<int, Hideout>();

        public StructureSpawn GetSpawn(int id)
        {
            return Spawns.ContainsKey(id) ? Spawns[id] : null;
        }

        public void Initialize()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\structure_spawn.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
                        continue;

                    string[] pt = line.Split('\t');
                    StructureSpawn spawn = new StructureSpawn
                                           {
                                               NpcId = Convert.ToInt32(pt[0])
                                           };

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
                                spawn.RespawnSec = Convert.ToInt32(value);
                                break;
                        }
                    }

                    Spawns.Add(spawn.NpcId, spawn);
                }
            }

            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\structures.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    HideoutTemplate template = null;
                    switch (pt[1])
                    {
                        case "hideout":
                            template = new Hideout();
                            break;
                    }

                    if (template == null)
                        continue;

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
                                    template.SetZoneLoc(str.Split(' '));
                            }

                                break;
                        }
                    }

                    Structures.Add(template.ID, template);
                }
            }

            Log.Info("Structs: loaded " + Structures.Count + " templates.");
            Log.Info("Hideouts: " + Hideouts.Count + ".");

            foreach (HideoutTemplate st in Structures.Values)
                st.init();
        }
    }
}