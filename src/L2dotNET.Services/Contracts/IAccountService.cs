using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services.Contracts
{
    public interface IAccountService
    {
        AccountModel GetAccountByLogin(string login);

        AccountModel CreateAccount(string login, string password);

        bool CheckIfAccountIsCorrect(string login, string password);
    }
}
