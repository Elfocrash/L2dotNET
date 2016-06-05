using System.Collections.Generic;
using L2dotNET.Models;

namespace L2dotNET.Services.Contracts
{
    public interface IAccountService
    {
        AccountModel GetAccountByLogin(string login);

        AccountModel CreateAccount(string login, string password);

        bool CheckIfAccountIsCorrect(string login, string password);

        List<int> GetPlayerIdsListByAccountName(string login);
    }
}