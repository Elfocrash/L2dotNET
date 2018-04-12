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
                string command = Console.ReadLine();
                string[] words =  command.Split(' ');
                string key = words[0];
                if (commands.ContainsKey(key))
                {
                    commands[key].Execute(words);
                }
            }
        }

        public ConsoleCommandController()
        {
            commands.Add("objectsCount", new ObjectsCount());
            commands.Add("playersCount", new PlayersCount());
            commands.Add("known", new KnownObj());
            commands.Add("damage", new Damage());
        }
    }
}
