using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Repositories;
using L2dotNET.Repositories.Abstract;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;

namespace L2dotNET.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICrudRepository<AccountContract> _accountCrudRepository;

        public AccountService(IAccountRepository accountRepository, ICrudRepository<AccountContract> accountCrudRepository)
        {
            _accountRepository = accountRepository;
            _accountCrudRepository = accountCrudRepository;
        }

        public async Task<AccountContract> GetAccountByLogin(string login)
        {
            return await _accountRepository.GetAccountByLogin(login);
        }

        public async Task<AccountContract> CreateAccount(string login, string password)
        {
            AccountContract acc = new AccountContract
                {
                    Login = login,
                    Password = L2Security.HashPassword(password)
                };

            acc.AccountId = (int)await _accountCrudRepository.Add(acc);

            return acc;
        }

        public async Task<bool> CheckIfAccountIsCorrect(string login, string password)
        {
            AccountContract account = await GetAccountByLogin(login);
            return account != null && account.Password.SequenceEqual(L2Security.HashPassword(password));
        }

        public void UpdateAccount(AccountContract account)
        {
            _accountCrudRepository.Update(account);
        }

        public void DeleteAccount(AccountContract account)
        {
            _accountCrudRepository.Delete(account);
        }
    }
}