using System.Collections.Generic;

namespace L2dotNET.GameService.Tables
{
    class FString
    {
        private static readonly FString inst = new FString();

        public static FString getInstance()
        {
            return inst;
        }

        public SortedList<int, string> strings = new SortedList<int, string>();

        public string get(int p)
        {
            if (strings.ContainsKey(p))
                return strings[p];

            return "" + p;
        }
    }
}