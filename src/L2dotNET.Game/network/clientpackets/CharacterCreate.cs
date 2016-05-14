using L2dotNET.GameService.model.inventory;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.templates;
using L2dotNET.GameService.world;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;
using System;

namespace L2dotNET.GameService.network.l2recv
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
            if (_name.Length > 16)
            {
                getClient().sendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.TOO_LONG_16_CHARS));
                return;
            }

            if (getClient().AccountChars.Count > 7)
            {
                getClient().sendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.TOO_MANY_CHARS_ON_ACCOUNT));
                return;
            }

            if (playerService.CheckIfPlayerNameExists(_name))
            {
                getClient().sendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.NAME_EXISTS));
                return;
            }

            PcTemplate template = CharTemplateTable.Instance.GetTemplate((byte)_classId);
            if (template == null)
            {
                getClient().sendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.CREATION_RESTRICTION));
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
            
            player.CStatsInit();
            player.CharacterStat.setTemplate(template);

            player.BaseClass = template;
            player.ActiveClass = template;

            player.CurHP = template.HpTable[player.Level];
            player.CurMP = template.MpTable[player.Level];
            player.CurCP = template.CpTable[player.Level];
            player.MaxMP = (int)player.CharacterStat.getStat(TEffectType.b_max_mp);
            player.MaxCP = (int)player.CharacterStat.getStat(TEffectType.b_max_cp);
            player.MaxHP = (int)player.CharacterStat.getStat(TEffectType.b_max_hp);

            player.X = 45901;
            player.Y = 41329;
            player.Z = -3508;

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

            player.CharSlot = player.Gameclient.AccountChars.Count;

            PlayerModel playerModel = new PlayerModel()
            {
                AccountName = player.AccountName,
                ObjectId = player.ObjID,
                Name = player.Name,
                Level = player.Level,
                MaxHp = (int)player.MaxHP,
                CurHp = (int)player.CurHP,
                MaxCp = player.MaxCP,
                CurCp = (int)player.CurCP,
                MaxMp = player.MaxMP,
                CurMp = (int)player.CurMP,
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
            player.Gameclient.AccountChars.Add(player);
            getClient().sendPacket(new CharCreateOk());
            L2World.Instance.RealiseEntry(player, null, true);
            CharacterSelectionInfo csl = new CharacterSelectionInfo(getClient().AccountName, getClient().AccountChars, getClient().SessionId);
            csl.charId = player.ObjID;
            getClient().sendPacket(csl);
        }
    }
}
