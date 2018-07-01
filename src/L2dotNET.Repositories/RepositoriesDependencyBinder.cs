using L2dotNET.Repositories.Abstract;
using L2dotNET.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Repositories
{
    public static class RepositoriesDependencyBinder
    {
        public static void Bind(IServiceCollection provider)
        {
            provider.AddSingleton(typeof(ICrudRepository<>), typeof(CrudRepositoryBase<>));

            provider.AddSingleton<IAccountRepository, AccountRepository>();

            provider.AddSingleton<ICharacterRepository, CharacterRepository>();
            provider.AddSingleton<IItemRepository, ItemRepository>();
        }
    }
}
