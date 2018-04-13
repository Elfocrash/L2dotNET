using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "summon2")]
    class AdminSpawnEnchanted : AAdminCommand
    {
        protected internal override void Use(L2Player admin, string alias)
        {
            //summon2 [enchant] [id | name] -- призывает предмет [id | name] , заточенный на [enchant]

            short enchant = short.Parse(alias.Split(' ')[1]);
            int id = int.Parse(alias.Split(' ')[2]);
            admin.AddItem(id, 1);
        }
    }
}