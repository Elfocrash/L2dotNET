using System;
using System.Data;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.player;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using MySql.Data.MySqlClient;
using L2dotNET.Services.Contracts;
using Ninject;
using L2dotNET.Models;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.templates;

namespace L2dotNET.Game.network.l2recv
{
    class CharacterCreate : GameServerNetworkRequest
    {
        [Inject]
        public IPlayerService playerService { get { return GameServer.Kernel.Get<IPlayerService>(); } }

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

            bool exists = playerService.CheckIfPlayerNameExists(_name);

            if (exists)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.NAME_EXISTS));
                return;
            }

            PcTemplate template = CharTemplateTable.Instance.GetTemplate((byte)_classId);
            if (template == null)
            {
                getClient().sendPacket(new CharCreateFail(getClient(), CharCreateFail.CharCreateFailReason.CREATION_RESTRICTION));
                return;
            }

            L2Player player = L2Player.create();
            player.Name = _name;
            player.AccountName = getClient().AccountName;
            player.Title = "";
            player.Sex = _sex;

            player.HairStyle = _hairStyle;
            player.HairColor = _hairColor;
            player.Face = _face;
            player.Level = 1;
            player.Gameclient = getClient();

            player.Exp = 0;
            
            //player.MaximumHp = template._hp[player.Level];
            player.CStatsInit();
            player.CharacterStat.setTemplate(template);

            player.BaseClass = template;
            player.ActiveClass = template;

            player.CurHP = template.HpTable[player.Level];
            player.CurMP = template.MpTable[player.Level];
            player.CurCP = template.CpTable[player.Level];
            player.MaxMp = (int)player.CharacterStat.getStat(TEffectType.b_max_mp);
            player.MaxCp = (int)player.CharacterStat.getStat(TEffectType.b_max_cp);

            player.X = -71338;
            player.Y = 258271;
            player.Z = -3104;

            

            if (template.Items != null)
            {
                player.Inventory = new InvPC();
                player.Inventory._owner = player;

                //foreach (PC_item i in template._items)
                //{
                //    if (!i.item.isStackable())
                //    {
                //        for (long s = 0; s < i.count; s++)
                //        {
                //            L2Item item = new L2Item(i.item);
                //            item.Enchant = i.enchant;
                //            if (i.lifetime != -1)
                //                item.AddLimitedHour(i.lifetime);

                //            item.Location = L2Item.L2ItemLocation.inventory;
                //            player.Inventory.addItem(item, false, false);

                //            if (i.equip)
                //            {
                //                int pdollId = player.Inventory.getPaperdollId(item.Template);
                //                player.setPaperdoll(pdollId, item, false);
                //            }
                //        }
                //    }
                //    else
                //        player.addItem(i.item.ItemID, i.count);
                //}
            }

            player.CharSlot = player.Gameclient._accountChars.Count + 1;

            PlayerModel playerModel = new PlayerModel()
            {
                AccountName = player.AccountName,
                ObjectId = player.ObjID,
                Name = player.Name,
                Level = player.Level,
                MaxHp = (int)player.MaximumHp,
                CurHp = (int)player.MaximumHp,
                MaxCp = player.MaxCp,
                CurCp = player.MaxCp,
                MaxMp = player.MaxMp,
                CurMp = (int)player.CurMp,
                Face = player.Face,
                HairStyle = player.HairStyle,
                HairColor = player.HairColor,
                Sex = player.Sex,
                Heading = player.Heading,
                X = player.X,
                Y = player.Y,
                Z = player.Z,
                Exp = player.Exp,
                ExpBeforeDeath = player.ExpOnDeath,
                Sp = player.SP,
                Karma = player.Karma,
                PvpKills = player.PvpKills,
                PkKills = player.PkKills,
                ClanId = player.ClanId,
                Race = (int)player.BaseClass.ClassId.ClassRace,
                ClassId = (int)player.ActiveClass.ClassId.Id,
                BaseClass = (int)player.BaseClass.ClassId.Id,
                DeleteTime = player.DeleteTime,
                CanCraft = player.CanCraft,
                Title = player.Title,
                RecHave = player.RecHave,
                RecLeft = player.RecLeft,
                AccessLevel = player.AccessLevel,
                Online = player.Online,
                OnlineTime = player.OnlineTime,
                CharSlot = player.CharSlot,
                LastAccess = player.LastAccess,
                ClanPrivs = player.ClanPrivs,
                WantsPeace = player.WantsPeace,
                IsIn7sDungeon = player.IsIn7sDungeon,
                PunishLevel = player.PunishLevel,
                PunishTimer = player.PunishTimer,
                PowerGrade = player.PowerGrade,
                Nobless = player.Nobless,
                Hero = player.Hero,
                Subpledge = player.Subpledge,
                LastRecomDate = player.LastRecomDate,
                LevelJoinedAcademy = player.LevelJoinedAcademy,
                Apprentice = player.Apprentice,
                Sponsor = player.Sponsor,
                VarkaKetraAlly = player.VarkaKetraAlly,
                ClanJoinExpiryTime = player.ClanJoinExpiryTime,
                ClanCreateExpiryTime = player.ClanCreateExpiryTime,
                DeathPenaltyLevel = player.DeathPenaltyLevel
            };
            playerService.CreatePlayer(playerModel);
            player.Gameclient._accountChars.Add(player);
            getClient().sendPacket(new CharCreateOk());
            CharacterSelectionInfo csl = new CharacterSelectionInfo(getClient().AccountName, getClient()._accountChars, getClient()._sessionId);
            csl.charId = player.ObjID;
            getClient().sendPacket(csl);
        }
    }
}
