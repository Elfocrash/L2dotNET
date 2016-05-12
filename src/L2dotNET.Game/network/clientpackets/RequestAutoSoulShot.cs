using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestAutoSoulShot : GameServerNetworkRequest
    {
        private int itemId;
        private int type;
        public RequestAutoSoulShot(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            itemId = readD();
            type = readD(); //1 - enable
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = player.Inventory.getItemById(itemId);
            if (item == null || !item.Template.isAutoSS)
            {
                player.sendActionFailed();
                return;
            }

            if (type == 1)
            {
                if (player.autoSoulshots.Contains(itemId))
                {
                    player.sendActionFailed();
                    return;
                }

                player.autoSoulshots.Add(itemId);
                player.sendPacket(new ExAutoSoulShot(itemId, type)); //The automatic use of $s1 has been activated.
                player.sendPacket(new SystemMessage(1433).AddItemName(itemId));

                L2Item weapon = player.Inventory.getWeapon();
                if (weapon != null)
                {
                    foreach (int sid in weapon.Template.getSoulshots())
                        if (sid == itemId)
                        {
                            if (!weapon.Soulshot)
                            {
                                if (!player.hasItem(sid, weapon.Template.SoulshotCount))
                                {
                                    player.sendPacket(new SystemMessage(1436).AddItemName(itemId));//Due to insufficient $s1, the automatic use function cannot be activated.
                                    player.sendActionFailed();
                                    return;
                                }

                                player.Inventory.destroyItem(itemId, weapon.Template.SoulshotCount, false, true);
                                weapon.Soulshot = true;
                                player.broadcastSoulshotUse(itemId);
                            }
                            break;
                        }
                }
            }
            else
            {
                lock (player.autoSoulshots)
                    player.autoSoulshots.Remove(itemId);

                player.sendPacket(new ExAutoSoulShot(itemId, 0)); //The automatic use of $s1 has been deactivated.
                player.sendPacket(new SystemMessage(1434).AddItemName(itemId));
            }
        }
    }
}
