using System.Collections.Generic;

namespace L2dotNET.GameService.Tables
{
    public class ND_shop
    {
        public SortedList<short, ND_shopList> lists = new SortedList<short, ND_shopList>();
        public double mod;
        public int id;
    }
}