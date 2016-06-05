namespace L2dotNET.GameService.model.items.effects
{
    class BottlesOfSouls : ItemEffect
    {
        public BottlesOfSouls()
        {
            ids = new int[] { 10409, //A Bottle of Souls
                              10410, //Full Bottle of Souls - 5 Souls
                              10411, //Full Bottle of Souls - 5 Souls (For Combat)
                              10412 //Full Bottle of Souls - 10 Souls
                            };
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            byte method = 0,
                 add = 0,
                 rem = 1,
                 add_battle = 2,
                 count = 5,
                 fin = 0;
            short reward = 0;
            switch (item.Template.ItemID)
            {
                case 10409:
                    method = rem;
                    reward = 10410;
                    break;
                case 10411:
                    method = add_battle;
                    break;
                case 10412:
                    count = 10;
                    break;
            }

            if (method == add)
            {
                fin = 1;
            }
            else if (method == add_battle)
            {
                fin = player.isInBattle() ? (byte)1 : (byte)0;
            }
            else if (method == rem)
            {
                if (!player.CheckFreeSlotsInventory80(reward, 1, true))
                    return;
            }

            player.Inventory.destroyItem(item, 1, true, true);

            if (reward > 0)
                player.Inventory.addItem(reward, 1, true, true);

            if (method != rem)
                player.AddSouls(count);
            else
                player.ReduceSouls(count);
        }
    }
}