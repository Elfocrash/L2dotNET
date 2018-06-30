using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Models.Npcs.Decor;
using L2dotNET.Models.Stats;
using L2dotNET.Utility;
using L2dotNET.World;
using NLog;

namespace L2dotNET.Tables
{
    class StaticObjTable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static volatile StaticObjTable _instance;
        private static readonly object SyncRoot = new object();

        public static StaticObjTable Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new StaticObjTable();
                }

                return _instance;
            }
        }

        public SortedList<int, L2StaticObject> Objects;

        public void Initialize()
        {
            Objects = new SortedList<int, L2StaticObject>();
            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\staticobjects.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if ((line.Length == 0) || line.StartsWithIgnoreCase("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    L2StaticObject obj = null;

                    switch (pt[1])
                    {
                        case "map":
                           // obj = new L2TownMap(IdFactory.Instance.NextId(),null);
                            break;
                        case "chair":
                           // obj = new L2Chair(IdFactory.Instance.NextId(), null);
                            break;
                        case "pvp":
                            //obj = new L2PvPSign(IdFactory.Instance.NextId(), null);
                            break;
                        case "door":
                            //obj = new L2Door(IdFactory.Instance.NextId(), null);
                            break;
                    }

                    if (obj == null)
                        continue;

                    obj.StaticId = Convert.ToInt32(pt[0]);

                    for (byte ord = 2; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1);
                        value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "spawn":
                                obj.SetLoc(value.Split(' '));
                                break;
                            case "tex":
                                obj.SetTex(value.Split(' '));
                                break;
                            case "htm":
                                obj.Htm = value;
                                break;
                            case "hp":
                                obj.CharacterStat = new CharacterStat(obj);
                                break;
                            case "defence":
                                obj.Pdef = Convert.ToInt32(value.Split(' ')[0]);
                                obj.Mdef = Convert.ToInt32(value.Split(' ')[1]);
                                break;
                            case "unlock":
                            {
                                foreach (string str in value.Split(' '))
                                {
                                    switch (str)
                                    {
                                        case "trigger":
                                            obj.UnlockTrigger = true;
                                            break;
                                        case "skill":
                                            obj.UnlockSkill = true;
                                            break;
                                        case "drop":
                                            obj.UnlockNpc = true;
                                            break;
                                    }
                                }
                            }

                                break;
                        }
                    }

                    Objects.Add(obj.StaticId, obj);
                }
            }

            foreach (L2StaticObject o in Objects.Values)
            {
                L2World.Instance.AddObject(o);
                o.OnSpawnAsync();
            }

            Log.Info($"Spawned {Objects.Count} objects.");
        }
    }
}