using L2dotNET.model.player;

namespace L2dotNET.Commands.Admin
{
    class AdminAdmin : AAdminCommand
    {
        public AdminAdmin()
        {
            Cmd = "admin";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            admin.ShowHtm("admin/main_menu.htm",admin);
        }
    }
}