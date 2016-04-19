using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.model.skills2.effects;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game._test
{
    class test2
    {
        public static void ss()
        {
            DateTime dtn = DateTime.Now;

            SystemMessage sm;
            for (int a = 0; a < 1000000; a++)
            {
                sm = new SystemMessage(33);
            }

            TimeSpan ts = DateTime.Now - dtn;
            Console.WriteLine("ms "+ts.Milliseconds+", "+ts.TotalMilliseconds);
        }
    }
}
