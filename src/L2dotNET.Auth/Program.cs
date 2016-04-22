using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginServer.Kernel = new StandardKernel(new DepInjectionModule());
            LoginServer server = LoginServer.Kernel.Get<LoginServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
