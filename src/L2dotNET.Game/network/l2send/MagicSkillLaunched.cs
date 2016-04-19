using System.Collections.Generic;
using L2dotNET.Game.world;

namespace L2dotNET.Game.network.l2send
{
    public class MagicSkillLaunched : GameServerNetworkPacket
    {
        private int _level;
        private int _id;
        private int CasterId;
        private int[] Targets;

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
            this.CasterId = id;
            this._id = skId;
            this._level = skLv;
            Targets = new int[] { id };
        }

        protected internal override void write()
        {
            writeC(0x54);
            writeD(CasterId);
            writeD(_id);
            writeD(_level);
            writeD(Targets.Length);
            foreach (int tid in Targets)
                writeD(tid);
            
        }
    }
}
