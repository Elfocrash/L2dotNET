using System;
using L2dotNET.World;

namespace L2dotNET.ConsoleCommand.Commands
{
    class ObjectsCount : Command
    {
        public ObjectsCount()
        {
            Name = "objects-count";
            Description = "Gets total number of objects on the server";
        }

        public override void Execute(string param)
        {
            Console.WriteLine("Count - " + L2World.GetObjects().Count);
        }
    }
}
