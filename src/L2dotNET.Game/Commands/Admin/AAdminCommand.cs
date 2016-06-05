using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Commands.Admin
{
    public abstract class AAdminCommand
    {
        protected internal abstract void Use(L2Player admin, string command);

        public string Cmd;
    }
}