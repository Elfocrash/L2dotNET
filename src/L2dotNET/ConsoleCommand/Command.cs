namespace L2dotNET.ConsoleCommand
{
    abstract class Command
    {
        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public abstract void Execute(string param = "");
    }
}
