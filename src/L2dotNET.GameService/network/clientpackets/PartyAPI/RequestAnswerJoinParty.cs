using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestAnswerJoinParty : PacketBase
    {
        private int _response;
        private readonly GameClient _client;

        public RequestAnswerJoinParty(Packet packet, GameClient client)
        {
            _client = client;
            _response = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
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