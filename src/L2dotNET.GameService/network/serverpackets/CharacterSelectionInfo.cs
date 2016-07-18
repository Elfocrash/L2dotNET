using System.Collections.Generic;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharacterSelectionInfo : GameserverPacket
    {
        private readonly List<L2Player> _players;
        public int CharId = -1;
        private readonly string _account;
        private readonly int _sessionId;

        public CharacterSelectionInfo(string account, List<L2Player> players, int sessionId)
        {
            _players = players;
            _account = account;
            _sessionId = sessionId;
        }

        protected internal override void Write()
        {
            WriteByte(0x13);
            WriteInt(_players.Count);

            foreach (L2Player player in _players)
            {
                WriteString(player.Name);
                WriteInt(player.ObjId);
                WriteString(_account);
                WriteInt(_sessionId);
                WriteInt(player.ClanId);
                WriteInt(0x00); // ??

                WriteInt(player.Sex);
                WriteInt((int)player.BaseClass.ClassId.ClassRace);

                if (player.ActiveClass.ClassId.Id == player.BaseClass.ClassId.Id)
                    WriteInt((int)player.ActiveClass.ClassId.Id);
                else
                    WriteInt((int)player.BaseClass.ClassId.Id);

                WriteInt(0x01); // active ??

                WriteInt(player.X);
                WriteInt(player.Y);
                WriteInt(player.Z);

                WriteDouble(player.CurHp);
                WriteDouble(player.CurMp);

                WriteInt(player.Sp);
                WriteLong(player.Exp);

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

                WriteInt(player.HairStyle);
                WriteInt(player.HairColor);

                WriteInt(player.Face);
                WriteDouble(player.CurHp); // hp max TODO
                WriteDouble(player.CurMp); // mp max TODO
                WriteInt(0); // days left before TODO

                WriteInt((int)player.ActiveClass.ClassId.Id);

                int selection = 0;

                if (CharId != -1)
                    selection = CharId == player.ObjId ? 1 : 0;

                if ((CharId == -1) && (player.LastAccountSelection == 1))
                    selection = 1;

                WriteInt(selection); // auto-select char
                WriteByte(player.GetEnchantValue());
                WriteInt(0x00); // augment
            }
        }
    }
}