using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Services.Contracts
{
    public interface IAccountService
    {
        Task<AccountContract> GetAccountByLogin(string login);

        Task<AccountContract> CreateAccount(string login, string password);

        Task<bool> CheckIfAccountIsCorrect(string login, string password);

        void UpdateAccount(AccountContract account);

        void DeleteAccount(AccountContract account);
    }
}