using System;
using System.Collections.Generic;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.templates;

namespace L2dotNET.GameService.Templates
{
    public class NpcTemplate : CharTemplate
    {
        public int NpcId { get; set; }
        public int IdTemplate { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool CantBeChampionMonster { get; set; }
        public byte Level { get; set; }
        public int Exp { get; set; }
        public int Sp { get; set; }
        public int RHand { get; set; }
        public int LHand { get; set; }
        public int EnchantEffect { get; set; }
        public int CorpseTime { get; set; }

        public int DropHerbGroup { get; set; }
        public Race race = Race.UNKNOWN;
        public AIType AiType { get; set; }

        public int SsCount { get; set; }
        public int SsRate { get; set; }
        public int SpsCount { get; set; }
        public int SpsRate { get; set; }
        public int AggroRange { get; set; }

        public string[] Clans { get; set; }
        public int ClanRange { get; set; }
        public int[] IgnoredIds { get; set; }

        public bool CanMove { get; set; }
        public bool IsSeedable { get; set; }

        //private List<L2Skill> _buffSkills = new ArrayList<>();
        //private List<L2Skill> _debuffSkills = new ArrayList<>();
        //private List<L2Skill> _healSkills = new ArrayList<>();
        //private List<L2Skill> _longRangeSkills = new ArrayList<>();
        //private List<L2Skill> _shortRangeSkills = new ArrayList<>();
        //private List<L2Skill> _suicideSkills = new ArrayList<>();

        //private List<DropCategory> _categories;
        //private List<MinionData> _minions;
        public List<ClassId> TeachInfo = new List<ClassId>();
        //private Dictionary<int, L2Skill> _skills = new Dictionary<>();
        //private Dictionary<EventType, List<Quest>> _questEvents = new Dictionary<>();

        public NpcTemplate(StatsSet set) : base(set)
        {
            NpcId = set.GetInt("id");
            IdTemplate = set.GetInt("idTemplate", NpcId);
            Type = set.GetString("type");
            Name = set.GetString("name");
            Title = set.GetString("title", "");
            CantBeChampionMonster = Title.Equals("Quest Monster", StringComparison.InvariantCultureIgnoreCase);
            Level = set.GetByte("level", (byte)1);
            Exp = set.GetInt("exp", 0);
            Sp = set.GetInt("sp", 0);
            RHand = set.GetInt("rHand", 0);
            LHand = set.GetInt("lHand", 0);
            EnchantEffect = set.GetInt("enchant", 0);
            CorpseTime = set.GetInt("corpseTime", 7);

            DropHerbGroup = set.GetInt("dropHerbGroup", 0);
            //if (_dropHerbGroup > 0 && HerbDropTable.getInstance().getHerbDroplist(_dropHerbGroup) == null)
            //{
            //    _log.warning("Missing dropHerbGroup information for npcId: " + _npcId + ", dropHerbGroup: " + _dropHerbGroup);
            //    _dropHerbGroup = 0;
            //}

            if (set.ContainsKey("raceId"))
                race = (Race)set.GetInt("raceId");

            //_aiType = set.GetEnumerator(new "aiType", AIType.DEFAULT);

            SsCount = set.GetInt("ssCount", 0);
            SsRate = set.GetInt("ssRate", 0);
            SpsCount = set.GetInt("spsCount", 0);
            SpsRate = set.GetInt("spsRate", 0);
            AggroRange = set.GetInt("aggro", 0);

            if (set.ContainsKey("clan"))
            {
                Clans = set.GetStringArray("clan");
                ClanRange = set.GetInt("clanRange");

                if (set.ContainsKey("ignoredIds"))
                    IgnoredIds = set.GetIntegerArray("ignoredIds");
            }

            CanMove = set.GetBool("canMove", true);
            IsSeedable = set.GetBool("seedable", false);

            // _categories = set.getList("drops");
            // _minions = set.getList("minions");

            //if (set.containsKey("teachTo"))
            //{
            // for (int classId : set.getIntegerArray("teachTo"))

            //addTeachInfo(ClassId.Values.classId]);
            //}

            //addSkills(set.getList("skills"));
        }
    }

    public enum AIType
    {
        DEFAULT,
        ARCHER,
        MAGE,
        HEALER,
        CORPSE
    }

    public enum Race
    {
        UNKNOWN,
        UNDEAD,
        MAGICCREATURE,
        BEAST,
        ANIMAL,
        PLANT,
        HUMANOID,
        SPIRIT,
        ANGEL,
        DEMON,
        DRAGON,
        GIANT,
        BUG,
        FAIRIE,
        HUMAN,
        ELVE,
        DARKELVE,
        ORC,
        DWARVE,
        OTHER,
        NONLIVING,
        SIEGEWEAPON,
        DEFENDINGARMY,
        MERCENARIE
    }
}