using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.managers;
using Ninject;

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
