using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.tables
{
    class FString
    {
        private static FString inst = new FString();
        public static FString getInstance()
        {
            return inst;
        }
        public SortedList<int, string> strings = new SortedList<int, string>();

        public string get(int p)
        {
            if (strings.ContainsKey(p))
                return strings[p];
            else
                return ""+p;
        }
    }
}
