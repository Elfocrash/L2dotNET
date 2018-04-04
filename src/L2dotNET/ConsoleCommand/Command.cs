using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.ConsoleCommand
{
    abstract class Command
    {
        public abstract void Execute(string param = "");
    }
}
