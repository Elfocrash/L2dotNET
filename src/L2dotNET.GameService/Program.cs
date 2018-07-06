using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using L2dotNET.ConsoleCommand;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Handlers;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Managers;
using L2dotNET.Managers.bbs;
using L2dotNET.Network;
using L2dotNET.Network.loginauth;
using L2dotNET.Repositories;
using L2dotNET.Services;
using L2dotNET.Tables;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.GameService
{
    class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static ConsoleCommandController consoleCommandController;

        private static void Main()
        {
            ClassLoggerConfigurator.ConfigureClassLogger($"{Assembly.GetExecutingAssembly().Location}.log");

            TaskScheduler.UnobservedTaskException += (sender, e) =>
                {
                    Log.ErrorTrace(e.Exception, "UnobservedTaskException");
                };

            consoleCommandController = new ConsoleCommandController();
            consoleCommandController.Start();

            try
            { 
                Log.Info("Starting GameService...");

                SetConsoleConfigurations();
                SetNumberDecimalSeparator();

                ServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                Task.Factory.StartNew(serviceProvider.GetService<GameServer>().Start);
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION : " + ex.Message + " " + ex.Data + " " + ex.Source + ex.StackTrace);
            }

            Process.GetCurrentProcess().WaitForExit();
            Log.Info("Press ENTER to exit...");
            Console.Read();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<Config.Config>();

            ServicesDependencyBinder.Bind(serviceCollection);
            RepositoriesDependencyBinder.Bind(serviceCollection);

            serviceCollection.AddSingleton<GamePacketHandlerAuth>();
            serviceCollection.AddSingleton<AuthThread>();
            serviceCollection.AddSingleton<GamePacketHandler>();
            serviceCollection.AddSingleton<ClientManager>();
            serviceCollection.AddSingleton<PreReqValidation>();
            serviceCollection.AddSingleton<AnnouncementManager>();
            serviceCollection.AddSingleton<SpawnTable>();
            serviceCollection.AddSingleton<IdFactory>();
            serviceCollection.AddSingleton<ItemTable>();
            serviceCollection.AddSingleton<HtmCache>();
            serviceCollection.AddSingleton<BbsManager>();
            serviceCollection.AddSingleton<IAdminCommandHandler, AdminCommandHandler>();
            serviceCollection.AddSingleton<GameServer>();
        }

        private static void SetConsoleConfigurations()
        {
            Console.Title = @"L2dotNET GameServer";
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