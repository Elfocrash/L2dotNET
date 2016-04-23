using System;
using System.Collections.Generic;
using System.IO;
using L2dotNET.Game.model.structures;
using MySql.Data.MySqlClient;
using System.Data;
using L2dotNET.Game.logger;

namespace L2dotNET.Game.tables
{
    class StructureTable
    {
        private static StructureTable instance = new StructureTable();
        public static StructureTable getInstance()
        {
            return instance;
        }

        public SortedList<int, HideoutTemplate> structures = new SortedList<int, HideoutTemplate>();
        public SortedList<int, StructureSpawn> spawns = new SortedList<int, StructureSpawn>();
        public SortedList<int, Hideout> hideouts = new SortedList<int, Hideout>();

        public StructureSpawn getSpawn(int id)
        {
            if (spawns.ContainsKey(id))
                return spawns[id];

            return null;
        }

        public void init()
        {
            foreach (HideoutTemplate st in structures.Values)
            {
                st.init();
            }
        }

        public void read()
        {
            StreamReader reader = new StreamReader(new FileInfo(@"scripts\structure_spawn.txt").FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                string[] pt = line.Split('\t');
                StructureSpawn spawn = new StructureSpawn();
                spawn.npcId = Convert.ToInt32(pt[0]);

                for (byte ord = 1; ord < pt.Length; ord++)
                {
                    string parameter = pt[ord];
                    string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

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
            reader.Close();

            reader = new StreamReader(new FileInfo(@"scripts\structures.txt").FullName);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
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

                template.ID = Convert.ToInt32(pt[0]);

                for (byte ord = 2; ord < pt.Length; ord++)
                {
                    string parameter = pt[ord];
                    string value = parameter.Substring(parameter.IndexOf('{') + 1); value = value.Remove(value.Length - 1);

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
            reader.Close();

            //{
            //    MySqlConnection connection = SQLjec.getInstance().conn();
            //    MySqlCommand cmd = connection.CreateCommand();

            //    connection.Open();

            //    cmd.CommandText = "SELECT * FROM st_hideouts";
            //    cmd.CommandType = CommandType.Text;


            //    MySqlDataReader msreader = cmd.ExecuteReader();

            //    while (msreader.Read())
            //    {
            //        int id = msreader.GetInt32("id");
            //        Hideout hideout = (Hideout)structures[id];

            //        hideout.Name = msreader.GetString("name");
            //        hideout.Descr = msreader.GetString("descr");
            //        //TODO paytime

            //        for (byte a = 1; a <= 12; a++)
            //            hideout.Decoration[a] = msreader.GetInt32("func_" + a);

            //        hideouts.Add(hideout.ID, hideout);
            //    }

            //    reader.Close();
            //    connection.Close();
            //}

            CLogger.info("Structs: loaded "+structures.Count+" templates.");
            CLogger.info("Hideouts: " + hideouts.Count + ".");
        }
    }

    class StructureSpawn
    {
        public int x, y, z, heading;
        public int respawnSec = 60;
        public int npcId;

        internal void SetLocation(string[] loc)
        {
            x = Convert.ToInt32(loc[0]);
            y = Convert.ToInt32(loc[1]);
            z = Convert.ToInt32(loc[2]);
            heading = Convert.ToInt32(loc[3]);
        }
    }
}
