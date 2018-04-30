namespace L2dotNET
{
    public interface IInitialisable
    {
        bool Initialised { get; }

        void Initialise();
    }
}