using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class PartySmallWindowAll : GameserverPacket
    {
        private readonly L2Party _party;

        public PartySmallWindowAll(L2Party party)
        {
            _party = party;
        }

        public override void Write()
        {
            WriteByte(0x4e);
            WriteInt(_party.Leader.ObjectId);
            WriteInt(_party.ItemDistribution);
            WriteInt(_party.Members.Count);

            foreach (L2Player member in _party.Members)
            {
                WriteInt(member.ObjectId);
                WriteString(member.Name);

                WriteInt(member.CurrentCp);
                WriteInt(member.MaxCp);
                WriteInt(member.CharStatus.CurrentHp);
                WriteInt(member.MaxHp);
                WriteInt(member.CharStatus.CurrentMp);
                WriteInt(member.MaxMp);
                WriteInt(member.Level);

                WriteInt((int)member.ActiveClass.ClassId.Id);
                WriteInt(0x00); // writeD(0x01); ??
                WriteInt((int)member.BaseClass.ClassId.ClassRace);
            }
        }
    }
}