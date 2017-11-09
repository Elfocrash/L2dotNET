using System;
using System.Linq;
using L2dotNET.Enums;
using L2dotNET.model.inventory;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Models;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.tables;
using L2dotNET.templates;
using L2dotNET.Utility;
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
        private readonly int _sex;
        private readonly int _classId;
        private readonly int _hairStyle;
        private readonly int _hairColor;
        private readonly int _face;

        public CharacterCreate(Packet packet, GameClient client)
        {
            _client = client;
            _name = packet.ReadString();
            _race = packet.ReadInt();
            _sex = packet.ReadInt();
            _classId = packet.ReadInt();
            packet.ReadInt(); //INT
            packet.ReadInt(); //STR
            packet.ReadInt(); //CON
            packet.ReadInt(); //MEN
            packet.ReadInt(); //DEX
            packet.ReadInt(); //WIT
            _hairStyle = packet.ReadInt();
            _hairColor = packet.ReadInt();
            _face = (byte)packet.ReadInt();
        }

        //TODO: Simplify method body
        public override void RunImpl()
        {
            if (!IsValidChar())
                return;

            PcTemplate template = CharTemplateTable.Instance.GetTemplate(_classId);

            L2Player player = L2Player.Create();
            player.Name = _name;
            player.AccountName = _client.AccountName;
            player.Title = string.Empty;
            player.Sex = (Gender)_sex;
            player.HairStyleId = (HairStyleId)_hairStyle;
            player.HairColor = (HairColor)_hairColor;
            player.Face = (Face)_face;
            player.Exp = 0;
            player.Level = 1;
            player.Gameclient = _client;
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
            player.CharSlot = player.Gameclient.AccountChars.Count;

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
                Face = (int)player.Face,
                HairStyle = (int)player.HairStyleId,
                HairColor = (int)player.HairColor,
                Sex = (int)player.Sex,
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

        private bool IsValidChar()
        {
            if (Config.Config.Instance.GameplayConfig.OtherConfig.CharCreationBlocked)
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.CharCreationBlocked));
                return false;
            }

            if (_client.AccountChars.Count >= Config.Config.Instance.GameplayConfig.OtherConfig.MaxCharactersByAccount)
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.TooManyCharsOnAccount));
                return false;
            }

            if (Config.Config.Instance.GameplayConfig.OtherConfig.ForbiddenCharNames.Contains(_name))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.IncorrectName));
                return false;
            }

            if (!StringHelper.IsValidPlayerName(_name))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.InvalidNamePattern));
                return false;
            }

            if (PlayerService.CheckIfPlayerNameExists(_name))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.NameAlreadyExists));
                return false;
            }

            if (!Enum.IsDefined(typeof(ClassRace), _race) ||
                !Enum.IsDefined(typeof(HairStyleId), _hairStyle) ||
                !Enum.IsDefined(typeof(HairColor), _hairColor) ||
                !Enum.IsDefined(typeof(Face), _face))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            if (!HairStyle.Values.Any(filter => (filter.Id == (HairStyleId)_hairStyle) &&
                                                filter.Sex.Contains((Gender)_sex)))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            if (!ClassId.Values.Any(filter => (filter.Level() == 0) &&
                                            (filter.ClassRace == (ClassRace)_race) &&
                                            (filter.Id == (ClassIds)_classId)))
            {
                _client.SendPacket(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            return true;
        }
    }
}