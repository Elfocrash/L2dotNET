using L2dotNET.GameService.Commands;

namespace L2dotNET.GameService.Command
{
    class AdminSpawnEnchanted : AAdminCommand
    {
        public AdminSpawnEnchanted()
        {
            Cmd = "summon2";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            //summon2 [enchant] [id | name] -- призывает предмет [id | name] , заточенный на [enchant]

            short enchant = short.Parse(alias.Split(' ')[1]);
            int id = id = int.Parse(alias.Split(' ')[2]);
            admin.Inventory.addItem(id, 1, enchant, true, true);
        }
    }
}