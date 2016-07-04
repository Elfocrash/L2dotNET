using System.Linq;
using L2dotNET.GameService.Model.Inventory;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAutoSoulShot : GameServerNetworkRequest
    {
        private int _itemId;
        private int _type;

        public RequestAutoSoulShot(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _itemId = ReadD();
            _type = ReadD(); //1 - enable
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Item item = player.Inventory.GetItemByItemId(_itemId);
            if ((item == null) || !item.Template.IsAutoSs)
            {
                player.SendActionFailed();
                return;
            }

            if (_type == 1)
            {
                if (player.AutoSoulshots.Contains(_itemId))
                {
                    player.SendActionFailed();
                    return;
                }

                player.AutoSoulshots.Add(_itemId);
                player.SendPacket(new ExAutoSoulShot(_itemId, _type));
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.UseOfS1WillBeAuto).AddItemName(_itemId));

                L2Item weapon = player.Inventory.GetPaperdollItem(Inventory.PaperdollRhand);
                if (weapon != null)
                    foreach (int sid in weapon.Template.GetSoulshots().Where(sid => sid == _itemId))
                    {
                        if (!weapon.Soulshot)
                        {
                            if (!player.HasItem(sid, weapon.Template.SoulshotCount))
                            {
                                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.CannotAutoUseLackOfS1).AddItemName(_itemId));
                                player.SendActionFailed();
                                return;
                            }

                            player.DestroyItemById(_itemId, weapon.Template.SoulshotCount);
                            weapon.Soulshot = true;
                            player.BroadcastSoulshotUse(_itemId);
                        }

                        break;
                    }
            }
            else
            {
                lock (player.AutoSoulshots)
                {
                    player.AutoSoulshots.Remove(_itemId);
                }

                player.SendPacket(new ExAutoSoulShot(_itemId, 0));
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.AutoUseOfS1Cancelled).AddItemName(_itemId));
            }
        }
    }
}