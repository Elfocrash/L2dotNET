using L2dotNET.GameService.model.player.telebooks;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.items.effects
{
    class TeleportBooks : ItemEffect
    {
        public TeleportBooks()
        {
            ids = new int[] { 
                13015, //My Teleport Spellbook
                13301, //My Teleport Spellbook (Event)
                20025  //My Teleport Spellbook
            };
        }

        public override void UsePlayer(L2Player player, L2Item item)
        {
            if (player.Telbook == null)
                player.Telbook = new TeleportBook();

            if (player.TelbookLimit >= player.TelbookLimitMax)
            {
                player.sendSystemMessage(2390);//Your number of My Teleports slots has reached its maximum limit.
                return;
            }

            player.Inventory.destroyItem(item, 1, true, true);

            player.TelbookLimit += 3;
            if (player.TelbookLimit >= player.TelbookLimitMax)
                player.TelbookLimit = player.TelbookLimitMax;

            //SQL_Block sqb = new SQL_Block("user_data");
            //sqb.param("telbook", player.TelbookLimit);
            //sqb.where("objId", player.ObjID);
            //sqb.sql_update(false);

            player.sendSystemMessage(2409);//The number of My Teleports slots has been increased.
            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, player.Telbook));
        }
    }
}
