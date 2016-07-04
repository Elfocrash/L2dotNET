using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowAll : GameServerNetworkPacket
    {
        private readonly L2Party _party;

        public PartySmallWindowAll(L2Party party)
        {
            this._party = party;
        }

        protected internal override void Write()
        {
            WriteC(0x4e);
            WriteD(_party.Leader.ObjId);
            WriteD(_party.ItemDistribution);
            WriteD(_party.Members.Count);

            foreach (L2Player member in _party.Members)
            {
                WriteD(member.ObjId);
                WriteS(member.Name);

                WriteD(member.CurCp);
                WriteD(member.CharacterStat.GetStat(EffectType.BMaxCp));
                WriteD(member.CurHp);
                WriteD(member.CharacterStat.GetStat(EffectType.BMaxHp));
                WriteD(member.CurMp);
                WriteD(member.CharacterStat.GetStat(EffectType.BMaxMp));
                WriteD(member.Level);

                WriteD((int)member.ActiveClass.ClassId.Id);
                WriteD(0x00); // writeD(0x01); ??
                WriteD((int)member.BaseClass.ClassId.ClassRace);
            }
        }
    }
}