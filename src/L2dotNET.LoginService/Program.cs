using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using log4net;
using Ninject;

namespace L2dotNET.LoginService
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            Log.Info("Starting LoginService...");
            SetConsoleConfigurations();
            SetNumberDecimalSeparator();
            LoginServer.Kernel = new StandardKernel(new DepInjectionModule());
            LoginServer server = LoginServer.Kernel.Get<LoginServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static void SetConsoleConfigurations()
        {
            Console.Title = "L2dotNET LoginServer";
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