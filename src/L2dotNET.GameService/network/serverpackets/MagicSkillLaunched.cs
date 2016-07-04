using System.Collections.Generic;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicSkillLaunched : GameServerNetworkPacket
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

        protected internal override void Write()
        {
            WriteC(0x76);
            WriteD(_casterId);
            WriteD(_id);
            WriteD(_level);
            WriteD(_targets.Length);
            foreach (int tid in _targets)
                WriteD(tid);
        }
    }
}