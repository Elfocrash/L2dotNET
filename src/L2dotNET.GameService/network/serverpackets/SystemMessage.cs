using System.Collections.Generic;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class SystemMessage : GameserverPacket
    {
        private readonly List<object[]> _data = new List<object[]>();
        public int MessgeId;

        public SystemMessage(SystemMessageId msgId)
        {
            MessgeId = (int)msgId;
        }

        public SystemMessage AddString(string val)
        {
            _data.Add(new object[] { 0, val });
            return this;
        }

        public SystemMessage AddNumber(int val)
        {
            _data.Add(new object[] { 1, val });
            return this;
        }

        public SystemMessage AddNumber(double val)
        {
            _data.Add(new object[] { 1, (int)val });
            return this;
        }

        public SystemMessage AddNpcName(int val)
        {
            _data.Add(new object[] { 2, 1000000 + val });
            return this;
        }

        public SystemMessage AddItemName(int val)
        {
            _data.Add(new object[] { 3, val });
            return this;
        }

        public SystemMessage AddSkillName(int val, int lvl)
        {
            _data.Add(new object[] { 4, val, lvl });
            return this;
        }

        public void AddCastleName(int val)
        {
            _data.Add(new object[] { 5, val });
        }

        public void AddItemCount(int val)
        {
            _data.Add(new object[] { 6, val });
        }

        public void AddZoneName(int val, int y, int z)
        {
            _data.Add(new object[] { 7, val, y, z });
        }

        public void AddElementName(int val)
        {
            _data.Add(new object[] { 9, val });
        }

        public void AddInstanceName(int val)
        {
            _data.Add(new object[] { 10, val });
        }

        public SystemMessage AddPlayerName(string val)
        {
            _data.Add(new object[] { 12, val });
            return this;
        }

        public SystemMessage AddName(L2Object obj)
        {
            if (obj is L2Player)
                return AddPlayerName(((L2Player)obj).Name);
            if (obj is L2Npc)
                return AddNpcName(((L2Npc)obj).NpcId);
            if (obj is L2Summon)
                return AddNpcName(((L2Summon)obj).NpcId);
            if (obj is L2Item)
                return AddItemName(((L2Item)obj).Template.ItemId);

            return AddString(obj.AsString());
        }

        public void AddSysStr(int val)
        {
            _data.Add(new object[] { 13, val });
        }

        public override void Write()
        {
            WriteByte(0x64);
            WriteInt(MessgeId);
            WriteInt(_data.Count);

            foreach (object[] d in _data)
            {
                int type = (int)d[0];

                WriteInt(type);

                switch (type)
                {
                    case 0: //text
                    case 12:
                        WriteString((string)d[1]);
                        break;
                    case 1: //number
                    case 2: //npcid
                    case 3: //itemid
                    case 5:
                    case 9:
                    case 10:
                    case 13:
                        WriteInt((int)d[1]);
                        break;
                    case 4: //skillname
                        WriteInt((int)d[1]);
                        WriteInt((int)d[2]);
                        break;
                    case 6:
                        WriteLong((long)d[1]);
                        break;
                    case 7: //zone
                        WriteInt((int)d[1]);
                        WriteInt((int)d[2]);
                        WriteInt((int)d[3]);
                        break;
                }
            }
        }

        public enum SystemMessageId
        {
            ///<summary>You have been disconnected from the server.</summary>
            YouHaveBeenDisconnected = 0,
            ///<summary>The server will be coming down in $1 seconds. Please find a safe place to log out.</summary>
            TheServerWillBeComingDownInS1Seconds = 1,
            ///<summary>$s1 does not exist.</summary>
            S1DoesNotExist = 2,
            ///<summary>$s1 is not currently logged in.</summary>
            S1IsNotOnline = 3,
            ///<summary>You cannot ask yourself to apply to a clan.</summary>
            CannotInviteYourself = 4,
            ///<summary>$s1 already exists.</summary>
            S1AlreadyExists = 5,
            ///<summary>$s1 does not exist.</summary>
            S1DoesNotExist2 = 6,
            ///<summary>You are already a member of $s1.</summary>
            AlreadyMemberOfS1 = 7,
            ///<summary>You are working with another clan.</summary>
            YouAreWorkingWithAnotherClan = 8,
            ///<summary>$s1 is not a clan leader.</summary>
            S1IsNotAClanLeader = 9,
            ///<summary>$s1 is working with another clan.</summary>
            S1WorkingWithAnotherClan = 10,
            ///<summary>There are no applicants for this clan.</summary>
            NoApplicantsForThisClan = 11,
            ///<summary>The applicant information is incorrect.</summary>
            ApplicantInformationIncorrect = 12,
            ///<summary>Unable to disperse: your clan has requested to participate in a castle siege.</summary>
            CannotDissolveCauseClanWillParticipateInCastleSiege = 13,
            ///<summary>Unable to disperse: your clan owns one or more castles or hideouts.</summary>
            CannotDissolveCauseClanOwnsCastlesHideouts = 14,
            ///<summary>You are in siege.</summary>
            YouAreInSiege = 15,
            ///<summary>You are not in siege.</summary>
            YouAreNotInSiege = 16,
            ///<summary>The castle siege has begun.</summary>
            CastleSiegeHasBegun = 17,
            ///<summary>The castle siege has ended.</summary>
            CastleSiegeHasEnded = 18,
            ///<summary>There is a new Lord of the castle!.</summary>
            NewCastleLord = 19,
            ///<summary>The gate is being opened.</summary>
            GateIsOpening = 20,
            ///<summary>The gate is being destroyed.</summary>
            GateIsDestroyed = 21,
            ///<summary>Your target is out of range.</summary>
            TargetTooFar = 22,
            ///<summary>Not enough HP.</summary>
            NotEnoughHp = 23,
            ///<summary>Not enough MP.</summary>
            NotEnoughMp = 24,
            ///<summary>Rejuvenating HP.</summary>
            RejuvenatingHp = 25,
            ///<summary>Rejuvenating MP.</summary>
            RejuvenatingMp = 26,
            ///<summary>Your casting has been interrupted.</summary>
            CastingInterrupted = 27,
            ///<summary>You have obtained $s1 adena.</summary>
            YouPickedUpS1Adena = 28,
            ///<summary>You have obtained $s2 $s1.</summary>
            YouPickedUpS2S1 = 29,
            ///<summary>You have obtained $s1.</summary>
            YouPickedUpS1 = 30,
            ///<summary>You cannot move while sitting.</summary>
            CantMoveSitting = 31,
            ///<summary>You are unable to engage in combat. Please go to the nearest restart point.</summary>
            UnableCombatPleaseGoRestart = 32,
            ///<summary>You cannot move while casting.</summary>
            CantMoveCasting = 32,
            ///<summary>Welcome to the World of Lineage II.</summary>
            WelcomeToLineage = 34,
            ///<summary>You hit for $s1 damage.</summary>
            YouDidS1Dmg = 35,
            ///<summary>$s1 hit you for $s2 damage.</summary>
            S1GaveYouS2Dmg = 36,
            ///<summary>$s1 hit you for $s2 damage.</summary>
            S1GaveYouS2Dmg2 = 37,
            ///<summary>You carefully nock an arrow.</summary>
            GettingReadyToShootAnArrow = 41,
            ///<summary>You have avoided $s1's attack.</summary>
            AvoidedS1Attack = 42,
            ///<summary>You have missed.</summary>
            MissedTarget = 43,
            ///<summary>Critical hit!.</summary>
            CriticalHit = 44,
            ///<summary>You have earned $s1 experience.</summary>
            EarnedS1Experience = 45,
            ///<summary>You use $s1.</summary>
            UseS1 = 46,
            ///<summary>You begin to use a(n) $s1.</summary>
            BeginToUseS1 = 47,
            ///<summary>$s1 is not available at this time: being prepared for reuse.</summary>
            S1PreparedForReuse = 48,
            ///<summary>You have equipped your $s1.</summary>
            S1Equipped = 49,
            ///<summary>Your target cannot be found.</summary>
            TargetCantFound = 50,
            ///<summary>You cannot use this on yourself.</summary>
            CannotUseOnYourself = 51,
            ///<summary>You have earned $s1 adena.</summary>
            EarnedS1Adena = 52,
            ///<summary>You have earned $s2 $s1(s).</summary>
            EarnedS2S1S = 53,
            ///<summary>You have earned $s1.</summary>
            EarnedItemS1 = 54,
            ///<summary>You have failed to pick up $s1 adena.</summary>
            FailedToPickupS1Adena = 55,
            ///<summary>You have failed to pick up $s1.</summary>
            FailedToPickupS1 = 56,
            ///<summary>You have failed to pick up $s2 $s1(s).</summary>
            FailedToPickupS2S1S = 57,
            ///<summary>You have failed to earn $s1 adena.</summary>
            FailedToEarnS1Adena = 58,
            ///<summary>You have failed to earn $s1.</summary>
            FailedToEarnS1 = 59,
            ///<summary>You have failed to earn $s2 $s1(s).</summary>
            FailedToEarnS2S1S = 60,
            ///<summary>Nothing happened.</summary>
            NothingHappened = 61,
            ///<summary>Your $s1 has been successfully enchanted.</summary>
            S1SuccessfullyEnchanted = 62,
            ///<summary>Your +$S1 $S2 has been successfully enchanted.</summary>
            S1S2SuccessfullyEnchanted = 63,
            ///<summary>The enchantment has failed! Your $s1 has been crystallized.</summary>
            EnchantmentFailedS1Evaporated = 64,
            ///<summary>The enchantment has failed! Your +$s1 $s2 has been crystallized.</summary>
            EnchantmentFailedS1S2Evaporated = 65,
            ///<summary>$s1 is inviting you to join a party. Do you accept?.</summary>
            S1InvitedYouToParty = 66,
            ///<summary>$s1 has invited you to the join the clan, $s2. Do you wish to join?.</summary>
            S1HasInvitedYouToJoinTheClanS2 = 67,
            ///<summary>Would you like to withdraw from the $s1 clan? If you leave, you will have to wait at least a day before joining another clan.</summary>
            WouldYouLikeToWithdrawFromTheS1Clan = 68,
            ///<summary>Would you like to dismiss $s1 from the clan? If you do so, you will have to wait at least a day before accepting a new member.</summary>
            WouldYouLikeToDismissS1FromTheClan = 69,
            ///<summary>Do you wish to disperse the clan, $s1?.</summary>
            DoYouWishToDisperseTheClanS1 = 70,
            ///<summary>How many of your $s1(s) do you wish to discard?.</summary>
            HowManyS1Discard = 71,
            ///<summary>How many of your $s1(s) do you wish to move?.</summary>
            HowManyS1Move = 72,
            ///<summary>How many of your $s1(s) do you wish to destroy?.</summary>
            HowManyS1Destroy = 73,
            ///<summary>Do you wish to destroy your $s1?.</summary>
            WishDestroyS1 = 74,
            ///<summary>ID does not exist.</summary>
            IdNotExist = 75,
            ///<summary>Incorrect password.</summary>
            IncorrectPassword = 76,
            ///<summary>You cannot create another character. Please delete the existing character and try again.</summary>
            CannotCreateCharacter = 77,
            ///<summary>When you delete a character, any items in his/her possession will also be deleted. Do you really wish to delete $s1%?.</summary>
            WishDeleteS1 = 78,
            ///<summary>This name already exists.</summary>
            NamingNameAlreadyExists = 79,
            ///<summary>Names must be between 1-16 characters, excluding spaces or special characters.</summary>
            NamingCharnameUpTo16Chars = 80,
            ///<summary>Please select your race.</summary>
            PleaseSelectRace = 81,
            ///<summary>Please select your occupation.</summary>
            PleaseSelectOccupation = 82,
            ///<summary>Please select your gender.</summary>
            PleaseSelectGender = 83,
            ///<summary>You may not attack in a peaceful zone.</summary>
            CantAtkPeacezone = 84,
            ///<summary>You may not attack this target in a peaceful zone.</summary>
            TargetInPeacezone = 85,
            ///<summary>Please enter your ID.</summary>
            PleaseEnterId = 86,
            ///<summary>Please enter your password.</summary>
            PleaseEnterPassword = 87,
            ///<summary>Your protocol version is different, please restart your client and run a full check.</summary>
            WrongProtocolCheck = 88,
            ///<summary>Your protocol version is different, please continue.</summary>
            WrongProtocolContinue = 89,
            ///<summary>You are unable to connect to the server.</summary>
            UnableToConnect = 90,
            ///<summary>Please select your hairstyle.</summary>
            PleaseSelectHairstyle = 91,
            ///<summary>$s1 has worn off.</summary>
            S1HasWornOff = 92,
            ///<summary>You do not have enough SP for this.</summary>
            NotEnoughSp = 93,
            ///<summary>2004-2009 (c) Copyright NCsoft Corporation. All Rights Reserved.</summary>
            Copyright = 94,
            ///<summary>You have earned $s1 experience and $s2 SP.</summary>
            YouEarnedS1ExpAndS2Sp = 95,
            ///<summary>Your level has increased!.</summary>
            YouIncreasedYourLevel = 96,
            ///<summary>This item cannot be moved.</summary>
            CannotMoveThisItem = 97,
            ///<summary>This item cannot be discarded.</summary>
            CannotDiscardThisItem = 98,
            ///<summary>This item cannot be traded or sold.</summary>
            CannotTradeThisItem = 99,
            ///<summary>$s1 is requesting to trade. Do you wish to continue?.</summary>
            S1RequestsTrade = 100,
            ///<summary>You cannot exit while in combat.</summary>
            CantLogoutWhileFighting = 101,
            ///<summary>You cannot restart while in combat.</summary>
            CantRestartWhileFighting = 102,
            ///<summary>This ID is currently logged in.</summary>
            IdLoggedIn = 103,
            ///<summary>You may not equip items while casting or performing a skill.</summary>
            CannotUseItemWhileUsingMagic = 104,
            ///<summary>You have invited $s1 to your party.</summary>
            YouInvitedS1ToParty = 105,
            ///<summary>You have joined $s1's party.</summary>
            YouJoinedS1Party = 106,
            ///<summary>$s1 has joined the party.</summary>
            S1JoinedParty = 107,
            ///<summary>$s1 has left the party.</summary>
            S1LeftParty = 108,
            ///<summary>Invalid target.</summary>
            IncorrectTarget = 109,
            ///<summary>$s1 $s2's effect can be felt.</summary>
            YouFeelS1Effect = 110,
            ///<summary>Your shield defense has succeeded.</summary>
            ShieldDefenceSuccessfull = 111,
            ///<summary>You may no longer adjust items in the trade because the trade has been confirmed.</summary>
            NotEnoughArrows = 112,
            ///<summary>$s1 cannot be used due to unsuitable terms.</summary>
            S1CannotBeUsed = 113,
            ///<summary>You have entered the shadow of the Mother Tree.</summary>
            EnterShadowMotherTree = 114,
            ///<summary>You have left the shadow of the Mother Tree.</summary>
            ExitShadowMotherTree = 115,
            ///<summary>You have entered a peaceful zone.</summary>
            EnterPeacefulZone = 116,
            ///<summary>You have left the peaceful zone.</summary>
            ExitPeacefulZone = 117,
            ///<summary>You have requested a trade with $s1.</summary>
            RequestS1ForTrade = 118,
            ///<summary>$s1 has denied your request to trade.</summary>
            S1DeniedTradeRequest = 119,
            ///<summary>You begin trading with $s1.</summary>
            BeginTradeWithS1 = 120,
            ///<summary>$s1 has confirmed the trade.</summary>
            S1ConfirmedTrade = 121,
            ///<summary>You may no longer adjust items in the trade because the trade has been confirmed.</summary>
            CannotAdjustItemsAfterTradeConfirmed = 122,
            ///<summary>Your trade is successful.</summary>
            TradeSuccessful = 123,
            ///<summary>$s1 has cancelled the trade.</summary>
            S1CanceledTrade = 124,
            ///<summary>Do you wish to exit the game?.</summary>
            WishExitGame = 125,
            ///<summary>Do you wish to return to the character select screen?.</summary>
            WishRestartGame = 126,
            ///<summary>You have been disconnected from the server. Please login again.</summary>
            DisconnectedFromServer = 127,
            ///<summary>Your character creation has failed.</summary>
            CharacterCreationFailed = 128,
            ///<summary>Your inventory is full.</summary>
            SlotsFull = 129,
            ///<summary>Your warehouse is full.</summary>
            WarehouseFull = 130,
            ///<summary>$s1 has logged in.</summary>
            S1LoggedIn = 131,
            ///<summary>$s1 has been added to your friends list.</summary>
            S1AddedToFriends = 132,
            ///<summary>$s1 has been removed from your friends list.</summary>
            S1RemovedFromYourFriendsList = 133,
            ///<summary>Please check your friends list again.</summary>
            PleaceCheckYourFriendListAgain = 134,
            ///<summary>$s1 did not reply to your invitation. Your invitation has been cancelled.</summary>
            S1DidNotReplyToYourInvite = 135,
            ///<summary>You have not replied to $s1's invitation. The offer has been cancelled.</summary>
            YouDidNotReplyToS1Invite = 136,
            ///<summary>There are no more items in the shortcut.</summary>
            NoMoreItemsShortcut = 137,
            ///<summary>Designate shortcut.</summary>
            DesignateShortcut = 138,
            ///<summary>$s1 has resisted your $s2.</summary>
            S1ResistedYourS2 = 139,
            ///<summary>Your skill was removed due to a lack of MP.</summary>
            SkillRemovedDueLackMp = 140,
            ///<summary>Once the trade is confirmed, the item cannot be moved again.</summary>
            OnceTheTradeIsConfirmedTheItemCannotBeMovedAgain = 141,
            ///<summary>You are already trading with someone.</summary>
            AlreadyTrading = 142,
            ///<summary>$s1 is already trading with another person. Please try again later.</summary>
            S1AlreadyTrading = 143,
            ///<summary>That is the incorrect target.</summary>
            TargetIsIncorrect = 144,
            ///<summary>That player is not online.</summary>
            TargetIsNotFoundInTheGame = 145,
            ///<summary>Chatting is now permitted.</summary>
            ChattingPermitted = 146,
            ///<summary>Chatting is currently prohibited.</summary>
            ChattingProhibited = 147,
            ///<summary>You cannot use quest items.</summary>
            CannotUseQuestItems = 148,
            ///<summary>You cannot pick up or use items while trading.</summary>
            CannotPickupOrUseItemWhileTrading = 149,
            ///<summary>You cannot discard or destroy an item while trading at a private store.</summary>
            CannotDiscardOrDestroyItemWhileTrading = 150,
            ///<summary>That is too far from you to discard.</summary>
            CannotDiscardDistanceTooFar = 151,
            ///<summary>You have invited the wrong target.</summary>
            YouHaveInvitedTheWrongTarget = 152,
            ///<summary>$s1 is on another task. Please try again later.</summary>
            S1IsBusyTryLater = 153,
            ///<summary>Only the leader can give out invitations.</summary>
            OnlyLeaderCanInvite = 154,
            ///<summary>The party is full.</summary>
            PartyFull = 155,
            ///<summary>Drain was only 50 percent successful.</summary>
            DrainHalfSuccesful = 156,
            ///<summary>You resisted $s1's drain.</summary>
            ResistedS1Drain = 157,
            ///<summary>Your attack has failed.</summary>
            AttackFailed = 158,
            ///<summary>You resisted $s1's magic.</summary>
            ResistedS1Magic = 159,
            ///<summary>$s1 is a member of another party and cannot be invited.</summary>
            S1IsAlreadyInParty = 160,
            ///<summary>That player is not currently online.</summary>
            InvitedUserNotOnline = 161,
            ///<summary>Warehouse is too far.</summary>
            WarehouseTooFar = 162,
            ///<summary>You cannot destroy it because the number is incorrect.</summary>
            CannotDestroyNumberIncorrect = 163,
            ///<summary>Waiting for another reply.</summary>
            WaitingForAnotherReply = 164,
            ///<summary>You cannot add yourself to your own friend list.</summary>
            YouCannotAddYourselfToOwnFriendList = 165,
            ///<summary>Friend list is not ready yet. Please register again later.</summary>
            FriendListNotReadyYetRegisterLater = 166,
            ///<summary>$s1 is already on your friend list.</summary>
            S1AlreadyOnFriendList = 167,
            ///<summary>$s1 has sent a friend request.</summary>
            S1RequestedToBecomeFriends = 168,
            ///<summary>Accept friendship 0/1 (1 to accept, 0 to deny).</summary>
            AcceptTheFriendship = 169,
            ///<summary>The user who requested to become friends is not found in the game.</summary>
            TheUserYouRequestedIsNotInGame = 170,
            ///<summary>$s1 is not on your friend list.</summary>
            S1NotOnYourFriendsList = 171,
            ///<summary>You lack the funds needed to pay for this transaction.</summary>
            LackFundsForTransaction1 = 172,
            ///<summary>You lack the funds needed to pay for this transaction.</summary>
            LackFundsForTransaction2 = 173,
            ///<summary>That person's inventory is full.</summary>
            OtherInventoryFull = 174,
            ///<summary>That skill has been de-activated as HP was fully recovered.</summary>
            SkillDeactivatedHpFull = 175,
            ///<summary>That person is in message refusal mode.</summary>
            ThePersonIsInMessageRefusalMode = 176,
            ///<summary>Message refusal mode.</summary>
            MessageRefusalMode = 177,
            ///<summary>Message acceptance mode.</summary>
            MessageAcceptanceMode = 178,
            ///<summary>You cannot discard those items here.</summary>
            CantDiscardHere = 179,
            ///<summary>You have $s1 day(s) left until deletion. Do you wish to cancel this action?.</summary>
            S1DaysLeftCancelAction = 180,
            ///<summary>Cannot see target.</summary>
            CantSeeTarget = 181,
            ///<summary>Do you want to quit the current quest?.</summary>
            WantQuitCurrentQuest = 182,
            ///<summary>There are too many users on the server. Please try again later.</summary>
            TooManyUsers = 183,
            ///<summary>Please try again later.</summary>
            TryAgainLater = 184,
            ///<summary>You must first select a user to invite to your party.</summary>
            FirstSelectUserToInviteToParty = 185,
            ///<summary>You must first select a user to invite to your clan.</summary>
            FirstSelectUserToInviteToClan = 186,
            ///<summary>Select user to expel.</summary>
            SelectUserToExpel = 187,
            ///<summary>Please create your clan name.</summary>
            PleaseCreateClanName = 188,
            ///<summary>Your clan has been created.</summary>
            ClanCreated = 189,
            ///<summary>You have failed to create a clan.</summary>
            FailedToCreateClan = 190,
            ///<summary>Clan member $s1 has been expelled.</summary>
            ClanMemberS1Expelled = 191,
            ///<summary>You have failed to expel $s1 from the clan.</summary>
            FailedExpelS1 = 192,
            ///<summary>Clan has dispersed.</summary>
            ClanHasDispersed = 193,
            ///<summary>You have failed to disperse the clan.</summary>
            FailedToDisperseClan = 194,
            ///<summary>Entered the clan.</summary>
            EnteredTheClan = 195,
            ///<summary>$s1 declined your clan invitation.</summary>
            S1RefusedToJoinClan = 196,
            ///<summary>You have withdrawn from the clan.</summary>
            YouHaveWithdrawnFromClan = 197,
            ///<summary>You have failed to withdraw from the $s1 clan.</summary>
            FailedToWithdrawFromS1Clan = 198,
            ///<summary>You have recently been dismissed from a clan. You are not allowed to join another clan for 24-hours.</summary>
            ClanMembershipTerminated = 199,
            ///<summary>You have withdrawn from the party.</summary>
            YouLeftParty = 200,
            ///<summary>$s1 was expelled from the party.</summary>
            S1WasExpelledFromParty = 201,
            ///<summary>You have been expelled from the party.</summary>
            HaveBeenExpelledFromParty = 202,
            ///<summary>The party has dispersed.</summary>
            PartyDispersed = 203,
            ///<summary>Incorrect name. Please try again.</summary>
            IncorrectNameTryAgain = 204,
            ///<summary>Incorrect character name. Please try again.</summary>
            IncorrectCharacterNameTryAgain = 205,
            ///<summary>Please enter the name of the clan you wish to declare war on.</summary>
            EnterClanNameToDeclareWar = 206,
            ///<summary>$s2 of the clan $s1 requests declaration of war. Do you accept?.</summary>
            S2OfTheClanS1RequestsWar = 207,
            ///<summary>You are not a clan member and cannot perform this action.</summary>
            YouAreNotAClanMember = 212,
            ///<summary>Not working. Please try again later.</summary>
            NotWorkingPleaseTryAgainLater = 213,
            ///<summary>Your title has been changed.</summary>
            TitleChanged = 214,
            ///<summary>War with the $s1 clan has begun.</summary>
            WarWithTheS1ClanHasBegun = 215,
            ///<summary>War with the $s1 clan has ended.</summary>
            WarWithTheS1ClanHasEnded = 216,
            ///<summary>You have won the war over the $s1 clan!.</summary>
            YouHaveWonTheWarOverTheS1Clan = 217,
            ///<summary>You have surrendered to the $s1 clan.</summary>
            YouHaveSurrenderedToTheS1Clan = 218,
            ///<summary>Your clan leader has died. You have been defeated by the $s1 clan.</summary>
            YouWereDefeatedByS1Clan = 219,
            ///<summary>You have $s1 minutes left until the clan war ends.</summary>
            S1MinutesLeftUntilClanWarEnds = 220,
            ///<summary>The time limit for the clan war is up. War with the $s1 clan is over.</summary>
            ClanWarWithS1ClanHasEnded = 221,
            ///<summary>$s1 has joined the clan.</summary>
            S1HasJoinedClan = 222,
            ///<summary>$s1 has withdrawn from the clan.</summary>
            S1HasWithdrawnFromTheClan = 223,
            ///<summary>$s1 did not respond: Invitation to the clan has been cancelled.</summary>
            S1DidNotRespondToClanInvitation = 224,
            ///<summary>You didn't respond to $s1's invitation: joining has been cancelled.</summary>
            YouDidNotRespondToS1ClanInvitation = 225,
            ///<summary>The $s1 clan did not respond: war proclamation has been refused.</summary>
            S1ClanDidNotRespond = 226,
            ///<summary>Clan war has been refused because you did not respond to $s1 clan's war proclamation.</summary>
            ClanWarRefusedYouDidNotRespondToS1 = 227,
            ///<summary>Request to end war has been denied.</summary>
            RequestToEndWarHasBeenDenied = 228,
            ///<summary>You do not meet the criteria in order to create a clan.</summary>
            YouDoNotMeetCriteriaInOrderToCreateAClan = 229,
            ///<summary>You must wait 10 days before creating a new clan.</summary>
            YouMustWaitXxDaysBeforeCreatingANewClan = 230,
            ///<summary>After a clan member is dismissed from a clan, the clan must wait at least a day before accepting a new member.</summary>
            YouMustWaitBeforeAcceptingANewMember = 231,
            ///<summary>After leaving or having been dismissed from a clan, you must wait at least a day before joining another clan.</summary>
            YouMustWaitBeforeJoiningAnotherClan = 232,
            ///<summary>The Academy/Royal Guard/Order of Knights is full and cannot accept new members at this time.</summary>
            SubclanIsFull = 233,
            ///<summary>The target must be a clan member.</summary>
            TargetMustBeInClan = 234,
            ///<summary>You are not authorized to bestow these rights.</summary>
            NotAuthorizedToBestowRights = 235,
            ///<summary>Only the clan leader is enabled.</summary>
            OnlyTheClanLeaderIsEnabled = 236,
            ///<summary>The clan leader could not be found.</summary>
            ClanLeaderNotFound = 237,
            ///<summary>Not joined in any clan.</summary>
            NotJoinedInAnyClan = 238,
            ///<summary>The clan leader cannot withdraw.</summary>
            ClanLeaderCannotWithdraw = 239,
            ///<summary>Currently involved in clan war.</summary>
            CurrentlyInvolvedInClanWar = 240,
            ///<summary>Leader of the $s1 Clan is not logged in.</summary>
            LeaderOfS1ClanNotFound = 241,
            ///<summary>Select target.</summary>
            SelectTarget = 242,
            ///<summary>You cannot declare war on an allied clan.</summary>
            CannotDeclareWarOnAlliedClan = 243,
            ///<summary>You are not allowed to issue this challenge.</summary>
            NotAllowedToChallenge = 244,
            ///<summary>5 days has not passed since you were refused war. Do you wish to continue?.</summary>
            FiveDaysNotPassedSinceRefusedWar = 245,
            ///<summary>That clan is currently at war.</summary>
            ClanCurrentlyAtWar = 246,
            ///<summary>You have already been at war with the $s1 clan: 5 days must pass before you can challenge this clan again.</summary>
            FiveDaysMustPassBeforeChallengeS1Again = 247,
            ///<summary>You cannot proclaim war: the $s1 clan does not have enough members.</summary>
            S1ClanNotEnoughMembersForWar = 248,
            ///<summary>Do you wish to surrender to the $s1 clan?.</summary>
            WishSurrenderToS1Clan = 249,
            ///<summary>You have personally surrendered to the $s1 clan. You are no longer participating in this clan war.</summary>
            YouHavePersonallySurrenderedToTheS1Clan = 250,
            ///<summary>You cannot proclaim war: you are at war with another clan.</summary>
            AlreadyAtWarWithAnotherClan = 251,
            ///<summary>Enter the clan name to surrender to.</summary>
            EnterClanNameToSurrenderTo = 252,
            ///<summary>Enter the name of the clan you wish to end the war with.</summary>
            EnterClanNameToEndWar = 253,
            ///<summary>A clan leader cannot personally surrender.</summary>
            LeaderCantPersonallySurrender = 254,
            ///<summary>The $s1 clan has requested to end war. Do you agree?.</summary>
            S1ClanRequestedEndWar = 255,
            ///<summary>Enter title.</summary>
            EnterTitle = 256,
            ///<summary>Do you offer the $s1 clan a proposal to end the war?.</summary>
            DoYouOfferS1ClanEndWar = 257,
            ///<summary>You are not involved in a clan war.</summary>
            NotInvolvedClanWar = 258,
            ///<summary>Select clan members from list.</summary>
            SelectMembersFromList = 259,
            ///<summary>Fame level has decreased: 5 days have not passed since you were refused war.</summary>
            FiveDaysNotPassedSinceYouWereRefusedWar = 260,
            ///<summary>Clan name is invalid.</summary>
            ClanNameInvalid = 261,
            ///<summary>Clan name's length is incorrect.</summary>
            ClanNameLengthIncorrect = 262,
            ///<summary>You have already requested the dissolution of your clan.</summary>
            DissolutionInProgress = 263,
            ///<summary>You cannot dissolve a clan while engaged in a war.</summary>
            CannotDissolveWhileInWar = 264,
            ///<summary>You cannot dissolve a clan during a siege or while protecting a castle.</summary>
            CannotDissolveWhileInSiege = 265,
            ///<summary>You cannot dissolve a clan while owning a clan hall or castle.</summary>
            CannotDissolveWhileOwningClanHallOrCastle = 266,
            ///<summary>There are no requests to disperse.</summary>
            NoRequestsToDisperse = 267,
            ///<summary>That player already belongs to another clan.</summary>
            PlayerAlreadyAnotherClan = 268,
            ///<summary>You cannot dismiss yourself.</summary>
            YouCannotDismissYourself = 269,
            ///<summary>You have already surrendered.</summary>
            YouHaveAlreadySurrendered = 270,
            ///<summary>A player can only be granted a title if the clan is level 3 or above.</summary>
            ClanLvl3NeededToEndoweTitle = 271,
            ///<summary>A clan crest can only be registered when the clan's skill level is 3 or above.</summary>
            ClanLvl3NeededToSetCrest = 272,
            ///<summary>A clan war can only be declared when a clan's skill level is 3 or above.</summary>
            ClanLvl3NeededToDeclareWar = 273,
            ///<summary>Your clan's skill level has increased.</summary>
            ClanLevelIncreased = 274,
            ///<summary>Clan has failed to increase skill level.</summary>
            ClanLevelIncreaseFailed = 275,
            ///<summary>You do not have the necessary materials or prerequisites to learn this skill.</summary>
            ItemMissingToLearnSkill = 276,
            ///<summary>You have earned $s1.</summary>
            LearnedSkillS1 = 277,
            ///<summary>You do not have enough SP to learn this skill.</summary>
            NotEnoughSpToLearnSkill = 278,
            ///<summary>You do not have enough adena.</summary>
            YouNotEnoughAdena = 279,
            ///<summary>You do not have any items to sell.</summary>
            NoItemsToSell = 280,
            ///<summary>You do not have enough adena to pay the fee.</summary>
            YouNotEnoughAdenaPayFee = 281,
            ///<summary>You have not deposited any items in your warehouse.</summary>
            NoItemDepositedInWh = 282,
            ///<summary>You have entered a combat zone.</summary>
            EnteredCombatZone = 283,
            ///<summary>You have left a combat zone.</summary>
            LeftCombatZone = 284,
            ///<summary>Clan $s1 has succeeded in engraving the ruler!.</summary>
            ClanS1EngravedRuler = 285,
            ///<summary>Your base is being attacked.</summary>
            BaseUnderAttack = 286,
            ///<summary>The opposing clan has stared to engrave to monument!.</summary>
            OpponentStartedEngraving = 287,
            ///<summary>The castle gate has been broken down.</summary>
            CastleGateBrokenDown = 288,
            ///<summary>An outpost or headquarters cannot be built because at least one already exists.</summary>
            NotAnotherHeadquarters = 289,
            ///<summary>You cannot set up a base here.</summary>
            NotSetUpBaseHere = 290,
            ///<summary>Clan $s1 is victorious over $s2's castle siege!.</summary>
            ClanS1VictoriousOverS2SSiege = 291,
            ///<summary>$s1 has announced the castle siege time.</summary>
            S1AnnouncedSiegeTime = 292,
            ///<summary>The registration term for $s1 has ended.</summary>
            RegistrationTermForS1Ended = 293,
            ///<summary>Because your clan is not currently on the offensive in a Clan Hall siege war, it cannot summon its base camp.</summary>
            BecauseYourClanIsNotCurrentlyOnTheOffensiveInAClanHallSiegeWarItCannotSummonItsBaseCamp = 294,
            ///<summary>$s1's siege was canceled because there were no clans that participated.</summary>
            S1SiegeWasCanceledBecauseNoClansParticipated = 295,
            ///<summary>You received $s1 damage from taking a high fall.</summary>
            FallDamageS1 = 296,
            ///<summary>You have taken $s1 damage because you were unable to breathe.</summary>
            DrownDamageS1 = 297,
            ///<summary>You have dropped $s1.</summary>
            YouDroppedS1 = 298,
            ///<summary>$s1 has obtained $s3 $s2.</summary>
            S1ObtainedS3S2 = 299,
            ///<summary>$s1 has obtained $s2.</summary>
            S1ObtainedS2 = 300,
            ///<summary>$s2 $s1 has disappeared.</summary>
            S2S1Disappeared = 301,
            ///<summary>$s1 has disappeared.</summary>
            S1Disappeared = 302,
            ///<summary>Select item to enchant.</summary>
            SelectItemToEnchant = 303,
            ///<summary>Clan member $s1 has logged into game.</summary>
            ClanMemberS1LoggedIn = 304,
            ///<summary>The player declined to join your party.</summary>
            PlayerDeclined = 305,
            ///<summary>You have succeeded in expelling the clan member.</summary>
            YouHaveSucceededInExpellingClanMember = 309,
            ///<summary>The clan war declaration has been accepted.</summary>
            ClanWarDeclarationAccepted = 311,
            ///<summary>The clan war declaration has been refused.</summary>
            ClanWarDeclarationRefused = 312,
            ///<summary>The cease war request has been accepted.</summary>
            CeaseWarRequestAccepted = 313,
            ///<summary>You have failed to surrender.</summary>
            FailedToSurrender = 314,
            ///<summary>You have failed to personally surrender.</summary>
            FailedToPersonallySurrender = 315,
            ///<summary>You have failed to withdraw from the party.</summary>
            FailedToWithdrawFromTheParty = 316,
            ///<summary>You have failed to expel the party member.</summary>
            FailedToExpelThePartyMember = 317,
            ///<summary>You have failed to disperse the party.</summary>
            FailedToDisperseTheParty = 318,
            ///<summary>This door cannot be unlocked.</summary>
            UnableToUnlockDoor = 319,
            ///<summary>You have failed to unlock the door.</summary>
            FailedToUnlockDoor = 320,
            ///<summary>It is not locked.</summary>
            ItsNotLocked = 321,
            ///<summary>Please decide on the sales price.</summary>
            DecideSalesPrice = 322,
            ///<summary>Your force has increased to $s1 level.</summary>
            ForceIncreasedToS1 = 323,
            ///<summary>Your force has reached maximum capacity.</summary>
            ForceMaxlevelReached = 324,
            ///<summary>The corpse has already disappeared.</summary>
            CorpseAlreadyDisappeared = 325,
            ///<summary>Select target from list.</summary>
            SelectTargetFromList = 326,
            ///<summary>You cannot exceed 80 characters.</summary>
            CannotExceed80Characters = 327,
            ///<summary>Please input title using less than 128 characters.</summary>
            PleaseInputTitleLess128Characters = 328,
            ///<summary>Please input content using less than 3000 characters.</summary>
            PleaseInputContentLess3000Characters = 329,
            ///<summary>A one-line response may not exceed 128 characters.</summary>
            OneLineResponseNotExceed128Characters = 330,
            ///<summary>You have acquired $s1 SP.</summary>
            AcquiredS1Sp = 331,
            ///<summary>Do you want to be restored?.</summary>
            DoYouWantToBeRestored = 332,
            ///<summary>You have received $s1 damage by Core's barrier.</summary>
            S1DamageByCoreBarrier = 333,
            ///<summary>Please enter your private store display message.</summary>
            EnterPrivateStoreMessage = 334,
            ///<summary>$s1 has been aborted.</summary>
            S1HasBeenAborted = 335,
            ///<summary>You are attempting to crystallize $s1. Do you wish to continue?.</summary>
            WishToCrystallizeS1 = 336,
            ///<summary>The soulshot you are attempting to use does not match the grade of your equipped weapon.</summary>
            SoulshotsGradeMismatch = 337,
            ///<summary>You do not have enough soulshots for that.</summary>
            NotEnoughSoulshots = 338,
            ///<summary>Cannot use soulshots.</summary>
            CannotUseSoulshots = 339,
            ///<summary>Your private store is now open for business.</summary>
            PrivateStoreUnderWay = 340,
            ///<summary>You do not have enough materials to perform that action.</summary>
            NotEnoughMaterials = 341,
            ///<summary>Power of the spirits enabled.</summary>
            EnabledSoulshot = 342,
            ///<summary>Sweeper failed, target not spoiled.</summary>
            SweeperFailedTargetNotSpoiled = 343,
            ///<summary>Power of the spirits disabled.</summary>
            SoulshotsDisabled = 344,
            ///<summary>Chat enabled.</summary>
            ChatEnabled = 345,
            ///<summary>Chat disabled.</summary>
            ChatDisabled = 346,
            ///<summary>Incorrect item count.</summary>
            IncorrectItemCount = 347,
            ///<summary>Incorrect item price.</summary>
            IncorrectItemPrice = 348,
            ///<summary>Private store already closed.</summary>
            PrivateStoreAlreadyClosed = 349,
            ///<summary>Item out of stock.</summary>
            ItemOutOfStock = 350,
            ///<summary>Incorrect item count.</summary>
            NotEnoughItems = 351,
            ///<summary>Cancel enchant.</summary>
            CancelEnchant = 354,
            ///<summary>Inappropriate enchant conditions.</summary>
            InappropriateEnchantCondition = 355,
            ///<summary>Reject resurrection.</summary>
            RejectResurrection = 356,
            ///<summary>It has already been spoiled.</summary>
            AlreadySpoiled = 357,
            ///<summary>$s1 hour(s) until catle siege conclusion.</summary>
            S1HoursUntilSiegeConclusion = 358,
            ///<summary>$s1 minute(s) until catle siege conclusion.</summary>
            S1MinutesUntilSiegeConclusion = 359,
            ///<summary>Castle siege $s1 second(s) left!.</summary>
            CastleSiegeS1SecondsLeft = 360,
            ///<summary>Over-hit!.</summary>
            OverHit = 361,
            ///<summary>You have acquired $s1 bonus experience from a successful over-hit.</summary>
            AcquiredBonusExperienceThroughOverHit = 362,
            ///<summary>Chat available time: $s1 minute.</summary>
            ChatAvailableS1Minute = 363,
            ///<summary>Enter user's name to search.</summary>
            EnterUserNameToSearch = 364,
            ///<summary>Are you sure?.</summary>
            AreYouSure = 365,
            ///<summary>Please select your hair color.</summary>
            PleaseSelectHairColor = 366,
            ///<summary>You cannot remove that clan character at this time.</summary>
            CannotRemoveClanCharacter = 367,
            ///<summary>Equipped +$s1 $s2.</summary>
            S1S2Equipped = 368,
            ///<summary>You have obtained a +$s1 $s2.</summary>
            YouPickedUpAS1S2 = 369,
            ///<summary>Failed to pickup $s1.</summary>
            FailedPickupS1 = 370,
            ///<summary>Acquired +$s1 $s2.</summary>
            AcquiredS1S2 = 371,
            ///<summary>Failed to earn $s1.</summary>
            FailedEarnS1 = 372,
            ///<summary>You are trying to destroy +$s1 $s2. Do you wish to continue?.</summary>
            WishDestroyS1S2 = 373,
            ///<summary>You are attempting to crystallize +$s1 $s2. Do you wish to continue?.</summary>
            WishCrystallizeS1S2 = 374,
            ///<summary>You have dropped +$s1 $s2 .</summary>
            DroppedS1S2 = 375,
            ///<summary>$s1 has obtained +$s2$s3.</summary>
            S1ObtainedS2S3 = 376,
            ///<summary>$S1 $S2 disappeared.</summary>
            S1S2Disappeared = 377,
            ///<summary>$s1 purchased $s2.</summary>
            S1PurchasedS2 = 378,
            ///<summary>$s1 purchased +$s2$s3.</summary>
            S1PurchasedS2S3 = 379,
            ///<summary>$s1 purchased $s3 $s2(s).</summary>
            S1PurchasedS3S2S = 380,
            ///<summary>The game client encountered an error and was unable to connect to the petition server.</summary>
            GameClientUnableToConnectToPetitionServer = 381,
            ///<summary>Currently there are no users that have checked out a GM ID.</summary>
            NoUsersCheckedOutGmId = 382,
            ///<summary>Request confirmed to end consultation at petition server.</summary>
            RequestConfirmedToEndConsultation = 383,
            ///<summary>The client is not logged onto the game server.</summary>
            ClientNotLoggedOntoGameServer = 384,
            ///<summary>Request confirmed to begin consultation at petition server.</summary>
            RequestConfirmedToBeginConsultation = 385,
            ///<summary>The body of your petition must be more than five characters in length.</summary>
            PetitionMoreThanFiveCharacters = 386,
            ///<summary>This ends the GM petition consultation. Please take a moment to provide feedback about this service.</summary>
            ThisEndThePetitionPleaseProvideFeedback = 387,
            ///<summary>Not under petition consultation.</summary>
            NotUnderPetitionConsultation = 388,
            ///<summary>our petition application has been accepted. - Receipt No. is $s1.</summary>
            PetitionAcceptedRecentNoS1 = 389,
            ///<summary>You may only submit one petition (active) at a time.</summary>
            OnlyOneActivePetitionAtTime = 390,
            ///<summary>Receipt No. $s1, petition cancelled.</summary>
            RecentNoS1Canceled = 391,
            ///<summary>Under petition advice.</summary>
            UnderPetitionAdvice = 392,
            ///<summary>Failed to cancel petition. Please try again later.</summary>
            FailedCancelPetitionTryLater = 393,
            ///<summary>Petition consultation with $s1, under way.</summary>
            PetitionWithS1UnderWay = 394,
            ///<summary>Ending petition consultation with $s1.</summary>
            PetitionEndedWithS1 = 395,
            ///<summary>Please login after changing your temporary password.</summary>
            TryAgainAfterChangingPassword = 396,
            ///<summary>Not a paid account.</summary>
            NoPaidAccount = 397,
            ///<summary>There is no time left on this account.</summary>
            NoTimeLeftOnAccount = 398,
            ///<summary>You are attempting to drop $s1. Dou you wish to continue?.</summary>
            WishToDropS1 = 400,
            ///<summary>You have to many ongoing quests.</summary>
            TooManyQuests = 401,
            ///<summary>You do not possess the correct ticket to board the boat.</summary>
            NotCorrectBoatTicket = 402,
            ///<summary>You have exceeded your out-of-pocket adena limit.</summary>
            ExceecedPocketAdenaLimit = 403,
            ///<summary>Your Create Item level is too low to register this recipe.</summary>
            CreateLvlTooLowToRegister = 404,
            ///<summary>The total price of the product is too high.</summary>
            TotalPriceTooHigh = 405,
            ///<summary>Petition application accepted.</summary>
            PetitionAppAccepted = 406,
            ///<summary>Petition under process.</summary>
            PetitionUnderProcess = 407,
            ///<summary>Set Period.</summary>
            SetPeriod = 408,
            ///<summary>Set Time-$s1:$s2:$s3.</summary>
            SetTimeS1S2S3 = 409,
            ///<summary>Registration Period.</summary>
            RegistrationPeriod = 410,
            ///<summary>Registration Time-$s1:$s2:$s3.</summary>
            RegistrationTimeS1S2S3 = 411,
            ///<summary>Battle begins in $s1:$s2:$s3.</summary>
            BattleBeginsS1S2S3 = 412,
            ///<summary>Battle ends in $s1:$s2:$s3.</summary>
            BattleEndsS1S2S3 = 413,
            ///<summary>Standby.</summary>
            Standby = 414,
            ///<summary>Under Siege.</summary>
            UnderSiege = 415,
            ///<summary>This item cannot be exchanged.</summary>
            ItemCannotExchange = 416,
            ///<summary>$s1 has been disarmed.</summary>
            S1Disarmed = 417,
            ///<summary>$s1 minute(s) of usage time left.</summary>
            S1MinutesUsageLeft = 419,
            ///<summary>Time expired.</summary>
            TimeExpired = 420,
            ///<summary>Another person has logged in with the same account.</summary>
            AnotherLoginWithAccount = 421,
            ///<summary>You have exceeded the weight limit.</summary>
            WeightLimitExceeded = 422,
            ///<summary>You have cancelled the enchanting process.</summary>
            EnchantScrollCancelled = 423,
            ///<summary>Does not fit strengthening conditions of the scroll.</summary>
            DoesNotFitScrollConditions = 424,
            ///<summary>Your Create Item level is too low to register this recipe.</summary>
            CreateLvlTooLowToRegister2 = 425,
            ///<summary>(Reference Number Regarding Membership Withdrawal Request: $s1).</summary>
            ReferenceMembershipWithdrawalS1 = 445,
            ///<summary>.</summary>
            Dot = 447,
            ///<summary>There is a system error. Please log in again later.</summary>
            SystemErrorLoginLater = 448,
            ///<summary>The password you have entered is incorrect.</summary>
            PasswordEnteredIncorrect1 = 449,
            ///<summary>Confirm your account information and log in later.</summary>
            ConfirmAccountLoginLater = 450,
            ///<summary>The password you have entered is incorrect.</summary>
            PasswordEnteredIncorrect2 = 451,
            ///<summary>Please confirm your account information and try logging in later.</summary>
            PleaseConfirmAccountLoginLater = 452,
            ///<summary>Your account information is incorrect.</summary>
            AccountInformationIncorrect = 453,
            ///<summary>Account is already in use. Unable to log in.</summary>
            AccountInUse = 455,
            ///<summary>Lineage II game services may be used by individuals 15 years of age or older except for PvP servers,which may only be used by adults 18 years of age and older (Korea Only).</summary>
            LinageMinimumAge = 456,
            ///<summary>Currently undergoing game server maintenance. Please log in again later.</summary>
            ServerMaintenance = 457,
            ///<summary>Your usage term has expired.</summary>
            UsageTermExpired = 458,
            ///<summary>to reactivate your account.</summary>
            ToReactivateYourAccount = 460,
            ///<summary>Access failed.</summary>
            AccessFailed = 461,
            ///<summary>Please try again later.</summary>
            PleaseTryAgainLater = 461,
            ///<summary>This feature is only available alliance leaders.</summary>
            FeatureOnlyForAllianceLeader = 464,
            ///<summary>You are not currently allied with any clans.</summary>
            NoCurrentAlliances = 465,
            ///<summary>You have exceeded the limit.</summary>
            YouHaveExceededTheLimit = 466,
            ///<summary>You may not accept any clan within a day after expelling another clan.</summary>
            CantInviteClanWithin1Day = 467,
            ///<summary>A clan that has withdrawn or been expelled cannot enter into an alliance within one day of withdrawal or expulsion.</summary>
            CantEnterAllianceWithin1Day = 468,
            ///<summary>You may not ally with a clan you are currently at war with. That would be diabolical and treacherous.</summary>
            MayNotAllyClanBattle = 469,
            ///<summary>Only the clan leader may apply for withdrawal from the alliance.</summary>
            OnlyClanLeaderWithdrawAlly = 470,
            ///<summary>Alliance leaders cannot withdraw.</summary>
            AllianceLeaderCantWithdraw = 471,
            ///<summary>You cannot expel yourself from the clan.</summary>
            CannotExpelYourself = 472,
            ///<summary>Different alliance.</summary>
            DifferentAlliance = 473,
            ///<summary>That clan does not exist.</summary>
            ClanDoesntExists = 474,
            ///<summary>Different alliance.</summary>
            DifferentAlliance2 = 475,
            ///<summary>Please adjust the image size to 8x12.</summary>
            AdjustImage812 = 476,
            ///<summary>No response. Invitation to join an alliance has been cancelled.</summary>
            NoResponseToAllyInvitation = 477,
            ///<summary>No response. Your entrance to the alliance has been cancelled.</summary>
            YouDidNotRespondToAllyInvitation = 478,
            ///<summary>$s1 has joined as a friend.</summary>
            S1JoinedAsFriend = 479,
            ///<summary>Please check your friend list.</summary>
            PleaseCheckYourFriendsList = 480,
            ///<summary>$s1 has been deleted from your friends list.</summary>
            S1HasBeenDeletedFromYourFriendsList = 481,
            ///<summary>You cannot add yourself to your own friend list.</summary>
            YouCannotAddYourselfToYourOwnFriendsList = 482,
            ///<summary>This function is inaccessible right now. Please try again later.</summary>
            FunctionInaccessibleNow = 483,
            ///<summary>This player is already registered in your friends list.</summary>
            S1AlreadyInFriendsList = 484,
            ///<summary>No new friend invitations may be accepted.</summary>
            NoNewInvitationsAccepted = 485,
            ///<summary>The following user is not in your friends list.</summary>
            TheUserNotInFriendsList = 486,
            ///<summary>======Friends List======.</summary>
            FriendListHeader = 487,
            ///<summary>$s1 (Currently: Online).</summary>
            S1Online = 488,
            ///<summary>$s1 (Currently: Offline).</summary>
            S1Offline = 489,
            ///<summary>========================.</summary>
            FriendListFooter = 490,
            ///<summary>=======Alliance Information=======.</summary>
            AllianceInfoHead = 491,
            ///<summary>Alliance Name: $s1.</summary>
            AllianceNameS1 = 492,
            ///<summary>Connection: $s1 / Total $s2.</summary>
            ConnectionS1TotalS2 = 493,
            ///<summary>Alliance Leader: $s2 of $s1.</summary>
            AllianceLeaderS2OfS1 = 494,
            ///<summary>Affiliated clans: Total $s1 clan(s).</summary>
            AllianceClanTotalS1 = 495,
            ///<summary>=====Clan Information=====.</summary>
            ClanInfoHead = 496,
            ///<summary>Clan Name: $s1.</summary>
            ClanInfoNameS1 = 497,
            ///<summary>Clan Leader: $s1.</summary>
            ClanInfoLeaderS1 = 498,
            ///<summary>Clan Level: $s1.</summary>
            ClanInfoLevelS1 = 499,
            ///<summary>------------------------.</summary>
            ClanInfoSeparator = 500,
            ///<summary>========================.</summary>
            ClanInfoFoot = 501,
            ///<summary>You already belong to another alliance.</summary>
            AlreadyJoinedAlliance = 502,
            ///<summary>$s1 (Friend) has logged in.</summary>
            FriendS1HasLoggedIn = 503,
            ///<summary>Only clan leaders may create alliances.</summary>
            OnlyClanLeaderCreateAlliance = 504,
            ///<summary>You cannot create a new alliance within 10 days after dissolution.</summary>
            CantCreateAlliance10DaysDisolution = 505,
            ///<summary>Incorrect alliance name. Please try again.</summary>
            IncorrectAllianceName = 506,
            ///<summary>Incorrect length for an alliance name.</summary>
            IncorrectAllianceNameLength = 507,
            ///<summary>This alliance name already exists.</summary>
            AllianceAlreadyExists = 508,
            ///<summary>Cannot accept. clan ally is registered as an enemy during siege battle.</summary>
            CantAcceptAllyEnemyForSiege = 509,
            ///<summary>You have invited someone to your alliance.</summary>
            YouInvitedForAlliance = 510,
            ///<summary>You must first select a user to invite.</summary>
            SelectUserToInvite = 511,
            ///<summary>Do you really wish to withdraw from the alliance?.</summary>
            DoYouWishToWithdrw = 512,
            ///<summary>Enter the name of the clan you wish to expel.</summary>
            EnterNameClanToExpel = 513,
            ///<summary>Do you really wish to dissolve the alliance?.</summary>
            DoYouWishToDisolve = 514,
            ///<summary>$s1 has invited you to be their friend.</summary>
            SiInvitedYouAsFriend = 516,
            ///<summary>You have accepted the alliance.</summary>
            YouAcceptedAlliance = 517,
            ///<summary>You have failed to invite a clan into the alliance.</summary>
            FailedToInviteClanInAlliance = 518,
            ///<summary>You have withdrawn from the alliance.</summary>
            YouHaveWithdrawnFromAlliance = 519,
            ///<summary>You have failed to withdraw from the alliance.</summary>
            YouHaveFailedToWithdrawnFromAlliance = 520,
            ///<summary>You have succeeded in expelling a clan.</summary>
            YouHaveExpeledAClan = 521,
            ///<summary>You have failed to expel a clan.</summary>
            FailedToExpeledAClan = 522,
            ///<summary>The alliance has been dissolved.</summary>
            AllianceDisolved = 523,
            ///<summary>You have failed to dissolve the alliance.</summary>
            FailedToDisolveAlliance = 524,
            ///<summary>You have succeeded in inviting a friend to your friends list.</summary>
            YouHaveSucceededInvitingFriend = 525,
            ///<summary>You have failed to add a friend to your friends list.</summary>
            FailedToInviteAFriend = 526,
            ///<summary>$s1 leader, $s2, has requested an alliance.</summary>
            S2AllianceLeaderOfS1RequestedAlliance = 527,
            ///<summary>The Spiritshot does not match the weapon's grade.</summary>
            SpiritshotsGradeMismatch = 530,
            ///<summary>You do not have enough Spiritshots for that.</summary>
            NotEnoughSpiritshots = 531,
            ///<summary>You may not use Spiritshots.</summary>
            CannotUseSpiritshots = 532,
            ///<summary>Power of Mana enabled.</summary>
            EnabledSpiritshot = 533,
            ///<summary>Power of Mana disabled.</summary>
            DisabledSpiritshot = 534,
            ///<summary>How much adena do you wish to transfer to your Inventory?.</summary>
            HowMuchAdenaTransfer = 536,
            ///<summary>How much will you transfer?.</summary>
            HowMuchTransfer = 537,
            ///<summary>Your SP has decreased by $s1.</summary>
            SpDecreasedS1 = 538,
            ///<summary>Your Experience has decreased by $s1.</summary>
            ExpDecreasedByS1 = 539,
            ///<summary>Clan leaders may not be deleted. Dissolve the clan first and try again.</summary>
            ClanLeadersMayNotBeDeleted = 540,
            ///<summary>You may not delete a clan member. Withdraw from the clan first and try again.</summary>
            ClanMemberMayNotBeDeleted = 541,
            ///<summary>The NPC server is currently down. Pets and servitors cannot be summoned at this time.</summary>
            TheNpcServerIsCurrentlyDown = 542,
            ///<summary>You already have a pet.</summary>
            YouAlreadyHaveAPet = 543,
            ///<summary>Your pet cannot carry this item.</summary>
            ItemNotForPets = 544,
            ///<summary>Your pet cannot carry any more items. Remove some, then try again.</summary>
            YourPetCannotCarryAnyMoreItems = 545,
            ///<summary>Unable to place item, your pet is too encumbered.</summary>
            UnableToPlaceItemYourPetIsTooEncumbered = 546,
            ///<summary>Summoning your pet.</summary>
            SummonAPet = 547,
            ///<summary>Your pet's name can be up to 8 characters in length.</summary>
            NamingPetnameUpTo_8Chars = 548,
            ///<summary>To create an alliance, your clan must be Level 5 or higher.</summary>
            ToCreateAnAllyYouClanMustBeLevel5OrHigher = 549,
            ///<summary>You may not create an alliance during the term of dissolution postponement.</summary>
            YouMayNotCreateAllyWhileDissolving = 550,
            ///<summary>You cannot raise your clan level during the term of dispersion postponement.</summary>
            CannotRiseLevelWhileDissolutionInProgress = 551,
            ///<summary>During the grace period for dissolving a clan, the registration or deletion of a clan's crest is not allowed.</summary>
            CannotSetCrestWhileDissolutionInProgress = 552,
            ///<summary>The opposing clan has applied for dispersion.</summary>
            OpposingClanAppliedDispersion = 553,
            ///<summary>You cannot disperse the clans in your alliance.</summary>
            CannotDisperseTheClansInAlly = 554,
            ///<summary>You cannot move - you are too encumbered.</summary>
            CantMoveTooEncumbered = 555,
            ///<summary>You cannot move in this state.</summary>
            CantMoveInThisState = 556,
            ///<summary>Your pet has been summoned and may not be destroyed.</summary>
            PetSummonedMayNotDestroyed = 557,
            ///<summary>Your pet has been summoned and may not be let go.</summary>
            PetSummonedMayNotLetGo = 558,
            ///<summary>You have purchased $s2 from $s1.</summary>
            PurchasedS2FromS1 = 559,
            ///<summary>You have purchased +$s2 $s3 from $s1.</summary>
            PurchasedS2S3FromS1 = 560,
            ///<summary>You have purchased $s3 $s2(s) from $s1.</summary>
            PurchasedS3S2SFromS1 = 561,
            ///<summary>You may not crystallize this item. Your crystallization skill level is too low.</summary>
            CrystallizeLevelTooLow = 562,
            ///<summary>Failed to disable attack target.</summary>
            FailedDisableTarget = 563,
            ///<summary>Failed to change attack target.</summary>
            FailedChangeTarget = 564,
            ///<summary>Not enough luck.</summary>
            NotEnoughLuck = 565,
            ///<summary>Your confusion spell failed.</summary>
            ConfusionFailed = 566,
            ///<summary>Your fear spell failed.</summary>
            FearFailed = 567,
            ///<summary>Cubic Summoning failed.</summary>
            CubicSummoningFailed = 568,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Finders Keepers.).</summary>
            S1InvitedYouToPartyFindersKeepers = 572,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Random.).</summary>
            S1InvitedYouToPartyRandom = 573,
            ///<summary>Pets and Servitors are not available at this time.</summary>
            PetsAreNotAvailableAtThisTime = 574,
            ///<summary>How much adena do you wish to transfer to your pet?.</summary>
            HowMuchAdenaTransferToPet = 575,
            ///<summary>How much do you wish to transfer?.</summary>
            HowMuchTransfer2 = 576,
            ///<summary>You cannot summon during a trade or while using the private shops.</summary>
            CannotSummonDuringTradeShop = 577,
            ///<summary>You cannot summon during combat.</summary>
            YouCannotSummonInCombat = 578,
            ///<summary>A pet cannot be sent back during battle.</summary>
            PetCannotSentBackDuringBattle = 579,
            ///<summary>You may not use multiple pets or servitors at the same time.</summary>
            SummonOnlyOne = 580,
            ///<summary>There is a space in the name.</summary>
            NamingThereIsASpace = 581,
            ///<summary>Inappropriate character name.</summary>
            NamingInappropriateCharacterName = 582,
            ///<summary>Name includes forbidden words.</summary>
            NamingIncludesForbiddenWords = 583,
            ///<summary>This is already in use by another pet.</summary>
            NamingAlreadyInUseByAnotherPet = 584,
            ///<summary>Please decide on the price.</summary>
            DecideOnPrice = 585,
            ///<summary>Pet items cannot be registered as shortcuts.</summary>
            PetNoShortcut = 586,
            ///<summary>Your pet's inventory is full.</summary>
            PetInventoryFull = 588,
            ///<summary>A dead pet cannot be sent back.</summary>
            DeadPetCannotBeReturned = 589,
            ///<summary>Your pet is motionless and any attempt you make to give it something goes unrecognized.</summary>
            CannotGiveItemsToDeadPet = 590,
            ///<summary>An invalid character is included in the pet's name.</summary>
            NamingPetnameContainsInvalidChars = 591,
            ///<summary>Do you wish to dismiss your pet? Dismissing your pet will cause the pet necklace to disappear.</summary>
            WishToDismissPet = 592,
            ///<summary>Starving, grumpy and fed up, your pet has left.</summary>
            StarvingGrumpyAndFedUpYourPetHasLeft = 593,
            ///<summary>You may not restore a hungry pet.</summary>
            YouCannotRestoreHungryPets = 594,
            ///<summary>Your pet is very hungry.</summary>
            YourPetIsVeryHungry = 595,
            ///<summary>Your pet ate a little, but is still hungry.</summary>
            YourPetAteALittleButIsStillHungry = 596,
            ///<summary>Your pet is very hungry. Please be careful.</summary>
            YourPetIsVeryHungryPleaseBeCareful = 597,
            ///<summary>You may not chat while you are invisible.</summary>
            NotChatWhileInvisible = 598,
            ///<summary>The GM has an important notice. Chat has been temporarily disabled.</summary>
            GmNoticeChatDisabled = 599,
            ///<summary>You may not equip a pet item.</summary>
            CannotEquipPetItem = 600,
            ///<summary>There are $S1 petitions currently on the waiting list.</summary>
            S1PetitionOnWaitingList = 601,
            ///<summary>The petition system is currently unavailable. Please try again later.</summary>
            PetitionSystemCurrentUnavailable = 602,
            ///<summary>That item cannot be discarded or exchanged.</summary>
            CannotDiscardExchangeItem = 603,
            ///<summary>You may not call forth a pet or summoned creature from this location.</summary>
            NotCallPetFromThisLocation = 604,
            ///<summary>You may register up to 64 people on your list.</summary>
            MayRegisterUpTo64People = 605,
            ///<summary>You cannot be registered because the other person has already registered 64 people on his/her list.</summary>
            OtherPersonAlready64People = 606,
            ///<summary>You do not have any further skills to learn. Come back when you have reached Level $s1.</summary>
            DoNotHaveFurtherSkillsToLearnS1 = 607,
            ///<summary>$s1 has obtained $s3 $s2 by using Sweeper.</summary>
            S1SweepedUpS3S2 = 608,
            ///<summary>$s1 has obtained $s2 by using Sweeper.</summary>
            S1SweepedUpS2 = 609,
            ///<summary>Your skill has been canceled due to lack of HP.</summary>
            SkillRemovedDueLackHp = 610,
            ///<summary>You have succeeded in Confusing the enemy.</summary>
            ConfusingSucceeded = 611,
            ///<summary>The Spoil condition has been activated.</summary>
            SpoilSuccess = 612,
            ///<summary>======Ignore List======.</summary>
            BlockListHeader = 613,
            ///<summary>$s1 : $s2.</summary>
            S1S2 = 614,
            ///<summary>You have failed to register the user to your Ignore List.</summary>
            FailedToRegisterToIgnoreList = 615,
            ///<summary>You have failed to delete the character.</summary>
            FailedToDeleteCharacter = 616,
            ///<summary>$s1 has been added to your Ignore List.</summary>
            S1WasAddedToYourIgnoreList = 617,
            ///<summary>$s1 has been removed from your Ignore List.</summary>
            S1WasRemovedFromYourIgnoreList = 618,
            ///<summary>$s1 has placed you on his/her Ignore List.</summary>
            S1HasAddedYouToIgnoreList = 619,
            ///<summary>$s1 has placed you on his/her Ignore List.</summary>
            S1HasAddedYouToIgnoreList2 = 620,
            ///<summary>Game connection attempted through a restricted IP.</summary>
            ConnectionRestrictedIp = 621,
            ///<summary>You may not make a declaration of war during an alliance battle.</summary>
            NoWarDuringAllyBattle = 622,
            ///<summary>Your opponent has exceeded the number of simultaneous alliance battles alllowed.</summary>
            OpponentTooMuchAllyBattles1 = 623,
            ///<summary>$s1 Clan leader is not currently connected to the game server.</summary>
            S1LeaderNotConnected = 624,
            ///<summary>Your request for Alliance Battle truce has been denied.</summary>
            AllyBattleTruceDenied = 625,
            ///<summary>The $s1 clan did not respond: war proclamation has been refused.</summary>
            WarProclamationHasBeenRefused = 626,
            ///<summary>Clan battle has been refused because you did not respond to $s1 clan's war proclamation.</summary>
            YouRefusedClanWarProclamation = 627,
            ///<summary>You have already been at war with the $s1 clan: 5 days must pass before you can declare war again.</summary>
            AlreadyAtWarWithS1Wait5Days = 628,
            ///<summary>Your opponent has exceeded the number of simultaneous alliance battles alllowed.</summary>
            OpponentTooMuchAllyBattles2 = 629,
            ///<summary>War with the clan has begun.</summary>
            WarWithClanBegun = 630,
            ///<summary>War with the clan is over.</summary>
            WarWithClanEnded = 631,
            ///<summary>You have won the war over the clan!.</summary>
            WonWarOverClan = 632,
            ///<summary>You have surrendered to the clan.</summary>
            SurrenderedToClan = 633,
            ///<summary>Your alliance leader has been slain. You have been defeated by the clan.</summary>
            DefeatedByClan = 634,
            ///<summary>The time limit for the clan war has been exceeded. War with the clan is over.</summary>
            TimeUpWarOver = 635,
            ///<summary>You are not involved in a clan war.</summary>
            NotInvolvedInWar = 636,
            ///<summary>A clan ally has registered itself to the opponent.</summary>
            AllyRegisteredSelfToOpponent = 637,
            ///<summary>You have already requested a Siege Battle.</summary>
            AlreadyRequestedSiegeBattle = 638,
            ///<summary>Your application has been denied because you have already submitted a request for another Siege Battle.</summary>
            ApplicationDeniedBecauseAlreadySubmittedARequestForAnotherSiegeBattle = 639,
            ///<summary>You are already registered to the attacker side and must not cancel your registration before submitting your request.</summary>
            AlreadyAttackerNotCancel = 642,
            ///<summary>You are already registered to the defender side and must not cancel your registration before submitting your request.</summary>
            AlreadyDefenderNotCancel = 643,
            ///<summary>You are not yet registered for the castle siege.</summary>
            NotRegisteredForSiege = 644,
            ///<summary>Only clans of level 4 or higher may register for a castle siege.</summary>
            OnlyClanLevel4AboveMaySiege = 645,
            ///<summary>No more registrations may be accepted for the attacker side.</summary>
            AttackerSideFull = 648,
            ///<summary>No more registrations may be accepted for the defender side.</summary>
            DefenderSideFull = 649,
            ///<summary>You may not summon from your current location.</summary>
            YouMayNotSummonFromYourCurrentLocation = 650,
            ///<summary>Place $s1 in the current location and direction. Do you wish to continue?.</summary>
            PlaceS1InCurrentLocationAndDirection = 651,
            ///<summary>The target of the summoned monster is wrong.</summary>
            TargetOfSummonWrong = 652,
            ///<summary>You do not have the authority to position mercenaries.</summary>
            YouDoNotHaveAuthorityToPositionMercenaries = 653,
            ///<summary>You do not have the authority to cancel mercenary positioning.</summary>
            YouDoNotHaveAuthorityToCancelMercenaryPositioning = 654,
            ///<summary>Mercenaries cannot be positioned here.</summary>
            MercenariesCannotBePositionedHere = 655,
            ///<summary>This mercenary cannot be positioned anymore.</summary>
            ThisMercenaryCannotBePositionedAnymore = 656,
            ///<summary>Positioning cannot be done here because the distance between mercenaries is too short.</summary>
            PositioningCannotBeDoneBecauseDistanceBetweenMercenariesTooShort = 657,
            ///<summary>This is not a mercenary of a castle that you own and so you cannot cancel its positioning.</summary>
            ThisIsNotAMercenaryOfACastleThatYouOwnAndSoCannotCancelPositioning = 658,
            ///<summary>This is not the time for siege registration and so registrations cannot be accepted or rejected.</summary>
            NotSiegeRegistrationTime1 = 659,
            ///<summary>This is not the time for siege registration and so registration and cancellation cannot be done.</summary>
            NotSiegeRegistrationTime2 = 659,
            ///<summary>This character cannot be spoiled.</summary>
            SpoilCannotUse = 661,
            ///<summary>The other player is rejecting friend invitations.</summary>
            ThePlayerIsRejectingFriendInvitations = 662,
            ///<summary>Please choose a person to receive.</summary>
            ChoosePersonToReceive = 664,
            ///<summary>of alliance is applying for alliance war. Do you want to accept the challenge?.</summary>
            ApplyingAllianceWar = 665,
            ///<summary>A request for ceasefire has been received from alliance. Do you agree?.</summary>
            RequestForCeasefire = 666,
            ///<summary>You are registering on the attacking side of the siege. Do you want to continue?.</summary>
            RegisteringOnAttackingSide = 667,
            ///<summary>You are registering on the defending side of the siege. Do you want to continue?.</summary>
            RegisteringOnDefendingSide = 668,
            ///<summary>You are canceling your application to participate in the siege battle. Do you want to continue?.</summary>
            CancelingRegistration = 669,
            ///<summary>You are refusing the registration of clan on the defending side. Do you want to continue?.</summary>
            RefusingRegistration = 670,
            ///<summary>You are agreeing to the registration of clan on the defending side. Do you want to continue?.</summary>
            AgreeingRegistration = 671,
            ///<summary>$s1 adena disappeared.</summary>
            S1DisappearedAdena = 672,
            ///<summary>Only a clan leader whose clan is of level 2 or higher is allowed to participate in a clan hall auction.</summary>
            AuctionOnlyClanLevel2Higher = 673,
            ///<summary>I has not yet been seven days since canceling an auction.</summary>
            NotSevenDaysSinceCancelingAuction = 674,
            ///<summary>There are no clan halls up for auction.</summary>
            NoClanHallsUpForAuction = 675,
            ///<summary>Since you have already submitted a bid, you are not allowed to participate in another auction at this time.</summary>
            AlreadySubmittedBid = 676,
            ///<summary>Your bid price must be higher than the minimum price that can be bid.</summary>
            BidPriceMustBeHigher = 677,
            ///<summary>You have submitted a bid for the auction of $s1.</summary>
            SubmittedABid = 678,
            ///<summary>You have canceled your bid.</summary>
            CanceledBid = 679,
            ///<summary>You cannot participate in an auction.</summary>
            CannotParticipateInAuction = 680,
            ///<summary>The clan does not own a clan hall.</summary>
            // CLAN_HAS_NO_CLAN_HALL(681) // Doesn't exist in Hellbound anymore = 681,
            ///<summary>There are no priority rights on a sweeper.</summary>
            SweepNotAllowed = 683,
            ///<summary>You cannot position mercenaries during a siege.</summary>
            CannotPositionMercsDuringSiege = 684,
            ///<summary>You cannot apply for clan war with a clan that belongs to the same alliance.</summary>
            CannotDeclareWarOnAlly = 685,
            ///<summary>You have received $s1 damage from the fire of magic.</summary>
            S1DamageFromFireMagic = 686,
            ///<summary>You cannot move while frozen. Please wait.</summary>
            CannotMoveFrozen = 687,
            ///<summary>The clan that owns the castle is automatically registered on the defending side.</summary>
            ClanThatOwnsCastleIsAutomaticallyRegisteredDefending = 688,
            ///<summary>A clan that owns a castle cannot participate in another siege.</summary>
            ClanThatOwnsCastleCannotParticipateOtherSiege = 689,
            ///<summary>You cannot register on the attacking side because you are part of an alliance with the clan that owns the castle.</summary>
            CannotAttackAllianceCastle = 690,
            ///<summary>$s1 clan is already a member of $s2 alliance.</summary>
            S1ClanAlreadyMemberOfS2Alliance = 691,
            ///<summary>The other party is frozen. Please wait a moment.</summary>
            OtherPartyIsFrozen = 692,
            ///<summary>The package that arrived is in another warehouse.</summary>
            PackageInAnotherWarehouse = 693,
            ///<summary>No packages have arrived.</summary>
            NoPackagesArrived = 694,
            ///<summary>You cannot set the name of the pet.</summary>
            NamingYouCannotSetNameOfThePet = 695,
            ///<summary>The item enchant value is strange.</summary>
            ItemEnchantValueStrange = 697,
            ///<summary>The price is different than the same item on the sales list.</summary>
            PriceDifferentFromSalesList = 698,
            ///<summary>Currently not purchasing.</summary>
            CurrentlyNotPurchasing = 699,
            ///<summary>The purchase is complete.</summary>
            ThePurchaseIsComplete = 700,
            ///<summary>You do not have enough required items.</summary>
            NotEnoughRequiredItems = 701,
            ///<summary>There are no GMs currently visible in the public list as they may be performing other functions at the moment.</summary>
            NoGmProvidingServiceNow = 702,
            ///<summary>======GM List======.</summary>
            GmList = 703,
            ///<summary>GM : $s1.</summary>
            GmS1 = 704,
            ///<summary>You cannot exclude yourself.</summary>
            CannotExcludeSelf = 705,
            ///<summary>You can only register up to 64 names on your exclude list.</summary>
            Only64NamesOnExcludeList = 706,
            ///<summary>You cannot teleport to a village that is in a siege.</summary>
            NoPortThatIsInSige = 707,
            ///<summary>You do not have the right to use the castle warehouse.</summary>
            YouDoNotHaveTheRightToUseCastleWarehouse = 708,
            ///<summary>You do not have the right to use the clan warehouse.</summary>
            YouDoNotHaveTheRightToUseClanWarehouse = 709,
            ///<summary>Only clans of clan level 1 or higher can use a clan warehouse.</summary>
            OnlyLevel1ClanOrHigherCanUseWarehouse = 710,
            ///<summary>The siege of $s1 has started.</summary>
            SiegeOfS1HasStarted = 711,
            ///<summary>The siege of $s1 has finished.</summary>
            SiegeOfS1HasEnded = 712,
            ///<summary>$s1/$s2/$s3 :.</summary>
            S1S2S3D = 713,
            ///<summary>A trap device has been tripped.</summary>
            ATrapDeviceHasBeenTripped = 714,
            ///<summary>A trap device has been stopped.</summary>
            ATrapDeviceHasBeenStopped = 715,
            ///<summary>If a base camp does not exist, resurrection is not possible.</summary>
            NoResurrectionWithoutBaseCamp = 716,
            ///<summary>The guardian tower has been destroyed and resurrection is not possible.</summary>
            TowerDestroyedNoResurrection = 717,
            ///<summary>The castle gates cannot be opened and closed during a siege.</summary>
            GatesNotOpenedClosedDuringSiege = 718,
            ///<summary>You failed at mixing the item.</summary>
            ItemMixingFailed = 719,
            ///<summary>The purchase price is higher than the amount of money that you have and so you cannot open a personal store.</summary>
            ThePurchasePriceIsHigherThanMoney = 720,
            ///<summary>You cannot create an alliance while participating in a siege.</summary>
            NoAllyCreationWhileSiege = 721,
            ///<summary>You cannot dissolve an alliance while an affiliated clan is participating in a siege battle.</summary>
            CannotDissolveAllyWhileInSiege = 722,
            ///<summary>The opposing clan is participating in a siege battle.</summary>
            OpposingClanIsParticipatingInSiege = 723,
            ///<summary>You cannot leave while participating in a siege battle.</summary>
            CannotLeaveWhileSiege = 724,
            ///<summary>You cannot banish a clan from an alliance while the clan is participating in a siege.</summary>
            CannotDismissWhileSiege = 725,
            ///<summary>Frozen condition has started. Please wait a moment.</summary>
            FrozenConditionStarted = 726,
            ///<summary>The frozen condition was removed.</summary>
            FrozenConditionRemoved = 727,
            ///<summary>You cannot apply for dissolution again within seven days after a previous application for dissolution.</summary>
            CannotApplyDissolutionAgain = 728,
            ///<summary>That item cannot be discarded.</summary>
            ItemNotDiscarded = 729,
            ///<summary>- You have submitted your $s1th petition. - You may submit $s2 more petition(s) today.</summary>
            SubmittedYouS1ThPetitionS2Left = 730,
            ///<summary>A petition has been received by the GM on behalf of $s1. The petition code is $s2.</summary>
            PetitionS1ReceivedCodeIsS2 = 731,
            ///<summary>$s1 has received a request for a consultation with the GM.</summary>
            S1ReceivedConsultationRequest = 732,
            ///<summary>We have received $s1 petitions from you today and that is the maximum that you can submit in one day. You cannot submit any more petitions.</summary>
            WeHaveReceivedS1PetitionsToday = 733,
            ///<summary>You have failed at submitting a petition on behalf of someone else. $s1 already submitted a petition.</summary>
            PetitionFailedS1AlreadySubmitted = 734,
            ///<summary>You have failed at submitting a petition on behalf of $s1. The error number is $s2.</summary>
            PetitionFailedForS1ErrorNumberS2 = 735,
            ///<summary>The petition was canceled. You may submit $s1 more petition(s) today.</summary>
            PetitionCanceledSubmitS1MoreToday = 736,
            ///<summary>You have cancelled submitting a petition on behalf of $s1.</summary>
            CanceledPetitionOnS1 = 737,
            ///<summary>You have not submitted a petition.</summary>
            PetitionNotSubmitted = 738,
            ///<summary>You have failed at cancelling a petition on behalf of $s1. The error number is $s2.</summary>
            PetitionCancelFailedForS1ErrorNumberS2 = 739,
            ///<summary>$s1 participated in a petition chat at the request of the GM.</summary>
            S1ParticipatePetition = 740,
            ///<summary>You have failed at adding $s1 to the petition chat. Petition has already been submitted.</summary>
            FailedAddingS1ToPetition = 741,
            ///<summary>You have failed at adding $s1 to the petition chat. The error code is $s2.</summary>
            PetitionAddingS1FailedErrorNumberS2 = 742,
            ///<summary>$s1 left the petition chat.</summary>
            S1LeftPetitionChat = 743,
            ///<summary>You have failed at removing $s1 from the petition chat. The error code is $s2.</summary>
            PetitionRemovingS1FailedErrorNumberS2 = 744,
            ///<summary>You are currently not in a petition chat.</summary>
            YouAreNotInPetitionChat = 745,
            ///<summary>It is not currently a petition.</summary>
            CurrentlyNoPetition = 746,
            ///<summary>The distance is too far and so the casting has been stopped.</summary>
            DistTooFarCastingStopped = 748,
            ///<summary>The effect of $s1 has been removed.</summary>
            EffectS1Disappeared = 749,
            ///<summary>There are no other skills to learn.</summary>
            NoMoreSkillsToLearn = 750,
            ///<summary>As there is a conflict in the siege relationship with a clan in the alliance, you cannot invite that clan to the alliance.</summary>
            CannotInviteConflictClan = 751,
            ///<summary>That name cannot be used.</summary>
            CannotUseName = 752,
            ///<summary>You cannot position mercenaries here.</summary>
            NoMercsHere = 753,
            ///<summary>There are $s1 hours and $s2 minutes left in this week's usage time.</summary>
            S1HoursS2MinutesLeftThisWeek = 754,
            ///<summary>There are $s1 minutes left in this week's usage time.</summary>
            S1MinutesLeftThisWeek = 755,
            ///<summary>This week's usage time has finished.</summary>
            WeeksUsageTimeFinished = 756,
            ///<summary>There are $s1 hours and $s2 minutes left in the fixed use time.</summary>
            S1HoursS2MinutesLeftInTime = 757,
            ///<summary>There are $s1 hours and $s2 minutes left in this week's play time.</summary>
            S1HoursS2MinutesLeftThisWeeksPlayTime = 758,
            ///<summary>There are $s1 minutes left in this week's play time.</summary>
            S1MinutesLeftThisWeeksPlayTime = 759,
            ///<summary>$s1 cannot join the clan because one day has not yet passed since he/she left another clan.</summary>
            S1MustWaitBeforeJoiningAnotherClan = 760,
            ///<summary>$s1 clan cannot join the alliance because one day has not yet passed since it left another alliance.</summary>
            S1CantEnterAllianceWithin1Day = 761,
            ///<summary>$s1 rolled $s2 and $s3's eye came out.</summary>
            S1RolledS2S3EyeCameOut = 762,
            ///<summary>You failed at sending the package because you are too far from the warehouse.</summary>
            FailedSendingPackageTooFar = 763,
            ///<summary>You have been playing for an extended period of time. Please consider taking a break.</summary>
            PlayingForLongTime = 764,
            ///<summary>A hacking tool has been discovered. Please try again after closing unnecessary programs.</summary>
            HackingTool = 769,
            ///<summary>Play time is no longer accumulating.</summary>
            PlayTimeNoLongerAccumulating = 774,
            ///<summary>From here on, play time will be expended.</summary>
            PlayTimeExpended = 775,
            ///<summary>The clan hall which was put up for auction has been awarded to clan s1.</summary>
            ClanhallAwardedToClanS1 = 776,
            ///<summary>The clan hall which was put up for auction was not sold and therefore has been re-listed.</summary>
            ClanhallNotSold = 777,
            ///<summary>You may not log out from this location.</summary>
            NoLogoutHere = 778,
            ///<summary>You may not restart in this location.</summary>
            NoRestartHere = 779,
            ///<summary>Observation is only possible during a siege.</summary>
            OnlyViewSiege = 780,
            ///<summary>Observers cannot participate.</summary>
            ObserversCannotParticipate = 781,
            ///<summary>You may not observe a siege with a pet or servitor summoned.</summary>
            NoObserveWithPet = 782,
            ///<summary>Lottery ticket sales have been temporarily suspended.</summary>
            LotteryTicketSalesTempSuspended = 783,
            ///<summary>Tickets for the current lottery are no longer available.</summary>
            NoLotteryTicketsAvailable = 784,
            ///<summary>The results of lottery number $s1 have not yet been published.</summary>
            LotteryS1ResultNotPublished = 785,
            ///<summary>Incorrect syntax.</summary>
            IncorrectSyntax = 786,
            ///<summary>The tryouts are finished.</summary>
            ClanhallSiegeTryoutsFinished = 787,
            ///<summary>The finals are finished.</summary>
            ClanhallSiegeFinalsFinished = 788,
            ///<summary>The tryouts have begun.</summary>
            ClanhallSiegeTryoutsBegun = 789,
            ///<summary>The finals are finished.</summary>
            ClanhallSiegeFinalsBegun = 790,
            ///<summary>The final match is about to begin. Line up!.</summary>
            FinalMatchBegin = 791,
            ///<summary>The siege of the clan hall is finished.</summary>
            ClanhallSiegeEnded = 792,
            ///<summary>The siege of the clan hall has begun.</summary>
            ClanhallSiegeBegun = 793,
            ///<summary>You are not authorized to do that.</summary>
            YouAreNotAuthorizedToDoThat = 794,
            ///<summary>Only clan leaders are authorized to set rights.</summary>
            OnlyLeadersCanSetRights = 795,
            ///<summary>Your remaining observation time is minutes.</summary>
            RemainingObservationTime = 796,
            ///<summary>You may create up to 24 macros.</summary>
            YouMayCreateUpTo24Macros = 797,
            ///<summary>Item registration is irreversible. Do you wish to continue?.</summary>
            ItemRegistrationIrreversible = 798,
            ///<summary>The observation time has expired.</summary>
            ObservationTimeExpired = 799,
            ///<summary>You are too late. The registration period is over.</summary>
            RegistrationPeriodOver = 800,
            ///<summary>Registration for the clan hall siege is closed.</summary>
            RegistrationClosed = 801,
            ///<summary>Petitions are not being accepted at this time. You may submit your petition after a.m./p.m.</summary>
            PetitionNotAcceptedNow = 802,
            ///<summary>Enter the specifics of your petition.</summary>
            PetitionNotSpecified = 803,
            ///<summary>Select a type.</summary>
            SelectType = 804,
            ///<summary>Petitions are not being accepted at this time. You may submit your petition after $s1 a.m./p.m.</summary>
            PetitionNotAcceptedSubmitAtS1 = 805,
            ///<summary>If you are trapped, try typing "/unstuck".</summary>
            TryUnstuckWhenTrapped = 806,
            ///<summary>This terrain is navigable. Prepare for transport to the nearest village.</summary>
            StuckPrepareForTransport = 807,
            ///<summary>You are stuck. You may submit a petition by typing "/gm".</summary>
            StuckSubmitPetition = 808,
            ///<summary>You are stuck. You will be transported to the nearest village in five minutes.</summary>
            StuckTransportInFiveMinutes = 809,
            ///<summary>Invalid macro. Refer to the Help file for instructions.</summary>
            InvalidMacro = 810,
            ///<summary>You will be moved to (). Do you wish to continue?.</summary>
            WillBeMoved = 811,
            ///<summary>The secret trap has inflicted $s1 damage on you.</summary>
            TrapDidS1Damage = 812,
            ///<summary>You have been poisoned by a Secret Trap.</summary>
            PoisonedByTrap = 813,
            ///<summary>Your speed has been decreased by a Secret Trap.</summary>
            SlowedByTrap = 814,
            ///<summary>The tryouts are about to begin. Line up!.</summary>
            TryoutsAboutToBegin = 815,
            ///<summary>Tickets are now available for Monster Race $s1!.</summary>
            MonsraceTicketsAvailableForS1Race = 816,
            ///<summary>Now selling tickets for Monster Race $s1!.</summary>
            MonsraceTicketsNowAvailableForS1Race = 817,
            ///<summary>Ticket sales for the Monster Race will end in $s1 minute(s).</summary>
            MonsraceTicketsStopInS1Minutes = 818,
            ///<summary>Tickets sales are closed for Monster Race $s1. Odds are posted.</summary>
            MonsraceS1TicketSalesClosed = 819,
            ///<summary>Monster Race $s2 will begin in $s1 minute(s)!.</summary>
            MonsraceS2BeginsInS1Minutes = 820,
            ///<summary>Monster Race $s1 will begin in 30 seconds!.</summary>
            MonsraceS1BeginsIn30Seconds = 821,
            ///<summary>Monster Race $s1 is about to begin! Countdown in five seconds!.</summary>
            MonsraceS1CountdownInFiveSeconds = 822,
            ///<summary>The race will begin in $s1 second(s)!.</summary>
            MonsraceBeginsInS1Seconds = 823,
            ///<summary>They're off!.</summary>
            MonsraceRaceStart = 824,
            ///<summary>Monster Race $s1 is finished!.</summary>
            MonsraceS1RaceEnd = 825,
            ///<summary>First prize goes to the player in lane $s1. Second prize goes to the player in lane $s2.</summary>
            MonsraceFirstPlaceS1SecondS2 = 826,
            ///<summary>You may not impose a block on a GM.</summary>
            YouMayNotImposeABlockOnGm = 827,
            ///<summary>Are you sure you wish to delete the $s1 macro?.</summary>
            WishToDeleteS1Macro = 828,
            ///<summary>You cannot recommend yourself.</summary>
            YouCannotRecommendYourself = 829,
            ///<summary>You have recommended $s1. You have $s2 recommendations left.</summary>
            YouHaveRecommendedS1YouHaveS2RecommendationsLeft = 830,
            ///<summary>You have been recommended by $s1.</summary>
            YouHaveBeenRecommendedByS1 = 831,
            ///<summary>That character has already been recommended.</summary>
            ThatCharacterIsRecommended = 832,
            ///<summary>You are not authorized to make further recommendations at this time. You will receive more recommendation credits each day at 1 p.m.</summary>
            NoMoreRecommendationsToHave = 833,
            ///<summary>$s1 has rolled $s2.</summary>
            S1RolledS2 = 834,
            ///<summary>You may not throw the dice at this time. Try again later.</summary>
            YouMayNotThrowTheDiceAtThisTimeTryAgainLater = 835,
            ///<summary>You have exceeded your inventory volume limit and cannot take this item.</summary>
            YouHaveExceededYourInventoryVolumeLimitAndCannotTakeThisItem = 836,
            ///<summary>Macro descriptions may contain up to 32 characters.</summary>
            MacroDescriptionMax32Chars = 837,
            ///<summary>Enter the name of the macro.</summary>
            EnterTheMacroName = 838,
            ///<summary>That name is already assigned to another macro.</summary>
            MacroNameAlreadyUsed = 839,
            ///<summary>That recipe is already registered.</summary>
            RecipeAlreadyRegistered = 840,
            ///<summary>No further recipes may be registered.</summary>
            NoFutherRecipesCanBeAdded = 841,
            ///<summary>You are not authorized to register a recipe.</summary>
            NotAuthorizedRegisterRecipe = 842,
            ///<summary>The siege of $s1 is finished.</summary>
            SiegeOfS1Finished = 843,
            ///<summary>The siege to conquer $s1 has begun.</summary>
            SiegeOfS1Begun = 844,
            ///<summary>The deadlineto register for the siege of $s1 has passed.</summary>
            DeadlineForSiegeS1Passed = 845,
            ///<summary>The siege of $s1 has been canceled due to lack of interest.</summary>
            SiegeOfS1HasBeenCanceledDueToLackOfInterest = 846,
            ///<summary>A clan that owns a clan hall may not participate in a clan hall siege.</summary>
            ClanOwningClanhallMayNotSiegeClanhall = 847,
            ///<summary>$s1 has been deleted.</summary>
            S1HasBeenDeleted = 848,
            ///<summary>$s1 cannot be found.</summary>
            S1NotFound = 849,
            ///<summary>$s1 already exists.</summary>
            S1AlreadyExists2 = 850,
            ///<summary>$s1 has been added.</summary>
            S1Added = 851,
            ///<summary>The recipe is incorrect.</summary>
            RecipeIncorrect = 852,
            ///<summary>You may not alter your recipe book while engaged in manufacturing.</summary>
            CantAlterRecipebookWhileCrafting = 853,
            ///<summary>You are missing $s2 $s1 required to create that.</summary>
            MissingS2S1ToCreate = 854,
            ///<summary>$s1 clan has defeated $s2.</summary>
            S1ClanDefeatedS2 = 855,
            ///<summary>The siege of $s1 has ended in a draw.</summary>
            SiegeS1Draw = 856,
            ///<summary>$s1 clan has won in the preliminary match of $s2.</summary>
            S1ClanWonMatchS2 = 857,
            ///<summary>The preliminary match of $s1 has ended in a draw.</summary>
            MatchOfS1Draw = 858,
            ///<summary>Please register a recipe.</summary>
            PleaseRegisterRecipe = 859,
            ///<summary>You may not buld your headquarters in close proximity to another headquarters.</summary>
            HeadquartersTooClose = 860,
            ///<summary>You have exceeded the maximum number of memos.</summary>
            TooManyMemos = 861,
            ///<summary>Odds are not posted until ticket sales have closed.</summary>
            OddsNotPosted = 862,
            ///<summary>You feel the energy of fire.</summary>
            FeelEnergyFire = 863,
            ///<summary>You feel the energy of water.</summary>
            FeelEnergyWater = 864,
            ///<summary>You feel the energy of wind.</summary>
            FeelEnergyWind = 865,
            ///<summary>You may no longer gather energy.</summary>
            NoLongerEnergy = 866,
            ///<summary>The energy is depleted.</summary>
            EnergyDepleted = 867,
            ///<summary>The energy of fire has been delivered.</summary>
            EnergyFireDelivered = 868,
            ///<summary>The energy of water has been delivered.</summary>
            EnergyWaterDelivered = 869,
            ///<summary>The energy of wind has been delivered.</summary>
            EnergyWindDelivered = 870,
            ///<summary>The seed has been sown.</summary>
            TheSeedHasBeenSown = 871,
            ///<summary>This seed may not be sown here.</summary>
            ThisSeedMayNotBeSownHere = 872,
            ///<summary>That character does not exist.</summary>
            CharacterDoesNotExist = 873,
            ///<summary>The capacity of the warehouse has been exceeded.</summary>
            WarehouseCapacityExceeded = 874,
            ///<summary>The transport of the cargo has been canceled.</summary>
            CargoCanceled = 875,
            ///<summary>The cargo was not delivered.</summary>
            CargoNotDelivered = 876,
            ///<summary>The symbol has been added.</summary>
            SymbolAdded = 877,
            ///<summary>The symbol has been deleted.</summary>
            SymbolDeleted = 878,
            ///<summary>The manor system is currently under maintenance.</summary>
            TheManorSystemIsCurrentlyUnderMaintenance = 879,
            ///<summary>The transaction is complete.</summary>
            TheTransactionIsComplete = 880,
            ///<summary>There is a discrepancy on the invoice.</summary>
            ThereIsADiscrepancyOnTheInvoice = 881,
            ///<summary>The seed quantity is incorrect.</summary>
            TheSeedQuantityIsIncorrect = 882,
            ///<summary>The seed information is incorrect.</summary>
            TheSeedInformationIsIncorrect = 883,
            ///<summary>The manor information has been updated.</summary>
            TheManorInformationHasBeenUpdated = 884,
            ///<summary>The number of crops is incorrect.</summary>
            TheNumberOfCropsIsIncorrect = 885,
            ///<summary>The crops are priced incorrectly.</summary>
            TheCropsArePricedIncorrectly = 886,
            ///<summary>The type is incorrect.</summary>
            TheTypeIsIncorrect = 887,
            ///<summary>No crops can be purchased at this time.</summary>
            NoCropsCanBePurchasedAtThisTime = 888,
            ///<summary>The seed was successfully sown.</summary>
            TheSeedWasSuccessfullySown = 889,
            ///<summary>The seed was not sown.</summary>
            TheSeedWasNotSown = 890,
            ///<summary>You are not authorized to harvest.</summary>
            YouAreNotAuthorizedToHarvest = 891,
            ///<summary>The harvest has failed.</summary>
            TheHarvestHasFailed = 892,
            ///<summary>The harvest failed because the seed was not sown.</summary>
            TheHarvestFailedBecauseTheSeedWasNotSown = 893,
            ///<summary>Up to $s1 recipes can be registered.</summary>
            UpToS1RecipesCanRegister = 894,
            ///<summary>No recipes have been registered.</summary>
            NoRecipesRegistered = 895,
            ///<summary>Message:The ferry has arrived at Gludin Harbor.</summary>
            FerryAtGludin = 896,
            ///<summary>Message:The ferry will leave for Talking Island Harbor after anchoring for ten minutes.</summary>
            FerryLeaveTalking = 897,
            ///<summary>Only characters of level 10 or above are authorized to make recommendations.</summary>
            OnlyLevelSup10CanRecommend = 898,
            ///<summary>The symbol cannot be drawn.</summary>
            CantDrawSymbol = 899,
            ///<summary>No slot exists to draw the symbol.</summary>
            SymbolsFull = 900,
            ///<summary>The symbol information cannot be found.</summary>
            SymbolNotFound = 901,
            ///<summary>The number of items is incorrect.</summary>
            NumberIncorrect = 902,
            ///<summary>You may not submit a petition while frozen. Be patient.</summary>
            NoPetitionWhileFrozen = 903,
            ///<summary>Items cannot be discarded while in private store status.</summary>
            NoDiscardWhilePrivateStore = 904,
            ///<summary>The current score for the Humans is $s1.</summary>
            HumanScoreS1 = 905,
            ///<summary>The current score for the Elves is $s1.</summary>
            ElvesScoreS1 = 906,
            ///<summary>The current score for the Dark Elves is $s1.</summary>
            DarkElvesScoreS1 = 907,
            ///<summary>The current score for the Orcs is $s1.</summary>
            OrcsScoreS1 = 908,
            ///<summary>The current score for the Dwarves is $s1.</summary>
            DwarvenScoreS1 = 909,
            ///<summary>Current location : $s1, $s2, $s3 (Near Talking Island Village).</summary>
            LocTiS1S2S3 = 910,
            ///<summary>Current location : $s1, $s2, $s3 (Near Gludin Village).</summary>
            LocGludinS1S2S3 = 911,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Gludio).</summary>
            LocGludioS1S2S3 = 912,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Neutral Zone).</summary>
            LocNeutralZoneS1S2S3 = 913,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Elven Village).</summary>
            LocElvenS1S2S3 = 914,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Dark Elf Village).</summary>
            LocDarkElvenS1S2S3 = 915,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Dion).</summary>
            LocDionS1S2S3 = 916,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Floran Village).</summary>
            LocFloranS1S2S3 = 917,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Giran).</summary>
            LocGiranS1S2S3 = 918,
            ///<summary>Current location : $s1, $s2, $s3 (Near Giran Harbor).</summary>
            LocGiranHarborS1S2S3 = 919,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Orc Village).</summary>
            LocOrcS1S2S3 = 920,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Dwarven Village).</summary>
            LocDwarvenS1S2S3 = 921,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Oren).</summary>
            LocOrenS1S2S3 = 922,
            ///<summary>Current location : $s1, $s2, $s3 (Near Hunters Village).</summary>
            LocHunterS1S2S3 = 923,
            ///<summary>Current location : $s1, $s2, $s3 (Near Aden Castle Town).</summary>
            LocAdenS1S2S3 = 924,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Coliseum).</summary>
            LocColiseumS1S2S3 = 925,
            ///<summary>Current location : $s1, $s2, $s3 (Near Heine).</summary>
            LocHeineS1S2S3 = 926,
            ///<summary>The current time is $s1:$s2.</summary>
            TimeS1S2InTheDay = 927,
            ///<summary>The current time is $s1:$s2.</summary>
            TimeS1S2InTheNight = 928,
            ///<summary>No compensation was given for the farm products.</summary>
            NoCompensationForFarmProducts = 929,
            ///<summary>Lottery tickets are not currently being sold.</summary>
            NoLotteryTicketsCurrentSold = 930,
            ///<summary>The winning lottery ticket numbers has not yet been anonunced.</summary>
            LotteryWinnersNotAnnouncedYet = 931,
            ///<summary>You cannot chat locally while observing.</summary>
            NoAllchatWhileObserving = 932,
            ///<summary>The seed pricing greatly differs from standard seed prices.</summary>
            TheSeedPricingGreatlyDiffersFromStandardSeedPrices = 933,
            ///<summary>It is a deleted recipe.</summary>
            ADeletedRecipe = 934,
            ///<summary>The amount is not sufficient and so the manor is not in operation.</summary>
            TheAmountIsNotSufficientAndSoTheManorIsNotInOperation = 935,
            ///<summary>Use $s1.</summary>
            //UseS1 = 936,
            ///<summary>Currently preparing for private workshop.</summary>
            PreparingPrivateWorkshop = 937,
            ///<summary>The community server is currently offline.</summary>
            CbOffline = 938,
            ///<summary>You cannot exchange while blocking everything.</summary>
            NoExchangeWhileBlocking = 939,
            ///<summary>$s1 is blocked everything.</summary>
            S1BlockedEverything = 940,
            ///<summary>Restart at Talking Island Village.</summary>
            RestartAtTi = 941,
            ///<summary>Restart at Gludin Village.</summary>
            RestartAtGludin = 942,
            ///<summary>Restart at the Town of Gludin. || guess should be Gludio ;).</summary>
            RestartAtGludio = 943,
            ///<summary>Restart at the Neutral Zone.</summary>
            RestartAtNeutralZone = 944,
            ///<summary>Restart at the Elven Village.</summary>
            RestartAtElfenVillage = 945,
            ///<summary>Restart at the Dark Elf Village.</summary>
            RestartAtDarkelfVillage = 946,
            ///<summary>Restart at the Town of Dion.</summary>
            RestartAtDion = 947,
            ///<summary>Restart at Floran Village.</summary>
            RestartAtFloran = 948,
            ///<summary>Restart at the Town of Giran.</summary>
            RestartAtGiran = 949,
            ///<summary>Restart at Giran Harbor.</summary>
            RestartAtGiranHarbor = 950,
            ///<summary>Restart at the Orc Village.</summary>
            RestartAtOrcVillage = 951,
            ///<summary>Restart at the Dwarven Village.</summary>
            RestartAtDwarfenVillage = 952,
            ///<summary>Restart at the Town of Oren.</summary>
            RestartAtOren = 953,
            ///<summary>Restart at Hunters Village.</summary>
            RestartAtHuntersVillage = 954,
            ///<summary>Restart at the Town of Aden.</summary>
            RestartAtAden = 955,
            ///<summary>Restart at the Coliseum.</summary>
            RestartAtColiseum = 956,
            ///<summary>Restart at Heine.</summary>
            RestartAtHeine = 957,
            ///<summary>Items cannot be discarded or destroyed while operating a private store or workshop.</summary>
            ItemsCannotBeDiscardedOrDestroyedWhileOperatingPrivateStoreOrWorkshop = 958,
            ///<summary>$s1 (*$s2) manufactured successfully.</summary>
            S1S2ManufacturedSuccessfully = 959,
            ///<summary>$s1 manufacturing failure.</summary>
            S1ManufactureFailure = 960,
            ///<summary>You are now blocking everything.</summary>
            BlockingAll = 961,
            ///<summary>You are no longer blocking everything.</summary>
            NotBlockingAll = 962,
            ///<summary>Please determine the manufacturing price.</summary>
            DetermineManufacturePrice = 963,
            ///<summary>Chatting is prohibited for one minute.</summary>
            ChatbanFor1Minute = 964,
            ///<summary>The chatting prohibition has been removed.</summary>
            ChatbanRemoved = 965,
            ///<summary>Chatting is currently prohibited. If you try to chat before the prohibition is removed, the prohibition time will become even longer.</summary>
            ChattingIsCurrentlyProhibited = 966,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Random including spoil.).</summary>
            S1PartyInviteRandomIncludingSpoil = 967,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: By Turn.).</summary>
            S1PartyInviteByTurn = 968,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: By Turn including spoil.).</summary>
            S1PartyInviteByTurnIncludingSpoil = 969,
            ///<summary>$s2's MP has been drained by $s1.</summary>
            S2MpHasBeenDrainedByS1 = 970,
            ///<summary>Petitions cannot exceed 255 characters.</summary>
            PetitionMaxChars255 = 971,
            ///<summary>This pet cannot use this item.</summary>
            PetCannotUseItem = 972,
            ///<summary>Please input no more than the number you have.</summary>
            InputNoMoreYouHave = 973,
            ///<summary>The soul crystal succeeded in absorbing a soul.</summary>
            SoulCrystalAbsorbingSucceeded = 974,
            ///<summary>The soul crystal was not able to absorb a soul.</summary>
            SoulCrystalAbsorbingFailed = 975,
            ///<summary>The soul crystal broke because it was not able to endure the soul energy.</summary>
            SoulCrystalBroke = 976,
            ///<summary>The soul crystals caused resonation and failed at absorbing a soul.</summary>
            SoulCrystalAbsorbingFailedResonation = 977,
            ///<summary>The soul crystal is refusing to absorb a soul.</summary>
            SoulCrystalAbsorbingRefused = 978,
            ///<summary>The ferry arrived at Talking Island Harbor.</summary>
            FerryArrivedAtTalking = 979,
            ///<summary>The ferry will leave for Gludin Harbor after anchoring for ten minutes.</summary>
            FerryLeaveForGludinAfter10Minutes = 980,
            ///<summary>The ferry will leave for Gludin Harbor in five minutes.</summary>
            FerryLeaveForGludinIn5Minutes = 981,
            ///<summary>The ferry will leave for Gludin Harbor in one minute.</summary>
            FerryLeaveForGludinIn1Minute = 982,
            ///<summary>Those wishing to ride should make haste to get on.</summary>
            MakeHasteGetOnBoat = 983,
            ///<summary>The ferry will be leaving soon for Gludin Harbor.</summary>
            FerryLeaveSoonForGludin = 984,
            ///<summary>The ferry is leaving for Gludin Harbor.</summary>
            FerryLeavingForGludin = 985,
            ///<summary>The ferry has arrived at Gludin Harbor.</summary>
            FerryArrivedAtGludin = 986,
            ///<summary>The ferry will leave for Talking Island Harbor after anchoring for ten minutes.</summary>
            FerryLeaveForTalkingAfter10Minutes = 987,
            ///<summary>The ferry will leave for Talking Island Harbor in five minutes.</summary>
            FerryLeaveForTalkingIn5Minutes = 988,
            ///<summary>The ferry will leave for Talking Island Harbor in one minute.</summary>
            FerryLeaveForTalkingIn1Minute = 989,
            ///<summary>The ferry will be leaving soon for Talking Island Harbor.</summary>
            FerryLeaveSoonForTalking = 990,
            ///<summary>The ferry is leaving for Talking Island Harbor.</summary>
            FerryLeavingForTalking = 991,
            ///<summary>The ferry has arrived at Giran Harbor.</summary>
            FerryArrivedAtGiran = 992,
            ///<summary>The ferry will leave for Giran Harbor after anchoring for ten minutes.</summary>
            FerryLeaveForGiranAfter10Minutes = 993,
            ///<summary>The ferry will leave for Giran Harbor in five minutes.</summary>
            FerryLeaveForGiranIn5Minutes = 994,
            ///<summary>The ferry will leave for Giran Harbor in one minute.</summary>
            FerryLeaveForGiranIn1Minute = 995,
            ///<summary>The ferry will be leaving soon for Giran Harbor.</summary>
            FerryLeaveSoonForGiran = 996,
            ///<summary>The ferry is leaving for Giran Harbor.</summary>
            FerryLeavingForGiran = 997,
            ///<summary>The Innadril pleasure boat has arrived. It will anchor for ten minutes.</summary>
            InnadrilBoatAnchor10Minutes = 998,
            ///<summary>The Innadril pleasure boat will leave in five minutes.</summary>
            InnadrilBoatLeaveIn5Minutes = 999,
            ///<summary>The Innadril pleasure boat will leave in one minute.</summary>
            InnadrilBoatLeaveIn1Minute = 1000,
            ///<summary>The Innadril pleasure boat will be leaving soon.</summary>
            InnadrilBoatLeaveSoon = 1001,
            ///<summary>The Innadril pleasure boat is leaving.</summary>
            InnadrilBoatLeaving = 1002,
            ///<summary>Cannot possess a monster race ticket.</summary>
            CannotPossesMonsTicket = 1003,
            ///<summary>You have registered for a clan hall auction.</summary>
            RegisteredForClanhall = 1004,
            ///<summary>There is not enough adena in the clan hall warehouse.</summary>
            NotEnoughAdenaInCwh = 1005,
            ///<summary>You have bid in a clan hall auction.</summary>
            BidInClanhallAuction = 1006,
            ///<summary>The preliminary match registration of $s1 has finished.</summary>
            PreliminaryRegistrationOfS1Finished = 1007,
            ///<summary>A hungry strider cannot be mounted or dismounted.</summary>
            HungryStriderNotMount = 1008,
            ///<summary>A strider cannot be ridden when dead.</summary>
            StriderCantBeRiddenWhileDead = 1009,
            ///<summary>A dead strider cannot be ridden.</summary>
            DeadStriderCantBeRidden = 1010,
            ///<summary>A strider in battle cannot be ridden.</summary>
            StriderInBatlleCantBeRidden = 1011,
            ///<summary>A strider cannot be ridden while in battle.</summary>
            StriderCantBeRiddenWhileInBattle = 1012,
            ///<summary>A strider can be ridden only when standing.</summary>
            StriderCanBeRiddenOnlyWhileStanding = 1013,
            ///<summary>Your pet gained $s1 experience points.</summary>
            PetEarnedS1Exp = 1014,
            ///<summary>Your pet hit for $s1 damage.</summary>
            PetHitForS1Damage = 1015,
            ///<summary>Pet received $s2 damage by $s1.</summary>
            PetReceivedS2DamageByS1 = 1016,
            ///<summary>Pet's critical hit!.</summary>
            CriticalHitByPet = 1017,
            ///<summary>Your pet uses $s1.</summary>
            PetUsesS1 = 1018,
            ///<summary>Your pet uses $s1.</summary>
            //PetUsesS1 = 1019,
            ///<summary>Your pet picked up $s1.</summary>
            PetPickedS1 = 1020,
            ///<summary>Your pet picked up $s2 $s1(s).</summary>
            PetPickedS2S1S = 1021,
            ///<summary>Your pet picked up +$s1 $s2.</summary>
            PetPickedS1S2 = 1022,
            ///<summary>Your pet picked up $s1 adena.</summary>
            PetPickedS1Adena = 1023,
            ///<summary>Your pet put on $s1.</summary>
            PetPutOnS1 = 1024,
            ///<summary>Your pet took off $s1.</summary>
            PetTookOffS1 = 1025,
            ///<summary>The summoned monster gave damage of $s1.</summary>
            SummonGaveDamageS1 = 1026,
            ///<summary>Servitor received $s2 damage caused by $s1.</summary>
            SummonReceivedDamageS2ByS1 = 1027,
            ///<summary>Summoned monster's critical hit!.</summary>
            CriticalHitBySummonedMob = 1028,
            ///<summary>Summoned monster uses $s1.</summary>
            SummonedMobUsesS1 = 1029,
            ///<summary>Party Information.</summary>
            PartyInformation = 1030,
            ///<summary>Looting method: Finders keepers.</summary>
            LootingFindersKeepers = 1031,
            ///<summary>Looting method: Random.</summary>
            LootingRandom = 1032,
            ///<summary>Looting method: Random including spoil.</summary>
            LootingRandomIncludeSpoil = 1033,
            ///<summary>Looting method: By turn.</summary>
            LootingByTurn = 1034,
            ///<summary>Looting method: By turn including spoil.</summary>
            LootingByTurnIncludeSpoil = 1035,
            ///<summary>You have exceeded the quantity that can be inputted.</summary>
            YouHaveExceededQuantityThatCanBeInputted = 1036,
            ///<summary>$s1 manufactured $s2.</summary>
            S1ManufacturedS2 = 1037,
            ///<summary>$s1 manufactured $s3 $s2(s).</summary>
            S1ManufacturedS3S2S = 1038,
            ///<summary>Items left at the clan hall warehouse can only be retrieved by the clan leader. Do you want to continue?.</summary>
            OnlyClanLeaderCanRetrieveItemsFromClanWarehouse = 1039,
            ///<summary>Items sent by freight can be picked up from any Warehouse location. Do you want to continue?.</summary>
            ItemsSentByFreightPickedUpFromAnywhere = 1040,
            ///<summary>The next seed purchase price is $s1 adena.</summary>
            TheNextSeedPurchasePriceIsS1Adena = 1041,
            ///<summary>The next farm goods purchase price is $s1 adena.</summary>
            TheNextFarmGoodsPurchasePriceIsS1Adena = 1042,
            ///<summary>At the current time, the "/unstuck" command cannot be used. Please send in a petition.</summary>
            NoUnstuckPleaseSendPetition = 1043,
            ///<summary>Monster race payout information is not available while tickets are being sold.</summary>
            MonsraceNoPayoutInfo = 1044,
            ///<summary>Monster race tickets are no longer available.</summary>
            MonsraceTicketsNotAvailable = 1046,
            ///<summary>We did not succeed in producing $s1 item.</summary>
            NotSucceedProducingS1 = 1047,
            ///<summary>When "blocking" everything, whispering is not possible.</summary>
            NoWhisperWhenBlocking = 1048,
            ///<summary>When "blocking" everything, it is not possible to send invitations for organizing parties.</summary>
            NoPartyWhenBlocking = 1049,
            ///<summary>There are no communities in my clan. Clan communities are allowed for clans with skill levels of 2 and higher.</summary>
            NoCbInMyClan = 1050,
            ///<summary>Payment for your clan hall has not been made please make payment tomorrow.</summary>
            PaymentForYourClanHallHasNotBeenMadePleaseMakePaymentToYourClanWarehouseByS1Tomorrow = 1051,
            ///<summary>Payment of Clan Hall is overdue the owner loose Clan Hall.</summary>
            TheClanHallFeeIsOneWeekOverdueThereforeTheClanHallOwnershipHasBeenRevoked = 1052,
            ///<summary>It is not possible to resurrect in battlefields where a siege war is taking place.</summary>
            CannotBeResurrectedDuringSiege = 1053,
            ///<summary>You have entered a mystical land.</summary>
            EnteredMysticalLand = 1054,
            ///<summary>You have left a mystical land.</summary>
            ExitedMysticalLand = 1055,
            ///<summary>You have exceeded the storage capacity of the castle's vault.</summary>
            VaultCapacityExceeded = 1056,
            ///<summary>This command can only be used in the relax server.</summary>
            RelaxServerOnly = 1057,
            ///<summary>The sales price for seeds is $s1 adena.</summary>
            TheSalesPriceForSeedsIsS1Adena = 1058,
            ///<summary>The remaining purchasing amount is $s1 adena.</summary>
            TheRemainingPurchasingIsS1Adena = 1059,
            ///<summary>The remainder after selling the seeds is $s1.</summary>
            TheRemainderAfterSellingTheSeedsIsS1 = 1060,
            ///<summary>The recipe cannot be registered. You do not have the ability to create items.</summary>
            CantRegisterNoAbilityToCraft = 1061,
            ///<summary>Writing something new is possible after level 10.</summary>
            WritingSomethingNewPossibleAfterLevel10 = 1062,
            ///<summary>if you become trapped or unable to move, please use the '/unstuck' command.</summary>
            PetitionUnavailable = 1063,
            ///<summary>The equipment, +$s1 $s2, has been removed.</summary>
            EquipmentS1S2Removed = 1064,
            ///<summary>While operating a private store or workshop, you cannot discard, destroy, or trade an item.</summary>
            CannotTradeDiscardDropItemWhileInShopmode = 1065,
            ///<summary>$s1 HP has been restored.</summary>
            S1HpRestored = 1066,
            ///<summary>$s2 HP has been restored by $s1.</summary>
            S2HpRestoredByS1 = 1067,
            ///<summary>$s1 MP has been restored.</summary>
            S1MpRestored = 1068,
            ///<summary>$s2 MP has been restored by $s1.</summary>
            S2MpRestoredByS1 = 1069,
            ///<summary>You do not have 'read' permission.</summary>
            NoReadPermission = 1070,
            ///<summary>You do not have 'write' permission.</summary>
            NoWritePermission = 1071,
            ///<summary>You have obtained a ticket for the Monster Race #$s1 - Single.</summary>
            ObtainedTicketForMonsRaceS1Single = 1072,
            ///<summary>You have obtained a ticket for the Monster Race #$s1 - Single.</summary>
            //ObtainedTicketForMonsRaceS1Single = 1073,
            ///<summary>You do not meet the age requirement to purchase a Monster Race Ticket.</summary>
            NotMeetAgeRequirementForMonsRace = 1074,
            ///<summary>The bid amount must be higher than the previous bid.</summary>
            BidAmountHigherThanPreviousBid = 1075,
            ///<summary>The game cannot be terminated at this time.</summary>
            GameCannotTerminateNow = 1076,
            ///<summary>A GameGuard Execution error has occurred. Please send the *.erl file(s) located in the GameGuard folder to game@inca.co.kr.</summary>
            GgExecutionError = 1077,
            ///<summary>When a user's keyboard input exceeds a certain cumulative score a chat ban will be applied. This is done to discourage spamming. Please avoid posting the same message multiple times during a short period.</summary>
            DontSpam = 1078,
            ///<summary>The target is currently banend from chatting.</summary>
            TargetIsChatBanned = 1079,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type A?.</summary>
            FaceliftPotionTypeA = 1080,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type A?.</summary>
            HairdyePotionTypeA = 1081,
            ///<summary>Do you wish to use the hair style change potion - Type A? It is permanent.</summary>
            HairstylePotionTypeA = 1082,
            ///<summary>Facelift potion - Type A is being applied.</summary>
            FaceliftPotionTypeAApplied = 1083,
            ///<summary>Hair dye potion - Type A is being applied.</summary>
            HairdyePotionTypeAApplied = 1084,
            ///<summary>The hair style chance potion - Type A is being used.</summary>
            HairstylePotionTypeAUsed = 1085,
            ///<summary>Your facial appearance has been changed.</summary>
            FaceAppearanceChanged = 1086,
            ///<summary>Your hair color has changed.</summary>
            HairColorChanged = 1087,
            ///<summary>Your hair style has been changed.</summary>
            HairStyleChanged = 1088,
            ///<summary>$s1 has obtained a first anniversary commemorative item.</summary>
            S1ObtainedAnniversaryItem = 1089,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type B?.</summary>
            FaceliftPotionTypeB = 1090,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type C?.</summary>
            FaceliftPotionTypeC = 1091,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type B?.</summary>
            HairdyePotionTypeB = 1092,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type C?.</summary>
            HairdyePotionTypeC = 1093,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type D?.</summary>
            HairdyePotionTypeD = 1094,
            ///<summary>Do you wish to use the hair style change potion - Type B? It is permanent.</summary>
            HairstylePotionTypeB = 1095,
            ///<summary>Do you wish to use the hair style change potion - Type C? It is permanent.</summary>
            HairstylePotionTypeC = 1096,
            ///<summary>Do you wish to use the hair style change potion - Type D? It is permanent.</summary>
            HairstylePotionTypeD = 1097,
            ///<summary>Do you wish to use the hair style change potion - Type E? It is permanent.</summary>
            HairstylePotionTypeE = 1098,
            ///<summary>Do you wish to use the hair style change potion - Type F? It is permanent.</summary>
            HairstylePotionTypeF = 1099,
            ///<summary>Do you wish to use the hair style change potion - Type G? It is permanent.</summary>
            HairstylePotionTypeG = 1100,
            ///<summary>Facelift potion - Type B is being applied.</summary>
            FaceliftPotionTypeBApplied = 1101,
            ///<summary>Facelift potion - Type C is being applied.</summary>
            FaceliftPotionTypeCApplied = 1102,
            ///<summary>Hair dye potion - Type B is being applied.</summary>
            HairdyePotionTypeBApplied = 1103,
            ///<summary>Hair dye potion - Type C is being applied.</summary>
            HairdyePotionTypeCApplied = 1104,
            ///<summary>Hair dye potion - Type D is being applied.</summary>
            HairdyePotionTypeDApplied = 1105,
            ///<summary>The hair style chance potion - Type B is being used.</summary>
            HairstylePotionTypeBUsed = 1106,
            ///<summary>The hair style chance potion - Type C is being used.</summary>
            HairstylePotionTypeCUsed = 1107,
            ///<summary>The hair style chance potion - Type D is being used.</summary>
            HairstylePotionTypeDUsed = 1108,
            ///<summary>The hair style chance potion - Type E is being used.</summary>
            HairstylePotionTypeEUsed = 1109,
            ///<summary>The hair style chance potion - Type F is being used.</summary>
            HairstylePotionTypeFUsed = 1110,
            ///<summary>The hair style chance potion - Type G is being used.</summary>
            HairstylePotionTypeGUsed = 1111,
            ///<summary>The prize amount for the winner of Lottery #$s1 is $s2 adena. We have $s3 first prize winners.</summary>
            AmountForWinnerS1IsS2AdenaWeHaveS3PrizeWinner = 1112,
            ///<summary>The prize amount for Lucky Lottery #$s1 is $s2 adena. There was no first prize winner in this drawing, therefore the jackpot will be added to the next drawing.</summary>
            AmountForLotteryS1IsS2AdenaNoWinner = 1113,
            ///<summary>Your clan may not register to participate in a siege while under a grace period of the clan's dissolution.</summary>
            CantParticipateInSiegeWhileDissolutionInProgress = 1114,
            ///<summary>Individuals may not surrender during combat.</summary>
            IndividualsNotSurrenderDuringCombat = 1115,
            ///<summary>One cannot leave one's clan during combat.</summary>
            YouCannotLeaveDuringCombat = 1116,
            ///<summary>A clan member may not be dismissed during combat.</summary>
            ClanMemberCannotBeDismissedDuringCombat = 1117,
            ///<summary>Progress in a quest is possible only when your inventory's weight and volume are less than 80 percent of capacity.</summary>
            InventoryLessThan80Percent = 1118,
            ///<summary>Quest was automatically canceled when you attempted to settle the accounts of your quest while your inventory exceeded 80 percent of capacity.</summary>
            QuestCanceledInventoryExceeds80Percent = 1119,
            ///<summary>You are still a member of the clan.</summary>
            StillClanMember = 1120,
            ///<summary>You do not have the right to vote.</summary>
            NoRightToVote = 1121,
            ///<summary>There is no candidate.</summary>
            NoCandidate = 1122,
            ///<summary>Weight and volume limit has been exceeded. That skill is currently unavailable.</summary>
            WeightExceededSkillUnavailable = 1123,
            ///<summary>Your recipe book may not be accessed while using a skill.</summary>
            NoRecipeBookWhileCasting = 1124,
            ///<summary>An item may not be created while engaged in trading.</summary>
            CannotCreatedWhileEngagedInTrading = 1125,
            ///<summary>You cannot enter a negative number.</summary>
            NoNegativeNumber = 1126,
            ///<summary>The reward must be less than 10 times the standard price.</summary>
            RewardLessThan10TimesStandardPrice = 1127,
            ///<summary>A private store may not be opened while using a skill.</summary>
            PrivateStoreNotWhileCasting = 1128,
            ///<summary>This is not allowed while riding a ferry or boat.</summary>
            NotAllowedOnBoat = 1129,
            ///<summary>You have given $s1 damage to your target and $s2 damage to the servitor.</summary>
            GivenS1DamageToYourTargetAndS2DamageToServitor = 1130,
            ///<summary>It is now midnight and the effect of $s1 can be felt.</summary>
            NightS1EffectApplies = 1131,
            ///<summary>It is now dawn and the effect of $s1 will now disappear.</summary>
            DayS1EffectDisappears = 1132,
            ///<summary>Since HP has decreased, the effect of $s1 can be felt.</summary>
            HpDecreasedEffectApplies = 1133,
            ///<summary>Since HP has increased, the effect of $s1 will disappear.</summary>
            HpIncreasedEffectDisappears = 1134,
            ///<summary>While you are engaged in combat, you cannot operate a private store or private workshop.</summary>
            CantOperatePrivateStoreDuringCombat = 1135,
            ///<summary>Since there was an account that used this IP and attempted to log in illegally, this account is not allowed to connect to the game server for $s1 minutes. Please use another game server.</summary>
            AccountNotAllowedToConnect = 1136,
            ///<summary>$s1 harvested $s3 $s2(s).</summary>
            S1HarvestedS3S2S = 1137,
            ///<summary>$s1 harvested $s2(s).</summary>
            S1HarvestedS2S = 1138,
            ///<summary>The weight and volume limit of your inventory must not be exceeded.</summary>
            InventoryLimitMustNotBeExceeded = 1139,
            ///<summary>Would you like to open the gate?.</summary>
            WouldYouLikeToOpenTheGate = 1140,
            ///<summary>Would you like to close the gate?.</summary>
            WouldYouLikeToCloseTheGate = 1141,
            ///<summary>Since $s1 already exists nearby, you cannot summon it again.</summary>
            CannotSummonS1Again = 1142,
            ///<summary>Since you do not have enough items to maintain the servitor's stay, the servitor will disappear.</summary>
            ServitorDisappearedNotEnoughItems = 1143,
            ///<summary>Currently, you don't have anybody to chat with in the game.</summary>
            NobodyInGameToChat = 1144,
            ///<summary>$s2 has been created for $s1 after the payment of $s3 adena is received.</summary>
            S2CreatedForS1ForS3Adena = 1145,
            ///<summary>$s1 created $s2 after receiving $s3 adena.</summary>
            S1CreatedS2ForS3Adena = 1146,
            ///<summary>$s2 $s3 have been created for $s1 at the price of $s4 adena.</summary>
            S2S3SCreatedForS1ForS4Adena = 1147,
            ///<summary>$s1 created $s2 $s3 at the price of $s4 adena.</summary>
            S1CreatedS2S3SForS4Adena = 1148,
            ///<summary>Your attempt to create $s2 for $s1 at the price of $s3 adena has failed.</summary>
            CreationOfS2ForS1AtS3AdenaFailed = 1149,
            ///<summary>$s1 has failed to create $s2 at the price of $s3 adena.</summary>
            S1FailedToCreateS2ForS3Adena = 1150,
            ///<summary>$s2 is sold to $s1 at the price of $s3 adena.</summary>
            S2SoldToS1ForS3Adena = 1151,
            ///<summary>$s2 $s3 have been sold to $s1 for $s4 adena.</summary>
            S3S2SSoldToS1ForS4Adena = 1152,
            ///<summary>$s2 has been purchased from $s1 at the price of $s3 adena.</summary>
            S2PurchasedFromS1ForS3Adena = 1153,
            ///<summary>$s3 $s2 has been purchased from $s1 for $s4 adena.</summary>
            S3S2SPurchasedFromS1ForS4Adena = 1154,
            ///<summary>+$s2 $s3 have been sold to $s1 for $s4 adena.</summary>
            S3S2SoldToS1ForS4Adena = 1155,
            ///<summary>+$s2 $s3 has been purchased from $s1 for $s4 adena.</summary>
            S2S3PurchasedFromS1ForS4Adena = 1156,
            ///<summary>Trying on state lasts for only 5 seconds. When a character's state changes, it can be cancelled.</summary>
            TryingOnState = 1157,
            ///<summary>You cannot dismount from this elevation.</summary>
            CannotDismountFromElevation = 1158,
            ///<summary>The ferry from Talking Island will arrive at Gludin Harbor in approximately 10 minutes.</summary>
            FerryFromTalkingArriveAtGludin10Minutes = 1159,
            ///<summary>The ferry from Talking Island will be arriving at Gludin Harbor in approximately 5 minutes.</summary>
            FerryFromTalkingArriveAtGludin5Minutes = 1160,
            ///<summary>The ferry from Talking Island will be arriving at Gludin Harbor in approximately 1 minute.</summary>
            FerryFromTalkingArriveAtGludin1Minute = 1161,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 15 minutes.</summary>
            FerryFromGiranArriveAtTalking15Minutes = 1162,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 10 minutes.</summary>
            FerryFromGiranArriveAtTalking10Minutes = 1163,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 5 minutes.</summary>
            FerryFromGiranArriveAtTalking5Minutes = 1164,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 1 minute.</summary>
            FerryFromGiranArriveAtTalking1Minute = 1165,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FerryFromTalkingArriveAtGiran20Minutes = 1166,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FerryFromTalkingArriveAtGiran15Minutes = 1167,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FerryFromTalkingArriveAtGiran10Minutes = 1168,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FerryFromTalkingArriveAtGiran5Minutes = 1169,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 1 minute.</summary>
            FerryFromTalkingArriveAtGiran1Minute = 1170,
            ///<summary>The Innadril pleasure boat will arrive in approximately 20 minutes.</summary>
            InnadrilBoatArrive20Minutes = 1171,
            ///<summary>The Innadril pleasure boat will arrive in approximately 15 minutes.</summary>
            InnadrilBoatArrive15Minutes = 1172,
            ///<summary>The Innadril pleasure boat will arrive in approximately 10 minutes.</summary>
            InnadrilBoatArrive10Minutes = 1173,
            ///<summary>The Innadril pleasure boat will arrive in approximately 5 minutes.</summary>
            InnadrilBoatArrive5Minutes = 1174,
            ///<summary>The Innadril pleasure boat will arrive in approximately 1 minute.</summary>
            InnadrilBoatArrive1Minute = 1175,
            ///<summary>This is a quest event period.</summary>
            QuestEventPeriod = 1176,
            ///<summary>This is the seal validation period.</summary>
            ValidationPeriod = 1177,
            ///<summary>Seal of Avarice description.</summary>
            AvariceDescription = 1178,
            ///<summary>Seal of Gnosis description.</summary>
            GnosisDescription = 1179,
            ///<summary>Seal of Strife description.</summary>
            StrifeDescription = 1180,
            ///<summary>Do you really wish to change the title?.</summary>
            ChangeTitleConfirm = 1181,
            ///<summary>Are you sure you wish to delete the clan crest?.</summary>
            CrestDeleteConfirm = 1182,
            ///<summary>This is the initial period.</summary>
            InitialPeriod = 1183,
            ///<summary>This is a period of calculating statistics in the server.</summary>
            ResultsPeriod = 1184,
            ///<summary>days left until deletion.</summary>
            DaysLeftUntilDeletion = 1185,
            ///<summary>To create a new account, please visit the PlayNC website (http://www.plaync.com/us/support/).</summary>
            ToCreateAccountVisitWebsite = 1186,
            ///<summary>If you forgotten your account information or password, please visit the Support Center on the PlayNC website(http://www.plaync.com/us/support/).</summary>
            AccountInformationForgottonVisitWebsite = 1187,
            ///<summary>Your selected target can no longer receive a recommendation.</summary>
            YourTargetNoLongerReceiveARecommendation = 1188,
            ///<summary>This temporary alliance of the Castle Attacker team is in effect. It will be dissolved when the Castle Lord is replaced.</summary>
            TemporaryAlliance = 1189,
            ///<summary>This temporary alliance of the Castle Attacker team has been dissolved.</summary>
            TemporaryAllianceDissolved = 1189,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 10 minutes.</summary>
            FerryFromGludinArriveAtTalking10Minutes = 1191,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 5 minutes.</summary>
            FerryFromGludinArriveAtTalking5Minutes = 1192,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 1 minute.</summary>
            FerryFromGludinArriveAtTalking1Minute = 1193,
            ///<summary>A mercenary can be assigned to a position from the beginning of the Seal Validatio period until the time when a siege starts.</summary>
            MercCanBeAssigned = 1194,
            ///<summary>This mercenary cannot be assigned to a position by using the Seal of Strife.</summary>
            MercCantBeAssignedUsingStrife = 1195,
            ///<summary>Your force has reached maximum capacity.</summary>
            ForceMaximum = 1196,
            ///<summary>Summoning a servitor costs $s2 $s1.</summary>
            SummoningServitorCostsS2S1 = 1197,
            ///<summary>The item has been successfully crystallized.</summary>
            CrystallizationSuccessful = 1198,
            ///<summary>=======Clan War Target=======.</summary>
            ClanWarHeader = 1199,
            ///<summary>Message:($s1 ($s2 Alliance).</summary>
            S1S2Alliance = 1200,
            ///<summary>Please select the quest you wish to abort.</summary>
            SelectQuestToAbor = 1201,
            ///<summary>Message:($s1 (No alliance exists).</summary>
            S1NoAlliExists = 1202,
            ///<summary>There is no clan war in progress.</summary>
            NoWarInProgress = 1203,
            ///<summary>The screenshot has been saved. ($s1 $s2x$s3).</summary>
            Screenshot = 1204,
            ///<summary>Your mailbox is full. There is a 100 message limit.</summary>
            MailboxFull = 1205,
            ///<summary>The memo box is full. There is a 100 memo limit.</summary>
            MemoboxFull = 1206,
            ///<summary>Please make an entry in the field.</summary>
            MakeAnEntry = 1207,
            ///<summary>$s1 died and dropped $s3 $s2.</summary>
            S1DiedDroppedS3S2 = 1208,
            ///<summary>Congratulations. Your raid was successful.</summary>
            RaidWasSuccessful = 1209,
            ///<summary>Seven Signs: The quest event period has begun. Visit a Priest of Dawn or Priestess of Dusk to participate in the event.</summary>
            QuestEventPeriodBegun = 1210,
            ///<summary>Seven Signs: The quest event period has ended. The next quest event will start in one week.</summary>
            QuestEventPeriodEnded = 1211,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Avarice.</summary>
            DawnObtainedAvarice = 1212,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Gnosis.</summary>
            DawnObtainedGnosis = 1213,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Strife.</summary>
            DawnObtainedStrife = 1214,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Avarice.</summary>
            DuskObtainedAvarice = 1215,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Gnosis.</summary>
            DuskObtainedGnosis = 1216,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Strife.</summary>
            DuskObtainedStrife = 1217,
            ///<summary>Seven Signs: The Seal Validation period has begun.</summary>
            SealValidationPeriodBegun = 1218,
            ///<summary>Seven Signs: The Seal Validation period has ended.</summary>
            SealValidationPeriodEnded = 1219,
            ///<summary>Are you sure you wish to summon it?.</summary>
            SummonConfirm = 1220,
            ///<summary>Are you sure you wish to return it?.</summary>
            ReturnConfirm = 1221,
            ///<summary>Current location : $s1, $s2, $s3 (GM Consultation Service).</summary>
            LocGmConsulationServiceS1S2S3 = 1222,
            ///<summary>We depart for Talking Island in five minutes.</summary>
            DepartForTalking5Minutes = 1223,
            ///<summary>We depart for Talking Island in one minute.</summary>
            DepartForTalking1Minute = 1224,
            ///<summary>All aboard for Talking Island.</summary>
            DepartForTalking = 1225,
            ///<summary>We are now leaving for Talking Island.</summary>
            LeavingForTalking = 1226,
            ///<summary>You have $s1 unread messages.</summary>
            S1UnreadMessages = 1227,
            ///<summary>$s1 has blocked you. You cannot send mail to $s1.</summary>
            S1BlockedYouCannotMail = 1228,
            ///<summary>No more messages may be sent at this time. Each account is allowed 10 messages per day.</summary>
            NoMoreMessagesToday = 1229,
            ///<summary>You are limited to five recipients at a time.</summary>
            OnlyFiveRecipients = 1230,
            ///<summary>You've sent mail.</summary>
            SentMail = 1231,
            ///<summary>The message was not sent.</summary>
            MessageNotSent = 1232,
            ///<summary>You've got mail.</summary>
            NewMail = 1233,
            ///<summary>The mail has been stored in your temporary mailbox.</summary>
            MailStoredInMailbox = 1234,
            ///<summary>Do you wish to delete all your friends?.</summary>
            AllFriendsDeleteConfirm = 1235,
            ///<summary>Please enter security card number.</summary>
            EnterSecurityCardNumber = 1236,
            ///<summary>Please enter the card number for number $s1.</summary>
            EnterCardNumberForS1 = 1237,
            ///<summary>Your temporary mailbox is full. No more mail can be stored; you have reached the 10 message limit.</summary>
            TempMailboxFull = 1238,
            ///<summary>The keyboard security module has failed to load. Please exit the game and try again.</summary>
            KeyboardModuleFailedLoad = 1239,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have won.</summary>
            DuskWon = 1240,
            ///<summary>Seven Signs: The Lords of Dawn have won.</summary>
            DawnWon = 1241,
            ///<summary>Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m.</summary>
            NotVerifiedAgeNoLogin = 1242,
            ///<summary>The security card number is invalid.</summary>
            SecurityCardNumberInvalid = 1243,
            ///<summary>Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m. Logging off now.</summary>
            NotVerifiedAgeLogOff = 1244,
            ///<summary>You will be loged out in $s1 minutes.</summary>
            LogoutInS1Minutes = 1245,
            ///<summary>$s1 died and has dropped $s2 adena.</summary>
            S1DiedDroppedS2Adena = 1246,
            ///<summary>The corpse is too old. The skill cannot be used.</summary>
            CorpseTooOldSkillNotUsed = 1247,
            ///<summary>You are out of feed. Mount status canceled.</summary>
            OutOfFeedMountCanceled = 1248,
            ///<summary>You may only ride a wyvern while you're riding a strider.</summary>
            YouMayOnlyRideWyvernWhileRidingStrider = 1249,
            ///<summary>Do you really want to surrender? If you surrender during an alliance war, your Exp will drop the same as if you were to die once.</summary>
            SurrenderAllyWarConfirm = 1250,
            ///<summary>you will not be able to accept another clan to your alliance for one day.</summary>
            DismissAllyConfirm = 1251,
            ///<summary>Are you sure you want to surrender? Exp penalty will be the same as death.</summary>
            SurrenderConfirm1 = 1252,
            ///<summary>Are you sure you want to surrender? Exp penalty will be the same as death and you will not be allowed to participate in clan war.</summary>
            SurrenderConfirm2 = 1253,
            ///<summary>Thank you for submitting feedback.</summary>
            ThanksForFeedback = 1254,
            ///<summary>GM consultation has begun.</summary>
            GmConsultationBegun = 1255,
            ///<summary>Please write the name after the command.</summary>
            PleaseWriteNameAfterCommand = 1256,
            ///<summary>The special skill of a servitor or pet cannot be registerd as a macro.</summary>
            PetSkillNotAsMacro = 1257,
            ///<summary>$s1 has been crystallized.</summary>
            S1Crystallized = 1258,
            ///<summary>=======Alliance Target=======.</summary>
            AllianceTargetHeader = 1259,
            ///<summary>Seven Signs: Preparations have begun for the next quest event.</summary>
            PreparationsPeriodBegun = 1260,
            ///<summary>Seven Signs: The quest event period has begun. Speak with a Priest of Dawn or Dusk Priestess if you wish to participate in the event.</summary>
            CompetitionPeriodBegun = 1261,
            ///<summary>Seven Signs: Quest event has ended. Results are being tallied.</summary>
            ResultsPeriodBegun = 1262,
            ///<summary>Seven Signs: This is the seal validation period. A new quest event period begins next Monday.</summary>
            ValidationPeriodBegun = 1263,
            ///<summary>This soul stone cannot currently absorb souls. Absorption has failed.</summary>
            StoneCannotAbsorb = 1264,
            ///<summary>You can't absorb souls without a soul stone.</summary>
            CantAbsorbWithoutStone = 1265,
            ///<summary>The exchange has ended.</summary>
            ExchangeHasEnded = 1266,
            ///<summary>Your contribution score is increased by $s1.</summary>
            ContribScoreIncreasedS1 = 1267,
            ///<summary>Do you wish to add class as your sub class?.</summary>
            AddSubclassConfirm = 1268,
            ///<summary>The new sub class has been added.</summary>
            AddNewSubclass = 1269,
            ///<summary>The transfer of sub class has been completed.</summary>
            SubclassTransferCompleted = 1270,
            ///<summary>Do you wish to participate? Until the next seal validation period, you are a member of the Lords of Dawn.</summary>
            DawnConfirm = 1271,
            ///<summary>Do you wish to participate? Until the next seal validation period, you are a member of the Revolutionaries of Dusk.</summary>
            DuskConfirm = 1271,
            ///<summary>You will participate in the Seven Signs as a member of the Lords of Dawn.</summary>
            SevensignsPartecipationDawn = 1273,
            ///<summary>You will participate in the Seven Signs as a member of the Revolutionaries of Dusk.</summary>
            SevensignsPartecipationDusk = 1274,
            ///<summary>You've chosen to fight for the Seal of Avarice during this quest event period.</summary>
            FightForAvarice = 1275,
            ///<summary>You've chosen to fight for the Seal of Gnosis during this quest event period.</summary>
            FightForGnosis = 1276,
            ///<summary>You've chosen to fight for the Seal of Strife during this quest event period.</summary>
            FightForStrife = 1277,
            ///<summary>The NPC server is not operating at this time.</summary>
            NpcServerNotOperating = 1278,
            ///<summary>Contribution level has exceeded the limit. You may not continue.</summary>
            ContribScoreExceeded = 1279,
            ///<summary>Magic Critical Hit!.</summary>
            CriticalHitMagic = 1280,
            ///<summary>Your excellent shield defense was a success!.</summary>
            YourExcellentShieldDefenseWasASuccess = 1281,
            ///<summary>Your Karma has been changed to $s1.</summary>
            YourKarmaHasBeenChangedToS1 = 1282,
            ///<summary>The minimum frame option has been activated.</summary>
            MinimumFrameActivated = 1283,
            ///<summary>The minimum frame option has been deactivated.</summary>
            MinimumFrameDeactivated = 1284,
            ///<summary>No inventory exists: You cannot purchase an item.</summary>
            NoInventoryCannotPurchase = 1285,
            ///<summary>(Until next Monday at 6:00 p.m.).</summary>
            UntilMonday_6Pm = 1286,
            ///<summary>(Until today at 6:00 p.m.).</summary>
            UntilToday_6Pm = 1287,
            ///<summary>If trends continue, $s1 will win and the seal will belong to:.</summary>
            S1WillWinCompetition = 1288,
            ///<summary>(Until next Monday at 6:00 p.m.).</summary>
            SealOwned10MoreVoted = 1289,
            ///<summary>Although the seal was not owned, since 35 percent or more people have voted.</summary>
            SealNotOwned35MoreVoted = 1290,
            ///<summary>because less than 10 percent of people have voted.</summary>
            SealOwned10LessVoted = 1291,
            ///<summary>and since less than 35 percent of people have voted.</summary>
            SealNotOwned35LessVoted = 1292,
            ///<summary>If current trends continue, it will end in a tie.</summary>
            CompetitionWillTie = 1293,
            ///<summary>The competition has ended in a tie. Therefore, nobody has been awarded the seal.</summary>
            CompetitionTieSealNotAwarded = 1294,
            ///<summary>Sub classes may not be created or changed while a skill is in use.</summary>
            SubclassNoChangeOrCreateWhileSkillInUse = 1295,
            ///<summary>You cannot open a Private Store here.</summary>
            NoPrivateStoreHere = 1296,
            ///<summary>You cannot open a Private Workshop here.</summary>
            NoPrivateWorkshopHere = 1297,
            ///<summary>Please confirm that you would like to exit the Monster Race Track.</summary>
            MonsExitConfirm = 1298,
            ///<summary>$s1's casting has been interrupted.</summary>
            S1CastingInterrupted = 1299,
            ///<summary>You are no longer trying on equipment.</summary>
            WearItemsStopped = 1300,
            ///<summary>Only a Lord of Dawn may use this.</summary>
            CanBeUsedByDawn = 1301,
            ///<summary>Only a Revolutionary of Dusk may use this.</summary>
            CanBeUsedByDusk = 1302,
            ///<summary>This may only be used during the quest event period.</summary>
            CanBeUsedDuringQuestEventPeriod = 1303,
            ///<summary>except for an Alliance with a castle owning clan.</summary>
            StrifeCanceledDefensiveRegistration = 1304,
            ///<summary>Seal Stones may only be transferred during the quest event period.</summary>
            SealStonesOnlyWhileQuest = 1305,
            ///<summary>You are no longer trying on equipment.</summary>
            NoLongerTryingOn = 1306,
            ///<summary>Only during the seal validation period may you settle your account.</summary>
            SettleAccountOnlyInSealValidation = 1307,
            ///<summary>Congratulations - You've completed a class transfer!.</summary>
            ClassTransfer = 1308,
            ///<summary>Message:To use this option, you must have the lastest version of MSN Messenger installed on your computer.</summary>
            LatestMsnRequired = 1309,
            ///<summary>For full functionality, the latest version of MSN Messenger must be installed on your computer.</summary>
            LatestMsnRecommended = 1310,
            ///<summary>Previous versions of MSN Messenger only provide the basic features for in-game MSN Messenger Chat. Add/Delete Contacts and other MSN Messenger options are not available.</summary>
            MsnOnlyBasic = 1311,
            ///<summary>The latest version of MSN Messenger may be obtained from the MSN web site (http://messenger.msn.com).</summary>
            MsnObtainedFrom = 1312,
            ///<summary>$s1, to better serve our customers, all chat histories [...].</summary>
            S1ChatHistoriesStored = 1313,
            ///<summary>Please enter the passport ID of the person you wish to add to your contact list.</summary>
            EnterPassportForAdding = 1314,
            ///<summary>Deleting a contact will remove that contact from MSN Messenger as well. The contact can still check your online status and well not be blocked from sending you a message.</summary>
            DeletingAContact = 1315,
            ///<summary>The contact will be deleted and blocked from your contact list.</summary>
            ContactWillDeleted = 1316,
            ///<summary>Would you like to delete this contact?.</summary>
            ContactDeleteConfirm = 1317,
            ///<summary>Please select the contact you want to block or unblock.</summary>
            SelectContactForBlockUnblock = 1318,
            ///<summary>Please select the name of the contact you wish to change to another group.</summary>
            SelectContactForChangeGroup = 1319,
            ///<summary>After selecting the group you wish to move your contact to, press the OK button.</summary>
            SelectGroupPressOk = 1320,
            ///<summary>Enter the name of the group you wish to add.</summary>
            EnterGroupName = 1321,
            ///<summary>Select the group and enter the new name.</summary>
            SelectGroupEnterName = 1322,
            ///<summary>Select the group you wish to delete and click the OK button.</summary>
            SelectGroupToDelete = 1323,
            ///<summary>Signing in.</summary>
            SigningIn = 1324,
            ///<summary>You've logged into another computer and have been logged out of the .NET Messenger Service on this computer.</summary>
            AnotherComputerLogout = 1325,
            ///<summary>$s1 :.</summary>
            S1D = 1326,
            ///<summary>The following message could not be delivered:.</summary>
            MessageNotDelivered = 1327,
            ///<summary>Members of the Revolutionaries of Dusk will not be resurrected.</summary>
            DuskNotResurrected = 1328,
            ///<summary>You are currently blocked from using the Private Store and Private Workshop.</summary>
            BlockedFromUsingStore = 1329,
            ///<summary>You may not open a Private Store or Private Workshop for another $s1 minute(s).</summary>
            NoStoreForS1Minutes = 1330,
            ///<summary>You are no longer blocked from using the Private Store and Private Workshop.</summary>
            NoLongerBlockedUsingStore = 1331,
            ///<summary>Items may not be used after your character or pet dies.</summary>
            NoItemsAfterDeath = 1332,
            ///<summary>The replay file is not accessible. Please verify that the replay.ini exists in your Linage 2 directory.</summary>
            ReplayInaccessible = 1333,
            ///<summary>The new camera data has been stored.</summary>
            NewCameraStored = 1334,
            ///<summary>The attempt to store the new camera data has failed.</summary>
            CameraStoringFailed = 1335,
            ///<summary>The replay file, $s1.$$s2 has been corrupted, please check the fle.</summary>
            ReplayS1S2Corrupted = 1336,
            ///<summary>This will terminate the replay. Do you wish to continue?.</summary>
            ReplayTerminateConfirm = 1337,
            ///<summary>You have exceeded the maximum amount that may be transferred at one time.</summary>
            ExceededMaximumAmount = 1338,
            ///<summary>Once a macro is assigned to a shortcut, it cannot be run as a macro again.</summary>
            MacroShortcutNotRun = 1339,
            ///<summary>This server cannot be accessed by the coupon you are using.</summary>
            ServerNotAccessedByCoupon = 1340,
            ///<summary>Incorrect name and/or email address.</summary>
            IncorrectNameOrAddress = 1341,
            ///<summary>You are already logged in.</summary>
            AlreadyLoggedIn = 1342,
            ///<summary>Incorrect email address and/or password. Your attempt to log into .NET Messenger Service has failed.</summary>
            IncorrectAddressOrPassword = 1343,
            ///<summary>Your request to log into the .NET Messenger service has failed. Please verify that you are currently connected to the internet.</summary>
            NetLoginFailed = 1344,
            ///<summary>Click the OK button after you have selected a contact name.</summary>
            SelectContactClickOk = 1345,
            ///<summary>You are currently entering a chat message.</summary>
            CurrentlyEnteringChat = 1346,
            ///<summary>The Linage II messenger could not carry out the task you requested.</summary>
            MessengerFailedCarryingOutTask = 1347,
            ///<summary>$s1 has entered the chat room.</summary>
            S1EnteredChatRoom = 1348,
            ///<summary>$s1 has left the chat room.</summary>
            S1LeftChatRoom = 1349,
            ///<summary>The state will be changed to indicate "off-line." All the chat windows currently opened will be closed.</summary>
            GoingOffline = 1350,
            ///<summary>Click the Delete button after selecting the contact you wish to remove.</summary>
            SelectContactClickRemove = 1351,
            ///<summary>You have been added to $s1 ($s2)'s contact list.</summary>
            AddedToS1S2ContactList = 1352,
            ///<summary>You can set the option to show your status as always being off-line to all of your contacts.</summary>
            CanSetOptionToAlwaysShowOffline = 1353,
            ///<summary>You are not allowed to chat with a contact while chatting block is imposed.</summary>
            NoChatWhileBlocked = 1354,
            ///<summary>The contact is currently blocked from chatting.</summary>
            ContactCurrentlyBlocked = 1355,
            ///<summary>The contact is not currently logged in.</summary>
            ContactCurrentlyOffline = 1356,
            ///<summary>You have been blocked from chatting with that contact.</summary>
            YouAreBlocked = 1357,
            ///<summary>You are being logged out.</summary>
            YouAreLoggingOut = 1358,
            ///<summary>$s1 has logged in.</summary>
            S1LoggedIn2 = 1359,
            ///<summary>You have received a message from $s1.</summary>
            GotMessageFromS1 = 1360,
            ///<summary>Due to a system error, you have been logged out of the .NET Messenger Service.</summary>
            LoggedOutDueToError = 1361,
            ///<summary>click the button next to My Status and then use the Options menu.</summary>
            SelectContactToDelete = 1362,
            ///<summary>Your request to participate in the alliance war has been denied.</summary>
            YourRequestAllianceWarDenied = 1363,
            ///<summary>The request for an alliance war has been rejected.</summary>
            RequestAllianceWarRejected = 1364,
            ///<summary>$s2 of $s1 clan has surrendered as an individual.</summary>
            S2OfS1SurrenderedAsIndividual = 1365,
            ///<summary>In order to delete a group, you must not [...].</summary>
            DelteGroupInstruction = 1366,
            ///<summary>Only members of the group are allowed to add records.</summary>
            OnlyGroupCanAddRecords = 1367,
            ///<summary>You can not try those items on at the same time.</summary>
            YouCanNotTryThoseItemsOnAtTheSameTime = 1368,
            ///<summary>You've exceeded the maximum.</summary>
            ExceededTheMaximum = 1369,
            ///<summary>Your message to $s1 did not reach its recipient. You cannot send mail to the GM staff.</summary>
            CannotMailGmS1 = 1370,
            ///<summary>It has been determined that you're not engaged in normal gameplay and a restriction has been imposed upon you. You may not move for $s1 minutes.</summary>
            GameplayRestrictionPenaltyS1 = 1371,
            ///<summary>Your punishment will continue for $s1 minutes.</summary>
            PunishmentContinueS1Minutes = 1372,
            ///<summary>$s1 has picked up $s2 that was dropped by a Raid Boss.</summary>
            S1ObtainedS2FromRaidboss = 1373,
            ///<summary>$s1 has picked up $s3 $s2(s) that was dropped by a Raid Boss.</summary>
            S1PickedUpS3S2SFromRaidboss = 1374,
            ///<summary>$s1 has picked up $s2 adena that was dropped by a Raid Boss.</summary>
            S1ObtainedS2AdenaFromRaidboss = 1375,
            ///<summary>$s1 has picked up $s2 that was dropped by another character.</summary>
            S1ObtainedS2FromAnotherCharacter = 1376,
            ///<summary>$s1 has picked up $s3 $s2(s) that was dropped by a another character.</summary>
            S1PickedUpS3S2SFromAnotherCharacter = 1377,
            ///<summary>$s1 has picked up +$s3 $s2 that was dropped by a another character.</summary>
            S1PickedUpS3S2FromAnotherCharacter = 1378,
            ///<summary>$s1 has obtained $s2 adena.</summary>
            S1ObtainedS2Adena = 1379,
            ///<summary>You can't summon a $s1 while on the battleground.</summary>
            CantSummonS1OnBattleground = 1380,
            ///<summary>The party leader has obtained $s2 of $s1.</summary>
            LeaderObtainedS2OfS1 = 1381,
            ///<summary>To fulfill the quest, you must bring the chosen weapon. Are you sure you want to choose this weapon?.</summary>
            ChooseWeaponConfirm = 1382,
            ///<summary>Are you sure you want to exchange?.</summary>
            ExchangeConfirm = 1383,
            ///<summary>$s1 has become the party leader.</summary>
            S1HasBecomeAPartyLeader = 1384,
            ///<summary>You are not allowed to dismount at this location.</summary>
            NoDismountHere = 1385,
            ///<summary>You are no longer held in place.</summary>
            NoLongerHeldInPlace = 1386,
            ///<summary>Please select the item you would like to try on.</summary>
            SelectItemToTryOn = 1387,
            ///<summary>A party room has been created.</summary>
            PartyRoomCreated = 1388,
            ///<summary>The party room's information has been revised.</summary>
            PartyRoomRevised = 1389,
            ///<summary>You are not allowed to enter the party room.</summary>
            PartyRoomForbidden = 1390,
            ///<summary>You have exited from the party room.</summary>
            PartyRoomExited = 1391,
            ///<summary>$s1 has left the party room.</summary>
            S1LeftPartyRoom = 1392,
            ///<summary>You have been ousted from the party room.</summary>
            OustedFromPartyRoom = 1393,
            ///<summary>$s1 has been kicked from the party room.</summary>
            S1KickedFromPartyRoom = 1394,
            ///<summary>The party room has been disbanded.</summary>
            PartyRoomDisbanded = 1395,
            ///<summary>The list of party rooms can only be viewed by a person who has not joined a party or who is currently the leader of a party.</summary>
            CantViewPartyRooms = 1396,
            ///<summary>The leader of the party room has changed.</summary>
            PartyRoomLeaderChanged = 1397,
            ///<summary>We are recruiting party members.</summary>
            RecruitingPartyMembers = 1398,
            ///<summary>Only the leader of the party can transfer party leadership to another player.</summary>
            OnlyAPartyLeaderCanTransferOnesRightsToAnotherPlayer = 1399,
            ///<summary>Please select the person you wish to make the party leader.</summary>
            PleaseSelectThePersonToWhomYouWouldLikeToTransferTheRightsOfAPartyLeader = 1400,
            ///<summary>Slow down.you are already the party leader.</summary>
            YouCannotTransferRightsToYourself = 1401,
            ///<summary>You may only transfer party leadership to another member of the party.</summary>
            YouCanTransferRightsOnlyToAnotherPartyMember = 1402,
            ///<summary>You have failed to transfer the party leadership.</summary>
            YouHaveFailedToTransferThePartyLeaderRights = 1403,
            ///<summary>The owner of the private manufacturing store has changed the price for creating this item. Please check the new price before trying again.</summary>
            ManufacturePriceHasChanged = 1404,
            ///<summary>$s1 CPs have been restored.</summary>
            S1CpWillBeRestored = 1405,
            ///<summary>$s2 CPs has been restored by $s1.</summary>
            S2CpWillBeRestoredByS1 = 1406,
            ///<summary>You are using a computer that does not allow you to log in with two accounts at the same time.</summary>
            NoLoginWithTwoAccounts = 1407,
            ///<summary>Your prepaid remaining usage time is $s1 hours and $s2 minutes. You have $s3 paid reservations left.</summary>
            PrepaidLeftS1S2S3 = 1408,
            ///<summary>Your prepaid usage time has expired. Your new prepaid reservation will be used. The remaining usage time is $s1 hours and $s2 minutes.</summary>
            PrepaidExpiredS1S2 = 1409,
            ///<summary>Your prepaid usage time has expired. You do not have any more prepaid reservations left.</summary>
            PrepaidExpired = 1410,
            ///<summary>The number of your prepaid reservations has changed.</summary>
            PrepaidChanged = 1411,
            ///<summary>Your prepaid usage time has $s1 minutes left.</summary>
            PrepaidLeftS1 = 1412,
            ///<summary>You do not meet the requirements to enter that party room.</summary>
            CantEnterPartyRoom = 1413,
            ///<summary>The width and length should be 100 or more grids and less than 5000 grids respectively.</summary>
            WrongGridCount = 1414,
            ///<summary>The command file is not sent.</summary>
            CommandFileNotSent = 1415,
            ///<summary>The representative of Team 1 has not been selected.</summary>
            Team1NoRepresentative = 1416,
            ///<summary>The representative of Team 2 has not been selected.</summary>
            Team2NoRepresentative = 1417,
            ///<summary>The name of Team 1 has not yet been chosen.</summary>
            Team1NoName = 1418,
            ///<summary>The name of Team 2 has not yet been chosen.</summary>
            Team2NoName = 1419,
            ///<summary>The name of Team 1 and the name of Team 2 are identical.</summary>
            TeamNameIdentical = 1420,
            ///<summary>The race setup file has not been designated.</summary>
            RaceSetupFile1 = 1421,
            ///<summary>Race setup file error - BuffCnt is not specified.</summary>
            RaceSetupFile2 = 1422,
            ///<summary>Race setup file error - BuffID$s1 is not specified.</summary>
            RaceSetupFile3 = 1423,
            ///<summary>Race setup file error - BuffLv$s1 is not specified.</summary>
            RaceSetupFile4 = 1424,
            ///<summary>Race setup file error - DefaultAllow is not specified.</summary>
            RaceSetupFile5 = 1425,
            ///<summary>Race setup file error - ExpSkillCnt is not specified.</summary>
            RaceSetupFile6 = 1426,
            ///<summary>Race setup file error - ExpSkillID$s1 is not specified.</summary>
            RaceSetupFile7 = 1427,
            ///<summary>Race setup file error - ExpItemCnt is not specified.</summary>
            RaceSetupFile8 = 1428,
            ///<summary>Race setup file error - ExpItemID$s1 is not specified.</summary>
            RaceSetupFile9 = 1429,
            ///<summary>Race setup file error - TeleportDelay is not specified.</summary>
            RaceSetupFile10 = 1430,
            ///<summary>The race will be stopped temporarily.</summary>
            RaceStoppedTemporarily = 1431,
            ///<summary>Your opponent is currently in a petrified state.</summary>
            OpponentPetrified = 1432,
            ///<summary>You will now automatically apply $s1 to your target.</summary>
            UseOfS1WillBeAuto = 1433,
            ///<summary>You will no longer automatically apply $s1 to your weapon.</summary>
            AutoUseOfS1Cancelled = 1434,
            ///<summary>Due to insufficient $s1, the automatic use function has been deactivated.</summary>
            AutoUseCancelledLackOfS1 = 1435,
            ///<summary>Due to insufficient $s1, the automatic use function cannot be activated.</summary>
            CannotAutoUseLackOfS1 = 1436,
            ///<summary>Players are no longer allowed to play dice. Dice can no longer be purchased from a village store. However, you can still sell them to any village store.</summary>
            DiceNoLongerAllowed = 1437,
            ///<summary>There is no skill that enables enchant.</summary>
            ThereIsNoSkillThatEnablesEnchant = 1438,
            ///<summary>You do not have all of the items needed to enchant that skill.</summary>
            YouDontHaveAllOfTheItemsNeededToEnchantThatSkill = 1439,
            ///<summary>You have succeeded in enchanting the skill $s1.</summary>
            YouHaveSucceededInEnchantingTheSkillS1 = 1440,
            ///<summary>Skill enchant failed. The skill will be initialized.</summary>
            YouHaveFailedToEnchantTheSkillS1 = 1441,
            ///<summary>You do not have enough SP to enchant that skill.</summary>
            YouDontHaveEnoughSpToEnchantThatSkill = 1443,
            ///<summary>You do not have enough experience (Exp) to enchant that skill.</summary>
            YouDontHaveEnoughExpToEnchantThatSkill = 1444,
            ///<summary>Your previous subclass will be removed and replaced with the new subclass at level 40. Do you wish to continue?.</summary>
            ReplaceSubclassConfirm = 1445,
            ///<summary>The ferry from $s1 to $s2 has been delayed.</summary>
            FerryFromS1ToS2Delayed = 1446,
            ///<summary>You cannot do that while fishing.</summary>
            CannotDoWhileFishing1 = 1447,
            ///<summary>Only fishing skills may be used at this time.</summary>
            OnlyFishingSkillsNow = 1448,
            ///<summary>You've got a bite!.</summary>
            GotABite = 1449,
            ///<summary>That fish is more determined than you are - it spit the hook!.</summary>
            FishSpitTheHook = 1450,
            ///<summary>Your bait was stolen by that fish!.</summary>
            BaitStolenByFish = 1451,
            ///<summary>Baits have been lost because the fish got away.</summary>
            BaitLostFishGotAway = 1452,
            ///<summary>You do not have a fishing pole equipped.</summary>
            FishingPoleNotEquipped = 1453,
            ///<summary>You must put bait on your hook before you can fish.</summary>
            BaitOnHookBeforeFishing = 1454,
            ///<summary>You cannot fish while under water.</summary>
            CannotFishUnderWater = 1455,
            ///<summary>You cannot fish while riding as a passenger of a boat - it's against the rules.</summary>
            CannotFishOnBoat = 1456,
            ///<summary>You can't fish here.</summary>
            CannotFishHere = 1457,
            ///<summary>Your attempt at fishing has been cancelled.</summary>
            FishingAttemptCancelled = 1458,
            ///<summary>You do not have enough bait.</summary>
            NotEnoughBait = 1459,
            ///<summary>You reel your line in and stop fishing.</summary>
            ReelLineAndStopFishing = 1460,
            ///<summary>You cast your line and start to fish.</summary>
            CastLineAndStartFishing = 1461,
            ///<summary>You may only use the Pumping skill while you are fishing.</summary>
            CanUsePumpingOnlyWhileFishing = 1462,
            ///<summary>You may only use the Reeling skill while you are fishing.</summary>
            CanUseReelingOnlyWhileFishing = 1463,
            ///<summary>The fish has resisted your attempt to bring it in.</summary>
            FishResistedAttemptToBringItIn = 1464,
            ///<summary>Your pumping is successful, causing $s1 damage.</summary>
            PumpingSuccesfulS1Damage = 1465,
            ///<summary>You failed to do anything with the fish and it regains $s1 HP.</summary>
            FishResistedPumpingS1HpRegained = 1466,
            ///<summary>You reel that fish in closer and cause $s1 damage.</summary>
            ReelingSuccesfulS1Damage = 1467,
            ///<summary>You failed to reel that fish in further and it regains $s1 HP.</summary>
            FishResistedReelingS1HpRegained = 1468,
            ///<summary>You caught something!.</summary>
            YouCaughtSomething = 1469,
            ///<summary>You cannot do that while fishing.</summary>
            CannotDoWhileFishing2 = 1470,
            ///<summary>You cannot do that while fishing.</summary>
            CannotDoWhileFishing3 = 1471,
            ///<summary>You look oddly at the fishing pole in disbelief and realize that you can't attack anything with this.</summary>
            CannotAttackWithFishingPole = 1472,
            ///<summary>$s1 is not sufficient.</summary>
            S1NotSufficient = 1473,
            ///<summary>$s1 is not available.</summary>
            S1NotAvailable = 1474,
            ///<summary>Pet has dropped $s1.</summary>
            PetDroppedS1 = 1475,
            ///<summary>Pet has dropped +$s1 $s2.</summary>
            PetDroppedS1S2 = 1476,
            ///<summary>Pet has dropped $s2 of $s1.</summary>
            PetDroppedS2S1S = 1477,
            ///<summary>You may only register a 64 x 64 pixel, 256-color BMP.</summary>
            Only64Pixel256ColorBmp = 1478,
            ///<summary>That is the wrong grade of soulshot for that fishing pole.</summary>
            WrongFishingshotGrade = 1479,
            ///<summary>Are you sure you want to remove yourself from the Grand Olympiad Games waiting list?.</summary>
            OlympiadRemoveConfirm = 1480,
            ///<summary>You have selected a class irrelevant individual match. Do you wish to participate?.</summary>
            OlympiadNonClassConfirm = 1481,
            ///<summary>You've selected to join a class specific game. Continue?.</summary>
            OlympiadClassConfirm = 1482,
            ///<summary>Are you ready to be a Hero?.</summary>
            HeroConfirm = 1483,
            ///<summary>Are you sure this is the Hero weapon you wish to use? Kamael race cannot use this.</summary>
            HeroWeaponConfirm = 1484,
            ///<summary>The ferry from Talking Island to Gludin Harbor has been delayed.</summary>
            FerryTalkingGludinDelayed = 1485,
            ///<summary>The ferry from Gludin Harbor to Talking Island has been delayed.</summary>
            FerryGludinTalkingDelayed = 1486,
            ///<summary>The ferry from Giran Harbor to Talking Island has been delayed.</summary>
            FerryGiranTalkingDelayed = 1487,
            ///<summary>The ferry from Talking Island to Giran Harbor has been delayed.</summary>
            FerryTalkingGiranDelayed = 1488,
            ///<summary>Innadril cruise service has been delayed.</summary>
            InnadrilBoatDelayed = 1489,
            ///<summary>Traded $s2 of crop $s1.</summary>
            TradedS2OfCropS1 = 1490,
            ///<summary>Failed in trading $s2 of crop $s1.</summary>
            FailedInTradingS2OfCropS1 = 1491,
            ///<summary>You will be moved to the Olympiad Stadium in $s1 second(s).</summary>
            YouWillEnterTheOlympiadStadiumInS1SecondS = 1492,
            ///<summary>Your opponent made haste with their tail between their legs, the match has been cancelled.</summary>
            TheGameHasBeenCancelledBecauseTheOtherPartyEndsTheGame = 1493,
            ///<summary>Your opponent does not meet the requirements to do battle, the match has been cancelled.</summary>
            TheGameHasBeenCancelledBecauseTheOtherPartyDoesNotMeetTheRequirementsForJoiningTheGame = 1494,
            ///<summary>The match will start in $s1 second(s).</summary>
            TheGameWillStartInS1SecondS = 1495,
            ///<summary>The match has started, fight!.</summary>
            StartsTheGame = 1496,
            ///<summary>Congratulations, $s1! You win the match!.</summary>
            S1HasWonTheGame = 1497,
            ///<summary>There is no victor, the match ends in a tie.</summary>
            TheGameEndedInATie = 1498,
            ///<summary>You will be moved back to town in $s1 second(s).</summary>
            YouWillBeMovedToTownInS1Seconds = 1499,
            ///<summary>You cannot participate in the Grand Olympiad Games with a character in their subclass.</summary>
            YouCantJoinTheOlympiadWithASubJobCharacter = 1500,
            ///<summary>Only Noblesse can participate in the Olympiad.</summary>
            OnlyNoblessCanParticipateInTheOlympiad = 1501,
            ///<summary>You have already been registered in a waiting list of an event.</summary>
            YouHaveAlreadyBeenRegisteredInAWaitingListOfAnEvent = 1502,
            ///<summary>You have been registered in the Grand Olympiad Games waiting list for a class specific match.</summary>
            YouHaveBeenRegisteredInAWaitingListOfClassifiedGames = 1503,
            ///<summary>You have registered on the waiting list for the non-class-limited individual match event.</summary>
            YouHaveBeenRegisteredInAWaitingListOfNoClassGames = 1504,
            ///<summary>You have been removed from the Grand Olympiad Games waiting list.</summary>
            YouHaveBeenDeletedFromTheWaitingListOfAGame = 1505,
            ///<summary>You are not currently registered on any Grand Olympiad Games waiting list.</summary>
            YouHaveNotBeenRegisteredInAWaitingListOfAGame = 1506,
            ///<summary>You cannot equip that item in a Grand Olympiad Games match.</summary>
            ThisItemCantBeEquippedForTheOlympiadEvent = 1507,
            ///<summary>You cannot use that item in a Grand Olympiad Games match.</summary>
            ThisItemIsNotAvailableForTheOlympiadEvent = 1508,
            ///<summary>You cannot use that skill in a Grand Olympiad Games match.</summary>
            ThisSkillIsNotAvailableForTheOlympiadEvent = 1509,
            ///<summary>$s1 is making an attempt at resurrection. Do you want to continue with this resurrection?.</summary>
            RessurectionRequestByS1 = 1510,
            ///<summary>While a pet is attempting to resurrect, it cannot help in resurrecting its master.</summary>
            MasterCannotRes = 1511,
            ///<summary>You cannot resurrect a pet while their owner is being resurrected.</summary>
            CannotResPet = 1512,
            ///<summary>Resurrection has already been proposed.</summary>
            ResHasAlreadyBeenProposed = 1513,
            ///<summary>You cannot the owner of a pet while their pet is being resurrected.</summary>
            CannotResMaster = 1514,
            ///<summary>A pet cannot be resurrected while it's owner is in the process of resurrecting.</summary>
            CannotResPet2 = 1515,
            ///<summary>The target is unavailable for seeding.</summary>
            TheTargetIsUnavailableForSeeding = 1516,
            ///<summary>Failed in Blessed Enchant. The enchant value of the item became 0.</summary>
            BlessedEnchantFailed = 1517,
            ///<summary>You do not meet the required condition to equip that item.</summary>
            CannotEquipItemDueToBadCondition = 1518,
            ///<summary>Your pet has been killed! Make sure you resurrect your pet within 20 minutes or your pet and all of it's items will disappear forever!.</summary>
            MakeSureYouRessurectYourPetWithin20Minutes = 1519,
            ///<summary>Servitor passed away.</summary>
            ServitorPassedAway = 1520,
            ///<summary>Your servitor has vanished! You'll need to summon a new one.</summary>
            YourServitorHasVanished = 1521,
            ///<summary>Your pet's corpse has decayed!.</summary>
            YourPetsCorpseHasDecayed = 1522,
            ///<summary>You should release your pet or servitor so that it does not fall off of the boat and drown!.</summary>
            ReleasePetOnBoat = 1523,
            ///<summary>$s1's pet gained $s2.</summary>
            S1PetGainedS2 = 1524,
            ///<summary>$s1's pet gained $s3 of $s2.</summary>
            S1PetGainedS3S2S = 1525,
            ///<summary>$s1's pet gained +$s2$s3.</summary>
            S1PetGainedS2S3 = 1526,
            ///<summary>Your pet was hungry so it ate $s1.</summary>
            PetTookS1BecauseHeWasHungry = 1527,
            ///<summary>You've sent a petition to the GM staff.</summary>
            SentPetitionToGm = 1528,
            ///<summary>$s1 is inviting you to the command channel. Do you want accept?.</summary>
            CommandChannelConfirmFromS1 = 1529,
            ///<summary>Select a target or enter the name.</summary>
            SelectTargetOrEnterName = 1530,
            ///<summary>Enter the name of the clan that you wish to declare war on.</summary>
            EnterClanNameToDeclareWar2 = 1531,
            ///<summary>Enter the name of the clan that you wish to have a cease-fire with.</summary>
            EnterClanNameToCeaseFire = 1532,
            ///<summary>Attention: $s1 has picked up $s2.</summary>
            AttentionS1PickedUpS2 = 1533,
            ///<summary>Attention: $s1 has picked up +$s2$s3.</summary>
            AttentionS1PickedUpS2S3 = 1534,
            ///<summary>Attention: $s1's pet has picked up $s2.</summary>
            AttentionS1PetPickedUpS2 = 1535,
            ///<summary>Attention: $s1's pet has picked up +$s2$s3.</summary>
            AttentionS1PetPickedUpS2S3 = 1536,
            ///<summary>Current Location: $s1, $s2, $s3 (near Rune Village).</summary>
            LocRuneS1S2S3 = 1537,
            ///<summary>Current Location: $s1, $s2, $s3 (near the Town of Goddard).</summary>
            LocGoddardS1S2S3 = 1538,
            ///<summary>Cargo has arrived at Talking Island Village.</summary>
            CargoAtTalkingVillage = 1539,
            ///<summary>Cargo has arrived at the Dark Elf Village.</summary>
            CargoAtDarkelfVillage = 1540,
            ///<summary>Cargo has arrived at Elven Village.</summary>
            CargoAtElvenVillage = 1541,
            ///<summary>Cargo has arrived at Orc Village.</summary>
            CargoAtOrcVillage = 1542,
            ///<summary>Cargo has arrived at Dwarfen Village.</summary>
            CargoAtDwarvenVillage = 1543,
            ///<summary>Cargo has arrived at Aden Castle Town.</summary>
            CargoAtAden = 1544,
            ///<summary>Cargo has arrived at Town of Oren.</summary>
            CargoAtOren = 1545,
            ///<summary>Cargo has arrived at Hunters Village.</summary>
            CargoAtHunters = 1546,
            ///<summary>Cargo has arrived at the Town of Dion.</summary>
            CargoAtDion = 1547,
            ///<summary>Cargo has arrived at Floran Village.</summary>
            CargoAtFloran = 1548,
            ///<summary>Cargo has arrived at Gludin Village.</summary>
            CargoAtGludin = 1549,
            ///<summary>Cargo has arrived at the Town of Gludio.</summary>
            CargoAtGludio = 1550,
            ///<summary>Cargo has arrived at Giran Castle Town.</summary>
            CargoAtGiran = 1551,
            ///<summary>Cargo has arrived at Heine.</summary>
            CargoAtHeine = 1552,
            ///<summary>Cargo has arrived at Rune Village.</summary>
            CargoAtRune = 1553,
            ///<summary>Cargo has arrived at the Town of Goddard.</summary>
            CargoAtGoddard = 1554,
            ///<summary>Do you want to cancel character deletion?.</summary>
            CancelCharacterDeletionConfirm = 1555,
            ///<summary>Your clan notice has been saved.</summary>
            ClanNoticeSaved = 1556,
            ///<summary>Seed price should be more than $s1 and less than $s2.</summary>
            SeedPriceShouldBeMoreThanS1AndLessThanS2 = 1557,
            ///<summary>The quantity of seed should be more than $s1 and less than $s2.</summary>
            TheQuantityOfSeedShouldBeMoreThanS1AndLessThanS2 = 1558,
            ///<summary>Crop price should be more than $s1 and less than $s2.</summary>
            CropPriceShouldBeMoreThanS1AndLessThanS2 = 1559,
            ///<summary>The quantity of crop should be more than $s1 and less than $s2.</summary>
            TheQuantityOfCropShouldBeMoreThanS1AndLessThanS2 = 1560,
            ///<summary>The clan, $s1, has declared a Clan War.</summary>
            ClanS1DeclaredWar = 1561,
            ///<summary>A Clan War has been declared against the clan, $s1. you will only lose a quarter of the normal experience from death.</summary>
            ClanWarDeclaredAgainstS1IfKilledLoseLowExp = 1562,
            ///<summary>The clan, $s1, cannot declare a Clan War because their clan is less than level three, and or they do not have enough members.</summary>
            S1ClanCannotDeclareWarTooLowLevelOrNotEnoughMembers = 1563,
            ///<summary>A Clan War can be declared only if the clan is level three or above, and the number of clan members is fifteen or greater.</summary>
            ClanWarDeclaredIfClanLvl3Or15Member = 1564,
            ///<summary>A Clan War cannot be declared against a clan that does not exist!.</summary>
            ClanWarCannotDeclaredClanNotExist = 1565,
            ///<summary>The clan, $s1, has decided to stop the war.</summary>
            ClanS1HasDecidedToStop = 1566,
            ///<summary>The war against $s1 Clan has been stopped.</summary>
            WarAgainstS1HasStopped = 1567,
            ///<summary>The target for declaration is wrong.</summary>
            WrongDeclarationTarget = 1568,
            ///<summary>A declaration of Clan War against an allied clan can't be made.</summary>
            ClanWarAgainstAAlliedClanNotWork = 1569,
            ///<summary>A declaration of war against more than 30 Clans can't be made at the same time.</summary>
            TooManyClanWars = 1570,
            ///<summary>======Clans You've Declared War On======.</summary>
            ClansYouDeclaredWarOn = 1571,
            ///<summary>======Clans That Have Declared War On You======.</summary>
            ClansThatHaveDeclaredWarOnYou = 1572,
            ///<summary>There are no clans that your clan has declared war against.</summary>
            YouArentInClanWars = 1573,
            ///<summary>All is well. There are no clans that have declared war against your clan.</summary>
            NoClanWarsVsYou = 1574,
            ///<summary>Command Channels can only be formed by a party leader who is also the leader of a level 5 clan.</summary>
            CommandChannelOnlyByLevel5ClanLeaderPartyLeader = 1575,
            ///<summary>Pet uses the power of spirit.</summary>
            PetUseThePowerOfSpirit = 1576,
            ///<summary>Servitor uses the power of spirit.</summary>
            ServitorUseThePowerOfSpirit = 1577,
            ///<summary>Items are not available for a private store or a private manufacture.</summary>
            ItemsUnavailableForStoreManufacture = 1578,
            ///<summary>$s1's pet gained $s2 adena.</summary>
            S1PetGainedS2Adena = 1579,
            ///<summary>The Command Channel has been formed.</summary>
            CommandChannelFormed = 1580,
            ///<summary>The Command Channel has been disbanded.</summary>
            CommandChannelDisbanded = 1581,
            ///<summary>You have joined the Command Channel.</summary>
            JoinedCommandChannel = 1582,
            ///<summary>You were dismissed from the Command Channel.</summary>
            DismissedFromCommandChannel = 1583,
            ///<summary>$s1's party has been dismissed from the Command Channel.</summary>
            S1PartyDismissedFromCommandChannel = 1584,
            ///<summary>The Command Channel has been disbanded.</summary>
            CommandChannelDisbanded2 = 1585,
            ///<summary>You have quit the Command Channel.</summary>
            LeftCommandChannel = 1586,
            ///<summary>$s1's party has left the Command Channel.</summary>
            S1PartyLeftCommandChannel = 1587,
            ///<summary>The Command Channel is activated only when there are at least 5 parties participating.</summary>
            CommandChannelOnlyAtLeast5Parties = 1588,
            ///<summary>Command Channel authority has been transferred to $s1.</summary>
            CommandChannelLeaderNowS1 = 1589,
            ///<summary>===Guild Info (Total Parties: $s1)===.</summary>
            GuildInfoHeader = 1590,
            ///<summary>No user has been invited to the Command Channel.</summary>
            NoUserInvitedToCommandChannel = 1591,
            ///<summary>You can no longer set up a Command Channel.</summary>
            CannotLongerSetupCommandChannel = 1592,
            ///<summary>You do not have authority to invite someone to the Command Channel.</summary>
            CannotInviteToCommandChannel = 1593,
            ///<summary>$s1's party is already a member of the Command Channel.</summary>
            S1AlreadyMemberOfCommandChannel = 1594,
            ///<summary>$s1 has succeeded.</summary>
            S1Succeeded = 1595,
            ///<summary>You were hit by $s1!.</summary>
            HitByS1 = 1596,
            ///<summary>$s1 has failed.</summary>
            S1Failed = 1597,
            ///<summary>Soulshots and spiritshots are not available for a dead pet or servitor. Sad, isn't it?.</summary>
            SoulshotsAndSpiritshotsAreNotAvailableForADeadPet = 1598,
            ///<summary>You cannot observe while you are in combat!.</summary>
            CannotObserveInCombat = 1599,
            ///<summary>Tomorrow's items will ALL be set to 0. Do you wish to continue?.</summary>
            TomorrowItemZeroConfirm = 1600,
            ///<summary>Tomorrow's items will all be set to the same value as today's items. Do you wish to continue?.</summary>
            TomorrowItemSameConfirm = 1601,
            ///<summary>Only a party leader can access the Command Channel.</summary>
            CommandChannelOnlyForPartyLeader = 1602,
            ///<summary>Only channel operator can give All Command.</summary>
            OnlyCommanderGiveCommand = 1603,
            ///<summary>While dressed in formal wear, you can't use items that require all skills and casting operations.</summary>
            CannotUseItemsSkillsWithFormalwear = 1604,
            ///<summary>* Here, you can buy only seeds of $s1 Manor.</summary>
            HereYouCanBuyOnlySeedsOfS1Manor = 1605,
            ///<summary>Congratulations - You've completed the third-class transfer quest!.</summary>
            ThirdClassTransfer = 1606,
            ///<summary>$s1 adena has been withdrawn to pay for purchasing fees.</summary>
            S1AdenaHasBeenWithdrawnToPayForPurchasingFees = 1607,
            ///<summary>Due to insufficient adena you cannot buy another castle.</summary>
            InsufficientAdenaToBuyCastle = 1608,
            ///<summary>War has already been declared against that clan... but I'll make note that you really don't like them.</summary>
            WarAlreadyDeclared = 1609,
            ///<summary>Fool! You cannot declare war against your own clan!.</summary>
            CannotDeclareAgainstOwnClan = 1610,
            ///<summary>Leader: $s1.</summary>
            PartyLeaderS1 = 1611,
            ///<summary>=====War List=====.</summary>
            WarList = 1612,
            ///<summary>There is no clan listed on War List.</summary>
            NoClanOnWarList = 1613,
            ///<summary>You have joined a channel that was already open.</summary>
            JoinedChannelAlreadyOpen = 1614,
            ///<summary>The number of remaining parties is $s1 until a channel is activated.</summary>
            S1PartiesRemainingUntilChannel = 1615,
            ///<summary>The Command Channel has been activated.</summary>
            CommandChannelActivated = 1616,
            ///<summary>You do not have the authority to use the Command Channel.</summary>
            CantUseCommandChannel = 1617,
            ///<summary>The ferry from Rune Harbor to Gludin Harbor has been delayed.</summary>
            FerryRuneGludinDelayed = 1618,
            ///<summary>The ferry from Gludin Harbor to Rune Harbor has been delayed.</summary>
            FerryGludinRuneDelayed = 1619,
            ///<summary>Arrived at Rune Harbor.</summary>
            ArrivedAtRune = 1620,
            ///<summary>Departure for Gludin Harbor will take place in five minutes!.</summary>
            DepartureForGludin5Minutes = 1621,
            ///<summary>Departure for Gludin Harbor will take place in one minute!.</summary>
            DepartureForGludin1Minute = 1622,
            ///<summary>Make haste! We will be departing for Gludin Harbor shortly.</summary>
            DepartureForGludinShortly = 1623,
            ///<summary>We are now departing for Gludin Harbor Hold on and enjoy the ride!.</summary>
            DepartureForGludinNow = 1624,
            ///<summary>Departure for Rune Harbor will take place after anchoring for ten minutes.</summary>
            DepartureForRune10Minutes = 1625,
            ///<summary>Departure for Rune Harbor will take place in five minutes!.</summary>
            DepartureForRune5Minutes = 1626,
            ///<summary>Departure for Rune Harbor will take place in one minute!.</summary>
            DepartureForRune1Minute = 1627,
            ///<summary>Make haste! We will be departing for Gludin Harbor shortly.</summary>
            DepartureForGludinShortly2 = 1628,
            ///<summary>We are now departing for Rune Harbor Hold on and enjoy the ride!.</summary>
            DepartureForRuneNow = 1629,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 15 minutes.</summary>
            FerryFromRuneAtGludin15Minutes = 1630,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.</summary>
            FerryFromRuneAtGludin10Minutes = 1631,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.</summary>
            FerryFromRuneAtGludin5Minutes = 1632,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 1 minute.</summary>
            FerryFromRuneAtGludin1Minute = 1633,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 15 minutes.</summary>
            FerryFromGludinAtRune15Minutes = 1634,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune harbor in approximately 10 minutes.</summary>
            FerryFromGludinAtRune10Minutes = 1635,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 10 minutes.</summary>
            FerryFromGludinAtRune5Minutes = 1636,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 1 minute.</summary>
            FerryFromGludinAtRune1Minute = 1637,
            ///<summary>You cannot fish while using a recipe book, private manufacture or private store.</summary>
            CannotFishWhileUsingRecipeBook = 1638,
            ///<summary>Period $s1 of the Grand Olympiad Games has started!.</summary>
            OlympiadPeriodS1HasStarted = 1639,
            ///<summary>Period $s1 of the Grand Olympiad Games has now ended.</summary>
            OlympiadPeriodS1HasEnded = 1640,
            ///<summary>and make haste to a Grand Olympiad Manager! Battles in the Grand Olympiad Games are now taking place!.</summary>
            TheOlympiadGameHasStarted = 1641,
            ///<summary>Much carnage has been left for the cleanup crew of the Olympiad Stadium. Battles in the Grand Olympiad Games are now over!.</summary>
            TheOlympiadGameHasEnded = 1642,
            ///<summary>Current Location: $s1, $s2, $s3 (Dimensional Gap).</summary>
            LocDimensionalGapS1S2S3 = 1643,
            ///<summary>Play time is now accumulating.</summary>
            PlayTimeNowAccumulating = 1649,
            ///<summary>Due to high server traffic, your login attempt has failed. Please try again soon.</summary>
            TryLoginLater = 1650,
            ///<summary>The Grand Olympiad Games are not currently in progress.</summary>
            TheOlympiadGameIsNotCurrentlyInProgress = 1651,
            ///<summary>You are now recording gameplay.</summary>
            RecordingGameplayStart = 1652,
            ///<summary>Your recording has been successfully stored. ($s1).</summary>
            RecordingGameplayStopS1 = 1653,
            ///<summary>Your attempt to record the replay file has failed.</summary>
            RecordingGameplayFailed = 1654,
            ///<summary>You caught something smelly and scary, maybe you should throw it back!?.</summary>
            YouCaughtSomethingSmellyThrowItBack = 1655,
            ///<summary>You have successfully traded the item with the NPC.</summary>
            SuccessfullyTradedWithNpc = 1656,
            ///<summary>$s1 has earned $s2 points in the Grand Olympiad Games.</summary>
            S1HasGainedS2OlympiadPoints = 1657,
            ///<summary>$s1 has lost $s2 points in the Grand Olympiad Games.</summary>
            S1HasLostS2OlympiadPoints = 1658,
            ///<summary>Current Location: $s1, $s2, $s3 (Cemetery of the Empire).</summary>
            LocCemetaryOfTheEmpireS1S2S3 = 1659,
            ///<summary>Channel Creator: $s1.</summary>
            ChannelCreatorS1 = 1660,
            ///<summary>$s1 has obtained $s3 $s2s.</summary>
            S1ObtainedS3S2S = 1661,
            ///<summary>The fish are no longer biting here because you've caught too many! Try fishing in another location.</summary>
            FishNoMoreBitingTryOtherLocation = 1662,
            ///<summary>The clan crest was successfully registered. Remember, only a clan that owns a clan hall or castle can have their crest displayed.</summary>
            ClanEmblemWasSuccessfullyRegistered = 1663,
            ///<summary>The fish is resisting your efforts to haul it in! Look at that bobber go!.</summary>
            FishResistingLookBobbler = 1664,
            ///<summary>You've worn that fish out! It can't even pull the bobber under the water!.</summary>
            YouWornFishOut = 1665,
            ///<summary>You have obtained +$s1 $s2.</summary>
            ObtainedS1S2 = 1666,
            ///<summary>Lethal Strike!.</summary>
            LethalStrike = 1667,
            ///<summary>Your lethal strike was successful!.</summary>
            LethalStrikeSuccessful = 1668,
            ///<summary>There was nothing found inside of that.</summary>
            NothingInsideThat = 1669,
            ///<summary>Due to your Reeling and/or Pumping skill being three or more levels higher than your Fishing skill, a 50 damage penalty will be applied.</summary>
            ReelingPumping3LevelsHigherThanFishingPenalty = 1670,
            ///<summary>Your reeling was successful! (Mastery Penalty:$s1 ).</summary>
            ReelingSuccessfulPenaltyS1 = 1671,
            ///<summary>Your pumping was successful! (Mastery Penalty:$s1 ).</summary>
            PumpingSuccessfulPenaltyS1 = 1672,
            ///<summary>Your current record for this Grand Olympiad is $s1 match(es), $s2 win(s) and $s3 defeat(s). You have earned $s4 Olympiad Point(s).</summary>
            TheCurrentRecordForThisOlympiadSessionIsS1MatchesS2WinsS3DefeatsYouHaveEarnedS4OlympiadPoints = 1673,
            ///<summary>This command can only be used by a Noblesse.</summary>
            NoblesseOnly = 1674,
            ///<summary>A manor cannot be set up between 6 a.m. and 8 p.m.</summary>
            AManorCannotBeSetUpBetween6AmAnd8Pm = 1675,
            ///<summary>You do not have a servitor or pet and therefore cannot use the automatic-use function.</summary>
            NoServitorCannotAutomateUse = 1676,
            ///<summary>A cease-fire during a Clan War can not be called while members of your clan are engaged in battle.</summary>
            CantStopClanWarWhileInCombat = 1677,
            ///<summary>You have not declared a Clan War against the clan $s1.</summary>
            NoClanWarAgainstClanS1 = 1678,
            ///<summary>Only the creator of a channel can issue a global command.</summary>
            OnlyChannelCreatorCanGlobalCommand = 1679,
            ///<summary>$s1 has declined the channel invitation.</summary>
            S1DeclinedChannelInvitation = 1680,
            ///<summary>Since $s1 did not respond, your channel invitation has failed.</summary>
            S1DidNotRespondChannelInvitationFailed = 1681,
            ///<summary>Only the creator of a channel can use the channel dismiss command.</summary>
            OnlyChannelCreatorCanDismiss = 1682,
            ///<summary>Only a party leader can choose the option to leave a channel.</summary>
            OnlyPartyLeaderCanLeaveChannel = 1683,
            ///<summary>A Clan War can not be declared against a clan that is being dissolved.</summary>
            NoClanWarAgainstDissolvingClan = 1684,
            ///<summary>You are unable to equip this item when your PK count is greater or equal to one.</summary>
            YouAreUnableToEquipThisItemWhenYourPkCountIsGreaterThanOrEqualToOne = 1685,
            ///<summary>Stones and mortar tumble to the earth - the castle wall has taken damage!.</summary>
            CastleWallDamaged = 1686,
            ///<summary>This area cannot be entered while mounted atop of a Wyvern. You will be dismounted from your Wyvern if you do not leave!.</summary>
            AreaCannotBeEnteredWhileMountedWyvern = 1687,
            ///<summary>You cannot enchant while operating a Private Store or Private Workshop.</summary>
            CannotEnchantWhileStore = 1688,
            ///<summary>You have already joined the waiting list for a class specific match.</summary>
            YouAreAlreadyOnTheWaitingListToParticipateInTheGameForYourClass = 1689,
            ///<summary>You have already joined the waiting list for a non-class specific match.</summary>
            YouAreAlreadyOnTheWaitingListForAllClassesWaitingToParticipateInTheGame = 1690,
            ///<summary>You can't join a Grand Olympiad Game match with that much stuff on you! Reduce your weight to below 80 percent full and request to join again!.</summary>
            Since80PercentOrMoreOfYourInventorySlotsAreFullYouCannotParticipateInTheOlympiad = 1691,
            ///<summary>You have changed from your main class to a subclass and therefore are removed from the Grand Olympiad Games waiting list.</summary>
            SinceYouHaveChangedYourClassIntoASubJobYouCannotParticipateInTheOlympiad = 1692,
            ///<summary>You may not observe a Grand Olympiad Games match while you are on the waiting list.</summary>
            WhileYouAreOnTheWaitingListYouAreNotAllowedToWatchTheGame = 1693,
            ///<summary>Only a clan leader that is a Noblesse can view the Siege War Status window during a siege war.</summary>
            OnlyNoblesseLeaderCanViewSiegeStatusWindow = 1694,
            ///<summary>You can only use that during a Siege War!.</summary>
            OnlyDuringSiege = 1695,
            ///<summary>Your accumulated play time is $s1.</summary>
            AccumulatedPlayTimeIsS1 = 1696,
            ///<summary>Your accumulated play time has reached Fatigue level, so you will receive experience or item drops at only 50 percent [...].</summary>
            AccumulatedPlayTimeWarning1 = 1697,
            ///<summary>Your accumulated play time has reached Ill-health level, so you will no longer gain experience or item drops. [...}.</summary>
            AccumulatedPlayTimeWarning2 = 1698,
            ///<summary>You cannot dismiss a party member by force.</summary>
            CannotDismissPartyMember = 1699,
            ///<summary>You don't have enough spiritshots needed for a pet/servitor.</summary>
            NotEnoughSpiritshotsForPet = 1700,
            ///<summary>You don't have enough soulshots needed for a pet/servitor.</summary>
            NotEnoughSoulshotsForPet = 1701,
            ///<summary>$s1 is using a third party program.</summary>
            S1UsingThirdPartyProgram = 1702,
            ///<summary>The previous investigated user is not using a third party program.</summary>
            NotUsingThirdPartyProgram = 1703,
            ///<summary>Please close the setup window for your private manufacturing store or private store, and try again.</summary>
            CloseStoreWindowAndTryAgain = 1704,
            ///<summary>PC Bang Points acquisition period. Points acquisition period left $s1 hour.</summary>
            PcpointAcquisitionPeriod = 1705,
            ///<summary>PC Bang Points use period. Points acquisition period left $s1 hour.</summary>
            PcpointUsePeriod = 1706,
            ///<summary>You acquired $s1 PC Bang Point.</summary>
            AcquiredS1Pcpoint = 1707,
            ///<summary>Double points! You acquired $s1 PC Bang Point.</summary>
            AcquiredS1PcpointDouble = 1708,
            ///<summary>You are using $s1 point.</summary>
            UsingS1Pcpoint = 1709,
            ///<summary>You are short of accumulated points.</summary>
            ShortOfAccumulatedPoints = 1710,
            ///<summary>PC Bang Points use period has expired.</summary>
            PcpointUsePeriodExpired = 1711,
            ///<summary>The PC Bang Points accumulation period has expired.</summary>
            PcpointAccumulationPeriodExpired = 1712,
            ///<summary>The games may be delayed due to an insufficient number of players waiting.</summary>
            GamesDelayed = 1713,
            ///<summary>Current Location: $s1, $s2, $s3 (Near the Town of Schuttgart).</summary>
            LocSchuttgartS1S2S3 = 1714,
            ///<summary>This is a Peaceful Zone.</summary>
            PeacefulZone = 1715,
            ///<summary>Altered Zone.</summary>
            AlteredZone = 1716,
            ///<summary>Siege War Zone.</summary>
            SiegeZone = 1717,
            ///<summary>General Field.</summary>
            GeneralZone = 1718,
            ///<summary>Seven Signs Zone.</summary>
            SevensignsZone = 1719,
            ///<summary>---.</summary>
            Unknown1 = 1720,
            ///<summary>Combat Zone.</summary>
            CombatZone = 1721,
            ///<summary>Please enter the name of the item you wish to search for.</summary>
            EnterItemNameSearch = 1722,
            ///<summary>Please take a moment to provide feedback about the petition service.</summary>
            PleaseProvidePetitionFeedback = 1723,
            ///<summary>A servitor whom is engaged in battle cannot be de-activated.</summary>
            ServitorNotReturnInBattle = 1724,
            ///<summary>You have earned $s1 raid point(s).</summary>
            EarnedS1RaidPoints = 1725,
            ///<summary>$s1 has disappeared because its time period has expired.</summary>
            S1PeriodExpiredDisappeared = 1726,
            ///<summary>$s1 has invited you to a party room. Do you accept?.</summary>
            S1InvitedYouToPartyRoomConfirm = 1727,
            ///<summary>The recipient of your invitation did not accept the party matching invitation.</summary>
            PartyMatchingRequestNoResponse = 1728,
            ///<summary>You cannot join a Command Channel while teleporting.</summary>
            NotJoinChannelWhileTeleporting = 1729,
            ///<summary>To establish a Clan Academy, your clan must be Level 5 or higher.</summary>
            YouDoNotMeetCriteriaInOrderToCreateAClanAcademy = 1730,
            ///<summary>Only the leader can create a Clan Academy.</summary>
            OnlyLeaderCanCreateAcademy = 1731,
            ///<summary>To create a Clan Academy, a Blood Mark is needed.</summary>
            NeedBloodmarkForAcademy = 1732,
            ///<summary>You do not have enough adena to create a Clan Academy.</summary>
            NeedAdenaForAcademy = 1733,
            ///<summary>not belong another clan and not yet completed their 2nd class transfer.</summary>
            AcademyRequirements = 1734,
            ///<summary>$s1 does not meet the requirements to join a Clan Academy.</summary>
            S1DoesnotMeetRequirementsToJoinAcademy = 1735,
            ///<summary>The Clan Academy has reached its maximum enrollment.</summary>
            AcademyMaximum = 1736,
            ///<summary>Your clan has not established a Clan Academy but is eligible to do so.</summary>
            ClanCanCreateAcademy = 1737,
            ///<summary>Your clan has already established a Clan Academy.</summary>
            ClanHasAlreadyEstablishedAClanAcademy = 1738,
            ///<summary>Would you like to create a Clan Academy?.</summary>
            ClanAcademyCreateConfirm = 1739,
            ///<summary>Please enter the name of the Clan Academy.</summary>
            AcademyCreateEnterName = 1740,
            ///<summary>Congratulations! The $s1's Clan Academy has been created.</summary>
            TheS1SClanAcademyHasBeenCreated = 1741,
            ///<summary>A message inviting $s1 to join the Clan Academy is being sent.</summary>
            AcademyInvitationSentToS1 = 1742,
            ///<summary>To open a Clan Academy, the leader of a Level 5 clan or above must pay XX Proofs of Blood or a certain amount of adena.</summary>
            OpenAcademyConditions = 1743,
            ///<summary>There was no response to your invitation to join the Clan Academy, so the invitation has been rescinded.</summary>
            AcademyJoinNoResponse = 1744,
            ///<summary>The recipient of your invitation to join the Clan Academy has declined.</summary>
            AcademyJoinDecline = 1745,
            ///<summary>You have already joined a Clan Academy.</summary>
            AlreadyJoinedAcademy = 1746,
            ///<summary>$s1 has sent you an invitation to join the Clan Academy belonging to the $s2 clan. Do you accept?.</summary>
            JoinAcademyRequestByS1ForClanS2 = 1747,
            ///<summary>Clan Academy member $s1 has successfully completed the 2nd class transfer and obtained $s2 Clan Reputation points.</summary>
            ClanMemberGraduatedFromAcademy = 1748,
            ///<summary>Congratulations! You will now graduate from the Clan Academy and leave your current clan. As a graduate of the academy, you can immediately join a clan as a regular member without being subject to any penalties.</summary>
            AcademyMembershipTerminated = 1749,
            ///<summary>If you possess $s1, you cannot participate in the Olympiad.</summary>
            CannotJoinOlympiadPossessingS1 = 1750,
            ///<summary>The Grand Master has given you a commemorative item.</summary>
            GrandMasterCommemorativeItem = 1751,
            ///<summary>Since the clan has received a graduate of the Clan Academy, it has earned $s1 points towards its reputation score.</summary>
            MemberGraduatedEarnedS1Repu = 1752,
            ///<summary>The clan leader has decreed that that particular privilege cannot be granted to a Clan Academy member.</summary>
            CantTransferPrivilegeToAcademyMember = 1753,
            ///<summary>That privilege cannot be granted to a Clan Academy member.</summary>
            RightCantTransferredToAcademyMember = 1754,
            ///<summary>$s2 has been designated as the apprentice of clan member $s1.</summary>
            S2HasBeenDesignatedAsApprenticeOfClanMemberS1 = 1755,
            ///<summary>Your apprentice, $s1, has logged in.</summary>
            YourApprenticeS1HasLoggedIn = 1756,
            ///<summary>Your apprentice, $s1, has logged out.</summary>
            YourApprenticeS1HasLoggedOut = 1757,
            ///<summary>Your sponsor, $s1, has logged in.</summary>
            YourSponsorS1HasLoggedIn = 1758,
            ///<summary>Your sponsor, $s1, has logged out.</summary>
            YourSponsorS1HasLoggedOut = 1759,
            ///<summary>Clan member $s1's name title has been changed to $2.</summary>
            ClanMemberS1TitleChangedToS2 = 1760,
            ///<summary>Clan member $s1's privilege level has been changed to $s2.</summary>
            ClanMemberS1PrivilegeChangedToS2 = 1761,
            ///<summary>You do not have the right to dismiss an apprentice.</summary>
            YouDoNotHaveTheRightToDismissAnApprentice = 1762,
            ///<summary>$s2, clan member $s1's apprentice, has been removed.</summary>
            S2ClanMemberS1ApprenticeHasBeenRemoved = 1763,
            ///<summary>This item can only be worn by a member of the Clan Academy.</summary>
            EquipOnlyForAcademy = 1764,
            ///<summary>As a graduate of the Clan Academy, you can no longer wear this item.</summary>
            EquipNotForGraduates = 1765,
            ///<summary>An application to join the clan has been sent to $s1 in $s2.</summary>
            ClanJoinApplicationSentToS1InS2 = 1766,
            ///<summary>An application to join the clan Academy has been sent to $s1.</summary>
            AcademyJoinApplicationSentToS1 = 1767,
            ///<summary>$s1 has invited you to join the Clan Academy of $s2 clan. Would you like to join?.</summary>
            JoinRequestByS1ToClanS2Academy = 1768,
            ///<summary>$s1 has sent you an invitation to join the $s3 Order of Knights under the $s2 clan. Would you like to join?.</summary>
            JoinRequestByS1ToOrderOfKnightsS3UnderClanS2 = 1769,
            ///<summary>The clan's reputation score has dropped below 0. The clan may face certain penalties as a result.</summary>
            ClanRepu0MayFacePenalties = 1770,
            ///<summary>Now that your clan level is above Level 5, it can accumulate clan reputation points.</summary>
            ClanCanAccumulateClanReputationPoints = 1771,
            ///<summary>Since your clan was defeated in a siege, $s1 points have been deducted from your clan's reputation score and given to the opposing clan.</summary>
            ClanWasDefeatedInSiegeAndLostS1ReputationPoints = 1772,
            ///<summary>Since your clan emerged victorious from the siege, $s1 points have been added to your clan's reputation score.</summary>
            ClanVictoriousInSiegeAndGainedS1ReputationPoints = 1773,
            ///<summary>Your clan's newly acquired contested clan hall has added $s1 points to your clan's reputation score.</summary>
            ClanAcquiredContestedClanHallAndS1ReputationPoints = 1774,
            ///<summary>Clan member $s1 was an active member of the highest-ranked party in the Festival of Darkness. $s2 points have been added to your clan's reputation score.</summary>
            ClanMemberS1WasInHighestRankedPartyInFestivalOfDarknessAndGainedS2Reputation = 1775,
            ///<summary>Clan member $s1 was named a hero. $2s points have been added to your clan's reputation score.</summary>
            ClanMemberS1BecameHeroAndGainedS2ReputationPoints = 1776,
            ///<summary>You have successfully completed a clan quest. $s1 points have been added to your clan's reputation score.</summary>
            ClanQuestCompletedAndS1PointsGained = 1777,
            ///<summary>An opposing clan has captured your clan's contested clan hall. $s1 points have been deducted from your clan's reputation score.</summary>
            OpposingClanCapturedClanHallAndYourClanLosesS1Points = 1778,
            ///<summary>After losing the contested clan hall, 300 points have been deducted from your clan's reputation score.</summary>
            ClanLostContestedClanHallAnd300Points = 1779,
            ///<summary>Your clan has captured your opponent's contested clan hall. $s1 points have been deducted from your opponent's clan reputation score.</summary>
            ClanCapturedContestedClanHallAndS1PointsDeductedFromOpponent = 1780,
            ///<summary>Your clan has added $1s points to its clan reputation score.</summary>
            ClanAddedS1SPointsToReputationScore = 1781,
            ///<summary>Your clan member $s1 was killed. $s2 points have been deducted from your clan's reputation score and added to your opponent's clan reputation score.</summary>
            ClanMemberS1WasKilledAndS2PointsDeductedFromReputation = 1782,
            ///<summary>For killing an opposing clan member, $s1 points have been deducted from your opponents' clan reputation score.</summary>
            ForKillingOpposingMemberS1PointsWereDeductedFromOpponents = 1783,
            ///<summary>Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score and added to your opponents'.</summary>
            YourClanFailedToDefendCastleAndS1PointsLostAndAddedToOpponent = 1784,
            ///<summary>The clan you belong to has been initialized. $s1 points have been deducted from your clan reputation score.</summary>
            YourClanHasBeenInitializedAndS1PointsLost = 1785,
            ///<summary>Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score.</summary>
            YourClanFailedToDefendCastleAndS1PointsLost = 1786,
            ///<summary>$s1 points have been deducted from the clan's reputation score.</summary>
            S1DeductedFromClanRep = 1787,
            ///<summary>The clan skill $s1 has been added.</summary>
            ClanSkillS1Added = 1788,
            ///<summary>Since the Clan Reputation Score has dropped to 0 or lower, your clan skill(s) will be de-activated.</summary>
            ReputationPoints0OrLowerClanSkillsDeactivated = 1789,
            ///<summary>The conditions necessary to increase the clan's level have not been met.</summary>
            FailedToIncreaseClanLevel = 1790,
            ///<summary>The conditions necessary to create a military unit have not been met.</summary>
            YouDoNotMeetCriteriaInOrderToCreateAMilitaryUnit = 1791,
            ///<summary>Please assign a manager for your new Order of Knights.</summary>
            AssignManagerForOrderOfKnights = 1792,
            ///<summary>$s1 has been selected as the captain of $s2.</summary>
            S1HasBeenSelectedAsCaptainOfS2 = 1793,
            ///<summary>The Knights of $s1 have been created.</summary>
            TheKnightsOfS1HaveBeenCreated = 1794,
            ///<summary>The Royal Guard of $s1 have been created.</summary>
            TheRoyalGuardOfS1HaveBeenCreated = 1795,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse17 = 1796,
            ///<summary>$s1 has been promoted to $s2.</summary>
            S1PromotedToS2 = 1797,
            ///<summary>Clan lord privileges have been transferred to $s1.</summary>
            ClanLeaderPrivilegesHaveBeenTransferredToS1 = 1798,
            ///<summary>We are searching for BOT users. Please try again later.</summary>
            SearchingForBotUsersTryAgainLater = 1799,
            ///<summary>User $s1 has a history of using BOT.</summary>
            S1HistoryUsingBot = 1800,
            ///<summary>The attempt to sell has failed.</summary>
            SellAttemptFailed = 1801,
            ///<summary>The attempt to trade has failed.</summary>
            TradeAttemptFailed = 1802,
            ///<summary>The request to participate in the game cannot be made starting from 10 minutes before the end of the game.</summary>
            GameRequestCannotBeMade = 1803,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse18 = 1804,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse19 = 1805,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse20 = 1806,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse21 = 1807,
            ///<summary>Your account has been suspended .</summary>
            IllegalUse22 = 1808,
            ///<summary>please visit the PlayNC website (http://www.plaync.com/us/support/).</summary>
            AccountMustVerified = 1809,
            ///<summary>The refuse invitation state has been activated.</summary>
            RefuseInvitationActivated = 1810,
            ///<summary>Since the refuse invitation state is currently activated, no invitation can be made.</summary>
            RefuseInvitationCurrentlyActive = 1812,
            ///<summary>$s1 has $s2 hour(s) of usage time remaining.</summary>
            S2HourOfUsageTimeAreLeftForS1 = 1813,
            ///<summary>$s1 has $s2 minute(s) of usage time remaining.</summary>
            S2MinuteOfUsageTimeAreLeftForS1 = 1814,
            ///<summary>$s2 was dropped in the $s1 region.</summary>
            S2WasDroppedInTheS1Region = 1815,
            ///<summary>The owner of $s2 has appeared in the $s1 region.</summary>
            TheOwnerOfS2HasAppearedInTheS1Region = 1816,
            ///<summary>$s2's owner has logged into the $s1 region.</summary>
            S2OwnerHasLoggedIntoTheS1Region = 1817,
            ///<summary>$s1 has disappeared.</summary>
            S1HasDisappeared = 1818,
            ///<summary>An evil is pulsating from $s2 in $s1.</summary>
            EvilFromS2InS1 = 1819,
            ///<summary>$s1 is currently asleep.</summary>
            S1CurrentlySleep = 1820,
            ///<summary>$s2's evil presence is felt in $s1.</summary>
            S2EvilPresenceFeltInS1 = 1821,
            ///<summary>$s1 has been sealed.</summary>
            S1Sealed = 1822,
            ///<summary>The registration period for a clan hall war has ended.</summary>
            ClanhallWarRegistrationPeriodEnded = 1823,
            ///<summary>You have been registered for a clan hall war. Please move to the left side of the clan hall's arena and get ready.</summary>
            RegisteredForClanhallWar = 1824,
            ///<summary>You have failed in your attempt to register for the clan hall war. Please try again.</summary>
            ClanhallWarRegistrationFailed = 1825,
            ///<summary>In $s1 minute(s), the game will begin. All players must hurry and move to the left side of the clan hall's arena.</summary>
            ClanhallWarBeginsInS1Minutes = 1826,
            ///<summary>In $s1 minute(s), the game will begin. All players must, please enter the arena now.</summary>
            ClanhallWarBeginsInS1MinutesEnterNow = 1827,
            ///<summary>In $s1 seconds(s), the game will begin.</summary>
            ClanhallWarBeginsInS1Seconds = 1828,
            ///<summary>The Command Channel is full.</summary>
            CommandChannelFull = 1829,
            ///<summary>$s1 is not allowed to use the party room invite command. Please update the waiting list.</summary>
            S1NotAllowedInviteToPartyRoom = 1830,
            ///<summary>$s1 does not meet the conditions of the party room. Please update the waiting list.</summary>
            S1NotMeetConditionsForPartyRoom = 1831,
            ///<summary>Only a room leader may invite others to a party room.</summary>
            OnlyRoomLeaderCanInvite = 1832,
            ///<summary>All of $s1 will be dropped. Would you like to continue?.</summary>
            ConfirmDropAllOfS1 = 1833,
            ///<summary>The party room is full. No more characters can be invitet in.</summary>
            PartyRoomFull = 1834,
            ///<summary>$s1 is full and cannot accept additional clan members at this time.</summary>
            S1ClanIsFull = 1835,
            ///<summary>You cannot join a Clan Academy because you have successfully completed your 2nd class transfer.</summary>
            CannotJoinAcademyAfter_2NdOccupation = 1836,
            ///<summary>$s1 has sent you an invitation to join the $s3 Royal Guard under the $s2 clan. Would you like to join?.</summary>
            S1SentInvitationToRoyalGuardS3OfClanS2 = 1837,
            ///<summary>1. The coupon an be used once per character.</summary>
            CouponOncePerCharacter = 1838,
            ///<summary>2. A used serial number may not be used again.</summary>
            SerialMayUsedOnce = 1839,
            ///<summary>3. If you enter the incorrect serial number more than 5 times, .</summary>
            SerialInputIncorrect = 1840,
            ///<summary>The clan hall war has been cancelled. Not enough clans have registered.</summary>
            ClanhallWarCancelled = 1841,
            ///<summary>$s1 wishes to summon you from $s2. Do you accept?.</summary>
            S1WishesToSummonYouFromS2DoYouAccept = 1842,
            ///<summary>$s1 is engaged in combat and cannot be summoned.</summary>
            S1IsEngagedInCombatAndCannotBeSummoned = 1843,
            ///<summary>$s1 is dead at the moment and cannot be summoned.</summary>
            S1IsDeadAtTheMomentAndCannotBeSummoned = 1844,
            ///<summary>Hero weapons cannot be destroyed.</summary>
            HeroWeaponsCantDestroyed = 1845,
            ///<summary>You are too far away from the Strider to mount it.</summary>
            TooFarAwayFromStriderToMount = 1846,
            ///<summary>You caught a fish $s1 in length.</summary>
            CaughtFishS1Length = 1847,
            ///<summary>Because of the size of fish caught, you will be registered in the ranking.</summary>
            RegisteredInFishSizeRanking = 1848,
            ///<summary>All of $s1 will be discarded. Would you like to continue?.</summary>
            ConfirmDiscardAllOfS1 = 1849,
            ///<summary>The Captain of the Order of Knights cannot be appointed.</summary>
            CaptainOfOrderOfKnightsCannotBeAppointed = 1850,
            ///<summary>The Captain of the Royal Guard cannot be appointed.</summary>
            CaptainOfRoyalGuardCannotBeAppointed = 1851,
            ///<summary>The attempt to acquire the skill has failed because of an insufficient Clan Reputation Score.</summary>
            AcquireSkillFailedBadClanRepScore = 1852,
            ///<summary>Quantity items of the same type cannot be exchanged at the same time.</summary>
            CantExchangeQuantityItemsOfSameType = 1853,
            ///<summary>The item was converted successfully.</summary>
            ItemConvertedSuccessfully = 1854,
            ///<summary>Another military unit is already using that name. Please enter a different name.</summary>
            AnotherMilitaryUnitIsAlreadyUsingThatName = 1855,
            ///<summary>Since your opponent is now the owner of $s1, the Olympiad has been cancelled.</summary>
            OpponentPossessesS1OlympiadCancelled = 1856,
            ///<summary>$s1 is the owner of $s2 and cannot participate in the Olympiad.</summary>
            S1OwnsS2AndCannotParticipateInOlympiad = 1857,
            ///<summary>You cannot participate in the Olympiad while dead.</summary>
            CannotParticipateOlympiadWhileDead = 1858,
            ///<summary>You exceeded the quantity that can be moved at one time.</summary>
            ExceededQuantityForMoved = 1859,
            ///<summary>The Clan Reputation Score is too low.</summary>
            TheClanReputationScoreIsTooLow = 1860,
            ///<summary>The clan's crest has been deleted.</summary>
            ClanCrestHasBeenDeleted = 1861,
            ///<summary>Clan skills will now be activated since the clan's reputation score is 0 or higher.</summary>
            ClanSkillsWillBeActivatedSinceReputationIs0OrHigher = 1862,
            ///<summary>$s1 purchased a clan item, reducing the Clan Reputation by $s2 points.</summary>
            S1PurchasedClanItemReducingS2RepuPoints = 1863,
            ///<summary>Your pet/servitor is unresponsive and will not obey any orders.</summary>
            PetRefusingOrder = 1864,
            ///<summary>Your pet/servitor is currently in a state of distress.</summary>
            PetInStateOfDistress = 1865,
            ///<summary>MP was reduced by $s1.</summary>
            MpReducedByS1 = 1866,
            ///<summary>Your opponent's MP was reduced by $s1.</summary>
            YourOpponentsMpWasReducedByS1 = 1867,
            ///<summary>You cannot exchange an item while it is being used.</summary>
            CannotExchanceUsedItem = 1868,
            ///<summary>$s1 has granted the Command Channel's master party the privilege of item looting.</summary>
            S1GrantedMasterPartyLootingRights = 1869,
            ///<summary>A Command Channel with looting rights already exists.</summary>
            CommandChannelWithLootingRightsExists = 1870,
            ///<summary>Do you want to dismiss $s1 from the clan?.</summary>
            ConfirmDismissS1FromClan = 1871,
            ///<summary>You have $s1 hour(s) and $s2 minute(s) left.</summary>
            S1HoursS2MinutesLeft = 1872,
            ///<summary>There are $s1 hour(s) and $s2 minute(s) left in the fixed use time for this PC Cafe.</summary>
            S1HoursS2MinutesLeftForThisPccafe = 1873,
            ///<summary>There are $s1 minute(s) left for this individual user.</summary>
            S1MinutesLeftForThisUser = 1874,
            ///<summary>There are $s1 minute(s) left in the fixed use time for this PC Cafe.</summary>
            S1MinutesLeftForThisPccafe = 1875,
            ///<summary>Do you want to leave $s1 clan?.</summary>
            ConfirmLeaveS1Clan = 1876,
            ///<summary>The game will end in $s1 minutes.</summary>
            GameWillEndInS1Minutes = 1877,
            ///<summary>The game will end in $s1 seconds.</summary>
            GameWillEndInS1Seconds = 1878,
            ///<summary>In $s1 minute(s), you will be teleported outside of the game arena.</summary>
            InS1MinutesTeleportedOutsideOfGameArena = 1879,
            ///<summary>In $s1 seconds(s), you will be teleported outside of the game arena.</summary>
            InS1SecondsTeleportedOutsideOfGameArena = 1880,
            ///<summary>The preliminary match will begin in $s1 second(s). Prepare yourself.</summary>
            PreliminaryMatchBeginInS1Seconds = 1881,
            ///<summary>Characters cannot be created from this server.</summary>
            CharactersNotCreatedFromThisServer = 1882,
            ///<summary>There are no offerings I own or I made a bid for.</summary>
            NoOfferingsOwnOrMadeBidFor = 1883,
            ///<summary>Enter the PC Room coupon serial number.</summary>
            EnterPcroomSerialNumber = 1884,
            ///<summary>This serial number cannot be entered. Please try again in minute(s).</summary>
            SerialNumberCantEntered = 1885,
            ///<summary>This serial has already been used.</summary>
            SerialNumberAlreadyUsed = 1886,
            ///<summary>Invalid serial number. Your attempt to enter the number has failed time(s). You will be allowed to make more attempt(s).</summary>
            SerialNumberEnteringFailed = 1887,
            ///<summary>Invalid serial number. Your attempt to enter the number has failed 5 time(s). Please try again in 4 hours.</summary>
            SerialNumberEnteringFailed5Times = 1888,
            ///<summary>Congratulations! You have received $s1.</summary>
            CongratulationsReceivedS1 = 1889,
            ///<summary>Since you have already used this coupon, you may not use this serial number.</summary>
            AlreadyUsedCouponNotUseSerialNumber = 1890,
            ///<summary>You may not use items in a private store or private work shop.</summary>
            NotUseItemsInPrivateStore = 1891,
            ///<summary>The replay file for the previous version cannot be played.</summary>
            ReplayFilePreviousVersionCantPlayed = 1892,
            ///<summary>This file cannot be replayed.</summary>
            FileCantReplayed = 1893,
            ///<summary>A sub-class cannot be created or changed while you are over your weight limit.</summary>
            NotSubclassWhileOverweight = 1894,
            ///<summary>$s1 is in an area which blocks summoning.</summary>
            S1InSummonBlockingArea = 1895,
            ///<summary>$s1 has already been summoned.</summary>
            S1AlreadySummoned = 1896,
            ///<summary>$s1 is required for summoning.</summary>
            S1RequiredForSummoning = 1897,
            ///<summary>$s1 is currently trading or operating a private store and cannot be summoned.</summary>
            S1CurrentlyTradingOrOperatingPrivateStoreAndCannotBeSummoned = 1898,
            ///<summary>Your target is in an area which blocks summoning.</summary>
            YourTargetIsInAnAreaWhichBlocksSummoning = 1899,
            ///<summary>$s1 has entered the party room.</summary>
            S1EnteredPartyRoom = 1900,
            ///<summary>$s1 has invited you to enter the party room.</summary>
            S1InvitedYouToPartyRoom = 1901,
            ///<summary>Incompatible item grade. This item cannot be used.</summary>
            IncompatibleItemGrade = 1902,
            ///<summary>Those of you who have requested NCOTP should run NCOTP by using your cell phone [...].</summary>
            Ncotp = 1903,
            ///<summary>A sub-class may not be created or changed while a servitor or pet is summoned.</summary>
            CantSubclassWithSummonedServitor = 1904,
            ///<summary>$s2 of $s1 will be replaced with $s4 of $s3.</summary>
            S2OfS1WillReplacedWithS4OfS3 = 1905,
            ///<summary>Select the combat unit.</summary>
            SelectCombatUnit = 1906,
            ///<summary>Select the character who will [...].</summary>
            SelectCharacterWhoWill = 1907,
            ///<summary>$s1 in a state which prevents summoning.</summary>
            S1StateForbidsSummoning = 1908,
            ///<summary>==List of Academy Graduates During the Past Week==.</summary>
            AcademyListHeader = 1909,
            ///<summary>Graduates: $s1.</summary>
            GraduatesS1 = 1910,
            ///<summary>You cannot summon players who are currently participating in the Grand Olympiad.</summary>
            YouCannotSummonPlayersWhoAreInOlympiad = 1911,
            ///<summary>Only those requesting NCOTP should make an entry into this field.</summary>
            Ncotp2 = 1912,
            ///<summary>The remaining recycle time for $s1 is $s2 minute(s).</summary>
            TimeForS1IsS2MinutesRemaining = 1913,
            ///<summary>The remaining recycle time for $s1 is $s2 seconds(s).</summary>
            TimeForS1IsS2SecondsRemaining = 1914,
            ///<summary>The game will end in $s1 second(s).</summary>
            GameEndsInS1Seconds = 1915,
            ///<summary>Your Death Penalty is now level $s1.</summary>
            DeathPenaltyLevelS1Added = 1916,
            ///<summary>Your Death Penalty has been lifted.</summary>
            DeathPenaltyLifted = 1917,
            ///<summary>Your pet is too high level to control.</summary>
            PetTooHighToControl = 1918,
            ///<summary>The Grand Olympiad registration period has ended.</summary>
            OlympiadRegistrationPeriodEnded = 1919,
            ///<summary>Your account is currently inactive because you have not logged into the game for some time. You may reactivate your account by visiting the PlayNC website (http://www.plaync.com/us/support/).</summary>
            AccountInactivity = 1920,
            ///<summary>$s2 hour(s) and $s3 minute(s) have passed since $s1 has killed.</summary>
            S2HoursS3MinutesSinceS1Killed = 1921,
            ///<summary>Because $s1 has failed to kill for one full day, it has expired.</summary>
            S1FailedKillingExpired = 1922,
            ///<summary>Court Magician: The portal has been created!.</summary>
            CourtMagicianCreatedPortal = 1923,
            ///<summary>Current Location: $s1, $s2, $s3 (Near the Primeval Isle).</summary>
            LocPrimevalIsleS1S2S3 = 1924,
            ///<summary>Due to the affects of the Seal of Strife, it is not possible to summon at this time.</summary>
            SealOfStrifeForbidsSummoning = 1925,
            ///<summary>There is no opponent to receive your challenge for a duel.</summary>
            ThereIsNoOpponentToReceiveYourChallengeForADuel = 1926,
            ///<summary>$s1 has been challenged to a duel.</summary>
            S1HasBeenChallengedToADuel = 1927,
            ///<summary>$s1's party has been challenged to a duel.</summary>
            S1PartyHasBeenChallengedToADuel = 1928,
            ///<summary>$s1 has accepted your challenge to a duel. The duel will begin in a few moments.</summary>
            S1HasAcceptedYourChallengeToADuelTheDuelWillBeginInAFewMoments = 1929,
            ///<summary>You have accepted $s1's challenge to a duel. The duel will begin in a few moments.</summary>
            YouHaveAcceptedS1ChallengeToADuelTheDuelWillBeginInAFewMoments = 1930,
            ///<summary>$s1 has declined your challenge to a duel.</summary>
            S1HasDeclinedYourChallengeToADuel = 1931,
            ///<summary>$s1 has declined your challenge to a duel.</summary>
            S1HasDeclinedYourChallengeToADuel2 = 1932,
            ///<summary>You have accepted $s1's challenge to a party duel. The duel will begin in a few moments.</summary>
            YouHaveAcceptedS1ChallengeToAPartyDuelTheDuelWillBeginInAFewMoments = 1933,
            ///<summary>$s1 has accepted your challenge to duel against their party. The duel will begin in a few moments.</summary>
            S1HasAcceptedYourChallengeToDuelAgainstTheirPartyTheDuelWillBeginInAFewMoments = 1934,
            ///<summary>$s1 has declined your challenge to a party duel.</summary>
            S1HasDeclinedYourChallengeToAPartyDuel = 1935,
            ///<summary>The opposing party has declined your challenge to a duel.</summary>
            TheOpposingPartyHasDeclinedYourChallengeToADuel = 1936,
            ///<summary>Since the person you challenged is not currently in a party, they cannot duel against your party.</summary>
            SinceThePersonYouChallengedIsNotCurrentlyInAPartyTheyCannotDuelAgainstYourParty = 1937,
            ///<summary>$s1 has challenged you to a duel.</summary>
            S1HasChallengedYouToADuel = 1938,
            ///<summary>$s1's party has challenged your party to a duel.</summary>
            S1PartyHasChallengedYourPartyToADuel = 1939,
            ///<summary>You are unable to request a duel at this time.</summary>
            YouAreUnableToRequestADuelAtThisTime = 1940,
            ///<summary>This is no suitable place to challenge anyone or party to a duel.</summary>
            NoPlaceForDuel = 1941,
            ///<summary>The opposing party is currently unable to accept a challenge to a duel.</summary>
            TheOpposingPartyIsCurrentlyUnableToAcceptAChallengeToADuel = 1942,
            ///<summary>The opposing party is currently not in a suitable location for a duel.</summary>
            TheOpposingPartyIsAtBadLocationForADuel = 1943,
            ///<summary>In a moment, you will be transported to the site where the duel will take place.</summary>
            InAMomentYouWillBeTransportedToTheSiteWhereTheDuelWillTakePlace = 1944,
            ///<summary>The duel will begin in $s1 second(s).</summary>
            TheDuelWillBeginInS1Seconds = 1945,
            ///<summary>$s1 has challenged you to a duel. Will you accept?.</summary>
            S1ChallengedYouToADuel = 1946,
            ///<summary>$s1's party has challenged your party to a duel. Will you accept?.</summary>
            S1ChallengedYouToAPartyDuel = 1947,
            ///<summary>The duel will begin in $s1 second(s).</summary>
            TheDuelWillBeginInS1Seconds2 = 1948,
            ///<summary>Let the duel begin!.</summary>
            LetTheDuelBegin = 1949,
            ///<summary>$s1 has won the duel.</summary>
            S1HasWonTheDuel = 1950,
            ///<summary>$s1's party has won the duel.</summary>
            S1PartyHasWonTheDuel = 1951,
            ///<summary>The duel has ended in a tie.</summary>
            TheDuelHasEndedInATie = 1952,
            ///<summary>Since $s1 was disqualified, $s2 has won.</summary>
            SinceS1WasDisqualifiedS2HasWon = 1953,
            ///<summary>Since $s1's party was disqualified, $s2's party has won.</summary>
            SinceS1PartyWasDisqualifiedS2PartyHasWon = 1954,
            ///<summary>Since $s1 withdrew from the duel, $s2 has won.</summary>
            SinceS1WithdrewFromTheDuelS2HasWon = 1955,
            ///<summary>Since $s1's party withdrew from the duel, $s2's party has won.</summary>
            SinceS1PartyWithdrewFromTheDuelS2PartyHasWon = 1956,
            ///<summary>Select the item to be augmented.</summary>
            SelectTheItemToBeAugmented = 1957,
            ///<summary>Select the catalyst for augmentation.</summary>
            SelectTheCatalystForAugmentation = 1958,
            ///<summary>Requires $s1 $s2.</summary>
            RequiresS1S2 = 1959,
            ///<summary>This is not a suitable item.</summary>
            ThisIsNotASuitableItem = 1960,
            ///<summary>Gemstone quantity is incorrect.</summary>
            GemstoneQuantityIsIncorrect = 1961,
            ///<summary>The item was successfully augmented!.</summary>
            TheItemWasSuccessfullyAugmented = 1962,
            /// ID : 1963
            ///<summary>Select the item from which you wish to remove augmentation.</summary>
            SelectTheItemFromWhichYouWishToRemoveAugmentation = 1963,
            ///<summary>Augmentation removal can only be done on an augmented item.</summary>
            AugmentationRemovalCanOnlyBeDoneOnAnAugmentedItem = 1964,
            ///<summary>Augmentation has been successfully removed from your $s1.</summary>
            AugmentationHasBeenSuccessfullyRemovedFromYourS1 = 1965,
            ///<summary>Only the clan leader may issue commands.</summary>
            OnlyClanLeaderCanIssueCommands = 1966,
            ///<summary>The gate is firmly locked. Please try again later.</summary>
            GateLockedTryAgainLater = 1967,
            ///<summary>$s1's owner.</summary>
            S1Owner = 1968,
            ///<summary>Area where $s1 appears.</summary>
            AreaS1Appears = 1968,
            ///<summary>Once an item is augmented, it cannot be augmented again.</summary>
            OnceAnItemIsAugmentedItCannotBeAugmentedAgain = 1970,
            ///<summary>The level of the hardener is too high to be used.</summary>
            HardenerLevelTooHigh = 1971,
            ///<summary>You cannot augment items while a private store or private workshop is in operation.</summary>
            YouCannotAugmentItemsWhileAPrivateStoreOrPrivateWorkshopIsInOperation = 1972,
            ///<summary>You cannot augment items while frozen.</summary>
            YouCannotAugmentItemsWhileFrozen = 1973,
            ///<summary>You cannot augment items while dead.</summary>
            YouCannotAugmentItemsWhileDead = 1974,
            ///<summary>You cannot augment items while engaged in trade activities.</summary>
            YouCannotAugmentItemsWhileTrading = 1975,
            ///<summary>You cannot augment items while paralyzed.</summary>
            YouCannotAugmentItemsWhileParalyzed = 1976,
            ///<summary>You cannot augment items while fishing.</summary>
            YouCannotAugmentItemsWhileFishing = 1977,
            ///<summary>You cannot augment items while sitting down.</summary>
            YouCannotAugmentItemsWhileSittingDown = 1978,
            ///<summary>$s1's remaining Mana is now 10.</summary>
            S1SRemainingManaIsNow10 = 1979,
            ///<summary>$s1's remaining Mana is now 5.</summary>
            S1SRemainingManaIsNow5 = 1980,
            ///<summary>$s1's remaining Mana is now 1. It will disappear soon.</summary>
            S1SRemainingManaIsNow1 = 1981,
            ///<summary>$s1's remaining Mana is now 0, and the item has disappeared.</summary>
            S1SRemainingManaIsNow0 = 1982,
            ///<summary>Press the Augment button to begin.</summary>
            PressTheAugmentButtonToBegin = 1984,
            ///<summary>$s1's drop area ($s2).</summary>
            S1DropAreaS2 = 1985,
            ///<summary>$s1's owner ($s2).</summary>
            S1OwnerS2 = 1986,
            ///<summary>$s1.</summary>
            S1 = 1987,
            ///<summary>The ferry has arrived at Primeval Isle.</summary>
            FerryArrivedAtPrimeval = 1988,
            ///<summary>The ferry will leave for Rune Harbor after anchoring for three minutes.</summary>
            FerryLeavingForRune3Minutes = 1989,
            ///<summary>The ferry is now departing Primeval Isle for Rune Harbor.</summary>
            FerryLeavingPrimevalForRuneNow = 1990,
            ///<summary>The ferry will leave for Primeval Isle after anchoring for three minutes.</summary>
            FerryLeavingForPrimeval3Minutes = 1991,
            ///<summary>The ferry is now departing Rune Harbor for Primeval Isle.</summary>
            FerryLeavingRuneForPrimevalNow = 1992,
            ///<summary>The ferry from Primeval Isle to Rune Harbor has been delayed.</summary>
            FerryFromPrimevalToRuneDelayed = 1993,
            ///<summary>The ferry from Rune Harbor to Primeval Isle has been delayed.</summary>
            FerryFromRuneToPrimevalDelayed = 1994,
            ///<summary>$s1 channel filtering option.</summary>
            S1ChannelFilterOption = 1995,
            ///<summary>The attack has been blocked.</summary>
            AttackWasBlocked = 1996,
            ///<summary>$s1 is performing a counterattack.</summary>
            S1PerformingCounterattack = 1997,
            ///<summary>You countered $s1's attack.</summary>
            CounteredS1Attack = 1998,
            ///<summary>$s1 dodges the attack.</summary>
            S1DodgesAttack = 1999,
            ///<summary>You have avoided $s1's attack.</summary>
            AvoidedS1Attack2 = 2000,
            ///<summary>Augmentation failed due to inappropriate conditions.</summary>
            AugmentationFailedDueToInappropriateConditions = 2001,
            ///<summary>Trap failed.</summary>
            TrapFailed = 2002,
            ///<summary>You obtained an ordinary material.</summary>
            ObtainedOrdinaryMaterial = 2003,
            ///<summary>You obtained a rare material.</summary>
            ObtainedRateMaterial = 2004,
            ///<summary>You obtained a unique material.</summary>
            ObtainedUniqueMaterial = 2005,
            ///<summary>You obtained the only material of this kind.</summary>
            ObtainedOnlyMaterial = 2006,
            ///<summary>Please enter the recipient's name.</summary>
            EnterRecipientsName = 2007,
            ///<summary>Please enter the text.</summary>
            EnterText = 2008,
            ///<summary>You cannot exceed 1500 characters.</summary>
            CantExceed1500Characters = 2009,
            ///<summary>$s2 $s1.</summary>
            S2S1 = 2009,
            ///<summary>The augmented item cannot be discarded.</summary>
            AugmentedItemCannotBeDiscarded = 2011,
            ///<summary>$s1 has been activated.</summary>
            S1HasBeenActivated = 2012,
            ///<summary>Your seed or remaining purchase amount is inadequate.</summary>
            YourSeedOrRemainingPurchaseAmountIsInadequate = 2013,
            ///<summary>You cannot proceed because the manor cannot accept any more crops. All crops have been returned and no adena withdrawn.</summary>
            ManorCantAcceptMoreCrops = 2014,
            ///<summary>A skill is ready to be used again.</summary>
            SkillReadyToUseAgain = 2015,
            ///<summary>A skill is ready to be used again but its re-use counter time has increased.</summary>
            SkillReadyToUseAgainButTimeIncreased = 2016,
            ///<summary>$s1 cannot duel because $s1 is currently engaged in a private store or manufacture.</summary>
            S1CannotDuelBecauseS1IsCurrentlyEngagedInAPrivateStoreOrManufacture = 2017,
            ///<summary>$s1 cannot duel because $s1 is currently fishing.</summary>
            S1CannotDuelBecauseS1IsCurrentlyFishing = 2018,
            ///<summary>$s1 cannot duel because $s1's HP or MP is below 50%.</summary>
            S1CannotDuelBecauseS1HpOrMpIsBelow50Percent = 2019,
            ///<summary>$s1 cannot make a challenge to a duel because $s1 is currently in a duel-prohibited area (Peaceful Zone / Seven Signs Zone / Near Water / Restart Prohibited Area).</summary>
            S1CannotMakeAChallangeToADuelBecauseS1IsCurrentlyInADuelProhibitedArea = 2020,
            ///<summary>$s1 cannot duel because $s1 is currently engaged in battle.</summary>
            S1CannotDuelBecauseS1IsCurrentlyEngagedInBattle = 2021,
            ///<summary>$s1 cannot duel because $s1 is already engaged in a duel.</summary>
            S1CannotDuelBecauseS1IsAlreadyEngagedInADuel = 2022,
            ///<summary>$s1 cannot duel because $s1 is in a chaotic state.</summary>
            S1CannotDuelBecauseS1IsInAChaoticState = 2023,
            ///<summary>$s1 cannot duel because $s1 is participating in the Olympiad.</summary>
            S1CannotDuelBecauseS1IsParticipatingInTheOlympiad = 2024,
            ///<summary>$s1 cannot duel because $s1 is participating in a clan hall war.</summary>
            S1CannotDuelBecauseS1IsParticipatingInAClanHallWar = 2025,
            ///<summary>$s1 cannot duel because $s1 is participating in a siege war.</summary>
            S1CannotDuelBecauseS1IsParticipatingInASiegeWar = 2026,
            ///<summary>$s1 cannot duel because $s1 is currently riding a boat or strider.</summary>
            S1CannotDuelBecauseS1IsCurrentlyRidingABoatWyvernOrStrider = 2027,
            ///<summary>$s1 cannot receive a duel challenge because $s1 is too far away.</summary>
            S1CannotReceiveADuelChallengeBecauseS1IsTooFarAway = 2028,
            ///<summary>$s1 is currently teleporting and cannot participate in the Olympiad.</summary>
            S1CannotParticipateInOlympiadDuringTeleport = 2029,
            ///<summary>You are currently logging in.</summary>
            CurrentlyLoggingIn = 2030,
            ///<summary>Please wait a moment.</summary>
            PleaseWaitAMoment = 2031,

            //Added (Missing?)
            ///<summary>You can only register 16x12 pixel 256 color bmp files.</summary>
            CanOnlyRegister1612Px256ColorBmpFiles = 211,
            ///<summary>Incorrect item.</summary>
            IncorrectItem = 352,

            //Other messages (Interlude+) being referenced in the project

            ///<summary>You already polymorphed and cannot polymorph again.</summary>
            AlreadyPolymorphedCannotPolymorphAgain = 2058,
            ///<summary>You cannot polymorph into the desired form in water.</summary>
            CannotPolymorphIntoTheDesiredFormInWater = 2060,
            ///<summary>You cannot polymorph when you have summoned a servitor/pet.</summary>
            CannotPolymorphWhenSummonedServitor = 2062,
            ///<summary>You cannot polymorph while riding a pet.</summary>
            CannotPolymorphWhileRidingPet = 2063,

            ///<summary>//You cannot enter due to the party having exceeded the limit.</summary>
            CannotEnterDuePartyHavingExceedLimit = 2102,
            ///<summary>The augmented item cannot be converted. Please convert after the augmentation has been removed.</summary>
            AugmentedItemCannotBeConverted = 2129,
            ///<summary>You cannot convert this item.</summary>
            CannotConvertThisItem = 2130,
            ///<summary>Your soul count has increased by $s1. It is now at $s2.</summary>
            YourSoulCountHasIncreasedByS1NowAtS2 = 2162,
            ///<summary>Soul cannot be increased anymore.</summary>
            SoulCannotBeIncreasedAnymore = 2163,
            ///<summary>You cannot polymorph while riding a boat.</summary>
            CannotPolymorphWhileRidingBoat = 2182,
            ///<summary>Another enchantment is in progress. Please complete the previous task, then try again.</summary>
            AnotherEnchantmentIsInProgress = 2188,

            ///<summary>Not enough bolts.</summary>
            NotEnoughBolts = 2226,
            ///<summary>$c1 has given $c2 damage of $s3.</summary>
            C1HasGivenC2DamageOfS3 = 2261,
            ///<summary>$c1 has received $s3 damage from $c2.</summary>
            C1HasReceivedS3DamageFromC2 = 2262,
            ///<summary>$c1 has evaded $c2's attack.</summary>
            C1HasEvadedC2Attack = 2264,
            ///<summary>$c1's attack went astray.</summary>
            C1AttackWentAstray = 2265,
            ///<summary>$c1 landed a critical hit!.</summary>
            C1LandedACriticalHit = 2266,
            ///<summary>You cannot transform while sitting.</summary>
            CannotTransformWhileSitting = 2283,
            ///<summary>The length of the crest or insignia does not meet the standard requirements.</summary>
            LengthCrestDoesNotMeetStandardRequirements = 2285,

            ///<summary>There are $s2 second(s) remaining in $s1's re-use time.</summary>
            S2SecondsRemainingInS1ReuseTime = 2303,
            ///<summary>There are $s2 minute(s), $s3 second(s) remaining in $s1's re-use time.</summary>
            S2MinutesS3SecondsRemainingInS1ReuseTime = 2304,
            ///<summary>There are $s2 hour(s), $s3 minute(s), and $s4 second(s) remaining in $s1's re-use time.</summary>
            S2HoursS3MinutesS4SecondsRemainingInS1ReuseTime = 2305,
            ///<summary>This is an incorrect support enhancement spellbook.</summary>
            IncorrectSupportEnhancementSpellbook = 2385,
            ///<summary>This item does not meet the requirements for the support enhancement spellbook.</summary>
            ItemDoesNotMeetRequirementsForSupportEnhancementSpellbook = 2386,
            ///<summary>Registration of the support enhancement spellbook has failed.</summary>
            RegistrationOfEnhancementSpellbookHasFailed = 2387,

            ///<summary>You cannot use My Teleports while flying.</summary>
            CannotUseMyTeleportsWhileFlying = 2351,
            ///<summary>You cannot use My Teleports while you are dead.</summary>
            CannotUseMyTeleportsWhileDead = 2354,
            ///<summary>You cannot use My Teleports underwater.</summary>
            CannotUseMyTeleportsUnderwater = 2356,
            ///<summary>You have no space to save the teleport location.</summary>
            NoSpaceToSaveTeleportLocation = 2358,
            ///<summary>You cannot teleport because you do not have a teleport item.</summary>
            CannotTeleportBecauseDoNotHaveTeleportItem = 2359,
            ///<summary>Your number of My Teleports slots has reached its maximum limit.</summary>
            YourNumberOfMyTeleportsSlotsHasReachedLimit = 2390,

            ///<summary>The number of My Teleports slots has been increased.</summary>
            NumberOfMyTeleportsSlotsHasBeenIncreased = 2409,

            ///<summary>//You cannot teleport while in possession of a ward.</summary>
            CannotTeleportWhilePossessionWard = 2778,

            ///<summary>You could not receive because your inventory is full.</summary>
            YouCouldNotReceiveBecauseYourInventoryIsFull = 2981,

            //A user currently participating in the Olympiad cannot send party and friend invitations.
            UserCurrentlyParticipatingInOlympiadCannotSendPartyAndFriendInvitations = 3094,

            ///<summary>Requesting approval for changing party loot to "$s1".</summary>
            RequestingApprovalForChangingPartyLootToS1 = 3135,
            ///<summary>Party loot change was cancelled.</summary>
            PartyLootChangeWasCancelled = 3137,
            ///<summary>Party loot was changed to "$s1".</summary>
            PartyLootWasChangedToS1 = 3138,
            ///<summary>The crest was successfully registered.</summary>
            ClanCrestWasSuccesfullyRegistered = 3140,
            ///<summary>$c1 is set to refuse party requests and cannot receive a party request.</summary>
            C1IsSetToRefusePartyRequests = 3168,

            ///<summary>You cannot bookmark this location because you do not have a My Teleport Flag.</summary>
            CannotBookmarkThisLocationBecauseNoMyTeleportFlag = 6501,

            //No description found

            NotImplementedYet2361 = 2361
        }
    }
}