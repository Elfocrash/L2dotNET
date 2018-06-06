using System.Collections.Generic;
using L2dotNET.DataContracts;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountContract GetAccountByLogin(string login)
        {
            return _accountRepository.GetAccountByLogin(login);
        }

        public AccountContract CreateAccount(string login, string password)
        {
            return _accountRepository.CreateAccount(login, password);
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            return _accountRepository.CheckIfAccountIsCorrect(login, password);
        }

        public List<int> GetPlayerIdsListByAccountName(string login)
        {
            return _accountRepository.GetPlayerIdsListByAccountName(login);
        }
    }
}