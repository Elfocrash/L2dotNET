using L2dotNET.Repositories;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services;
using L2dotNET.Services.Contracts;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth
{
    public class DepInjectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IAccountService>().To<AccountService>();

            Bind<IPlayerRepository>().To<PlayerRepository>();
            Bind<IAccountRepository>().To<AccountRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
