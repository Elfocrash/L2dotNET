using System.Collections.Generic;

namespace L2dotNET.GameService.Model.Skills2
{
    public class AcquireSkill
    {
        public int GetLv;
        public int LvUpSp;
        public bool AutoGet = false;
        public int Id;
        public int Lv;
        public int SocialClass;
        public bool ResidenceSkill;
        public string PledgeType = "";
        public int IdPrerequisiteSkill;
        public int LvPrerequisiteSkill;
        public int ItemCount;
        public int ItemId;

        public List<int> Quests = new List<int>();
        public List<byte> Races = new List<byte>();
    }
}