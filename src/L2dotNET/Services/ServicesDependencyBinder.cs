using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Services
{
    public static class ServicesDependencyBinder
    {
        public static void Bind(IServiceCollection provider)
        {
            provider.AddSingleton(typeof(ICrudService<>), typeof(CrudServiceBase<>));

            provider.AddSingleton<ICharacterService, CharacterService>();
            provider.AddSingleton<IAccountService, AccountService>();
            provider.AddSingleton<IServerService, ServerService>();
            provider.AddSingleton<ICheckService, CheckService>();
            provider.AddSingleton<IItemService, ItemService>();
        }
    }
}
