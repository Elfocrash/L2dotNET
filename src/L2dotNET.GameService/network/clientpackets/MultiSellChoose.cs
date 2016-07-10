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
            Makeme(client, data);
        }

        public override void Read()
        {
            _listId = ReadD();
            _entryId = ReadD();
            _amount = ReadQ();
            _enchant = ReadH();
            _unk2 = ReadD();
            _unk3 = ReadD();
            _unk4 = ReadH(); // elemental attributes
            _unk5 = ReadH(); // elemental attributes
            _unk6 = ReadH(); // elemental attributes
            _unk7 = ReadH(); // elemental attributes
            _unk8 = ReadH(); // elemental attributes
            _unk9 = ReadH(); // elemental attributes
            _unk10 = ReadH(); // elemental attributes
            _unk11 = ReadH(); // elemental attributes
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if (_amount < 0)
            {
                _amount = 1;
            }

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
                if (list.Id != _listId)
                {
                    list = MultiSell.Instance.GetList(_listId);
                }
            }
            else
            {
                list = MultiSell.Instance.GetList(_listId);
            }

            if ((list == null) || (list.Container.Count < _entryId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.TradeAttemptFailed);
                player.SendActionFailed();
                return;
            }

            MultiSellEntry entry = list.Container[_entryId];

            bool ok = true;
            foreach (MultiSellItem i in entry.Take)
                if (i.Id > 0)
                {
                    if (player.HasItem(i.Id, i.Count))
                    {
                        continue;
                    }

                    ok = false;
                    break;
                }
                else
                {
                    switch (i.Id)
                    {
                        case -100: //pvppoint
                            if (player.Fame < (i.Count * _amount))
                            {
                                ok = false;
                            }
                            break;
                    }
                }

            if (!ok)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughRequiredItems);
                player.SendActionFailed();
                return;
            }

            foreach (MultiSellItem i in entry.Take)
                if (i.L2Item != null)
                {
                    player.DestroyItem(i.L2Item, 1);
                }
                else
                {
                    if (i.Id > 0)
                    {
                        player.DestroyItemById(i.Id, i.Count);
                    }
                    else
                    {
                        switch (i.Id)
                        {
                            case -100: //pvppoint
                                break;
                        }
                    }
                }

            player.SendSystemMessage(SystemMessage.SystemMessageId.SuccessfullyTradedWithNpc);
        }
    }
}