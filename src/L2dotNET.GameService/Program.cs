﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using L2dotNET.ConsoleCommand;
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
        
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        static ConsoleCommandController consoleCommandController;

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if(consoleCommandController != null)
            consoleCommandController.isWorkConsoleEnter = false;
            return true;
        }

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            ClassLoggerConfigurator.ConfigureClassLogger($"{Assembly.GetExecutingAssembly().Location}.log");
            consoleCommandController = new ConsoleCommandController();
            consoleCommandController.Launch();
            try { 
                Log.Info("Starting GameService...");
                SetConsoleConfigurations();
                SetNumberDecimalSeparator();
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();
                serviceProvider.GetService<GameServer>().Start();
                Process.GetCurrentProcess().WaitForExit();
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION : " + ex.Message + " " + ex.Data + " " + ex.Source + ex.StackTrace);
            }
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