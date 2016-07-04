using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets.PartyAPI
{
    class RequestJoinParty : GameServerNetworkRequest
    {
        private string name;
        private int itemDistribution;

        public RequestJoinParty(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readS();
            itemDistribution = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;
            L2Player target = player.KnownObjects.Values.OfType<L2Player>().FirstOrDefault(obj => obj.Name.Equals(name));

            //if (name.Equals(player.Name))
            //{
            //    player.sendActionFailed();
            //    return;
            //}

            //if (target == null)
            //    target = L2World.Instance.GetPlayer(name);

            if (target == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.FIRST_SELECT_USER_TO_INVITE_TO_PARTY);
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
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_IS_ALREADY_IN_PARTY);
                sm.AddPlayerName(target.Name);
                player.SendPacket(sm);
                player.SendActionFailed();
                return;
            }

            if (player.IsCursed || target.IsCursed)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YOU_HAVE_INVITED_THE_WRONG_TARGET);
                player.SendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.WAITING_FOR_ANOTHER_REPLY);
                player.SendActionFailed();
                return;
            }

            if ((target.TradeState == 1) || (target.TradeState == 2))
            {
                player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_IS_BUSY_TRY_LATER).AddPlayerName(target.Name));
                player.SendActionFailed();
                return;
            }

            if ((player.Party != null) && (player.Party.leader.ObjId != player.ObjId))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.ONLY_LEADER_CAN_INVITE);
                player.SendActionFailed();
                return;
            }

            if (player.IsInOlympiad)
            {
                player.requester.SendSystemMessage(SystemMessage.SystemMessageId.USER_CURRENTLY_PARTICIPATING_IN_OLYMPIAD_CANNOT_SEND_PARTY_AND_FRIEND_INVITATIONS);
                player.SendActionFailed();
                return;
            }

            if ((player.Party != null) && (player.Party.Members.Count == 9))
            {
                player.requester.SendSystemMessage(SystemMessage.SystemMessageId.PARTY_FULL);
                player.SendActionFailed();
                return;
            }

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.YOU_INVITED_S1_TO_PARTY).AddPlayerName(target.Name));
            target.PendToJoinParty(player, itemDistribution);
        }
    }
}