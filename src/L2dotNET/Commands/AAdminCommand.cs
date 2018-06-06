using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands
{
    public abstract class AAdminCommand
    {
        protected readonly IServiceProvider ServiceProvider;

        protected AAdminCommand(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected internal abstract Task UseAsync(L2Player admin, string command);
    }
}