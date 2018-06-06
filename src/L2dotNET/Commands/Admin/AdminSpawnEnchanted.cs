using System;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "summon2")]
    class AdminSpawnEnchanted : AAdminCommand
    {
        protected internal override async Task UseAsync(L2Player admin, string alias)
        {
            await Task.Run(() =>
            {
                short enchant = short.Parse(alias.Split(' ')[1]);
                int id = int.Parse(alias.Split(' ')[2]);
                admin.AddItem(id, 1);
            });
        }

        public AdminSpawnEnchanted(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}