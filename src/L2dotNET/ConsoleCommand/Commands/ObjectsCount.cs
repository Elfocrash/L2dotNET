using System;

namespace L2dotNET.ConsoleCommand.Commands
{
    class ObjectsCount : Command
    {
        public override void Execute(string param)
        {
            Console.WriteLine("Count - " + World.L2World.Instance.GetObjects().Count);
        }
    }
}
