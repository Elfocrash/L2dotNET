using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Skills2
{
    public class SkillEnchantInfo
    {
        public SortedList<int, SkillEnchantInfoDetail> Details = new SortedList<int, SkillEnchantInfoDetail>();
        public int Id;
        public int Lv;
    }
}