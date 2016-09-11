using L2dotNET.Attributes;
using L2dotNET.model.player;

namespace L2dotNET.Commands.Admin
{
    [AdminCommand(CommandName = "chat")]
    class AdminChat : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            admin.ShowHtmAdmin("main.htm", false);
        }
    }
}