using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Skills2
{
    public class TAcquireSkill
    {
        public int get_lv;
        public int lv_up_sp;
        public bool auto_get = false;
        public int id;
        public int lv;
        public int social_class;
        public bool residence_skill;
        public string pledge_type = "";
        public int id_prerequisite_skill;
        public int lv_prerequisite_skill;
        public int itemcount;
        public int itemid;

        public List<int> quests = new List<int>();
        public List<byte> races = new List<byte>();
    }
}