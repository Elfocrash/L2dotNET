using L2dotNET.GameService.model.communities;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
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
                player.sendSystemMessage(SystemMessage.SystemMessageId.CLAN_LVL_3_NEEDED_TO_SET_CREST);
                player.sendActionFailed();
                return;
            }

            if (clan.IsDissolving())
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_SET_CREST_WHILE_DISSOLUTION_IN_PROGRESS);
                player.sendActionFailed();
                return;
            }

            if (_size < 0 || _size > 256)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CAN_ONLY_REGISTER_16_12_PX_256_COLOR_BMP_FILES);
                player.sendActionFailed();
                return;
            }

            if ((player.ClanPrivs & L2Clan.CP_CL_REGISTER_CREST) != L2Clan.CP_CL_REGISTER_CREST)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NOT_AUTHORIZED_TO_BESTOW_RIGHTS);
                player.sendActionFailed();
                return;
            }

            clan.updateCrest(_size, _picture);
        }
    }
}