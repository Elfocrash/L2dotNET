using L2dotNET.GameService.Model.items;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
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
                player.sendPacket(new ExAutoSoulShot(itemId, type));
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.USE_OF_S1_WILL_BE_AUTO).AddItemName(itemId));

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
                                    player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.CANNOT_AUTO_USE_LACK_OF_S1).AddItemName(itemId));
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

                player.sendPacket(new ExAutoSoulShot(itemId, 0));
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.AUTO_USE_OF_S1_CANCELLED).AddItemName(itemId));
            }
        }
    }
}