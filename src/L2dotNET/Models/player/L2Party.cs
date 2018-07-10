﻿using System.Collections.Generic;
using System.Linq;
using System.Timers;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Network;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Models.Player
{

    public class L2Party
    {
        public LinkedList<L2Player> Members;
        private readonly L2Player _player;
        public int ItemDistribution;
        public L2Player Leader;

        public L2Party(L2Player player)
        {
            Members = new LinkedList<L2Player>();
            Members.AddLast(player);
            Leader = player;
            _player = player;
            ItemDistribution = player.ItemDistribution;
        }

        public double[] BonusExp = { 1, 1.30, 1.39, 1.50, 1.54, 1.58, 1.63, 1.67, 1.71 };
        public short[] LootSysstrings = { 487, 488, 798, 799, 800 };

        public byte ItemLooter = 0;
        public byte ItemRandom = 1;
        public byte ItemRandomSpoil = 2;
        public byte ItemOrder = 3;
        public byte ItemOrderSpoil = 4;

        public short VoteId = -1;

        public void AddMember(L2Player playerMember, bool onCreate = false)
        {
            Members.AddLast(playerMember);

            if (!onCreate)
                playerMember.SendPacketAsync(new PartySmallWindowAll(this));
            else
                BroadcastToMembers(new PartySmallWindowAll(this));

            playerMember.Party = this;

            SystemMessage sm = new SystemMessage(SystemMessageId.YouJoinedS1Party);
            sm.AddPlayerName(Leader.Name);
            playerMember.SendPacketAsync(sm);

            sm = new SystemMessage(SystemMessageId.S1JoinedParty);
            sm.AddPlayerName(Leader.Name);
            BroadcastToMembers(sm, playerMember.ObjectId);
        }

        public void BroadcastToMembers(GameserverPacket pk)
        {
            foreach (L2Player pl in Members)
                pl.SendPacketAsync(pk);
        }

        public void BroadcastToMembers(GameserverPacket pk, int except)
        {
            foreach (L2Player pl in Members.Where(pl => pl.ObjectId != except))
                pl.SendPacketAsync(pk);
        }

        private byte _votesOnStart,
                     _votesVoted;
        private Timer _voteTimer;
        private SortedList<int, byte> _votes;

        public void VoteForLootChange(byte mode)
        {
            VoteId = mode;
            BroadcastToMembers(new ExAskModifyPartyLooting(Leader.Name, mode));
            SystemMessage sm = new SystemMessage(SystemMessageId.RequestingApprovalForChangingPartyLootToS1);
            sm.AddSysStr(LootSysstrings[mode]);
            Leader.SendPacketAsync(sm);

            _votesOnStart = (byte)Members.Count;

            _voteTimer = new Timer
            {
                Interval = 30000,
                Enabled = true
            };
            _voteTimer.Elapsed += new ElapsedEventHandler(voteTimer_Elapsed);
            _votes = new SortedList<int, byte>(_votesOnStart);
        }

        private void voteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FinishVoting();
        }

        public void AnswerLootVote(L2Player playerMember, byte answer)
        {
            _votesVoted++;

            if (!_votes.ContainsKey(playerMember.ObjectId))
                _votes.Add(playerMember.ObjectId, answer);

            if (_votes.Count == _votesOnStart)
                FinishVoting();
        }

        private void FinishVoting()
        {
            _voteTimer.Enabled = false;

            double half = _votesOnStart * 0.5;
            byte agreed = (byte)_votes.Values.Count(vote => vote == 1);

            _votes.Clear();
            SystemMessage sm;
            if (agreed > half)
            {
                sm = new SystemMessage(SystemMessageId.PartyLootWasChangedToS1);
                sm.AddSysStr(LootSysstrings[VoteId]);
                ItemDistribution = VoteId;
            }
            else
            {
                sm = new SystemMessage(SystemMessageId.PartyLootChangeWasCancelled);
                VoteId = -1;
            }

            BroadcastToMembers(sm);
            BroadcastToMembers(new ExSetPartyLooting(VoteId));
            VoteId = -1;
        }

        public void Leave(L2Player playerMember)
        {
            if (Leader.ObjectId == playerMember.ObjectId)
            {
                if (Members.Count > 2)
                {
                    Kick(playerMember);

                    Leader = Members.First.Value;
                    SystemMessage sm = new SystemMessage(SystemMessageId.S1HasBecomeAPartyLeader);
                    sm.AddPlayerName(Leader.Name);
                    BroadcastToMembers(sm);

                    BroadcastToMembers(new PartySmallWindowDeleteAll());
                    BroadcastToMembers(new PartySmallWindowAll(this));
                }
                else
                {
                    foreach (L2Player pl in Members)
                    {
                        pl.SendSystemMessage(SystemMessageId.PartyDispersed);
                        pl.SendPacketAsync(new PartySmallWindowDeleteAll());
                        pl.Party = null;
                    }

                    Members.Clear();

                    if ((_voteTimer != null) && _voteTimer.Enabled)
                        _voteTimer.Enabled = false;
                }
            }
            else
                Kick(playerMember);
        }

        private void Kick(L2Player playerMember)
        {
            //TODO: missing messages packets?
            //(int)SystemMessageId.HAVE_BEEN_EXPELLED_FROM_PARTY
            //(int)SystemMessageId.S1_WAS_EXPELLED_FROM_PARTY

            lock (Members)
                Members.Remove(playerMember);

            if (Members.Count > 2)
            {
                playerMember.SendSystemMessage(SystemMessageId.YouLeftParty);
                playerMember.SendPacketAsync(new PartySmallWindowDeleteAll());
                playerMember.Party = null;

                SystemMessage sm = new SystemMessage(SystemMessageId.S1LeftParty);
                sm.AddPlayerName(playerMember.Name);
                BroadcastToMembers(sm);
                BroadcastToMembers(new PartySmallWindowDelete(playerMember.ObjectId, playerMember.Name));

                
            }
            else
            {
                foreach (L2Player pl in Members)
                {
                    pl.SendSystemMessage(SystemMessageId.PartyDispersed);
                    pl.SendPacketAsync(new PartySmallWindowDeleteAll());
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
                _player.SendSystemMessage(SystemMessageId.FailedToExpelThePartyMember);
                _player.SendActionFailedAsync();
                return;
            }

            Kick(playerToExpel);
        }
    }
}