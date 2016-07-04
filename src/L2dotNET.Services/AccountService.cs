using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;

namespace L2dotNET.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public AccountModel GetAccountByLogin(string login)
        {
            return _unitOfWork.AccountRepository.GetAccountByLogin(login);
        }

        public AccountModel CreateAccount(string login, string password)
        {
            return _unitOfWork.AccountRepository.CreateAccount(login, password);
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            return _unitOfWork.AccountRepository.CheckIfAccountIsCorrect(login, password);
        }

        public List<int> GetPlayerIdsListByAccountName(string login)
        {
            return _unitOfWork.AccountRepository.GetPlayerIdsListByAccountName(login);
        }
    }
}