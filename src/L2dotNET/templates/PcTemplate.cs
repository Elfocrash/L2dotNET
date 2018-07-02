using System.Collections.Generic;
using System.Linq;
using L2dotNET.Enums;
using L2dotNET.Models.Items;

namespace L2dotNET.Templates
{
    public class PcTemplate : CharTemplate
    {
        public ClassId ClassId { get; }

        public int FallingHeight { get; }

        public int BaseSwimSpeed { get; }

        public double CollisionRadiusFemale { get; }
        public double CollisionHeightFemale { get; }

        public int SpawnX { get; }
        public int SpawnY { get; }
        public int SpawnZ { get; }

        public int BaseLevel { get; }

        public List<int> DefaultInventory { get; }

        private double[] HpTable { get; }
        private double[] MpTable { get; }
        private double[] CpTable { get; }

        public PcTemplate(ClassId classId, StatsSet set) : base(set)
        {
            DefaultInventory = set.GetString("items")?.Split(';').Select(int.Parse).ToList() ?? new List<int>();
            ClassId = classId;

            BaseSwimSpeed = set.GetInt("swimSpd", 1);

            FallingHeight = set.GetInt("falling_height", 333);

            CollisionRadiusFemale = set.GetDouble("radiusFemale");
            CollisionHeightFemale = set.GetDouble("heightFemale");

            SpawnX = set.GetInt("spawnX");
            SpawnY = set.GetInt("spawnY");
            SpawnZ = set.GetInt("spawnZ");

            BaseLevel = set.GetInt("baseLvl");

            HpTable = set.GetString("hpTable").Split(';').Select(double.Parse).ToArray();
            MpTable = set.GetString("mpTable").Split(';').Select(double.Parse).ToArray();
            CpTable = set.GetString("cpTable").Split(';').Select(double.Parse).ToArray();
        }

        public override double GetBaseMaxHp(int level)
        {
            return HpTable[level - 1];
        }

        public override double GetBaseMaxMp(int level)
        {
            return MpTable[level - 1];
        }

        public double GetBaseMaxCp(int level)
        {
            return CpTable[level - 1];
        }
    }
}