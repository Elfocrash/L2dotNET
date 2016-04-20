using System;
using System.Data;
using L2dotNET.Game.db;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.player;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using MySql.Data.MySqlClient;

namespace L2dotNET.Game.network.l2recv
{
    class CharacterCreate : GameServerNetworkRequest
    {
        public CharacterCreate(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private String _name;
        private int _race;
        private byte _sex;
        private int _classId;
        private int _int;
        private int _str;
        private int _con;
        private int _men;
        private int _dex;
        private int _wit;
        private byte _hairStyle;
        private byte _hairColor;
        private byte _face;

        public override void read()
        {
            _name = readS();
            _race = readD();
            _sex = (byte)readD();
            _classId = readD();
            _int = readD();
            _str = readD();
            _con = readD();
            _men = readD();
            _dex = readD();
            _wit = readD();
            _hairStyle = (byte)readD();
            _hairColor = (byte)readD();
            _face = (byte)readD();
        }

        public override void run()
        {
            if (_name.Length < 2 || _name.Length > 16)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.TOO_LONG_16_CHARS));
                return;
            }

            if (getClient()._accountChars.Count > 7)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.TOO_MANY_CHARS_ON_ACCOUNT));
                return;
            }

            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT count(*) FROM user_data WHERE pname = '" + _name + "'";
            cmd.CommandType = CommandType.Text;

            MySqlDataReader reader = cmd.ExecuteReader();
            bool exists = false;
            while (reader.Read())
            {
                exists = reader.GetInt32(0) > 0;
            }

            reader.Close();
            connection.Close();

            if (exists)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.NAME_EXISTS));
                return;
            }

            PlayerTemplate template = PClassess.getInstance().getTemplate((byte)_classId);
            if (template == null)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.CREATION_RESTRICTION));
                return;
            }

            L2Player player = L2Player.create();
            player.Name = _name;

            player.Sex = _sex;
            player.BaseClass = template;
            player.ActiveClass = template;

            player.HairStyle = _hairStyle;
            player.HairColor = _hairColor;
            player.Face = _face;

            player.Gameclient = getClient();

            player.Exp = 0;
            player.CurHP = template._hp[player.Level];
            player.CurMP = template._mp[player.Level];
            player.CurCP = template._cp[player.Level];

            player.X = -71338;
            player.Y = 258271;
            player.Z = -3104;

            player.CStatsInit();
            player.CharacterStat.setTemplate(template);

            if (template._items != null)
            {
                player.Inventory = new InvPC();
                player.Inventory._owner = player;

                foreach (PC_item i in template._items)
                {
                    if (!i.item.isStackable())
                    {
                        for (long s = 0; s < i.count; s++)
                        {
                            L2Item item = new L2Item(i.item);
                            item.Enchant = i.enchant;
                            if (i.lifetime != -1)
                                item.AddLimitedHour(i.lifetime);

                            item.Location = L2Item.L2ItemLocation.inventory;
                            player.Inventory.addItem(item, false, false);

                            if (i.equip)
                            {
                                int pdollId = player.Inventory.getPaperdollId(item.Template);
                                player.setPaperdoll(pdollId, item, false);
                            }
                        }
                    }
                    else
                        player.addItem(i.item.ItemID, i.count);
                }
            }

            player._slotId = player.Gameclient._accountChars.Count + 1;
            player.sql_create();
            player.Gameclient._accountChars.Add(player);

            getClient().sendPacket(new CharCreateOk());
            CharacterSelectionInfo csl = new CharacterSelectionInfo(getClient().AccountName, getClient()._accountChars);
            csl.charId = player.ObjID;
            getClient().sendPacket(csl);
        }
    }
}
