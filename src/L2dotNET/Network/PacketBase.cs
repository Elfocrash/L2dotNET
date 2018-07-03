using System;
using System.Threading.Tasks;
using NLog;

namespace L2dotNET.Network
{
    public abstract class PacketBase
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected readonly IServiceProvider ServiceProvider;

        protected PacketBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract Task RunImpl();
    }
}