using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Repositories.Contracts
{
    public interface IAccountRepository
    {
        AccountModel GetAccountByLogin(string login);

        AccountModel CreateAccount(string login, string password);

        bool CheckIfAccountIsCorrect(string login, string password);

        List<int> GetPlayerIdsListByAccountName(string login);
    }
}
