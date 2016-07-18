using System.Runtime.Remoting.Contexts;

namespace L2dotNET.Network
{
    [Synchronization]
    public abstract class PacketBase
    {
        public abstract void RunImpl();
    }
}