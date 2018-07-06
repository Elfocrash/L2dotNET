﻿using System.Collections.Generic;
using L2dotNET.Enums;
using L2dotNET.Utility;

namespace L2dotNET.Templates
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
        public double Hp { get; set; }
        public double Mp { get; set; }

        public int DropHerbGroup { get; set; }
        public Race race = Race.Unknown;
        public AiType AiType { get; set; }

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
        public List<ClassId> TeachInfo { get; }
        //private Dictionary<int, L2Skill> _skills = new Dictionary<>();
        //private Dictionary<EventType, List<Quest>> _questEvents = new Dictionary<>();

        public NpcTemplate(StatsSet set) : base(set)
        {
            //Refactor this
            TeachInfo = new List<ClassId>();

            NpcId = set.GetInt("id");
            IdTemplate = set.GetInt("idTemplate", NpcId);
            Type = set.GetString("type");
            Name = set.GetString("name");
            Title = set.GetString("title", string.Empty);
            CantBeChampionMonster = Title.EqualsIgnoreCase("Quest Monster");
            Level = set.GetByte("level", 1);
            Exp = set.GetInt("exp");
            Sp = set.GetInt("sp");
            RHand = set.GetInt("rHand");
            LHand = set.GetInt("lHand");
            EnchantEffect = set.GetInt("enchant");
            CorpseTime = set.GetInt("corpseTime", 7);
            Hp = set.GetDouble("hp");
            Mp = set.GetDouble("mp");

            DropHerbGroup = set.GetInt("dropHerbGroup");
            //if (_dropHerbGroup > 0 && HerbDropTable.getInstance().getHerbDroplist(_dropHerbGroup) == null)
            //{
            //    Log.warning($"Missing dropHerbGroup information for npcId: {_npcId}, dropHerbGroup: {_dropHerbGroup});
            //    _dropHerbGroup = 0;
            //}

            if (set.ContainsKey("raceId"))
                race = (Race)set.GetInt("raceId");

            //_aiType = set.GetEnumerator(new "aiType", AIType.DEFAULT);

            SsCount = set.GetInt("ssCount");
            SsRate = set.GetInt("ssRate");
            SpsCount = set.GetInt("spsCount");
            SpsRate = set.GetInt("spsRate");
            AggroRange = set.GetInt("aggro");

            if (set.ContainsKey("clan"))
            {
                Clans = set.GetStringArray("clan");
                ClanRange = set.GetInt("clanRange");

                if (set.ContainsKey("ignoredIds"))
                    IgnoredIds = set.GetIntegerArray("ignoredIds");
            }

            CanMove = set.GetBool("canMove", true);
            IsSeedable = set.GetBool("seedable");

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
}