﻿using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class PartySmallWindowUpdate : GameserverPacket
    {
        private readonly L2Player _member;

        public PartySmallWindowUpdate(L2Player member)
        {
            _member = member;
        }

        public override void Write()
        {
            WriteByte(0x52);
            WriteInt(_member.ObjectId);
            WriteString(_member.Name);
            WriteInt(_member.CurrentCp);
            WriteInt(_member.MaxCp);
            WriteInt(_member.CharStatus.CurrentHp);
            WriteInt(_member.MaxHp);
            WriteInt(_member.CharStatus.CurrentMp);
            WriteInt(_member.MaxMp);
            WriteInt(_member.Level);
            WriteInt((int)_member.ActiveClass.ClassId.Id);
        }
    }
}