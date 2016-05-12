using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.templates
{
    public class CharTemplate
    {
        private int baseSTR;
        public int BaseSTR { get { return baseSTR; } }
        private int baseCON;
        public int BaseCON { get { return baseCON; } }
        private int baseDEX;
        public int BaseDEX { get { return baseDEX; } }
        private int baseINT;
        public int BaseINT { get { return baseINT; } }
        private int baseWIT;
        public int BaseWIT { get { return baseWIT; } }
        private int baseMEN;
        public int BaseMEN { get { return baseMEN; } }

        private double baseHpMax;
        public double BaseHpMax { get { return baseHpMax; } }
        private double baseMpMax;
        public double BaseMpMax { get { return baseMpMax; } }

        private double baseHpReg;
        public double BaseHpReg { get { return baseHpReg; } }
        private double baseMpReg;
        public double BaseMpReg { get { return baseMpReg; } }

        private double basePAtk;
        public double BasePAtk { get { return basePAtk; } }
        private double baseMAtk;
        public double BaseMAtk { get { return baseMAtk; } }
        private double basePDef;
        public double BasePDef { get { return basePDef; } }
        private double baseMDef;
        public double BaseMDef { get { return baseMDef; } }

        private int basePAtkSpd;
        public int BasePAtkSpd { get { return basePAtkSpd; } }

        private int baseCritRate;
        public int BaseCritRate { get { return baseCritRate; } }

        private int baseWalkSpd;
        public int BaseWalkSpd { get { return baseWalkSpd; } }
        private int baseRunSpd;
        public int BaseRunSpd { get { return baseRunSpd; } }

        private double collisionRadius;
        public double CollisionRadius { get { return collisionRadius; } }
        private double collisionHeight;
        public double CollisionHeight { get { return collisionHeight; } }

        public CharTemplate(StatsSet set)
        {
            baseSTR = set.GetInt("str", 40);
            baseCON = set.GetInt("con", 21);
            baseDEX = set.GetInt("dex", 30);
            baseINT = set.GetInt("int", 20);
            baseWIT = set.GetInt("wit", 43);
            baseMEN = set.GetInt("men", 20);

            baseHpMax = set.GetDouble("hp", 0);
            baseMpMax = set.GetDouble("mp", 0);

            baseHpReg = set.GetDouble("hpRegen", 1.5d);
            baseMpReg = set.GetDouble("mpRegen", 0.9d);

            basePAtk = set.GetDouble("pAtk");
            baseMAtk = set.GetDouble("mAtk");
            basePDef = set.GetDouble("pDef");
            baseMDef = set.GetDouble("mDef");

            basePAtkSpd = set.GetInt("atkSpd", 300);

            baseCritRate = set.GetInt("crit", 4);

            baseWalkSpd = set.GetInt("walkSpd", 0);
            baseRunSpd = set.GetInt("runSpd", 1);

            collisionRadius = set.GetInt("radius");
            collisionHeight = set.GetInt("height");
        }

    }
}
