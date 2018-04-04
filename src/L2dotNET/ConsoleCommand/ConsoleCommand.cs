using L2dotNET.ConsoleCommand.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L2dotNET.ConsoleCommand
{
    public class ConsoleCommandController
    {
        public bool isWorkConsoleEnter = true;
        public void Launch()
        {
            Thread thread = new Thread(EnterCommand);
            thread.Start();
        }

        Dictionary<string, Command> commands = new Dictionary<string, Command>();

        void EnterCommand()
        {
            while (isWorkConsoleEnter)
            {
                string key = Console.ReadLine();
                if (commands.ContainsKey(key))
                {
                    commands[key].Execute();
                }
            }
        }

        public ConsoleCommandController()
        {
            commands.Add("ObjectsCount", new ObjectsCount());
            commands.Add("PlayersCount", new PlayersCount());
        }
    }
}
