using L2dotNET.model.inventory;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Models;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.tables;
using L2dotNET.templates;
using L2dotNET.world;
using Ninject;

namespace L2dotNET.Network.clientpackets
{
    class CharacterCreate : PacketBase
    {
        [Inject]
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

        private readonly GameClient _client;
        private readonly string _name;
        private readonly int _race;
        private readonly byte _sex;
        private readonly int _classId;
        private readonly int _int;
        private readonly int _str;
        private readonly int _con;
        private readonly int _men;
        private readonly int _dex;
        private readonly int _wit;
        private readonly byte _hairStyle;
        private readonly byte _hairColor;
        private readonly byte _face;

        public CharacterCreate(Packet packet, GameClient client)
        {
            _client = client;
            _name = packet.ReadString();
            _race = packet.ReadInt();
            _sex = (byte)packet.ReadInt();
            _classId = packet.ReadInt();
            _int = packet.ReadInt();
            _str = packet.ReadInt();
            _con = packet.ReadInt();
            _men = packet.ReadInt();
            _dex = packet.ReadInt();
            _wit = packet.ReadInt();
            _hairStyle = (byte)packet.ReadInt();
            _hairColor = (byte)packet.ReadInt();
            _face = (byte)packet.ReadInt();
        }

        //TODO: Simplify method body
        public override void RunImpl()
        {
            if (_name.Length > 16)
            {
                _client.SendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.TooLong16Chars));
                return;
            }

            if (_client.AccountChars.Count > 7)
            {
                _client.SendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.TooManyCharsOnAccount));
                return;
            }

            if (PlayerService.CheckIfPlayerNameExists(_name))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.NameExists));
                return;
            }

            PcTemplate template = CharTemplateTable.Instance.GetTemplate((byte)_classId);
            if (template == null)
            {
                _client.SendPacket(new CharCreateFail(CharCreateFail.CharCreateFailReason.CreationRestriction));
                return;
            }

            L2Player player = L2Player.Create();
            player.Name = _name;
            player.AccountName = _client.AccountName;
            player.Title = string.Empty;
            player.Sex = _sex;

            player.HairStyle = _hairStyle;
            player.HairColor = _hairColor;
            player.Face = _face;
            player.Level = 1;
            player.Gameclient = _client;

            player.Exp = 0;

            player.CStatsInit();
            player.CharacterStat.SetTemplate(template);

            player.BaseClass = template;
            player.ActiveClass = template;

            player.CurHp = template.HpTable[player.Level];
            player.CurMp = template.MpTable[player.Level];
            player.CurCp = template.CpTable[player.Level];
            player.MaxMp = (int)player.CharacterStat.GetStat(EffectType.BMaxMp);
            player.MaxCp = (int)player.CharacterStat.GetStat(EffectType.BMaxCp);
            player.MaxHp = (int)player.CharacterStat.GetStat(EffectType.BMaxHp);

            player.X = template.SpawnX;
            player.Y = template.SpawnY;
            player.Z = template.SpawnZ;

            if (template.Items != null)
            {
                player.Inventory = new PcInventory(player);

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

            PlayerModel playerModel = new PlayerModel
            {
                AccountName = player.AccountName,
                ObjectId = player.ObjId,
                Name = player.Name,
                Level = player.Level,
                MaxHp = player.MaxHp,
                CurHp = (int)player.CurHp,
                MaxCp = player.MaxCp,
                CurCp = (int)player.CurCp,
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
                Sp = player.Sp,
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
                IsIn7SDungeon = player.IsIn7SDungeon,
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
            PlayerService.CreatePlayer(playerModel);
            player.Gameclient.AccountChars.Add(player);
            _client.SendPacket(new CharCreateOk());
            L2World.Instance.AddPlayer(player);
            _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId)
            {
                CharId = player.ObjId
            });
        }
    }
}