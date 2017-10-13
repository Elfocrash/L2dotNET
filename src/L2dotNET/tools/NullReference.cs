using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.tools
{
    public class NullReference
    {
        public static string GetTypeLower<T>(T obj)
        {
            Type t;
            if (obj == null)
                t = typeof(T);
            else
                t = obj.GetType();
            return t.Name.ToLower();
        }
    }
}
