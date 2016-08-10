using System.Collections.Generic;
using L2dotNET.model.player;
using L2dotNET.model.skills;

namespace L2dotNET.Network.serverpackets
{
    class SkillCoolTime : GameserverPacket
    {
        private readonly ICollection<L2SkillCoolTime> _list;

        public SkillCoolTime(L2Player player)
        {
            _list = player.Reuse.Values;
        }

        public override void Write()
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