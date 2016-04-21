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
            IPlayerRepository playerRepository
            )
        {
            this.playerRepository = playerRepository;

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

        #endregion
    }
}
