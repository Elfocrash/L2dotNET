using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestAnswerJoinParty : GameServerNetworkRequest
    {
        private int _response;

        public RequestAnswerJoinParty(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _response = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;
            player.PartyState = 0;

            if (player.Requester == null)
            {
                player.SendActionFailed();
                return;
            }

            if (player.Requester.IsInOlympiad)
            {
                _response = 0;
                player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.UserCurrentlyParticipatingInOlympiadCannotSendPartyAndFriendInvitations);
            }

            player.Requester.SendPacket(new JoinParty(_response));

            switch (_response)
            {
                case -1:
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.C1IsSetToRefusePartyRequests);
                    sm.AddPlayerName(player.Name);
                    player.Requester.SendPacket(sm);
                }
                    break;
                case 1:
                    AcceptPartyInvite(player.Requester, player);
                    break;
            }
        }

        private void AcceptPartyInvite(L2Player leader, L2Player player)
        {
            if (leader.Party == null)
            {
                leader.Party = new L2Party(leader);
            }

            if (leader.Party.Members.Count == 9)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotEnterDuePartyHavingExceedLimit);
                return;
            }

            leader.Party.AddMember(player, true);
        }
    }
}