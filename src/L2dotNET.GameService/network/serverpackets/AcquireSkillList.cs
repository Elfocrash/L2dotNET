using System.Collections.Generic;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AcquireSkillList : GameserverPacket
    {
        public enum SkillType
        {
            Usual = 0,
            Fishing = 1,
            Clan = 2
        }

        private List<AcquireSkill> _list;
        private readonly SkillType _skillType;

        public AcquireSkillList(SkillType type)
        {
            _skillType = type;
        }

        protected internal override void Write()
        {
            WriteByte(0x8a);
            WriteInt((int)_skillType);
            WriteInt(_list.Count);

            foreach (AcquireSkill sk in _list)
            {
                WriteInt(sk.Id);
                WriteInt(sk.Lv);
                WriteInt(sk.Lv);
                WriteInt(sk.LvUpSp);
                WriteInt(0); //reqs
            }
        }
    }
}