using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Timers;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Player
{
    [Synchronization]
    public class L2Party
    {
        public LinkedList<L2Player> Members;
        private readonly L2Player player;
        public int itemDistribution;
        public L2Player leader;

        public L2Party(L2Player player)
        {
            Members = new LinkedList<L2Player>();
            Members.AddLast(player);
            leader = player;
            this.player = player;
            itemDistribution = player.itemDistribution;
        }

        public double[] bonusExp = { 1, 1.30, 1.39, 1.50, 1.54, 1.58, 1.63, 1.67, 1.71 };
        public short[] LOOT_SYSSTRINGS = { 487, 488, 798, 799, 800 };

        public byte ITEM_LOOTER = 0;
        public byte ITEM_RANDOM = 1;
        public byte ITEM_RANDOM_SPOIL = 2;
        public byte ITEM_ORDER = 3;
        public byte ITEM_ORDER_SPOIL = 4;

        public short VoteId = -1;

        public void addMember(L2Player playerMember, bool onCreate = false)
        {
            Members.AddLast(playerMember);

            if (!onCreate)
                playerMember.SendPacket(new PartySmallWindowAll(this));
            else
                broadcastToMembers(new PartySmallWindowAll(this));

            playerMember.Party = this;

            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.YOU_JOINED_S1_PARTY);
            sm.AddPlayerName(leader.Name);
            playerMember.SendPacket(sm);

            sm = new SystemMessage(SystemMessage.SystemMessageId.S1_JOINED_PARTY);
            sm.AddPlayerName(leader.Name);
            broadcastToMembers(sm, playerMember.ObjId);
        }

        public void broadcastToMembers(GameServerNetworkPacket pk)
        {
            foreach (L2Player pl in Members)
                pl.SendPacket(pk);
        }

        public void broadcastToMembers(GameServerNetworkPacket pk, int except)
        {
            foreach (L2Player pl in Members.Where(pl => pl.ObjId != except))
                pl.SendPacket(pk);
        }

        private byte votesOnStart,
                     votesVoted;
        private Timer voteTimer;
        private SortedList<int, byte> votes;

        public void VoteForLootChange(byte mode)
        {
            VoteId = mode;
            broadcastToMembers(new ExAskModifyPartyLooting(leader.Name, mode));
            SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.REQUESTING_APPROVAL_FOR_CHANGING_PARTY_LOOT_TO_S1);
            sm.AddSysStr(LOOT_SYSSTRINGS[mode]);
            leader.SendPacket(sm);

            votesOnStart = (byte)Members.Count;

            voteTimer = new Timer();
            voteTimer.Interval = 30000;
            voteTimer.Enabled = true;
            voteTimer.Elapsed += new ElapsedEventHandler(voteTimer_Elapsed);
            votes = new SortedList<int, byte>(votesOnStart);
        }

        private void voteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FinishVoting();
        }

        public void AnswerLootVote(L2Player playerMember, byte answer)
        {
            votesVoted++;

            if (!votes.ContainsKey(playerMember.ObjId))
                votes.Add(playerMember.ObjId, answer);

            if (votes.Count == votesOnStart)
                FinishVoting();
        }

        private void FinishVoting()
        {
            voteTimer.Enabled = false;

            double half = votesOnStart * 0.5;
            byte agreed = (byte)votes.Values.Count(vote => vote == 1);

            votes.Clear();
            SystemMessage sm;
            if (agreed > half)
            {
                sm = new SystemMessage(SystemMessage.SystemMessageId.PARTY_LOOT_WAS_CHANGED_TO_S1);
                sm.AddSysStr(LOOT_SYSSTRINGS[VoteId]);
                itemDistribution = VoteId;
            }
            else
            {
                sm = new SystemMessage(SystemMessage.SystemMessageId.PARTY_LOOT_CHANGE_WAS_CANCELLED);
                VoteId = -1;
            }

            broadcastToMembers(sm);
            broadcastToMembers(new ExSetPartyLooting(VoteId));
            VoteId = -1;
        }

        public void Leave(L2Player playerMember)
        {
            if (leader.ObjId == playerMember.ObjId)
                if (Members.Count > 2)
                {
                    kick(playerMember);

                    leader = Members.First.Value;
                    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_HAS_BECOME_A_PARTY_LEADER);
                    sm.AddPlayerName(leader.Name);
                    broadcastToMembers(sm);

                    broadcastToMembers(new PartySmallWindowDeleteAll());
                    broadcastToMembers(new PartySmallWindowAll(this));
                }
                else
                {
                    foreach (L2Player pl in Members)
                    {
                        pl.SendSystemMessage(SystemMessage.SystemMessageId.PARTY_DISPERSED);
                        pl.SendPacket(new PartySmallWindowDeleteAll());
                        pl.Party = null;
                    }

                    Members.Clear();

                    if ((voteTimer != null) && voteTimer.Enabled)
                        voteTimer.Enabled = false;
                }
            else
                kick(playerMember);
        }

        private void kick(L2Player playerMember)
        {
            //TODO: missing messages packets?
            //(int)SystemMessage.SystemMessageId.HAVE_BEEN_EXPELLED_FROM_PARTY
            //(int)SystemMessage.SystemMessageId.S1_WAS_EXPELLED_FROM_PARTY

            lock (Members)
            {
                Members.Remove(playerMember);
            }

            if (Members.Count > 2)
            {
                playerMember.SendSystemMessage(SystemMessage.SystemMessageId.YOU_LEFT_PARTY);
                playerMember.SendPacket(new PartySmallWindowDeleteAll());
                playerMember.Party = null;

                SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_LEFT_PARTY);
                sm.AddPlayerName(playerMember.Name);
                broadcastToMembers(sm);
                broadcastToMembers(new PartySmallWindowDelete(playerMember.ObjId, playerMember.Name));

                if (playerMember.Summon != null)
                    broadcastToMembers(new ExPartyPetWindowDelete(playerMember.Summon.ObjId, playerMember.ObjId, playerMember.Summon.Name));
            }
            else
            {
                foreach (L2Player pl in Members)
                {
                    pl.SendSystemMessage(SystemMessage.SystemMessageId.PARTY_DISPERSED);
                    pl.SendPacket(new PartySmallWindowDeleteAll());
                    pl.Party = null;
                }

                Members.Clear();
            }
        }

        public void Expel(string name)
        {
            L2Player playerToExpel = Members.FirstOrDefault(pl => pl.Name.Equals(name));

            if (playerToExpel == null)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.FAILED_TO_EXPEL_THE_PARTY_MEMBER);
                player.SendActionFailed();
                return;
            }

            kick(playerToExpel);
        }
    }
}