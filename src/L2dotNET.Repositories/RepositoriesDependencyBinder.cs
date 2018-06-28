using System;
using System.Collections.Generic;
using System.Text;
using L2dotNET.Repositories.Abstract;
using L2dotNET.Repositories.Abstract.Contracts;
using L2dotNET.Repositories.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Repositories
{
    public static class RepositoriesDependencyBinder
    {
        public static void Bind(IServiceCollection provider)
        {
            provider.AddSingleton(typeof(ICrudRepository<>), typeof(CrudRepositoryBase<>));

            provider.AddSingleton<IAccountRepository, AccountRepository>();
            provider.AddSingleton<IEtcItemRepository, EtcItemRepository>();
            provider.AddSingleton<IArmorRepository, ArmorRepository>();
        }
    }
}
