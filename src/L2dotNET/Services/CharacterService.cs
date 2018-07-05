using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using L2dotNET.DataContracts;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Inventory;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.General;
using L2dotNET.Tables;

namespace L2dotNET.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICrudService<CharacterContract> _characterCrudService;
        private readonly ICharacterRepository _characterRepository;
        private readonly ICrudService<ItemContract> _itemCrudService;
        private readonly IItemService _itemService;
        private readonly Config.Config _config;
        private readonly IdFactory _idFactory;
        private readonly ItemTable _itemTable;

        public CharacterService(ICrudService<ItemContract> itemCrudService,
            ICrudService<CharacterContract> characterCrudService,
            ICharacterRepository characterRepository,
            IItemService itemService,
            IdFactory idFactory,
            ItemTable itemTable,
            Config.Config config)
        {
            _itemCrudService = itemCrudService;
            _itemService = itemService;
            _idFactory = idFactory;
            _itemTable = itemTable;
            _config = config;
            _characterCrudService = characterCrudService;
            _characterRepository = characterRepository;
        }

        public async Task<L2Player> GetById(int characterId)
        {
            var characterContract = await _characterCrudService.GetById(characterId);

            return MapContractToPlayer(characterContract);
        }

        public async Task<bool> CheckIfPlayerNameExists(string name)
        {
            return await _characterRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(L2Player player)
        {
            CharacterContract playerContract = MapPlayerToContract(player);

            _characterCrudService.Add(playerContract);
        }

        public void UpdatePlayer(L2Player player)
        {
            CharacterContract characterContract = MapPlayerToContract(player);

            _characterCrudService.Update(characterContract);
        }

        private CharacterContract MapPlayerToContract(L2Player player)
        {
            return new CharacterContract
                {
                    AccountId = player.Account.AccountId,
                    CharacterId = player.CharacterId,
                    Name = player.Name,
                    Level = player.Level,
                    MaxHp = player.MaxHp,
                    CurHp = (int) player.CharStatus.CurrentHp,
                    MaxCp = player.MaxCp,
                    CurCp = (int) player.CurrentCp,
                    MaxMp = player.MaxMp,
                    CurMp = (int) player.CharStatus.CurrentMp,
                    Face = (byte) player.Face,
                    HairStyle = (byte) player.HairStyleId,
                    HairColor = (byte) player.HairColor,
                    Sex = (byte) player.Sex,
                    Heading = player.Heading,
                    X = player.X,
                    Y = player.Y,
                    Z = player.Z,
                    Exp = player.Experience,
                    ExpBeforeDeath = player.ExpOnDeath,
                    Sp = player.Sp,
                    Karma = player.Karma,
                    PvpKills = player.PvpKills,
                    PkKills = player.PkKills,
                    Race = (int) player.BaseClass.ClassId.ClassRace,
                    ClassId = (int) player.ActiveClass.ClassId.Id,
                    BaseClass = (int) player.BaseClass.ClassId.Id,
                    DeleteTime = player.DeleteTime,
                    CanCraft = player.CanCraft,
                    Title = player.Title,
                    RecHave = player.RecomandationsHave,
                    RecLeft = player.RecomendationsLeft,
                    AccessLevel = player.AccessLevel,
                    Online = player.Online,
                    OnlineTime = player.OnlineTime,
                    CharSlot = player.CharacterSlot,
                    LastAccess = player.LastAccess,
                    PunishLevel = player.PunishLevel,
                    PunishTimer = player.PunishTimer,
                    PowerGrade = player.PowerGrade,
                    Nobless = player.Nobless,
                    Hero = player.Hero,
                    LastRecomDate = player.LastRecomendationDate
                };
        }

        private L2Player MapContractToPlayer(CharacterContract characterContract)
        {
            var player = new L2Player(this, characterContract.CharacterId, CharTemplateTable.GetTemplate(characterContract.ClassId))
                {
                    CharacterId = characterContract.CharacterId,
                    Name = characterContract.Name,
                    Title = characterContract.Title,
                    Level = (byte) characterContract.Level,
                    Face = (Face) characterContract.Face,
                    HairStyleId = (HairStyleId) characterContract.HairStyle,
                    HairColor = (HairColor) characterContract.HairColor,
                    Sex = (Gender) characterContract.Sex,
                    X = characterContract.X,
                    Y = characterContract.Y,
                    Z = characterContract.Z,
                    Heading = characterContract.Heading,
                    Experience = characterContract.Exp,
                    ExpOnDeath = characterContract.ExpBeforeDeath,
                    Sp = characterContract.Sp,
                    Karma = characterContract.Karma,
                    PvpKills = characterContract.PvpKills,
                    PkKills = characterContract.PkKills,
                    BaseClass = CharTemplateTable.GetTemplate(characterContract.BaseClass),
                    ActiveClass = CharTemplateTable.GetTemplate(characterContract.ClassId),
                    RecomendationsLeft = characterContract.RecLeft,
                    RecomandationsHave = characterContract.RecHave,
                    CharacterSlot = characterContract.CharSlot,
                    Inventory = new PcInventory(_itemCrudService, _itemService, _idFactory, _itemTable, null),
                    DeleteTime = characterContract.DeleteTime,
                    LastAccess = characterContract.LastAccess,
                    CanCraft = characterContract.CanCraft,
                    AccessLevel = characterContract.AccessLevel,
                    Online = characterContract.Online,
                    OnlineTime = characterContract.OnlineTime,
                    PunishLevel = characterContract.PunishLevel,
                    PunishTimer = characterContract.PunishTimer,
                    PowerGrade = characterContract.PowerGrade,
                    Nobless = characterContract.Nobless,
                    Hero = characterContract.Hero,
                    LastRecomendationDate = characterContract.LastRecomDate
                };
            player.CharStatus.SetCurrentCp(characterContract.CurCp,false); //player.CharStatus.CurrentCp = playerContract.CurCp; //???after repairing the broadcast, return it back???
            player.CharStatus.SetCurrentHp(characterContract.CurHp, false); //player.CharStatus.CurrentHp = playerContract.CurHp;
            player.CharStatus.SetCurrentMp(characterContract.CurMp, false); //player.CharStatus.CurrentMp = playerContract.CurMp;

            return player;
        }

        public async Task<L2Player> GetPlayerBySlotId(int accountId, int slotId)
        {
            CharacterContract playerContract = await _characterRepository.GetCharacterBySlot(accountId, slotId);

            return await RestorePlayer(playerContract);
        }

        public async Task<IEnumerable<L2Player>> GetPlayersOnAccount(int accountId)
        {
            IEnumerable<CharacterContract> playersOnAccount = await _characterRepository.GetCharactersOnAccount(accountId);

            return await Task.WhenAll(playersOnAccount.Select(playerContract => RestorePlayer(playerContract)));
        }

        public bool DeleteCharById(int characterId)
        {
            _characterCrudService.Delete(new CharacterContract
                {
                    CharacterId = characterId
                });

            return true;
        }

        public async Task<L2Player> RestorePlayer(CharacterContract characterContract, GameClient client = null)
        {
            L2Player player = MapContractToPlayer(characterContract);

            player.Gameclient = client;
            //player.CStatsInit();
            await player.Inventory.Restore(player);
            player.SessionData = new PlayerBag();

            return player;
        }



        public int GetDaysRequiredToDeletePlayer()
        {
            return _config.GameplayConfig.Server.Client.DeleteCharAfterDays;
        }
    }
}