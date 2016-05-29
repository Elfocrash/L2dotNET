using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.GameService.model.npcs.decor;
using L2dotNET.GameService.world;
using log4net;

namespace L2dotNET.GameService.tables
{
    class StaticObjTable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StaticObjTable));
        private static volatile StaticObjTable instance;
        private static object syncRoot = new object();

        public static StaticObjTable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new StaticObjTable();
                        }
                    }
                }

                return instance;
            }
        }

        public StaticObjTable()
        {

        }

        public SortedList<int, L2StaticObject> objects;
        public void Initialize()
        {
            objects = new SortedList<int, L2StaticObject>();
            using (StreamReader reader = new StreamReader(new FileInfo(@"scripts\staticobjects.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] pt = line.Split('\t');

                    L2StaticObject obj = null;

                    switch (pt[1])
                    {
                        case "map":
                            obj = new L2TownMap();
                            break;
                        case "chair":
                            obj = new L2Chair();
                            break;
                        case "pvp":
                            obj = new L2PvPSign();
                            break;
                        case "door":
                            obj = new L2Door();
                            break;
                    }

                    obj.StaticID = Convert.ToInt32(pt[0]);

                    for (byte ord = 2; ord < pt.Length; ord++)
                    {
                        string parameter = pt[ord];
                        string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

                        switch (parameter.Split('{')[0].ToLower())
                        {
                            case "spawn":
                                obj.setLoc(value.Split(' '));
                                break;
                            case "tex":
                                obj.setTex(value.Split(' '));
                                break;
                            case "htm":
                                obj.htm = value;
                                break;
                            case "hp":
                                obj.MaxHP = Convert.ToInt32(value);
                                break;
                            case "defence":
                                obj.pdef = Convert.ToInt32(value.Split(' ')[0]);
                                obj.mdef = Convert.ToInt32(value.Split(' ')[1]);
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

                    objects.Add(obj.StaticID, obj);
                }
            }
            foreach (L2StaticObject o in objects.Values)
            {
                L2World.Instance.AddObject(o);
                o.onSpawn();
            }

            log.Info($"StaticObjTable: Spanwed { objects.Count } objects.");
        }

        public L2Door GetDoor(int id)
        {
            if (objects.ContainsKey(id))
                return (L2Door)objects[id];

            return null;
        }
    }
}
