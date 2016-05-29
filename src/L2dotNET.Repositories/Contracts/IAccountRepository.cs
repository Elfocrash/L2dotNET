using L2dotNET.Models;
using System.Collections.Generic;

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
