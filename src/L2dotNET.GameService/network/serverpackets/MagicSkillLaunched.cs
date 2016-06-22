using System.Collections.Generic;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicSkillLaunched : GameServerNetworkPacket
    {
        private readonly int _level;
        private readonly int _id;
        private readonly int CasterId;
        private readonly int[] Targets;

        public MagicSkillLaunched(L2Character caster, List<int> targets, int id, int lvl)
        {
            _id = id;
            _level = lvl;
            Targets = targets.ToArray();
            CasterId = caster.ObjID;
        }

        /// <summary>
        /// self visual cast
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skId"></param>
        /// <param name="skLv"></param>
        public MagicSkillLaunched(int id, int skId, int skLv)
        {
            CasterId = id;
            _id = skId;
            _level = skLv;
            Targets = new[] { id };
        }

        protected internal override void write()
        {
            writeC(0x76);
            writeD(CasterId);
            writeD(_id);
            writeD(_level);
            writeD(Targets.Length);
            foreach (int tid in Targets)
                writeD(tid);
        }
    }
}