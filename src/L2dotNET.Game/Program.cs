using System.Diagnostics;
using L2dotNET.Game._test;
using L2dotNET.Game.tools;
using System;
using L2dotNET.Game.geo;

namespace L2dotNET.Game
{
    class Program
    {
        static void Main(string[] args)
        {
           // test2.ss();
           // Cfg.init("all");
           // drop_l2j_to_rcs.ss();

            //short spdx = 300;
            //for (long s1 = 1; s1 < int.MaxValue; s1++)
            //{
            //    double x = 6000 * spdx;
            //}
            //double dx = (14107 - 12107), dy = 0, dz = 0;
            //double distance = Math.Sqrt(dx * dx + dy * dy);

            //int ticks = 1 + (int)(10 * distance / 300); ;

            //Console.WriteLine("ticks " + ticks);


            //int spd = 200;
            //double formula = (14400 * spd) / (11148.38709677421 * (spd / 4));
            //Console.WriteLine("result " + spd + " >> " + (formula == 5.16666666666666 ? "yes" : "no") + " " + formula);
            GameServer.getInstance();
          //  new GeoData().loadGeo();
          //  Console.WriteLine("end.");
          //  Console.ReadLine();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
