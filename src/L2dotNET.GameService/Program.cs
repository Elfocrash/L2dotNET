using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using log4net;
using Ninject;

namespace L2dotNET.GameService
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            try { 
            Log.Info("Starting GameService...");
            SetConsoleConfigurations();
            SetNumberDecimalSeparator();
            GameServer.Kernel = new StandardKernel(new DepInjectionModule());
            GameServer server = GameServer.Kernel.Get<GameServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION : " + ex.Message);
            }
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