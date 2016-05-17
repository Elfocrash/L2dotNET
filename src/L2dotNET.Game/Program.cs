using Ninject;
using System;
using System.Diagnostics;

namespace L2dotNET.GameService
{
    class Program
    {
        
        static void Main(string[] args)
        {
            SetConsoleConfigurations();
            SetNumberDecimalSeparator();
            GameServer.Kernel = new StandardKernel(new DepInjectionModule());
            GameServer server = GameServer.Kernel.Get<GameServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static void SetConsoleConfigurations()
        {
            Console.Title = "L2dotNET GameServer";
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
