using System.Collections.Generic;
using L2dotNET.Game.model.inventory;

namespace L2dotNET.Game.network.l2send
{
    class CharacterSelectionInfo : GameServerNetworkPacket
    {
        private List<L2Player> players;
        public int charId = -1;
        private string account;
        public CharacterSelectionInfo(string account, List<L2Player> players)
        {
            this.players = players;
            this.account = account;
        }

        protected internal override void write()
        {
            writeC(0x09);
            writeD(players.Count);

            writeD(0x07);
            writeC(0x00);

            foreach (L2Player player in players)
            {
                writeS(player.Name);
                writeD(player.ObjID);
                writeS(account);
                writeD(player.Gameclient._sessionId);

                writeD(player.ClanId);
                writeD(0x00); // ??
                writeD(player.Sex);
                writeD(player.BaseClass.race);

                writeD(player.BaseClass.id);
                writeD(0x00); // active ??
                writeD(0x00); // x
                writeD(0x00); // y
                writeD(0x00); // z

                writeF(player.CurHP); // hp cur
                writeF(player.CurMP); // mp cur
                writeD(player.SP);

                writeQ(player.Exp);

                if (player.Gameclient.Protocol > 250)
                    writeF(0.0); //annivers

                writeD(player.Level);
                writeD(player.Karma); // karma
                writeD(0);

                writeD(0);
                writeD(0);
                writeD(0);
                writeD(0);

                writeD(0);
                writeD(0);
                writeD(0);
                writeD(0);

                for (byte id = 0; id < InvPC.EQUIPITEM_Max; id++)
                {
                    writeD(player.Inventory._paperdoll[id][0]);
                }

                writeD(player.HairStyle);
                writeD(player.HairColor);

                writeD(player.Face);
                writeF(player.CurHP); // hp max TODO
                writeF(player.CurMP); // mp max TODO
                writeD(0); // days left before TODO

                writeD(player.ActiveClass.id);

                int selection = 0;

                if (charId != -1)
                    selection = charId == player.ObjID ? 1 : 0;

                if (charId == -1 && player.LastAccountSelection == 1)
                    selection = 1;

                writeD(selection); // auto-select char
                writeC(player.GetEnchantValue());
                writeH(0x00);

                writeH(0x00);
                writeD(0x00); //transform
                writeD(0x00);
                writeD(0x00);

                writeD(0x00);
                writeD(0x00);
                writeF(0.0);
                writeF(0.0);

                if (player.Gameclient.Protocol > 250)
                    writeD(0); //anniversary
            }
        }
    }
}
