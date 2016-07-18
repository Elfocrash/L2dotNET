using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class SkillList : GameserverPacket
    {
        private readonly IList<Skill> _skills;
        private readonly int _blockAct;
        private readonly int _blockSpell;
        private readonly int _blockSkill;

        public SkillList(L2Player player, int blockAct, int blockSpell, int blockSkill)
        {
            _skills = player.Skills.Values;
            _blockAct = blockAct;
            _blockSpell = blockSpell;
            _blockSkill = blockSkill;
        }

        protected internal override void Write()
        {
            WriteByte(0x58);
            WriteInt(_skills.Count);

            foreach (Skill skill in _skills)
            {
                int passive = skill.IsPassive();
                WriteInt(passive);
                WriteInt(skill.Level);
                WriteInt(skill.SkillId);

                byte blocked = 0;
                if (passive == 0)
                {
                    if (_blockAct == 1)
                        blocked = 1;
                    else
                    {
                        switch (skill.IsMagic)
                        {
                            case 0:
                                blocked = (byte)_blockSkill;
                                break;
                            case 1:
                                blocked = (byte)_blockSpell;
                                break;
                            default:
                                blocked = 0;
                                break;
                        }
                    }
                }

                WriteByte(blocked);
            }
        }
    }
}