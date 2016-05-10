﻿using L2dotNET.Game.model.communities;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2recv
{
    class RequestPledgeInfo : GameServerNetworkRequest
    {
        public RequestPledgeInfo(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _clanId;
        public override void read()
        {
            _clanId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            L2Clan clan = ClanTable.Instance.getClan(_clanId);
            if (clan != null)
                player.sendPacket(new PledgeInfo(clan.ClanID, clan.Name, clan.AllianceName));
        }
    }
}
