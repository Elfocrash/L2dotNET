using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AcquireSkillList : GameServerNetworkPacket
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
            WriteC(0x8a);
            WriteD((int)_skillType);
            WriteD(_list.Count);

            foreach (AcquireSkill sk in _list)
            {
                WriteD(sk.Id);
                WriteD(sk.Lv);
                WriteD(sk.Lv);
                WriteD(sk.LvUpSp);
                WriteD(0); //reqs
            }
        }
    }
}