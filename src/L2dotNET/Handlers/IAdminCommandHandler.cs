using L2dotNET.Models.player;

namespace L2dotNET.Handlers
{
    public interface IAdminCommandHandler
    {
        void Request(L2Player admin, string alias);

        void Register(object processor);
    }
}