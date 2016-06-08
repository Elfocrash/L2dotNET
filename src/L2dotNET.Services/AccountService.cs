using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public AccountModel GetAccountByLogin(string login)
        {
            return unitOfWork.AccountRepository.GetAccountByLogin(login);
        }

        public AccountModel CreateAccount(string login, string password)
        {
            return unitOfWork.AccountRepository.CreateAccount(login, password);
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            return unitOfWork.AccountRepository.CheckIfAccountIsCorrect(login, password);
        }

        public List<int> GetPlayerIdsListByAccountName(string login)
        {
            return unitOfWork.AccountRepository.GetPlayerIdsListByAccountName(login);
        }
    }
}