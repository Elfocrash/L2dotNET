using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowAll : GameserverPacket
    {
        private readonly L2Party _party;

        public PartySmallWindowAll(L2Party party)
        {
            _party = party;
        }

        protected internal override void Write()
        {
            WriteByte(0x4e);
            WriteInt(_party.Leader.ObjId);
            WriteInt(_party.ItemDistribution);
            WriteInt(_party.Members.Count);

            foreach (L2Player member in _party.Members)
            {
                WriteInt(member.ObjId);
                WriteString(member.Name);

                WriteInt(member.CurCp);
                WriteInt(member.CharacterStat.GetStat(EffectType.BMaxCp));
                WriteInt(member.CurHp);
                WriteInt(member.CharacterStat.GetStat(EffectType.BMaxHp));
                WriteInt(member.CurMp);
                WriteInt(member.CharacterStat.GetStat(EffectType.BMaxMp));
                WriteInt(member.Level);

                WriteInt((int)member.ActiveClass.ClassId.Id);
                WriteInt(0x00); // writeD(0x01); ??
                WriteInt((int)member.BaseClass.ClassId.ClassRace);
            }
        }
    }
}