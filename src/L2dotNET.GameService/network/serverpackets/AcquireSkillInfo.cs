using System.Collections.Generic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AcquireSkillInfo : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _level;
        private readonly int _spCost;
        private readonly int _mode;
        public List<int[]> Reqs = new List<int[]>();

        public AcquireSkillInfo(int id, int level, int sp, int skillType)
        {
            _id = id;
            _level = level;
            _spCost = sp;
            _mode = skillType;
        }

        protected internal override void Write()
        {
            WriteC(0x8b);
            WriteD(_id);
            WriteD(_level);
            WriteD(_spCost);
            WriteD(_mode);

            WriteD(Reqs.Count);

            foreach (int[] r in Reqs)
            {
                WriteD(r[0]);
                WriteD(r[1]);
                WriteD(r[2]);
                WriteD(r[3]);
            }
        }
    }
}