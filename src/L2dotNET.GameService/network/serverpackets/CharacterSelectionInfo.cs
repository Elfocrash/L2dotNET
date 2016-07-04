using System.Collections.Generic;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharacterSelectionInfo : GameServerNetworkPacket
    {
        private readonly List<L2Player> _players;
        public int CharId = -1;
        private readonly string _account;
        private readonly int _sessionId;

        public CharacterSelectionInfo(string account, List<L2Player> players, int sessionId)
        {
            this._players = players;
            this._account = account;
            this._sessionId = sessionId;
        }

        protected internal override void Write()
        {
            WriteC(0x13);
            WriteD(_players.Count);

            foreach (L2Player player in _players)
            {
                WriteS(player.Name);
                WriteD(player.ObjId);
                WriteS(_account);
                WriteD(_sessionId);
                WriteD(player.ClanId);
                WriteD(0x00); // ??

                WriteD(player.Sex);
                WriteD((int)player.BaseClass.ClassId.ClassRace);

                if (player.ActiveClass.ClassId.Id == player.BaseClass.ClassId.Id)
                    WriteD((int)player.ActiveClass.ClassId.Id);
                else
                    WriteD((int)player.BaseClass.ClassId.Id);

                WriteD(0x01); // active ??

                WriteD(player.X);
                WriteD(player.Y);
                WriteD(player.Z);

                WriteF(player.CurHp);
                WriteF(player.CurMp);

                WriteD(player.Sp);
                WriteQ(player.Exp);

                WriteD(player.Level);
                WriteD(player.Karma);
                WriteD(player.PkKills);
                WriteD(player.PvpKills);

                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(0);
                WriteD(0);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteD(player.Inventory.Paperdoll[id].Template.ItemId);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteD(player.Inventory.Paperdoll[id].Template.ItemId);

                WriteD(player.HairStyle);
                WriteD(player.HairColor);

                WriteD(player.Face);
                WriteF(player.CurHp); // hp max TODO
                WriteF(player.CurMp); // mp max TODO
                WriteD(0); // days left before TODO

                WriteD((int)player.ActiveClass.ClassId.Id);

                int selection = 0;

                if (CharId != -1)
                    selection = CharId == player.ObjId ? 1 : 0;

                if ((CharId == -1) && (player.LastAccountSelection == 1))
                    selection = 1;

                WriteD(selection); // auto-select char
                WriteC(player.GetEnchantValue());
                WriteD(0x00); // augment
            }
        }
    }
}