﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using L2dotNET.Logging.Abstraction;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network;
using L2dotNET.Network;
using L2dotNET.Network.loginauth;
using L2dotNET.Repositories;
using L2dotNET.Services;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.LoginService
{
    class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            ClassLoggerConfigurator.ConfigureClassLogger($"{Assembly.GetExecutingAssembly().Location}.log");
            Log.Info("Starting LoginService...");
            SetConsoleConfigurations();
            SetNumberDecimalSeparator();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<LoginServer>().Start();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            ServicesDependencyBinder.Bind(serviceCollection);
            RepositoriesDependencyBinder.Bind(serviceCollection);

            serviceCollection.AddSingleton<GamePacketHandlerAuth>();
            serviceCollection.AddSingleton<AuthThread>();
            serviceCollection.AddSingleton<PacketHandler>();
            serviceCollection.AddSingleton<ServerThread>();

            serviceCollection.AddSingleton<ServerThreadPool>();
            serviceCollection.AddSingleton<GamePacketHandler>();
            serviceCollection.AddSingleton<ClientManager>();
            serviceCollection.AddSingleton<Managers.ClientManager>();
            
            serviceCollection.AddSingleton<PreReqValidation>();
            serviceCollection.AddSingleton<Config.Config>();
            serviceCollection.AddSingleton<LoginServer>();

        }

        private static void SetConsoleConfigurations()
        {
            Console.Title = @"L2dotNET LoginServer";
        }

        //TODO: Temporary fix. Need a better workaround to fix the Culture conversion issues. (Note: parsing error when reading "." in Latin cultures from XML files)
        private static void SetNumberDecimalSeparator()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
        }
    }
}