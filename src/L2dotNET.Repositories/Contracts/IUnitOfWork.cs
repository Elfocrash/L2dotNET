using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
