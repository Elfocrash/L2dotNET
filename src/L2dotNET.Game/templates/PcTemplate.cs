using System;
using System.Collections.Generic;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.model.items;

namespace L2dotNET.GameService.templates
{
    public class PcTemplate : CharTemplate
    {
        private ClassId classId;
        public ClassId ClassId { get { return classId; }
            set { classId = value; } }

        public int fallingHeight;
        public int FallingHeight { get { return fallingHeight; } }
        private int baseSwimSpd;
        public int BaseSwimSpd { get { return baseSwimSpd; } }

        private double collisionRadiusFemale;
        public double CollisionRadiusFemale { get { return collisionRadiusFemale; } }
        private double collisionHeightFemale;
        public double CollisionHeightFemale { get { return collisionHeightFemale; } }

        private int spawnX;
        public int SpawnX { get { return spawnX; } }
        private int spawnY;
        public int SpawnY { get { return spawnY; } }
        private int spawnZ;
        public int SpawnZ { get { return spawnZ; } }

        private int classBaseLevel;
        public int ClassBaseLevel { get { return classBaseLevel; } }

        private double[] hpTable;
        public double[] HpTable { get { return hpTable; } }
        private double[] mpTable;
        public double[] MpTable { get { return mpTable; } }
        private double[] cpTable;
        public double[] CpTable { get { return cpTable; } }

        private List<L2Item> items = new List<L2Item>();
        public List<L2Item> Items { get { return items; } }

        public PcTemplate(ClassId classId, StatsSet set)
            :base(set)
        {
            this.classId = classId;

            baseSwimSpd = set.GetInt("swimSpd", 1);

            fallingHeight = set.GetInt("falling_height", 333);

            collisionRadiusFemale = set.GetDouble("radiusFemale");
            collisionHeightFemale = set.GetDouble("heightFemale");

            spawnX = set.GetInt("spawnX");
            spawnY = set.GetInt("spawnY");
            spawnZ = set.GetInt("spawnZ");

            classBaseLevel = set.GetInt("baseLvl");

            string[] _hpTable = set.GetString("hpTable").Split(';');

            hpTable = new double[_hpTable.Length];

            for (int i = 0; i < _hpTable.Length; i++)
                hpTable[i] = Double.Parse(_hpTable[i]);

            string[] _mpTable = set.GetString("mpTable").Split(';');

            mpTable = new double[_mpTable.Length];
            for (int i = 0; i < _mpTable.Length; i++)
                mpTable[i] = Double.Parse(_mpTable[i]);

            string[] _cpTable = set.GetString("cpTable").Split(';');

            cpTable = new double[_cpTable.Length];
            for (int i = 0; i < _cpTable.Length; i++)
                cpTable[i] = Double.Parse(_cpTable[i]);
        }
    }
}
