using System.Collections.Generic;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.skills;

namespace L2dotNET.GameService.network.serverpackets
{
    class SkillCoolTime : GameServerNetworkPacket
    {
        private readonly ICollection<L2SkillCoolTime> list;

        public SkillCoolTime(L2Player player)
        {
            list = player._reuse.Values;
        }

        protected internal override void write()
        {
            writeC(0xc1);
            writeD(list.Count);

            foreach (L2SkillCoolTime ct in list)
            {
                writeD(ct.id);
                writeD(ct.lvl);
                writeD(ct.total);
                writeD(ct.getDelay());
            }
        }
    }
}