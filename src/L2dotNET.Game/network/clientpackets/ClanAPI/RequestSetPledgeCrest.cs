using L2dotNET.Game.model.communities;

namespace L2dotNET.Game.network.l2recv
{
    class RequestSetPledgeCrest : GameServerNetworkRequest
    {
        private int _size;
        private byte[] _picture;
        public RequestSetPledgeCrest(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            _size = readD();

            if (_size > 0 && _size <= 256)
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

            if (clan.Level < 3)
            {
                player.sendSystemMessage(272); //A clan crest can only be registered when the clan's skill level is 3 or above.
                player.sendActionFailed();
                return;
            }

            if (clan.IsDissolving())
            {
                player.sendSystemMessage(552); //As you are currently schedule for clan dissolution, you cannot register or delete a Clan Crest.
                player.sendActionFailed();
                return;
            }

            if (_size < 0 || _size > 256)
            {
                player.sendSystemMessage(211); //You can only register 16x12 pixel 256 color bmp files.
                player.sendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CP_CL_REGISTER_CREST) != L2Clan.CP_CL_REGISTER_CREST)
            {
                player.sendSystemMessage(235); //You are not authorized to bestow these rights.
                player.sendActionFailed();
                return;
            }

            clan.updateCrest(_size, _picture);
        }
    }
}
