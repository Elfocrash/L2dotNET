using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SkillCoolTime : GameserverPacket
    {
        private readonly ICollection<L2SkillCoolTime> _list;

        public SkillCoolTime(L2Player player)
        {
            _list = player.Reuse.Values;
        }

        protected internal override void Write()
        {
            WriteByte(0xc1);
            WriteInt(_list.Count);

            foreach (L2SkillCoolTime ct in _list)
            {
                WriteInt(ct.Id);
                WriteInt(ct.Lvl);
                WriteInt(ct.Total);
                WriteInt(ct.GetDelay());
            }
        }
    }
}