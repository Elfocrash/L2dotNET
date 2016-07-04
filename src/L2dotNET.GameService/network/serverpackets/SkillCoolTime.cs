using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SkillCoolTime : GameServerNetworkPacket
    {
        private readonly ICollection<L2SkillCoolTime> list;

        public SkillCoolTime(L2Player player)
        {
            list = player.Reuse.Values;
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