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
    class drop_l2j_to_rcs
    {
        public static void ss()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT * FROM droplist";
            cmd.CommandType = CommandType.Text;

            
            MySqlDataReader reader = cmd.ExecuteReader();
            SortedList<int, l2jnpc> npcs = new SortedList<int, l2jnpc>();
            while (reader.Read())
            {
                l2jnpc npc;
                int mobId = reader.GetInt32("mobId");
                byte a = 0;
                if (npcs.ContainsKey(mobId))
                {
                    npc = npcs[mobId]; a = 1;
                }
                else 
                {
                    npc = new l2jnpc();
                    npc.mobId = mobId;
                }

                npc.additem(reader.GetInt32("itemId"), reader.GetInt32("min"), reader.GetInt32("max"), reader.GetInt32("category"), reader.GetInt32("chance"));
                 
                if(a==0)
                    npcs.Add(npc.mobId, npc);
            }
            reader.Close();
            Console.WriteLine("sql drops "+npcs.Count);

            SortedList<int, string> names = new SortedList<int, string>();
            foreach (string st in File.ReadAllLines("itemname-e.tsv"))
            {
                string[] s = st.Split('\t');
                names.Add(int.Parse(s[0]), s[1]);
            }

            SortedList<int, string> names2 = new SortedList<int, string>();
            foreach (string st in File.ReadAllLines("npcname-e.tsv"))
            {
                string[] s = st.Split('\t');
                names2.Add(int.Parse(s[0]), s[1]);

            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<null>\n <list>\n");
            foreach (l2jnpc npc in npcs.Values)
            {

                sb.Append("  <npc id=\"" + npc.mobId + "\"> <!-- " + names2[npc.mobId] + " -->\n");

                foreach (category cat in npc.cats.Values)
                {
                    sb.Append("    <category id=\""+cat.id+"\">\n");

                    foreach (l2jitem item in cat.items)
                    {
                        string nm = "not found ! " + item.itemid;
                        if (names.ContainsKey(item.itemid))
                            nm = names[item.itemid];
                        sb.Append("     <item id=\"" + item.itemid + "\" count=\"" + item.min + "-" + item.max + "\" rate=\"" + item.rate + "\"/> <!-- " + nm + " -->\n");
                    }
                    sb.Append("    </category>\n\n");
                }

                sb.Append("  </npc>\n\n");
            }

            sb.Append(" </list>\n</null>\n");

            File.WriteAllText(@"scripts\drops.xml", sb.ToString());

            Console.WriteLine("l2j drops done");
        }

        class l2jnpc
        {
            public int mobId;

            public SortedList<int, category> cats = new SortedList<int, category>();

            public void additem(int itemId, int min, int max, int cat, int rate)
            {
                l2jitem item = new l2jitem();
                item.itemid = itemId;
                item.min = min;
                item.max = max;
                item.cat = cat;
                item.rate = rate;

                if (cats.ContainsKey(cat))
                    cats[cat].additem(item);
                else
                {
                    category c = new category();
                    c.id = cat;
                    c.additem(item);
                    cats.Add(cat, c);
                }
            }
        }
    }
}
