using L2dotNET.GameService.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.Model.Items
{
    public sealed class Henna
    {
        public int SymbolId { get; set; }
        public int Dye { get; set; }
        public int Price { get; set; }
        public int StatINT { get; set; }
        public int StatSTR { get; set; }
        public int StatCON { get; set; }
        public int StatMEN { get; set; }
        public int StatDEX { get; set; }
        public int StatWIT { get; set; }

        public Henna(StatsSet set)
        {
            SymbolId = set.GetInt("symbol_id");
            Dye = set.GetInt("dye");
            Price = set.GetInt("price");
            StatINT = set.GetInt("INT");
            StatSTR = set.GetInt("STR");
            StatCON = set.GetInt("CON");
            StatMEN = set.GetInt("MEN");
            StatDEX = set.GetInt("DEX");
            StatWIT = set.GetInt("WIT");
        }

        public static int AmountRequired { get { return 10; } }
    }

    
}
