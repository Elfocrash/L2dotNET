using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.skills2.effects;
using L2dotNET.Game.network.l2send;
using MySql.Data.MySqlClient;
using L2dotNET.Game.db;
using System.Data;
using System.IO;

namespace L2dotNET.Game._test
{
    class npc_l2j_to_rcs
    {
        public static void ss()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT * FROM npc";
            cmd.CommandType = CommandType.Text;

            
            MySqlDataReader reader = cmd.ExecuteReader();
            SortedList<int, l2jnpc> npcs = new SortedList<int, l2jnpc>();
            while (reader.Read())
            {
                l2jnpc npc = new l2jnpc();
                npc.id = reader.GetInt32("id");
                npc.name = reader.GetString("name");
                npc.title = reader.GetString("title");
                npc.classs = reader.GetString("class");
                npc.coll_radius = reader.GetDouble("collision_radius");
                npc.coll_hei = reader.GetDouble("collision_height");
                npc.level = reader.GetInt32("level");
                npc.type = reader.GetString("type");
                npc.attackrange = reader.GetInt32("attackrange");
                npc.hp = reader.GetInt32("hp");
                npc.mp = reader.GetInt32("mp");
                npc.hpreg = reader.GetDouble("hpreg");
                npc.mpreg = reader.GetDouble("mpreg");
                npc.exp = reader.GetInt64("exp");
                npc.sp = reader.GetInt32("sp");
                npc.patk = reader.GetDouble("patk");
                npc.pdef = reader.GetDouble("pdef");
                npc.matk = reader.GetDouble("matk");
                npc.mdef = reader.GetDouble("mdef");
                npc.atkspd = reader.GetInt32("atkspd");
                npc.aggro = reader.GetInt32("aggro");
                npc.matkspd = reader.GetInt32("matkspd");
                npc.rhand = reader.GetInt32("rhand");
                npc.lhand = reader.GetInt32("lhand");
                npc.walkspd = reader.GetInt32("walkspd");
                npc.runspd = reader.GetInt32("runspd");
                try
                {
                    npc.faction_id = reader.GetString("faction_id");
                    npc.faction_range = reader.GetInt32("faction_range");
                }
                catch (Exception)
                {

                    npc.faction_id = "none";
                }
                npc.isUndead = reader.GetInt32("isUndead");
                npc.ss = reader.GetInt32("ss");
                npc.bss = reader.GetInt32("bss");
                npc.ss_rate = reader.GetInt32("ss_rate");
                npc.drop_herbs = reader.GetBoolean("drop_herbs");
                npcs.Add(npc.id, npc);
            }
            reader.Close();
            Console.WriteLine("sql npcs "+npcs.Count);

            string[] npcnames = File.ReadAllLines("npcname-e.tsv");
            SortedList<int, string[]> names = new SortedList<int, string[]>();
            foreach (string str in npcnames)
            {
                string[] st = str.Split('\t');
                int idz = int.Parse(st[0]);
                names.Add(idz, st);

                if (!npcs.ContainsKey(idz))
                {
                    Console.WriteLine("adding sufficient npc " + st[1]);
                    l2jnpc npc = new l2jnpc();
                    npc.id = idz;
                    npc.name = st[1];
                    npc.title = st[2];

                    npcs.Add(npc.id, npc);
                }
                else
                {
                    l2jnpc npc = npcs[idz];
                    if (npc.name != st[1])
                    {
                        Console.WriteLine("#" + idz + " updated name from " + npc.name + " to " + st[1]);
                        npcs[idz].name = st[1];
                    }

                    if (npc.title != st[1])
                    {
                        Console.WriteLine("#"+idz+" updated title from " + npc.title + " to " + st[2]);
                        npcs[idz].title = st[2];
                    }
                }
            }
            Console.WriteLine("npc names " + names.Count);

            string[] npcgrp = File.ReadAllLines("npcgrp.tsv");
            SortedList<int, string[]> grps = new SortedList<int, string[]>();
            foreach (string str in npcgrp)
            {
                string[] st = str.Split('\t');
                int idz = int.Parse(st[0]);
                grps.Add(idz, st);


                l2jnpc npc = npcs[idz];
                if (npc.classs == null || npc.classs.ToLower() != st[1].ToLower())
                {
                    //  Console.WriteLine("#" + idz + " updated class from " + npc.classs.ToLower() + " to " + st[1].ToLower());
                    npcs[idz].classs = st[1].ToLower();
                }
            }
            Console.WriteLine("npc grps " + grps.Count);

            StringBuilder sb = new StringBuilder();
            string tab = "\t";
            foreach (l2jnpc npc in npcs.Values)
            {
                string a = "";
                a += npc.id + tab;

                if (npc.name.Length > 0)
                    a += "name{" + npc.name + "}"+tab;
                if(npc.title.Length > 0)
                    a += "title{" + npc.title + "}" + tab;
                a += "class{" + npc.classs.ToLower() + "}" + tab;
                a += "collision{" + npc.coll_radius + " "+npc.coll_hei+"}" + tab;
                a += "level{" + npc.level + "}" + tab;
                a += "life{" + npc.hp + " "+npc.hpreg+" "+npc.mp+" "+npc.mpreg+"}" + tab;
                a += "attack{" + npc.patk + " " + npc.matk + " " + npc.attackrange + " " + npc.atkspd + " " + npc.matkspd + "}" + tab;
                a += "defense{" + npc.pdef + " " + npc.mdef + "}" + tab;
                a += "reward{" + npc.exp + " " + npc.sp + "}" + tab;
                if(npc.rhand > 0 || npc.lhand > 0)
                    a += "equip{" + npc.rhand + " " + npc.lhand + "}" + tab;
                a += "speed{" + npc.runspd + " " + npc.walkspd + "}" + tab;
                if(npc.ss > 0 || npc.bss > 0)
                    a += "sshot{" + npc.ss + " " + npc.bss + " " + npc.ss_rate + "}" + tab;
                if(npc.faction_id != "none" || npc.faction_id != "NULL")
                    a += "clan{" + npc.faction_id + " " + npc.faction_range + "}" + tab;

                string herb = "";
                if (npc.drop_herbs)
                    herb = "herb ";

                if (npc.isUndead == 1)
                    herb += "undead";

                if(herb.Length > 0)
                    a += "bonus{" + herb + "}";

                a += "\n";

                sb.Append(a);
            }

            File.WriteAllText(@"scripts\npcs.txt", sb.ToString());

            Console.WriteLine("l2j npcs done");
        }

        class l2jnpc
        {
            public int id;
            public string name;
            public string title;
            public string classs;
            public double coll_radius = 25.993;
            public double coll_hei = 25.993;
            public int level = 70;
            public string type;
            public int attackrange = 40;
            public int hp = 3862;
            public int mp = 1494;
            public double hpreg;
            public double mpreg;
            public long exp;
            public int sp;
            public double patk = 1303;
            public double pdef = 471;
            public double matk = 607;
            public double mdef = 382;
            public int atkspd = 253;
            public int aggro = 0;
            public int matkspd = 333;
            public int rhand;
            public int lhand;
            public int walkspd = 88;
            public int runspd = 130;
            public string faction_id;
            public int faction_range;
            public int isUndead = 0;
            public int ss;
            public int bss;
            public int ss_rate;
            public bool drop_herbs = false;

        }
    }
}
