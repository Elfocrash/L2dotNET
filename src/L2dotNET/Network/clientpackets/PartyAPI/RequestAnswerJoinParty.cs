using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestAnswerJoinParty : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _response;

        public RequestAnswerJoinParty(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _response = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
                player.PartyState = 0;

                if (player.Requester == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.Requester.IsInOlympiad)
                {
                    player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.UserCurrentlyParticipatingInOlympiadCannotSendPartyAndFriendInvitations);
                    return;
                }

                player.Requester.SendPacketAsync(new JoinParty(_response));

                switch (_response)
                {
                    case -1:
                    {
                        SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.C1IsSetToRefusePartyRequests);
                        sm.AddPlayerName(player.Name);
                        player.Requester.SendPacketAsync(sm);
                    }
                        break;
                    case 1:
                        AcceptPartyInvite(player.Requester, player);
                        break;
                }
            });
        }

        private void AcceptPartyInvite(L2Player leader, L2Player player)
        {
            if (leader.Party == null)
                leader.Party = new L2Party(leader);

            if (leader.Party.Members.Count == 9)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.CannotEnterDuePartyHavingExceedLimit);
                return;
            }

            leader.Party.AddMember(player, true);
        }
    }
}