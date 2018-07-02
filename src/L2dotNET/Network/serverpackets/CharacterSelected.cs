﻿using L2dotNET.Controllers;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class CharacterSelected : GameserverPacket
    {
        private readonly int _session;
        private readonly L2Player _player;

        public CharacterSelected(L2Player player, int session)
        {
            _player = player;
            _session = session;
        }

        public override void Write()
        {
            WriteByte(0x15);

            WriteString(_player.Name);
            WriteInt(_player.ObjId);
            WriteString(_player.Title);
            WriteInt(_session);

            WriteInt(0);//_player.ClanId
            WriteInt(0x00); //??
            WriteInt((int)_player.Sex);
            WriteInt((int)_player.BaseClass.ClassId.ClassRace);

            WriteInt((int)_player.ActiveClass.ClassId.Id);
            WriteInt(0x01); // active ??
            WriteInt(_player.X);
            WriteInt(_player.Y);

            WriteInt(_player.Z);
            WriteDouble(_player.CharStatus.CurrentHp);
            WriteDouble(_player.CharStatus.CurrentMp);
            WriteInt(_player.Sp);

            WriteLong(_player.Exp);
            WriteInt(_player.Level);
            WriteInt(_player.Karma);
            WriteInt(0); //?

            WriteInt(_player.Int);
            WriteInt(_player.Str);
            WriteInt(_player.Con);
            WriteInt(_player.Men);

            WriteInt(_player.Dex);
            WriteInt(_player.Wit);
            for (int i = 0; i < 30; i++)
                WriteInt(0x00);

            WriteInt(0x00); // c3 work
            WriteInt(0x00); // c3 work

            WriteInt(GameTime.IngameTime);

            WriteInt(0x00); // c3

            WriteInt((int)_player.ActiveClass.ClassId.Id);

            WriteInt(0x00); // c3 InspectorBin
            WriteInt(0x00); // c3
            WriteInt(0x00); // c3
            WriteInt(0x00); // c3
        }
    }
}