namespace L2dotNET.GameService.templates
{
    public class CharTemplate
    {
        public int BaseSTR { get; }
        public int BaseCON { get; }
        public int BaseDEX { get; }
        public int BaseINT { get; }
        public int BaseWIT { get; }
        public int BaseMEN { get; }

        public double BaseHpMax { get; }
        public double BaseMpMax { get; }

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
            BaseSTR = set.GetInt("str", 40);
            BaseCON = set.GetInt("con", 21);
            BaseDEX = set.GetInt("dex", 30);
            BaseINT = set.GetInt("int", 20);
            BaseWIT = set.GetInt("wit", 43);
            BaseMEN = set.GetInt("men", 20);

            BaseHpMax = set.GetDouble("hp", 0);
            BaseMpMax = set.GetDouble("mp", 0);

            BaseHpReg = set.GetDouble("hpRegen", 1.5d);
            BaseMpReg = set.GetDouble("mpRegen", 0.9d);

            BasePAtk = set.GetDouble("pAtk");
            BaseMAtk = set.GetDouble("mAtk");
            BasePDef = set.GetDouble("pDef");
            BaseMDef = set.GetDouble("mDef");

            BasePAtkSpd = set.GetInt("atkSpd", 300);

            BaseCritRate = set.GetInt("crit", 4);

            BaseWalkSpd = set.GetInt("walkSpd", 0);
            BaseRunSpd = set.GetInt("runSpd", 1);

            CollisionRadius = set.GetInt("radius");
            CollisionHeight = set.GetInt("height");
        }
    }
}