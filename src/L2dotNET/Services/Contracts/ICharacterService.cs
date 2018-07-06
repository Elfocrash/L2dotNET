using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Models.Player;

namespace L2dotNET.Services.Contracts
{
    public interface ICharacterService
    {
        Task<L2Player> GetById(int characterId);

        Task<bool> CheckIfPlayerNameExists(string name);

        void CreatePlayer(L2Player player);

        void UpdatePlayer(L2Player player);

        Task<L2Player> GetPlayerBySlotId(int accountId, int slotId);

        bool DeleteCharById(int characterId);

        Task<L2Player> RestorePlayer(CharacterContract characterContract, GameClient client);

        Task<IEnumerable<L2Player>> GetPlayersOnAccount(int accountId);

        int GetDaysRequiredToDeletePlayer();
    }
}