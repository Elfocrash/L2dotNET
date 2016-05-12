using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.LoginService
{
    class Program
    {
        static void Main(string[] args)
        {
            SetNumberDecimalSeparator();
            LoginServer.Kernel = new StandardKernel(new DepInjectionModule());
            LoginServer server = LoginServer.Kernel.Get<LoginServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
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
