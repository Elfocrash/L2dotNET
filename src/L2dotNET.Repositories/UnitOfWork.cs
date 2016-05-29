using L2dotNET.Repositories.Contracts;

namespace L2dotNET.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork()
        {
        }

        public UnitOfWork(IPlayerRepository playerRepository,
                          IAccountRepository accountRepository,
                          IServerRepository serverRepository,
                          ICheckRepository checkRepository)
        {
            this.playerRepository = playerRepository;
            this.accountRepository = accountRepository;
            this.serverRepository = serverRepository;
            this.checkRepository = checkRepository;
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

        private IServerRepository serverRepository;
        public IServerRepository ServerRepository
        {
            get
            {
                if (this.serverRepository == null)
                    this.serverRepository = new ServerRepository();

                return this.serverRepository;
            }
        }

        private ICheckRepository checkRepository;
        public ICheckRepository CheckRepository
        {
            get
            {
                if (this.checkRepository == null)
                    this.checkRepository = new CheckRepository();

                return this.checkRepository;
            }
        }

        #endregion
    }
}
