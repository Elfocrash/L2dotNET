using System.Linq;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAutoSoulShot : GameServerNetworkRequest
    {
        private int itemId;
        private int type;

        public RequestAutoSoulShot(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
        }

        public override void read()
        {
            itemId = readD();
            type = readD(); //1 - enable
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = player.Inventory.GetItemByItemId(itemId);
            if ((item == null) || !item.Template.isAutoSS)
            {
                player.SendActionFailed();
                return;
            }

            if (type == 1)
            {
                if (player.autoSoulshots.Contains(itemId))
                {
                    player.SendActionFailed();
                    return;
                }

                player.autoSoulshots.Add(itemId);
                player.SendPacket(new ExAutoSoulShot(itemId, type));
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.USE_OF_S1_WILL_BE_AUTO).AddItemName(itemId));

                L2Item weapon = player.Inventory.GetPaperdollItem(Inventory.PaperdollRhand);
                if (weapon != null)
                    foreach (int sid in weapon.Template.getSoulshots().Where(sid => sid == itemId))
                    {
                        if (!weapon.Soulshot)
                        {
                            if (!player.HasItem(sid, weapon.Template.SoulshotCount))
                            {
                                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.CANNOT_AUTO_USE_LACK_OF_S1).AddItemName(itemId));
                                player.SendActionFailed();
                                return;
                            }

                            player.DestroyItemById(itemId, weapon.Template.SoulshotCount);
                            weapon.Soulshot = true;
                            player.BroadcastSoulshotUse(itemId);
                        }

                        break;
                    }
            }
            else
            {
                lock (player.autoSoulshots)
                {
                    player.autoSoulshots.Remove(itemId);
                }

                player.SendPacket(new ExAutoSoulShot(itemId, 0));
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.AUTO_USE_OF_S1_CANCELLED).AddItemName(itemId));
            }
        }
    }
}