using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestJoinParty : GameServerNetworkRequest
    {
        private string name;
        private int itemDistribution;

        public RequestJoinParty(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            name = readS();
            itemDistribution = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;
            L2Player target = null;

            //if (name.Equals(player.Name))
            //{
            //    player.sendActionFailed();
            //    return;
            //}

            foreach (L2Object obj in player.knownObjects.Values)
            {
                if (obj is L2Player)
                    if (((L2Player)obj).Name.Equals(name))
                    {
                        target = (L2Player)obj;
                        break;
                    }
            }

            //if (target == null)
            //    target = L2World.Instance.GetPlayer(name);

            if (target == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.FIRST_SELECT_USER_TO_INVITE_TO_PARTY);
                player.sendActionFailed();
                return;
            }

            if (!target.Visible)
            {
                player.sendMessage("That player is invisible and cannot be invited.");
                player.sendActionFailed();
                return;
            }

            if (target.Party != null)
            {
                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_IS_ALREADY_IN_PARTY);
                sm.AddPlayerName(target.Name);
                player.sendPacket(sm);
                player.sendActionFailed();
                return;
            }

            if (player.IsCursed || target.IsCursed)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_HAVE_INVITED_THE_WRONG_TARGET);
                player.sendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.WAITING_FOR_ANOTHER_REPLY);
                player.sendActionFailed();
                return;
            }

            if (target.TradeState == 1 || target.TradeState == 2)
            {
                player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.S1_IS_BUSY_TRY_LATER).AddPlayerName(target.Name));
                player.sendActionFailed();
                return;
            }

            if (player.Party != null && player.Party.leader.ObjID != player.ObjID)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.ONLY_LEADER_CAN_INVITE);
                player.sendActionFailed();
                return;
            }

            if (player.IsInOlympiad)
            {
                player.requester.sendSystemMessage(SystemMessage.SystemMessageId.USER_CURRENTLY_PARTICIPATING_IN_OLYMPIAD_CANNOT_SEND_PARTY_AND_FRIEND_INVITATIONS);
                player.sendActionFailed();
                return;
            }

            if (player.Party != null && player.Party.Members.Count == 9)
            {
                player.requester.sendSystemMessage(SystemMessage.SystemMessageId.PARTY_FULL);
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.YOU_INVITED_S1_TO_PARTY).AddPlayerName(target.Name));
            target.PendToJoinParty(player, itemDistribution);
        }
    }
}