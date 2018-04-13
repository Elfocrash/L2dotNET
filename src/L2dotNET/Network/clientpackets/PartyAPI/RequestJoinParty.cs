using System.Linq;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.PartyAPI
{
    class RequestJoinParty : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _name;
        private readonly int _itemDistribution;

        public RequestJoinParty(Packet packet, GameClient client)
        {
            _client = client;
            _name = packet.ReadString();
            _itemDistribution = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            L2Player target = player.KnownObjects.Values.OfType<L2Player>().FirstOrDefault(obj => obj.Name.Equals(_name));

            //if (name.Equals(player.Name))
            //{
            //    player.sendActionFailed();
            //    return;
            //}

            //if (target == null)
            //    target = L2World.Instance.GetPlayer(name);

            if (target == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.FirstSelectUserToInviteToParty);
                player.SendActionFailed();
                return;
            }

            if (!target.Visible)
            {
                player.SendMessage("That player is invisible and cannot be invited.");
                player.SendActionFailed();
                return;
            }

            if (target.Party != null)
            {
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1IsAlreadyInParty);
                sm.AddPlayerName(target.Name);
                player.SendPacket(sm);
                player.SendActionFailed();
                return;
            }

            if (player.IsCursed || target.IsCursed)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouHaveInvitedTheWrongTarget);
                player.SendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.WaitingForAnotherReply);
                player.SendActionFailed();
                return;
            }

            if ((target.TradeState == 1) || (target.TradeState == 2))
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1IsBusyTryLater).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if ((player.Party != null) && (player.Party.Leader.ObjId != player.ObjId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLeaderCanInvite);
                player.SendActionFailed();
                return;
            }

            if (player.IsInOlympiad)
            {
                player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.UserCurrentlyParticipatingInOlympiadCannotSendPartyAndFriendInvitations);
                player.SendActionFailed();
                return;
            }

            if ((player.Party != null) && (player.Party.Members.Count == 9))
            {
                player.Requester.SendSystemMessage(SystemMessage.SystemMessageId.PartyFull);
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.YouInvitedS1ToParty).AddPlayerName(target.Name));
            target.PendToJoinParty(player, _itemDistribution);
        }
    }
}