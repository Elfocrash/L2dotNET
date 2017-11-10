using System.Collections.Generic;

namespace L2dotNET.Network.serverpackets
{
    class AcquireSkillList : GameserverPacket
    {
        public enum SkillType
        {
            Usual = 0,
            Fishing = 1,
            Clan = 2
        }

        private readonly SkillType _skillType;

        public AcquireSkillList(SkillType type)
        {
            _skillType = type;
        }

        public override void Write()
        {
            WriteByte(0x8a);
            //WriteInt((int)_skillType);
            //WriteInt(_list.Count);

            //foreach (AcquireSkill sk in _list)
            //{
            //    WriteInt(sk.Id);
            //    WriteInt(sk.Lv);
            //    WriteInt(sk.Lv);
            //    WriteInt(sk.LvUpSp);
            //    WriteInt(0); //reqs
            //}
        }
    }
}