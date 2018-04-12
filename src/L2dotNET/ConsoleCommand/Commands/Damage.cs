using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.ConsoleCommand.Commands
{
    class Damage : Command
    {
        public override void Execute(string[] param)
        {
            L2Character l2Character = (L2Character)world.L2World.Instance.GetObject(int.Parse(param[1]));
            l2Character.CharStatus.ReduceHp(Double.Parse(param[2]), world.L2World.Instance.GetPlayers()[0]);
        }
    }
}
