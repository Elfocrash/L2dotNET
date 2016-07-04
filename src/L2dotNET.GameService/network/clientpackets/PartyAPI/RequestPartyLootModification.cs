using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestPartyLootModification : GameServerNetworkRequest
    {
        private byte _mode;

        public RequestPartyLootModification(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _mode = (byte)ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Party == null)
            {
                player.SendActionFailed();
                return;
            }

            if ((_mode < player.Party.ItemLooter) || (_mode > player.Party.ItemOrderSpoil) || (_mode == player.Party.ItemDistribution) || (player.Party.Leader.ObjId != player.ObjId))
            {
                player.SendActionFailed();
                return;
            }

            player.Party.VoteForLootChange(_mode);
        }
    }
}