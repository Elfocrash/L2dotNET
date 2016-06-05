using System.Collections.Generic;

namespace L2dotNET.GameService.model.skills2
{
    public class TSkillEnchantInfo
    {
        public SortedList<int, TSkillEnchantInfoDetail> details = new SortedList<int, TSkillEnchantInfoDetail>();
        public int id;
        public int lv;
    }

    public class TSkillEnchantInfoDetail
    {
        public int route_id;
        public int enchant_id;
        public int enchanted_skill_level;
        public byte importance;
        public int r1;
        public int r2;
        public int r3;
        public int r4;
    }
}