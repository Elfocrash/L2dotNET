using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Models.Inventory;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Tables;
using L2dotNET.Templates;
using L2dotNET.Utility;
using L2dotNET.World;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class CharacterCreate : PacketBase
    {
        private readonly ICharacterService _characterService;
        private readonly IItemService _itemService;
        private readonly Config.Config _config;

        private readonly IdFactory _idFactory;
        private readonly ItemTable _itemTable;
        private readonly ICrudService<ItemContract> _itemCrudService;
        private readonly GameClient _client;
        private readonly string _name;
        private readonly int _race;
        private readonly int _sex;
        private readonly int _classId;
        private readonly int _hairStyle;
        private readonly int _hairColor;
        private readonly int _face;

        public CharacterCreate(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _config = serviceProvider.GetService<Config.Config>();
            _itemService = serviceProvider.GetService<IItemService>();
            _itemTable = serviceProvider.GetService<ItemTable>();
            _idFactory = serviceProvider.GetService<IdFactory>();
            _characterService = serviceProvider.GetService<ICharacterService>();
            _itemCrudService = serviceProvider.GetService<ICrudService<ItemContract>>();

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
        public override async Task RunImpl()
        {
            if (!await IsValidChar())
            {
                return;
            }

            PcTemplate template = CharTemplateTable.GetTemplate(_classId);

            L2Player player = new L2Player(template, _idFactory.NextId())
                {
                    Name = _name,
                    Account = _client.Account,
                    Sex = (Gender) _sex,
                    HairStyleId = (HairStyleId) _hairStyle,
                    HairColor = (HairColor) _hairColor,
                    Face = (Face) _face,
                    Gameclient = _client,
                    CharacterSlot = _client.AccountCharacters.Count
                };

            _characterService.CreatePlayer(player);
            player.Gameclient.AccountCharacters.Add(player);
            _client.SendPacketAsync(new CharCreateOk());
            L2World.AddPlayer(player);
            _client.SendPacketAsync(new CharList(_client.Account.Login, _client.AccountCharacters, _client.SessionKey.PlayOkId1));
        }

        private async Task<bool> IsValidChar()
        {
            if (_config.GameplayConfig.OtherConfig.CharCreationBlocked)
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.CharCreationBlocked));
                return false;
            }

            if (_client.AccountCharacters.Count >= _config.GameplayConfig.OtherConfig.MaxCharactersByAccount)
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.TooManyCharsOnAccount));
                return false;
            }

            if (_config.GameplayConfig.OtherConfig.ForbiddenCharNames.Contains(_name))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.IncorrectName));
                return false;
            }

            if (!StringHelper.IsValidPlayerName(_name))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.InvalidNamePattern));
                return false;
            }

            if (await _characterService.CheckIfPlayerNameExists(_name))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.NameAlreadyExists));
                return false;
            }

            if (!Enum.IsDefined(typeof(ClassRace), _race) ||
                !Enum.IsDefined(typeof(HairStyleId), _hairStyle) ||
                !Enum.IsDefined(typeof(HairColor), _hairColor) ||
                !Enum.IsDefined(typeof(Face), _face))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            if (!HairStyle.Values.Any(filter => (filter.Id == (HairStyleId)_hairStyle) &&
                                                filter.Sex.Contains((Gender)_sex)))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            if (!ClassId.Values.Any(filter => (filter.Level() == 0) &&
                                            (filter.ClassRace == (ClassRace)_race) &&
                                            (filter.Id == (ClassIds)_classId)))
            {
                _client.SendPacketAsync(new CharCreateFail(CharCreateFailReason.CreationFailed));
                return false;
            }

            return true;
        }
    }
}