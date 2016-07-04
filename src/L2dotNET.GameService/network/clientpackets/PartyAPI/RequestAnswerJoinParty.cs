using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestAnswerJoinParty : GameServerNetworkRequest
    {
        private int response;

        public RequestAnswerJoinParty(GameClient client, byte[] data)
        {
            makeme(client, data);
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
                player.SendActionFailed();
                return;
            }

            if (player.requester.IsInOlympiad)
            {
                response = 0;
                player.requester.SendSystemMessage(SystemMessage.SystemMessageId.USER_CURRENTLY_PARTICIPATING_IN_OLYMPIAD_CANNOT_SEND_PARTY_AND_FRIEND_INVITATIONS);
            }

            player.requester.SendPacket(new JoinParty(response));

            switch (response)
            {
                case -1:
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.C1_IS_SET_TO_REFUSE_PARTY_REQUESTS);
                    sm.AddPlayerName(player.Name);
                    player.requester.SendPacket(sm);
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
                player.SendSystemMessage(SystemMessage.SystemMessageId.CANNOT_ENTER_DUE_PARTY_HAVING_EXCEED_LIMIT);
                return;
            }

            leader.Party.addMember(player, true);
        }
    }
}