using L2dotNET.GameService.Commands;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.Command
{
    class AdminSpawnItemRange : AAdminCommand
    {
        public AdminSpawnItemRange()
        {
            Cmd = "summon3";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            int idmin = int.Parse(alias.Split(' ')[1]);
            int idmax = int.Parse(alias.Split(' ')[2]);

            if (idmax - idmin > 200)
            {
                admin.sendMessage("Too big id range.");
                return;
            }

            bool x = false;
            for (int i = idmin; i <= idmax; i++)
            {
                ItemTemplate item = ItemTable.Instance.GetItem(i);

                if (item == null)
                    admin.sendMessage("Item with id " + i + " not exists.");
                else
                {
                    admin.Inventory.addItem(i, 1, true, false);
                    x = true;
                }
            }

            if (x)
                admin.sendItemList(true);
        }
    }
}