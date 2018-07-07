﻿using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Inventory;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class CharList : GameserverPacket
    {
        private readonly GameClient _client;
        private readonly int _sessionId;

        public CharList(GameClient client, int sessionId)
        {
            _client = client;
            _sessionId = sessionId;
        }

        public override void Write()
        {
            WriteByte(0x13);
            WriteInt(_client.AccountCharacters.Count);

            int lastUsedCharId = 0;

            if (_client.AccountCharacters.Count > 0)
            {
                L2Player lastUsedChar = _client.AccountCharacters.OrderByDescending(sort => sort.LastAccess)
                    .FirstOrDefault(filter => !filter.DeleteTime.HasValue);

                lastUsedCharId = lastUsedChar?.ObjectId ?? 0;
            }

            foreach (L2Player player in _client.AccountCharacters)
            {
                WriteString(player.Name);
                WriteInt(player.ObjectId);
                WriteString(_client.Account.Login);
                WriteInt(_sessionId);
                WriteInt(0);//player.ClanId
                WriteInt(0x00); // ??   

                WriteInt((int)player.Sex);
                WriteInt((int)player.BaseClass.ClassId.ClassRace);

                if (player.ActiveClass.ClassId.Id == player.BaseClass.ClassId.Id)
                {
                    WriteInt((int)player.ActiveClass.ClassId.Id);
                }
                else
                {
                    WriteInt((int)player.BaseClass.ClassId.Id);
                }

                WriteInt(0x01); // active ??

                WriteInt(player.X);
                WriteInt(player.Y);
                WriteInt(player.Z);

                WriteDouble(player.CharStatus.CurrentHp);
                WriteDouble(player.CharStatus.CurrentMp);

                WriteInt(player.Sp);
                WriteLong(player.Experience);

                WriteInt(player.Level);
                WriteInt(player.Karma);
                WriteInt(player.PkKills);
                WriteInt(player.PvpKills);

                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteInt(player.Inventory.Paperdoll[id]?.Template?.ItemId ?? 0);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteInt(player.Inventory.Paperdoll[id]?.Template?.ItemId ?? 0);

                WriteInt((int)player.HairStyleId);
                WriteInt((int)player.HairColor);

                WriteInt((int)player.Face);
                WriteDouble(player.MaxHp);
                WriteDouble(player.MaxMp);
                WriteInt(player.RemainingDeleteTime());

                WriteInt((int)player.ActiveClass.ClassId.Id);

                int selection = 0;

                if (lastUsedCharId > 0 && lastUsedCharId == player.ObjectId)
                {
                    selection = 1;
                }

                WriteInt(selection); // auto-select char
                WriteByte(player.GetEnchantValue());
                WriteInt(0x00); // augment
            }
        }
    }
}