using System;
using System.Diagnostics;
using Ninject;

namespace L2dotNET.LoginService
{
    class Program
    {
        private static void Main(string[] args)
        {
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
            Console.WindowWidth = (Console.LargestWindowWidth * 7 / 10);
        }

        //TODO: Temporary fix. Need a better workaround to fix the Culture conversion issues. (Note: parsing error when reading "." in Latin cultures from XML files)
        private static void SetNumberDecimalSeparator()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
    }
}