using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Skills2
{
    public class TSkillEnchantInfo
    {
        public SortedList<int, TSkillEnchantInfoDetail> details = new SortedList<int, TSkillEnchantInfoDetail>();
        public int id;
        public int lv;
    }
}