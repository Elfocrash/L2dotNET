using System;
using System.Linq;

namespace L2dotNET.Utility
{
    public static class StringHelper
    {
        // used to convert string to C# enum naming (i_p_attack -> IPAttack, can_summon_cubic -> CanSummonCubic and etc)
        public static string ToTitleCase (this string str, char delimeter)
        {
            if (str == null) return null;
            return String.Join(String.Empty, str.Split(delimeter).Select(item => item.First().ToString().ToUpper() + item.Substring(1)).ToArray());
        }
    }
}
