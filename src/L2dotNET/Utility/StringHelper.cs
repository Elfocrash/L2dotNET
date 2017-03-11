using System;
using System.Linq;

namespace L2dotNET.Utility
{
    public static class StringHelper
    {
        public static string ToTitleCase (this string str, char delimeter)
        {
            if (str == null) return null;
            return String.Join(String.Empty, str.Split(delimeter).Select(item => item.First().ToString().ToUpper() + item.Substring(1)).ToArray());
        }
    }
}
