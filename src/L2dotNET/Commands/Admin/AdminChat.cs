using L2dotNET.model.player;

namespace L2dotNET.Commands.Admin
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