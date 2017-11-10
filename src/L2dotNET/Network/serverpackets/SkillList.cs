using System.Collections.Generic;
using L2dotNET.model.player;

namespace L2dotNET.Network.serverpackets
{
    class SkillList : GameserverPacket
    {
        public SkillList(L2Player player, int blockAct, int blockSpell, int blockSkill)
        {

        }

        public override void Write()
        {
            WriteByte(0x58);
        }
    }
}