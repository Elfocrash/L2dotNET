using System;

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
            Console.WriteLine("Count - " + World.L2World.Instance.GetObjects().Count);
        }
    }
}
