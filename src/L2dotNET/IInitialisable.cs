using System.Threading.Tasks;

namespace L2dotNET
{
    public interface IInitialisable
    {
        bool Initialised { get; }

        Task Initialise();
    }
}