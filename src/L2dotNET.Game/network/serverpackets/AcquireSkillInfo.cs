using System.Collections.Generic;

namespace L2dotNET.GameService.network.serverpackets
{
    class AcquireSkillInfo : GameServerNetworkPacket
    {
        private readonly int _id;
        private readonly int _level;
        private readonly int _spCost;
        private readonly int _mode;
        public List<int[]> _reqs = new List<int[]>();

        public AcquireSkillInfo(int _id, int _level, int sp, int _skillType)
        {
            this._id = _id;
            this._level = _level;
            this._spCost = sp;
            this._mode = _skillType;
        }

        protected internal override void write()
        {
            writeC(0x8b);
            writeD(_id);
            writeD(_level);
            writeD(_spCost);
            writeD(_mode);

            writeD(_reqs.Count);

            foreach (int[] r in _reqs)
            {
                writeD(r[0]);
                writeD(r[1]);
                writeD(r[2]);
                writeD(r[3]);
            }
        }
    }
}