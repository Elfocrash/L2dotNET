using L2dotNET.Repositories;
using L2dotNET.Repositories.Contracts;
using L2dotNET.Services;
using L2dotNET.Services.Contracts;
using Ninject.Modules;

namespace L2dotNET
{
    public class DepInjectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPlayerService>().To<PlayerService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IServerService>().To<ServerService>();
            Bind<ICheckService>().To<CheckService>();
            Bind<IItemService>().To<ItemService>();
            Bind<ISkillService>().To<SkillService>();
            
            Bind<IPlayerRepository>().To<PlayerRepository>();
            Bind<IAccountRepository>().To<AccountRepository>();
            Bind<IServerRepository>().To<ServerRepository>();
            Bind<ICheckRepository>().To<CheckRepository>();
            Bind<IItemRepository>().To<ItemRepository>();
            Bind<ISkillRepository>().To<SkillRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();

        }
    }
}