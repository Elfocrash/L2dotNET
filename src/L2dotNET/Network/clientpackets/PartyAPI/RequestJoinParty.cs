using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestJoinParty : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _name;
        private readonly int _itemDistribution;

        public RequestJoinParty(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _name = packet.ReadString();
            _itemDistribution = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
                L2Player target = player.KnownObjects.Values.OfType<L2Player>().FirstOrDefault(obj => obj.Name.Equals(_name));

                //if (name.Equals(player.Name))
                //{
                //    player.sendActionFailed();
                //    return;
                //}

                //if (target == null)
                //    target = L2World.GetPlayer(name);

                if (target == null)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.FirstSelectUserToInviteToParty);
                    player.SendActionFailedAsync();
                    return;
                }

                if (!target.Visible)
                {
                    player.SendMessageAsync("That player is invisible and cannot be invited.");
                    player.SendActionFailedAsync();
                    return;
                }

                if (target.Party != null)
                {
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1IsAlreadyInParty);
                    sm.AddPlayerName(target.Name);
                    player.SendPacketAsync(sm);
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.IsCursed || target.IsCursed)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.YouHaveInvitedTheWrongTarget);
                    player.SendActionFailedAsync();
                    return;
                }

                if (target.PartyState == 1)
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.WaitingForAnotherReply);
                    player.SendActionFailedAsync();
                    return;
                }

                if ((target.TradeState == 1) || (target.TradeState == 2))
                {
                    player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.S1IsBusyTryLater).AddPlayerName(target.Name));
                    player.SendActionFailedAsync();
                    return;
                }

                if ((player.Party != null) && (player.Party.Leader.ObjId != player.ObjId))
                {
                    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLeaderCanInvite);
                    player.SendActionFailedAsync();
                    return;
                }

                if (player.IsInOlympiad)
                {
                    player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.UserCurrentlyParticipatingInOlympiadCannotSendPartyAndFriendInvitations);
                    player.SendActionFailedAsync();
                    return;
                }

                if ((player.Party != null) && (player.Party.Members.Count == 9))
                {
                    player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.PartyFull);
                    player.SendActionFailedAsync();
                    return;
                }

                player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.YouInvitedS1ToParty).AddPlayerName(target.Name));
                target.PendToJoinParty(player, _itemDistribution);
            });
        }
    }
}