using System.Collections.Generic;
using L2dotNET.Enums;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Templates
{
    public class PcTemplate : CharTemplate
    {
        public ClassId ClassId { get; set; }

        public int FallingHeight { get; }

        public int BaseSwimSpd { get; }

        public double CollisionRadiusFemale { get; }
        public double CollisionHeightFemale { get; }

        public int SpawnX { get; }
        public int SpawnY { get; }
        public int SpawnZ { get; }

        public int ClassBaseLevel { get; }

        public double[] HpTable { get; }
        public double[] MpTable { get; }
        public double[] CpTable { get; }

        public List<L2Item> Items { get; } = new List<L2Item>();

        public PcTemplate(ClassId classId, StatsSet set) : base(set)
        {
            ClassId = classId;

            BaseSwimSpd = set.GetInt("swimSpd", 1);

            FallingHeight = set.GetInt("falling_height", 333);

            CollisionRadiusFemale = set.GetDouble("radiusFemale");
            CollisionHeightFemale = set.GetDouble("heightFemale");

            SpawnX = set.GetInt("spawnX");
            SpawnY = set.GetInt("spawnY");
            SpawnZ = set.GetInt("spawnZ");

            ClassBaseLevel = set.GetInt("baseLvl");

            string[] hpTable = set.GetString("hpTable").Split(';');

            HpTable = new double[hpTable.Length];

            for (int i = 0; i < hpTable.Length; i++)
                HpTable[i] = double.Parse(hpTable[i]);

            string[] mpTable = set.GetString("mpTable").Split(';');

            MpTable = new double[mpTable.Length];
            for (int i = 0; i < mpTable.Length; i++)
                MpTable[i] = double.Parse(mpTable[i]);

            string[] cpTable = set.GetString("cpTable").Split(';');

            CpTable = new double[cpTable.Length];
            for (int i = 0; i < cpTable.Length; i++)
                CpTable[i] = double.Parse(cpTable[i]);
        }
    }
}