using L2dotNET.GameService.network.l2send;
using System;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestAnswerJoinParty : GameServerNetworkRequest
    {
        private int response;
        public RequestAnswerJoinParty(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            response = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;
            player.PartyState = 0;

            if (player.requester == null)
            {
                player.sendActionFailed();
                return;
            }

            if (player.requester.IsInOlympiad)
            {
                response = 0;
                player.requester.sendSystemMessage(3094);//A user currently participating in the Olympiad cannot send party and friend invitations.
            }

            player.requester.sendPacket(new JoinParty(response));

            switch (response)
            {
                case -1:
                    {
                        SystemMessage sm = new SystemMessage(3168); //$c1 is set to refuse party requests and cannot receive a party request.
                        sm.AddPlayerName(player.Name);
                        player.requester.sendPacket(sm);
                    }
                    break;
                case 1:
                    acceptPartyInvite(player.requester, player);
                    break;
            }
        }

        private void acceptPartyInvite(L2Player leader, L2Player player)
        {
            if (leader.Party == null)
                leader.Party = new L2Party(leader);

            if (leader.Party.Members.Count == 9)
            {
                player.sendSystemMessage(2102); //You cannot enter due to the party having exceeded the limit
                return;
            }

            leader.Party.addMember(player, true);
        }
    }
}
