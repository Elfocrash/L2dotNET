using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables.Multisell;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class MultiSellChoose : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _listId;
        private readonly int _entryId;
        private readonly int _amount;
        private readonly short _enchant;
        private readonly int _unk2;
        private readonly int _unk3;

        public MultiSellChoose(Packet packet, GameClient client)
        {
            _client = client;
            _listId = packet.ReadInt();
            _entryId = packet.ReadInt();
            _amount = packet.ReadInt();
            if (_amount < 0)
                _amount = 1;
            _enchant = packet.ReadShort();
            _unk2 = packet.ReadInt();
            _unk3 = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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
                    list = MultiSell.Instance.GetList(_listId);
            }
            else
                list = MultiSell.Instance.GetList(_listId);

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
                        continue;

                    ok = false;
                    break;
                }
                else
                {
                    switch (i.Id)
                    {
                        case -100: //pvppoint
                            if (player.Fame < (i.Count * _amount))
                                ok = false;
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
                    player.DestroyItem(i.L2Item, 1);
                else
                {
                    if (i.Id > 0)
                        player.DestroyItemById(i.Id, i.Count);
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