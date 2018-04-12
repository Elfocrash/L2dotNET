using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace L2dotNET.ConsoleCommand.Commands
{
    class KnownObj : Command
    {
        public override void Execute(string[] param)
        {
            string str = "";
            if (world.L2World.Instance.GetPlayers()[0].KnownObjects.Count > 0)
            {
                foreach (var obj in world.L2World.Instance.GetPlayers()[0].KnownObjects)
                {
                    str += (obj.Key + " ");
                }
            }
            Console.WriteLine("Known count - " + world.L2World.Instance.GetPlayers()[0].KnownObjects.Count + " | Known - " +str);
        }
    }
}