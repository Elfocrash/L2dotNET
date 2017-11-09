using System.Collections.Generic;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IAccountRepository
    {
        AccountContract GetAccountByLogin(string login);

        AccountContract CreateAccount(string login, string password);

        bool CheckIfAccountIsCorrect(string login, string password);

        List<int> GetPlayerIdsListByAccountName(string login);
    }
}