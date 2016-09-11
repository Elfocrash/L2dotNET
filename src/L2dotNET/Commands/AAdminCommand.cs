using L2dotNET.model.player;

namespace L2dotNET.Commands
{
    public abstract class AAdminCommand
    {
        protected internal abstract void Use(L2Player admin, string command);
    }
}