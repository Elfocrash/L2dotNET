using System;
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
using L2dotNET.Utility;

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

            return characterContract.ToPlayer();
        }

        public async Task<bool> CheckIfPlayerNameExists(string name)
        {
            return await _characterRepository.CheckIfPlayerNameExists(name);
        }

        public void CreatePlayer(L2Player player)
        {
            _characterCrudService.Add(player.ToContract());
        }

        public async Task UpdatePlayer(L2Player player)
        {
            player.LastAccess = DateTime.UtcNow;
            await _characterCrudService.Update(player.ToContract());
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

        public async Task<L2Player> RestorePlayer(CharacterContract characterContract)
        {
            L2Player player = characterContract.ToPlayer();

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