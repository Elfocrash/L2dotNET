using L2dotNET.templates;

namespace L2dotNET.model.items
{
    public sealed class Henna
    {
        public int SymbolId { get; set; }
        public int Dye { get; set; }
        public int Price { get; set; }
        public int StatInt { get; set; }
        public int StatStr { get; set; }
        public int StatCon { get; set; }
        public int StatMen { get; set; }
        public int StatDex { get; set; }
        public int StatWit { get; set; }

        public Henna(StatsSet set)
        {
            SymbolId = set.GetInt("symbol_id");
            Dye = set.GetInt("dye");
            Price = set.GetInt("price");
            StatInt = set.GetInt("INT");
            StatStr = set.GetInt("STR");
            StatCon = set.GetInt("CON");
            StatMen = set.GetInt("MEN");
            StatDex = set.GetInt("DEX");
            StatWit = set.GetInt("WIT");
        }

        public static int AmountRequired => 10;
    }
}