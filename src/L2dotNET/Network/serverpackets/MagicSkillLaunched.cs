using System.Collections.Generic;
using L2dotNET.world;

namespace L2dotNET.Network.serverpackets
{
    public class MagicSkillLaunched : GameserverPacket
    {
        private readonly int _level;
        private readonly int _id;
        private readonly int _casterId;
        private readonly int[] _targets;

        public MagicSkillLaunched(L2Character caster, List<int> targets, int id, int lvl)
        {
            _id = id;
            _level = lvl;
            _targets = targets.ToArray();
            _casterId = caster.ObjId;
        }

        /// <summary>
        /// self visual cast
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skId"></param>
        /// <param name="skLv"></param>
        public MagicSkillLaunched(int id, int skId, int skLv)
        {
            _casterId = id;
            _id = skId;
            _level = skLv;
            _targets = new[] { id };
        }

        public override void Write()
        {
            WriteByte(0x76);
            WriteInt(_casterId);
            WriteInt(_id);
            WriteInt(_level);
            WriteInt(_targets.Length);
            foreach (int tid in _targets)
                WriteInt(tid);
        }
    }
}