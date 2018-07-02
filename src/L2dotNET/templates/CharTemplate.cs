namespace L2dotNET.Templates
{
    public class CharTemplate
    {
        public int BaseStr { get; }
        public int BaseCon { get; }
        public int BaseDex { get; }
        public int BaseInt { get; }
        public int BaseWit { get; }
        public int BaseMen { get; }

        private readonly double _baseHpMax;
        private readonly double _baseMpMax;

        public virtual double GetBaseMaxHp(int level) => _baseHpMax;
        public virtual double GetBaseMaxMp(int level) => _baseMpMax;

        public double BaseHpReg { get; }
        public double BaseMpReg { get; }

        public double BasePAtk { get; }
        public double BaseMAtk { get; }
        public double BasePDef { get; }
        public double BaseMDef { get; }

        public int BasePAtkSpd { get; }

        public int BaseCritRate { get; }

        public int BaseWalkSpd { get; }
        public int BaseRunSpd { get; }

        public double CollisionRadius { get; }
        public double CollisionHeight { get; }

        public CharTemplate(StatsSet set)
        {
            BaseStr = set.GetInt("str", 40);
            BaseCon = set.GetInt("con", 21);
            BaseDex = set.GetInt("dex", 30);
            BaseInt = set.GetInt("int", 20);
            BaseWit = set.GetInt("wit", 43);
            BaseMen = set.GetInt("men", 20);

            _baseHpMax = set.GetDouble("hp");
            _baseMpMax = set.GetDouble("mp");

            BaseHpReg = set.GetDouble("hpRegen", 1.5d);
            BaseMpReg = set.GetDouble("mpRegen", 0.9d);

            BasePAtk = set.GetDouble("pAtk");
            BaseMAtk = set.GetDouble("mAtk");
            BasePDef = set.GetDouble("pDef");
            BaseMDef = set.GetDouble("mDef");

            BasePAtkSpd = set.GetInt("atkSpd", 300);

            BaseCritRate = set.GetInt("crit", 4);

            BaseWalkSpd = set.GetInt("walkSpd");
            BaseRunSpd = set.GetInt("runSpd", 1);

            CollisionRadius = set.GetDouble("radius");
            CollisionHeight = set.GetDouble("height");
        }
    }
}