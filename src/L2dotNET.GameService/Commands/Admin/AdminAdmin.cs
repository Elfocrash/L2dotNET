using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Commands.Admin
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