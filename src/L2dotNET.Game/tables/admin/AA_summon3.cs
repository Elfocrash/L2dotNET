using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.tables.admin
{
    class AA_summon3 : _adminAlias
    {
        public AA_summon3()
        {
            cmd = "summon3";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //summon3 [id] [id2] -- призывает предметы от [id] до [id2]

            int idmin = int.Parse(alias.Split(' ')[1]);
            int idmax = int.Parse(alias.Split(' ')[2]);

            if (idmax - idmin > 200)
            {
                admin.sendMessage("Too big id range.");
                return;
            }

            bool x = false;
            for(int i = idmin; i <= idmax; i++)
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
