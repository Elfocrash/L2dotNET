using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables.Multisell;

namespace L2dotNET.GameService.Network.Clientpackets
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
            makeme(client, data);
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
            _unk5 = readH(); // elemental attributes
            _unk6 = readH(); // elemental attributes
            _unk7 = readH(); // elemental attributes
            _unk8 = readH(); // elemental attributes
            _unk9 = readH(); // elemental attributes
            _unk10 = readH(); // elemental attributes
            _unk11 = readH(); // elemental attributes
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (_amount < 0)
                _amount = 1;

            if (player.LastRequestedMultiSellId != _listId)
            {
                player.SendMessage("You are not authorized to use this list now.");
                player.SendActionFailed();
                return;
            }

            MultiSellList list;
            if (player.CustomMultiSellList != null)
            {
                list = player.CustomMultiSellList;
                if (list.id != _listId)
                    list = MultiSell.Instance.getList(_listId);
            }
            else
                list = MultiSell.Instance.getList(_listId);

            if ((list == null) || (list.container.Count < _entryId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TRADE_ATTEMPT_FAILED);
                player.SendActionFailed();
                return;
            }

            MultiSellEntry entry = list.container[_entryId];

            bool ok = true;
            foreach (MultiSellItem i in entry.take)
                if (i.id > 0)
                {
                    if (!player.HasItem(i.id, i.count))
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

            if (!ok)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_REQUIRED_ITEMS);
                player.SendActionFailed();
                return;
            }

            foreach (MultiSellItem i in entry.take)
                if (i.l2item != null)
                    player.DestroyItem(i.l2item, 1);
                else
                {
                    if (i.id > 0)
                        player.DestroyItemById(i.id, i.count);
                    else
                        switch (i.id)
                        {
                            case -100: //pvppoint
                                break;
                        }
                }

           

            player.SendSystemMessage(SystemMessage.SystemMessageId.SUCCESSFULLY_TRADED_WITH_NPC);
        }
    }
}