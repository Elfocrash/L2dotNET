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

        public async Task<L2Player> GetPlayerByLogin(int characterId)
        {
            var playerContract = await _characterCrudService.GetById(characterId);
            //TODO Use automapper to map this
            var player = new L2Player(this, characterId, CharTemplateTable.Instance.GetTemplate(playerContract.ClassId))
            {
                ObjId = characterId,
                Name = playerContract.Name,
                Title = playerContract.Title,
                Level = (byte)playerContract.Level,
                //MaxHp = playerContract.MaxHp,
                //MaxCp = playerContract.MaxCp,
                //MaxMp = playerContract.MaxMp,
                Face = (Face)playerContract.Face,
                HairStyleId = (HairStyleId)playerContract.HairStyle,
                HairColor = (HairColor)playerContract.HairColor,
                Sex = (Gender)playerContract.Sex,
                X = playerContract.X,
                Y = playerContract.Y,
                Z = playerContract.Z,
                Heading = playerContract.Heading,
                Exp = playerContract.Exp,
                ExpOnDeath = playerContract.ExpBeforeDeath,
                Sp = playerContract.Sp,
                Karma = playerContract.Karma,
                PvpKills = playerContract.PvpKills,
                PkKills = playerContract.PkKills,
                BaseClass = CharTemplateTable.Instance.GetTemplate(playerContract.BaseClass),
                ActiveClass = CharTemplateTable.Instance.GetTemplate(playerContract.ClassId),
                //ClassId = playerContract.ClassId,
                RecLeft = playerContract.RecLeft,
                RecHave = playerContract.RecHave,
                CharSlot = playerContract.CharSlot,
                Inventory = new PcInventory(_itemCrudService, _itemService, _idFactory, _itemTable, null),
                DeleteTime = playerContract.DeleteTime,
                LastAccess = playerContract.LastAccess,
                CanCraft = playerContract.CanCraft,
                AccessLevel = playerContract.AccessLevel,
                Online = playerContract.Online,
                OnlineTime = playerContract.OnlineTime,
                PunishLevel = playerContract.PunishLevel,
                PunishTimer = playerContract.PunishTimer,
                PowerGrade = playerContract.PowerGrade,
                Nobless = playerContract.Nobless,
                Hero = playerContract.Hero,
                LastRecomDate = playerContract.LastRecomDate
            };
            player.CharStatus.SetCurrentCp(playerContract.CurCp,false); //player.CharStatus.CurrentCp = playerContract.CurCp; //???after repairing the broadcast, return it back???
            player.CharStatus.SetCurrentHp(playerContract.CurHp, false); //player.CharStatus.CurrentHp = playerContract.CurHp;
            player.CharStatus.SetCurrentMp(playerContract.CurMp, false); //player.CharStatus.CurrentMp = playerContract.CurMp;

            return player;
        }

        public async Task<bool> CheckIfPlayerNameExists(string name)
        {
            return await _characterRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(L2Player player)
        {
            // TODO Use automapper to map this
            var playerContract = new CharacterContract
            {
                AccountName = player.AccountName,
                CharacterId = player.ObjId,
                Name = player.Name,
                Level = player.Level,
                MaxHp = player.MaxHp,
                CurHp = (int)player.CharStatus.CurrentHp,
                MaxCp = player.MaxCp,
                CurCp = (int)player.CurCp,
                MaxMp = player.MaxMp,
                CurMp = (int)player.CharStatus.CurrentMp,
                Face = (byte)player.Face,
                HairStyle = (byte)player.HairStyleId,
                HairColor = (byte)player.HairColor,
                Sex = (byte)player.Sex,
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
                PunishLevel = player.PunishLevel,
                PunishTimer = player.PunishTimer,
                PowerGrade = player.PowerGrade,
                Nobless = player.Nobless,
                Hero = player.Hero,
                LastRecomDate = player.LastRecomDate
            };
            _characterCrudService.Add(playerContract);
        }

        public void UpdatePlayer(L2Player player)
        {
            //TODO Use automapper to map this
            CharacterContract characterContract = new CharacterContract
            {
                CharacterId = player.ObjId,
                Level = player.Level,
                MaxHp = player.MaxHp,
                CurHp = (int)player.CharStatus.CurrentHp,
                MaxCp = player.MaxCp,
                CurCp = (int)player.CurCp,
                MaxMp = player.MaxMp,
                CurMp = (int)player.CharStatus.CurrentMp,
                Face = (byte)player.Face,
                HairStyle = (byte)player.HairStyleId,
                HairColor = (byte)player.HairColor,
                Sex = (byte)player.Sex,
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
                PunishLevel = player.PunishLevel,
                PunishTimer = player.PunishTimer,
                PowerGrade = player.PowerGrade,
                Nobless = player.Nobless,
                LastAccess = player.LastAccess
            };
            _characterCrudService.Update(characterContract);
        }

        public async Task<L2Player> GetPlayerBySlotId(string accountName, int slotId)
        {
            var playerContract = await _characterRepository.GetPlayerModelBySlotId(accountName, slotId);
            var player = await RestorePlayer(playerContract.CharacterId, null);
            return player;
        }

        public bool DeleteCharByObjId(int characterId)
        {
            _characterCrudService.Delete(new CharacterContract
                {
                    CharacterId = characterId
                });

            return true;
        }

        public async Task<L2Player> RestorePlayer(int id, GameClient client)
        {
            var player = await GetPlayerByLogin(id);

            player.Gameclient = client;
            //player.CStatsInit();
            player.Inventory.Restore(player);
            player.SessionData = new PlayerBag();

            return player;
        }

        public int GetDaysRequiredToDeletePlayer()
        {
            return _config.GameplayConfig.Server.Client.DeleteCharAfterDays;
        }
    }
}