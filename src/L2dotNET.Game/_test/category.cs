using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game._test
{
    class category
    {
        public List<l2jitem> items = new List<l2jitem>();
        public int id;
        internal void additem(l2jitem item)
        {
            items.Add(item);
        }
    }
}
