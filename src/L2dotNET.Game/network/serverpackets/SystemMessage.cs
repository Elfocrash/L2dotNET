using System.Collections.Generic;
using L2dotNET.GameService.Model.items;
using L2dotNET.GameService.Model.npcs;
using L2dotNET.GameService.Model.playable;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.network.serverpackets
{
    public class SystemMessage : GameServerNetworkPacket
    {
        private readonly List<object[]> data = new List<object[]>();
        public int MessgeID;

        public SystemMessage(SystemMessageId msgId)
        {
            MessgeID = (int)msgId;
        }

        public SystemMessage AddString(string val)
        {
            data.Add(new object[] { 0, val });
            return this;
        }

        public SystemMessage AddNumber(int val)
        {
            data.Add(new object[] { 1, val });
            return this;
        }

        public SystemMessage AddNumber(double val)
        {
            data.Add(new object[] { 1, (int)val });
            return this;
        }

        public SystemMessage AddNpcName(int val)
        {
            data.Add(new object[] { 2, (1000000 + val) });
            return this;
        }

        public SystemMessage AddItemName(int val)
        {
            data.Add(new object[] { 3, val });
            return this;
        }

        public SystemMessage AddSkillName(int val, int lvl)
        {
            data.Add(new object[] { 4, val, lvl });
            return this;
        }

        public void AddCastleName(int val)
        {
            data.Add(new object[] { 5, val });
        }

        public void AddItemCount(long val)
        {
            data.Add(new object[] { 6, val });
        }

        public void AddZoneName(int val, int y, int z)
        {
            data.Add(new object[] { 7, val, y, z });
        }

        public void AddElementName(int val)
        {
            data.Add(new object[] { 9, val });
        }

        public void AddInstanceName(int val)
        {
            data.Add(new object[] { 10, val });
        }

        public SystemMessage AddPlayerName(string val)
        {
            data.Add(new object[] { 12, val });
            return this;
        }

        public SystemMessage AddName(L2Object obj)
        {
            if (obj is L2Player)
                return AddPlayerName(((L2Player)obj).Name);
            else if (obj is L2Npc)
                return AddNpcName(((L2Npc)obj).NpcId);
            else if (obj is L2Summon)
                return AddNpcName(((L2Summon)obj).NpcId);
            else if (obj is L2Item)
                return AddItemName(((L2Item)obj).Template.ItemID);
            else
                return AddString(obj.asString());
        }

        public void AddSysStr(int val)
        {
            data.Add(new object[] { 13, val });
        }

        protected internal override void write()
        {
            writeC(0x64);
            writeD(MessgeID);
            writeD(data.Count);

            foreach (object[] d in data)
            {
                int type = (int)d[0];

                writeD(type);

                switch (type)
                {
                    case 0: //text
                    case 12:
                        writeS((string)d[1]);
                        break;
                    case 1: //number
                    case 2: //npcid
                    case 3: //itemid
                    case 5:
                    case 9:
                    case 10:
                    case 13:
                        writeD((int)d[1]);
                        break;
                    case 4: //skillname
                        writeD((int)d[1]);
                        writeD((int)d[2]);
                        break;
                    case 6:
                        writeQ((long)d[1]);
                        break;
                    case 7: //zone
                        writeD((int)d[1]);
                        writeD((int)d[2]);
                        writeD((int)d[3]);
                        break;
                }
            }
        }

        public enum SystemMessageId
        {
            ///<summary>You have been disconnected from the server.</summary>
            YOU_HAVE_BEEN_DISCONNECTED = 0,
            ///<summary>The server will be coming down in $1 seconds. Please find a safe place to log out.</summary>
            THE_SERVER_WILL_BE_COMING_DOWN_IN_S1_SECONDS = 1,
            ///<summary>$s1 does not exist.</summary>
            S1_DOES_NOT_EXIST = 2,
            ///<summary>$s1 is not currently logged in.</summary>
            S1_IS_NOT_ONLINE = 3,
            ///<summary>You cannot ask yourself to apply to a clan.</summary>
            CANNOT_INVITE_YOURSELF = 4,
            ///<summary>$s1 already exists.</summary>
            S1_ALREADY_EXISTS = 5,
            ///<summary>$s1 does not exist</summary>
            S1_DOES_NOT_EXIST2 = 6,
            ///<summary>You are already a member of $s1.</summary>
            ALREADY_MEMBER_OF_S1 = 7,
            ///<summary>You are working with another clan.</summary>
            YOU_ARE_WORKING_WITH_ANOTHER_CLAN = 8,
            ///<summary>$s1 is not a clan leader.</summary>
            S1_IS_NOT_A_CLAN_LEADER = 9,
            ///<summary>$s1 is working with another clan.</summary>
            S1_WORKING_WITH_ANOTHER_CLAN = 10,
            ///<summary>There are no applicants for this clan.</summary>
            NO_APPLICANTS_FOR_THIS_CLAN = 11,
            ///<summary>The applicant information is incorrect.</summary>
            APPLICANT_INFORMATION_INCORRECT = 12,
            ///<summary>Unable to disperse: your clan has requested to participate in a castle siege.</summary>
            CANNOT_DISSOLVE_CAUSE_CLAN_WILL_PARTICIPATE_IN_CASTLE_SIEGE = 13,
            ///<summary>Unable to disperse: your clan owns one or more castles or hideouts.</summary>
            CANNOT_DISSOLVE_CAUSE_CLAN_OWNS_CASTLES_HIDEOUTS = 14,
            ///<summary>You are in siege.</summary>
            YOU_ARE_IN_SIEGE = 15,
            ///<summary>You are not in siege.</summary>
            YOU_ARE_NOT_IN_SIEGE = 16,
            ///<summary>The castle siege has begun.</summary>
            CASTLE_SIEGE_HAS_BEGUN = 17,
            ///<summary>The castle siege has ended.</summary>
            CASTLE_SIEGE_HAS_ENDED = 18,
            ///<summary>There is a new Lord of the castle!</summary>
            NEW_CASTLE_LORD = 19,
            ///<summary>The gate is being opened.</summary>
            GATE_IS_OPENING = 20,
            ///<summary>The gate is being destroyed.</summary>
            GATE_IS_DESTROYED = 21,
            ///<summary>Your target is out of range.</summary>
            TARGET_TOO_FAR = 22,
            ///<summary>Not enough HP.</summary>
            NOT_ENOUGH_HP = 23,
            ///<summary>Not enough MP.</summary>
            NOT_ENOUGH_MP = 24,
            ///<summary>Rejuvenating HP.</summary>
            REJUVENATING_HP = 25,
            ///<summary>Rejuvenating MP.</summary>
            REJUVENATING_MP = 26,
            ///<summary>Your casting has been interrupted.</summary>
            CASTING_INTERRUPTED = 27,
            ///<summary>You have obtained $s1 adena.</summary>
            YOU_PICKED_UP_S1_ADENA = 28,
            ///<summary>You have obtained $s2 $s1.</summary>
            YOU_PICKED_UP_S2_S1 = 29,
            ///<summary>You have obtained $s1.</summary>
            YOU_PICKED_UP_S1 = 30,
            ///<summary>You cannot move while sitting.</summary>
            CANT_MOVE_SITTING = 31,
            ///<summary>You are unable to engage in combat. Please go to the nearest restart point.</summary>
            UNABLE_COMBAT_PLEASE_GO_RESTART = 32,
            ///<summary>You cannot move while casting.</summary>
            CANT_MOVE_CASTING = 32,
            ///<summary>Welcome to the World of Lineage II.</summary>
            WELCOME_TO_LINEAGE = 34,
            ///<summary>You hit for $s1 damage</summary>
            YOU_DID_S1_DMG = 35,
            ///<summary>$s1 hit you for $s2 damage.</summary>
            S1_GAVE_YOU_S2_DMG = 36,
            ///<summary>$s1 hit you for $s2 damage.</summary>
            S1_GAVE_YOU_S2_DMG2 = 37,
            ///<summary>You carefully nock an arrow.</summary>
            GETTING_READY_TO_SHOOT_AN_ARROW = 41,
            ///<summary>You have avoided $s1's attack.</summary>
            AVOIDED_S1_ATTACK = 42,
            ///<summary>You have missed.</summary>
            MISSED_TARGET = 43,
            ///<summary>Critical hit!</summary>
            CRITICAL_HIT = 44,
            ///<summary>You have earned $s1 experience.</summary>
            EARNED_S1_EXPERIENCE = 45,
            ///<summary>You use $s1.</summary>
            USE_S1 = 46,
            ///<summary>You begin to use a(n) $s1.</summary>
            BEGIN_TO_USE_S1 = 47,
            ///<summary>$s1 is not available at this time: being prepared for reuse.</summary>
            S1_PREPARED_FOR_REUSE = 48,
            ///<summary>You have equipped your $s1.</summary>
            S1_EQUIPPED = 49,
            ///<summary>Your target cannot be found.</summary>
            TARGET_CANT_FOUND = 50,
            ///<summary>You cannot use this on yourself.</summary>
            CANNOT_USE_ON_YOURSELF = 51,
            ///<summary>You have earned $s1 adena.</summary>
            EARNED_S1_ADENA = 52,
            ///<summary>You have earned $s2 $s1(s).</summary>
            EARNED_S2_S1_S = 53,
            ///<summary>You have earned $s1.</summary>
            EARNED_ITEM_S1 = 54,
            ///<summary>You have failed to pick up $s1 adena.</summary>
            FAILED_TO_PICKUP_S1_ADENA = 55,
            ///<summary>You have failed to pick up $s1.</summary>
            FAILED_TO_PICKUP_S1 = 56,
            ///<summary>You have failed to pick up $s2 $s1(s).</summary>
            FAILED_TO_PICKUP_S2_S1_S = 57,
            ///<summary>You have failed to earn $s1 adena.</summary>
            FAILED_TO_EARN_S1_ADENA = 58,
            ///<summary>You have failed to earn $s1.</summary>
            FAILED_TO_EARN_S1 = 59,
            ///<summary>You have failed to earn $s2 $s1(s).</summary>
            FAILED_TO_EARN_S2_S1_S = 60,
            ///<summary>Nothing happened.</summary>
            NOTHING_HAPPENED = 61,
            ///<summary>Your $s1 has been successfully enchanted.</summary>
            S1_SUCCESSFULLY_ENCHANTED = 62,
            ///<summary>Your +$S1 $S2 has been successfully enchanted.</summary>
            S1_S2_SUCCESSFULLY_ENCHANTED = 63,
            ///<summary>The enchantment has failed! Your $s1 has been crystallized.</summary>
            ENCHANTMENT_FAILED_S1_EVAPORATED = 64,
            ///<summary>The enchantment has failed! Your +$s1 $s2 has been crystallized.</summary>
            ENCHANTMENT_FAILED_S1_S2_EVAPORATED = 65,
            ///<summary>$s1 is inviting you to join a party. Do you accept?</summary>
            S1_INVITED_YOU_TO_PARTY = 66,
            ///<summary>$s1 has invited you to the join the clan, $s2. Do you wish to join?</summary>
            S1_HAS_INVITED_YOU_TO_JOIN_THE_CLAN_S2 = 67,
            ///<summary>Would you like to withdraw from the $s1 clan? If you leave, you will have to wait at least a day before joining another clan.</summary>
            WOULD_YOU_LIKE_TO_WITHDRAW_FROM_THE_S1_CLAN = 68,
            ///<summary>Would you like to dismiss $s1 from the clan? If you do so, you will have to wait at least a day before accepting a new member.</summary>
            WOULD_YOU_LIKE_TO_DISMISS_S1_FROM_THE_CLAN = 69,
            ///<summary>Do you wish to disperse the clan, $s1?</summary>
            DO_YOU_WISH_TO_DISPERSE_THE_CLAN_S1 = 70,
            ///<summary>How many of your $s1(s) do you wish to discard?</summary>
            HOW_MANY_S1_DISCARD = 71,
            ///<summary>How many of your $s1(s) do you wish to move?</summary>
            HOW_MANY_S1_MOVE = 72,
            ///<summary>How many of your $s1(s) do you wish to destroy?</summary>
            HOW_MANY_S1_DESTROY = 73,
            ///<summary>Do you wish to destroy your $s1?</summary>
            WISH_DESTROY_S1 = 74,
            ///<summary>ID does not exist.</summary>
            ID_NOT_EXIST = 75,
            ///<summary>Incorrect password.</summary>
            INCORRECT_PASSWORD = 76,
            ///<summary>You cannot create another character. Please delete the existing character and try again.</summary>
            CANNOT_CREATE_CHARACTER = 77,
            ///<summary>When you delete a character, any items in his/her possession will also be deleted. Do you really wish to delete $s1%?</summary>
            WISH_DELETE_S1 = 78,
            ///<summary>This name already exists.</summary>
            NAMING_NAME_ALREADY_EXISTS = 79,
            ///<summary>Names must be between 1-16 characters, excluding spaces or special characters.</summary>
            NAMING_CHARNAME_UP_TO_16CHARS = 80,
            ///<summary>Please select your race.</summary>
            PLEASE_SELECT_RACE = 81,
            ///<summary>Please select your occupation.</summary>
            PLEASE_SELECT_OCCUPATION = 82,
            ///<summary>Please select your gender.</summary>
            PLEASE_SELECT_GENDER = 83,
            ///<summary>You may not attack in a peaceful zone.</summary>
            CANT_ATK_PEACEZONE = 84,
            ///<summary>You may not attack this target in a peaceful zone.</summary>
            TARGET_IN_PEACEZONE = 85,
            ///<summary>Please enter your ID.</summary>
            PLEASE_ENTER_ID = 86,
            ///<summary>Please enter your password.</summary>
            PLEASE_ENTER_PASSWORD = 87,
            ///<summary>Your protocol version is different, please restart your client and run a full check.</summary>
            WRONG_PROTOCOL_CHECK = 88,
            ///<summary>Your protocol version is different, please continue.</summary>
            WRONG_PROTOCOL_CONTINUE = 89,
            ///<summary>You are unable to connect to the server.</summary>
            UNABLE_TO_CONNECT = 90,
            ///<summary>Please select your hairstyle.</summary>
            PLEASE_SELECT_HAIRSTYLE = 91,
            ///<summary>$s1 has worn off.</summary>
            S1_HAS_WORN_OFF = 92,
            ///<summary>You do not have enough SP for this.</summary>
            NOT_ENOUGH_SP = 93,
            ///<summary>2004-2009 (c) Copyright NCsoft Corporation. All Rights Reserved.</summary>
            COPYRIGHT = 94,
            ///<summary>You have earned $s1 experience and $s2 SP.</summary>
            YOU_EARNED_S1_EXP_AND_S2_SP = 95,
            ///<summary>Your level has increased!</summary>
            YOU_INCREASED_YOUR_LEVEL = 96,
            ///<summary>This item cannot be moved.</summary>
            CANNOT_MOVE_THIS_ITEM = 97,
            ///<summary>This item cannot be discarded.</summary>
            CANNOT_DISCARD_THIS_ITEM = 98,
            ///<summary>This item cannot be traded or sold.</summary>
            CANNOT_TRADE_THIS_ITEM = 99,
            ///<summary>$s1 is requesting to trade. Do you wish to continue?</summary>
            S1_REQUESTS_TRADE = 100,
            ///<summary>You cannot exit while in combat.</summary>
            CANT_LOGOUT_WHILE_FIGHTING = 101,
            ///<summary>You cannot restart while in combat.</summary>
            CANT_RESTART_WHILE_FIGHTING = 102,
            ///<summary>This ID is currently logged in.</summary>
            ID_LOGGED_IN = 103,
            ///<summary>You may not equip items while casting or performing a skill.</summary>
            CANNOT_USE_ITEM_WHILE_USING_MAGIC = 104,
            ///<summary>You have invited $s1 to your party.</summary>
            YOU_INVITED_S1_TO_PARTY = 105,
            ///<summary>You have joined $s1's party.</summary>
            YOU_JOINED_S1_PARTY = 106,
            ///<summary>$s1 has joined the party.</summary>
            S1_JOINED_PARTY = 107,
            ///<summary>$s1 has left the party.</summary>
            S1_LEFT_PARTY = 108,
            ///<summary>Invalid target.</summary>
            INCORRECT_TARGET = 109,
            ///<summary>$s1 $s2's effect can be felt.</summary>
            YOU_FEEL_S1_EFFECT = 110,
            ///<summary>Your shield defense has succeeded.</summary>
            SHIELD_DEFENCE_SUCCESSFULL = 111,
            ///<summary>You may no longer adjust items in the trade because the trade has been confirmed.</summary>
            NOT_ENOUGH_ARROWS = 112,
            ///<summary>$s1 cannot be used due to unsuitable terms.</summary>
            S1_CANNOT_BE_USED = 113,
            ///<summary>You have entered the shadow of the Mother Tree.</summary>
            ENTER_SHADOW_MOTHER_TREE = 114,
            ///<summary>You have left the shadow of the Mother Tree.</summary>
            EXIT_SHADOW_MOTHER_TREE = 115,
            ///<summary>You have entered a peaceful zone.</summary>
            ENTER_PEACEFUL_ZONE = 116,
            ///<summary>You have left the peaceful zone.</summary>
            EXIT_PEACEFUL_ZONE = 117,
            ///<summary>You have requested a trade with $s1</summary>
            REQUEST_S1_FOR_TRADE = 118,
            ///<summary>$s1 has denied your request to trade.</summary>
            S1_DENIED_TRADE_REQUEST = 119,
            ///<summary>You begin trading with $s1.</summary>
            BEGIN_TRADE_WITH_S1 = 120,
            ///<summary>$s1 has confirmed the trade.</summary>
            S1_CONFIRMED_TRADE = 121,
            ///<summary>You may no longer adjust items in the trade because the trade has been confirmed.</summary>
            CANNOT_ADJUST_ITEMS_AFTER_TRADE_CONFIRMED = 122,
            ///<summary>Your trade is successful.</summary>
            TRADE_SUCCESSFUL = 123,
            ///<summary>$s1 has cancelled the trade.</summary>
            S1_CANCELED_TRADE = 124,
            ///<summary>Do you wish to exit the game?</summary>
            WISH_EXIT_GAME = 125,
            ///<summary>Do you wish to return to the character select screen?</summary>
            WISH_RESTART_GAME = 126,
            ///<summary>You have been disconnected from the server. Please login again.</summary>
            DISCONNECTED_FROM_SERVER = 127,
            ///<summary>Your character creation has failed.</summary>
            CHARACTER_CREATION_FAILED = 128,
            ///<summary>Your inventory is full.</summary>
            SLOTS_FULL = 129,
            ///<summary>Your warehouse is full.</summary>
            WAREHOUSE_FULL = 130,
            ///<summary>$s1 has logged in.</summary>
            S1_LOGGED_IN = 131,
            ///<summary>$s1 has been added to your friends list.</summary>
            S1_ADDED_TO_FRIENDS = 132,
            ///<summary>$s1 has been removed from your friends list.</summary>
            S1_REMOVED_FROM_YOUR_FRIENDS_LIST = 133,
            ///<summary>Please check your friends list again.</summary>
            PLEACE_CHECK_YOUR_FRIEND_LIST_AGAIN = 134,
            ///<summary>$s1 did not reply to your invitation. Your invitation has been cancelled.</summary>
            S1_DID_NOT_REPLY_TO_YOUR_INVITE = 135,
            ///<summary>You have not replied to $s1's invitation. The offer has been cancelled.</summary>
            YOU_DID_NOT_REPLY_TO_S1_INVITE = 136,
            ///<summary>There are no more items in the shortcut.</summary>
            NO_MORE_ITEMS_SHORTCUT = 137,
            ///<summary>Designate shortcut.</summary>
            DESIGNATE_SHORTCUT = 138,
            ///<summary>$s1 has resisted your $s2.</summary>
            S1_RESISTED_YOUR_S2 = 139,
            ///<summary>Your skill was removed due to a lack of MP.</summary>
            SKILL_REMOVED_DUE_LACK_MP = 140,
            ///<summary>Once the trade is confirmed, the item cannot be moved again.</summary>
            ONCE_THE_TRADE_IS_CONFIRMED_THE_ITEM_CANNOT_BE_MOVED_AGAIN = 141,
            ///<summary>You are already trading with someone.</summary>
            ALREADY_TRADING = 142,
            ///<summary>$s1 is already trading with another person. Please try again later.</summary>
            S1_ALREADY_TRADING = 143,
            ///<summary>That is the incorrect target.</summary>
            TARGET_IS_INCORRECT = 144,
            ///<summary>That player is not online.</summary>
            TARGET_IS_NOT_FOUND_IN_THE_GAME = 145,
            ///<summary>Chatting is now permitted.</summary>
            CHATTING_PERMITTED = 146,
            ///<summary>Chatting is currently prohibited.</summary>
            CHATTING_PROHIBITED = 147,
            ///<summary>You cannot use quest items.</summary>
            CANNOT_USE_QUEST_ITEMS = 148,
            ///<summary>You cannot pick up or use items while trading.</summary>
            CANNOT_PICKUP_OR_USE_ITEM_WHILE_TRADING = 149,
            ///<summary>You cannot discard or destroy an item while trading at a private store.</summary>
            CANNOT_DISCARD_OR_DESTROY_ITEM_WHILE_TRADING = 150,
            ///<summary>That is too far from you to discard.</summary>
            CANNOT_DISCARD_DISTANCE_TOO_FAR = 151,
            ///<summary>You have invited the wrong target.</summary>
            YOU_HAVE_INVITED_THE_WRONG_TARGET = 152,
            ///<summary>$s1 is on another task. Please try again later.</summary>
            S1_IS_BUSY_TRY_LATER = 153,
            ///<summary>Only the leader can give out invitations.</summary>
            ONLY_LEADER_CAN_INVITE = 154,
            ///<summary>The party is full.</summary>
            PARTY_FULL = 155,
            ///<summary>Drain was only 50 percent successful.</summary>
            DRAIN_HALF_SUCCESFUL = 156,
            ///<summary>You resisted $s1's drain.</summary>
            RESISTED_S1_DRAIN = 157,
            ///<summary>Your attack has failed.</summary>
            ATTACK_FAILED = 158,
            ///<summary>You resisted $s1's magic.</summary>
            RESISTED_S1_MAGIC = 159,
            ///<summary>$s1 is a member of another party and cannot be invited.</summary>
            S1_IS_ALREADY_IN_PARTY = 160,
            ///<summary>That player is not currently online.</summary>
            INVITED_USER_NOT_ONLINE = 161,
            ///<summary>Warehouse is too far.</summary>
            WAREHOUSE_TOO_FAR = 162,
            ///<summary>You cannot destroy it because the number is incorrect.</summary>
            CANNOT_DESTROY_NUMBER_INCORRECT = 163,
            ///<summary>Waiting for another reply.</summary>
            WAITING_FOR_ANOTHER_REPLY = 164,
            ///<summary>You cannot add yourself to your own friend list.</summary>
            YOU_CANNOT_ADD_YOURSELF_TO_OWN_FRIEND_LIST = 165,
            ///<summary>Friend list is not ready yet. Please register again later.</summary>
            FRIEND_LIST_NOT_READY_YET_REGISTER_LATER = 166,
            ///<summary>$s1 is already on your friend list.</summary>
            S1_ALREADY_ON_FRIEND_LIST = 167,
            ///<summary>$s1 has sent a friend request.</summary>
            S1_REQUESTED_TO_BECOME_FRIENDS = 168,
            ///<summary>Accept friendship 0/1 (1 to accept, 0 to deny)</summary>
            ACCEPT_THE_FRIENDSHIP = 169,
            ///<summary>The user who requested to become friends is not found in the game.</summary>
            THE_USER_YOU_REQUESTED_IS_NOT_IN_GAME = 170,
            ///<summary>$s1 is not on your friend list.</summary>
            S1_NOT_ON_YOUR_FRIENDS_LIST = 171,
            ///<summary>You lack the funds needed to pay for this transaction.</summary>
            LACK_FUNDS_FOR_TRANSACTION1 = 172,
            ///<summary>You lack the funds needed to pay for this transaction.</summary>
            LACK_FUNDS_FOR_TRANSACTION2 = 173,
            ///<summary>That person's inventory is full.</summary>
            OTHER_INVENTORY_FULL = 174,
            ///<summary>That skill has been de-activated as HP was fully recovered.</summary>
            SKILL_DEACTIVATED_HP_FULL = 175,
            ///<summary>That person is in message refusal mode.</summary>
            THE_PERSON_IS_IN_MESSAGE_REFUSAL_MODE = 176,
            ///<summary>Message refusal mode.</summary>
            MESSAGE_REFUSAL_MODE = 177,
            ///<summary>Message acceptance mode.</summary>
            MESSAGE_ACCEPTANCE_MODE = 178,
            ///<summary>You cannot discard those items here.</summary>
            CANT_DISCARD_HERE = 179,
            ///<summary>You have $s1 day(s) left until deletion. Do you wish to cancel this action?</summary>
            S1_DAYS_LEFT_CANCEL_ACTION = 180,
            ///<summary>Cannot see target.</summary>
            CANT_SEE_TARGET = 181,
            ///<summary>Do you want to quit the current quest?</summary>
            WANT_QUIT_CURRENT_QUEST = 182,
            ///<summary>There are too many users on the server. Please try again later</summary>
            TOO_MANY_USERS = 183,
            ///<summary>Please try again later.</summary>
            TRY_AGAIN_LATER = 184,
            ///<summary>You must first select a user to invite to your party.</summary>
            FIRST_SELECT_USER_TO_INVITE_TO_PARTY = 185,
            ///<summary>You must first select a user to invite to your clan.</summary>
            FIRST_SELECT_USER_TO_INVITE_TO_CLAN = 186,
            ///<summary>Select user to expel.</summary>
            SELECT_USER_TO_EXPEL = 187,
            ///<summary>Please create your clan name.</summary>
            PLEASE_CREATE_CLAN_NAME = 188,
            ///<summary>Your clan has been created.</summary>
            CLAN_CREATED = 189,
            ///<summary>You have failed to create a clan.</summary>
            FAILED_TO_CREATE_CLAN = 190,
            ///<summary>Clan member $s1 has been expelled.</summary>
            CLAN_MEMBER_S1_EXPELLED = 191,
            ///<summary>You have failed to expel $s1 from the clan.</summary>
            FAILED_EXPEL_S1 = 192,
            ///<summary>Clan has dispersed.</summary>
            CLAN_HAS_DISPERSED = 193,
            ///<summary>You have failed to disperse the clan.</summary>
            FAILED_TO_DISPERSE_CLAN = 194,
            ///<summary>Entered the clan.</summary>
            ENTERED_THE_CLAN = 195,
            ///<summary>$s1 declined your clan invitation.</summary>
            S1_REFUSED_TO_JOIN_CLAN = 196,
            ///<summary>You have withdrawn from the clan.</summary>
            YOU_HAVE_WITHDRAWN_FROM_CLAN = 197,
            ///<summary>You have failed to withdraw from the $s1 clan.</summary>
            FAILED_TO_WITHDRAW_FROM_S1_CLAN = 198,
            ///<summary>You have recently been dismissed from a clan. You are not allowed to join another clan for 24-hours.</summary>
            CLAN_MEMBERSHIP_TERMINATED = 199,
            ///<summary>You have withdrawn from the party.</summary>
            YOU_LEFT_PARTY = 200,
            ///<summary>$s1 was expelled from the party.</summary>
            S1_WAS_EXPELLED_FROM_PARTY = 201,
            ///<summary>You have been expelled from the party.</summary>
            HAVE_BEEN_EXPELLED_FROM_PARTY = 202,
            ///<summary>The party has dispersed.</summary>
            PARTY_DISPERSED = 203,
            ///<summary>Incorrect name. Please try again.</summary>
            INCORRECT_NAME_TRY_AGAIN = 204,
            ///<summary>Incorrect character name. Please try again.</summary>
            INCORRECT_CHARACTER_NAME_TRY_AGAIN = 205,
            ///<summary>Please enter the name of the clan you wish to declare war on.</summary>
            ENTER_CLAN_NAME_TO_DECLARE_WAR = 206,
            ///<summary>$s2 of the clan $s1 requests declaration of war. Do you accept?</summary>
            S2_OF_THE_CLAN_S1_REQUESTS_WAR = 207,
            ///<summary>You are not a clan member and cannot perform this action.</summary>
            YOU_ARE_NOT_A_CLAN_MEMBER = 212,
            ///<summary>Not working. Please try again later.</summary>
            NOT_WORKING_PLEASE_TRY_AGAIN_LATER = 213,
            ///<summary>Your title has been changed.</summary>
            TITLE_CHANGED = 214,
            ///<summary>War with the $s1 clan has begun.</summary>
            WAR_WITH_THE_S1_CLAN_HAS_BEGUN = 215,
            ///<summary>War with the $s1 clan has ended.</summary>
            WAR_WITH_THE_S1_CLAN_HAS_ENDED = 216,
            ///<summary>You have won the war over the $s1 clan!</summary>
            YOU_HAVE_WON_THE_WAR_OVER_THE_S1_CLAN = 217,
            ///<summary>You have surrendered to the $s1 clan.</summary>
            YOU_HAVE_SURRENDERED_TO_THE_S1_CLAN = 218,
            ///<summary>Your clan leader has died. You have been defeated by the $s1 clan.</summary>
            YOU_WERE_DEFEATED_BY_S1_CLAN = 219,
            ///<summary>You have $s1 minutes left until the clan war ends.</summary>
            S1_MINUTES_LEFT_UNTIL_CLAN_WAR_ENDS = 220,
            ///<summary>The time limit for the clan war is up. War with the $s1 clan is over.</summary>
            CLAN_WAR_WITH_S1_CLAN_HAS_ENDED = 221,
            ///<summary>$s1 has joined the clan.</summary>
            S1_HAS_JOINED_CLAN = 222,
            ///<summary>$s1 has withdrawn from the clan.</summary>
            S1_HAS_WITHDRAWN_FROM_THE_CLAN = 223,
            ///<summary>$s1 did not respond: Invitation to the clan has been cancelled.</summary>
            S1_DID_NOT_RESPOND_TO_CLAN_INVITATION = 224,
            ///<summary>You didn't respond to $s1's invitation: joining has been cancelled.</summary>
            YOU_DID_NOT_RESPOND_TO_S1_CLAN_INVITATION = 225,
            ///<summary>The $s1 clan did not respond: war proclamation has been refused.</summary>
            S1_CLAN_DID_NOT_RESPOND = 226,
            ///<summary>Clan war has been refused because you did not respond to $s1 clan's war proclamation.</summary>
            CLAN_WAR_REFUSED_YOU_DID_NOT_RESPOND_TO_S1 = 227,
            ///<summary>Request to end war has been denied.</summary>
            REQUEST_TO_END_WAR_HAS_BEEN_DENIED = 228,
            ///<summary>You do not meet the criteria in order to create a clan.</summary>
            YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_CLAN = 229,
            ///<summary>You must wait 10 days before creating a new clan.</summary>
            YOU_MUST_WAIT_XX_DAYS_BEFORE_CREATING_A_NEW_CLAN = 230,
            ///<summary>After a clan member is dismissed from a clan, the clan must wait at least a day before accepting a new member.</summary>
            YOU_MUST_WAIT_BEFORE_ACCEPTING_A_NEW_MEMBER = 231,
            ///<summary>After leaving or having been dismissed from a clan, you must wait at least a day before joining another clan.</summary>
            YOU_MUST_WAIT_BEFORE_JOINING_ANOTHER_CLAN = 232,
            ///<summary>The Academy/Royal Guard/Order of Knights is full and cannot accept new members at this time.</summary>
            SUBCLAN_IS_FULL = 233,
            ///<summary>The target must be a clan member.</summary>
            TARGET_MUST_BE_IN_CLAN = 234,
            ///<summary>You are not authorized to bestow these rights.</summary>
            NOT_AUTHORIZED_TO_BESTOW_RIGHTS = 235,
            ///<summary>Only the clan leader is enabled.</summary>
            ONLY_THE_CLAN_LEADER_IS_ENABLED = 236,
            ///<summary>The clan leader could not be found.</summary>
            CLAN_LEADER_NOT_FOUND = 237,
            ///<summary>Not joined in any clan.</summary>
            NOT_JOINED_IN_ANY_CLAN = 238,
            ///<summary>The clan leader cannot withdraw.</summary>
            CLAN_LEADER_CANNOT_WITHDRAW = 239,
            ///<summary>Currently involved in clan war.</summary>
            CURRENTLY_INVOLVED_IN_CLAN_WAR = 240,
            ///<summary>Leader of the $s1 Clan is not logged in.</summary>
            LEADER_OF_S1_CLAN_NOT_FOUND = 241,
            ///<summary>Select target.</summary>
            SELECT_TARGET = 242,
            ///<summary>You cannot declare war on an allied clan.</summary>
            CANNOT_DECLARE_WAR_ON_ALLIED_CLAN = 243,
            ///<summary>You are not allowed to issue this challenge.</summary>
            NOT_ALLOWED_TO_CHALLENGE = 244,
            ///<summary>5 days has not passed since you were refused war. Do you wish to continue?</summary>
            FIVE_DAYS_NOT_PASSED_SINCE_REFUSED_WAR = 245,
            ///<summary>That clan is currently at war.</summary>
            CLAN_CURRENTLY_AT_WAR = 246,
            ///<summary>You have already been at war with the $s1 clan: 5 days must pass before you can challenge this clan again</summary>
            FIVE_DAYS_MUST_PASS_BEFORE_CHALLENGE_S1_AGAIN = 247,
            ///<summary>You cannot proclaim war: the $s1 clan does not have enough members.</summary>
            S1_CLAN_NOT_ENOUGH_MEMBERS_FOR_WAR = 248,
            ///<summary>Do you wish to surrender to the $s1 clan?</summary>
            WISH_SURRENDER_TO_S1_CLAN = 249,
            ///<summary>You have personally surrendered to the $s1 clan. You are no longer participating in this clan war.</summary>
            YOU_HAVE_PERSONALLY_SURRENDERED_TO_THE_S1_CLAN = 250,
            ///<summary>You cannot proclaim war: you are at war with another clan.</summary>
            ALREADY_AT_WAR_WITH_ANOTHER_CLAN = 251,
            ///<summary>Enter the clan name to surrender to.</summary>
            ENTER_CLAN_NAME_TO_SURRENDER_TO = 252,
            ///<summary>Enter the name of the clan you wish to end the war with.</summary>
            ENTER_CLAN_NAME_TO_END_WAR = 253,
            ///<summary>A clan leader cannot personally surrender.</summary>
            LEADER_CANT_PERSONALLY_SURRENDER = 254,
            ///<summary>The $s1 clan has requested to end war. Do you agree?</summary>
            S1_CLAN_REQUESTED_END_WAR = 255,
            ///<summary>Enter title</summary>
            ENTER_TITLE = 256,
            ///<summary>Do you offer the $s1 clan a proposal to end the war?</summary>
            DO_YOU_OFFER_S1_CLAN_END_WAR = 257,
            ///<summary>You are not involved in a clan war.</summary>
            NOT_INVOLVED_CLAN_WAR = 258,
            ///<summary>Select clan members from list.</summary>
            SELECT_MEMBERS_FROM_LIST = 259,
            ///<summary>Fame level has decreased: 5 days have not passed since you were refused war</summary>
            FIVE_DAYS_NOT_PASSED_SINCE_YOU_WERE_REFUSED_WAR = 260,
            ///<summary>Clan name is invalid.</summary>
            CLAN_NAME_INVALID = 261,
            ///<summary>Clan name's length is incorrect.</summary>
            CLAN_NAME_LENGTH_INCORRECT = 262,
            ///<summary>You have already requested the dissolution of your clan.</summary>
            DISSOLUTION_IN_PROGRESS = 263,
            ///<summary>You cannot dissolve a clan while engaged in a war.</summary>
            CANNOT_DISSOLVE_WHILE_IN_WAR = 264,
            ///<summary>You cannot dissolve a clan during a siege or while protecting a castle.</summary>
            CANNOT_DISSOLVE_WHILE_IN_SIEGE = 265,
            ///<summary>You cannot dissolve a clan while owning a clan hall or castle.</summary>
            CANNOT_DISSOLVE_WHILE_OWNING_CLAN_HALL_OR_CASTLE = 266,
            ///<summary>There are no requests to disperse.</summary>
            NO_REQUESTS_TO_DISPERSE = 267,
            ///<summary>That player already belongs to another clan.</summary>
            PLAYER_ALREADY_ANOTHER_CLAN = 268,
            ///<summary>You cannot dismiss yourself.</summary>
            YOU_CANNOT_DISMISS_YOURSELF = 269,
            ///<summary>You have already surrendered.</summary>
            YOU_HAVE_ALREADY_SURRENDERED = 270,
            ///<summary>A player can only be granted a title if the clan is level 3 or above</summary>
            CLAN_LVL_3_NEEDED_TO_ENDOWE_TITLE = 271,
            ///<summary>A clan crest can only be registered when the clan's skill level is 3 or above.</summary>
            CLAN_LVL_3_NEEDED_TO_SET_CREST = 272,
            ///<summary>A clan war can only be declared when a clan's skill level is 3 or above.</summary>
            CLAN_LVL_3_NEEDED_TO_DECLARE_WAR = 273,
            ///<summary>Your clan's skill level has increased.</summary>
            CLAN_LEVEL_INCREASED = 274,
            ///<summary>Clan has failed to increase skill level.</summary>
            CLAN_LEVEL_INCREASE_FAILED = 275,
            ///<summary>You do not have the necessary materials or prerequisites to learn this skill.</summary>
            ITEM_MISSING_TO_LEARN_SKILL = 276,
            ///<summary>You have earned $s1.</summary>
            LEARNED_SKILL_S1 = 277,
            ///<summary>You do not have enough SP to learn this skill.</summary>
            NOT_ENOUGH_SP_TO_LEARN_SKILL = 278,
            ///<summary>You do not have enough adena.</summary>
            YOU_NOT_ENOUGH_ADENA = 279,
            ///<summary>You do not have any items to sell.</summary>
            NO_ITEMS_TO_SELL = 280,
            ///<summary>You do not have enough adena to pay the fee.</summary>
            YOU_NOT_ENOUGH_ADENA_PAY_FEE = 281,
            ///<summary>You have not deposited any items in your warehouse.</summary>
            NO_ITEM_DEPOSITED_IN_WH = 282,
            ///<summary>You have entered a combat zone.</summary>
            ENTERED_COMBAT_ZONE = 283,
            ///<summary>You have left a combat zone.</summary>
            LEFT_COMBAT_ZONE = 284,
            ///<summary>Clan $s1 has succeeded in engraving the ruler!</summary>
            CLAN_S1_ENGRAVED_RULER = 285,
            ///<summary>Your base is being attacked.</summary>
            BASE_UNDER_ATTACK = 286,
            ///<summary>The opposing clan has stared to engrave to monument!</summary>
            OPPONENT_STARTED_ENGRAVING = 287,
            ///<summary>The castle gate has been broken down.</summary>
            CASTLE_GATE_BROKEN_DOWN = 288,
            ///<summary>An outpost or headquarters cannot be built because at least one already exists.</summary>
            NOT_ANOTHER_HEADQUARTERS = 289,
            ///<summary>You cannot set up a base here.</summary>
            NOT_SET_UP_BASE_HERE = 290,
            ///<summary>Clan $s1 is victorious over $s2's castle siege!</summary>
            CLAN_S1_VICTORIOUS_OVER_S2_S_SIEGE = 291,
            ///<summary>$s1 has announced the castle siege time.</summary>
            S1_ANNOUNCED_SIEGE_TIME = 292,
            ///<summary>The registration term for $s1 has ended.</summary>
            REGISTRATION_TERM_FOR_S1_ENDED = 293,
            ///<summary>Because your clan is not currently on the offensive in a Clan Hall siege war, it cannot summon its base camp.</summary>
            BECAUSE_YOUR_CLAN_IS_NOT_CURRENTLY_ON_THE_OFFENSIVE_IN_A_CLAN_HALL_SIEGE_WAR_IT_CANNOT_SUMMON_ITS_BASE_CAMP = 294,
            ///<summary>$s1's siege was canceled because there were no clans that participated.</summary>
            S1_SIEGE_WAS_CANCELED_BECAUSE_NO_CLANS_PARTICIPATED = 295,
            ///<summary>You received $s1 damage from taking a high fall.</summary>
            FALL_DAMAGE_S1 = 296,
            ///<summary>You have taken $s1 damage because you were unable to breathe.</summary>
            DROWN_DAMAGE_S1 = 297,
            ///<summary>You have dropped $s1.</summary>
            YOU_DROPPED_S1 = 298,
            ///<summary>$s1 has obtained $s3 $s2.</summary>
            S1_OBTAINED_S3_S2 = 299,
            ///<summary>$s1 has obtained $s2.</summary>
            S1_OBTAINED_S2 = 300,
            ///<summary>$s2 $s1 has disappeared.</summary>
            S2_S1_DISAPPEARED = 301,
            ///<summary>$s1 has disappeared.</summary>
            S1_DISAPPEARED = 302,
            ///<summary>Select item to enchant.</summary>
            SELECT_ITEM_TO_ENCHANT = 303,
            ///<summary>Clan member $s1 has logged into game.</summary>
            CLAN_MEMBER_S1_LOGGED_IN = 304,
            ///<summary>The player declined to join your party.</summary>
            PLAYER_DECLINED = 305,
            ///<summary>You have succeeded in expelling the clan member.</summary>
            YOU_HAVE_SUCCEEDED_IN_EXPELLING_CLAN_MEMBER = 309,
            ///<summary>The clan war declaration has been accepted.</summary>
            CLAN_WAR_DECLARATION_ACCEPTED = 311,
            ///<summary>The clan war declaration has been refused.</summary>
            CLAN_WAR_DECLARATION_REFUSED = 312,
            ///<summary>The cease war request has been accepted.</summary>
            CEASE_WAR_REQUEST_ACCEPTED = 313,
            ///<summary>You have failed to surrender.</summary>
            FAILED_TO_SURRENDER = 314,
            ///<summary>You have failed to personally surrender.</summary>
            FAILED_TO_PERSONALLY_SURRENDER = 315,
            ///<summary>You have failed to withdraw from the party.</summary>
            FAILED_TO_WITHDRAW_FROM_THE_PARTY = 316,
            ///<summary>You have failed to expel the party member.</summary>
            FAILED_TO_EXPEL_THE_PARTY_MEMBER = 317,
            ///<summary>You have failed to disperse the party.</summary>
            FAILED_TO_DISPERSE_THE_PARTY = 318,
            ///<summary>This door cannot be unlocked.</summary>
            UNABLE_TO_UNLOCK_DOOR = 319,
            ///<summary>You have failed to unlock the door.</summary>
            FAILED_TO_UNLOCK_DOOR = 320,
            ///<summary>It is not locked.</summary>
            ITS_NOT_LOCKED = 321,
            ///<summary>Please decide on the sales price.</summary>
            DECIDE_SALES_PRICE = 322,
            ///<summary>Your force has increased to $s1 level.</summary>
            FORCE_INCREASED_TO_S1 = 323,
            ///<summary>Your force has reached maximum capacity.</summary>
            FORCE_MAXLEVEL_REACHED = 324,
            ///<summary>The corpse has already disappeared.</summary>
            CORPSE_ALREADY_DISAPPEARED = 325,
            ///<summary>Select target from list.</summary>
            SELECT_TARGET_FROM_LIST = 326,
            ///<summary>You cannot exceed 80 characters.</summary>
            CANNOT_EXCEED_80_CHARACTERS = 327,
            ///<summary>Please input title using less than 128 characters.</summary>
            PLEASE_INPUT_TITLE_LESS_128_CHARACTERS = 328,
            ///<summary>Please input content using less than 3000 characters.</summary>
            PLEASE_INPUT_CONTENT_LESS_3000_CHARACTERS = 329,
            ///<summary>A one-line response may not exceed 128 characters.</summary>
            ONE_LINE_RESPONSE_NOT_EXCEED_128_CHARACTERS = 330,
            ///<summary>You have acquired $s1 SP.</summary>
            ACQUIRED_S1_SP = 331,
            ///<summary>Do you want to be restored?</summary>
            DO_YOU_WANT_TO_BE_RESTORED = 332,
            ///<summary>You have received $s1 damage by Core's barrier.</summary>
            S1_DAMAGE_BY_CORE_BARRIER = 333,
            ///<summary>Please enter your private store display message.</summary>
            ENTER_PRIVATE_STORE_MESSAGE = 334,
            ///<summary>$s1 has been aborted.</summary>
            S1_HAS_BEEN_ABORTED = 335,
            ///<summary>You are attempting to crystallize $s1. Do you wish to continue?</summary>
            WISH_TO_CRYSTALLIZE_S1 = 336,
            ///<summary>The soulshot you are attempting to use does not match the grade of your equipped weapon.</summary>
            SOULSHOTS_GRADE_MISMATCH = 337,
            ///<summary>You do not have enough soulshots for that.</summary>
            NOT_ENOUGH_SOULSHOTS = 338,
            ///<summary>Cannot use soulshots.</summary>
            CANNOT_USE_SOULSHOTS = 339,
            ///<summary>Your private store is now open for business.</summary>
            PRIVATE_STORE_UNDER_WAY = 340,
            ///<summary>You do not have enough materials to perform that action.</summary>
            NOT_ENOUGH_MATERIALS = 341,
            ///<summary>Power of the spirits enabled.</summary>
            ENABLED_SOULSHOT = 342,
            ///<summary>Sweeper failed, target not spoiled.</summary>
            SWEEPER_FAILED_TARGET_NOT_SPOILED = 343,
            ///<summary>Power of the spirits disabled.</summary>
            SOULSHOTS_DISABLED = 344,
            ///<summary>Chat enabled.</summary>
            CHAT_ENABLED = 345,
            ///<summary>Chat disabled.</summary>
            CHAT_DISABLED = 346,
            ///<summary>Incorrect item count.</summary>
            INCORRECT_ITEM_COUNT = 347,
            ///<summary>Incorrect item price.</summary>
            INCORRECT_ITEM_PRICE = 348,
            ///<summary>Private store already closed.</summary>
            PRIVATE_STORE_ALREADY_CLOSED = 349,
            ///<summary>Item out of stock.</summary>
            ITEM_OUT_OF_STOCK = 350,
            ///<summary>Incorrect item count.</summary>
            NOT_ENOUGH_ITEMS = 351,
            ///<summary>Cancel enchant.</summary>
            CANCEL_ENCHANT = 354,
            ///<summary>Inappropriate enchant conditions.</summary>
            INAPPROPRIATE_ENCHANT_CONDITION = 355,
            ///<summary>Reject resurrection.</summary>
            REJECT_RESURRECTION = 356,
            ///<summary>It has already been spoiled.</summary>
            ALREADY_SPOILED = 357,
            ///<summary>$s1 hour(s) until catle siege conclusion.</summary>
            S1_HOURS_UNTIL_SIEGE_CONCLUSION = 358,
            ///<summary>$s1 minute(s) until catle siege conclusion.</summary>
            S1_MINUTES_UNTIL_SIEGE_CONCLUSION = 359,
            ///<summary>Castle siege $s1 second(s) left!</summary>
            CASTLE_SIEGE_S1_SECONDS_LEFT = 360,
            ///<summary>Over-hit!</summary>
            OVER_HIT = 361,
            ///<summary>You have acquired $s1 bonus experience from a successful over-hit.</summary>
            ACQUIRED_BONUS_EXPERIENCE_THROUGH_OVER_HIT = 362,
            ///<summary>Chat available time: $s1 minute.</summary>
            CHAT_AVAILABLE_S1_MINUTE = 363,
            ///<summary>Enter user's name to search</summary>
            ENTER_USER_NAME_TO_SEARCH = 364,
            ///<summary>Are you sure?</summary>
            ARE_YOU_SURE = 365,
            ///<summary>Please select your hair color.</summary>
            PLEASE_SELECT_HAIR_COLOR = 366,
            ///<summary>You cannot remove that clan character at this time.</summary>
            CANNOT_REMOVE_CLAN_CHARACTER = 367,
            ///<summary>Equipped +$s1 $s2.</summary>
            S1_S2_EQUIPPED = 368,
            ///<summary>You have obtained a +$s1 $s2.</summary>
            YOU_PICKED_UP_A_S1_S2 = 369,
            ///<summary>Failed to pickup $s1.</summary>
            FAILED_PICKUP_S1 = 370,
            ///<summary>Acquired +$s1 $s2.</summary>
            ACQUIRED_S1_S2 = 371,
            ///<summary>Failed to earn $s1.</summary>
            FAILED_EARN_S1 = 372,
            ///<summary>You are trying to destroy +$s1 $s2. Do you wish to continue?</summary>
            WISH_DESTROY_S1_S2 = 373,
            ///<summary>You are attempting to crystallize +$s1 $s2. Do you wish to continue?</summary>
            WISH_CRYSTALLIZE_S1_S2 = 374,
            ///<summary>You have dropped +$s1 $s2 .</summary>
            DROPPED_S1_S2 = 375,
            ///<summary>$s1 has obtained +$s2$s3.</summary>
            S1_OBTAINED_S2_S3 = 376,
            ///<summary>$S1 $S2 disappeared.</summary>
            S1_S2_DISAPPEARED = 377,
            ///<summary>$s1 purchased $s2.</summary>
            S1_PURCHASED_S2 = 378,
            ///<summary>$s1 purchased +$s2$s3.</summary>
            S1_PURCHASED_S2_S3 = 379,
            ///<summary>$s1 purchased $s3 $s2(s).</summary>
            S1_PURCHASED_S3_S2_S = 380,
            ///<summary>The game client encountered an error and was unable to connect to the petition server.</summary>
            GAME_CLIENT_UNABLE_TO_CONNECT_TO_PETITION_SERVER = 381,
            ///<summary>Currently there are no users that have checked out a GM ID.</summary>
            NO_USERS_CHECKED_OUT_GM_ID = 382,
            ///<summary>Request confirmed to end consultation at petition server.</summary>
            REQUEST_CONFIRMED_TO_END_CONSULTATION = 383,
            ///<summary>The client is not logged onto the game server.</summary>
            CLIENT_NOT_LOGGED_ONTO_GAME_SERVER = 384,
            ///<summary>Request confirmed to begin consultation at petition server.</summary>
            REQUEST_CONFIRMED_TO_BEGIN_CONSULTATION = 385,
            ///<summary>The body of your petition must be more than five characters in length.</summary>
            PETITION_MORE_THAN_FIVE_CHARACTERS = 386,
            ///<summary>This ends the GM petition consultation. Please take a moment to provide feedback about this service.</summary>
            THIS_END_THE_PETITION_PLEASE_PROVIDE_FEEDBACK = 387,
            ///<summary>Not under petition consultation.</summary>
            NOT_UNDER_PETITION_CONSULTATION = 388,
            ///<summary>our petition application has been accepted. - Receipt No. is $s1.</summary>
            PETITION_ACCEPTED_RECENT_NO_S1 = 389,
            ///<summary>You may only submit one petition (active) at a time.</summary>
            ONLY_ONE_ACTIVE_PETITION_AT_TIME = 390,
            ///<summary>Receipt No. $s1, petition cancelled.</summary>
            RECENT_NO_S1_CANCELED = 391,
            ///<summary>Under petition advice.</summary>
            UNDER_PETITION_ADVICE = 392,
            ///<summary>Failed to cancel petition. Please try again later.</summary>
            FAILED_CANCEL_PETITION_TRY_LATER = 393,
            ///<summary>Petition consultation with $s1, under way.</summary>
            PETITION_WITH_S1_UNDER_WAY = 394,
            ///<summary>Ending petition consultation with $s1.</summary>
            PETITION_ENDED_WITH_S1 = 395,
            ///<summary>Please login after changing your temporary password.</summary>
            TRY_AGAIN_AFTER_CHANGING_PASSWORD = 396,
            ///<summary>Not a paid account.</summary>
            NO_PAID_ACCOUNT = 397,
            ///<summary>There is no time left on this account.</summary>
            NO_TIME_LEFT_ON_ACCOUNT = 398,
            ///<summary>You are attempting to drop $s1. Dou you wish to continue?</summary>
            WISH_TO_DROP_S1 = 400,
            ///<summary>You have to many ongoing quests.</summary>
            TOO_MANY_QUESTS = 401,
            ///<summary>You do not possess the correct ticket to board the boat.</summary>
            NOT_CORRECT_BOAT_TICKET = 402,
            ///<summary>You have exceeded your out-of-pocket adena limit.</summary>
            EXCEECED_POCKET_ADENA_LIMIT = 403,
            ///<summary>Your Create Item level is too low to register this recipe.</summary>
            CREATE_LVL_TOO_LOW_TO_REGISTER = 404,
            ///<summary>The total price of the product is too high.</summary>
            TOTAL_PRICE_TOO_HIGH = 405,
            ///<summary>Petition application accepted.</summary>
            PETITION_APP_ACCEPTED = 406,
            ///<summary>Petition under process.</summary>
            PETITION_UNDER_PROCESS = 407,
            ///<summary>Set Period</summary>
            SET_PERIOD = 408,
            ///<summary>Set Time-$s1:$s2:$s3</summary>
            SET_TIME_S1_S2_S3 = 409,
            ///<summary>Registration Period</summary>
            REGISTRATION_PERIOD = 410,
            ///<summary>Registration Time-$s1:$s2:$s3</summary>
            REGISTRATION_TIME_S1_S2_S3 = 411,
            ///<summary>Battle begins in $s1:$s2:$s3</summary>
            BATTLE_BEGINS_S1_S2_S3 = 412,
            ///<summary>Battle ends in $s1:$s2:$s3</summary>
            BATTLE_ENDS_S1_S2_S3 = 413,
            ///<summary>Standby</summary>
            STANDBY = 414,
            ///<summary>Under Siege</summary>
            UNDER_SIEGE = 415,
            ///<summary>This item cannot be exchanged.</summary>
            ITEM_CANNOT_EXCHANGE = 416,
            ///<summary>$s1 has been disarmed.</summary>
            S1_DISARMED = 417,
            ///<summary>$s1 minute(s) of usage time left.</summary>
            S1_MINUTES_USAGE_LEFT = 419,
            ///<summary>Time expired.</summary>
            TIME_EXPIRED = 420,
            ///<summary>Another person has logged in with the same account.</summary>
            ANOTHER_LOGIN_WITH_ACCOUNT = 421,
            ///<summary>You have exceeded the weight limit.</summary>
            WEIGHT_LIMIT_EXCEEDED = 422,
            ///<summary>You have cancelled the enchanting process.</summary>
            ENCHANT_SCROLL_CANCELLED = 423,
            ///<summary>Does not fit strengthening conditions of the scroll.</summary>
            DOES_NOT_FIT_SCROLL_CONDITIONS = 424,
            ///<summary>Your Create Item level is too low to register this recipe.</summary>
            CREATE_LVL_TOO_LOW_TO_REGISTER2 = 425,
            ///<summary>(Reference Number Regarding Membership Withdrawal Request: $s1)</summary>
            REFERENCE_MEMBERSHIP_WITHDRAWAL_S1 = 445,
            ///<summary>.</summary>
            DOT = 447,
            ///<summary>There is a system error. Please log in again later.</summary>
            SYSTEM_ERROR_LOGIN_LATER = 448,
            ///<summary>The password you have entered is incorrect.</summary>
            PASSWORD_ENTERED_INCORRECT1 = 449,
            ///<summary>Confirm your account information and log in later.</summary>
            CONFIRM_ACCOUNT_LOGIN_LATER = 450,
            ///<summary>The password you have entered is incorrect.</summary>
            PASSWORD_ENTERED_INCORRECT2 = 451,
            ///<summary>Please confirm your account information and try logging in later.</summary>
            PLEASE_CONFIRM_ACCOUNT_LOGIN_LATER = 452,
            ///<summary>Your account information is incorrect.</summary>
            ACCOUNT_INFORMATION_INCORRECT = 453,
            ///<summary>Account is already in use. Unable to log in.</summary>
            ACCOUNT_IN_USE = 455,
            ///<summary>Lineage II game services may be used by individuals 15 years of age or older except for PvP servers,which may only be used by adults 18 years of age and older (Korea Only)</summary>
            LINAGE_MINIMUM_AGE = 456,
            ///<summary>Currently undergoing game server maintenance. Please log in again later.</summary>
            SERVER_MAINTENANCE = 457,
            ///<summary>Your usage term has expired.</summary>
            USAGE_TERM_EXPIRED = 458,
            ///<summary>to reactivate your account.</summary>
            TO_REACTIVATE_YOUR_ACCOUNT = 460,
            ///<summary>Access failed.</summary>
            ACCESS_FAILED = 461,
            ///<summary>Please try again later.</summary>
            PLEASE_TRY_AGAIN_LATER = 461,
            ///<summary>This feature is only available alliance leaders.</summary>
            FEATURE_ONLY_FOR_ALLIANCE_LEADER = 464,
            ///<summary>You are not currently allied with any clans.</summary>
            NO_CURRENT_ALLIANCES = 465,
            ///<summary>You have exceeded the limit.</summary>
            YOU_HAVE_EXCEEDED_THE_LIMIT = 466,
            ///<summary>You may not accept any clan within a day after expelling another clan.</summary>
            CANT_INVITE_CLAN_WITHIN_1_DAY = 467,
            ///<summary>A clan that has withdrawn or been expelled cannot enter into an alliance within one day of withdrawal or expulsion.</summary>
            CANT_ENTER_ALLIANCE_WITHIN_1_DAY = 468,
            ///<summary>You may not ally with a clan you are currently at war with. That would be diabolical and treacherous.</summary>
            MAY_NOT_ALLY_CLAN_BATTLE = 469,
            ///<summary>Only the clan leader may apply for withdrawal from the alliance.</summary>
            ONLY_CLAN_LEADER_WITHDRAW_ALLY = 470,
            ///<summary>Alliance leaders cannot withdraw.</summary>
            ALLIANCE_LEADER_CANT_WITHDRAW = 471,
            ///<summary>You cannot expel yourself from the clan.</summary>
            CANNOT_EXPEL_YOURSELF = 472,
            ///<summary>Different alliance.</summary>
            DIFFERENT_ALLIANCE = 473,
            ///<summary>That clan does not exist.</summary>
            CLAN_DOESNT_EXISTS = 474,
            ///<summary>Different alliance.</summary>
            DIFFERENT_ALLIANCE2 = 475,
            ///<summary>Please adjust the image size to 8x12.</summary>
            ADJUST_IMAGE_8_12 = 476,
            ///<summary>No response. Invitation to join an alliance has been cancelled.</summary>
            NO_RESPONSE_TO_ALLY_INVITATION = 477,
            ///<summary>No response. Your entrance to the alliance has been cancelled.</summary>
            YOU_DID_NOT_RESPOND_TO_ALLY_INVITATION = 478,
            ///<summary>$s1 has joined as a friend.</summary>
            S1_JOINED_AS_FRIEND = 479,
            ///<summary>Please check your friend list.</summary>
            PLEASE_CHECK_YOUR_FRIENDS_LIST = 480,
            ///<summary>$s1 has been deleted from your friends list.</summary>
            S1_HAS_BEEN_DELETED_FROM_YOUR_FRIENDS_LIST = 481,
            ///<summary>You cannot add yourself to your own friend list.</summary>
            YOU_CANNOT_ADD_YOURSELF_TO_YOUR_OWN_FRIENDS_LIST = 482,
            ///<summary>This function is inaccessible right now. Please try again later.</summary>
            FUNCTION_INACCESSIBLE_NOW = 483,
            ///<summary>This player is already registered in your friends list.</summary>
            S1_ALREADY_IN_FRIENDS_LIST = 484,
            ///<summary>No new friend invitations may be accepted.</summary>
            NO_NEW_INVITATIONS_ACCEPTED = 485,
            ///<summary>The following user is not in your friends list.</summary>
            THE_USER_NOT_IN_FRIENDS_LIST = 486,
            ///<summary>======<Friends List>======</summary>
            FRIEND_LIST_HEADER = 487,
            ///<summary>$s1 (Currently: Online)</summary>
            S1_ONLINE = 488,
            ///<summary>$s1 (Currently: Offline)</summary>
            S1_OFFLINE = 489,
            ///<summary>========================</summary>
            FRIEND_LIST_FOOTER = 490,
            ///<summary>=======<Alliance Information>=======</summary>
            ALLIANCE_INFO_HEAD = 491,
            ///<summary>Alliance Name: $s1</summary>
            ALLIANCE_NAME_S1 = 492,
            ///<summary>Connection: $s1 / Total $s2</summary>
            CONNECTION_S1_TOTAL_S2 = 493,
            ///<summary>Alliance Leader: $s2 of $s1</summary>
            ALLIANCE_LEADER_S2_OF_S1 = 494,
            ///<summary>Affiliated clans: Total $s1 clan(s)</summary>
            ALLIANCE_CLAN_TOTAL_S1 = 495,
            ///<summary>=====<Clan Information>=====</summary>
            CLAN_INFO_HEAD = 496,
            ///<summary>Clan Name: $s1</summary>
            CLAN_INFO_NAME_S1 = 497,
            ///<summary>Clan Leader: $s1</summary>
            CLAN_INFO_LEADER_S1 = 498,
            ///<summary>Clan Level: $s1</summary>
            CLAN_INFO_LEVEL_S1 = 499,
            ///<summary>------------------------</summary>
            CLAN_INFO_SEPARATOR = 500,
            ///<summary>========================</summary>
            CLAN_INFO_FOOT = 501,
            ///<summary>You already belong to another alliance.</summary>
            ALREADY_JOINED_ALLIANCE = 502,
            ///<summary>$s1 (Friend) has logged in.</summary>
            FRIEND_S1_HAS_LOGGED_IN = 503,
            ///<summary>Only clan leaders may create alliances.</summary>
            ONLY_CLAN_LEADER_CREATE_ALLIANCE = 504,
            ///<summary>You cannot create a new alliance within 10 days after dissolution.</summary>
            CANT_CREATE_ALLIANCE_10_DAYS_DISOLUTION = 505,
            ///<summary>Incorrect alliance name. Please try again.</summary>
            INCORRECT_ALLIANCE_NAME = 506,
            ///<summary>Incorrect length for an alliance name.</summary>
            INCORRECT_ALLIANCE_NAME_LENGTH = 507,
            ///<summary>This alliance name already exists.</summary>
            ALLIANCE_ALREADY_EXISTS = 508,
            ///<summary>Cannot accept. clan ally is registered as an enemy during siege battle.</summary>
            CANT_ACCEPT_ALLY_ENEMY_FOR_SIEGE = 509,
            ///<summary>You have invited someone to your alliance.</summary>
            YOU_INVITED_FOR_ALLIANCE = 510,
            ///<summary>You must first select a user to invite.</summary>
            SELECT_USER_TO_INVITE = 511,
            ///<summary>Do you really wish to withdraw from the alliance?</summary>
            DO_YOU_WISH_TO_WITHDRW = 512,
            ///<summary>Enter the name of the clan you wish to expel.</summary>
            ENTER_NAME_CLAN_TO_EXPEL = 513,
            ///<summary>Do you really wish to dissolve the alliance?</summary>
            DO_YOU_WISH_TO_DISOLVE = 514,
            ///<summary>$s1 has invited you to be their friend.</summary>
            SI_INVITED_YOU_AS_FRIEND = 516,
            ///<summary>You have accepted the alliance.</summary>
            YOU_ACCEPTED_ALLIANCE = 517,
            ///<summary>You have failed to invite a clan into the alliance.</summary>
            FAILED_TO_INVITE_CLAN_IN_ALLIANCE = 518,
            ///<summary>You have withdrawn from the alliance.</summary>
            YOU_HAVE_WITHDRAWN_FROM_ALLIANCE = 519,
            ///<summary>You have failed to withdraw from the alliance.</summary>
            YOU_HAVE_FAILED_TO_WITHDRAWN_FROM_ALLIANCE = 520,
            ///<summary>You have succeeded in expelling a clan.</summary>
            YOU_HAVE_EXPELED_A_CLAN = 521,
            ///<summary>You have failed to expel a clan.</summary>
            FAILED_TO_EXPELED_A_CLAN = 522,
            ///<summary>The alliance has been dissolved.</summary>
            ALLIANCE_DISOLVED = 523,
            ///<summary>You have failed to dissolve the alliance.</summary>
            FAILED_TO_DISOLVE_ALLIANCE = 524,
            ///<summary>You have succeeded in inviting a friend to your friends list.</summary>
            YOU_HAVE_SUCCEEDED_INVITING_FRIEND = 525,
            ///<summary>You have failed to add a friend to your friends list.</summary>
            FAILED_TO_INVITE_A_FRIEND = 526,
            ///<summary>$s1 leader, $s2, has requested an alliance.</summary>
            S2_ALLIANCE_LEADER_OF_S1_REQUESTED_ALLIANCE = 527,
            ///<summary>The Spiritshot does not match the weapon's grade.</summary>
            SPIRITSHOTS_GRADE_MISMATCH = 530,
            ///<summary>You do not have enough Spiritshots for that.</summary>
            NOT_ENOUGH_SPIRITSHOTS = 531,
            ///<summary>You may not use Spiritshots.</summary>
            CANNOT_USE_SPIRITSHOTS = 532,
            ///<summary>Power of Mana enabled.</summary>
            ENABLED_SPIRITSHOT = 533,
            ///<summary>Power of Mana disabled.</summary>
            DISABLED_SPIRITSHOT = 534,
            ///<summary>How much adena do you wish to transfer to your Inventory?</summary>
            HOW_MUCH_ADENA_TRANSFER = 536,
            ///<summary>How much will you transfer?</summary>
            HOW_MUCH_TRANSFER = 537,
            ///<summary>Your SP has decreased by $s1.</summary>
            SP_DECREASED_S1 = 538,
            ///<summary>Your Experience has decreased by $s1.</summary>
            EXP_DECREASED_BY_S1 = 539,
            ///<summary>Clan leaders may not be deleted. Dissolve the clan first and try again.</summary>
            CLAN_LEADERS_MAY_NOT_BE_DELETED = 540,
            ///<summary>You may not delete a clan member. Withdraw from the clan first and try again.</summary>
            CLAN_MEMBER_MAY_NOT_BE_DELETED = 541,
            ///<summary>The NPC server is currently down. Pets and servitors cannot be summoned at this time.</summary>
            THE_NPC_SERVER_IS_CURRENTLY_DOWN = 542,
            ///<summary>You already have a pet.</summary>
            YOU_ALREADY_HAVE_A_PET = 543,
            ///<summary>Your pet cannot carry this item.</summary>
            ITEM_NOT_FOR_PETS = 544,
            ///<summary>Your pet cannot carry any more items. Remove some, then try again.</summary>
            YOUR_PET_CANNOT_CARRY_ANY_MORE_ITEMS = 545,
            ///<summary>Unable to place item, your pet is too encumbered.</summary>
            UNABLE_TO_PLACE_ITEM_YOUR_PET_IS_TOO_ENCUMBERED = 546,
            ///<summary>Summoning your pet.</summary>
            SUMMON_A_PET = 547,
            ///<summary>Your pet's name can be up to 8 characters in length.</summary>
            NAMING_PETNAME_UP_TO_8CHARS = 548,
            ///<summary>To create an alliance, your clan must be Level 5 or higher.</summary>
            TO_CREATE_AN_ALLY_YOU_CLAN_MUST_BE_LEVEL_5_OR_HIGHER = 549,
            ///<summary>You may not create an alliance during the term of dissolution postponement.</summary>
            YOU_MAY_NOT_CREATE_ALLY_WHILE_DISSOLVING = 550,
            ///<summary>You cannot raise your clan level during the term of dispersion postponement.</summary>
            CANNOT_RISE_LEVEL_WHILE_DISSOLUTION_IN_PROGRESS = 551,
            ///<summary>During the grace period for dissolving a clan, the registration or deletion of a clan's crest is not allowed.</summary>
            CANNOT_SET_CREST_WHILE_DISSOLUTION_IN_PROGRESS = 552,
            ///<summary>The opposing clan has applied for dispersion.</summary>
            OPPOSING_CLAN_APPLIED_DISPERSION = 553,
            ///<summary>You cannot disperse the clans in your alliance.</summary>
            CANNOT_DISPERSE_THE_CLANS_IN_ALLY = 554,
            ///<summary>You cannot move - you are too encumbered</summary>
            CANT_MOVE_TOO_ENCUMBERED = 555,
            ///<summary>You cannot move in this state</summary>
            CANT_MOVE_IN_THIS_STATE = 556,
            ///<summary>Your pet has been summoned and may not be destroyed</summary>
            PET_SUMMONED_MAY_NOT_DESTROYED = 557,
            ///<summary>Your pet has been summoned and may not be let go.</summary>
            PET_SUMMONED_MAY_NOT_LET_GO = 558,
            ///<summary>You have purchased $s2 from $s1.</summary>
            PURCHASED_S2_FROM_S1 = 559,
            ///<summary>You have purchased +$s2 $s3 from $s1.</summary>
            PURCHASED_S2_S3_FROM_S1 = 560,
            ///<summary>You have purchased $s3 $s2(s) from $s1.</summary>
            PURCHASED_S3_S2_S_FROM_S1 = 561,
            ///<summary>You may not crystallize this item. Your crystallization skill level is too low.</summary>
            CRYSTALLIZE_LEVEL_TOO_LOW = 562,
            ///<summary>Failed to disable attack target.</summary>
            FAILED_DISABLE_TARGET = 563,
            ///<summary>Failed to change attack target.</summary>
            FAILED_CHANGE_TARGET = 564,
            ///<summary>Not enough luck.</summary>
            NOT_ENOUGH_LUCK = 565,
            ///<summary>Your confusion spell failed.</summary>
            CONFUSION_FAILED = 566,
            ///<summary>Your fear spell failed.</summary>
            FEAR_FAILED = 567,
            ///<summary>Cubic Summoning failed.</summary>
            CUBIC_SUMMONING_FAILED = 568,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Finders Keepers.)</summary>
            S1_INVITED_YOU_TO_PARTY_FINDERS_KEEPERS = 572,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Random.)</summary>
            S1_INVITED_YOU_TO_PARTY_RANDOM = 573,
            ///<summary>Pets and Servitors are not available at this time.</summary>
            PETS_ARE_NOT_AVAILABLE_AT_THIS_TIME = 574,
            ///<summary>How much adena do you wish to transfer to your pet?</summary>
            HOW_MUCH_ADENA_TRANSFER_TO_PET = 575,
            ///<summary>How much do you wish to transfer?</summary>
            HOW_MUCH_TRANSFER2 = 576,
            ///<summary>You cannot summon during a trade or while using the private shops.</summary>
            CANNOT_SUMMON_DURING_TRADE_SHOP = 577,
            ///<summary>You cannot summon during combat.</summary>
            YOU_CANNOT_SUMMON_IN_COMBAT = 578,
            ///<summary>A pet cannot be sent back during battle.</summary>
            PET_CANNOT_SENT_BACK_DURING_BATTLE = 579,
            ///<summary>You may not use multiple pets or servitors at the same time.</summary>
            SUMMON_ONLY_ONE = 580,
            ///<summary>There is a space in the name.</summary>
            NAMING_THERE_IS_A_SPACE = 581,
            ///<summary>Inappropriate character name.</summary>
            NAMING_INAPPROPRIATE_CHARACTER_NAME = 582,
            ///<summary>Name includes forbidden words.</summary>
            NAMING_INCLUDES_FORBIDDEN_WORDS = 583,
            ///<summary>This is already in use by another pet.</summary>
            NAMING_ALREADY_IN_USE_BY_ANOTHER_PET = 584,
            ///<summary>Please decide on the price.</summary>
            DECIDE_ON_PRICE = 585,
            ///<summary>Pet items cannot be registered as shortcuts.</summary>
            PET_NO_SHORTCUT = 586,
            ///<summary>Your pet's inventory is full.</summary>
            PET_INVENTORY_FULL = 588,
            ///<summary>A dead pet cannot be sent back.</summary>
            DEAD_PET_CANNOT_BE_RETURNED = 589,
            ///<summary>Your pet is motionless and any attempt you make to give it something goes unrecognized.</summary>
            CANNOT_GIVE_ITEMS_TO_DEAD_PET = 590,
            ///<summary>An invalid character is included in the pet's name.</summary>
            NAMING_PETNAME_CONTAINS_INVALID_CHARS = 591,
            ///<summary>Do you wish to dismiss your pet? Dismissing your pet will cause the pet necklace to disappear</summary>
            WISH_TO_DISMISS_PET = 592,
            ///<summary>Starving, grumpy and fed up, your pet has left.</summary>
            STARVING_GRUMPY_AND_FED_UP_YOUR_PET_HAS_LEFT = 593,
            ///<summary>You may not restore a hungry pet.</summary>
            YOU_CANNOT_RESTORE_HUNGRY_PETS = 594,
            ///<summary>Your pet is very hungry.</summary>
            YOUR_PET_IS_VERY_HUNGRY = 595,
            ///<summary>Your pet ate a little, but is still hungry.</summary>
            YOUR_PET_ATE_A_LITTLE_BUT_IS_STILL_HUNGRY = 596,
            ///<summary>Your pet is very hungry. Please be careful.</summary>
            YOUR_PET_IS_VERY_HUNGRY_PLEASE_BE_CAREFUL = 597,
            ///<summary>You may not chat while you are invisible.</summary>
            NOT_CHAT_WHILE_INVISIBLE = 598,
            ///<summary>The GM has an important notice. Chat has been temporarily disabled.</summary>
            GM_NOTICE_CHAT_DISABLED = 599,
            ///<summary>You may not equip a pet item.</summary>
            CANNOT_EQUIP_PET_ITEM = 600,
            ///<summary>There are $S1 petitions currently on the waiting list.</summary>
            S1_PETITION_ON_WAITING_LIST = 601,
            ///<summary>The petition system is currently unavailable. Please try again later.</summary>
            PETITION_SYSTEM_CURRENT_UNAVAILABLE = 602,
            ///<summary>That item cannot be discarded or exchanged.</summary>
            CANNOT_DISCARD_EXCHANGE_ITEM = 603,
            ///<summary>You may not call forth a pet or summoned creature from this location</summary>
            NOT_CALL_PET_FROM_THIS_LOCATION = 604,
            ///<summary>You may register up to 64 people on your list.</summary>
            MAY_REGISTER_UP_TO_64_PEOPLE = 605,
            ///<summary>You cannot be registered because the other person has already registered 64 people on his/her list.</summary>
            OTHER_PERSON_ALREADY_64_PEOPLE = 606,
            ///<summary>You do not have any further skills to learn. Come back when you have reached Level $s1.</summary>
            DO_NOT_HAVE_FURTHER_SKILLS_TO_LEARN_S1 = 607,
            ///<summary>$s1 has obtained $s3 $s2 by using Sweeper.</summary>
            S1_SWEEPED_UP_S3_S2 = 608,
            ///<summary>$s1 has obtained $s2 by using Sweeper.</summary>
            S1_SWEEPED_UP_S2 = 609,
            ///<summary>Your skill has been canceled due to lack of HP.</summary>
            SKILL_REMOVED_DUE_LACK_HP = 610,
            ///<summary>You have succeeded in Confusing the enemy.</summary>
            CONFUSING_SUCCEEDED = 611,
            ///<summary>The Spoil condition has been activated.</summary>
            SPOIL_SUCCESS = 612,
            ///<summary>======<Ignore List>======</summary>
            BLOCK_LIST_HEADER = 613,
            ///<summary>$s1 : $s2</summary>
            S1_S2 = 614,
            ///<summary>You have failed to register the user to your Ignore List.</summary>
            FAILED_TO_REGISTER_TO_IGNORE_LIST = 615,
            ///<summary>You have failed to delete the character.</summary>
            FAILED_TO_DELETE_CHARACTER = 616,
            ///<summary>$s1 has been added to your Ignore List.</summary>
            S1_WAS_ADDED_TO_YOUR_IGNORE_LIST = 617,
            ///<summary>$s1 has been removed from your Ignore List.</summary>
            S1_WAS_REMOVED_FROM_YOUR_IGNORE_LIST = 618,
            ///<summary>$s1 has placed you on his/her Ignore List.</summary>
            S1_HAS_ADDED_YOU_TO_IGNORE_LIST = 619,
            ///<summary>$s1 has placed you on his/her Ignore List.</summary>
            S1_HAS_ADDED_YOU_TO_IGNORE_LIST2 = 620,
            ///<summary>Game connection attempted through a restricted IP.</summary>
            CONNECTION_RESTRICTED_IP = 621,
            ///<summary>You may not make a declaration of war during an alliance battle.</summary>
            NO_WAR_DURING_ALLY_BATTLE = 622,
            ///<summary>Your opponent has exceeded the number of simultaneous alliance battles alllowed.</summary>
            OPPONENT_TOO_MUCH_ALLY_BATTLES1 = 623,
            ///<summary>$s1 Clan leader is not currently connected to the game server.</summary>
            S1_LEADER_NOT_CONNECTED = 624,
            ///<summary>Your request for Alliance Battle truce has been denied.</summary>
            ALLY_BATTLE_TRUCE_DENIED = 625,
            ///<summary>The $s1 clan did not respond: war proclamation has been refused.</summary>
            WAR_PROCLAMATION_HAS_BEEN_REFUSED = 626,
            ///<summary>Clan battle has been refused because you did not respond to $s1 clan's war proclamation.</summary>
            YOU_REFUSED_CLAN_WAR_PROCLAMATION = 627,
            ///<summary>You have already been at war with the $s1 clan: 5 days must pass before you can declare war again.</summary>
            ALREADY_AT_WAR_WITH_S1_WAIT_5_DAYS = 628,
            ///<summary>Your opponent has exceeded the number of simultaneous alliance battles alllowed.</summary>
            OPPONENT_TOO_MUCH_ALLY_BATTLES2 = 629,
            ///<summary>War with the clan has begun.</summary>
            WAR_WITH_CLAN_BEGUN = 630,
            ///<summary>War with the clan is over.</summary>
            WAR_WITH_CLAN_ENDED = 631,
            ///<summary>You have won the war over the clan!</summary>
            WON_WAR_OVER_CLAN = 632,
            ///<summary>You have surrendered to the clan.</summary>
            SURRENDERED_TO_CLAN = 633,
            ///<summary>Your alliance leader has been slain. You have been defeated by the clan.</summary>
            DEFEATED_BY_CLAN = 634,
            ///<summary>The time limit for the clan war has been exceeded. War with the clan is over.</summary>
            TIME_UP_WAR_OVER = 635,
            ///<summary>You are not involved in a clan war.</summary>
            NOT_INVOLVED_IN_WAR = 636,
            ///<summary>A clan ally has registered itself to the opponent.</summary>
            ALLY_REGISTERED_SELF_TO_OPPONENT = 637,
            ///<summary>You have already requested a Siege Battle.</summary>
            ALREADY_REQUESTED_SIEGE_BATTLE = 638,
            ///<summary>Your application has been denied because you have already submitted a request for another Siege Battle.</summary>
            APPLICATION_DENIED_BECAUSE_ALREADY_SUBMITTED_A_REQUEST_FOR_ANOTHER_SIEGE_BATTLE = 639,
            ///<summary>You are already registered to the attacker side and must not cancel your registration before submitting your request</summary>
            ALREADY_ATTACKER_NOT_CANCEL = 642,
            ///<summary>You are already registered to the defender side and must not cancel your registration before submitting your request</summary>
            ALREADY_DEFENDER_NOT_CANCEL = 643,
            ///<summary>You are not yet registered for the castle siege.</summary>
            NOT_REGISTERED_FOR_SIEGE = 644,
            ///<summary>Only clans of level 4 or higher may register for a castle siege.</summary>
            ONLY_CLAN_LEVEL_4_ABOVE_MAY_SIEGE = 645,
            ///<summary>No more registrations may be accepted for the attacker side.</summary>
            ATTACKER_SIDE_FULL = 648,
            ///<summary>No more registrations may be accepted for the defender side.</summary>
            DEFENDER_SIDE_FULL = 649,
            ///<summary>You may not summon from your current location.</summary>
            YOU_MAY_NOT_SUMMON_FROM_YOUR_CURRENT_LOCATION = 650,
            ///<summary>Place $s1 in the current location and direction. Do you wish to continue?</summary>
            PLACE_S1_IN_CURRENT_LOCATION_AND_DIRECTION = 651,
            ///<summary>The target of the summoned monster is wrong.</summary>
            TARGET_OF_SUMMON_WRONG = 652,
            ///<summary>You do not have the authority to position mercenaries.</summary>
            YOU_DO_NOT_HAVE_AUTHORITY_TO_POSITION_MERCENARIES = 653,
            ///<summary>You do not have the authority to cancel mercenary positioning.</summary>
            YOU_DO_NOT_HAVE_AUTHORITY_TO_CANCEL_MERCENARY_POSITIONING = 654,
            ///<summary>Mercenaries cannot be positioned here.</summary>
            MERCENARIES_CANNOT_BE_POSITIONED_HERE = 655,
            ///<summary>This mercenary cannot be positioned anymore.</summary>
            THIS_MERCENARY_CANNOT_BE_POSITIONED_ANYMORE = 656,
            ///<summary>Positioning cannot be done here because the distance between mercenaries is too short.</summary>
            POSITIONING_CANNOT_BE_DONE_BECAUSE_DISTANCE_BETWEEN_MERCENARIES_TOO_SHORT = 657,
            ///<summary>This is not a mercenary of a castle that you own and so you cannot cancel its positioning.</summary>
            THIS_IS_NOT_A_MERCENARY_OF_A_CASTLE_THAT_YOU_OWN_AND_SO_CANNOT_CANCEL_POSITIONING = 658,
            ///<summary>This is not the time for siege registration and so registrations cannot be accepted or rejected.</summary>
            NOT_SIEGE_REGISTRATION_TIME1 = 659,
            ///<summary>This is not the time for siege registration and so registration and cancellation cannot be done.</summary>
            NOT_SIEGE_REGISTRATION_TIME2 = 659,
            ///<summary>This character cannot be spoiled.</summary>
            SPOIL_CANNOT_USE = 661,
            ///<summary>The other player is rejecting friend invitations.</summary>
            THE_PLAYER_IS_REJECTING_FRIEND_INVITATIONS = 662,
            ///<summary>Please choose a person to receive.</summary>
            CHOOSE_PERSON_TO_RECEIVE = 664,
            ///<summary>of alliance is applying for alliance war. Do you want to accept the challenge?</summary>
            APPLYING_ALLIANCE_WAR = 665,
            ///<summary>A request for ceasefire has been received from alliance. Do you agree?</summary>
            REQUEST_FOR_CEASEFIRE = 666,
            ///<summary>You are registering on the attacking side of the siege. Do you want to continue?</summary>
            REGISTERING_ON_ATTACKING_SIDE = 667,
            ///<summary>You are registering on the defending side of the siege. Do you want to continue?</summary>
            REGISTERING_ON_DEFENDING_SIDE = 668,
            ///<summary>You are canceling your application to participate in the siege battle. Do you want to continue?</summary>
            CANCELING_REGISTRATION = 669,
            ///<summary>You are refusing the registration of clan on the defending side. Do you want to continue?</summary>
            REFUSING_REGISTRATION = 670,
            ///<summary>You are agreeing to the registration of clan on the defending side. Do you want to continue?</summary>
            AGREEING_REGISTRATION = 671,
            ///<summary>$s1 adena disappeared.</summary>
            S1_DISAPPEARED_ADENA = 672,
            ///<summary>Only a clan leader whose clan is of level 2 or higher is allowed to participate in a clan hall auction.</summary>
            AUCTION_ONLY_CLAN_LEVEL_2_HIGHER = 673,
            ///<summary>I has not yet been seven days since canceling an auction.</summary>
            NOT_SEVEN_DAYS_SINCE_CANCELING_AUCTION = 674,
            ///<summary>There are no clan halls up for auction.</summary>
            NO_CLAN_HALLS_UP_FOR_AUCTION = 675,
            ///<summary>Since you have already submitted a bid, you are not allowed to participate in another auction at this time.</summary>
            ALREADY_SUBMITTED_BID = 676,
            ///<summary>Your bid price must be higher than the minimum price that can be bid.</summary>
            BID_PRICE_MUST_BE_HIGHER = 677,
            ///<summary>You have submitted a bid for the auction of $s1.</summary>
            SUBMITTED_A_BID = 678,
            ///<summary>You have canceled your bid.</summary>
            CANCELED_BID = 679,
            /// You cannot participate in an auction.</summary>
            CANNOT_PARTICIPATE_IN_AUCTION = 680,
            ///<summary>The clan does not own a clan hall.</summary>
            // CLAN_HAS_NO_CLAN_HALL(681) // Doesn't exist in Hellbound anymore = 681,
            ///<summary>There are no priority rights on a sweeper.</summary>
            SWEEP_NOT_ALLOWED = 683,
            ///<summary>You cannot position mercenaries during a siege.</summary>
            CANNOT_POSITION_MERCS_DURING_SIEGE = 684,
            ///<summary>You cannot apply for clan war with a clan that belongs to the same alliance</summary>
            CANNOT_DECLARE_WAR_ON_ALLY = 685,
            ///<summary>You have received $s1 damage from the fire of magic.</summary>
            S1_DAMAGE_FROM_FIRE_MAGIC = 686,
            ///<summary>You cannot move while frozen. Please wait.</summary>
            CANNOT_MOVE_FROZEN = 687,
            ///<summary>The clan that owns the castle is automatically registered on the defending side.</summary>
            CLAN_THAT_OWNS_CASTLE_IS_AUTOMATICALLY_REGISTERED_DEFENDING = 688,
            ///<summary>A clan that owns a castle cannot participate in another siege.</summary>
            CLAN_THAT_OWNS_CASTLE_CANNOT_PARTICIPATE_OTHER_SIEGE = 689,
            ///<summary>You cannot register on the attacking side because you are part of an alliance with the clan that owns the castle.</summary>
            CANNOT_ATTACK_ALLIANCE_CASTLE = 690,
            ///<summary>$s1 clan is already a member of $s2 alliance.</summary>
            S1_CLAN_ALREADY_MEMBER_OF_S2_ALLIANCE = 691,
            ///<summary>The other party is frozen. Please wait a moment.</summary>
            OTHER_PARTY_IS_FROZEN = 692,
            ///<summary>The package that arrived is in another warehouse.</summary>
            PACKAGE_IN_ANOTHER_WAREHOUSE = 693,
            ///<summary>No packages have arrived.</summary>
            NO_PACKAGES_ARRIVED = 694,
            ///<summary>You cannot set the name of the pet.</summary>
            NAMING_YOU_CANNOT_SET_NAME_OF_THE_PET = 695,
            ///<summary>The item enchant value is strange</summary>
            ITEM_ENCHANT_VALUE_STRANGE = 697,
            ///<summary>The price is different than the same item on the sales list.</summary>
            PRICE_DIFFERENT_FROM_SALES_LIST = 698,
            ///<summary>Currently not purchasing.</summary>
            CURRENTLY_NOT_PURCHASING = 699,
            ///<summary>The purchase is complete.</summary>
            THE_PURCHASE_IS_COMPLETE = 700,
            ///<summary>You do not have enough required items.</summary>
            NOT_ENOUGH_REQUIRED_ITEMS = 701,
            ///<summary>There are no GMs currently visible in the public list as they may be performing other functions at the moment.</summary>
            NO_GM_PROVIDING_SERVICE_NOW = 702,
            ///<summary>======<GM List>======</summary>
            GM_LIST = 703,
            ///<summary>GM : $s1</summary>
            GM_S1 = 704,
            ///<summary>You cannot exclude yourself.</summary>
            CANNOT_EXCLUDE_SELF = 705,
            ///<summary>You can only register up to 64 names on your exclude list.</summary>
            ONLY_64_NAMES_ON_EXCLUDE_LIST = 706,
            ///<summary>You cannot teleport to a village that is in a siege.</summary>
            NO_PORT_THAT_IS_IN_SIGE = 707,
            ///<summary>You do not have the right to use the castle warehouse.</summary>
            YOU_DO_NOT_HAVE_THE_RIGHT_TO_USE_CASTLE_WAREHOUSE = 708,
            ///<summary>You do not have the right to use the clan warehouse.</summary>
            YOU_DO_NOT_HAVE_THE_RIGHT_TO_USE_CLAN_WAREHOUSE = 709,
            ///<summary>Only clans of clan level 1 or higher can use a clan warehouse.</summary>
            ONLY_LEVEL_1_CLAN_OR_HIGHER_CAN_USE_WAREHOUSE = 710,
            ///<summary>The siege of $s1 has started.</summary>
            SIEGE_OF_S1_HAS_STARTED = 711,
            ///<summary>The siege of $s1 has finished.</summary>
            SIEGE_OF_S1_HAS_ENDED = 712,
            ///<summary>$s1/$s2/$s3 :</summary>
            S1_S2_S3_D = 713,
            ///<summary>A trap device has been tripped.</summary>
            A_TRAP_DEVICE_HAS_BEEN_TRIPPED = 714,
            ///<summary>A trap device has been stopped.</summary>
            A_TRAP_DEVICE_HAS_BEEN_STOPPED = 715,
            ///<summary>If a base camp does not exist, resurrection is not possible.</summary>
            NO_RESURRECTION_WITHOUT_BASE_CAMP = 716,
            ///<summary>The guardian tower has been destroyed and resurrection is not possible</summary>
            TOWER_DESTROYED_NO_RESURRECTION = 717,
            ///<summary>The castle gates cannot be opened and closed during a siege.</summary>
            GATES_NOT_OPENED_CLOSED_DURING_SIEGE = 718,
            ///<summary>You failed at mixing the item.</summary>
            ITEM_MIXING_FAILED = 719,
            ///<summary>The purchase price is higher than the amount of money that you have and so you cannot open a personal store.</summary>
            THE_PURCHASE_PRICE_IS_HIGHER_THAN_MONEY = 720,
            ///<summary>You cannot create an alliance while participating in a siege.</summary>
            NO_ALLY_CREATION_WHILE_SIEGE = 721,
            ///<summary>You cannot dissolve an alliance while an affiliated clan is participating in a siege battle.</summary>
            CANNOT_DISSOLVE_ALLY_WHILE_IN_SIEGE = 722,
            ///<summary>The opposing clan is participating in a siege battle.</summary>
            OPPOSING_CLAN_IS_PARTICIPATING_IN_SIEGE = 723,
            ///<summary>You cannot leave while participating in a siege battle.</summary>
            CANNOT_LEAVE_WHILE_SIEGE = 724,
            ///<summary>You cannot banish a clan from an alliance while the clan is participating in a siege</summary>
            CANNOT_DISMISS_WHILE_SIEGE = 725,
            ///<summary>Frozen condition has started. Please wait a moment.</summary>
            FROZEN_CONDITION_STARTED = 726,
            ///<summary>The frozen condition was removed.</summary>
            FROZEN_CONDITION_REMOVED = 727,
            ///<summary>You cannot apply for dissolution again within seven days after a previous application for dissolution.</summary>
            CANNOT_APPLY_DISSOLUTION_AGAIN = 728,
            ///<summary>That item cannot be discarded.</summary>
            ITEM_NOT_DISCARDED = 729,
            ///<summary>- You have submitted your $s1th petition. - You may submit $s2 more petition(s) today.</summary>
            SUBMITTED_YOU_S1_TH_PETITION_S2_LEFT = 730,
            ///<summary>A petition has been received by the GM on behalf of $s1. The petition code is $s2.</summary>
            PETITION_S1_RECEIVED_CODE_IS_S2 = 731,
            ///<summary>$s1 has received a request for a consultation with the GM.</summary>
            S1_RECEIVED_CONSULTATION_REQUEST = 732,
            ///<summary>We have received $s1 petitions from you today and that is the maximum that you can submit in one day. You cannot submit any more petitions.</summary>
            WE_HAVE_RECEIVED_S1_PETITIONS_TODAY = 733,
            ///<summary>You have failed at submitting a petition on behalf of someone else. $s1 already submitted a petition.</summary>
            PETITION_FAILED_S1_ALREADY_SUBMITTED = 734,
            ///<summary>You have failed at submitting a petition on behalf of $s1. The error number is $s2.</summary>
            PETITION_FAILED_FOR_S1_ERROR_NUMBER_S2 = 735,
            ///<summary>The petition was canceled. You may submit $s1 more petition(s) today.</summary>
            PETITION_CANCELED_SUBMIT_S1_MORE_TODAY = 736,
            ///<summary>You have cancelled submitting a petition on behalf of $s1.</summary>
            CANCELED_PETITION_ON_S1 = 737,
            ///<summary>You have not submitted a petition.</summary>
            PETITION_NOT_SUBMITTED = 738,
            ///<summary>You have failed at cancelling a petition on behalf of $s1. The error number is $s2.</summary>
            PETITION_CANCEL_FAILED_FOR_S1_ERROR_NUMBER_S2 = 739,
            ///<summary>$s1 participated in a petition chat at the request of the GM.</summary>
            S1_PARTICIPATE_PETITION = 740,
            ///<summary>You have failed at adding $s1 to the petition chat. Petition has already been submitted.</summary>
            FAILED_ADDING_S1_TO_PETITION = 741,
            ///<summary>You have failed at adding $s1 to the petition chat. The error code is $s2.</summary>
            PETITION_ADDING_S1_FAILED_ERROR_NUMBER_S2 = 742,
            ///<summary>$s1 left the petition chat.</summary>
            S1_LEFT_PETITION_CHAT = 743,
            ///<summary>You have failed at removing $s1 from the petition chat. The error code is $s2.</summary>
            PETITION_REMOVING_S1_FAILED_ERROR_NUMBER_S2 = 744,
            ///<summary>You are currently not in a petition chat.</summary>
            YOU_ARE_NOT_IN_PETITION_CHAT = 745,
            ///<summary>It is not currently a petition.</summary>
            CURRENTLY_NO_PETITION = 746,
            ///<summary>The distance is too far and so the casting has been stopped.</summary>
            DIST_TOO_FAR_CASTING_STOPPED = 748,
            ///<summary>The effect of $s1 has been removed.</summary>
            EFFECT_S1_DISAPPEARED = 749,
            ///<summary>There are no other skills to learn.</summary>
            NO_MORE_SKILLS_TO_LEARN = 750,
            ///<summary>As there is a conflict in the siege relationship with a clan in the alliance, you cannot invite that clan to the alliance.</summary>
            CANNOT_INVITE_CONFLICT_CLAN = 751,
            ///<summary>That name cannot be used.</summary>
            CANNOT_USE_NAME = 752,
            ///<summary>You cannot position mercenaries here.</summary>
            NO_MERCS_HERE = 753,
            ///<summary>There are $s1 hours and $s2 minutes left in this week's usage time.</summary>
            S1_HOURS_S2_MINUTES_LEFT_THIS_WEEK = 754,
            ///<summary>There are $s1 minutes left in this week's usage time.</summary>
            S1_MINUTES_LEFT_THIS_WEEK = 755,
            ///<summary>This week's usage time has finished.</summary>
            WEEKS_USAGE_TIME_FINISHED = 756,
            ///<summary>There are $s1 hours and $s2 minutes left in the fixed use time.</summary>
            S1_HOURS_S2_MINUTES_LEFT_IN_TIME = 757,
            ///<summary>There are $s1 hours and $s2 minutes left in this week's play time.</summary>
            S1_HOURS_S2_MINUTES_LEFT_THIS_WEEKS_PLAY_TIME = 758,
            ///<summary>There are $s1 minutes left in this week's play time.</summary>
            S1_MINUTES_LEFT_THIS_WEEKS_PLAY_TIME = 759,
            ///<summary>$s1 cannot join the clan because one day has not yet passed since he/she left another clan.</summary>
            S1_MUST_WAIT_BEFORE_JOINING_ANOTHER_CLAN = 760,
            ///<summary>$s1 clan cannot join the alliance because one day has not yet passed since it left another alliance.</summary>
            S1_CANT_ENTER_ALLIANCE_WITHIN_1_DAY = 761,
            ///<summary>$s1 rolled $s2 and $s3's eye came out.</summary>
            S1_ROLLED_S2_S3_EYE_CAME_OUT = 762,
            ///<summary>You failed at sending the package because you are too far from the warehouse.</summary>
            FAILED_SENDING_PACKAGE_TOO_FAR = 763,
            ///<summary>You have been playing for an extended period of time. Please consider taking a break.</summary>
            PLAYING_FOR_LONG_TIME = 764,
            ///<summary>A hacking tool has been discovered. Please try again after closing unnecessary programs.</summary>
            HACKING_TOOL = 769,
            ///<summary>Play time is no longer accumulating.</summary>
            PLAY_TIME_NO_LONGER_ACCUMULATING = 774,
            ///<summary>From here on, play time will be expended.</summary>
            PLAY_TIME_EXPENDED = 775,
            ///<summary>The clan hall which was put up for auction has been awarded to clan s1.</summary>
            CLANHALL_AWARDED_TO_CLAN_S1 = 776,
            ///<summary>The clan hall which was put up for auction was not sold and therefore has been re-listed.</summary>
            CLANHALL_NOT_SOLD = 777,
            ///<summary>You may not log out from this location.</summary>
            NO_LOGOUT_HERE = 778,
            ///<summary>You may not restart in this location.</summary>
            NO_RESTART_HERE = 779,
            ///<summary>Observation is only possible during a siege.</summary>
            ONLY_VIEW_SIEGE = 780,
            ///<summary>Observers cannot participate.</summary>
            OBSERVERS_CANNOT_PARTICIPATE = 781,
            ///<summary>You may not observe a siege with a pet or servitor summoned.</summary>
            NO_OBSERVE_WITH_PET = 782,
            ///<summary>Lottery ticket sales have been temporarily suspended.</summary>
            LOTTERY_TICKET_SALES_TEMP_SUSPENDED = 783,
            ///<summary>Tickets for the current lottery are no longer available.</summary>
            NO_LOTTERY_TICKETS_AVAILABLE = 784,
            ///<summary>The results of lottery number $s1 have not yet been published.</summary>
            LOTTERY_S1_RESULT_NOT_PUBLISHED = 785,
            ///<summary>Incorrect syntax.</summary>
            INCORRECT_SYNTAX = 786,
            ///<summary>The tryouts are finished.</summary>
            CLANHALL_SIEGE_TRYOUTS_FINISHED = 787,
            ///<summary>The finals are finished.</summary>
            CLANHALL_SIEGE_FINALS_FINISHED = 788,
            ///<summary>The tryouts have begun.</summary>
            CLANHALL_SIEGE_TRYOUTS_BEGUN = 789,
            ///<summary>The finals are finished.</summary>
            CLANHALL_SIEGE_FINALS_BEGUN = 790,
            ///<summary>The final match is about to begin. Line up!</summary>
            FINAL_MATCH_BEGIN = 791,
            ///<summary>The siege of the clan hall is finished.</summary>
            CLANHALL_SIEGE_ENDED = 792,
            ///<summary>The siege of the clan hall has begun.</summary>
            CLANHALL_SIEGE_BEGUN = 793,
            ///<summary>You are not authorized to do that.</summary>
            YOU_ARE_NOT_AUTHORIZED_TO_DO_THAT = 794,
            ///<summary>Only clan leaders are authorized to set rights.</summary>
            ONLY_LEADERS_CAN_SET_RIGHTS = 795,
            ///<summary>Your remaining observation time is minutes.</summary>
            REMAINING_OBSERVATION_TIME = 796,
            ///<summary>You may create up to 24 macros.</summary>
            YOU_MAY_CREATE_UP_TO_24_MACROS = 797,
            ///<summary>Item registration is irreversible. Do you wish to continue?</summary>
            ITEM_REGISTRATION_IRREVERSIBLE = 798,
            ///<summary>The observation time has expired.</summary>
            OBSERVATION_TIME_EXPIRED = 799,
            ///<summary>You are too late. The registration period is over.</summary>
            REGISTRATION_PERIOD_OVER = 800,
            ///<summary>Registration for the clan hall siege is closed.</summary>
            REGISTRATION_CLOSED = 801,
            ///<summary>Petitions are not being accepted at this time. You may submit your petition after a.m./p.m.</summary>
            PETITION_NOT_ACCEPTED_NOW = 802,
            ///<summary>Enter the specifics of your petition.</summary>
            PETITION_NOT_SPECIFIED = 803,
            ///<summary>Select a type.</summary>
            SELECT_TYPE = 804,
            ///<summary>Petitions are not being accepted at this time. You may submit your petition after $s1 a.m./p.m.</summary>
            PETITION_NOT_ACCEPTED_SUBMIT_AT_S1 = 805,
            ///<summary>If you are trapped, try typing "/unstuck".</summary>
            TRY_UNSTUCK_WHEN_TRAPPED = 806,
            ///<summary>This terrain is navigable. Prepare for transport to the nearest village.</summary>
            STUCK_PREPARE_FOR_TRANSPORT = 807,
            ///<summary>You are stuck. You may submit a petition by typing "/gm".</summary>
            STUCK_SUBMIT_PETITION = 808,
            ///<summary>You are stuck. You will be transported to the nearest village in five minutes.</summary>
            STUCK_TRANSPORT_IN_FIVE_MINUTES = 809,
            ///<summary>Invalid macro. Refer to the Help file for instructions.</summary>
            INVALID_MACRO = 810,
            ///<summary>You will be moved to (). Do you wish to continue?</summary>
            WILL_BE_MOVED = 811,
            ///<summary>The secret trap has inflicted $s1 damage on you.</summary>
            TRAP_DID_S1_DAMAGE = 812,
            ///<summary>You have been poisoned by a Secret Trap.</summary>
            POISONED_BY_TRAP = 813,
            ///<summary>Your speed has been decreased by a Secret Trap.</summary>
            SLOWED_BY_TRAP = 814,
            ///<summary>The tryouts are about to begin. Line up!</summary>
            TRYOUTS_ABOUT_TO_BEGIN = 815,
            ///<summary>Tickets are now available for Monster Race $s1!</summary>
            MONSRACE_TICKETS_AVAILABLE_FOR_S1_RACE = 816,
            ///<summary>Now selling tickets for Monster Race $s1!</summary>
            MONSRACE_TICKETS_NOW_AVAILABLE_FOR_S1_RACE = 817,
            ///<summary>Ticket sales for the Monster Race will end in $s1 minute(s).</summary>
            MONSRACE_TICKETS_STOP_IN_S1_MINUTES = 818,
            ///<summary>Tickets sales are closed for Monster Race $s1. Odds are posted.</summary>
            MONSRACE_S1_TICKET_SALES_CLOSED = 819,
            ///<summary>Monster Race $s2 will begin in $s1 minute(s)!</summary>
            MONSRACE_S2_BEGINS_IN_S1_MINUTES = 820,
            ///<summary>Monster Race $s1 will begin in 30 seconds!</summary>
            MONSRACE_S1_BEGINS_IN_30_SECONDS = 821,
            ///<summary>Monster Race $s1 is about to begin! Countdown in five seconds!</summary>
            MONSRACE_S1_COUNTDOWN_IN_FIVE_SECONDS = 822,
            ///<summary>The race will begin in $s1 second(s)!</summary>
            MONSRACE_BEGINS_IN_S1_SECONDS = 823,
            ///<summary>They're off!</summary>
            MONSRACE_RACE_START = 824,
            ///<summary>Monster Race $s1 is finished!</summary>
            MONSRACE_S1_RACE_END = 825,
            ///<summary>First prize goes to the player in lane $s1. Second prize goes to the player in lane $s2.</summary>
            MONSRACE_FIRST_PLACE_S1_SECOND_S2 = 826,
            ///<summary>You may not impose a block on a GM.</summary>
            YOU_MAY_NOT_IMPOSE_A_BLOCK_ON_GM = 827,
            ///<summary>Are you sure you wish to delete the $s1 macro?</summary>
            WISH_TO_DELETE_S1_MACRO = 828,
            ///<summary>You cannot recommend yourself.</summary>
            YOU_CANNOT_RECOMMEND_YOURSELF = 829,
            ///<summary>You have recommended $s1. You have $s2 recommendations left.</summary>
            YOU_HAVE_RECOMMENDED_S1_YOU_HAVE_S2_RECOMMENDATIONS_LEFT = 830,
            ///<summary>You have been recommended by $s1.</summary>
            YOU_HAVE_BEEN_RECOMMENDED_BY_S1 = 831,
            ///<summary>That character has already been recommended.</summary>
            THAT_CHARACTER_IS_RECOMMENDED = 832,
            ///<summary>You are not authorized to make further recommendations at this time. You will receive more recommendation credits each day at 1 p.m.</summary>
            NO_MORE_RECOMMENDATIONS_TO_HAVE = 833,
            ///<summary>$s1 has rolled $s2.</summary>
            S1_ROLLED_S2 = 834,
            ///<summary>You may not throw the dice at this time. Try again later.</summary>
            YOU_MAY_NOT_THROW_THE_DICE_AT_THIS_TIME_TRY_AGAIN_LATER = 835,
            ///<summary>You have exceeded your inventory volume limit and cannot take this item.</summary>
            YOU_HAVE_EXCEEDED_YOUR_INVENTORY_VOLUME_LIMIT_AND_CANNOT_TAKE_THIS_ITEM = 836,
            ///<summary>Macro descriptions may contain up to 32 characters.</summary>
            MACRO_DESCRIPTION_MAX_32_CHARS = 837,
            ///<summary>Enter the name of the macro.</summary>
            ENTER_THE_MACRO_NAME = 838,
            ///<summary>That name is already assigned to another macro.</summary>
            MACRO_NAME_ALREADY_USED = 839,
            ///<summary>That recipe is already registered.</summary>
            RECIPE_ALREADY_REGISTERED = 840,
            ///<summary>No further recipes may be registered.</summary>
            NO_FUTHER_RECIPES_CAN_BE_ADDED = 841,
            ///<summary>You are not authorized to register a recipe.</summary>
            NOT_AUTHORIZED_REGISTER_RECIPE = 842,
            ///<summary>The siege of $s1 is finished.</summary>
            SIEGE_OF_S1_FINISHED = 843,
            ///<summary>The siege to conquer $s1 has begun.</summary>
            SIEGE_OF_S1_BEGUN = 844,
            ///<summary>The deadlineto register for the siege of $s1 has passed.</summary>
            DEADLINE_FOR_SIEGE_S1_PASSED = 845,
            ///<summary>The siege of $s1 has been canceled due to lack of interest.</summary>
            SIEGE_OF_S1_HAS_BEEN_CANCELED_DUE_TO_LACK_OF_INTEREST = 846,
            ///<summary>A clan that owns a clan hall may not participate in a clan hall siege.</summary>
            CLAN_OWNING_CLANHALL_MAY_NOT_SIEGE_CLANHALL = 847,
            ///<summary>$s1 has been deleted.</summary>
            S1_HAS_BEEN_DELETED = 848,
            ///<summary>$s1 cannot be found.</summary>
            S1_NOT_FOUND = 849,
            ///<summary>$s1 already exists.</summary>
            S1_ALREADY_EXISTS2 = 850,
            ///<summary>$s1 has been added.</summary>
            S1_ADDED = 851,
            ///<summary>The recipe is incorrect.</summary>
            RECIPE_INCORRECT = 852,
            ///<summary>You may not alter your recipe book while engaged in manufacturing.</summary>
            CANT_ALTER_RECIPEBOOK_WHILE_CRAFTING = 853,
            ///<summary>You are missing $s2 $s1 required to create that.</summary>
            MISSING_S2_S1_TO_CREATE = 854,
            ///<summary>$s1 clan has defeated $s2.</summary>
            S1_CLAN_DEFEATED_S2 = 855,
            ///<summary>The siege of $s1 has ended in a draw.</summary>
            SIEGE_S1_DRAW = 856,
            ///<summary>$s1 clan has won in the preliminary match of $s2.</summary>
            S1_CLAN_WON_MATCH_S2 = 857,
            ///<summary>The preliminary match of $s1 has ended in a draw.</summary>
            MATCH_OF_S1_DRAW = 858,
            ///<summary>Please register a recipe.</summary>
            PLEASE_REGISTER_RECIPE = 859,
            ///<summary>You may not buld your headquarters in close proximity to another headquarters.</summary>
            HEADQUARTERS_TOO_CLOSE = 860,
            ///<summary>You have exceeded the maximum number of memos.</summary>
            TOO_MANY_MEMOS = 861,
            ///<summary>Odds are not posted until ticket sales have closed.</summary>
            ODDS_NOT_POSTED = 862,
            ///<summary>You feel the energy of fire.</summary>
            FEEL_ENERGY_FIRE = 863,
            ///<summary>You feel the energy of water.</summary>
            FEEL_ENERGY_WATER = 864,
            ///<summary>You feel the energy of wind.</summary>
            FEEL_ENERGY_WIND = 865,
            ///<summary>You may no longer gather energy.</summary>
            NO_LONGER_ENERGY = 866,
            ///<summary>The energy is depleted.</summary>
            ENERGY_DEPLETED = 867,
            ///<summary>The energy of fire has been delivered.</summary>
            ENERGY_FIRE_DELIVERED = 868,
            ///<summary>The energy of water has been delivered.</summary>
            ENERGY_WATER_DELIVERED = 869,
            ///<summary>The energy of wind has been delivered.</summary>
            ENERGY_WIND_DELIVERED = 870,
            ///<summary>The seed has been sown.</summary>
            THE_SEED_HAS_BEEN_SOWN = 871,
            ///<summary>This seed may not be sown here.</summary>
            THIS_SEED_MAY_NOT_BE_SOWN_HERE = 872,
            ///<summary>That character does not exist.</summary>
            CHARACTER_DOES_NOT_EXIST = 873,
            ///<summary>The capacity of the warehouse has been exceeded.</summary>
            WAREHOUSE_CAPACITY_EXCEEDED = 874,
            ///<summary>The transport of the cargo has been canceled.</summary>
            CARGO_CANCELED = 875,
            ///<summary>The cargo was not delivered.</summary>
            CARGO_NOT_DELIVERED = 876,
            ///<summary>The symbol has been added.</summary>
            SYMBOL_ADDED = 877,
            ///<summary>The symbol has been deleted.</summary>
            SYMBOL_DELETED = 878,
            ///<summary>The manor system is currently under maintenance.</summary>
            THE_MANOR_SYSTEM_IS_CURRENTLY_UNDER_MAINTENANCE = 879,
            ///<summary>The transaction is complete.</summary>
            THE_TRANSACTION_IS_COMPLETE = 880,
            ///<summary>There is a discrepancy on the invoice.</summary>
            THERE_IS_A_DISCREPANCY_ON_THE_INVOICE = 881,
            ///<summary>The seed quantity is incorrect.</summary>
            THE_SEED_QUANTITY_IS_INCORRECT = 882,
            ///<summary>The seed information is incorrect.</summary>
            THE_SEED_INFORMATION_IS_INCORRECT = 883,
            ///<summary>The manor information has been updated.</summary>
            THE_MANOR_INFORMATION_HAS_BEEN_UPDATED = 884,
            ///<summary>The number of crops is incorrect.</summary>
            THE_NUMBER_OF_CROPS_IS_INCORRECT = 885,
            ///<summary>The crops are priced incorrectly.</summary>
            THE_CROPS_ARE_PRICED_INCORRECTLY = 886,
            ///<summary>The type is incorrect.</summary>
            THE_TYPE_IS_INCORRECT = 887,
            ///<summary>No crops can be purchased at this time.</summary>
            NO_CROPS_CAN_BE_PURCHASED_AT_THIS_TIME = 888,
            ///<summary>The seed was successfully sown.</summary>
            THE_SEED_WAS_SUCCESSFULLY_SOWN = 889,
            ///<summary>The seed was not sown.</summary>
            THE_SEED_WAS_NOT_SOWN = 890,
            ///<summary>You are not authorized to harvest.</summary>
            YOU_ARE_NOT_AUTHORIZED_TO_HARVEST = 891,
            ///<summary>The harvest has failed.</summary>
            THE_HARVEST_HAS_FAILED = 892,
            ///<summary>The harvest failed because the seed was not sown.</summary>
            THE_HARVEST_FAILED_BECAUSE_THE_SEED_WAS_NOT_SOWN = 893,
            ///<summary>Up to $s1 recipes can be registered.</summary>
            UP_TO_S1_RECIPES_CAN_REGISTER = 894,
            ///<summary>No recipes have been registered.</summary>
            NO_RECIPES_REGISTERED = 895,
            /// Message:The ferry has arrived at Gludin Harbor.</summary>
            FERRY_AT_GLUDIN = 896,
            /// Message:The ferry will leave for Talking Island Harbor after anchoring for ten minutes.</summary>
            FERRY_LEAVE_TALKING = 897,
            ///<summary>Only characters of level 10 or above are authorized to make recommendations.</summary>
            ONLY_LEVEL_SUP_10_CAN_RECOMMEND = 898,
            ///<summary>The symbol cannot be drawn.</summary>
            CANT_DRAW_SYMBOL = 899,
            ///<summary>No slot exists to draw the symbol</summary>
            SYMBOLS_FULL = 900,
            ///<summary>The symbol information cannot be found.</summary>
            SYMBOL_NOT_FOUND = 901,
            ///<summary>The number of items is incorrect.</summary>
            NUMBER_INCORRECT = 902,
            ///<summary>You may not submit a petition while frozen. Be patient.</summary>
            NO_PETITION_WHILE_FROZEN = 903,
            ///<summary>Items cannot be discarded while in private store status.</summary>
            NO_DISCARD_WHILE_PRIVATE_STORE = 904,
            ///<summary>The current score for the Humans is $s1.</summary>
            HUMAN_SCORE_S1 = 905,
            ///<summary>The current score for the Elves is $s1.</summary>
            ELVES_SCORE_S1 = 906,
            ///<summary>The current score for the Dark Elves is $s1.</summary>
            DARK_ELVES_SCORE_S1 = 907,
            ///<summary>The current score for the Orcs is $s1.</summary>
            ORCS_SCORE_S1 = 908,
            ///<summary>The current score for the Dwarves is $s1.</summary>
            DWARVEN_SCORE_S1 = 909,
            ///<summary>Current location : $s1, $s2, $s3 (Near Talking Island Village)</summary>
            LOC_TI_S1_S2_S3 = 910,
            ///<summary>Current location : $s1, $s2, $s3 (Near Gludin Village)</summary>
            LOC_GLUDIN_S1_S2_S3 = 911,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Gludio)</summary>
            LOC_GLUDIO_S1_S2_S3 = 912,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Neutral Zone)</summary>
            LOC_NEUTRAL_ZONE_S1_S2_S3 = 913,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Elven Village)</summary>
            LOC_ELVEN_S1_S2_S3 = 914,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Dark Elf Village)</summary>
            LOC_DARK_ELVEN_S1_S2_S3 = 915,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Dion)</summary>
            LOC_DION_S1_S2_S3 = 916,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Floran Village)</summary>
            LOC_FLORAN_S1_S2_S3 = 917,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Giran)</summary>
            LOC_GIRAN_S1_S2_S3 = 918,
            ///<summary>Current location : $s1, $s2, $s3 (Near Giran Harbor)</summary>
            LOC_GIRAN_HARBOR_S1_S2_S3 = 919,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Orc Village)</summary>
            LOC_ORC_S1_S2_S3 = 920,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Dwarven Village)</summary>
            LOC_DWARVEN_S1_S2_S3 = 921,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Town of Oren)</summary>
            LOC_OREN_S1_S2_S3 = 922,
            ///<summary>Current location : $s1, $s2, $s3 (Near Hunters Village)</summary>
            LOC_HUNTER_S1_S2_S3 = 923,
            ///<summary>Current location : $s1, $s2, $s3 (Near Aden Castle Town)</summary>
            LOC_ADEN_S1_S2_S3 = 924,
            ///<summary>Current location : $s1, $s2, $s3 (Near the Coliseum)</summary>
            LOC_COLISEUM_S1_S2_S3 = 925,
            ///<summary>Current location : $s1, $s2, $s3 (Near Heine)</summary>
            LOC_HEINE_S1_S2_S3 = 926,
            ///<summary>The current time is $s1:$s2.</summary>
            TIME_S1_S2_IN_THE_DAY = 927,
            ///<summary>The current time is $s1:$s2.</summary>
            TIME_S1_S2_IN_THE_NIGHT = 928,
            ///<summary>No compensation was given for the farm products.</summary>
            NO_COMPENSATION_FOR_FARM_PRODUCTS = 929,
            ///<summary>Lottery tickets are not currently being sold.</summary>
            NO_LOTTERY_TICKETS_CURRENT_SOLD = 930,
            ///<summary>The winning lottery ticket numbers has not yet been anonunced.</summary>
            LOTTERY_WINNERS_NOT_ANNOUNCED_YET = 931,
            ///<summary>You cannot chat locally while observing.</summary>
            NO_ALLCHAT_WHILE_OBSERVING = 932,
            ///<summary>The seed pricing greatly differs from standard seed prices.</summary>
            THE_SEED_PRICING_GREATLY_DIFFERS_FROM_STANDARD_SEED_PRICES = 933,
            ///<summary>It is a deleted recipe.</summary>
            A_DELETED_RECIPE = 934,
            ///<summary>The amount is not sufficient and so the manor is not in operation.</summary>
            THE_AMOUNT_IS_NOT_SUFFICIENT_AND_SO_THE_MANOR_IS_NOT_IN_OPERATION = 935,
            ///<summary>Use $s1.</summary>
            USE_S1_ = 936,
            ///<summary>Currently preparing for private workshop.</summary>
            PREPARING_PRIVATE_WORKSHOP = 937,
            ///<summary>The community server is currently offline.</summary>
            CB_OFFLINE = 938,
            ///<summary>You cannot exchange while blocking everything.</summary>
            NO_EXCHANGE_WHILE_BLOCKING = 939,
            ///<summary>$s1 is blocked everything.</summary>
            S1_BLOCKED_EVERYTHING = 940,
            ///<summary>Restart at Talking Island Village.</summary>
            RESTART_AT_TI = 941,
            ///<summary>Restart at Gludin Village.</summary>
            RESTART_AT_GLUDIN = 942,
            ///<summary>Restart at the Town of Gludin. || guess should be Gludio ;)</summary>
            RESTART_AT_GLUDIO = 943,
            ///<summary>Restart at the Neutral Zone.</summary>
            RESTART_AT_NEUTRAL_ZONE = 944,
            ///<summary>Restart at the Elven Village.</summary>
            RESTART_AT_ELFEN_VILLAGE = 945,
            ///<summary>Restart at the Dark Elf Village.</summary>
            RESTART_AT_DARKELF_VILLAGE = 946,
            ///<summary>Restart at the Town of Dion.</summary>
            RESTART_AT_DION = 947,
            ///<summary>Restart at Floran Village.</summary>
            RESTART_AT_FLORAN = 948,
            ///<summary>Restart at the Town of Giran.</summary>
            RESTART_AT_GIRAN = 949,
            ///<summary>Restart at Giran Harbor.</summary>
            RESTART_AT_GIRAN_HARBOR = 950,
            ///<summary>Restart at the Orc Village.</summary>
            RESTART_AT_ORC_VILLAGE = 951,
            ///<summary>Restart at the Dwarven Village.</summary>
            RESTART_AT_DWARFEN_VILLAGE = 952,
            ///<summary>Restart at the Town of Oren.</summary>
            RESTART_AT_OREN = 953,
            ///<summary>Restart at Hunters Village.</summary>
            RESTART_AT_HUNTERS_VILLAGE = 954,
            ///<summary>Restart at the Town of Aden.</summary>
            RESTART_AT_ADEN = 955,
            ///<summary>Restart at the Coliseum.</summary>
            RESTART_AT_COLISEUM = 956,
            ///<summary>Restart at Heine.</summary>
            RESTART_AT_HEINE = 957,
            ///<summary>Items cannot be discarded or destroyed while operating a private store or workshop.</summary>
            ITEMS_CANNOT_BE_DISCARDED_OR_DESTROYED_WHILE_OPERATING_PRIVATE_STORE_OR_WORKSHOP = 958,
            ///<summary>$s1 (*$s2) manufactured successfully.</summary>
            S1_S2_MANUFACTURED_SUCCESSFULLY = 959,
            ///<summary>$s1 manufacturing failure.</summary>
            S1_MANUFACTURE_FAILURE = 960,
            ///<summary>You are now blocking everything.</summary>
            BLOCKING_ALL = 961,
            ///<summary>You are no longer blocking everything.</summary>
            NOT_BLOCKING_ALL = 962,
            ///<summary>Please determine the manufacturing price.</summary>
            DETERMINE_MANUFACTURE_PRICE = 963,
            ///<summary>Chatting is prohibited for one minute.</summary>
            CHATBAN_FOR_1_MINUTE = 964,
            ///<summary>The chatting prohibition has been removed.</summary>
            CHATBAN_REMOVED = 965,
            ///<summary>Chatting is currently prohibited. If you try to chat before the prohibition is removed, the prohibition time will become even longer.</summary>
            CHATTING_IS_CURRENTLY_PROHIBITED = 966,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: Random including spoil.)</summary>
            S1_PARTY_INVITE_RANDOM_INCLUDING_SPOIL = 967,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: By Turn.)</summary>
            S1_PARTY_INVITE_BY_TURN = 968,
            ///<summary>Do you accept $s1's party invitation? (Item Distribution: By Turn including spoil.)</summary>
            S1_PARTY_INVITE_BY_TURN_INCLUDING_SPOIL = 969,
            ///<summary>$s2's MP has been drained by $s1.</summary>
            S2_MP_HAS_BEEN_DRAINED_BY_S1 = 970,
            ///<summary>Petitions cannot exceed 255 characters.</summary>
            PETITION_MAX_CHARS_255 = 971,
            ///<summary>This pet cannot use this item.</summary>
            PET_CANNOT_USE_ITEM = 972,
            ///<summary>Please input no more than the number you have.</summary>
            INPUT_NO_MORE_YOU_HAVE = 973,
            ///<summary>The soul crystal succeeded in absorbing a soul.</summary>
            SOUL_CRYSTAL_ABSORBING_SUCCEEDED = 974,
            ///<summary>The soul crystal was not able to absorb a soul.</summary>
            SOUL_CRYSTAL_ABSORBING_FAILED = 975,
            ///<summary>The soul crystal broke because it was not able to endure the soul energy.</summary>
            SOUL_CRYSTAL_BROKE = 976,
            ///<summary>The soul crystals caused resonation and failed at absorbing a soul.</summary>
            SOUL_CRYSTAL_ABSORBING_FAILED_RESONATION = 977,
            ///<summary>The soul crystal is refusing to absorb a soul.</summary>
            SOUL_CRYSTAL_ABSORBING_REFUSED = 978,
            ///<summary>The ferry arrived at Talking Island Harbor.</summary>
            FERRY_ARRIVED_AT_TALKING = 979,
            ///<summary>The ferry will leave for Gludin Harbor after anchoring for ten minutes.</summary>
            FERRY_LEAVE_FOR_GLUDIN_AFTER_10_MINUTES = 980,
            ///<summary>The ferry will leave for Gludin Harbor in five minutes.</summary>
            FERRY_LEAVE_FOR_GLUDIN_IN_5_MINUTES = 981,
            ///<summary>The ferry will leave for Gludin Harbor in one minute.</summary>
            FERRY_LEAVE_FOR_GLUDIN_IN_1_MINUTE = 982,
            ///<summary>Those wishing to ride should make haste to get on.</summary>
            MAKE_HASTE_GET_ON_BOAT = 983,
            ///<summary>The ferry will be leaving soon for Gludin Harbor.</summary>
            FERRY_LEAVE_SOON_FOR_GLUDIN = 984,
            ///<summary>The ferry is leaving for Gludin Harbor.</summary>
            FERRY_LEAVING_FOR_GLUDIN = 985,
            ///<summary>The ferry has arrived at Gludin Harbor.</summary>
            FERRY_ARRIVED_AT_GLUDIN = 986,
            ///<summary>The ferry will leave for Talking Island Harbor after anchoring for ten minutes.</summary>
            FERRY_LEAVE_FOR_TALKING_AFTER_10_MINUTES = 987,
            ///<summary>The ferry will leave for Talking Island Harbor in five minutes.</summary>
            FERRY_LEAVE_FOR_TALKING_IN_5_MINUTES = 988,
            ///<summary>The ferry will leave for Talking Island Harbor in one minute.</summary>
            FERRY_LEAVE_FOR_TALKING_IN_1_MINUTE = 989,
            ///<summary>The ferry will be leaving soon for Talking Island Harbor.</summary>
            FERRY_LEAVE_SOON_FOR_TALKING = 990,
            ///<summary>The ferry is leaving for Talking Island Harbor.</summary>
            FERRY_LEAVING_FOR_TALKING = 991,
            ///<summary>The ferry has arrived at Giran Harbor.</summary>
            FERRY_ARRIVED_AT_GIRAN = 992,
            ///<summary>The ferry will leave for Giran Harbor after anchoring for ten minutes.</summary>
            FERRY_LEAVE_FOR_GIRAN_AFTER_10_MINUTES = 993,
            ///<summary>The ferry will leave for Giran Harbor in five minutes.</summary>
            FERRY_LEAVE_FOR_GIRAN_IN_5_MINUTES = 994,
            ///<summary>The ferry will leave for Giran Harbor in one minute.</summary>
            FERRY_LEAVE_FOR_GIRAN_IN_1_MINUTE = 995,
            ///<summary>The ferry will be leaving soon for Giran Harbor.</summary>
            FERRY_LEAVE_SOON_FOR_GIRAN = 996,
            ///<summary>The ferry is leaving for Giran Harbor.</summary>
            FERRY_LEAVING_FOR_GIRAN = 997,
            ///<summary>The Innadril pleasure boat has arrived. It will anchor for ten minutes.</summary>
            INNADRIL_BOAT_ANCHOR_10_MINUTES = 998,
            ///<summary>The Innadril pleasure boat will leave in five minutes.</summary>
            INNADRIL_BOAT_LEAVE_IN_5_MINUTES = 999,
            ///<summary>The Innadril pleasure boat will leave in one minute.</summary>
            INNADRIL_BOAT_LEAVE_IN_1_MINUTE = 1000,
            ///<summary>The Innadril pleasure boat will be leaving soon.</summary>
            INNADRIL_BOAT_LEAVE_SOON = 1001,
            ///<summary>The Innadril pleasure boat is leaving.</summary>
            INNADRIL_BOAT_LEAVING = 1002,
            ///<summary>Cannot possess a monster race ticket.</summary>
            CANNOT_POSSES_MONS_TICKET = 1003,
            ///<summary>You have registered for a clan hall auction.</summary>
            REGISTERED_FOR_CLANHALL = 1004,
            ///<summary>There is not enough adena in the clan hall warehouse.</summary>
            NOT_ENOUGH_ADENA_IN_CWH = 1005,
            ///<summary>You have bid in a clan hall auction.</summary>
            BID_IN_CLANHALL_AUCTION = 1006,
            ///<summary>The preliminary match registration of $s1 has finished.</summary>
            PRELIMINARY_REGISTRATION_OF_S1_FINISHED = 1007,
            ///<summary>A hungry strider cannot be mounted or dismounted.</summary>
            HUNGRY_STRIDER_NOT_MOUNT = 1008,
            ///<summary>A strider cannot be ridden when dead.</summary>
            STRIDER_CANT_BE_RIDDEN_WHILE_DEAD = 1009,
            ///<summary>A dead strider cannot be ridden.</summary>
            DEAD_STRIDER_CANT_BE_RIDDEN = 1010,
            ///<summary>A strider in battle cannot be ridden.</summary>
            STRIDER_IN_BATLLE_CANT_BE_RIDDEN = 1011,
            ///<summary>A strider cannot be ridden while in battle.</summary>
            STRIDER_CANT_BE_RIDDEN_WHILE_IN_BATTLE = 1012,
            ///<summary>A strider can be ridden only when standing.</summary>
            STRIDER_CAN_BE_RIDDEN_ONLY_WHILE_STANDING = 1013,
            ///<summary>Your pet gained $s1 experience points.</summary>
            PET_EARNED_S1_EXP = 1014,
            ///<summary>Your pet hit for $s1 damage.</summary>
            PET_HIT_FOR_S1_DAMAGE = 1015,
            ///<summary>Pet received $s2 damage by $s1.</summary>
            PET_RECEIVED_S2_DAMAGE_BY_S1 = 1016,
            ///<summary>Pet's critical hit!</summary>
            CRITICAL_HIT_BY_PET = 1017,
            ///<summary>Your pet uses $s1.</summary>
            PET_USES_S1 = 1018,
            ///<summary>Your pet uses $s1.</summary>
            PET_USES_S1_ = 1019,
            ///<summary>Your pet picked up $s1.</summary>
            PET_PICKED_S1 = 1020,
            ///<summary>Your pet picked up $s2 $s1(s).</summary>
            PET_PICKED_S2_S1_S = 1021,
            ///<summary>Your pet picked up +$s1 $s2.</summary>
            PET_PICKED_S1_S2 = 1022,
            ///<summary>Your pet picked up $s1 adena.</summary>
            PET_PICKED_S1_ADENA = 1023,
            ///<summary>Your pet put on $s1.</summary>
            PET_PUT_ON_S1 = 1024,
            ///<summary>Your pet took off $s1.</summary>
            PET_TOOK_OFF_S1 = 1025,
            ///<summary>The summoned monster gave damage of $s1</summary>
            SUMMON_GAVE_DAMAGE_S1 = 1026,
            ///<summary>Servitor received $s2 damage caused by $s1.</summary>
            SUMMON_RECEIVED_DAMAGE_S2_BY_S1 = 1027,
            ///<summary>Summoned monster's critical hit!</summary>
            CRITICAL_HIT_BY_SUMMONED_MOB = 1028,
            ///<summary>Summoned monster uses $s1.</summary>
            SUMMONED_MOB_USES_S1 = 1029,
            ///<summary><Party Information></summary>
            PARTY_INFORMATION = 1030,
            ///<summary>Looting method: Finders keepers</summary>
            LOOTING_FINDERS_KEEPERS = 1031,
            ///<summary>Looting method: Random</summary>
            LOOTING_RANDOM = 1032,
            ///<summary>Looting method: Random including spoil</summary>
            LOOTING_RANDOM_INCLUDE_SPOIL = 1033,
            ///<summary>Looting method: By turn</summary>
            LOOTING_BY_TURN = 1034,
            ///<summary>Looting method: By turn including spoil</summary>
            LOOTING_BY_TURN_INCLUDE_SPOIL = 1035,
            ///<summary>You have exceeded the quantity that can be inputted.</summary>
            YOU_HAVE_EXCEEDED_QUANTITY_THAT_CAN_BE_INPUTTED = 1036,
            ///<summary>$s1 manufactured $s2.</summary>
            S1_MANUFACTURED_S2 = 1037,
            ///<summary>$s1 manufactured $s3 $s2(s).</summary>
            S1_MANUFACTURED_S3_S2_S = 1038,
            ///<summary>Items left at the clan hall warehouse can only be retrieved by the clan leader. Do you want to continue?</summary>
            ONLY_CLAN_LEADER_CAN_RETRIEVE_ITEMS_FROM_CLAN_WAREHOUSE = 1039,
            ///<summary>Items sent by freight can be picked up from any Warehouse location. Do you want to continue?</summary>
            ITEMS_SENT_BY_FREIGHT_PICKED_UP_FROM_ANYWHERE = 1040,
            ///<summary>The next seed purchase price is $s1 adena.</summary>
            THE_NEXT_SEED_PURCHASE_PRICE_IS_S1_ADENA = 1041,
            ///<summary>The next farm goods purchase price is $s1 adena.</summary>
            THE_NEXT_FARM_GOODS_PURCHASE_PRICE_IS_S1_ADENA = 1042,
            ///<summary>At the current time, the "/unstuck" command cannot be used. Please send in a petition.</summary>
            NO_UNSTUCK_PLEASE_SEND_PETITION = 1043,
            ///<summary>Monster race payout information is not available while tickets are being sold.</summary>
            MONSRACE_NO_PAYOUT_INFO = 1044,
            ///<summary>Monster race tickets are no longer available.</summary>
            MONSRACE_TICKETS_NOT_AVAILABLE = 1046,
            ///<summary>We did not succeed in producing $s1 item.</summary>
            NOT_SUCCEED_PRODUCING_S1 = 1047,
            ///<summary>When "blocking" everything, whispering is not possible.</summary>
            NO_WHISPER_WHEN_BLOCKING = 1048,
            ///<summary>When "blocking" everything, it is not possible to send invitations for organizing parties.</summary>
            NO_PARTY_WHEN_BLOCKING = 1049,
            ///<summary>There are no communities in my clan. Clan communities are allowed for clans with skill levels of 2 and higher.</summary>
            NO_CB_IN_MY_CLAN = 1050,
            ///<summary>Payment for your clan hall has not been made please make payment tomorrow.</summary>
            PAYMENT_FOR_YOUR_CLAN_HALL_HAS_NOT_BEEN_MADE_PLEASE_MAKE_PAYMENT_TO_YOUR_CLAN_WAREHOUSE_BY_S1_TOMORROW = 1051,
            ///<summary>Payment of Clan Hall is overdue the owner loose Clan Hall.</summary>
            THE_CLAN_HALL_FEE_IS_ONE_WEEK_OVERDUE_THEREFORE_THE_CLAN_HALL_OWNERSHIP_HAS_BEEN_REVOKED = 1052,
            ///<summary>It is not possible to resurrect in battlefields where a siege war is taking place.</summary>
            CANNOT_BE_RESURRECTED_DURING_SIEGE = 1053,
            ///<summary>You have entered a mystical land.</summary>
            ENTERED_MYSTICAL_LAND = 1054,
            ///<summary>You have left a mystical land.</summary>
            EXITED_MYSTICAL_LAND = 1055,
            ///<summary>You have exceeded the storage capacity of the castle's vault.</summary>
            VAULT_CAPACITY_EXCEEDED = 1056,
            ///<summary>This command can only be used in the relax server.</summary>
            RELAX_SERVER_ONLY = 1057,
            ///<summary>The sales price for seeds is $s1 adena.</summary>
            THE_SALES_PRICE_FOR_SEEDS_IS_S1_ADENA = 1058,
            ///<summary>The remaining purchasing amount is $s1 adena.</summary>
            THE_REMAINING_PURCHASING_IS_S1_ADENA = 1059,
            ///<summary>The remainder after selling the seeds is $s1.</summary>
            THE_REMAINDER_AFTER_SELLING_THE_SEEDS_IS_S1 = 1060,
            ///<summary>The recipe cannot be registered. You do not have the ability to create items.</summary>
            CANT_REGISTER_NO_ABILITY_TO_CRAFT = 1061,
            ///<summary>Writing something new is possible after level 10.</summary>
            WRITING_SOMETHING_NEW_POSSIBLE_AFTER_LEVEL_10 = 1062,
            /// if you become trapped or unable to move, please use the '/unstuck' command.</summary>
            PETITION_UNAVAILABLE = 1063,
            ///<summary>The equipment, +$s1 $s2, has been removed.</summary>
            EQUIPMENT_S1_S2_REMOVED = 1064,
            ///<summary>While operating a private store or workshop, you cannot discard, destroy, or trade an item.</summary>
            CANNOT_TRADE_DISCARD_DROP_ITEM_WHILE_IN_SHOPMODE = 1065,
            ///<summary>$s1 HP has been restored.</summary>
            S1_HP_RESTORED = 1066,
            ///<summary>$s2 HP has been restored by $s1</summary>
            S2_HP_RESTORED_BY_S1 = 1067,
            ///<summary>$s1 MP has been restored.</summary>
            S1_MP_RESTORED = 1068,
            ///<summary>$s2 MP has been restored by $s1.</summary>
            S2_MP_RESTORED_BY_S1 = 1069,
            ///<summary>You do not have 'read' permission.</summary>
            NO_READ_PERMISSION = 1070,
            ///<summary>You do not have 'write' permission.</summary>
            NO_WRITE_PERMISSION = 1071,
            ///<summary>You have obtained a ticket for the Monster Race #$s1 - Single</summary>
            OBTAINED_TICKET_FOR_MONS_RACE_S1_SINGLE = 1072,
            ///<summary>You have obtained a ticket for the Monster Race #$s1 - Single</summary>
            OBTAINED_TICKET_FOR_MONS_RACE_S1_SINGLE_ = 1073,
            ///<summary>You do not meet the age requirement to purchase a Monster Race Ticket.</summary>
            NOT_MEET_AGE_REQUIREMENT_FOR_MONS_RACE = 1074,
            ///<summary>The bid amount must be higher than the previous bid.</summary>
            BID_AMOUNT_HIGHER_THAN_PREVIOUS_BID = 1075,
            ///<summary>The game cannot be terminated at this time.</summary>
            GAME_CANNOT_TERMINATE_NOW = 1076,
            ///<summary>A GameGuard Execution error has occurred. Please send the *.erl file(s) located in the GameGuard folder to game@inca.co.kr</summary>
            GG_EXECUTION_ERROR = 1077,
            ///<summary>When a user's keyboard input exceeds a certain cumulative score a chat ban will be applied. This is done to discourage spamming. Please avoid posting the same message multiple times during a short period.</summary>
            DONT_SPAM = 1078,
            ///<summary>The target is currently banend from chatting.</summary>
            TARGET_IS_CHAT_BANNED = 1079,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type A?</summary>
            FACELIFT_POTION_TYPE_A = 1080,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type A?</summary>
            HAIRDYE_POTION_TYPE_A = 1081,
            ///<summary>Do you wish to use the hair style change potion - Type A? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_A = 1082,
            ///<summary>Facelift potion - Type A is being applied.</summary>
            FACELIFT_POTION_TYPE_A_APPLIED = 1083,
            ///<summary>Hair dye potion - Type A is being applied.</summary>
            HAIRDYE_POTION_TYPE_A_APPLIED = 1084,
            ///<summary>The hair style chance potion - Type A is being used.</summary>
            HAIRSTYLE_POTION_TYPE_A_USED = 1085,
            ///<summary>Your facial appearance has been changed.</summary>
            FACE_APPEARANCE_CHANGED = 1086,
            ///<summary>Your hair color has changed.</summary>
            HAIR_COLOR_CHANGED = 1087,
            ///<summary>Your hair style has been changed.</summary>
            HAIR_STYLE_CHANGED = 1088,
            ///<summary>$s1 has obtained a first anniversary commemorative item.</summary>
            S1_OBTAINED_ANNIVERSARY_ITEM = 1089,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type B?</summary>
            FACELIFT_POTION_TYPE_B = 1090,
            ///<summary>Being permanent, are you sure you wish to use the facelift potion - Type C?</summary>
            FACELIFT_POTION_TYPE_C = 1091,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type B?</summary>
            HAIRDYE_POTION_TYPE_B = 1092,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type C?</summary>
            HAIRDYE_POTION_TYPE_C = 1093,
            ///<summary>Being permanent, are you sure you wish to use the hair dye potion - Type D?</summary>
            HAIRDYE_POTION_TYPE_D = 1094,
            ///<summary>Do you wish to use the hair style change potion - Type B? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_B = 1095,
            ///<summary>Do you wish to use the hair style change potion - Type C? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_C = 1096,
            ///<summary>Do you wish to use the hair style change potion - Type D? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_D = 1097,
            ///<summary>Do you wish to use the hair style change potion - Type E? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_E = 1098,
            ///<summary>Do you wish to use the hair style change potion - Type F? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_F = 1099,
            ///<summary>Do you wish to use the hair style change potion - Type G? It is permanent.</summary>
            HAIRSTYLE_POTION_TYPE_G = 1100,
            ///<summary>Facelift potion - Type B is being applied.</summary>
            FACELIFT_POTION_TYPE_B_APPLIED = 1101,
            ///<summary>Facelift potion - Type C is being applied.</summary>
            FACELIFT_POTION_TYPE_C_APPLIED = 1102,
            ///<summary>Hair dye potion - Type B is being applied.</summary>
            HAIRDYE_POTION_TYPE_B_APPLIED = 1103,
            ///<summary>Hair dye potion - Type C is being applied.</summary>
            HAIRDYE_POTION_TYPE_C_APPLIED = 1104,
            ///<summary>Hair dye potion - Type D is being applied.</summary>
            HAIRDYE_POTION_TYPE_D_APPLIED = 1105,
            ///<summary>The hair style chance potion - Type B is being used.</summary>
            HAIRSTYLE_POTION_TYPE_B_USED = 1106,
            ///<summary>The hair style chance potion - Type C is being used.</summary>
            HAIRSTYLE_POTION_TYPE_C_USED = 1107,
            ///<summary>The hair style chance potion - Type D is being used.</summary>
            HAIRSTYLE_POTION_TYPE_D_USED = 1108,
            ///<summary>The hair style chance potion - Type E is being used.</summary>
            HAIRSTYLE_POTION_TYPE_E_USED = 1109,
            ///<summary>The hair style chance potion - Type F is being used.</summary>
            HAIRSTYLE_POTION_TYPE_F_USED = 1110,
            ///<summary>The hair style chance potion - Type G is being used.</summary>
            HAIRSTYLE_POTION_TYPE_G_USED = 1111,
            ///<summary>The prize amount for the winner of Lottery #$s1 is $s2 adena. We have $s3 first prize winners.</summary>
            AMOUNT_FOR_WINNER_S1_IS_S2_ADENA_WE_HAVE_S3_PRIZE_WINNER = 1112,
            ///<summary>The prize amount for Lucky Lottery #$s1 is $s2 adena. There was no first prize winner in this drawing, therefore the jackpot will be added to the next drawing.</summary>
            AMOUNT_FOR_LOTTERY_S1_IS_S2_ADENA_NO_WINNER = 1113,
            ///<summary>Your clan may not register to participate in a siege while under a grace period of the clan's dissolution.</summary>
            CANT_PARTICIPATE_IN_SIEGE_WHILE_DISSOLUTION_IN_PROGRESS = 1114,
            ///<summary>Individuals may not surrender during combat.</summary>
            INDIVIDUALS_NOT_SURRENDER_DURING_COMBAT = 1115,
            ///<summary>One cannot leave one's clan during combat.</summary>
            YOU_CANNOT_LEAVE_DURING_COMBAT = 1116,
            ///<summary>A clan member may not be dismissed during combat.</summary>
            CLAN_MEMBER_CANNOT_BE_DISMISSED_DURING_COMBAT = 1117,
            ///<summary>Progress in a quest is possible only when your inventory's weight and volume are less than 80 percent of capacity.</summary>
            INVENTORY_LESS_THAN_80_PERCENT = 1118,
            ///<summary>Quest was automatically canceled when you attempted to settle the accounts of your quest while your inventory exceeded 80 percent of capacity.</summary>
            QUEST_CANCELED_INVENTORY_EXCEEDS_80_PERCENT = 1119,
            ///<summary>You are still a member of the clan.</summary>
            STILL_CLAN_MEMBER = 1120,
            ///<summary>You do not have the right to vote.</summary>
            NO_RIGHT_TO_VOTE = 1121,
            ///<summary>There is no candidate.</summary>
            NO_CANDIDATE = 1122,
            ///<summary>Weight and volume limit has been exceeded. That skill is currently unavailable.</summary>
            WEIGHT_EXCEEDED_SKILL_UNAVAILABLE = 1123,
            ///<summary>Your recipe book may not be accessed while using a skill.</summary>
            NO_RECIPE_BOOK_WHILE_CASTING = 1124,
            ///<summary>An item may not be created while engaged in trading.</summary>
            CANNOT_CREATED_WHILE_ENGAGED_IN_TRADING = 1125,
            ///<summary>You cannot enter a negative number.</summary>
            NO_NEGATIVE_NUMBER = 1126,
            ///<summary>The reward must be less than 10 times the standard price.</summary>
            REWARD_LESS_THAN_10_TIMES_STANDARD_PRICE = 1127,
            ///<summary>A private store may not be opened while using a skill.</summary>
            PRIVATE_STORE_NOT_WHILE_CASTING = 1128,
            ///<summary>This is not allowed while riding a ferry or boat.</summary>
            NOT_ALLOWED_ON_BOAT = 1129,
            ///<summary>You have given $s1 damage to your target and $s2 damage to the servitor.</summary>
            GIVEN_S1_DAMAGE_TO_YOUR_TARGET_AND_S2_DAMAGE_TO_SERVITOR = 1130,
            ///<summary>It is now midnight and the effect of $s1 can be felt.</summary>
            NIGHT_S1_EFFECT_APPLIES = 1131,
            ///<summary>It is now dawn and the effect of $s1 will now disappear.</summary>
            DAY_S1_EFFECT_DISAPPEARS = 1132,
            ///<summary>Since HP has decreased, the effect of $s1 can be felt.</summary>
            HP_DECREASED_EFFECT_APPLIES = 1133,
            ///<summary>Since HP has increased, the effect of $s1 will disappear.</summary>
            HP_INCREASED_EFFECT_DISAPPEARS = 1134,
            ///<summary>While you are engaged in combat, you cannot operate a private store or private workshop.</summary>
            CANT_OPERATE_PRIVATE_STORE_DURING_COMBAT = 1135,
            ///<summary>Since there was an account that used this IP and attempted to log in illegally, this account is not allowed to connect to the game server for $s1 minutes. Please use another game server.</summary>
            ACCOUNT_NOT_ALLOWED_TO_CONNECT = 1136,
            ///<summary>$s1 harvested $s3 $s2(s).</summary>
            S1_HARVESTED_S3_S2S = 1137,
            ///<summary>$s1 harvested $s2(s).</summary>
            S1_HARVESTED_S2S = 1138,
            ///<summary>The weight and volume limit of your inventory must not be exceeded.</summary>
            INVENTORY_LIMIT_MUST_NOT_BE_EXCEEDED = 1139,
            ///<summary>Would you like to open the gate?</summary>
            WOULD_YOU_LIKE_TO_OPEN_THE_GATE = 1140,
            ///<summary>Would you like to close the gate?</summary>
            WOULD_YOU_LIKE_TO_CLOSE_THE_GATE = 1141,
            ///<summary>Since $s1 already exists nearby, you cannot summon it again.</summary>
            CANNOT_SUMMON_S1_AGAIN = 1142,
            ///<summary>Since you do not have enough items to maintain the servitor's stay, the servitor will disappear.</summary>
            SERVITOR_DISAPPEARED_NOT_ENOUGH_ITEMS = 1143,
            ///<summary>Currently, you don't have anybody to chat with in the game.</summary>
            NOBODY_IN_GAME_TO_CHAT = 1144,
            ///<summary>$s2 has been created for $s1 after the payment of $s3 adena is received.</summary>
            S2_CREATED_FOR_S1_FOR_S3_ADENA = 1145,
            ///<summary>$s1 created $s2 after receiving $s3 adena.</summary>
            S1_CREATED_S2_FOR_S3_ADENA = 1146,
            ///<summary>$s2 $s3 have been created for $s1 at the price of $s4 adena.</summary>
            S2_S3_S_CREATED_FOR_S1_FOR_S4_ADENA = 1147,
            ///<summary>$s1 created $s2 $s3 at the price of $s4 adena.</summary>
            S1_CREATED_S2_S3_S_FOR_S4_ADENA = 1148,
            ///<summary>Your attempt to create $s2 for $s1 at the price of $s3 adena has failed.</summary>
            CREATION_OF_S2_FOR_S1_AT_S3_ADENA_FAILED = 1149,
            ///<summary>$s1 has failed to create $s2 at the price of $s3 adena.</summary>
            S1_FAILED_TO_CREATE_S2_FOR_S3_ADENA = 1150,
            ///<summary>$s2 is sold to $s1 at the price of $s3 adena.</summary>
            S2_SOLD_TO_S1_FOR_S3_ADENA = 1151,
            ///<summary>$s2 $s3 have been sold to $s1 for $s4 adena.</summary>
            S3_S2_S_SOLD_TO_S1_FOR_S4_ADENA = 1152,
            ///<summary>$s2 has been purchased from $s1 at the price of $s3 adena.</summary>
            S2_PURCHASED_FROM_S1_FOR_S3_ADENA = 1153,
            ///<summary>$s3 $s2 has been purchased from $s1 for $s4 adena.</summary>
            S3_S2_S_PURCHASED_FROM_S1_FOR_S4_ADENA = 1154,
            ///<summary>+$s2 $s3 have been sold to $s1 for $s4 adena.</summary>
            S3_S2_SOLD_TO_S1_FOR_S4_ADENA = 1155,
            ///<summary>+$s2 $s3 has been purchased from $s1 for $s4 adena.</summary>
            S2_S3_PURCHASED_FROM_S1_FOR_S4_ADENA = 1156,
            ///<summary>Trying on state lasts for only 5 seconds. When a character's state changes, it can be cancelled.</summary>
            TRYING_ON_STATE = 1157,
            ///<summary>You cannot dismount from this elevation.</summary>
            CANNOT_DISMOUNT_FROM_ELEVATION = 1158,
            ///<summary>The ferry from Talking Island will arrive at Gludin Harbor in approximately 10 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_10_MINUTES = 1159,
            ///<summary>The ferry from Talking Island will be arriving at Gludin Harbor in approximately 5 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_5_MINUTES = 1160,
            ///<summary>The ferry from Talking Island will be arriving at Gludin Harbor in approximately 1 minute.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_1_MINUTE = 1161,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 15 minutes.</summary>
            FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_15_MINUTES = 1162,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 10 minutes.</summary>
            FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_10_MINUTES = 1163,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 5 minutes.</summary>
            FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_5_MINUTES = 1164,
            ///<summary>The ferry from Giran Harbor will be arriving at Talking Island in approximately 1 minute.</summary>
            FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_1_MINUTE = 1165,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_20_MINUTES = 1166,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_15_MINUTES = 1167,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_10_MINUTES = 1168,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_5_MINUTES = 1169,
            ///<summary>The ferry from Talking Island will be arriving at Giran Harbor in approximately 1 minute.</summary>
            FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_1_MINUTE = 1170,
            ///<summary>The Innadril pleasure boat will arrive in approximately 20 minutes.</summary>
            INNADRIL_BOAT_ARRIVE_20_MINUTES = 1171,
            ///<summary>The Innadril pleasure boat will arrive in approximately 15 minutes.</summary>
            INNADRIL_BOAT_ARRIVE_15_MINUTES = 1172,
            ///<summary>The Innadril pleasure boat will arrive in approximately 10 minutes.</summary>
            INNADRIL_BOAT_ARRIVE_10_MINUTES = 1173,
            ///<summary>The Innadril pleasure boat will arrive in approximately 5 minutes.</summary>
            INNADRIL_BOAT_ARRIVE_5_MINUTES = 1174,
            ///<summary>The Innadril pleasure boat will arrive in approximately 1 minute.</summary>
            INNADRIL_BOAT_ARRIVE_1_MINUTE = 1175,
            ///<summary>This is a quest event period.</summary>
            QUEST_EVENT_PERIOD = 1176,
            ///<summary>This is the seal validation period.</summary>
            VALIDATION_PERIOD = 1177,
            /// <Seal of Avarice description></summary>
            AVARICE_DESCRIPTION = 1178,
            /// <Seal of Gnosis description></summary>
            GNOSIS_DESCRIPTION = 1179,
            /// <Seal of Strife description></summary>
            STRIFE_DESCRIPTION = 1180,
            ///<summary>Do you really wish to change the title?</summary>
            CHANGE_TITLE_CONFIRM = 1181,
            ///<summary>Are you sure you wish to delete the clan crest?</summary>
            CREST_DELETE_CONFIRM = 1182,
            ///<summary>This is the initial period.</summary>
            INITIAL_PERIOD = 1183,
            ///<summary>This is a period of calculating statistics in the server.</summary>
            RESULTS_PERIOD = 1184,
            ///<summary>days left until deletion.</summary>
            DAYS_LEFT_UNTIL_DELETION = 1185,
            ///<summary>To create a new account, please visit the PlayNC website (http://www.plaync.com/us/support/)</summary>
            TO_CREATE_ACCOUNT_VISIT_WEBSITE = 1186,
            ///<summary>If you forgotten your account information or password, please visit the Support Center on the PlayNC website(http://www.plaync.com/us/support/)</summary>
            ACCOUNT_INFORMATION_FORGOTTON_VISIT_WEBSITE = 1187,
            ///<summary>Your selected target can no longer receive a recommendation.</summary>
            YOUR_TARGET_NO_LONGER_RECEIVE_A_RECOMMENDATION = 1188,
            ///<summary>This temporary alliance of the Castle Attacker team is in effect. It will be dissolved when the Castle Lord is replaced.</summary>
            TEMPORARY_ALLIANCE = 1189,
            ///<summary>This temporary alliance of the Castle Attacker team has been dissolved.</summary>
            TEMPORARY_ALLIANCE_DISSOLVED = 1189,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 10 minutes.</summary>
            FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_10_MINUTES = 1191,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 5 minutes.</summary>
            FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_5_MINUTES = 1192,
            ///<summary>The ferry from Gludin Harbor will be arriving at Talking Island in approximately 1 minute.</summary>
            FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_1_MINUTE = 1193,
            ///<summary>A mercenary can be assigned to a position from the beginning of the Seal Validatio period until the time when a siege starts.</summary>
            MERC_CAN_BE_ASSIGNED = 1194,
            ///<summary>This mercenary cannot be assigned to a position by using the Seal of Strife.</summary>
            MERC_CANT_BE_ASSIGNED_USING_STRIFE = 1195,
            ///<summary>Your force has reached maximum capacity.</summary>
            FORCE_MAXIMUM = 1196,
            ///<summary>Summoning a servitor costs $s2 $s1.</summary>
            SUMMONING_SERVITOR_COSTS_S2_S1 = 1197,
            ///<summary>The item has been successfully crystallized.</summary>
            CRYSTALLIZATION_SUCCESSFUL = 1198,
            ///<summary>=======<Clan War Target>=======</summary>
            CLAN_WAR_HEADER = 1199,
            /// Message:($s1 ($s2 Alliance)</summary>
            S1_S2_ALLIANCE = 1200,
            ///<summary>Please select the quest you wish to abort.</summary>
            SELECT_QUEST_TO_ABOR = 1201,
            /// Message:($s1 (No alliance exists)</summary>
            S1_NO_ALLI_EXISTS = 1202,
            ///<summary>There is no clan war in progress.</summary>
            NO_WAR_IN_PROGRESS = 1203,
            ///<summary>The screenshot has been saved. ($s1 $s2x$s3)</summary>
            SCREENSHOT = 1204,
            ///<summary>Your mailbox is full. There is a 100 message limit.</summary>
            MAILBOX_FULL = 1205,
            ///<summary>The memo box is full. There is a 100 memo limit.</summary>
            MEMOBOX_FULL = 1206,
            ///<summary>Please make an entry in the field.</summary>
            MAKE_AN_ENTRY = 1207,
            ///<summary>$s1 died and dropped $s3 $s2.</summary>
            S1_DIED_DROPPED_S3_S2 = 1208,
            ///<summary>Congratulations. Your raid was successful.</summary>
            RAID_WAS_SUCCESSFUL = 1209,
            ///<summary>Seven Signs: The quest event period has begun. Visit a Priest of Dawn or Priestess of Dusk to participate in the event.</summary>
            QUEST_EVENT_PERIOD_BEGUN = 1210,
            ///<summary>Seven Signs: The quest event period has ended. The next quest event will start in one week.</summary>
            QUEST_EVENT_PERIOD_ENDED = 1211,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Avarice.</summary>
            DAWN_OBTAINED_AVARICE = 1212,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Gnosis.</summary>
            DAWN_OBTAINED_GNOSIS = 1213,
            ///<summary>Seven Signs: The Lords of Dawn have obtained the Seal of Strife.</summary>
            DAWN_OBTAINED_STRIFE = 1214,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Avarice.</summary>
            DUSK_OBTAINED_AVARICE = 1215,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Gnosis.</summary>
            DUSK_OBTAINED_GNOSIS = 1216,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Strife.</summary>
            DUSK_OBTAINED_STRIFE = 1217,
            ///<summary>Seven Signs: The Seal Validation period has begun.</summary>
            SEAL_VALIDATION_PERIOD_BEGUN = 1218,
            ///<summary>Seven Signs: The Seal Validation period has ended.</summary>
            SEAL_VALIDATION_PERIOD_ENDED = 1219,
            ///<summary>Are you sure you wish to summon it?</summary>
            SUMMON_CONFIRM = 1220,
            ///<summary>Are you sure you wish to return it?</summary>
            RETURN_CONFIRM = 1221,
            ///<summary>Current location : $s1, $s2, $s3 (GM Consultation Service)</summary>
            LOC_GM_CONSULATION_SERVICE_S1_S2_S3 = 1222,
            ///<summary>We depart for Talking Island in five minutes.</summary>
            DEPART_FOR_TALKING_5_MINUTES = 1223,
            ///<summary>We depart for Talking Island in one minute.</summary>
            DEPART_FOR_TALKING_1_MINUTE = 1224,
            ///<summary>All aboard for Talking Island</summary>
            DEPART_FOR_TALKING = 1225,
            ///<summary>We are now leaving for Talking Island.</summary>
            LEAVING_FOR_TALKING = 1226,
            ///<summary>You have $s1 unread messages.</summary>
            S1_UNREAD_MESSAGES = 1227,
            ///<summary>$s1 has blocked you. You cannot send mail to $s1.</summary>
            S1_BLOCKED_YOU_CANNOT_MAIL = 1228,
            ///<summary>No more messages may be sent at this time. Each account is allowed 10 messages per day.</summary>
            NO_MORE_MESSAGES_TODAY = 1229,
            ///<summary>You are limited to five recipients at a time.</summary>
            ONLY_FIVE_RECIPIENTS = 1230,
            ///<summary>You've sent mail.</summary>
            SENT_MAIL = 1231,
            ///<summary>The message was not sent.</summary>
            MESSAGE_NOT_SENT = 1232,
            ///<summary>You've got mail.</summary>
            NEW_MAIL = 1233,
            ///<summary>The mail has been stored in your temporary mailbox.</summary>
            MAIL_STORED_IN_MAILBOX = 1234,
            ///<summary>Do you wish to delete all your friends?</summary>
            ALL_FRIENDS_DELETE_CONFIRM = 1235,
            ///<summary>Please enter security card number.</summary>
            ENTER_SECURITY_CARD_NUMBER = 1236,
            ///<summary>Please enter the card number for number $s1.</summary>
            ENTER_CARD_NUMBER_FOR_S1 = 1237,
            ///<summary>Your temporary mailbox is full. No more mail can be stored; you have reached the 10 message limit.</summary>
            TEMP_MAILBOX_FULL = 1238,
            ///<summary>The keyboard security module has failed to load. Please exit the game and try again.</summary>
            KEYBOARD_MODULE_FAILED_LOAD = 1239,
            ///<summary>Seven Signs: The Revolutionaries of Dusk have won.</summary>
            DUSK_WON = 1240,
            ///<summary>Seven Signs: The Lords of Dawn have won.</summary>
            DAWN_WON = 1241,
            ///<summary>Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m.</summary>
            NOT_VERIFIED_AGE_NO_LOGIN = 1242,
            ///<summary>The security card number is invalid.</summary>
            SECURITY_CARD_NUMBER_INVALID = 1243,
            ///<summary>Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m. Logging off now</summary>
            NOT_VERIFIED_AGE_LOG_OFF = 1244,
            ///<summary>You will be loged out in $s1 minutes.</summary>
            LOGOUT_IN_S1_MINUTES = 1245,
            ///<summary>$s1 died and has dropped $s2 adena.</summary>
            S1_DIED_DROPPED_S2_ADENA = 1246,
            ///<summary>The corpse is too old. The skill cannot be used.</summary>
            CORPSE_TOO_OLD_SKILL_NOT_USED = 1247,
            ///<summary>You are out of feed. Mount status canceled.</summary>
            OUT_OF_FEED_MOUNT_CANCELED = 1248,
            ///<summary>You may only ride a wyvern while you're riding a strider.</summary>
            YOU_MAY_ONLY_RIDE_WYVERN_WHILE_RIDING_STRIDER = 1249,
            ///<summary>Do you really want to surrender? If you surrender during an alliance war, your Exp will drop the same as if you were to die once.</summary>
            SURRENDER_ALLY_WAR_CONFIRM = 1250,
            /// you will not be able to accept another clan to your alliance for one day.</summary>
            DISMISS_ALLY_CONFIRM = 1251,
            ///<summary>Are you sure you want to surrender? Exp penalty will be the same as death.</summary>
            SURRENDER_CONFIRM1 = 1252,
            ///<summary>Are you sure you want to surrender? Exp penalty will be the same as death and you will not be allowed to participate in clan war.</summary>
            SURRENDER_CONFIRM2 = 1253,
            ///<summary>Thank you for submitting feedback.</summary>
            THANKS_FOR_FEEDBACK = 1254,
            ///<summary>GM consultation has begun.</summary>
            GM_CONSULTATION_BEGUN = 1255,
            ///<summary>Please write the name after the command.</summary>
            PLEASE_WRITE_NAME_AFTER_COMMAND = 1256,
            ///<summary>The special skill of a servitor or pet cannot be registerd as a macro.</summary>
            PET_SKILL_NOT_AS_MACRO = 1257,
            ///<summary>$s1 has been crystallized</summary>
            S1_CRYSTALLIZED = 1258,
            ///<summary>=======<Alliance Target>=======</summary>
            ALLIANCE_TARGET_HEADER = 1259,
            ///<summary>Seven Signs: Preparations have begun for the next quest event.</summary>
            PREPARATIONS_PERIOD_BEGUN = 1260,
            ///<summary>Seven Signs: The quest event period has begun. Speak with a Priest of Dawn or Dusk Priestess if you wish to participate in the event.</summary>
            COMPETITION_PERIOD_BEGUN = 1261,
            ///<summary>Seven Signs: Quest event has ended. Results are being tallied.</summary>
            RESULTS_PERIOD_BEGUN = 1262,
            ///<summary>Seven Signs: This is the seal validation period. A new quest event period begins next Monday.</summary>
            VALIDATION_PERIOD_BEGUN = 1263,
            ///<summary>This soul stone cannot currently absorb souls. Absorption has failed.</summary>
            STONE_CANNOT_ABSORB = 1264,
            ///<summary>You can't absorb souls without a soul stone.</summary>
            CANT_ABSORB_WITHOUT_STONE = 1265,
            ///<summary>The exchange has ended.</summary>
            EXCHANGE_HAS_ENDED = 1266,
            ///<summary>Your contribution score is increased by $s1.</summary>
            CONTRIB_SCORE_INCREASED_S1 = 1267,
            ///<summary>Do you wish to add class as your sub class?</summary>
            ADD_SUBCLASS_CONFIRM = 1268,
            ///<summary>The new sub class has been added.</summary>
            ADD_NEW_SUBCLASS = 1269,
            ///<summary>The transfer of sub class has been completed.</summary>
            SUBCLASS_TRANSFER_COMPLETED = 1270,
            ///<summary>Do you wish to participate? Until the next seal validation period, you are a member of the Lords of Dawn.</summary>
            DAWN_CONFIRM = 1271,
            ///<summary>Do you wish to participate? Until the next seal validation period, you are a member of the Revolutionaries of Dusk.</summary>
            DUSK_CONFIRM = 1271,
            ///<summary>You will participate in the Seven Signs as a member of the Lords of Dawn.</summary>
            SEVENSIGNS_PARTECIPATION_DAWN = 1273,
            ///<summary>You will participate in the Seven Signs as a member of the Revolutionaries of Dusk.</summary>
            SEVENSIGNS_PARTECIPATION_DUSK = 1274,
            ///<summary>You've chosen to fight for the Seal of Avarice during this quest event period.</summary>
            FIGHT_FOR_AVARICE = 1275,
            ///<summary>You've chosen to fight for the Seal of Gnosis during this quest event period.</summary>
            FIGHT_FOR_GNOSIS = 1276,
            ///<summary>You've chosen to fight for the Seal of Strife during this quest event period.</summary>
            FIGHT_FOR_STRIFE = 1277,
            ///<summary>The NPC server is not operating at this time.</summary>
            NPC_SERVER_NOT_OPERATING = 1278,
            ///<summary>Contribution level has exceeded the limit. You may not continue.</summary>
            CONTRIB_SCORE_EXCEEDED = 1279,
            ///<summary>Magic Critical Hit!</summary>
            CRITICAL_HIT_MAGIC = 1280,
            ///<summary>Your excellent shield defense was a success!</summary>
            YOUR_EXCELLENT_SHIELD_DEFENSE_WAS_A_SUCCESS = 1281,
            ///<summary>Your Karma has been changed to $s1</summary>
            YOUR_KARMA_HAS_BEEN_CHANGED_TO_S1 = 1282,
            ///<summary>The minimum frame option has been activated.</summary>
            MINIMUM_FRAME_ACTIVATED = 1283,
            ///<summary>The minimum frame option has been deactivated.</summary>
            MINIMUM_FRAME_DEACTIVATED = 1284,
            ///<summary>No inventory exists: You cannot purchase an item.</summary>
            NO_INVENTORY_CANNOT_PURCHASE = 1285,
            ///<summary>(Until next Monday at 6:00 p.m.)</summary>
            UNTIL_MONDAY_6PM = 1286,
            ///<summary>(Until today at 6:00 p.m.)</summary>
            UNTIL_TODAY_6PM = 1287,
            ///<summary>If trends continue, $s1 will win and the seal will belong to:</summary>
            S1_WILL_WIN_COMPETITION = 1288,
            ///<summary>(Until next Monday at 6:00 p.m.)</summary>
            SEAL_OWNED_10_MORE_VOTED = 1289,
            ///<summary>Although the seal was not owned, since 35 percent or more people have voted.</summary>
            SEAL_NOT_OWNED_35_MORE_VOTED = 1290,
            /// because less than 10 percent of people have voted.</summary>
            SEAL_OWNED_10_LESS_VOTED = 1291,
            /// and since less than 35 percent of people have voted.</summary>
            SEAL_NOT_OWNED_35_LESS_VOTED = 1292,
            ///<summary>If current trends continue, it will end in a tie.</summary>
            COMPETITION_WILL_TIE = 1293,
            ///<summary>The competition has ended in a tie. Therefore, nobody has been awarded the seal.</summary>
            COMPETITION_TIE_SEAL_NOT_AWARDED = 1294,
            ///<summary>Sub classes may not be created or changed while a skill is in use.</summary>
            SUBCLASS_NO_CHANGE_OR_CREATE_WHILE_SKILL_IN_USE = 1295,
            ///<summary>You cannot open a Private Store here.</summary>
            NO_PRIVATE_STORE_HERE = 1296,
            ///<summary>You cannot open a Private Workshop here.</summary>
            NO_PRIVATE_WORKSHOP_HERE = 1297,
            ///<summary>Please confirm that you would like to exit the Monster Race Track.</summary>
            MONS_EXIT_CONFIRM = 1298,
            ///<summary>$s1's casting has been interrupted.</summary>
            S1_CASTING_INTERRUPTED = 1299,
            ///<summary>You are no longer trying on equipment.</summary>
            WEAR_ITEMS_STOPPED = 1300,
            ///<summary>Only a Lord of Dawn may use this.</summary>
            CAN_BE_USED_BY_DAWN = 1301,
            ///<summary>Only a Revolutionary of Dusk may use this.</summary>
            CAN_BE_USED_BY_DUSK = 1302,
            ///<summary>This may only be used during the quest event period.</summary>
            CAN_BE_USED_DURING_QUEST_EVENT_PERIOD = 1303,
            /// except for an Alliance with a castle owning clan.</summary>
            STRIFE_CANCELED_DEFENSIVE_REGISTRATION = 1304,
            ///<summary>Seal Stones may only be transferred during the quest event period.</summary>
            SEAL_STONES_ONLY_WHILE_QUEST = 1305,
            ///<summary>You are no longer trying on equipment.</summary>
            NO_LONGER_TRYING_ON = 1306,
            ///<summary>Only during the seal validation period may you settle your account.</summary>
            SETTLE_ACCOUNT_ONLY_IN_SEAL_VALIDATION = 1307,
            ///<summary>Congratulations - You've completed a class transfer!</summary>
            CLASS_TRANSFER = 1308,
            /// Message:To use this option, you must have the lastest version of MSN Messenger installed on your computer.</summary>
            LATEST_MSN_REQUIRED = 1309,
            ///<summary>For full functionality, the latest version of MSN Messenger must be installed on your computer.</summary>
            LATEST_MSN_RECOMMENDED = 1310,
            ///<summary>Previous versions of MSN Messenger only provide the basic features for in-game MSN Messenger Chat. Add/Delete Contacts and other MSN Messenger options are not available</summary>
            MSN_ONLY_BASIC = 1311,
            ///<summary>The latest version of MSN Messenger may be obtained from the MSN web site (http://messenger.msn.com).</summary>
            MSN_OBTAINED_FROM = 1312,
            ///<summary>$s1, to better serve our customers, all chat histories [...]</summary>
            S1_CHAT_HISTORIES_STORED = 1313,
            ///<summary>Please enter the passport ID of the person you wish to add to your contact list.</summary>
            ENTER_PASSPORT_FOR_ADDING = 1314,
            ///<summary>Deleting a contact will remove that contact from MSN Messenger as well. The contact can still check your online status and well not be blocked from sending you a message.</summary>
            DELETING_A_CONTACT = 1315,
            ///<summary>The contact will be deleted and blocked from your contact list.</summary>
            CONTACT_WILL_DELETED = 1316,
            ///<summary>Would you like to delete this contact?</summary>
            CONTACT_DELETE_CONFIRM = 1317,
            ///<summary>Please select the contact you want to block or unblock.</summary>
            SELECT_CONTACT_FOR_BLOCK_UNBLOCK = 1318,
            ///<summary>Please select the name of the contact you wish to change to another group.</summary>
            SELECT_CONTACT_FOR_CHANGE_GROUP = 1319,
            ///<summary>After selecting the group you wish to move your contact to, press the OK button.</summary>
            SELECT_GROUP_PRESS_OK = 1320,
            ///<summary>Enter the name of the group you wish to add.</summary>
            ENTER_GROUP_NAME = 1321,
            ///<summary>Select the group and enter the new name.</summary>
            SELECT_GROUP_ENTER_NAME = 1322,
            ///<summary>Select the group you wish to delete and click the OK button.</summary>
            SELECT_GROUP_TO_DELETE = 1323,
            ///<summary>Signing in...</summary>
            SIGNING_IN = 1324,
            ///<summary>You've logged into another computer and have been logged out of the .NET Messenger Service on this computer.</summary>
            ANOTHER_COMPUTER_LOGOUT = 1325,
            ///<summary>$s1 :</summary>
            S1_D = 1326,
            ///<summary>The following message could not be delivered:</summary>
            MESSAGE_NOT_DELIVERED = 1327,
            ///<summary>Members of the Revolutionaries of Dusk will not be resurrected.</summary>
            DUSK_NOT_RESURRECTED = 1328,
            ///<summary>You are currently blocked from using the Private Store and Private Workshop.</summary>
            BLOCKED_FROM_USING_STORE = 1329,
            ///<summary>You may not open a Private Store or Private Workshop for another $s1 minute(s)</summary>
            NO_STORE_FOR_S1_MINUTES = 1330,
            ///<summary>You are no longer blocked from using the Private Store and Private Workshop</summary>
            NO_LONGER_BLOCKED_USING_STORE = 1331,
            ///<summary>Items may not be used after your character or pet dies.</summary>
            NO_ITEMS_AFTER_DEATH = 1332,
            ///<summary>The replay file is not accessible. Please verify that the replay.ini exists in your Linage 2 directory.</summary>
            REPLAY_INACCESSIBLE = 1333,
            ///<summary>The new camera data has been stored.</summary>
            NEW_CAMERA_STORED = 1334,
            ///<summary>The attempt to store the new camera data has failed.</summary>
            CAMERA_STORING_FAILED = 1335,
            ///<summary>The replay file, $s1.$$s2 has been corrupted, please check the fle.</summary>
            REPLAY_S1_S2_CORRUPTED = 1336,
            ///<summary>This will terminate the replay. Do you wish to continue?</summary>
            REPLAY_TERMINATE_CONFIRM = 1337,
            ///<summary>You have exceeded the maximum amount that may be transferred at one time.</summary>
            EXCEEDED_MAXIMUM_AMOUNT = 1338,
            ///<summary>Once a macro is assigned to a shortcut, it cannot be run as a macro again.</summary>
            MACRO_SHORTCUT_NOT_RUN = 1339,
            ///<summary>This server cannot be accessed by the coupon you are using.</summary>
            SERVER_NOT_ACCESSED_BY_COUPON = 1340,
            ///<summary>Incorrect name and/or email address.</summary>
            INCORRECT_NAME_OR_ADDRESS = 1341,
            ///<summary>You are already logged in.</summary>
            ALREADY_LOGGED_IN = 1342,
            ///<summary>Incorrect email address and/or password. Your attempt to log into .NET Messenger Service has failed.</summary>
            INCORRECT_ADDRESS_OR_PASSWORD = 1343,
            ///<summary>Your request to log into the .NET Messenger service has failed. Please verify that you are currently connected to the internet.</summary>
            NET_LOGIN_FAILED = 1344,
            ///<summary>Click the OK button after you have selected a contact name.</summary>
            SELECT_CONTACT_CLICK_OK = 1345,
            ///<summary>You are currently entering a chat message.</summary>
            CURRENTLY_ENTERING_CHAT = 1346,
            ///<summary>The Linage II messenger could not carry out the task you requested.</summary>
            MESSENGER_FAILED_CARRYING_OUT_TASK = 1347,
            ///<summary>$s1 has entered the chat room.</summary>
            S1_ENTERED_CHAT_ROOM = 1348,
            ///<summary>$s1 has left the chat room.</summary>
            S1_LEFT_CHAT_ROOM = 1349,
            ///<summary>The state will be changed to indicate "off-line." All the chat windows currently opened will be closed.</summary>
            GOING_OFFLINE = 1350,
            ///<summary>Click the Delete button after selecting the contact you wish to remove.</summary>
            SELECT_CONTACT_CLICK_REMOVE = 1351,
            ///<summary>You have been added to $s1 ($s2)'s contact list.</summary>
            ADDED_TO_S1_S2_CONTACT_LIST = 1352,
            ///<summary>You can set the option to show your status as always being off-line to all of your contacts.</summary>
            CAN_SET_OPTION_TO_ALWAYS_SHOW_OFFLINE = 1353,
            ///<summary>You are not allowed to chat with a contact while chatting block is imposed.</summary>
            NO_CHAT_WHILE_BLOCKED = 1354,
            ///<summary>The contact is currently blocked from chatting.</summary>
            CONTACT_CURRENTLY_BLOCKED = 1355,
            ///<summary>The contact is not currently logged in.</summary>
            CONTACT_CURRENTLY_OFFLINE = 1356,
            ///<summary>You have been blocked from chatting with that contact.</summary>
            YOU_ARE_BLOCKED = 1357,
            ///<summary>You are being logged out...</summary>
            YOU_ARE_LOGGING_OUT = 1358,
            ///<summary>$s1 has logged in.</summary>
            S1_LOGGED_IN2 = 1359,
            ///<summary>You have received a message from $s1.</summary>
            GOT_MESSAGE_FROM_S1 = 1360,
            ///<summary>Due to a system error, you have been logged out of the .NET Messenger Service.</summary>
            LOGGED_OUT_DUE_TO_ERROR = 1361,
            /// click the button next to My Status and then use the Options menu.</summary>
            SELECT_CONTACT_TO_DELETE = 1362,
            ///<summary>Your request to participate in the alliance war has been denied.</summary>
            YOUR_REQUEST_ALLIANCE_WAR_DENIED = 1363,
            ///<summary>The request for an alliance war has been rejected.</summary>
            REQUEST_ALLIANCE_WAR_REJECTED = 1364,
            ///<summary>$s2 of $s1 clan has surrendered as an individual.</summary>
            S2_OF_S1_SURRENDERED_AS_INDIVIDUAL = 1365,
            ///<summary>In order to delete a group, you must not [...]</summary>
            DELTE_GROUP_INSTRUCTION = 1366,
            ///<summary>Only members of the group are allowed to add records.</summary>
            ONLY_GROUP_CAN_ADD_RECORDS = 1367,
            ///<summary>You can not try those items on at the same time.</summary>
            YOU_CAN_NOT_TRY_THOSE_ITEMS_ON_AT_THE_SAME_TIME = 1368,
            ///<summary>You've exceeded the maximum.</summary>
            EXCEEDED_THE_MAXIMUM = 1369,
            ///<summary>Your message to $s1 did not reach its recipient. You cannot send mail to the GM staff.</summary>
            CANNOT_MAIL_GM_S1 = 1370,
            ///<summary>It has been determined that you're not engaged in normal gameplay and a restriction has been imposed upon you. You may not move for $s1 minutes.</summary>
            GAMEPLAY_RESTRICTION_PENALTY_S1 = 1371,
            ///<summary>Your punishment will continue for $s1 minutes.</summary>
            PUNISHMENT_CONTINUE_S1_MINUTES = 1372,
            ///<summary>$s1 has picked up $s2 that was dropped by a Raid Boss.</summary>
            S1_OBTAINED_S2_FROM_RAIDBOSS = 1373,
            ///<summary>$s1 has picked up $s3 $s2(s) that was dropped by a Raid Boss.</summary>
            S1_PICKED_UP_S3_S2_S_FROM_RAIDBOSS = 1374,
            ///<summary>$s1 has picked up $s2 adena that was dropped by a Raid Boss.</summary>
            S1_OBTAINED_S2_ADENA_FROM_RAIDBOSS = 1375,
            ///<summary>$s1 has picked up $s2 that was dropped by another character.</summary>
            S1_OBTAINED_S2_FROM_ANOTHER_CHARACTER = 1376,
            ///<summary>$s1 has picked up $s3 $s2(s) that was dropped by a another character.</summary>
            S1_PICKED_UP_S3_S2_S_FROM_ANOTHER_CHARACTER = 1377,
            ///<summary>$s1 has picked up +$s3 $s2 that was dropped by a another character.</summary>
            S1_PICKED_UP_S3_S2_FROM_ANOTHER_CHARACTER = 1378,
            ///<summary>$s1 has obtained $s2 adena.</summary>
            S1_OBTAINED_S2_ADENA = 1379,
            ///<summary>You can't summon a $s1 while on the battleground.</summary>
            CANT_SUMMON_S1_ON_BATTLEGROUND = 1380,
            ///<summary>The party leader has obtained $s2 of $s1.</summary>
            LEADER_OBTAINED_S2_OF_S1 = 1381,
            ///<summary>To fulfill the quest, you must bring the chosen weapon. Are you sure you want to choose this weapon?</summary>
            CHOOSE_WEAPON_CONFIRM = 1382,
            ///<summary>Are you sure you want to exchange?</summary>
            EXCHANGE_CONFIRM = 1383,
            ///<summary>$s1 has become the party leader.</summary>
            S1_HAS_BECOME_A_PARTY_LEADER = 1384,
            ///<summary>You are not allowed to dismount at this location.</summary>
            NO_DISMOUNT_HERE = 1385,
            ///<summary>You are no longer held in place.</summary>
            NO_LONGER_HELD_IN_PLACE = 1386,
            ///<summary>Please select the item you would like to try on.</summary>
            SELECT_ITEM_TO_TRY_ON = 1387,
            ///<summary>A party room has been created.</summary>
            PARTY_ROOM_CREATED = 1388,
            ///<summary>The party room's information has been revised.</summary>
            PARTY_ROOM_REVISED = 1389,
            ///<summary>You are not allowed to enter the party room.</summary>
            PARTY_ROOM_FORBIDDEN = 1390,
            ///<summary>You have exited from the party room.</summary>
            PARTY_ROOM_EXITED = 1391,
            ///<summary>$s1 has left the party room.</summary>
            S1_LEFT_PARTY_ROOM = 1392,
            ///<summary>You have been ousted from the party room.</summary>
            OUSTED_FROM_PARTY_ROOM = 1393,
            ///<summary>$s1 has been kicked from the party room.</summary>
            S1_KICKED_FROM_PARTY_ROOM = 1394,
            ///<summary>The party room has been disbanded.</summary>
            PARTY_ROOM_DISBANDED = 1395,
            ///<summary>The list of party rooms can only be viewed by a person who has not joined a party or who is currently the leader of a party.</summary>
            CANT_VIEW_PARTY_ROOMS = 1396,
            ///<summary>The leader of the party room has changed.</summary>
            PARTY_ROOM_LEADER_CHANGED = 1397,
            ///<summary>We are recruiting party members.</summary>
            RECRUITING_PARTY_MEMBERS = 1398,
            ///<summary>Only the leader of the party can transfer party leadership to another player.</summary>
            ONLY_A_PARTY_LEADER_CAN_TRANSFER_ONES_RIGHTS_TO_ANOTHER_PLAYER = 1399,
            ///<summary>Please select the person you wish to make the party leader.</summary>
            PLEASE_SELECT_THE_PERSON_TO_WHOM_YOU_WOULD_LIKE_TO_TRANSFER_THE_RIGHTS_OF_A_PARTY_LEADER = 1400,
            ///<summary>Slow down.you are already the party leader.</summary>
            YOU_CANNOT_TRANSFER_RIGHTS_TO_YOURSELF = 1401,
            ///<summary>You may only transfer party leadership to another member of the party.</summary>
            YOU_CAN_TRANSFER_RIGHTS_ONLY_TO_ANOTHER_PARTY_MEMBER = 1402,
            ///<summary>You have failed to transfer the party leadership.</summary>
            YOU_HAVE_FAILED_TO_TRANSFER_THE_PARTY_LEADER_RIGHTS = 1403,
            ///<summary>The owner of the private manufacturing store has changed the price for creating this item. Please check the new price before trying again.</summary>
            MANUFACTURE_PRICE_HAS_CHANGED = 1404,
            ///<summary>$s1 CPs have been restored.</summary>
            S1_CP_WILL_BE_RESTORED = 1405,
            ///<summary>$s2 CPs has been restored by $s1.</summary>
            S2_CP_WILL_BE_RESTORED_BY_S1 = 1406,
            ///<summary>You are using a computer that does not allow you to log in with two accounts at the same time.</summary>
            NO_LOGIN_WITH_TWO_ACCOUNTS = 1407,
            ///<summary>Your prepaid remaining usage time is $s1 hours and $s2 minutes. You have $s3 paid reservations left.</summary>
            PREPAID_LEFT_S1_S2_S3 = 1408,
            ///<summary>Your prepaid usage time has expired. Your new prepaid reservation will be used. The remaining usage time is $s1 hours and $s2 minutes.</summary>
            PREPAID_EXPIRED_S1_S2 = 1409,
            ///<summary>Your prepaid usage time has expired. You do not have any more prepaid reservations left.</summary>
            PREPAID_EXPIRED = 1410,
            ///<summary>The number of your prepaid reservations has changed.</summary>
            PREPAID_CHANGED = 1411,
            ///<summary>Your prepaid usage time has $s1 minutes left.</summary>
            PREPAID_LEFT_S1 = 1412,
            ///<summary>You do not meet the requirements to enter that party room.</summary>
            CANT_ENTER_PARTY_ROOM = 1413,
            ///<summary>The width and length should be 100 or more grids and less than 5000 grids respectively.</summary>
            WRONG_GRID_COUNT = 1414,
            ///<summary>The command file is not sent.</summary>
            COMMAND_FILE_NOT_SENT = 1415,
            ///<summary>The representative of Team 1 has not been selected.</summary>
            TEAM_1_NO_REPRESENTATIVE = 1416,
            ///<summary>The representative of Team 2 has not been selected.</summary>
            TEAM_2_NO_REPRESENTATIVE = 1417,
            ///<summary>The name of Team 1 has not yet been chosen.</summary>
            TEAM_1_NO_NAME = 1418,
            ///<summary>The name of Team 2 has not yet been chosen.</summary>
            TEAM_2_NO_NAME = 1419,
            ///<summary>The name of Team 1 and the name of Team 2 are identical.</summary>
            TEAM_NAME_IDENTICAL = 1420,
            ///<summary>The race setup file has not been designated.</summary>
            RACE_SETUP_FILE1 = 1421,
            ///<summary>Race setup file error - BuffCnt is not specified</summary>
            RACE_SETUP_FILE2 = 1422,
            ///<summary>Race setup file error - BuffID$s1 is not specified.</summary>
            RACE_SETUP_FILE3 = 1423,
            ///<summary>Race setup file error - BuffLv$s1 is not specified.</summary>
            RACE_SETUP_FILE4 = 1424,
            ///<summary>Race setup file error - DefaultAllow is not specified</summary>
            RACE_SETUP_FILE5 = 1425,
            ///<summary>Race setup file error - ExpSkillCnt is not specified.</summary>
            RACE_SETUP_FILE6 = 1426,
            ///<summary>Race setup file error - ExpSkillID$s1 is not specified.</summary>
            RACE_SETUP_FILE7 = 1427,
            ///<summary>Race setup file error - ExpItemCnt is not specified.</summary>
            RACE_SETUP_FILE8 = 1428,
            ///<summary>Race setup file error - ExpItemID$s1 is not specified.</summary>
            RACE_SETUP_FILE9 = 1429,
            ///<summary>Race setup file error - TeleportDelay is not specified</summary>
            RACE_SETUP_FILE10 = 1430,
            ///<summary>The race will be stopped temporarily.</summary>
            RACE_STOPPED_TEMPORARILY = 1431,
            ///<summary>Your opponent is currently in a petrified state.</summary>
            OPPONENT_PETRIFIED = 1432,
            ///<summary>You will now automatically apply $s1 to your target.</summary>
            USE_OF_S1_WILL_BE_AUTO = 1433,
            ///<summary>You will no longer automatically apply $s1 to your weapon.</summary>
            AUTO_USE_OF_S1_CANCELLED = 1434,
            ///<summary>Due to insufficient $s1, the automatic use function has been deactivated.</summary>
            AUTO_USE_CANCELLED_LACK_OF_S1 = 1435,
            ///<summary>Due to insufficient $s1, the automatic use function cannot be activated.</summary>
            CANNOT_AUTO_USE_LACK_OF_S1 = 1436,
            ///<summary>Players are no longer allowed to play dice. Dice can no longer be purchased from a village store. However, you can still sell them to any village store.</summary>
            DICE_NO_LONGER_ALLOWED = 1437,
            ///<summary>There is no skill that enables enchant.</summary>
            THERE_IS_NO_SKILL_THAT_ENABLES_ENCHANT = 1438,
            ///<summary>You do not have all of the items needed to enchant that skill.</summary>
            YOU_DONT_HAVE_ALL_OF_THE_ITEMS_NEEDED_TO_ENCHANT_THAT_SKILL = 1439,
            ///<summary>You have succeeded in enchanting the skill $s1.</summary>
            YOU_HAVE_SUCCEEDED_IN_ENCHANTING_THE_SKILL_S1 = 1440,
            ///<summary>Skill enchant failed. The skill will be initialized.</summary>
            YOU_HAVE_FAILED_TO_ENCHANT_THE_SKILL_S1 = 1441,
            ///<summary>You do not have enough SP to enchant that skill.</summary>
            YOU_DONT_HAVE_ENOUGH_SP_TO_ENCHANT_THAT_SKILL = 1443,
            ///<summary>You do not have enough experience (Exp) to enchant that skill.</summary>
            YOU_DONT_HAVE_ENOUGH_EXP_TO_ENCHANT_THAT_SKILL = 1444,
            ///<summary>Your previous subclass will be removed and replaced with the new subclass at level 40. Do you wish to continue?</summary>
            REPLACE_SUBCLASS_CONFIRM = 1445,
            ///<summary>The ferry from $s1 to $s2 has been delayed.</summary>
            FERRY_FROM_S1_TO_S2_DELAYED = 1446,
            ///<summary>You cannot do that while fishing.</summary>
            CANNOT_DO_WHILE_FISHING_1 = 1447,
            ///<summary>Only fishing skills may be used at this time.</summary>
            ONLY_FISHING_SKILLS_NOW = 1448,
            ///<summary>You've got a bite!</summary>
            GOT_A_BITE = 1449,
            ///<summary>That fish is more determined than you are - it spit the hook!</summary>
            FISH_SPIT_THE_HOOK = 1450,
            ///<summary>Your bait was stolen by that fish!</summary>
            BAIT_STOLEN_BY_FISH = 1451,
            ///<summary>Baits have been lost because the fish got away.</summary>
            BAIT_LOST_FISH_GOT_AWAY = 1452,
            ///<summary>You do not have a fishing pole equipped.</summary>
            FISHING_POLE_NOT_EQUIPPED = 1453,
            ///<summary>You must put bait on your hook before you can fish.</summary>
            BAIT_ON_HOOK_BEFORE_FISHING = 1454,
            ///<summary>You cannot fish while under water.</summary>
            CANNOT_FISH_UNDER_WATER = 1455,
            ///<summary>You cannot fish while riding as a passenger of a boat - it's against the rules.</summary>
            CANNOT_FISH_ON_BOAT = 1456,
            ///<summary>You can't fish here.</summary>
            CANNOT_FISH_HERE = 1457,
            ///<summary>Your attempt at fishing has been cancelled.</summary>
            FISHING_ATTEMPT_CANCELLED = 1458,
            ///<summary>You do not have enough bait.</summary>
            NOT_ENOUGH_BAIT = 1459,
            ///<summary>You reel your line in and stop fishing.</summary>
            REEL_LINE_AND_STOP_FISHING = 1460,
            ///<summary>You cast your line and start to fish.</summary>
            CAST_LINE_AND_START_FISHING = 1461,
            ///<summary>You may only use the Pumping skill while you are fishing.</summary>
            CAN_USE_PUMPING_ONLY_WHILE_FISHING = 1462,
            ///<summary>You may only use the Reeling skill while you are fishing.</summary>
            CAN_USE_REELING_ONLY_WHILE_FISHING = 1463,
            ///<summary>The fish has resisted your attempt to bring it in.</summary>
            FISH_RESISTED_ATTEMPT_TO_BRING_IT_IN = 1464,
            ///<summary>Your pumping is successful, causing $s1 damage.</summary>
            PUMPING_SUCCESFUL_S1_DAMAGE = 1465,
            ///<summary>You failed to do anything with the fish and it regains $s1 HP.</summary>
            FISH_RESISTED_PUMPING_S1_HP_REGAINED = 1466,
            ///<summary>You reel that fish in closer and cause $s1 damage.</summary>
            REELING_SUCCESFUL_S1_DAMAGE = 1467,
            ///<summary>You failed to reel that fish in further and it regains $s1 HP.</summary>
            FISH_RESISTED_REELING_S1_HP_REGAINED = 1468,
            ///<summary>You caught something!</summary>
            YOU_CAUGHT_SOMETHING = 1469,
            ///<summary>You cannot do that while fishing.</summary>
            CANNOT_DO_WHILE_FISHING_2 = 1470,
            ///<summary>You cannot do that while fishing.</summary>
            CANNOT_DO_WHILE_FISHING_3 = 1471,
            ///<summary>You look oddly at the fishing pole in disbelief and realize that you can't attack anything with this.</summary>
            CANNOT_ATTACK_WITH_FISHING_POLE = 1472,
            ///<summary>$s1 is not sufficient.</summary>
            S1_NOT_SUFFICIENT = 1473,
            ///<summary>$s1 is not available.</summary>
            S1_NOT_AVAILABLE = 1474,
            ///<summary>Pet has dropped $s1.</summary>
            PET_DROPPED_S1 = 1475,
            ///<summary>Pet has dropped +$s1 $s2.</summary>
            PET_DROPPED_S1_S2 = 1476,
            ///<summary>Pet has dropped $s2 of $s1.</summary>
            PET_DROPPED_S2_S1_S = 1477,
            ///<summary>You may only register a 64 x 64 pixel, 256-color BMP.</summary>
            ONLY_64_PIXEL_256_COLOR_BMP = 1478,
            ///<summary>That is the wrong grade of soulshot for that fishing pole.</summary>
            WRONG_FISHINGSHOT_GRADE = 1479,
            ///<summary>Are you sure you want to remove yourself from the Grand Olympiad Games waiting list?</summary>
            OLYMPIAD_REMOVE_CONFIRM = 1480,
            ///<summary>You have selected a class irrelevant individual match. Do you wish to participate?</summary>
            OLYMPIAD_NON_CLASS_CONFIRM = 1481,
            ///<summary>You've selected to join a class specific game. Continue?</summary>
            OLYMPIAD_CLASS_CONFIRM = 1482,
            ///<summary>Are you ready to be a Hero?</summary>
            HERO_CONFIRM = 1483,
            ///<summary>Are you sure this is the Hero weapon you wish to use? Kamael race cannot use this.</summary>
            HERO_WEAPON_CONFIRM = 1484,
            ///<summary>The ferry from Talking Island to Gludin Harbor has been delayed.</summary>
            FERRY_TALKING_GLUDIN_DELAYED = 1485,
            ///<summary>The ferry from Gludin Harbor to Talking Island has been delayed.</summary>
            FERRY_GLUDIN_TALKING_DELAYED = 1486,
            ///<summary>The ferry from Giran Harbor to Talking Island has been delayed.</summary>
            FERRY_GIRAN_TALKING_DELAYED = 1487,
            ///<summary>The ferry from Talking Island to Giran Harbor has been delayed.</summary>
            FERRY_TALKING_GIRAN_DELAYED = 1488,
            ///<summary>Innadril cruise service has been delayed.</summary>
            INNADRIL_BOAT_DELAYED = 1489,
            ///<summary>Traded $s2 of crop $s1.</summary>
            TRADED_S2_OF_CROP_S1 = 1490,
            ///<summary>Failed in trading $s2 of crop $s1.</summary>
            FAILED_IN_TRADING_S2_OF_CROP_S1 = 1491,
            ///<summary>You will be moved to the Olympiad Stadium in $s1 second(s).</summary>
            YOU_WILL_ENTER_THE_OLYMPIAD_STADIUM_IN_S1_SECOND_S = 1492,
            ///<summary>Your opponent made haste with their tail between their legs, the match has been cancelled.</summary>
            THE_GAME_HAS_BEEN_CANCELLED_BECAUSE_THE_OTHER_PARTY_ENDS_THE_GAME = 1493,
            ///<summary>Your opponent does not meet the requirements to do battle, the match has been cancelled.</summary>
            THE_GAME_HAS_BEEN_CANCELLED_BECAUSE_THE_OTHER_PARTY_DOES_NOT_MEET_THE_REQUIREMENTS_FOR_JOINING_THE_GAME = 1494,
            ///<summary>The match will start in $s1 second(s).</summary>
            THE_GAME_WILL_START_IN_S1_SECOND_S = 1495,
            ///<summary>The match has started, fight!</summary>
            STARTS_THE_GAME = 1496,
            ///<summary>Congratulations, $s1! You win the match!</summary>
            S1_HAS_WON_THE_GAME = 1497,
            ///<summary>There is no victor, the match ends in a tie.</summary>
            THE_GAME_ENDED_IN_A_TIE = 1498,
            ///<summary>You will be moved back to town in $s1 second(s).</summary>
            YOU_WILL_BE_MOVED_TO_TOWN_IN_S1_SECONDS = 1499,
            ///<summary>You cannot participate in the Grand Olympiad Games with a character in their subclass.</summary>
            YOU_CANT_JOIN_THE_OLYMPIAD_WITH_A_SUB_JOB_CHARACTER = 1500,
            ///<summary>Only Noblesse can participate in the Olympiad.</summary>
            ONLY_NOBLESS_CAN_PARTICIPATE_IN_THE_OLYMPIAD = 1501,
            ///<summary>You have already been registered in a waiting list of an event.</summary>
            YOU_HAVE_ALREADY_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_AN_EVENT = 1502,
            ///<summary>You have been registered in the Grand Olympiad Games waiting list for a class specific match.</summary>
            YOU_HAVE_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_CLASSIFIED_GAMES = 1503,
            ///<summary>You have registered on the waiting list for the non-class-limited individual match event.</summary>
            YOU_HAVE_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_NO_CLASS_GAMES = 1504,
            ///<summary>You have been removed from the Grand Olympiad Games waiting list.</summary>
            YOU_HAVE_BEEN_DELETED_FROM_THE_WAITING_LIST_OF_A_GAME = 1505,
            ///<summary>You are not currently registered on any Grand Olympiad Games waiting list.</summary>
            YOU_HAVE_NOT_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_A_GAME = 1506,
            ///<summary>You cannot equip that item in a Grand Olympiad Games match.</summary>
            THIS_ITEM_CANT_BE_EQUIPPED_FOR_THE_OLYMPIAD_EVENT = 1507,
            ///<summary>You cannot use that item in a Grand Olympiad Games match.</summary>
            THIS_ITEM_IS_NOT_AVAILABLE_FOR_THE_OLYMPIAD_EVENT = 1508,
            ///<summary>You cannot use that skill in a Grand Olympiad Games match.</summary>
            THIS_SKILL_IS_NOT_AVAILABLE_FOR_THE_OLYMPIAD_EVENT = 1509,
            ///<summary>$s1 is making an attempt at resurrection. Do you want to continue with this resurrection?</summary>
            RESSURECTION_REQUEST_BY_S1 = 1510,
            ///<summary>While a pet is attempting to resurrect, it cannot help in resurrecting its master.</summary>
            MASTER_CANNOT_RES = 1511,
            ///<summary>You cannot resurrect a pet while their owner is being resurrected.</summary>
            CANNOT_RES_PET = 1512,
            ///<summary>Resurrection has already been proposed.</summary>
            RES_HAS_ALREADY_BEEN_PROPOSED = 1513,
            ///<summary>You cannot the owner of a pet while their pet is being resurrected</summary>
            CANNOT_RES_MASTER = 1514,
            ///<summary>A pet cannot be resurrected while it's owner is in the process of resurrecting.</summary>
            CANNOT_RES_PET2 = 1515,
            ///<summary>The target is unavailable for seeding.</summary>
            THE_TARGET_IS_UNAVAILABLE_FOR_SEEDING = 1516,
            ///<summary>Failed in Blessed Enchant. The enchant value of the item became 0.</summary>
            BLESSED_ENCHANT_FAILED = 1517,
            ///<summary>You do not meet the required condition to equip that item.</summary>
            CANNOT_EQUIP_ITEM_DUE_TO_BAD_CONDITION = 1518,
            ///<summary>Your pet has been killed! Make sure you resurrect your pet within 20 minutes or your pet and all of it's items will disappear forever!</summary>
            MAKE_SURE_YOU_RESSURECT_YOUR_PET_WITHIN_20_MINUTES = 1519,
            ///<summary>Servitor passed away.</summary>
            SERVITOR_PASSED_AWAY = 1520,
            ///<summary>Your servitor has vanished! You'll need to summon a new one.</summary>
            YOUR_SERVITOR_HAS_VANISHED = 1521,
            ///<summary>Your pet's corpse has decayed!</summary>
            YOUR_PETS_CORPSE_HAS_DECAYED = 1522,
            ///<summary>You should release your pet or servitor so that it does not fall off of the boat and drown!</summary>
            RELEASE_PET_ON_BOAT = 1523,
            ///<summary>$s1's pet gained $s2.</summary>
            S1_PET_GAINED_S2 = 1524,
            ///<summary>$s1's pet gained $s3 of $s2.</summary>
            S1_PET_GAINED_S3_S2_S = 1525,
            ///<summary>$s1's pet gained +$s2$s3.</summary>
            S1_PET_GAINED_S2_S3 = 1526,
            ///<summary>Your pet was hungry so it ate $s1.</summary>
            PET_TOOK_S1_BECAUSE_HE_WAS_HUNGRY = 1527,
            ///<summary>You've sent a petition to the GM staff.</summary>
            SENT_PETITION_TO_GM = 1528,
            ///<summary>$s1 is inviting you to the command channel. Do you want accept?</summary>
            COMMAND_CHANNEL_CONFIRM_FROM_S1 = 1529,
            ///<summary>Select a target or enter the name.</summary>
            SELECT_TARGET_OR_ENTER_NAME = 1530,
            ///<summary>Enter the name of the clan that you wish to declare war on.</summary>
            ENTER_CLAN_NAME_TO_DECLARE_WAR2 = 1531,
            ///<summary>Enter the name of the clan that you wish to have a cease-fire with.</summary>
            ENTER_CLAN_NAME_TO_CEASE_FIRE = 1532,
            ///<summary>Attention: $s1 has picked up $s2.</summary>
            ATTENTION_S1_PICKED_UP_S2 = 1533,
            ///<summary>Attention: $s1 has picked up +$s2$s3.</summary>
            ATTENTION_S1_PICKED_UP_S2_S3 = 1534,
            ///<summary>Attention: $s1's pet has picked up $s2.</summary>
            ATTENTION_S1_PET_PICKED_UP_S2 = 1535,
            ///<summary>Attention: $s1's pet has picked up +$s2$s3.</summary>
            ATTENTION_S1_PET_PICKED_UP_S2_S3 = 1536,
            ///<summary>Current Location: $s1, $s2, $s3 (near Rune Village)</summary>
            LOC_RUNE_S1_S2_S3 = 1537,
            ///<summary>Current Location: $s1, $s2, $s3 (near the Town of Goddard)</summary>
            LOC_GODDARD_S1_S2_S3 = 1538,
            ///<summary>Cargo has arrived at Talking Island Village.</summary>
            CARGO_AT_TALKING_VILLAGE = 1539,
            ///<summary>Cargo has arrived at the Dark Elf Village.</summary>
            CARGO_AT_DARKELF_VILLAGE = 1540,
            ///<summary>Cargo has arrived at Elven Village.</summary>
            CARGO_AT_ELVEN_VILLAGE = 1541,
            ///<summary>Cargo has arrived at Orc Village.</summary>
            CARGO_AT_ORC_VILLAGE = 1542,
            ///<summary>Cargo has arrived at Dwarfen Village.</summary>
            CARGO_AT_DWARVEN_VILLAGE = 1543,
            ///<summary>Cargo has arrived at Aden Castle Town.</summary>
            CARGO_AT_ADEN = 1544,
            ///<summary>Cargo has arrived at Town of Oren.</summary>
            CARGO_AT_OREN = 1545,
            ///<summary>Cargo has arrived at Hunters Village.</summary>
            CARGO_AT_HUNTERS = 1546,
            ///<summary>Cargo has arrived at the Town of Dion.</summary>
            CARGO_AT_DION = 1547,
            ///<summary>Cargo has arrived at Floran Village.</summary>
            CARGO_AT_FLORAN = 1548,
            ///<summary>Cargo has arrived at Gludin Village.</summary>
            CARGO_AT_GLUDIN = 1549,
            ///<summary>Cargo has arrived at the Town of Gludio.</summary>
            CARGO_AT_GLUDIO = 1550,
            ///<summary>Cargo has arrived at Giran Castle Town.</summary>
            CARGO_AT_GIRAN = 1551,
            ///<summary>Cargo has arrived at Heine.</summary>
            CARGO_AT_HEINE = 1552,
            ///<summary>Cargo has arrived at Rune Village.</summary>
            CARGO_AT_RUNE = 1553,
            ///<summary>Cargo has arrived at the Town of Goddard.</summary>
            CARGO_AT_GODDARD = 1554,
            ///<summary>Do you want to cancel character deletion?</summary>
            CANCEL_CHARACTER_DELETION_CONFIRM = 1555,
            ///<summary>Your clan notice has been saved.</summary>
            CLAN_NOTICE_SAVED = 1556,
            ///<summary>Seed price should be more than $s1 and less than $s2.</summary>
            SEED_PRICE_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = 1557,
            ///<summary>The quantity of seed should be more than $s1 and less than $s2.</summary>
            THE_QUANTITY_OF_SEED_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = 1558,
            ///<summary>Crop price should be more than $s1 and less than $s2.</summary>
            CROP_PRICE_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = 1559,
            ///<summary>The quantity of crop should be more than $s1 and less than $s2</summary>
            THE_QUANTITY_OF_CROP_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = 1560,
            ///<summary>The clan, $s1, has declared a Clan War.</summary>
            CLAN_S1_DECLARED_WAR = 1561,
            ///<summary>A Clan War has been declared against the clan, $s1. you will only lose a quarter of the normal experience from death.</summary>
            CLAN_WAR_DECLARED_AGAINST_S1_IF_KILLED_LOSE_LOW_EXP = 1562,
            ///<summary>The clan, $s1, cannot declare a Clan War because their clan is less than level three, and or they do not have enough members.</summary>
            S1_CLAN_CANNOT_DECLARE_WAR_TOO_LOW_LEVEL_OR_NOT_ENOUGH_MEMBERS = 1563,
            ///<summary>A Clan War can be declared only if the clan is level three or above, and the number of clan members is fifteen or greater.</summary>
            CLAN_WAR_DECLARED_IF_CLAN_LVL3_OR_15_MEMBER = 1564,
            ///<summary>A Clan War cannot be declared against a clan that does not exist!</summary>
            CLAN_WAR_CANNOT_DECLARED_CLAN_NOT_EXIST = 1565,
            ///<summary>The clan, $s1, has decided to stop the war.</summary>
            CLAN_S1_HAS_DECIDED_TO_STOP = 1566,
            ///<summary>The war against $s1 Clan has been stopped.</summary>
            WAR_AGAINST_S1_HAS_STOPPED = 1567,
            ///<summary>The target for declaration is wrong.</summary>
            WRONG_DECLARATION_TARGET = 1568,
            ///<summary>A declaration of Clan War against an allied clan can't be made.</summary>
            CLAN_WAR_AGAINST_A_ALLIED_CLAN_NOT_WORK = 1569,
            ///<summary>A declaration of war against more than 30 Clans can't be made at the same time</summary>
            TOO_MANY_CLAN_WARS = 1570,
            ///<summary>======<Clans You've Declared War On>======</summary>
            CLANS_YOU_DECLARED_WAR_ON = 1571,
            ///<summary>======<Clans That Have Declared War On You>======</summary>
            CLANS_THAT_HAVE_DECLARED_WAR_ON_YOU = 1572,
            ///<summary>There are no clans that your clan has declared war against.</summary>
            YOU_ARENT_IN_CLAN_WARS = 1573,
            ///<summary>All is well. There are no clans that have declared war against your clan.</summary>
            NO_CLAN_WARS_VS_YOU = 1574,
            ///<summary>Command Channels can only be formed by a party leader who is also the leader of a level 5 clan.</summary>
            COMMAND_CHANNEL_ONLY_BY_LEVEL_5_CLAN_LEADER_PARTY_LEADER = 1575,
            ///<summary>Pet uses the power of spirit.</summary>
            PET_USE_THE_POWER_OF_SPIRIT = 1576,
            ///<summary>Servitor uses the power of spirit.</summary>
            SERVITOR_USE_THE_POWER_OF_SPIRIT = 1577,
            ///<summary>Items are not available for a private store or a private manufacture.</summary>
            ITEMS_UNAVAILABLE_FOR_STORE_MANUFACTURE = 1578,
            ///<summary>$s1's pet gained $s2 adena.</summary>
            S1_PET_GAINED_S2_ADENA = 1579,
            ///<summary>The Command Channel has been formed.</summary>
            COMMAND_CHANNEL_FORMED = 1580,
            ///<summary>The Command Channel has been disbanded.</summary>
            COMMAND_CHANNEL_DISBANDED = 1581,
            ///<summary>You have joined the Command Channel.</summary>
            JOINED_COMMAND_CHANNEL = 1582,
            ///<summary>You were dismissed from the Command Channel.</summary>
            DISMISSED_FROM_COMMAND_CHANNEL = 1583,
            ///<summary>$s1's party has been dismissed from the Command Channel.</summary>
            S1_PARTY_DISMISSED_FROM_COMMAND_CHANNEL = 1584,
            ///<summary>The Command Channel has been disbanded.</summary>
            COMMAND_CHANNEL_DISBANDED2 = 1585,
            ///<summary>You have quit the Command Channel.</summary>
            LEFT_COMMAND_CHANNEL = 1586,
            ///<summary>$s1's party has left the Command Channel.</summary>
            S1_PARTY_LEFT_COMMAND_CHANNEL = 1587,
            ///<summary>The Command Channel is activated only when there are at least 5 parties participating.</summary>
            COMMAND_CHANNEL_ONLY_AT_LEAST_5_PARTIES = 1588,
            ///<summary>Command Channel authority has been transferred to $s1.</summary>
            COMMAND_CHANNEL_LEADER_NOW_S1 = 1589,
            ///<summary>===<Guild Info (Total Parties: $s1)>===</summary>
            GUILD_INFO_HEADER = 1590,
            ///<summary>No user has been invited to the Command Channel.</summary>
            NO_USER_INVITED_TO_COMMAND_CHANNEL = 1591,
            ///<summary>You can no longer set up a Command Channel.</summary>
            CANNOT_LONGER_SETUP_COMMAND_CHANNEL = 1592,
            ///<summary>You do not have authority to invite someone to the Command Channel.</summary>
            CANNOT_INVITE_TO_COMMAND_CHANNEL = 1593,
            ///<summary>$s1's party is already a member of the Command Channel.</summary>
            S1_ALREADY_MEMBER_OF_COMMAND_CHANNEL = 1594,
            ///<summary>$s1 has succeeded.</summary>
            S1_SUCCEEDED = 1595,
            ///<summary>You were hit by $s1!</summary>
            HIT_BY_S1 = 1596,
            ///<summary>$s1 has failed.</summary>
            S1_FAILED = 1597,
            ///<summary>Soulshots and spiritshots are not available for a dead pet or servitor. Sad, isn't it?</summary>
            SOULSHOTS_AND_SPIRITSHOTS_ARE_NOT_AVAILABLE_FOR_A_DEAD_PET = 1598,
            ///<summary>You cannot observe while you are in combat!</summary>
            CANNOT_OBSERVE_IN_COMBAT = 1599,
            ///<summary>Tomorrow's items will ALL be set to 0. Do you wish to continue?</summary>
            TOMORROW_ITEM_ZERO_CONFIRM = 1600,
            ///<summary>Tomorrow's items will all be set to the same value as today's items. Do you wish to continue?</summary>
            TOMORROW_ITEM_SAME_CONFIRM = 1601,
            ///<summary>Only a party leader can access the Command Channel.</summary>
            COMMAND_CHANNEL_ONLY_FOR_PARTY_LEADER = 1602,
            ///<summary>Only channel operator can give All Command.</summary>
            ONLY_COMMANDER_GIVE_COMMAND = 1603,
            ///<summary>While dressed in formal wear, you can't use items that require all skills and casting operations.</summary>
            CANNOT_USE_ITEMS_SKILLS_WITH_FORMALWEAR = 1604,
            ///<summary>* Here, you can buy only seeds of $s1 Manor.</summary>
            HERE_YOU_CAN_BUY_ONLY_SEEDS_OF_S1_MANOR = 1605,
            ///<summary>Congratulations - You've completed the third-class transfer quest!</summary>
            THIRD_CLASS_TRANSFER = 1606,
            ///<summary>$s1 adena has been withdrawn to pay for purchasing fees.</summary>
            S1_ADENA_HAS_BEEN_WITHDRAWN_TO_PAY_FOR_PURCHASING_FEES = 1607,
            ///<summary>Due to insufficient adena you cannot buy another castle.</summary>
            INSUFFICIENT_ADENA_TO_BUY_CASTLE = 1608,
            ///<summary>War has already been declared against that clan... but I'll make note that you really don't like them.</summary>
            WAR_ALREADY_DECLARED = 1609,
            ///<summary>Fool! You cannot declare war against your own clan!</summary>
            CANNOT_DECLARE_AGAINST_OWN_CLAN = 1610,
            ///<summary>Leader: $s1</summary>
            PARTY_LEADER_S1 = 1611,
            ///<summary>=====<War List>=====</summary>
            WAR_LIST = 1612,
            ///<summary>There is no clan listed on War List.</summary>
            NO_CLAN_ON_WAR_LIST = 1613,
            ///<summary>You have joined a channel that was already open.</summary>
            JOINED_CHANNEL_ALREADY_OPEN = 1614,
            ///<summary>The number of remaining parties is $s1 until a channel is activated</summary>
            S1_PARTIES_REMAINING_UNTIL_CHANNEL = 1615,
            ///<summary>The Command Channel has been activated.</summary>
            COMMAND_CHANNEL_ACTIVATED = 1616,
            ///<summary>You do not have the authority to use the Command Channel.</summary>
            CANT_USE_COMMAND_CHANNEL = 1617,
            ///<summary>The ferry from Rune Harbor to Gludin Harbor has been delayed.</summary>
            FERRY_RUNE_GLUDIN_DELAYED = 1618,
            ///<summary>The ferry from Gludin Harbor to Rune Harbor has been delayed.</summary>
            FERRY_GLUDIN_RUNE_DELAYED = 1619,
            ///<summary>Arrived at Rune Harbor.</summary>
            ARRIVED_AT_RUNE = 1620,
            ///<summary>Departure for Gludin Harbor will take place in five minutes!</summary>
            DEPARTURE_FOR_GLUDIN_5_MINUTES = 1621,
            ///<summary>Departure for Gludin Harbor will take place in one minute!</summary>
            DEPARTURE_FOR_GLUDIN_1_MINUTE = 1622,
            ///<summary>Make haste! We will be departing for Gludin Harbor shortly...</summary>
            DEPARTURE_FOR_GLUDIN_SHORTLY = 1623,
            ///<summary>We are now departing for Gludin Harbor Hold on and enjoy the ride!</summary>
            DEPARTURE_FOR_GLUDIN_NOW = 1624,
            ///<summary>Departure for Rune Harbor will take place after anchoring for ten minutes.</summary>
            DEPARTURE_FOR_RUNE_10_MINUTES = 1625,
            ///<summary>Departure for Rune Harbor will take place in five minutes!</summary>
            DEPARTURE_FOR_RUNE_5_MINUTES = 1626,
            ///<summary>Departure for Rune Harbor will take place in one minute!</summary>
            DEPARTURE_FOR_RUNE_1_MINUTE = 1627,
            ///<summary>Make haste! We will be departing for Gludin Harbor shortly...</summary>
            DEPARTURE_FOR_GLUDIN_SHORTLY2 = 1628,
            ///<summary>We are now departing for Rune Harbor Hold on and enjoy the ride!</summary>
            DEPARTURE_FOR_RUNE_NOW = 1629,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 15 minutes.</summary>
            FERRY_FROM_RUNE_AT_GLUDIN_15_MINUTES = 1630,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.</summary>
            FERRY_FROM_RUNE_AT_GLUDIN_10_MINUTES = 1631,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.</summary>
            FERRY_FROM_RUNE_AT_GLUDIN_5_MINUTES = 1632,
            ///<summary>The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 1 minute.</summary>
            FERRY_FROM_RUNE_AT_GLUDIN_1_MINUTE = 1633,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 15 minutes.</summary>
            FERRY_FROM_GLUDIN_AT_RUNE_15_MINUTES = 1634,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune harbor in approximately 10 minutes.</summary>
            FERRY_FROM_GLUDIN_AT_RUNE_10_MINUTES = 1635,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 10 minutes.</summary>
            FERRY_FROM_GLUDIN_AT_RUNE_5_MINUTES = 1636,
            ///<summary>The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 1 minute.</summary>
            FERRY_FROM_GLUDIN_AT_RUNE_1_MINUTE = 1637,
            ///<summary>You cannot fish while using a recipe book, private manufacture or private store.</summary>
            CANNOT_FISH_WHILE_USING_RECIPE_BOOK = 1638,
            ///<summary>Period $s1 of the Grand Olympiad Games has started!</summary>
            OLYMPIAD_PERIOD_S1_HAS_STARTED = 1639,
            ///<summary>Period $s1 of the Grand Olympiad Games has now ended.</summary>
            OLYMPIAD_PERIOD_S1_HAS_ENDED = 1640,
            /// and make haste to a Grand Olympiad Manager! Battles in the Grand Olympiad Games are now taking place!</summary>
            THE_OLYMPIAD_GAME_HAS_STARTED = 1641,
            ///<summary>Much carnage has been left for the cleanup crew of the Olympiad Stadium. Battles in the Grand Olympiad Games are now over!</summary>
            THE_OLYMPIAD_GAME_HAS_ENDED = 1642,
            ///<summary>Current Location: $s1, $s2, $s3 (Dimensional Gap)</summary>
            LOC_DIMENSIONAL_GAP_S1_S2_S3 = 1643,
            ///<summary>Play time is now accumulating.</summary>
            PLAY_TIME_NOW_ACCUMULATING = 1649,
            ///<summary>Due to high server traffic, your login attempt has failed. Please try again soon.</summary>
            TRY_LOGIN_LATER = 1650,
            ///<summary>The Grand Olympiad Games are not currently in progress.</summary>
            THE_OLYMPIAD_GAME_IS_NOT_CURRENTLY_IN_PROGRESS = 1651,
            ///<summary>You are now recording gameplay.</summary>
            RECORDING_GAMEPLAY_START = 1652,
            ///<summary>Your recording has been successfully stored. ($s1)</summary>
            RECORDING_GAMEPLAY_STOP_S1 = 1653,
            ///<summary>Your attempt to record the replay file has failed.</summary>
            RECORDING_GAMEPLAY_FAILED = 1654,
            ///<summary>You caught something smelly and scary, maybe you should throw it back!?</summary>
            YOU_CAUGHT_SOMETHING_SMELLY_THROW_IT_BACK = 1655,
            ///<summary>You have successfully traded the item with the NPC.</summary>
            SUCCESSFULLY_TRADED_WITH_NPC = 1656,
            ///<summary>$s1 has earned $s2 points in the Grand Olympiad Games.</summary>
            S1_HAS_GAINED_S2_OLYMPIAD_POINTS = 1657,
            ///<summary>$s1 has lost $s2 points in the Grand Olympiad Games.</summary>
            S1_HAS_LOST_S2_OLYMPIAD_POINTS = 1658,
            ///<summary>Current Location: $s1, $s2, $s3 (Cemetery of the Empire)</summary>
            LOC_CEMETARY_OF_THE_EMPIRE_S1_S2_S3 = 1659,
            ///<summary>Channel Creator: $s1.</summary>
            CHANNEL_CREATOR_S1 = 1660,
            ///<summary>$s1 has obtained $s3 $s2s.</summary>
            S1_OBTAINED_S3_S2_S = 1661,
            ///<summary>The fish are no longer biting here because you've caught too many! Try fishing in another location.</summary>
            FISH_NO_MORE_BITING_TRY_OTHER_LOCATION = 1662,
            ///<summary>The clan crest was successfully registered. Remember, only a clan that owns a clan hall or castle can have their crest displayed.</summary>
            CLAN_EMBLEM_WAS_SUCCESSFULLY_REGISTERED = 1663,
            ///<summary>The fish is resisting your efforts to haul it in! Look at that bobber go!</summary>
            FISH_RESISTING_LOOK_BOBBLER = 1664,
            ///<summary>You've worn that fish out! It can't even pull the bobber under the water!</summary>
            YOU_WORN_FISH_OUT = 1665,
            ///<summary>You have obtained +$s1 $s2.</summary>
            OBTAINED_S1_S2 = 1666,
            ///<summary>Lethal Strike!</summary>
            LETHAL_STRIKE = 1667,
            ///<summary>Your lethal strike was successful!</summary>
            LETHAL_STRIKE_SUCCESSFUL = 1668,
            ///<summary>There was nothing found inside of that.</summary>
            NOTHING_INSIDE_THAT = 1669,
            ///<summary>Due to your Reeling and/or Pumping skill being three or more levels higher than your Fishing skill, a 50 damage penalty will be applied.</summary>
            REELING_PUMPING_3_LEVELS_HIGHER_THAN_FISHING_PENALTY = 1670,
            ///<summary>Your reeling was successful! (Mastery Penalty:$s1 )</summary>
            REELING_SUCCESSFUL_PENALTY_S1 = 1671,
            ///<summary>Your pumping was successful! (Mastery Penalty:$s1 )</summary>
            PUMPING_SUCCESSFUL_PENALTY_S1 = 1672,
            ///<summary>Your current record for this Grand Olympiad is $s1 match(es), $s2 win(s) and $s3 defeat(s). You have earned $s4 Olympiad Point(s).</summary>
            THE_CURRENT_RECORD_FOR_THIS_OLYMPIAD_SESSION_IS_S1_MATCHES_S2_WINS_S3_DEFEATS_YOU_HAVE_EARNED_S4_OLYMPIAD_POINTS = 1673,
            ///<summary>This command can only be used by a Noblesse.</summary>
            NOBLESSE_ONLY = 1674,
            ///<summary>A manor cannot be set up between 6 a.m. and 8 p.m.</summary>
            A_MANOR_CANNOT_BE_SET_UP_BETWEEN_6_AM_AND_8_PM = 1675,
            ///<summary>You do not have a servitor or pet and therefore cannot use the automatic-use function.</summary>
            NO_SERVITOR_CANNOT_AUTOMATE_USE = 1676,
            ///<summary>A cease-fire during a Clan War can not be called while members of your clan are engaged in battle.</summary>
            CANT_STOP_CLAN_WAR_WHILE_IN_COMBAT = 1677,
            ///<summary>You have not declared a Clan War against the clan $s1.</summary>
            NO_CLAN_WAR_AGAINST_CLAN_S1 = 1678,
            ///<summary>Only the creator of a channel can issue a global command.</summary>
            ONLY_CHANNEL_CREATOR_CAN_GLOBAL_COMMAND = 1679,
            ///<summary>$s1 has declined the channel invitation.</summary>
            S1_DECLINED_CHANNEL_INVITATION = 1680,
            ///<summary>Since $s1 did not respond, your channel invitation has failed.</summary>
            S1_DID_NOT_RESPOND_CHANNEL_INVITATION_FAILED = 1681,
            ///<summary>Only the creator of a channel can use the channel dismiss command.</summary>
            ONLY_CHANNEL_CREATOR_CAN_DISMISS = 1682,
            ///<summary>Only a party leader can choose the option to leave a channel.</summary>
            ONLY_PARTY_LEADER_CAN_LEAVE_CHANNEL = 1683,
            ///<summary>A Clan War can not be declared against a clan that is being dissolved.</summary>
            NO_CLAN_WAR_AGAINST_DISSOLVING_CLAN = 1684,
            ///<summary>You are unable to equip this item when your PK count is greater or equal to one.</summary>
            YOU_ARE_UNABLE_TO_EQUIP_THIS_ITEM_WHEN_YOUR_PK_COUNT_IS_GREATER_THAN_OR_EQUAL_TO_ONE = 1685,
            ///<summary>Stones and mortar tumble to the earth - the castle wall has taken damage!</summary>
            CASTLE_WALL_DAMAGED = 1686,
            ///<summary>This area cannot be entered while mounted atop of a Wyvern. You will be dismounted from your Wyvern if you do not leave!</summary>
            AREA_CANNOT_BE_ENTERED_WHILE_MOUNTED_WYVERN = 1687,
            ///<summary>You cannot enchant while operating a Private Store or Private Workshop.</summary>
            CANNOT_ENCHANT_WHILE_STORE = 1688,
            ///<summary>You have already joined the waiting list for a class specific match.</summary>
            YOU_ARE_ALREADY_ON_THE_WAITING_LIST_TO_PARTICIPATE_IN_THE_GAME_FOR_YOUR_CLASS = 1689,
            ///<summary>You have already joined the waiting list for a non-class specific match.</summary>
            YOU_ARE_ALREADY_ON_THE_WAITING_LIST_FOR_ALL_CLASSES_WAITING_TO_PARTICIPATE_IN_THE_GAME = 1690,
            ///<summary>You can't join a Grand Olympiad Game match with that much stuff on you! Reduce your weight to below 80 percent full and request to join again!</summary>
            SINCE_80_PERCENT_OR_MORE_OF_YOUR_INVENTORY_SLOTS_ARE_FULL_YOU_CANNOT_PARTICIPATE_IN_THE_OLYMPIAD = 1691,
            ///<summary>You have changed from your main class to a subclass and therefore are removed from the Grand Olympiad Games waiting list.</summary>
            SINCE_YOU_HAVE_CHANGED_YOUR_CLASS_INTO_A_SUB_JOB_YOU_CANNOT_PARTICIPATE_IN_THE_OLYMPIAD = 1692,
            ///<summary>You may not observe a Grand Olympiad Games match while you are on the waiting list.</summary>
            WHILE_YOU_ARE_ON_THE_WAITING_LIST_YOU_ARE_NOT_ALLOWED_TO_WATCH_THE_GAME = 1693,
            ///<summary>Only a clan leader that is a Noblesse can view the Siege War Status window during a siege war.</summary>
            ONLY_NOBLESSE_LEADER_CAN_VIEW_SIEGE_STATUS_WINDOW = 1694,
            ///<summary>You can only use that during a Siege War!</summary>
            ONLY_DURING_SIEGE = 1695,
            ///<summary>Your accumulated play time is $s1.</summary>
            ACCUMULATED_PLAY_TIME_IS_S1 = 1696,
            ///<summary>Your accumulated play time has reached Fatigue level, so you will receive experience or item drops at only 50 percent [...]</summary>
            ACCUMULATED_PLAY_TIME_WARNING1 = 1697,
            ///<summary>Your accumulated play time has reached Ill-health level, so you will no longer gain experience or item drops. [...}</summary>
            ACCUMULATED_PLAY_TIME_WARNING2 = 1698,
            ///<summary>You cannot dismiss a party member by force.</summary>
            CANNOT_DISMISS_PARTY_MEMBER = 1699,
            ///<summary>You don't have enough spiritshots needed for a pet/servitor.</summary>
            NOT_ENOUGH_SPIRITSHOTS_FOR_PET = 1700,
            ///<summary>You don't have enough soulshots needed for a pet/servitor.</summary>
            NOT_ENOUGH_SOULSHOTS_FOR_PET = 1701,
            ///<summary>$s1 is using a third party program.</summary>
            S1_USING_THIRD_PARTY_PROGRAM = 1702,
            ///<summary>The previous investigated user is not using a third party program</summary>
            NOT_USING_THIRD_PARTY_PROGRAM = 1703,
            ///<summary>Please close the setup window for your private manufacturing store or private store, and try again.</summary>
            CLOSE_STORE_WINDOW_AND_TRY_AGAIN = 1704,
            ///<summary>PC Bang Points acquisition period. Points acquisition period left $s1 hour.</summary>
            PCPOINT_ACQUISITION_PERIOD = 1705,
            ///<summary>PC Bang Points use period. Points acquisition period left $s1 hour.</summary>
            PCPOINT_USE_PERIOD = 1706,
            ///<summary>You acquired $s1 PC Bang Point.</summary>
            ACQUIRED_S1_PCPOINT = 1707,
            ///<summary>Double points! You acquired $s1 PC Bang Point.</summary>
            ACQUIRED_S1_PCPOINT_DOUBLE = 1708,
            ///<summary>You are using $s1 point.</summary>
            USING_S1_PCPOINT = 1709,
            ///<summary>You are short of accumulated points.</summary>
            SHORT_OF_ACCUMULATED_POINTS = 1710,
            ///<summary>PC Bang Points use period has expired.</summary>
            PCPOINT_USE_PERIOD_EXPIRED = 1711,
            ///<summary>The PC Bang Points accumulation period has expired.</summary>
            PCPOINT_ACCUMULATION_PERIOD_EXPIRED = 1712,
            ///<summary>The games may be delayed due to an insufficient number of players waiting.</summary>
            GAMES_DELAYED = 1713,
            ///<summary>Current Location: $s1, $s2, $s3 (Near the Town of Schuttgart)</summary>
            LOC_SCHUTTGART_S1_S2_S3 = 1714,
            ///<summary>This is a Peaceful Zone</summary>
            PEACEFUL_ZONE = 1715,
            ///<summary>Altered Zone</summary>
            ALTERED_ZONE = 1716,
            ///<summary>Siege War Zone</summary>
            SIEGE_ZONE = 1717,
            ///<summary>General Field</summary>
            GENERAL_ZONE = 1718,
            ///<summary>Seven Signs Zone</summary>
            SEVENSIGNS_ZONE = 1719,
            ///<summary>---</summary>
            UNKNOWN1 = 1720,
            ///<summary>Combat Zone</summary>
            COMBAT_ZONE = 1721,
            ///<summary>Please enter the name of the item you wish to search for.</summary>
            ENTER_ITEM_NAME_SEARCH = 1722,
            ///<summary>Please take a moment to provide feedback about the petition service.</summary>
            PLEASE_PROVIDE_PETITION_FEEDBACK = 1723,
            ///<summary>A servitor whom is engaged in battle cannot be de-activated.</summary>
            SERVITOR_NOT_RETURN_IN_BATTLE = 1724,
            ///<summary>You have earned $s1 raid point(s).</summary>
            EARNED_S1_RAID_POINTS = 1725,
            ///<summary>$s1 has disappeared because its time period has expired.</summary>
            S1_PERIOD_EXPIRED_DISAPPEARED = 1726,
            ///<summary>$s1 has invited you to a party room. Do you accept?</summary>
            S1_INVITED_YOU_TO_PARTY_ROOM_CONFIRM = 1727,
            ///<summary>The recipient of your invitation did not accept the party matching invitation.</summary>
            PARTY_MATCHING_REQUEST_NO_RESPONSE = 1728,
            ///<summary>You cannot join a Command Channel while teleporting.</summary>
            NOT_JOIN_CHANNEL_WHILE_TELEPORTING = 1729,
            ///<summary>To establish a Clan Academy, your clan must be Level 5 or higher.</summary>
            YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_CLAN_ACADEMY = 1730,
            ///<summary>Only the leader can create a Clan Academy.</summary>
            ONLY_LEADER_CAN_CREATE_ACADEMY = 1731,
            ///<summary>To create a Clan Academy, a Blood Mark is needed.</summary>
            NEED_BLOODMARK_FOR_ACADEMY = 1732,
            ///<summary>You do not have enough adena to create a Clan Academy.</summary>
            NEED_ADENA_FOR_ACADEMY = 1733,
            /// not belong another clan and not yet completed their 2nd class transfer.</summary>
            ACADEMY_REQUIREMENTS = 1734,
            ///<summary>$s1 does not meet the requirements to join a Clan Academy.</summary>
            S1_DOESNOT_MEET_REQUIREMENTS_TO_JOIN_ACADEMY = 1735,
            ///<summary>The Clan Academy has reached its maximum enrollment.</summary>
            ACADEMY_MAXIMUM = 1736,
            ///<summary>Your clan has not established a Clan Academy but is eligible to do so.</summary>
            CLAN_CAN_CREATE_ACADEMY = 1737,
            ///<summary>Your clan has already established a Clan Academy.</summary>
            CLAN_HAS_ALREADY_ESTABLISHED_A_CLAN_ACADEMY = 1738,
            ///<summary>Would you like to create a Clan Academy?</summary>
            CLAN_ACADEMY_CREATE_CONFIRM = 1739,
            ///<summary>Please enter the name of the Clan Academy.</summary>
            ACADEMY_CREATE_ENTER_NAME = 1740,
            ///<summary>Congratulations! The $s1's Clan Academy has been created.</summary>
            THE_S1S_CLAN_ACADEMY_HAS_BEEN_CREATED = 1741,
            ///<summary>A message inviting $s1 to join the Clan Academy is being sent.</summary>
            ACADEMY_INVITATION_SENT_TO_S1 = 1742,
            ///<summary>To open a Clan Academy, the leader of a Level 5 clan or above must pay XX Proofs of Blood or a certain amount of adena.</summary>
            OPEN_ACADEMY_CONDITIONS = 1743,
            ///<summary>There was no response to your invitation to join the Clan Academy, so the invitation has been rescinded.</summary>
            ACADEMY_JOIN_NO_RESPONSE = 1744,
            ///<summary>The recipient of your invitation to join the Clan Academy has declined.</summary>
            ACADEMY_JOIN_DECLINE = 1745,
            ///<summary>You have already joined a Clan Academy.</summary>
            ALREADY_JOINED_ACADEMY = 1746,
            ///<summary>$s1 has sent you an invitation to join the Clan Academy belonging to the $s2 clan. Do you accept?</summary>
            JOIN_ACADEMY_REQUEST_BY_S1_FOR_CLAN_S2 = 1747,
            ///<summary>Clan Academy member $s1 has successfully completed the 2nd class transfer and obtained $s2 Clan Reputation points.</summary>
            CLAN_MEMBER_GRADUATED_FROM_ACADEMY = 1748,
            ///<summary>Congratulations! You will now graduate from the Clan Academy and leave your current clan. As a graduate of the academy, you can immediately join a clan as a regular member without being subject to any penalties.</summary>
            ACADEMY_MEMBERSHIP_TERMINATED = 1749,
            ///<summary>If you possess $s1, you cannot participate in the Olympiad.</summary>
            CANNOT_JOIN_OLYMPIAD_POSSESSING_S1 = 1750,
            ///<summary>The Grand Master has given you a commemorative item.</summary>
            GRAND_MASTER_COMMEMORATIVE_ITEM = 1751,
            ///<summary>Since the clan has received a graduate of the Clan Academy, it has earned $s1 points towards its reputation score.</summary>
            MEMBER_GRADUATED_EARNED_S1_REPU = 1752,
            ///<summary>The clan leader has decreed that that particular privilege cannot be granted to a Clan Academy member.</summary>
            CANT_TRANSFER_PRIVILEGE_TO_ACADEMY_MEMBER = 1753,
            ///<summary>That privilege cannot be granted to a Clan Academy member.</summary>
            RIGHT_CANT_TRANSFERRED_TO_ACADEMY_MEMBER = 1754,
            ///<summary>$s2 has been designated as the apprentice of clan member $s1.</summary>
            S2_HAS_BEEN_DESIGNATED_AS_APPRENTICE_OF_CLAN_MEMBER_S1 = 1755,
            ///<summary>Your apprentice, $s1, has logged in.</summary>
            YOUR_APPRENTICE_S1_HAS_LOGGED_IN = 1756,
            ///<summary>Your apprentice, $s1, has logged out.</summary>
            YOUR_APPRENTICE_S1_HAS_LOGGED_OUT = 1757,
            ///<summary>Your sponsor, $s1, has logged in.</summary>
            YOUR_SPONSOR_S1_HAS_LOGGED_IN = 1758,
            ///<summary>Your sponsor, $s1, has logged out.</summary>
            YOUR_SPONSOR_S1_HAS_LOGGED_OUT = 1759,
            ///<summary>Clan member $s1's name title has been changed to $2.</summary>
            CLAN_MEMBER_S1_TITLE_CHANGED_TO_S2 = 1760,
            ///<summary>Clan member $s1's privilege level has been changed to $s2.</summary>
            CLAN_MEMBER_S1_PRIVILEGE_CHANGED_TO_S2 = 1761,
            ///<summary>You do not have the right to dismiss an apprentice.</summary>
            YOU_DO_NOT_HAVE_THE_RIGHT_TO_DISMISS_AN_APPRENTICE = 1762,
            ///<summary>$s2, clan member $s1's apprentice, has been removed.</summary>
            S2_CLAN_MEMBER_S1_APPRENTICE_HAS_BEEN_REMOVED = 1763,
            ///<summary>This item can only be worn by a member of the Clan Academy.</summary>
            EQUIP_ONLY_FOR_ACADEMY = 1764,
            ///<summary>As a graduate of the Clan Academy, you can no longer wear this item.</summary>
            EQUIP_NOT_FOR_GRADUATES = 1765,
            ///<summary>An application to join the clan has been sent to $s1 in $s2.</summary>
            CLAN_JOIN_APPLICATION_SENT_TO_S1_IN_S2 = 1766,
            ///<summary>An application to join the clan Academy has been sent to $s1.</summary>
            ACADEMY_JOIN_APPLICATION_SENT_TO_S1 = 1767,
            ///<summary>$s1 has invited you to join the Clan Academy of $s2 clan. Would you like to join?</summary>
            JOIN_REQUEST_BY_S1_TO_CLAN_S2_ACADEMY = 1768,
            ///<summary>$s1 has sent you an invitation to join the $s3 Order of Knights under the $s2 clan. Would you like to join?</summary>
            JOIN_REQUEST_BY_S1_TO_ORDER_OF_KNIGHTS_S3_UNDER_CLAN_S2 = 1769,
            ///<summary>The clan's reputation score has dropped below 0. The clan may face certain penalties as a result.</summary>
            CLAN_REPU_0_MAY_FACE_PENALTIES = 1770,
            ///<summary>Now that your clan level is above Level 5, it can accumulate clan reputation points.</summary>
            CLAN_CAN_ACCUMULATE_CLAN_REPUTATION_POINTS = 1771,
            ///<summary>Since your clan was defeated in a siege, $s1 points have been deducted from your clan's reputation score and given to the opposing clan.</summary>
            CLAN_WAS_DEFEATED_IN_SIEGE_AND_LOST_S1_REPUTATION_POINTS = 1772,
            ///<summary>Since your clan emerged victorious from the siege, $s1 points have been added to your clan's reputation score.</summary>
            CLAN_VICTORIOUS_IN_SIEGE_AND_GAINED_S1_REPUTATION_POINTS = 1773,
            ///<summary>Your clan's newly acquired contested clan hall has added $s1 points to your clan's reputation score.</summary>
            CLAN_ACQUIRED_CONTESTED_CLAN_HALL_AND_S1_REPUTATION_POINTS = 1774,
            ///<summary>Clan member $s1 was an active member of the highest-ranked party in the Festival of Darkness. $s2 points have been added to your clan's reputation score.</summary>
            CLAN_MEMBER_S1_WAS_IN_HIGHEST_RANKED_PARTY_IN_FESTIVAL_OF_DARKNESS_AND_GAINED_S2_REPUTATION = 1775,
            ///<summary>Clan member $s1 was named a hero. $2s points have been added to your clan's reputation score.</summary>
            CLAN_MEMBER_S1_BECAME_HERO_AND_GAINED_S2_REPUTATION_POINTS = 1776,
            ///<summary>You have successfully completed a clan quest. $s1 points have been added to your clan's reputation score.</summary>
            CLAN_QUEST_COMPLETED_AND_S1_POINTS_GAINED = 1777,
            ///<summary>An opposing clan has captured your clan's contested clan hall. $s1 points have been deducted from your clan's reputation score.</summary>
            OPPOSING_CLAN_CAPTURED_CLAN_HALL_AND_YOUR_CLAN_LOSES_S1_POINTS = 1778,
            ///<summary>After losing the contested clan hall, 300 points have been deducted from your clan's reputation score.</summary>
            CLAN_LOST_CONTESTED_CLAN_HALL_AND_300_POINTS = 1779,
            ///<summary>Your clan has captured your opponent's contested clan hall. $s1 points have been deducted from your opponent's clan reputation score.</summary>
            CLAN_CAPTURED_CONTESTED_CLAN_HALL_AND_S1_POINTS_DEDUCTED_FROM_OPPONENT = 1780,
            ///<summary>Your clan has added $1s points to its clan reputation score.</summary>
            CLAN_ADDED_S1S_POINTS_TO_REPUTATION_SCORE = 1781,
            ///<summary>Your clan member $s1 was killed. $s2 points have been deducted from your clan's reputation score and added to your opponent's clan reputation score.</summary>
            CLAN_MEMBER_S1_WAS_KILLED_AND_S2_POINTS_DEDUCTED_FROM_REPUTATION = 1782,
            ///<summary>For killing an opposing clan member, $s1 points have been deducted from your opponents' clan reputation score.</summary>
            FOR_KILLING_OPPOSING_MEMBER_S1_POINTS_WERE_DEDUCTED_FROM_OPPONENTS = 1783,
            ///<summary>Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score and added to your opponents'.</summary>
            YOUR_CLAN_FAILED_TO_DEFEND_CASTLE_AND_S1_POINTS_LOST_AND_ADDED_TO_OPPONENT = 1784,
            ///<summary>The clan you belong to has been initialized. $s1 points have been deducted from your clan reputation score.</summary>
            YOUR_CLAN_HAS_BEEN_INITIALIZED_AND_S1_POINTS_LOST = 1785,
            ///<summary>Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score.</summary>
            YOUR_CLAN_FAILED_TO_DEFEND_CASTLE_AND_S1_POINTS_LOST = 1786,
            ///<summary>$s1 points have been deducted from the clan's reputation score.</summary>
            S1_DEDUCTED_FROM_CLAN_REP = 1787,
            ///<summary>The clan skill $s1 has been added.</summary>
            CLAN_SKILL_S1_ADDED = 1788,
            ///<summary>Since the Clan Reputation Score has dropped to 0 or lower, your clan skill(s) will be de-activated.</summary>
            REPUTATION_POINTS_0_OR_LOWER_CLAN_SKILLS_DEACTIVATED = 1789,
            ///<summary>The conditions necessary to increase the clan's level have not been met.</summary>
            FAILED_TO_INCREASE_CLAN_LEVEL = 1790,
            ///<summary>The conditions necessary to create a military unit have not been met.</summary>
            YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_MILITARY_UNIT = 1791,
            ///<summary>Please assign a manager for your new Order of Knights.</summary>
            ASSIGN_MANAGER_FOR_ORDER_OF_KNIGHTS = 1792,
            ///<summary>$s1 has been selected as the captain of $s2.</summary>
            S1_HAS_BEEN_SELECTED_AS_CAPTAIN_OF_S2 = 1793,
            ///<summary>The Knights of $s1 have been created.</summary>
            THE_KNIGHTS_OF_S1_HAVE_BEEN_CREATED = 1794,
            ///<summary>The Royal Guard of $s1 have been created.</summary>
            THE_ROYAL_GUARD_OF_S1_HAVE_BEEN_CREATED = 1795,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE17 = 1796,
            ///<summary>$s1 has been promoted to $s2.</summary>
            S1_PROMOTED_TO_S2 = 1797,
            ///<summary>Clan lord privileges have been transferred to $s1.</summary>
            CLAN_LEADER_PRIVILEGES_HAVE_BEEN_TRANSFERRED_TO_S1 = 1798,
            ///<summary>We are searching for BOT users. Please try again later.</summary>
            SEARCHING_FOR_BOT_USERS_TRY_AGAIN_LATER = 1799,
            ///<summary>User $s1 has a history of using BOT.</summary>
            S1_HISTORY_USING_BOT = 1800,
            ///<summary>The attempt to sell has failed.</summary>
            SELL_ATTEMPT_FAILED = 1801,
            ///<summary>The attempt to trade has failed.</summary>
            TRADE_ATTEMPT_FAILED = 1802,
            ///<summary>The request to participate in the game cannot be made starting from 10 minutes before the end of the game.</summary>
            GAME_REQUEST_CANNOT_BE_MADE = 1803,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE18 = 1804,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE19 = 1805,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE20 = 1806,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE21 = 1807,
            ///<summary>Your account has been suspended ...</summary>
            ILLEGAL_USE22 = 1808,
            /// please visit the PlayNC website (http://www.plaync.com/us/support/)</summary>
            ACCOUNT_MUST_VERIFIED = 1809,
            ///<summary>The refuse invitation state has been activated.</summary>
            REFUSE_INVITATION_ACTIVATED = 1810,
            ///<summary>Since the refuse invitation state is currently activated, no invitation can be made</summary>
            REFUSE_INVITATION_CURRENTLY_ACTIVE = 1812,
            ///<summary>$s1 has $s2 hour(s) of usage time remaining.</summary>
            S2_HOUR_OF_USAGE_TIME_ARE_LEFT_FOR_S1 = 1813,
            ///<summary>$s1 has $s2 minute(s) of usage time remaining.</summary>
            S2_MINUTE_OF_USAGE_TIME_ARE_LEFT_FOR_S1 = 1814,
            ///<summary>$s2 was dropped in the $s1 region.</summary>
            S2_WAS_DROPPED_IN_THE_S1_REGION = 1815,
            ///<summary>The owner of $s2 has appeared in the $s1 region.</summary>
            THE_OWNER_OF_S2_HAS_APPEARED_IN_THE_S1_REGION = 1816,
            ///<summary>$s2's owner has logged into the $s1 region.</summary>
            S2_OWNER_HAS_LOGGED_INTO_THE_S1_REGION = 1817,
            ///<summary>$s1 has disappeared.</summary>
            S1_HAS_DISAPPEARED = 1818,
            ///<summary>An evil is pulsating from $s2 in $s1.</summary>
            EVIL_FROM_S2_IN_S1 = 1819,
            ///<summary>$s1 is currently asleep.</summary>
            S1_CURRENTLY_SLEEP = 1820,
            ///<summary>$s2's evil presence is felt in $s1.</summary>
            S2_EVIL_PRESENCE_FELT_IN_S1 = 1821,
            ///<summary>$s1 has been sealed.</summary>
            S1_SEALED = 1822,
            ///<summary>The registration period for a clan hall war has ended.</summary>
            CLANHALL_WAR_REGISTRATION_PERIOD_ENDED = 1823,
            ///<summary>You have been registered for a clan hall war. Please move to the left side of the clan hall's arena and get ready.</summary>
            REGISTERED_FOR_CLANHALL_WAR = 1824,
            ///<summary>You have failed in your attempt to register for the clan hall war. Please try again.</summary>
            CLANHALL_WAR_REGISTRATION_FAILED = 1825,
            ///<summary>In $s1 minute(s), the game will begin. All players must hurry and move to the left side of the clan hall's arena.</summary>
            CLANHALL_WAR_BEGINS_IN_S1_MINUTES = 1826,
            ///<summary>In $s1 minute(s), the game will begin. All players must, please enter the arena now</summary>
            CLANHALL_WAR_BEGINS_IN_S1_MINUTES_ENTER_NOW = 1827,
            ///<summary>In $s1 seconds(s), the game will begin.</summary>
            CLANHALL_WAR_BEGINS_IN_S1_SECONDS = 1828,
            ///<summary>The Command Channel is full.</summary>
            COMMAND_CHANNEL_FULL = 1829,
            ///<summary>$s1 is not allowed to use the party room invite command. Please update the waiting list.</summary>
            S1_NOT_ALLOWED_INVITE_TO_PARTY_ROOM = 1830,
            ///<summary>$s1 does not meet the conditions of the party room. Please update the waiting list.</summary>
            S1_NOT_MEET_CONDITIONS_FOR_PARTY_ROOM = 1831,
            ///<summary>Only a room leader may invite others to a party room.</summary>
            ONLY_ROOM_LEADER_CAN_INVITE = 1832,
            ///<summary>All of $s1 will be dropped. Would you like to continue?</summary>
            CONFIRM_DROP_ALL_OF_S1 = 1833,
            ///<summary>The party room is full. No more characters can be invitet in</summary>
            PARTY_ROOM_FULL = 1834,
            ///<summary>$s1 is full and cannot accept additional clan members at this time.</summary>
            S1_CLAN_IS_FULL = 1835,
            ///<summary>You cannot join a Clan Academy because you have successfully completed your 2nd class transfer.</summary>
            CANNOT_JOIN_ACADEMY_AFTER_2ND_OCCUPATION = 1836,
            ///<summary>$s1 has sent you an invitation to join the $s3 Royal Guard under the $s2 clan. Would you like to join?</summary>
            S1_SENT_INVITATION_TO_ROYAL_GUARD_S3_OF_CLAN_S2 = 1837,
            ///<summary>1. The coupon an be used once per character.</summary>
            COUPON_ONCE_PER_CHARACTER = 1838,
            ///<summary>2. A used serial number may not be used again.</summary>
            SERIAL_MAY_USED_ONCE = 1839,
            ///<summary>3. If you enter the incorrect serial number more than 5 times, ...</summary>
            SERIAL_INPUT_INCORRECT = 1840,
            ///<summary>The clan hall war has been cancelled. Not enough clans have registered.</summary>
            CLANHALL_WAR_CANCELLED = 1841,
            ///<summary>$s1 wishes to summon you from $s2. Do you accept?</summary>
            S1_WISHES_TO_SUMMON_YOU_FROM_S2_DO_YOU_ACCEPT = 1842,
            ///<summary>$s1 is engaged in combat and cannot be summoned.</summary>
            S1_IS_ENGAGED_IN_COMBAT_AND_CANNOT_BE_SUMMONED = 1843,
            ///<summary>$s1 is dead at the moment and cannot be summoned.</summary>
            S1_IS_DEAD_AT_THE_MOMENT_AND_CANNOT_BE_SUMMONED = 1844,
            ///<summary>Hero weapons cannot be destroyed.</summary>
            HERO_WEAPONS_CANT_DESTROYED = 1845,
            ///<summary>You are too far away from the Strider to mount it.</summary>
            TOO_FAR_AWAY_FROM_STRIDER_TO_MOUNT = 1846,
            ///<summary>You caught a fish $s1 in length.</summary>
            CAUGHT_FISH_S1_LENGTH = 1847,
            ///<summary>Because of the size of fish caught, you will be registered in the ranking</summary>
            REGISTERED_IN_FISH_SIZE_RANKING = 1848,
            ///<summary>All of $s1 will be discarded. Would you like to continue?</summary>
            CONFIRM_DISCARD_ALL_OF_S1 = 1849,
            ///<summary>The Captain of the Order of Knights cannot be appointed.</summary>
            CAPTAIN_OF_ORDER_OF_KNIGHTS_CANNOT_BE_APPOINTED = 1850,
            ///<summary>The Captain of the Royal Guard cannot be appointed.</summary>
            CAPTAIN_OF_ROYAL_GUARD_CANNOT_BE_APPOINTED = 1851,
            ///<summary>The attempt to acquire the skill has failed because of an insufficient Clan Reputation Score.</summary>
            ACQUIRE_SKILL_FAILED_BAD_CLAN_REP_SCORE = 1852,
            ///<summary>Quantity items of the same type cannot be exchanged at the same time</summary>
            CANT_EXCHANGE_QUANTITY_ITEMS_OF_SAME_TYPE = 1853,
            ///<summary>The item was converted successfully.</summary>
            ITEM_CONVERTED_SUCCESSFULLY = 1854,
            ///<summary>Another military unit is already using that name. Please enter a different name.</summary>
            ANOTHER_MILITARY_UNIT_IS_ALREADY_USING_THAT_NAME = 1855,
            ///<summary>Since your opponent is now the owner of $s1, the Olympiad has been cancelled.</summary>
            OPPONENT_POSSESSES_S1_OLYMPIAD_CANCELLED = 1856,
            ///<summary>$s1 is the owner of $s2 and cannot participate in the Olympiad.</summary>
            S1_OWNS_S2_AND_CANNOT_PARTICIPATE_IN_OLYMPIAD = 1857,
            ///<summary>You cannot participate in the Olympiad while dead.</summary>
            CANNOT_PARTICIPATE_OLYMPIAD_WHILE_DEAD = 1858,
            ///<summary>You exceeded the quantity that can be moved at one time.</summary>
            EXCEEDED_QUANTITY_FOR_MOVED = 1859,
            ///<summary>The Clan Reputation Score is too low.</summary>
            THE_CLAN_REPUTATION_SCORE_IS_TOO_LOW = 1860,
            ///<summary>The clan's crest has been deleted.</summary>
            CLAN_CREST_HAS_BEEN_DELETED = 1861,
            ///<summary>Clan skills will now be activated since the clan's reputation score is 0 or higher.</summary>
            CLAN_SKILLS_WILL_BE_ACTIVATED_SINCE_REPUTATION_IS_0_OR_HIGHER = 1862,
            ///<summary>$s1 purchased a clan item, reducing the Clan Reputation by $s2 points.</summary>
            S1_PURCHASED_CLAN_ITEM_REDUCING_S2_REPU_POINTS = 1863,
            ///<summary>Your pet/servitor is unresponsive and will not obey any orders.</summary>
            PET_REFUSING_ORDER = 1864,
            ///<summary>Your pet/servitor is currently in a state of distress.</summary>
            PET_IN_STATE_OF_DISTRESS = 1865,
            ///<summary>MP was reduced by $s1.</summary>
            MP_REDUCED_BY_S1 = 1866,
            ///<summary>Your opponent's MP was reduced by $s1.</summary>
            YOUR_OPPONENTS_MP_WAS_REDUCED_BY_S1 = 1867,
            ///<summary>You cannot exchange an item while it is being used.</summary>
            CANNOT_EXCHANCE_USED_ITEM = 1868,
            ///<summary>$s1 has granted the Command Channel's master party the privilege of item looting.</summary>
            S1_GRANTED_MASTER_PARTY_LOOTING_RIGHTS = 1869,
            ///<summary>A Command Channel with looting rights already exists.</summary>
            COMMAND_CHANNEL_WITH_LOOTING_RIGHTS_EXISTS = 1870,
            ///<summary>Do you want to dismiss $s1 from the clan?</summary>
            CONFIRM_DISMISS_S1_FROM_CLAN = 1871,
            ///<summary>You have $s1 hour(s) and $s2 minute(s) left.</summary>
            S1_HOURS_S2_MINUTES_LEFT = 1872,
            ///<summary>There are $s1 hour(s) and $s2 minute(s) left in the fixed use time for this PC Cafe.</summary>
            S1_HOURS_S2_MINUTES_LEFT_FOR_THIS_PCCAFE = 1873,
            ///<summary>There are $s1 minute(s) left for this individual user.</summary>
            S1_MINUTES_LEFT_FOR_THIS_USER = 1874,
            ///<summary>There are $s1 minute(s) left in the fixed use time for this PC Cafe.</summary>
            S1_MINUTES_LEFT_FOR_THIS_PCCAFE = 1875,
            ///<summary>Do you want to leave $s1 clan?</summary>
            CONFIRM_LEAVE_S1_CLAN = 1876,
            ///<summary>The game will end in $s1 minutes.</summary>
            GAME_WILL_END_IN_S1_MINUTES = 1877,
            ///<summary>The game will end in $s1 seconds.</summary>
            GAME_WILL_END_IN_S1_SECONDS = 1878,
            ///<summary>In $s1 minute(s), you will be teleported outside of the game arena.</summary>
            IN_S1_MINUTES_TELEPORTED_OUTSIDE_OF_GAME_ARENA = 1879,
            ///<summary>In $s1 seconds(s), you will be teleported outside of the game arena.</summary>
            IN_S1_SECONDS_TELEPORTED_OUTSIDE_OF_GAME_ARENA = 1880,
            ///<summary>The preliminary match will begin in $s1 second(s). Prepare yourself.</summary>
            PRELIMINARY_MATCH_BEGIN_IN_S1_SECONDS = 1881,
            ///<summary>Characters cannot be created from this server.</summary>
            CHARACTERS_NOT_CREATED_FROM_THIS_SERVER = 1882,
            ///<summary>There are no offerings I own or I made a bid for.</summary>
            NO_OFFERINGS_OWN_OR_MADE_BID_FOR = 1883,
            ///<summary>Enter the PC Room coupon serial number.</summary>
            ENTER_PCROOM_SERIAL_NUMBER = 1884,
            ///<summary>This serial number cannot be entered. Please try again in minute(s).</summary>
            SERIAL_NUMBER_CANT_ENTERED = 1885,
            ///<summary>This serial has already been used.</summary>
            SERIAL_NUMBER_ALREADY_USED = 1886,
            ///<summary>Invalid serial number. Your attempt to enter the number has failed time(s). You will be allowed to make more attempt(s).</summary>
            SERIAL_NUMBER_ENTERING_FAILED = 1887,
            ///<summary>Invalid serial number. Your attempt to enter the number has failed 5 time(s). Please try again in 4 hours.</summary>
            SERIAL_NUMBER_ENTERING_FAILED_5_TIMES = 1888,
            ///<summary>Congratulations! You have received $s1.</summary>
            CONGRATULATIONS_RECEIVED_S1 = 1889,
            ///<summary>Since you have already used this coupon, you may not use this serial number.</summary>
            ALREADY_USED_COUPON_NOT_USE_SERIAL_NUMBER = 1890,
            ///<summary>You may not use items in a private store or private work shop.</summary>
            NOT_USE_ITEMS_IN_PRIVATE_STORE = 1891,
            ///<summary>The replay file for the previous version cannot be played.</summary>
            REPLAY_FILE_PREVIOUS_VERSION_CANT_PLAYED = 1892,
            ///<summary>This file cannot be replayed.</summary>
            FILE_CANT_REPLAYED = 1893,
            ///<summary>A sub-class cannot be created or changed while you are over your weight limit.</summary>
            NOT_SUBCLASS_WHILE_OVERWEIGHT = 1894,
            ///<summary>$s1 is in an area which blocks summoning.</summary>
            S1_IN_SUMMON_BLOCKING_AREA = 1895,
            ///<summary>$s1 has already been summoned.</summary>
            S1_ALREADY_SUMMONED = 1896,
            ///<summary>$s1 is required for summoning.</summary>
            S1_REQUIRED_FOR_SUMMONING = 1897,
            ///<summary>$s1 is currently trading or operating a private store and cannot be summoned.</summary>
            S1_CURRENTLY_TRADING_OR_OPERATING_PRIVATE_STORE_AND_CANNOT_BE_SUMMONED = 1898,
            ///<summary>Your target is in an area which blocks summoning.</summary>
            YOUR_TARGET_IS_IN_AN_AREA_WHICH_BLOCKS_SUMMONING = 1899,
            ///<summary>$s1 has entered the party room.</summary>
            S1_ENTERED_PARTY_ROOM = 1900,
            ///<summary>$s1 has invited you to enter the party room.</summary>
            S1_INVITED_YOU_TO_PARTY_ROOM = 1901,
            ///<summary>Incompatible item grade. This item cannot be used.</summary>
            INCOMPATIBLE_ITEM_GRADE = 1902,
            ///<summary>Those of you who have requested NCOTP should run NCOTP by using your cell phone [...]</summary>
            NCOTP = 1903,
            ///<summary>A sub-class may not be created or changed while a servitor or pet is summoned.</summary>
            CANT_SUBCLASS_WITH_SUMMONED_SERVITOR = 1904,
            ///<summary>$s2 of $s1 will be replaced with $s4 of $s3.</summary>
            S2_OF_S1_WILL_REPLACED_WITH_S4_OF_S3 = 1905,
            ///<summary>Select the combat unit</summary>
            SELECT_COMBAT_UNIT = 1906,
            ///<summary>Select the character who will [...]</summary>
            SELECT_CHARACTER_WHO_WILL = 1907,
            ///<summary>$s1 in a state which prevents summoning.</summary>
            S1_STATE_FORBIDS_SUMMONING = 1908,
            ///<summary>==< List of Academy Graduates During the Past Week >==</summary>
            ACADEMY_LIST_HEADER = 1909,
            ///<summary>Graduates: $s1.</summary>
            GRADUATES_S1 = 1910,
            ///<summary>You cannot summon players who are currently participating in the Grand Olympiad.</summary>
            YOU_CANNOT_SUMMON_PLAYERS_WHO_ARE_IN_OLYMPIAD = 1911,
            ///<summary>Only those requesting NCOTP should make an entry into this field.</summary>
            NCOTP2 = 1912,
            ///<summary>The remaining recycle time for $s1 is $s2 minute(s).</summary>
            TIME_FOR_S1_IS_S2_MINUTES_REMAINING = 1913,
            ///<summary>The remaining recycle time for $s1 is $s2 seconds(s).</summary>
            TIME_FOR_S1_IS_S2_SECONDS_REMAINING = 1914,
            ///<summary>The game will end in $s1 second(s).</summary>
            GAME_ENDS_IN_S1_SECONDS = 1915,
            ///<summary>Your Death Penalty is now level $s1.</summary>
            DEATH_PENALTY_LEVEL_S1_ADDED = 1916,
            ///<summary>Your Death Penalty has been lifted.</summary>
            DEATH_PENALTY_LIFTED = 1917,
            ///<summary>Your pet is too high level to control.</summary>
            PET_TOO_HIGH_TO_CONTROL = 1918,
            ///<summary>The Grand Olympiad registration period has ended.</summary>
            OLYMPIAD_REGISTRATION_PERIOD_ENDED = 1919,
            ///<summary>Your account is currently inactive because you have not logged into the game for some time. You may reactivate your account by visiting the PlayNC website (http://www.plaync.com/us/support/).</summary>
            ACCOUNT_INACTIVITY = 1920,
            ///<summary>$s2 hour(s) and $s3 minute(s) have passed since $s1 has killed.</summary>
            S2_HOURS_S3_MINUTES_SINCE_S1_KILLED = 1921,
            ///<summary>Because $s1 has failed to kill for one full day, it has expired.</summary>
            S1_FAILED_KILLING_EXPIRED = 1922,
            ///<summary>Court Magician: The portal has been created!</summary>
            COURT_MAGICIAN_CREATED_PORTAL = 1923,
            ///<summary>Current Location: $s1, $s2, $s3 (Near the Primeval Isle)</summary>
            LOC_PRIMEVAL_ISLE_S1_S2_S3 = 1924,
            ///<summary>Due to the affects of the Seal of Strife, it is not possible to summon at this time.</summary>
            SEAL_OF_STRIFE_FORBIDS_SUMMONING = 1925,
            ///<summary>There is no opponent to receive your challenge for a duel.</summary>
            THERE_IS_NO_OPPONENT_TO_RECEIVE_YOUR_CHALLENGE_FOR_A_DUEL = 1926,
            ///<summary>$s1 has been challenged to a duel.</summary>
            S1_HAS_BEEN_CHALLENGED_TO_A_DUEL = 1927,
            ///<summary>$s1's party has been challenged to a duel.</summary>
            S1_PARTY_HAS_BEEN_CHALLENGED_TO_A_DUEL = 1928,
            ///<summary>$s1 has accepted your challenge to a duel. The duel will begin in a few moments.</summary>
            S1_HAS_ACCEPTED_YOUR_CHALLENGE_TO_A_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = 1929,
            ///<summary>You have accepted $s1's challenge to a duel. The duel will begin in a few moments.</summary>
            YOU_HAVE_ACCEPTED_S1_CHALLENGE_TO_A_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = 1930,
            ///<summary>$s1 has declined your challenge to a duel.</summary>
            S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL = 1931,
            ///<summary>$s1 has declined your challenge to a duel.</summary>
            S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL2 = 1932,
            ///<summary>You have accepted $s1's challenge to a party duel. The duel will begin in a few moments.</summary>
            YOU_HAVE_ACCEPTED_S1_CHALLENGE_TO_A_PARTY_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = 1933,
            ///<summary>$s1 has accepted your challenge to duel against their party. The duel will begin in a few moments.</summary>
            S1_HAS_ACCEPTED_YOUR_CHALLENGE_TO_DUEL_AGAINST_THEIR_PARTY_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = 1934,
            ///<summary>$s1 has declined your challenge to a party duel.</summary>
            S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_PARTY_DUEL = 1935,
            ///<summary>The opposing party has declined your challenge to a duel.</summary>
            THE_OPPOSING_PARTY_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL = 1936,
            ///<summary>Since the person you challenged is not currently in a party, they cannot duel against your party.</summary>
            SINCE_THE_PERSON_YOU_CHALLENGED_IS_NOT_CURRENTLY_IN_A_PARTY_THEY_CANNOT_DUEL_AGAINST_YOUR_PARTY = 1937,
            ///<summary>$s1 has challenged you to a duel.</summary>
            S1_HAS_CHALLENGED_YOU_TO_A_DUEL = 1938,
            ///<summary>$s1's party has challenged your party to a duel.</summary>
            S1_PARTY_HAS_CHALLENGED_YOUR_PARTY_TO_A_DUEL = 1939,
            ///<summary>You are unable to request a duel at this time.</summary>
            YOU_ARE_UNABLE_TO_REQUEST_A_DUEL_AT_THIS_TIME = 1940,
            ///<summary>This is no suitable place to challenge anyone or party to a duel.</summary>
            NO_PLACE_FOR_DUEL = 1941,
            ///<summary>The opposing party is currently unable to accept a challenge to a duel.</summary>
            THE_OPPOSING_PARTY_IS_CURRENTLY_UNABLE_TO_ACCEPT_A_CHALLENGE_TO_A_DUEL = 1942,
            ///<summary>The opposing party is currently not in a suitable location for a duel.</summary>
            THE_OPPOSING_PARTY_IS_AT_BAD_LOCATION_FOR_A_DUEL = 1943,
            ///<summary>In a moment, you will be transported to the site where the duel will take place.</summary>
            IN_A_MOMENT_YOU_WILL_BE_TRANSPORTED_TO_THE_SITE_WHERE_THE_DUEL_WILL_TAKE_PLACE = 1944,
            ///<summary>The duel will begin in $s1 second(s).</summary>
            THE_DUEL_WILL_BEGIN_IN_S1_SECONDS = 1945,
            ///<summary>$s1 has challenged you to a duel. Will you accept?</summary>
            S1_CHALLENGED_YOU_TO_A_DUEL = 1946,
            ///<summary>$s1's party has challenged your party to a duel. Will you accept?</summary>
            S1_CHALLENGED_YOU_TO_A_PARTY_DUEL = 1947,
            ///<summary>The duel will begin in $s1 second(s).</summary>
            THE_DUEL_WILL_BEGIN_IN_S1_SECONDS2 = 1948,
            ///<summary>Let the duel begin!</summary>
            LET_THE_DUEL_BEGIN = 1949,
            ///<summary>$s1 has won the duel.</summary>
            S1_HAS_WON_THE_DUEL = 1950,
            ///<summary>$s1's party has won the duel.</summary>
            S1_PARTY_HAS_WON_THE_DUEL = 1951,
            ///<summary>The duel has ended in a tie.</summary>
            THE_DUEL_HAS_ENDED_IN_A_TIE = 1952,
            ///<summary>Since $s1 was disqualified, $s2 has won.</summary>
            SINCE_S1_WAS_DISQUALIFIED_S2_HAS_WON = 1953,
            ///<summary>Since $s1's party was disqualified, $s2's party has won.</summary>
            SINCE_S1_PARTY_WAS_DISQUALIFIED_S2_PARTY_HAS_WON = 1954,
            ///<summary>Since $s1 withdrew from the duel, $s2 has won.</summary>
            SINCE_S1_WITHDREW_FROM_THE_DUEL_S2_HAS_WON = 1955,
            ///<summary>Since $s1's party withdrew from the duel, $s2's party has won.</summary>
            SINCE_S1_PARTY_WITHDREW_FROM_THE_DUEL_S2_PARTY_HAS_WON = 1956,
            ///<summary>Select the item to be augmented.</summary>
            SELECT_THE_ITEM_TO_BE_AUGMENTED = 1957,
            ///<summary>Select the catalyst for augmentation.</summary>
            SELECT_THE_CATALYST_FOR_AUGMENTATION = 1958,
            ///<summary>Requires $s1 $s2.</summary>
            REQUIRES_S1_S2 = 1959,
            ///<summary>This is not a suitable item.</summary>
            THIS_IS_NOT_A_SUITABLE_ITEM = 1960,
            ///<summary>Gemstone quantity is incorrect.</summary>
            GEMSTONE_QUANTITY_IS_INCORRECT = 1961,
            ///<summary>The item was successfully augmented!</summary>
            THE_ITEM_WAS_SUCCESSFULLY_AUGMENTED = 1962,
            /// ID : 1963
            ///<summary>Select the item from which you wish to remove augmentation.</summary>
            SELECT_THE_ITEM_FROM_WHICH_YOU_WISH_TO_REMOVE_AUGMENTATION = 1963,
            ///<summary>Augmentation removal can only be done on an augmented item.</summary>
            AUGMENTATION_REMOVAL_CAN_ONLY_BE_DONE_ON_AN_AUGMENTED_ITEM = 1964,
            ///<summary>Augmentation has been successfully removed from your $s1.</summary>
            AUGMENTATION_HAS_BEEN_SUCCESSFULLY_REMOVED_FROM_YOUR_S1 = 1965,
            ///<summary>Only the clan leader may issue commands.</summary>
            ONLY_CLAN_LEADER_CAN_ISSUE_COMMANDS = 1966,
            ///<summary>The gate is firmly locked. Please try again later.</summary>
            GATE_LOCKED_TRY_AGAIN_LATER = 1967,
            ///<summary>$s1's owner.</summary>
            S1_OWNER = 1968,
            ///<summary>Area where $s1 appears.</summary>
            AREA_S1_APPEARS = 1968,
            ///<summary>Once an item is augmented, it cannot be augmented again.</summary>
            ONCE_AN_ITEM_IS_AUGMENTED_IT_CANNOT_BE_AUGMENTED_AGAIN = 1970,
            ///<summary>The level of the hardener is too high to be used.</summary>
            HARDENER_LEVEL_TOO_HIGH = 1971,
            ///<summary>You cannot augment items while a private store or private workshop is in operation.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_A_PRIVATE_STORE_OR_PRIVATE_WORKSHOP_IS_IN_OPERATION = 1972,
            ///<summary>You cannot augment items while frozen.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_FROZEN = 1973,
            ///<summary>You cannot augment items while dead.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_DEAD = 1974,
            ///<summary>You cannot augment items while engaged in trade activities.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_TRADING = 1975,
            ///<summary>You cannot augment items while paralyzed.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_PARALYZED = 1976,
            ///<summary>You cannot augment items while fishing.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_FISHING = 1977,
            ///<summary>You cannot augment items while sitting down.</summary>
            YOU_CANNOT_AUGMENT_ITEMS_WHILE_SITTING_DOWN = 1978,
            ///<summary>$s1's remaining Mana is now 10.</summary>
            S1S_REMAINING_MANA_IS_NOW_10 = 1979,
            ///<summary>$s1's remaining Mana is now 5.</summary>
            S1S_REMAINING_MANA_IS_NOW_5 = 1980,
            ///<summary>$s1's remaining Mana is now 1. It will disappear soon.</summary>
            S1S_REMAINING_MANA_IS_NOW_1 = 1981,
            ///<summary>$s1's remaining Mana is now 0, and the item has disappeared.</summary>
            S1S_REMAINING_MANA_IS_NOW_0 = 1982,
            ///<summary>Press the Augment button to begin.</summary>
            PRESS_THE_AUGMENT_BUTTON_TO_BEGIN = 1984,
            ///<summary>$s1's drop area ($s2)</summary>
            S1_DROP_AREA_S2 = 1985,
            ///<summary>$s1's owner ($s2)</summary>
            S1_OWNER_S2 = 1986,
            ///<summary>$s1</summary>
            S1 = 1987,
            ///<summary>The ferry has arrived at Primeval Isle.</summary>
            FERRY_ARRIVED_AT_PRIMEVAL = 1988,
            ///<summary>The ferry will leave for Rune Harbor after anchoring for three minutes.</summary>
            FERRY_LEAVING_FOR_RUNE_3_MINUTES = 1989,
            ///<summary>The ferry is now departing Primeval Isle for Rune Harbor.</summary>
            FERRY_LEAVING_PRIMEVAL_FOR_RUNE_NOW = 1990,
            ///<summary>The ferry will leave for Primeval Isle after anchoring for three minutes.</summary>
            FERRY_LEAVING_FOR_PRIMEVAL_3_MINUTES = 1991,
            ///<summary>The ferry is now departing Rune Harbor for Primeval Isle.</summary>
            FERRY_LEAVING_RUNE_FOR_PRIMEVAL_NOW = 1992,
            ///<summary>The ferry from Primeval Isle to Rune Harbor has been delayed.</summary>
            FERRY_FROM_PRIMEVAL_TO_RUNE_DELAYED = 1993,
            ///<summary>The ferry from Rune Harbor to Primeval Isle has been delayed.</summary>
            FERRY_FROM_RUNE_TO_PRIMEVAL_DELAYED = 1994,
            ///<summary>$s1 channel filtering option</summary>
            S1_CHANNEL_FILTER_OPTION = 1995,
            ///<summary>The attack has been blocked.</summary>
            ATTACK_WAS_BLOCKED = 1996,
            ///<summary>$s1 is performing a counterattack.</summary>
            S1_PERFORMING_COUNTERATTACK = 1997,
            ///<summary>You countered $s1's attack.</summary>
            COUNTERED_S1_ATTACK = 1998,
            ///<summary>$s1 dodges the attack.</summary>
            S1_DODGES_ATTACK = 1999,
            ///<summary>You have avoided $s1's attack.</summary>
            AVOIDED_S1_ATTACK2 = 2000,
            ///<summary>Augmentation failed due to inappropriate conditions.</summary>
            AUGMENTATION_FAILED_DUE_TO_INAPPROPRIATE_CONDITIONS = 2001,
            ///<summary>Trap failed.</summary>
            TRAP_FAILED = 2002,
            ///<summary>You obtained an ordinary material.</summary>
            OBTAINED_ORDINARY_MATERIAL = 2003,
            ///<summary>You obtained a rare material.</summary>
            OBTAINED_RATE_MATERIAL = 2004,
            ///<summary>You obtained a unique material.</summary>
            OBTAINED_UNIQUE_MATERIAL = 2005,
            ///<summary>You obtained the only material of this kind.</summary>
            OBTAINED_ONLY_MATERIAL = 2006,
            ///<summary>Please enter the recipient's name.</summary>
            ENTER_RECIPIENTS_NAME = 2007,
            ///<summary>Please enter the text.</summary>
            ENTER_TEXT = 2008,
            ///<summary>You cannot exceed 1500 characters.</summary>
            CANT_EXCEED_1500_CHARACTERS = 2009,
            ///<summary>$s2 $s1</summary>
            S2_S1 = 2009,
            ///<summary>The augmented item cannot be discarded.</summary>
            AUGMENTED_ITEM_CANNOT_BE_DISCARDED = 2011,
            ///<summary>$s1 has been activated.</summary>
            S1_HAS_BEEN_ACTIVATED = 2012,
            ///<summary>Your seed or remaining purchase amount is inadequate.</summary>
            YOUR_SEED_OR_REMAINING_PURCHASE_AMOUNT_IS_INADEQUATE = 2013,
            ///<summary>You cannot proceed because the manor cannot accept any more crops. All crops have been returned and no adena withdrawn.</summary>
            MANOR_CANT_ACCEPT_MORE_CROPS = 2014,
            ///<summary>A skill is ready to be used again.</summary>
            SKILL_READY_TO_USE_AGAIN = 2015,
            ///<summary>A skill is ready to be used again but its re-use counter time has increased.</summary>
            SKILL_READY_TO_USE_AGAIN_BUT_TIME_INCREASED = 2016,
            ///<summary>$s1 cannot duel because $s1 is currently engaged in a private store or manufacture.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_ENGAGED_IN_A_PRIVATE_STORE_OR_MANUFACTURE = 2017,
            ///<summary>$s1 cannot duel because $s1 is currently fishing.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_FISHING = 2018,
            ///<summary>$s1 cannot duel because $s1's HP or MP is below 50%.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_HP_OR_MP_IS_BELOW_50_PERCENT = 2019,
            ///<summary>$s1 cannot make a challenge to a duel because $s1 is currently in a duel-prohibited area (Peaceful Zone / Seven Signs Zone / Near Water / Restart Prohibited Area).</summary>
            S1_CANNOT_MAKE_A_CHALLANGE_TO_A_DUEL_BECAUSE_S1_IS_CURRENTLY_IN_A_DUEL_PROHIBITED_AREA = 2020,
            ///<summary>$s1 cannot duel because $s1 is currently engaged in battle.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_ENGAGED_IN_BATTLE = 2021,
            ///<summary>$s1 cannot duel because $s1 is already engaged in a duel.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_ALREADY_ENGAGED_IN_A_DUEL = 2022,
            ///<summary>$s1 cannot duel because $s1 is in a chaotic state.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_IN_A_CHAOTIC_STATE = 2023,
            ///<summary>$s1 cannot duel because $s1 is participating in the Olympiad.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_THE_OLYMPIAD = 2024,
            ///<summary>$s1 cannot duel because $s1 is participating in a clan hall war.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_A_CLAN_HALL_WAR = 2025,
            ///<summary>$s1 cannot duel because $s1 is participating in a siege war.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_A_SIEGE_WAR = 2026,
            ///<summary>$s1 cannot duel because $s1 is currently riding a boat or strider.</summary>
            S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_RIDING_A_BOAT_WYVERN_OR_STRIDER = 2027,
            ///<summary>$s1 cannot receive a duel challenge because $s1 is too far away.</summary>
            S1_CANNOT_RECEIVE_A_DUEL_CHALLENGE_BECAUSE_S1_IS_TOO_FAR_AWAY = 2028,
            ///<summary>$s1 is currently teleporting and cannot participate in the Olympiad.</summary>
            S1_CANNOT_PARTICIPATE_IN_OLYMPIAD_DURING_TELEPORT = 2029,
            ///<summary>You are currently logging in.</summary>
            CURRENTLY_LOGGING_IN = 2030,
            ///<summary>Please wait a moment.</summary>
            PLEASE_WAIT_A_MOMENT = 2031,

            //Added (Missing?)
            ///<summary>You can only register 16x12 pixel 256 color bmp files.</summary>            
            CAN_ONLY_REGISTER_16_12_PX_256_COLOR_BMP_FILES = 211,
            ///<summary>Incorrect item.</summary>            
            INCORRECT_ITEM = 352,

            //Other messages (Interlude+) being referenced in the project            

            ///<summary>You already polymorphed and cannot polymorph again.</summary>
            ALREADY_POLYMORPHED_CANNOT_POLYMORPH_AGAIN = 2058,
            ///<summary>You cannot polymorph into the desired form in water.</summary>
            CANNOT_POLYMORPH_INTO_THE_DESIRED_FORM_IN_WATER = 2060,
            ///<summary>You cannot polymorph when you have summoned a servitor/pet.</summary>
            CANNOT_POLYMORPH_WHEN_SUMMONED_SERVITOR = 2062,
            ///<summary>You cannot polymorph while riding a pet.</summary>
            CANNOT_POLYMORPH_WHILE_RIDING_PET = 2063,

            ///<summary>//You cannot enter due to the party having exceeded the limit.</summary>
            CANNOT_ENTER_DUE_PARTY_HAVING_EXCEED_LIMIT = 2102,
            ///<summary>The augmented item cannot be converted. Please convert after the augmentation has been removed.</summary>
            AUGMENTED_ITEM_CANNOT_BE_CONVERTED = 2129,
            ///<summary>You cannot convert this item.</summary>
            CANNOT_CONVERT_THIS_ITEM = 2130,
            ///<summary>Your soul count has increased by $s1. It is now at $s2.</summary>
            YOUR_SOUL_COUNT_HAS_INCREASED_BY_S1_NOW_AT_S2 = 2162,
            ///<summary>Soul cannot be increased anymore.</summary>
            SOUL_CANNOT_BE_INCREASED_ANYMORE = 2163,
            ///<summary>You cannot polymorph while riding a boat.</summary>
            CANNOT_POLYMORPH_WHILE_RIDING_BOAT = 2182,
            ///<summary>Another enchantment is in progress. Please complete the previous task, then try again.</summary>
            ANOTHER_ENCHANTMENT_IS_IN_PROGRESS = 2188,

            ///<summary>Not enough bolts.</summary>
            NOT_ENOUGH_BOLTS = 2226,
            ///<summary>$c1 has given $c2 damage of $s3.</summary>
            C1_HAS_GIVEN_C2_DAMAGE_OF_S3 = 2261,
            ///<summary>$c1 has received $s3 damage from $c2.</summary>
            C1_HAS_RECEIVED_S3_DAMAGE_FROM_C2 = 2262,
            ///<summary>$c1 has evaded $c2's attack.</summary>
            C1_HAS_EVADED_C2_ATTACK = 2264,
            ///<summary>$c1's attack went astray.</summary>
            C1_ATTACK_WENT_ASTRAY = 2265,
            ///<summary>$c1 landed a critical hit!</summary>
            C1_LANDED_A_CRITICAL_HIT = 2266,
            ///<summary>You cannot transform while sitting.</summary>
            CANNOT_TRANSFORM_WHILE_SITTING = 2283,
            ///<summary>The length of the crest or insignia does not meet the standard requirements.</summary>
            LENGTH_CREST_DOES_NOT_MEET_STANDARD_REQUIREMENTS = 2285,

            ///<summary>There are $s2 second(s) remaining in $s1's re-use time.</summary>
            S2_SECONDS_REMAINING_IN_S1_REUSE_TIME = 2303,
            ///<summary>There are $s2 minute(s), $s3 second(s) remaining in $s1's re-use time.</summary>
            S2_MINUTES_S3_SECONDS_REMAINING_IN_S1_REUSE_TIME = 2304,
            ///<summary>There are $s2 hour(s), $s3 minute(s), and $s4 second(s) remaining in $s1's re-use time.</summary>
            S2_HOURS_S3_MINUTES_S4_SECONDS_REMAINING_IN_S1_REUSE_TIME = 2305,
            ///<summary>This is an incorrect support enhancement spellbook.</summary>
            INCORRECT_SUPPORT_ENHANCEMENT_SPELLBOOK = 2385,
            ///<summary>This item does not meet the requirements for the support enhancement spellbook.</summary>
            ITEM_DOES_NOT_MEET_REQUIREMENTS_FOR_SUPPORT_ENHANCEMENT_SPELLBOOK = 2386,
            ///<summary>Registration of the support enhancement spellbook has failed.</summary>
            REGISTRATION_OF_ENHANCEMENT_SPELLBOOK_HAS_FAILED = 2387,

            ///<summary>You cannot use My Teleports while flying.</summary>
            CANNOT_USE_MY_TELEPORTS_WHILE_FLYING = 2351,
            ///<summary>You cannot use My Teleports while you are dead.</summary>
            CANNOT_USE_MY_TELEPORTS_WHILE_DEAD = 2354,
            ///<summary>You cannot use My Teleports underwater.</summary>
            CANNOT_USE_MY_TELEPORTS_UNDERWATER = 2356,
            ///<summary>You have no space to save the teleport location.</summary>            
            NO_SPACE_TO_SAVE_TELEPORT_LOCATION = 2358,
            ///<summary>You cannot teleport because you do not have a teleport item.</summary>
            CANNOT_TELEPORT_BECAUSE_DO_NOT_HAVE_TELEPORT_ITEM = 2359,
            ///<summary>Your number of My Teleports slots has reached its maximum limit.</summary>
            YOUR_NUMBER_OF_MY_TELEPORTS_SLOTS_HAS_REACHED_LIMIT = 2390,

            ///<summary>The number of My Teleports slots has been increased.</summary>
            NUMBER_OF_MY_TELEPORTS_SLOTS_HAS_BEEN_INCREASED = 2409,

            ///<summary>//You cannot teleport while in possession of a ward.</summary>
            CANNOT_TELEPORT_WHILE_POSSESSION_WARD = 2778,

            ///<summary>You could not receive because your inventory is full.</summary>
            YOU_COULD_NOT_RECEIVE_BECAUSE_YOUR_INVENTORY_IS_FULL = 2981,

            //A user currently participating in the Olympiad cannot send party and friend invitations.
            USER_CURRENTLY_PARTICIPATING_IN_OLYMPIAD_CANNOT_SEND_PARTY_AND_FRIEND_INVITATIONS = 3094,

            ///<summary>Requesting approval for changing party loot to "$s1".</summary>
            REQUESTING_APPROVAL_FOR_CHANGING_PARTY_LOOT_TO_S1 = 3135,
            ///<summary>Party loot change was cancelled.</summary>
            PARTY_LOOT_CHANGE_WAS_CANCELLED = 3137,
            ///<summary>Party loot was changed to "$s1".</summary>
            PARTY_LOOT_WAS_CHANGED_TO_S1 = 3138,
            ///<summary>$c1 is set to refuse party requests and cannot receive a party request.</summary>
            C1_IS_SET_TO_REFUSE_PARTY_REQUESTS = 3168,

            ///<summary>You cannot bookmark this location because you do not have a My Teleport Flag.</summary>
            CANNOT_BOOKMARK_THIS_LOCATION_BECAUSE_NO_MY_TELEPORT_FLAG = 6501,

            //No description found

            NOT_IMPLEMENTED_YET_2361 = 2361,
        }
    }
}