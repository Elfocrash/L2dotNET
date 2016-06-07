using L2dotNET.Repositories.Contracts;

namespace L2dotNET.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork() { }

        public UnitOfWork(IPlayerRepository playerRepository, IAccountRepository accountRepository, IServerRepository serverRepository, ICheckRepository checkRepository)
        {
            this.playerRepository = playerRepository;
            this.accountRepository = accountRepository;
            this.serverRepository = serverRepository;
            this.checkRepository = checkRepository;
        }

        public void Commit() { }

        public void Dispose() { }

        #region REPOSITORIES

        private IPlayerRepository playerRepository;

        public IPlayerRepository PlayerRepository
        {
            get { return this.playerRepository ?? (this.playerRepository = new PlayerRepository()); }
        }

        private IAccountRepository accountRepository;

        public IAccountRepository AccountRepository
        {
            get { return this.accountRepository ?? (this.accountRepository = new AccountRepository()); }
        }

        private IServerRepository serverRepository;

        public IServerRepository ServerRepository
        {
            get { return this.serverRepository ?? (this.serverRepository = new ServerRepository()); }
        }

        private ICheckRepository checkRepository;

        public ICheckRepository CheckRepository
        {
            get { return this.checkRepository ?? (this.checkRepository = new CheckRepository()); }
        }

        #endregion
    }
}