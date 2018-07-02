using L2dotNET.ConsoleCommand.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace L2dotNET.ConsoleCommand
{
    public class ConsoleCommandController
    {
        private readonly ICollection<Command> _commands;
        private Thread _commandThread;

        public ConsoleCommandController()
        {
            _commands = new List<Command>
            {
                new ObjectsCount(),
                new PlayersCount()
            };
        }

        public void Start()
        {
            if (_commandThread != null)
            {
                return;
            }

            _commandThread = new Thread(EnterCommand)
                {
                    Priority = ThreadPriority.Lowest,
                    IsBackground = true
                };

            _commandThread.Start();
        }

        public void Stop()
        {
            if (_commandThread == null)
            {
                return;
            }

            _commandThread.Abort();
            _commandThread = null;
        }

        private void EnterCommand()
        {
            Console.WriteLine("Command engine is running. Type help to get the info about available commands.");
            while (true)
            {
                string key = Console.ReadLine()?.ToLower();

                if (key == "help")
                {
                    Console.WriteLine("Available commands:");
                    _commands.Select(x => $"{x.Name} - {x.Description}")
                        .ToList()
                        .ForEach(Console.WriteLine);
                }
                else
                {
                    Command cmd = _commands.FirstOrDefault(x => x.Name.ToLower() == key);

                    if (cmd != null)
                    {
                        cmd.Execute();
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized command");
                    }
                }
            }
        }
    }
}
