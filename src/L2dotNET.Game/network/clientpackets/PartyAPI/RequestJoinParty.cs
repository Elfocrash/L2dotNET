using L2dotNET.GameService.network.l2send;
using System;
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

            if (target == null)
                target = L2World.Instance.GetPlayer(name);

            if (target == null)
            {
                player.sendSystemMessage(185);//You must first select a user to invite to your party.
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
                SystemMessage sm = new SystemMessage(160);
                sm.AddPlayerName(target.Name);
                player.sendPacket(sm);//$c1 is a member of another party and cannot be invited.
                player.sendActionFailed();
                return;
            }

            if (player.IsCursed || target.IsCursed)
            {
                player.sendSystemMessage(152);//You have invited the wrong target.
                player.sendActionFailed();
                return;
            }

            if (target.PartyState == 1)
            {
                player.sendSystemMessage(164);//Waiting for another reply.
                player.sendActionFailed();
                return;
            }

            if (target.TradeState == 1 || target.TradeState == 2)
            {
                player.sendPacket(new SystemMessage(153).AddPlayerName(target.Name));//$c1 is on another task. Please try again later.
                player.sendActionFailed();
                return;
            }

            if (player.Party != null && player.Party.leader.ObjID != player.ObjID)
            {
                player.sendSystemMessage(154);//Only the leader can give out invitations.
                player.sendActionFailed();
                return;
            }

            if (player.IsInOlympiad)
            {
                player.requester.sendSystemMessage(3094);//A user currently participating in the Olympiad cannot send party and friend invitations.
                player.sendActionFailed();
                return;
            }

            if (player.Party != null && player.Party.Members.Count == 9)
            {
                player.requester.sendSystemMessage(155);//The party is full.
                player.sendActionFailed();
                return;
            }

            player.sendPacket(new SystemMessage(105).AddPlayerName(target.Name));//$c1 has been invited to the party.
            target.PendToJoinParty(player, itemDistribution);
        }
    }
}
