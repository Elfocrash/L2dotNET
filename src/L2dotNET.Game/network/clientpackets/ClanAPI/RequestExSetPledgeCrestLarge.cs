using L2dotNET.GameService.Model.communities;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets.ClanAPI
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
                player.sendSystemMessage(SystemMessage.SystemMessageId.LENGTH_CREST_DOES_NOT_MEET_STANDARD_REQUIREMENTS);
                player.sendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CP_CL_REGISTER_CREST) != L2Clan.CP_CL_REGISTER_CREST)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NOT_AUTHORIZED_TO_BESTOW_RIGHTS);
                player.sendActionFailed();
                return;
            }

            clan.updateCrestLarge(_size, _picture);
        }
    }
}