namespace L2dotNET.model.templates
{
    public class Object
    {
        public int NpcId;
        public ObjectCategory Category;
        public double CollisionRadius;
        public double CollisionHeight;
        public byte Level;
        public long Exp;
        public int ExCrtEffect = 1;
        public int Unique = 0;
        public double SNpcPropHpRate = 1;
        public ObjectRace Race;
        public ObjectSex Sex;
        public string SlotChest;
        public string SlotRhand;
        public string SlotLhand;
        public double HitTimeFactor;
        public double HitTimeFactorSkill = -1;
        public int Str;
        public int Int;
        public int Dex;
        public int Wit;
        public int Con;
        public int Men;
        public double OrgHp;
        public double OrgHpRegen;
        public double OrgMp;
        public double OrgMpRegen;
        public ObjectBaseAttackType BaseAttackType;
        public int BaseAttackRange;
        public int BaseRandDam;
        public double BasePhysicalAttack;
        public int BaseCritical;
        public double PhysicalHitModify;
        public int BaseAttackSpeed;
        public int BaseReuseDelay;
        public double BaseMagicAttack;
        public double BaseDefend;
        public double BaseMagicDefend;
        public double PhysicalAvoidModify;
        public int ShieldDefenseRate;
        public double ShieldDefense;
        public int SafeHeight = 100;
        public int SoulshotCount;
        public int SpiritshotCount;
        public string Clan;
        public int ClanHelpRange;
        public int Undying = 0;
        public int CanBeAttacked = 1;
        public int CorpseTime = 7;
        public int NoSleepMode;
        public int AgroRange;
        public int PassableDoor;
        public int CanMove = 1;
        public int Flying;
        public int HasSummoner;
        public int Targetable = 1;
        public int ShowNameTag = 1;
        public int EventFlag;
        public int Unsowing = 1;
        public int PrivateRespawnLog;
        public double AcquireExpRate;
        public int AcquireSp;
        public int AcquireRp;
        public int FakeClassId = -1;

        internal void SetNpcSkills(string value) { }
    }
}