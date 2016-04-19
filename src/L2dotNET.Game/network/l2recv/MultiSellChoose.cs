using L2dotNET.Game.model.items;
using L2dotNET.Game.tables.multisell;

namespace L2dotNET.Game.network.l2recv
{
    class MultiSellChoose : GameServerNetworkRequest
    {
        private int _listId;
        private int _entryId;
        private long _amount;
        private short _enchant;
        private int _unk2;
        private int _unk3;
        private short _unk4;
        private short _unk5;
        private short _unk6;
        private short _unk7;
        private short _unk8;
        private short _unk9;
        private short _unk10;
        private short _unk11;

        public MultiSellChoose(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            _listId = readD();
            _entryId = readD();
            _amount = readQ();
            _enchant = readH();
            _unk2 = readD();
            _unk3 = readD();
            _unk4 = readH(); // elemental attributes
            _unk5 = readH();// elemental attributes
            _unk6 = readH();// elemental attributes
            _unk7 = readH();// elemental attributes
            _unk8 = readH();// elemental attributes
            _unk9 = readH();// elemental attributes
            _unk10 = readH();// elemental attributes
            _unk11 = readH();// elemental attributes
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (_amount < 0)
                _amount = 1;

            if (player.LastRequestedMultiSellId != _listId)
            {
                player.sendMessage("You are not authorized to use this list now.");
                player.sendActionFailed();
                return;
            }

            MultiSellList list = null;
            if (player.CustomMultiSellList != null)
            {
                list = player.CustomMultiSellList;
                if(list.id != _listId)
                    list = MultiSell.getInstance().getList(_listId);
            }
            else
                list = MultiSell.getInstance().getList(_listId);

            if (list == null || list.container.Count < _entryId)
            {
                player.sendSystemMessage(1802);//The attempt to trade has failed.
                player.sendActionFailed();
                return;
            }

            MultiSellEntry entry = list.container[_entryId];

            bool ok = true;
            foreach (MultiSellItem i in entry.take)
            {
                if (i.id > 0)
                {
                    if (!player.hasItem(i.id, i.count))
                    {
                        ok = false;
                        break;
                    }
                }
                else
                {
                    switch (i.id)
                    {
                        case -100: //pvppoint
                            if (player.Fame < i.count * _amount)
                                ok = false;
                            break;
                    }
                }

            }

            if(!ok)
            {
                player.sendSystemMessage(701);//You do not have enough required items.
                player.sendActionFailed();
                return;
            }

            foreach (MultiSellItem i in entry.take)
            {
                if (i.l2item != null)
                    player.Inventory.destroyItem(i.l2item, 1, true, true);
                else
                {
                    if (i.id > 0)
                        player.Inventory.destroyItem(i.id, i.count, true, true);
                    else
                    {
                        switch (i.id)
                        {
                            case -100: //pvppoint
                                break;
                        }
                    }
                }
            }

            foreach (MultiSellItem i in entry.give)
            {
                if (i.id > 0)
                {
                    if (i.l2item != null)
                    {
                        L2Item item = new L2Item(i.template);
                        item.Enchant = i.l2item.Enchant;
                        player.Inventory.addItem(item, true, true);
                    }
                    else
                    {
                        if (i.template.isStackable())
                            player.Inventory.addItem(i.id, i.count * _amount, i.enchant, true, true);
                        else
                        {
                            player.Inventory.addItem(i.id, 1, i.enchant, true, true);
                        }
                    }
                }
                else
                {
                    switch (i.id)
                    {
                        case -100: //pvppoint
                            break;
                    }
                }
            }

            player.sendSystemMessage(1656);//You have successfully traded the item with the NPC.
        }
    }
}
