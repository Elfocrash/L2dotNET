using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IAccountRepository
    {
        Task<AccountContract> GetAccountByLogin(string login);

        Task<AccountContract> CreateAccount(string login, string password);

        Task<bool> CheckIfAccountIsCorrect(string login, string password);

        Task<List<int>> GetPlayerIdsListByAccountName(string login);
    }
}