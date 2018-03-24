using L2dotNET.Attributes;
using L2dotNET.Models.player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "chat")]
    class AdminChat : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            admin.ShowHtmAdmin("main.htm", false);
        }
    }
}