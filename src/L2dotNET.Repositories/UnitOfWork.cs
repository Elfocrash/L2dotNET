using L2dotNET.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork()
        {
        }

        public UnitOfWork(
            IPlayerRepository playerRepository,
            IAccountRepository accountRepository
            )
        {
            this.playerRepository = playerRepository;
            this.accountRepository = accountRepository;

        }

        public void Commit()
        {

        }

        public void Dispose()
        {

        }

        #region REPOSITORIES

        private IPlayerRepository playerRepository;
        public IPlayerRepository PlayerRepository
        {
            get
            {
                if (this.playerRepository == null)
                    this.playerRepository = new PlayerRepository();

                return this.playerRepository;
            }
        }

        private IAccountRepository accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (this.accountRepository == null)
                    this.accountRepository = new AccountRepository();

                return this.accountRepository;
            }
        }

        #endregion
    }
}
