using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestPartyLootModification : GameServerNetworkRequest
    {
        private byte mode;

        public RequestPartyLootModification(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
        }

        public override void read()
        {
            mode = (byte)readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.sendActionFailed();
                return;
            }

            if ((mode < player.Party.ITEM_LOOTER) || (mode > player.Party.ITEM_ORDER_SPOIL) || (mode == player.Party.itemDistribution) || (player.Party.leader.ObjID != player.ObjID))
            {
                player.sendActionFailed();
                return;
            }

            player.Party.VoteForLootChange(mode);
        }
    }
}