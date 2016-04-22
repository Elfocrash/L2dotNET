using System.Diagnostics;
using L2dotNET.Game._test;
using L2dotNET.Game.tools;
using System;
using L2dotNET.Game.geo;
using Ninject;

namespace L2dotNET.Game
{
    class Program
    {
        
        static void Main(string[] args)
        {
            GameServer.Kernel = new StandardKernel(new DepInjectionModule());
            GameServer server = GameServer.Kernel.Get<GameServer>();
            server.Start();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
