using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminChat : AAdminCommand
    {
        public AdminChat()
        {
            Cmd = "chat";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            admin.ShowHtmAdmin("main.htm", false);
        }
    }
}