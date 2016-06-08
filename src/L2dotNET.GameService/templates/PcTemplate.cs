using System.Collections.Generic;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Templates
{
    public class PcTemplate : CharTemplate
    {
        public ClassId ClassId { get; set; }

        public int fallingHeight;

        public int FallingHeight
        {
            get { return fallingHeight; }
        }

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

            fallingHeight = set.GetInt("falling_height", 333);

            CollisionRadiusFemale = set.GetDouble("radiusFemale");
            CollisionHeightFemale = set.GetDouble("heightFemale");

            SpawnX = set.GetInt("spawnX");
            SpawnY = set.GetInt("spawnY");
            SpawnZ = set.GetInt("spawnZ");

            ClassBaseLevel = set.GetInt("baseLvl");

            string[] _hpTable = set.GetString("hpTable").Split(';');

            HpTable = new double[_hpTable.Length];

            for (int i = 0; i < _hpTable.Length; i++)
                HpTable[i] = double.Parse(_hpTable[i]);

            string[] _mpTable = set.GetString("mpTable").Split(';');

            MpTable = new double[_mpTable.Length];
            for (int i = 0; i < _mpTable.Length; i++)
                MpTable[i] = double.Parse(_mpTable[i]);

            string[] _cpTable = set.GetString("cpTable").Split(';');

            CpTable = new double[_cpTable.Length];
            for (int i = 0; i < _cpTable.Length; i++)
                CpTable[i] = double.Parse(_cpTable[i]);
        }
    }
}