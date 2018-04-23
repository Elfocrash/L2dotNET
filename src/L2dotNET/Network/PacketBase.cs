using System;

namespace L2dotNET.Network
{
    public abstract class PacketBase
    {
        protected readonly IServiceProvider ServiceProvider;

        protected PacketBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract void RunImpl();
    }
}