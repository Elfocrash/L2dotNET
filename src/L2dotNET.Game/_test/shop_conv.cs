using System.Collections.Generic;
using L2dotNET.Game.tables;
using System.IO;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game._test
{
    class shop_conv
    {
        public static void test()
        {
            ItemTable it = ItemTable.getInstance();

            string[] lines = File.ReadAllLines("source.txt");
            List<string> list = new List<string>();
            list.Add("     <selllist id=\"__\">\n       <item>");
            foreach (string ln in lines)
            {
                string lnl = ln.Replace("\t","").Substring(1).Split(';')[0];
                int id = int.Parse(lnl);
                ItemTemplate item = it.getItem(id);
                list.Add("       " + item.ItemID + ",");
            }
            list.Add("       </item>\n     </selllist>");

            File.WriteAllLines("output.txt", list.ToArray());
        }
    }
}
