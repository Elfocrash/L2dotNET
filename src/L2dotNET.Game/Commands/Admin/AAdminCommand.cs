
namespace L2dotNET.GameService.Commands
{
    public abstract class AAdminCommand
    {
        protected internal abstract void Use(L2Player admin, string command);
        public string Cmd;
    }
}
