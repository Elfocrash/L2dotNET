using System.Collections.Generic;

namespace L2dotNET.GameService.Tables
{
    public class NDShop
    {
        public SortedList<short, NdShopList> Lists = new SortedList<short, NdShopList>();
        public double Mod;
        public int Id;
    }
}