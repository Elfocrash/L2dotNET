using L2dotNET.Repositories.Contracts;

namespace L2dotNET.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork() { }

        public UnitOfWork(IPlayerRepository playerRepository, IAccountRepository accountRepository, IServerRepository serverRepository, ICheckRepository checkRepository)
        {
            this._playerRepository = playerRepository;
            this._accountRepository = accountRepository;
            this._serverRepository = serverRepository;
            this._checkRepository = checkRepository;
        }

        public void Commit() { }

        public void Dispose() { }

        #region REPOSITORIES

        private IPlayerRepository _playerRepository;

        public IPlayerRepository PlayerRepository => _playerRepository ?? (_playerRepository = new PlayerRepository());

        private IAccountRepository _accountRepository;

        public IAccountRepository AccountRepository => _accountRepository ?? (_accountRepository = new AccountRepository());

        private IServerRepository _serverRepository;

        public IServerRepository ServerRepository => _serverRepository ?? (_serverRepository = new ServerRepository());

        private ICheckRepository _checkRepository;

        public ICheckRepository CheckRepository => _checkRepository ?? (_checkRepository = new CheckRepository());

        #endregion
    }
}