using System.Collections.Generic;

namespace L2dotNET.Network.serverpackets
{
    class AcquireSkillInfo : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0x8b);
            WriteInt(_id);
            WriteInt(_level);
            WriteInt(_spCost);
            WriteInt(_mode);

            WriteInt(Reqs.Count);

            foreach (int[] r in Reqs)
            {
                WriteInt(r[0]);
                WriteInt(r[1]);
                WriteInt(r[2]);
                WriteInt(r[3]);
            }
        }
    }
}