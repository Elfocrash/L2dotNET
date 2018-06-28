﻿using System;
using System.Collections.Generic;
using System.Text;
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

            provider.AddSingleton<IPlayerRepository, PlayerRepository>();
            provider.AddSingleton<IServerRepository, ServerRepository>();
            provider.AddSingleton<ICheckRepository, CheckRepository>();
            provider.AddSingleton<IItemRepository, ItemRepository>();
            provider.AddSingleton<ISkillRepository, SkillRepository>();
        }
    }
}
