using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SkillCoolTime : GameServerNetworkPacket
    {
        private readonly ICollection<L2SkillCoolTime> _list;

        public SkillCoolTime(L2Player player)
        {
            _list = player.Reuse.Values;
        }

        protected internal override void Write()
        {
            WriteC(0xc1);
            WriteD(_list.Count);

            foreach (L2SkillCoolTime ct in _list)
            {
                WriteD(ct.Id);
                WriteD(ct.Lvl);
                WriteD(ct.Total);
                WriteD(ct.GetDelay());
            }
        }
    }
}