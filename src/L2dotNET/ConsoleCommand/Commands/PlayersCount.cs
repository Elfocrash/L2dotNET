using System;

namespace L2dotNET.ConsoleCommand.Commands
{
    class PlayersCount : Command
    {
        public PlayersCount()
        {
            Name = "players-count";
            Description = "Gets total players on the server";
        }

        public override void Execute(string param)
        {
            Console.WriteLine("Count - " + World.L2World.Instance.GetPlayers().Count);
        }
    }
}
