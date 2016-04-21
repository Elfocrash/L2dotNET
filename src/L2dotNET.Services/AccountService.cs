using L2dotNET.Models;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Services
{
    public class AccountService : IAccountService
    {
        IUnitOfWork unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public AccountModel GetAccountByLogin(string login)
        {
            return this.unitOfWork.AccountRepository.GetAccountByLogin(login);
        }

        public AccountModel CreateAccount(string login, string password)
        {
            return this.unitOfWork.AccountRepository.CreateAccount(login, password);
        }

        public bool CheckIfAccountIsCorrect(string login, string password)
        {
            return this.unitOfWork.AccountRepository.CheckIfAccountIsCorrect(login, password);
        }
    }
}
