using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface ICharacterRepository
    {
        Task<bool> CheckIfPlayerNameExists(string name);

        Task<CharacterContract> GetCharacterBySlot(int accountId, int slotId);

        Task<IEnumerable<CharacterContract>> GetCharactersOnAccount(int accountId);
    }
}