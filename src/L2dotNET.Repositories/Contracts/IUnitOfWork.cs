using System;

namespace L2dotNET.Repositories.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository PlayerRepository { get; }
        IAccountRepository AccountRepository { get; }
        IServerRepository ServerRepository { get; }
        ICheckRepository CheckRepository { get; }

        void Commit();
    }
}