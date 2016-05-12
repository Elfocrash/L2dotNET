using L2dotNET.GameService.model.communities;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestExSetPledgeCrestLarge : GameServerNetworkRequest
    {
        private int _size;
        private byte[] _picture;
        public RequestExSetPledgeCrestLarge(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            _size = readD();

            if (_size > 0 && _size <= 2176)
                _picture = readB(_size);
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player.ClanId == 0)
            {
                player.sendActionFailed();
                return;
            }

            L2Clan clan = player.Clan;

            if (clan.HideoutID == 0 || clan.CastleID == 0)
            {
                player.sendMessage("You need to own clan hall or castle to assign this emblem.");
                player.sendActionFailed();
                return;
            }

            if (_size < 0 || _size > 2176)
            {
                player.sendSystemMessage(2285); //The length of the crest or insignia does not meet the standard requirements.
                player.sendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CP_CL_REGISTER_CREST) != L2Clan.CP_CL_REGISTER_CREST)
            {
                player.sendSystemMessage(235); //You are not authorized to bestow these rights.
                player.sendActionFailed();
                return;
            }

            clan.updateCrestLarge(_size, _picture);
        }
    }
}
