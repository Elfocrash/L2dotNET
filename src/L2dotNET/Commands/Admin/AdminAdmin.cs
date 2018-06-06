using System;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "admin")]
    class AdminAdmin : AAdminCommand
    {
        protected internal override async Task UseAsync(L2Player admin, string alias)
        {
            await Task.Run(() =>
            {
                admin.ShowHtm("admin/main_menu.htm",admin);
            });
        }

        public AdminAdmin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}