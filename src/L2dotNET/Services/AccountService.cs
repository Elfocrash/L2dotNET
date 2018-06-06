using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<AccountContract> GetAccountByLogin(string login)
        {
            return await _accountRepository.GetAccountByLogin(login);
        }

        public async Task<AccountContract> CreateAccount(string login, string password)
        {
            return await _accountRepository.CreateAccount(login, password);
        }

        public async Task<bool> CheckIfAccountIsCorrect(string login, string password)
        {
            return await _accountRepository.CheckIfAccountIsCorrect(login, password);
        }

        public async Task<List<int>> GetPlayerIdsListByAccountName(string login)
        {
            return await _accountRepository.GetPlayerIdsListByAccountName(login);
        }
    }
}