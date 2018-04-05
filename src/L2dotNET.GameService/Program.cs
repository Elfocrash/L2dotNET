using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using Ninject;
using L2dotNET.ConsoleCommand;

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

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {

            consoleCommandController = new ConsoleCommandController();
            consoleCommandController.Launch();
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