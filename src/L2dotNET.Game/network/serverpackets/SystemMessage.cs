using System.Collections.Generic;
using L2dotNET.Game.world;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class SystemMessage : GameServerNetworkPacket
    {
        private List<object[]> data = new List<object[]>();
        public int MessgeID;

        public SystemMessage(int msgId)
        {
            MessgeID = msgId;
        }

        public SystemMessage addString(string val)
        {
            data.Add(new object[] { 0, val });
            return this;
        }

        public SystemMessage addNumber(int val)
        {
            data.Add(new object[] { 1, val });
            return this;
        }

        public SystemMessage addNumber(double val)
        {
            data.Add(new object[] { 1, (int)val });
            return this;
        }

        public SystemMessage addNpcName(int val)
        {
            data.Add(new object[] { 2, (1000000 + val) });
            return this;
        }

        public SystemMessage addItemName(int val)
        {
            data.Add(new object[] { 3, val });
            return this;
        }

        public SystemMessage addSkillName(int val, int lvl)
        {
            data.Add(new object[] { 4, val, lvl });
            return this;
        }

        public void addCastleName(int val)
        {
            data.Add(new object[] { 5, val });
        }

        public void addItemCount(long val)
        {
            data.Add(new object[] { 6, val });
        }

        public void addZoneName(int val, int y, int z)
        {
            data.Add(new object[] { 7, val, y, z });
        }

        public void addElementName(int val)
        {
            data.Add(new object[] { 9, val });
        }

        public void addInstanceName(int val)
        {
            data.Add(new object[] { 10, val });
        }

        public SystemMessage addPlayerName(string val)
        {
            data.Add(new object[] { 12, val });
            return this;
        }

        public SystemMessage addName(L2Object obj)
        {
            if (obj is L2Player)
                return addPlayerName(((L2Player)obj).Name);
            else if (obj is L2Citizen)
                return addNpcName(((L2Citizen)obj).NpcId);
            else if (obj is L2Summon)
                return addNpcName(((L2Summon)obj).NpcId);
            else if (obj is L2Item)
                return addItemName(((L2Item)obj).Template.ItemID);
            else
                return addString(obj.asString());
        }

        public void addSysStr(int val)
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

        /// <summary>
        /// ID: 0
        /// <para/>
        /// Message: You have been disconnected from the server.
        /// </summary>
        public static SystemMessage YOU_HAVE_BEEN_DISCONNECTED = new SystemMessage(0);

        /// <summary>
        /// ID: 1
        /// <para/>
        /// Message: The server will be coming down in $1 seconds. Please find a safe place to log out.
        /// </summary>
        public static SystemMessage THE_SERVER_WILL_BE_COMING_DOWN_IN_S1_SECONDS = new SystemMessage(1);

        /// <summary>
        /// ID: 2
        /// <para/>
        /// Message: $s1 does not exist.
        /// </summary>
        public static SystemMessage S1_DOES_NOT_EXIST = new SystemMessage(2);

        /// <summary>
        /// ID: 3
        /// <para/>
        /// Message: $s1 is not currently logged in.
        /// </summary>
        public static SystemMessage S1_IS_NOT_ONLINE = new SystemMessage(3);

        /// <summary>
        /// ID: 4
        /// <para/>
        /// Message: You cannot ask yourself to apply to a clan.
        /// </summary>
        public static SystemMessage CANNOT_INVITE_YOURSELF = new SystemMessage(4);

        /// <summary>
        /// ID: 5
        /// <para/>
        /// Message: $s1 already exists.
        /// </summary>
        public static SystemMessage S1_ALREADY_EXISTS = new SystemMessage(5);

        /// <summary>
        /// ID: 6
        /// <para/>
        /// Message: $s1 does not exist
        /// </summary>
        public static SystemMessage S1_DOES_NOT_EXIST2 = new SystemMessage(6);

        /// <summary>
        /// ID: 7
        /// <para/>
        /// Message: You are already a member of $s1.
        /// </summary>
        public static SystemMessage ALREADY_MEMBER_OF_S1 = new SystemMessage(7);

        /// <summary>
        /// ID: 8
        /// <para/>
        /// Message: You are working with another clan.
        /// </summary>
        public static SystemMessage YOU_ARE_WORKING_WITH_ANOTHER_CLAN = new SystemMessage(8);

        /// <summary>
        /// ID: 9
        /// <para/>
        /// Message: $s1 is not a clan leader.
        /// </summary>
        public static SystemMessage S1_IS_NOT_A_CLAN_LEADER = new SystemMessage(9);

        /// <summary>
        /// ID: 10
        /// <para/>
        /// Message: $s1 is working with another clan.
        /// </summary>
        public static SystemMessage S1_WORKING_WITH_ANOTHER_CLAN = new SystemMessage(10);

        /// <summary>
        /// ID: 11
        /// <para/>
        /// Message: There are no applicants for this clan.
        /// </summary>
        public static SystemMessage NO_APPLICANTS_FOR_THIS_CLAN = new SystemMessage(11);

        /// <summary>
        /// ID: 12
        /// <para/>
        /// Message: The applicant information is incorrect.
        /// </summary>
        public static SystemMessage APPLICANT_INFORMATION_INCORRECT = new SystemMessage(12);

        /// <summary>
        /// ID: 13
        /// <para/>
        /// Message: Unable to disperse: your clan has requested to participate in a castle siege.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_CAUSE_CLAN_WILL_PARTICIPATE_IN_CASTLE_SIEGE = new SystemMessage(13);

        /// <summary>
        /// ID: 14
        /// <para/>
        /// Message: Unable to disperse: your clan owns one or more castles or hideouts.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_CAUSE_CLAN_OWNS_CASTLES_HIDEOUTS = new SystemMessage(14);

        /// <summary>
        /// ID: 15
        /// <para/>
        /// Message: You are in siege.
        /// </summary>
        public static SystemMessage YOU_ARE_IN_SIEGE = new SystemMessage(15);

        /// <summary>
        /// ID: 16
        /// <para/>
        /// Message: You are not in siege.
        /// </summary>
        public static SystemMessage YOU_ARE_NOT_IN_SIEGE = new SystemMessage(16);

        /// <summary>
        /// ID: 17
        /// <para/>
        /// Message: The castle siege has begun.
        /// </summary>
        public static SystemMessage CASTLE_SIEGE_HAS_BEGUN = new SystemMessage(17);

        /// <summary>
        /// ID: 18
        /// <para/>
        /// Message: The castle siege has ended.
        /// </summary>
        public static SystemMessage CASTLE_SIEGE_HAS_ENDED = new SystemMessage(18);

        /// <summary>
        /// ID: 19
        /// <para/>
        /// Message: There is a new Lord of the castle!
        /// </summary>
        public static SystemMessage NEW_CASTLE_LORD = new SystemMessage(19);

        /// <summary>
        /// ID: 20
        /// <para/>
        /// Message: The gate is being opened.
        /// </summary>
        public static SystemMessage GATE_IS_OPENING = new SystemMessage(20);

        /// <summary>
        /// ID: 21
        /// <para/>
        /// Message: The gate is being destroyed.
        /// </summary>
        public static SystemMessage GATE_IS_DESTROYED = new SystemMessage(21);

        /// <summary>
        /// ID: 22
        /// <para/>
        /// Message: Your target is out of range.
        /// </summary>
        public static SystemMessage TARGET_TOO_FAR = new SystemMessage(22);

        /// <summary>
        /// ID: 23
        /// <para/>
        /// Message: Not enough HP.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_HP = new SystemMessage(23);

        /// <summary>
        /// ID: 24
        /// <para/>
        /// Message: Not enough MP.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_MP = new SystemMessage(24);

        /// <summary>
        /// ID: 25
        /// <para/>
        /// Message: Rejuvenating HP.
        /// </summary>
        public static SystemMessage REJUVENATING_HP = new SystemMessage(25);

        /// <summary>
        /// ID: 26
        /// <para/>
        /// Message: Rejuvenating MP.
        /// </summary>
        public static SystemMessage REJUVENATING_MP = new SystemMessage(26);

        /// <summary>
        /// ID: 27
        /// <para/>
        /// Message: Your casting has been interrupted.
        /// </summary>
        public static SystemMessage CASTING_INTERRUPTED = new SystemMessage(27);

        /// <summary>
        /// ID: 28
        /// <para/>
        /// Message: You have obtained $s1 adena.
        /// </summary>
        public static SystemMessage YOU_PICKED_UP_S1_ADENA = new SystemMessage(28);

        /// <summary>
        /// ID: 29
        /// <para/>
        /// Message: You have obtained $s2 $s1.
        /// </summary>
        public static SystemMessage YOU_PICKED_UP_S2_S1 = new SystemMessage(29);

        /// <summary>
        /// ID: 30
        /// <para/>
        /// Message: You have obtained $s1.
        /// </summary>
        public static SystemMessage YOU_PICKED_UP_S1 = new SystemMessage(30);

        /// <summary>
        /// ID: 31
        /// <para/>
        /// Message: You cannot move while sitting.
        /// </summary>
        public static SystemMessage CANT_MOVE_SITTING = new SystemMessage(31);

        /// <summary>
        /// ID: 32
        /// <para/>
        /// Message: You are unable to engage in combat. Please go to the nearest restart point.
        /// </summary>
        public static SystemMessage UNABLE_COMBAT_PLEASE_GO_RESTART = new SystemMessage(32);

        /// <summary>
        /// ID: 32
        /// <para/>
        /// Message: You cannot move while casting.
        /// </summary>
        public static SystemMessage CANT_MOVE_CASTING = new SystemMessage(32);

        /// <summary>
        /// ID: 34
        /// <para/>
        /// Message: Welcome to the World of Lineage II.
        /// </summary>
        public static SystemMessage WELCOME_TO_LINEAGE = new SystemMessage(34);

        /// <summary>
        /// ID: 35
        /// <para/>
        /// Message: You hit for $s1 damage
        /// </summary>
        public static SystemMessage YOU_DID_S1_DMG = new SystemMessage(35);

        /// <summary>
        /// ID: 36
        /// <para/>
        /// Message: $s1 hit you for $s2 damage.
        /// </summary>
        public static SystemMessage S1_GAVE_YOU_S2_DMG = new SystemMessage(36);

        /// <summary>
        /// ID: 37
        /// <para/>
        /// Message: $s1 hit you for $s2 damage.
        /// </summary>
        public static SystemMessage S1_GAVE_YOU_S2_DMG2 = new SystemMessage(37);

        /// <summary>
        /// ID: 41
        /// <para/>
        /// Message: You carefully nock an arrow.
        /// </summary>
        public static SystemMessage GETTING_READY_TO_SHOOT_AN_ARROW = new SystemMessage(41);

        /// <summary>
        /// ID: 42
        /// <para/>
        /// Message: You have avoided $s1's attack.
        /// </summary>
        public static SystemMessage AVOIDED_S1_ATTACK = new SystemMessage(42);

        /// <summary>
        /// ID: 43
        /// <para/>
        /// Message: You have missed.
        /// </summary>
        public static SystemMessage MISSED_TARGET = new SystemMessage(43);

        /// <summary>
        /// ID: 44
        /// <para/>
        /// Message: Critical hit!
        /// </summary>
        public static SystemMessage CRITICAL_HIT = new SystemMessage(44);

        /// <summary>
        /// ID: 45
        /// <para/>
        /// Message: You have earned $s1 experience.
        /// </summary>
        public static SystemMessage EARNED_S1_EXPERIENCE = new SystemMessage(45);

        /// <summary>
        /// ID: 46
        /// <para/>
        /// Message: You use $s1.
        /// </summary>
        public static SystemMessage USE_S1 = new SystemMessage(46);

        /// <summary>
        /// ID: 47
        /// <para/>
        /// Message: You begin to use a(n) $s1.
        /// </summary>
        public static SystemMessage BEGIN_TO_USE_S1 = new SystemMessage(47);

        /// <summary>
        /// ID: 48
        /// <para/>
        /// Message: $s1 is not available at this time: being prepared for reuse.
        /// </summary>
        public static SystemMessage S1_PREPARED_FOR_REUSE = new SystemMessage(48);

        /// <summary>
        /// ID: 49
        /// <para/>
        /// Message: You have equipped your $s1.
        /// </summary>
        public static SystemMessage S1_EQUIPPED = new SystemMessage(49);

        /// <summary>
        /// ID: 50
        /// <para/>
        /// Message: Your target cannot be found.
        /// </summary>
        public static SystemMessage TARGET_CANT_FOUND = new SystemMessage(50);

        /// <summary>
        /// ID: 51
        /// <para/>
        /// Message: You cannot use this on yourself.
        /// </summary>
        public static SystemMessage CANNOT_USE_ON_YOURSELF = new SystemMessage(51);

        /// <summary>
        /// ID: 52
        /// <para/>
        /// Message: You have earned $s1 adena.
        /// </summary>
        public static SystemMessage EARNED_S1_ADENA = new SystemMessage(52);

        /// <summary>
        /// ID: 53
        /// <para/>
        /// Message: You have earned $s2 $s1(s).
        /// </summary>
        public static SystemMessage EARNED_S2_S1_S = new SystemMessage(53);

        /// <summary>
        /// ID: 54
        /// <para/>
        /// Message: You have earned $s1.
        /// </summary>
        public static SystemMessage EARNED_ITEM_S1 = new SystemMessage(54);

        /// <summary>
        /// ID: 55
        /// <para/>
        /// Message: You have failed to pick up $s1 adena.
        /// </summary>
        public static SystemMessage FAILED_TO_PICKUP_S1_ADENA = new SystemMessage(55);

        /// <summary>
        /// ID: 56
        /// <para/>
        /// Message: You have failed to pick up $s1.
        /// </summary>
        public static SystemMessage FAILED_TO_PICKUP_S1 = new SystemMessage(56);

        /// <summary>
        /// ID: 57
        /// <para/>
        /// Message: You have failed to pick up $s2 $s1(s).
        /// </summary>
        public static SystemMessage FAILED_TO_PICKUP_S2_S1_S = new SystemMessage(57);

        /// <summary>
        /// ID: 58
        /// <para/>
        /// Message: You have failed to earn $s1 adena.
        /// </summary>
        public static SystemMessage FAILED_TO_EARN_S1_ADENA = new SystemMessage(58);

        /// <summary>
        /// ID: 59
        /// <para/>
        /// Message: You have failed to earn $s1.
        /// </summary>
        public static SystemMessage FAILED_TO_EARN_S1 = new SystemMessage(59);

        /// <summary>
        /// ID: 60
        /// <para/>
        /// Message: You have failed to earn $s2 $s1(s).
        /// </summary>
        public static SystemMessage FAILED_TO_EARN_S2_S1_S = new SystemMessage(60);

        /// <summary>
        /// ID: 61
        /// <para/>
        /// Message: Nothing happened.
        /// </summary>
        public static SystemMessage NOTHING_HAPPENED = new SystemMessage(61);

        /// <summary>
        /// ID: 62
        /// <para/>
        /// Message: Your $s1 has been successfully enchanted.
        /// </summary>
        public static SystemMessage S1_SUCCESSFULLY_ENCHANTED = new SystemMessage(62);

        /// <summary>
        /// ID: 63
        /// <para/>
        /// Message: Your +$S1 $S2 has been successfully enchanted.
        /// </summary>
        public static SystemMessage S1_S2_SUCCESSFULLY_ENCHANTED = new SystemMessage(63);

        /// <summary>
        /// ID: 64
        /// <para/>
        /// Message: The enchantment has failed! Your $s1 has been crystallized.
        /// </summary>
        public static SystemMessage ENCHANTMENT_FAILED_S1_EVAPORATED = new SystemMessage(64);

        /// <summary>
        /// ID: 65
        /// <para/>
        /// Message: The enchantment has failed! Your +$s1 $s2 has been crystallized.
        /// </summary>
        public static SystemMessage ENCHANTMENT_FAILED_S1_S2_EVAPORATED = new SystemMessage(65);

        /// <summary>
        /// ID: 66
        /// <para/>
        /// Message: $s1 is inviting you to join a party. Do you accept?
        /// </summary>
        public static SystemMessage S1_INVITED_YOU_TO_PARTY = new SystemMessage(66);

        /// <summary>
        /// ID: 67
        /// <para/>
        /// Message: $s1 has invited you to the join the clan, $s2. Do you wish to join?
        /// </summary>
        public static SystemMessage S1_HAS_INVITED_YOU_TO_JOIN_THE_CLAN_S2 = new SystemMessage(67);

        /// <summary>
        /// ID: 68
        /// <para/>
        /// Message: Would you like to withdraw from the $s1 clan? If you leave, you will have to wait at least a day before joining another clan.
        /// </summary>
        public static SystemMessage WOULD_YOU_LIKE_TO_WITHDRAW_FROM_THE_S1_CLAN = new SystemMessage(68);

        /// <summary>
        /// ID: 69
        /// <para/>
        /// Message: Would you like to dismiss $s1 from the clan? If you do so, you will have to wait at least a day before accepting a new member.
        /// </summary>
        public static SystemMessage WOULD_YOU_LIKE_TO_DISMISS_S1_FROM_THE_CLAN = new SystemMessage(69);

        /// <summary>
        /// ID: 70
        /// <para/>
        /// Message: Do you wish to disperse the clan, $s1?
        /// </summary>
        public static SystemMessage DO_YOU_WISH_TO_DISPERSE_THE_CLAN_S1 = new SystemMessage(70);

        /// <summary>
        /// ID: 71
        /// <para/>
        /// Message: How many of your $s1(s) do you wish to discard?
        /// </summary>
        public static SystemMessage HOW_MANY_S1_DISCARD = new SystemMessage(71);

        /// <summary>
        /// ID: 72
        /// <para/>
        /// Message: How many of your $s1(s) do you wish to move?
        /// </summary>
        public static SystemMessage HOW_MANY_S1_MOVE = new SystemMessage(72);

        /// <summary>
        /// ID: 73
        /// <para/>
        /// Message: How many of your $s1(s) do you wish to destroy?
        /// </summary>
        public static SystemMessage HOW_MANY_S1_DESTROY = new SystemMessage(73);

        /// <summary>
        /// ID: 74
        /// <para/>
        /// Message: Do you wish to destroy your $s1?
        /// </summary>
        public static SystemMessage WISH_DESTROY_S1 = new SystemMessage(74);

        /// <summary>
        /// ID: 75
        /// <para/>
        /// Message: ID does not exist.
        /// </summary>
        public static SystemMessage ID_NOT_EXIST = new SystemMessage(75);

        /// <summary>
        /// ID: 76
        /// <para/>
        /// Message: Incorrect password.
        /// </summary>
        public static SystemMessage INCORRECT_PASSWORD = new SystemMessage(76);

        /// <summary>
        /// ID: 77
        /// <para/>
        /// Message: You cannot create another character. Please delete the existing character and try again.
        /// </summary>
        public static SystemMessage CANNOT_CREATE_CHARACTER = new SystemMessage(77);

        /// <summary>
        /// ID: 78
        /// <para/>
        /// Message: When you delete a character, any items in his/her possession will also be deleted. Do you really wish to delete $s1%?
        /// </summary>
        public static SystemMessage WISH_DELETE_S1 = new SystemMessage(78);

        /// <summary>
        /// ID: 79
        /// <para/>
        /// Message: This name already exists.
        /// </summary>
        public static SystemMessage NAMING_NAME_ALREADY_EXISTS = new SystemMessage(79);

        /// <summary>
        /// ID: 80
        /// <para/>
        /// Message: Names must be between 1-16 characters, excluding spaces or special characters.
        /// </summary>
        public static SystemMessage NAMING_CHARNAME_UP_TO_16CHARS = new SystemMessage(80);

        /// <summary>
        /// ID: 81
        /// <para/>
        /// Message: Please select your race.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_RACE = new SystemMessage(81);

        /// <summary>
        /// ID: 82
        /// <para/>
        /// Message: Please select your occupation.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_OCCUPATION = new SystemMessage(82);

        /// <summary>
        /// ID: 83
        /// <para/>
        /// Message: Please select your gender.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_GENDER = new SystemMessage(83);

        /// <summary>
        /// ID: 84
        /// <para/>
        /// Message: You may not attack in a peaceful zone.
        /// </summary>
        public static SystemMessage CANT_ATK_PEACEZONE = new SystemMessage(84);

        /// <summary>
        /// ID: 85
        /// <para/>
        /// Message: You may not attack this target in a peaceful zone.
        /// </summary>
        public static SystemMessage TARGET_IN_PEACEZONE = new SystemMessage(85);

        /// <summary>
        /// ID: 86
        /// <para/>
        /// Message: Please enter your ID.
        /// </summary>
        public static SystemMessage PLEASE_ENTER_ID = new SystemMessage(86);

        /// <summary>
        /// ID: 87
        /// <para/>
        /// Message: Please enter your password.
        /// </summary>
        public static SystemMessage PLEASE_ENTER_PASSWORD = new SystemMessage(87);

        /// <summary>
        /// ID: 88
        /// <para/>
        /// Message: Your protocol version is different, please restart your client and run a full check.
        /// </summary>
        public static SystemMessage WRONG_PROTOCOL_CHECK = new SystemMessage(88);

        /// <summary>
        /// ID: 89
        /// <para/>
        /// Message: Your protocol version is different, please continue.
        /// </summary>
        public static SystemMessage WRONG_PROTOCOL_CONTINUE = new SystemMessage(89);

        /// <summary>
        /// ID: 90
        /// <para/>
        /// Message: You are unable to connect to the server.
        /// </summary>
        public static SystemMessage UNABLE_TO_CONNECT = new SystemMessage(90);

        /// <summary>
        /// ID: 91
        /// <para/>
        /// Message: Please select your hairstyle.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_HAIRSTYLE = new SystemMessage(91);

        /// <summary>
        /// ID: 92
        /// <para/>
        /// Message: $s1 has worn off.
        /// </summary>
        public static SystemMessage S1_HAS_WORN_OFF = new SystemMessage(92);

        /// <summary>
        /// ID: 93
        /// <para/>
        /// Message: You do not have enough SP for this.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SP = new SystemMessage(93);

        /// <summary>
        /// ID: 94
        /// <para/>
        /// Message: 2004-2009 (c) Copyright NCsoft Corporation. All Rights Reserved.
        /// </summary>
        public static SystemMessage COPYRIGHT = new SystemMessage(94);

        /// <summary>
        /// ID: 95
        /// <para/>
        /// Message: You have earned $s1 experience and $s2 SP.
        /// </summary>
        public static SystemMessage YOU_EARNED_S1_EXP_AND_S2_SP = new SystemMessage(95);

        /// <summary>
        /// ID: 96
        /// <para/>
        /// Message: Your level has increased!
        /// </summary>
        public static SystemMessage YOU_INCREASED_YOUR_LEVEL = new SystemMessage(96);

        /// <summary>
        /// ID: 97
        /// <para/>
        /// Message: This item cannot be moved.
        /// </summary>
        public static SystemMessage CANNOT_MOVE_THIS_ITEM = new SystemMessage(97);

        /// <summary>
        /// ID: 98
        /// <para/>
        /// Message: This item cannot be discarded.
        /// </summary>
        public static SystemMessage CANNOT_DISCARD_THIS_ITEM = new SystemMessage(98);

        /// <summary>
        /// ID: 99
        /// <para/>
        /// Message: This item cannot be traded or sold.
        /// </summary>
        public static SystemMessage CANNOT_TRADE_THIS_ITEM = new SystemMessage(99);

        /// <summary>
        /// ID: 100
        /// <para/>
        /// Message: $s1 is requesting to trade. Do you wish to continue?
        /// </summary>
        public static SystemMessage S1_REQUESTS_TRADE = new SystemMessage(100);

        /// <summary>
        /// ID: 101
        /// <para/>
        /// Message: You cannot exit while in combat.
        /// </summary>
        public static SystemMessage CANT_LOGOUT_WHILE_FIGHTING = new SystemMessage(101);

        /// <summary>
        /// ID: 102
        /// <para/>
        /// Message: You cannot restart while in combat.
        /// </summary>
        public static SystemMessage CANT_RESTART_WHILE_FIGHTING = new SystemMessage(102);

        /// <summary>
        /// ID: 103
        /// <para/>
        /// Message: This ID is currently logged in.
        /// </summary>
        public static SystemMessage ID_LOGGED_IN = new SystemMessage(103);

        /// <summary>
        /// ID: 104
        /// <para/>
        /// Message: You may not equip items while casting or performing a skill.
        /// </summary>
        public static SystemMessage CANNOT_USE_ITEM_WHILE_USING_MAGIC = new SystemMessage(104);

        /// <summary>
        /// ID: 105
        /// <para/>
        /// Message: You have invited $s1 to your party.
        /// </summary>
        public static SystemMessage YOU_INVITED_S1_TO_PARTY = new SystemMessage(105);

        /// <summary>
        /// ID: 106
        /// <para/>
        /// Message: You have joined $s1's party.
        /// </summary>
        public static SystemMessage YOU_JOINED_S1_PARTY = new SystemMessage(106);

        /// <summary>
        /// ID: 107
        /// <para/>
        /// Message: $s1 has joined the party.
        /// </summary>
        public static SystemMessage S1_JOINED_PARTY = new SystemMessage(107);

        /// <summary>
        /// ID: 108
        /// <para/>
        /// Message: $s1 has left the party.
        /// </summary>
        public static SystemMessage S1_LEFT_PARTY = new SystemMessage(108);

        /// <summary>
        /// ID: 109
        /// <para/>
        /// Message: Invalid target.
        /// </summary>
        public static SystemMessage INCORRECT_TARGET = new SystemMessage(109);

        /// <summary>
        /// ID: 110
        /// <para/>
        /// Message: $s1 $s2's effect can be felt.
        /// </summary>
        public static SystemMessage YOU_FEEL_S1_EFFECT = new SystemMessage(110);

        /// <summary>
        /// ID: 111
        /// <para/>
        /// Message: Your shield defense has succeeded.
        /// </summary>
        public static SystemMessage SHIELD_DEFENCE_SUCCESSFULL = new SystemMessage(111);

        /// <summary>
        /// ID: 112
        /// <para/>
        /// Message: You may no longer adjust items in the trade because the trade has been confirmed.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_ARROWS = new SystemMessage(112);

        /// <summary>
        /// ID: 113
        /// <para/>
        /// Message: $s1 cannot be used due to unsuitable terms.
        /// </summary>
        public static SystemMessage S1_CANNOT_BE_USED = new SystemMessage(113);

        /// <summary>
        /// ID: 114
        /// <para/>
        /// Message: You have entered the shadow of the Mother Tree.
        /// </summary>
        public static SystemMessage ENTER_SHADOW_MOTHER_TREE = new SystemMessage(114);

        /// <summary>
        /// ID: 115
        /// <para/>
        /// Message: You have left the shadow of the Mother Tree.
        /// </summary>
        public static SystemMessage EXIT_SHADOW_MOTHER_TREE = new SystemMessage(115);

        /// <summary>
        /// ID: 116
        /// <para/>
        /// Message: You have entered a peaceful zone.
        /// </summary>
        public static SystemMessage ENTER_PEACEFUL_ZONE = new SystemMessage(116);

        /// <summary>
        /// ID: 117
        /// <para/>
        /// Message: You have left the peaceful zone.
        /// </summary>
        public static SystemMessage EXIT_PEACEFUL_ZONE = new SystemMessage(117);

        /// <summary>
        /// ID: 118
        /// <para/>
        /// Message: You have requested a trade with $s1
        /// </summary>
        public static SystemMessage REQUEST_S1_FOR_TRADE = new SystemMessage(118);

        /// <summary>
        /// ID: 119
        /// <para/>
        /// Message: $s1 has denied your request to trade.
        /// </summary>
        public static SystemMessage S1_DENIED_TRADE_REQUEST = new SystemMessage(119);

        /// <summary>
        /// ID: 120
        /// <para/>
        /// Message: You begin trading with $s1.
        /// </summary>
        public static SystemMessage BEGIN_TRADE_WITH_S1 = new SystemMessage(120);

        /// <summary>
        /// ID: 121
        /// <para/>
        /// Message: $s1 has confirmed the trade.
        /// </summary>
        public static SystemMessage S1_CONFIRMED_TRADE = new SystemMessage(121);

        /// <summary>
        /// ID: 122
        /// <para/>
        /// Message: You may no longer adjust items in the trade because the trade has been confirmed.
        /// </summary>
        public static SystemMessage CANNOT_ADJUST_ITEMS_AFTER_TRADE_CONFIRMED = new SystemMessage(122);

        /// <summary>
        /// ID: 123
        /// <para/>
        /// Message: Your trade is successful.
        /// </summary>
        public static SystemMessage TRADE_SUCCESSFUL = new SystemMessage(123);

        /// <summary>
        /// ID: 124
        /// <para/>
        /// Message: $s1 has cancelled the trade.
        /// </summary>
        public static SystemMessage S1_CANCELED_TRADE = new SystemMessage(124);

        /// <summary>
        /// ID: 125
        /// <para/>
        /// Message: Do you wish to exit the game?
        /// </summary>
        public static SystemMessage WISH_EXIT_GAME = new SystemMessage(125);

        /// <summary>
        /// ID: 126
        /// <para/>
        /// Message: Do you wish to return to the character select screen?
        /// </summary>
        public static SystemMessage WISH_RESTART_GAME = new SystemMessage(126);

        /// <summary>
        /// ID: 127
        /// <para/>
        /// Message: You have been disconnected from the server. Please login again.
        /// </summary>
        public static SystemMessage DISCONNECTED_FROM_SERVER = new SystemMessage(127);

        /// <summary>
        /// ID: 128
        /// <para/>
        /// Message: Your character creation has failed.
        /// </summary>
        public static SystemMessage CHARACTER_CREATION_FAILED = new SystemMessage(128);

        /// <summary>
        /// ID: 129
        /// <para/>
        /// Message: Your inventory is full.
        /// </summary>
        public static SystemMessage SLOTS_FULL = new SystemMessage(129);

        /// <summary>
        /// ID: 130
        /// <para/>
        /// Message: Your warehouse is full.
        /// </summary>
        public static SystemMessage WAREHOUSE_FULL = new SystemMessage(130);

        /// <summary>
        /// ID: 131
        /// <para/>
        /// Message: $s1 has logged in.
        /// </summary>
        public static SystemMessage S1_LOGGED_IN = new SystemMessage(131);

        /// <summary>
        /// ID: 132
        /// <para/>
        /// Message: $s1 has been added to your friends list.
        /// </summary>
        public static SystemMessage S1_ADDED_TO_FRIENDS = new SystemMessage(132);

        /// <summary>
        /// ID: 133
        /// <para/>
        /// Message: $s1 has been removed from your friends list.
        /// </summary>
        public static SystemMessage S1_REMOVED_FROM_YOUR_FRIENDS_LIST = new SystemMessage(133);

        /// <summary>
        /// ID: 134
        /// <para/>
        /// Message: Please check your friends list again.
        /// </summary>
        public static SystemMessage PLEACE_CHECK_YOUR_FRIEND_LIST_AGAIN = new SystemMessage(134);

        /// <summary>
        /// ID: 135
        /// <para/>
        /// Message: $s1 did not reply to your invitation. Your invitation has been cancelled.
        /// </summary>
        public static SystemMessage S1_DID_NOT_REPLY_TO_YOUR_INVITE = new SystemMessage(135);

        /// <summary>
        /// ID: 136
        /// <para/>
        /// Message: You have not replied to $s1's invitation. The offer has been cancelled.
        /// </summary>
        public static SystemMessage YOU_DID_NOT_REPLY_TO_S1_INVITE = new SystemMessage(136);

        /// <summary>
        /// ID: 137
        /// <para/>
        /// Message: There are no more items in the shortcut.
        /// </summary>
        public static SystemMessage NO_MORE_ITEMS_SHORTCUT = new SystemMessage(137);

        /// <summary>
        /// ID: 138
        /// <para/>
        /// Message: Designate shortcut.
        /// </summary>
        public static SystemMessage DESIGNATE_SHORTCUT = new SystemMessage(138);

        /// <summary>
        /// ID: 139
        /// <para/>
        /// Message: $s1 has resisted your $s2.
        /// </summary>
        public static SystemMessage S1_RESISTED_YOUR_S2 = new SystemMessage(139);

        /// <summary>
        /// ID: 140
        /// <para/>
        /// Message: Your skill was removed due to a lack of MP.
        /// </summary>
        public static SystemMessage SKILL_REMOVED_DUE_LACK_MP = new SystemMessage(140);

        /// <summary>
        /// ID: 141
        /// <para/>
        /// Message: Once the trade is confirmed, the item cannot be moved again.
        /// </summary>
        public static SystemMessage ONCE_THE_TRADE_IS_CONFIRMED_THE_ITEM_CANNOT_BE_MOVED_AGAIN = new SystemMessage(141);

        /// <summary>
        /// ID: 142
        /// <para/>
        /// Message: You are already trading with someone.
        /// </summary>
        public static SystemMessage ALREADY_TRADING = new SystemMessage(142);

        /// <summary>
        /// ID: 143
        /// <para/>
        /// Message: $s1 is already trading with another person. Please try again later.
        /// </summary>
        public static SystemMessage S1_ALREADY_TRADING = new SystemMessage(143);

        /// <summary>
        /// ID: 144
        /// <para/>
        /// Message: That is the incorrect target.
        /// </summary>
        public static SystemMessage TARGET_IS_INCORRECT = new SystemMessage(144);

        /// <summary>
        /// ID: 145
        /// <para/>
        /// Message: That player is not online.
        /// </summary>
        public static SystemMessage TARGET_IS_NOT_FOUND_IN_THE_GAME = new SystemMessage(145);

        /// <summary>
        /// ID: 146
        /// <para/>
        /// Message: Chatting is now permitted.
        /// </summary>
        public static SystemMessage CHATTING_PERMITTED = new SystemMessage(146);

        /// <summary>
        /// ID: 147
        /// <para/>
        /// Message: Chatting is currently prohibited.
        /// </summary>
        public static SystemMessage CHATTING_PROHIBITED = new SystemMessage(147);

        /// <summary>
        /// ID: 148
        /// <para/>
        /// Message: You cannot use quest items.
        /// </summary>
        public static SystemMessage CANNOT_USE_QUEST_ITEMS = new SystemMessage(148);

        /// <summary>
        /// ID: 149
        /// <para/>
        /// Message: You cannot pick up or use items while trading.
        /// </summary>
        public static SystemMessage CANNOT_PICKUP_OR_USE_ITEM_WHILE_TRADING = new SystemMessage(149);

        /// <summary>
        /// ID: 150
        /// <para/>
        /// Message: You cannot discard or destroy an item while trading at a private store.
        /// </summary>
        public static SystemMessage CANNOT_DISCARD_OR_DESTROY_ITEM_WHILE_TRADING = new SystemMessage(150);

        /// <summary>
        /// ID: 151
        /// <para/>
        /// Message: That is too far from you to discard.
        /// </summary>
        public static SystemMessage CANNOT_DISCARD_DISTANCE_TOO_FAR = new SystemMessage(151);

        /// <summary>
        /// ID: 152
        /// <para/>
        /// Message: You have invited the wrong target.
        /// </summary>
        public static SystemMessage YOU_HAVE_INVITED_THE_WRONG_TARGET = new SystemMessage(152);

        /// <summary>
        /// ID: 153
        /// <para/>
        /// Message: $s1 is on another task. Please try again later.
        /// </summary>
        public static SystemMessage S1_IS_BUSY_TRY_LATER = new SystemMessage(153);

        /// <summary>
        /// ID: 154
        /// <para/>
        /// Message: Only the leader can give out invitations.
        /// </summary>
        public static SystemMessage ONLY_LEADER_CAN_INVITE = new SystemMessage(154);

        /// <summary>
        /// ID: 155
        /// <para/>
        /// Message: The party is full.
        /// </summary>
        public static SystemMessage PARTY_FULL = new SystemMessage(155);

        /// <summary>
        /// ID: 156
        /// <para/>
        /// Message: Drain was only 50 percent successful.
        /// </summary>
        public static SystemMessage DRAIN_HALF_SUCCESFUL = new SystemMessage(156);

        /// <summary>
        /// ID: 157
        /// <para/>
        /// Message: You resisted $s1's drain.
        /// </summary>
        public static SystemMessage RESISTED_S1_DRAIN = new SystemMessage(157);

        /// <summary>
        /// ID: 158
        /// <para/>
        /// Message: Your attack has failed.
        /// </summary>
        public static SystemMessage ATTACK_FAILED = new SystemMessage(158);

        /// <summary>
        /// ID: 159
        /// <para/>
        /// Message: You resisted $s1's magic.
        /// </summary>
        public static SystemMessage RESISTED_S1_MAGIC = new SystemMessage(159);

        /// <summary>
        /// ID: 160
        /// <para/>
        /// Message: $s1 is a member of another party and cannot be invited.
        /// </summary>
        public static SystemMessage S1_IS_ALREADY_IN_PARTY = new SystemMessage(160);

        /// <summary>
        /// ID: 161
        /// <para/>
        /// Message: That player is not currently online.
        /// </summary>
        public static SystemMessage INVITED_USER_NOT_ONLINE = new SystemMessage(161);

        /// <summary>
        /// ID: 162
        /// <para/>
        /// Message: Warehouse is too far.
        /// </summary>
        public static SystemMessage WAREHOUSE_TOO_FAR = new SystemMessage(162);

        /// <summary>
        /// ID: 163
        /// <para/>
        /// Message: You cannot destroy it because the number is incorrect.
        /// </summary>
        public static SystemMessage CANNOT_DESTROY_NUMBER_INCORRECT = new SystemMessage(163);

        /// <summary>
        /// ID: 164
        /// <para/>
        /// Message: Waiting for another reply.
        /// </summary>
        public static SystemMessage WAITING_FOR_ANOTHER_REPLY = new SystemMessage(164);

        /// <summary>
        /// ID: 165
        /// <para/>
        /// Message: You cannot add yourself to your own friend list.
        /// </summary>
        public static SystemMessage YOU_CANNOT_ADD_YOURSELF_TO_OWN_FRIEND_LIST = new SystemMessage(165);

        /// <summary>
        /// ID: 166
        /// <para/>
        /// Message: Friend list is not ready yet. Please register again later.
        /// </summary>
        public static SystemMessage FRIEND_LIST_NOT_READY_YET_REGISTER_LATER = new SystemMessage(166);

        /// <summary>
        /// ID: 167
        /// <para/>
        /// Message: $s1 is already on your friend list.
        /// </summary>
        public static SystemMessage S1_ALREADY_ON_FRIEND_LIST = new SystemMessage(167);

        /// <summary>
        /// ID: 168
        /// <para/>
        /// Message: $s1 has sent a friend request.
        /// </summary>
        public static SystemMessage S1_REQUESTED_TO_BECOME_FRIENDS = new SystemMessage(168);

        /// <summary>
        /// ID: 169
        /// <para/>
        /// Message: Accept friendship 0/1 (1 to accept, 0 to deny)
        /// </summary>
        public static SystemMessage ACCEPT_THE_FRIENDSHIP = new SystemMessage(169);

        /// <summary>
        /// ID: 170
        /// <para/>
        /// Message: The user who requested to become friends is not found in the game.
        /// </summary>
        public static SystemMessage THE_USER_YOU_REQUESTED_IS_NOT_IN_GAME = new SystemMessage(170);

        /// <summary>
        /// ID: 171
        /// <para/>
        /// Message: $s1 is not on your friend list.
        /// </summary>
        public static SystemMessage S1_NOT_ON_YOUR_FRIENDS_LIST = new SystemMessage(171);

        /// <summary>
        /// ID: 172
        /// <para/>
        /// Message: You lack the funds needed to pay for this transaction.
        /// </summary>
        public static SystemMessage LACK_FUNDS_FOR_TRANSACTION1 = new SystemMessage(172);

        /// <summary>
        /// ID: 173
        /// <para/>
        /// Message: You lack the funds needed to pay for this transaction.
        /// </summary>
        public static SystemMessage LACK_FUNDS_FOR_TRANSACTION2 = new SystemMessage(173);

        /// <summary>
        /// ID: 174
        /// <para/>
        /// Message: That person's inventory is full.
        /// </summary>
        public static SystemMessage OTHER_INVENTORY_FULL = new SystemMessage(174);

        /// <summary>
        /// ID: 175
        /// <para/>
        /// Message: That skill has been de-activated as HP was fully recovered.
        /// </summary>
        public static SystemMessage SKILL_DEACTIVATED_HP_FULL = new SystemMessage(175);

        /// <summary>
        /// ID: 176
        /// <para/>
        /// Message: That person is in message refusal mode.
        /// </summary>
        public static SystemMessage THE_PERSON_IS_IN_MESSAGE_REFUSAL_MODE = new SystemMessage(176);

        /// <summary>
        /// ID: 177
        /// <para/>
        /// Message: Message refusal mode.
        /// </summary>
        public static SystemMessage MESSAGE_REFUSAL_MODE = new SystemMessage(177);

        /// <summary>
        /// ID: 178
        /// <para/>
        /// Message: Message acceptance mode.
        /// </summary>
        public static SystemMessage MESSAGE_ACCEPTANCE_MODE = new SystemMessage(178);

        /// <summary>
        /// ID: 179
        /// <para/>
        /// Message: You cannot discard those items here.
        /// </summary>
        public static SystemMessage CANT_DISCARD_HERE = new SystemMessage(179);

        /// <summary>
        /// ID: 180
        /// <para/>
        /// Message: You have $s1 day(s) left until deletion. Do you wish to cancel this action?
        /// </summary>
        public static SystemMessage S1_DAYS_LEFT_CANCEL_ACTION = new SystemMessage(180);

        /// <summary>
        /// ID: 181
        /// <para/>
        /// Message: Cannot see target.
        /// </summary>
        public static SystemMessage CANT_SEE_TARGET = new SystemMessage(181);

        /// <summary>
        /// ID: 182
        /// <para/>
        /// Message: Do you want to quit the current quest?
        /// </summary>
        public static SystemMessage WANT_QUIT_CURRENT_QUEST = new SystemMessage(182);

        /// <summary>
        /// ID: 183
        /// <para/>
        /// Message: There are too many users on the server. Please try again later
        /// </summary>
        public static SystemMessage TOO_MANY_USERS = new SystemMessage(183);

        /// <summary>
        /// ID: 184
        /// <para/>
        /// Message: Please try again later.
        /// </summary>
        public static SystemMessage TRY_AGAIN_LATER = new SystemMessage(184);

        /// <summary>
        /// ID: 185
        /// <para/>
        /// Message: You must first select a user to invite to your party.
        /// </summary>
        public static SystemMessage FIRST_SELECT_USER_TO_INVITE_TO_PARTY = new SystemMessage(185);

        /// <summary>
        /// ID: 186
        /// <para/>
        /// Message: You must first select a user to invite to your clan.
        /// </summary>
        public static SystemMessage FIRST_SELECT_USER_TO_INVITE_TO_CLAN = new SystemMessage(186);

        /// <summary>
        /// ID: 187
        /// <para/>
        /// Message: Select user to expel.
        /// </summary>
        public static SystemMessage SELECT_USER_TO_EXPEL = new SystemMessage(187);

        /// <summary>
        /// ID: 188
        /// <para/>
        /// Message: Please create your clan name.
        /// </summary>
        public static SystemMessage PLEASE_CREATE_CLAN_NAME = new SystemMessage(188);

        /// <summary>
        /// ID: 189
        /// <para/>
        /// Message: Your clan has been created.
        /// </summary>
        public static SystemMessage CLAN_CREATED = new SystemMessage(189);

        /// <summary>
        /// ID: 190
        /// <para/>
        /// Message: You have failed to create a clan.
        /// </summary>
        public static SystemMessage FAILED_TO_CREATE_CLAN = new SystemMessage(190);

        /// <summary>
        /// ID: 191
        /// <para/>
        /// Message: Clan member $s1 has been expelled.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_EXPELLED = new SystemMessage(191);

        /// <summary>
        /// ID: 192
        /// <para/>
        /// Message: You have failed to expel $s1 from the clan.
        /// </summary>
        public static SystemMessage FAILED_EXPEL_S1 = new SystemMessage(192);

        /// <summary>
        /// ID: 193
        /// <para/>
        /// Message: Clan has dispersed.
        /// </summary>
        public static SystemMessage CLAN_HAS_DISPERSED = new SystemMessage(193);

        /// <summary>
        /// ID: 194
        /// <para/>
        /// Message: You have failed to disperse the clan.
        /// </summary>
        public static SystemMessage FAILED_TO_DISPERSE_CLAN = new SystemMessage(194);

        /// <summary>
        /// ID: 195
        /// <para/>
        /// Message: Entered the clan.
        /// </summary>
        public static SystemMessage ENTERED_THE_CLAN = new SystemMessage(195);

        /// <summary>
        /// ID: 196
        /// <para/>
        /// Message: $s1 declined your clan invitation.
        /// </summary>
        public static SystemMessage S1_REFUSED_TO_JOIN_CLAN = new SystemMessage(196);

        /// <summary>
        /// ID: 197
        /// <para/>
        /// Message: You have withdrawn from the clan.
        /// </summary>
        public static SystemMessage YOU_HAVE_WITHDRAWN_FROM_CLAN = new SystemMessage(197);

        /// <summary>
        /// ID: 198
        /// <para/>
        /// Message: You have failed to withdraw from the $s1 clan.
        /// </summary>
        public static SystemMessage FAILED_TO_WITHDRAW_FROM_S1_CLAN = new SystemMessage(198);

        /// <summary>
        /// ID: 199
        /// <para/>
        /// Message: You have recently been dismissed from a clan. You are not allowed to join another clan for 24-hours.
        /// </summary>
        public static SystemMessage CLAN_MEMBERSHIP_TERMINATED = new SystemMessage(199);

        /// <summary>
        /// ID: 200
        /// <para/>
        /// Message: You have withdrawn from the party.
        /// </summary>
        public static SystemMessage YOU_LEFT_PARTY = new SystemMessage(200);

        /// <summary>
        /// ID: 201
        /// <para/>
        /// Message: $s1 was expelled from the party.
        /// </summary>
        public static SystemMessage S1_WAS_EXPELLED_FROM_PARTY = new SystemMessage(201);

        /// <summary>
        /// ID: 202
        /// <para/>
        /// Message: You have been expelled from the party.
        /// </summary>
        public static SystemMessage HAVE_BEEN_EXPELLED_FROM_PARTY = new SystemMessage(202);

        /// <summary>
        /// ID: 203
        /// <para/>
        /// Message: The party has dispersed.
        /// </summary>
        public static SystemMessage PARTY_DISPERSED = new SystemMessage(203);

        /// <summary>
        /// ID: 204
        /// <para/>
        /// Message: Incorrect name. Please try again.
        /// </summary>
        public static SystemMessage INCORRECT_NAME_TRY_AGAIN = new SystemMessage(204);

        /// <summary>
        /// ID: 205
        /// <para/>
        /// Message: Incorrect character name. Please try again.
        /// </summary>
        public static SystemMessage INCORRECT_CHARACTER_NAME_TRY_AGAIN = new SystemMessage(205);

        /// <summary>
        /// ID: 206
        /// <para/>
        /// Message: Please enter the name of the clan you wish to declare war on.
        /// </summary>
        public static SystemMessage ENTER_CLAN_NAME_TO_DECLARE_WAR = new SystemMessage(206);

        /// <summary>
        /// ID: 207
        /// <para/>
        /// Message: $s2 of the clan $s1 requests declaration of war. Do you accept?
        /// </summary>
        public static SystemMessage S2_OF_THE_CLAN_S1_REQUESTS_WAR = new SystemMessage(207);

        /// <summary>
        /// ID: 212
        /// <para/>
        /// Message: You are not a clan member and cannot perform this action.
        /// </summary>
        public static SystemMessage YOU_ARE_NOT_A_CLAN_MEMBER = new SystemMessage(212);

        /// <summary>
        /// ID: 213
        /// <para/>
        /// Message: Not working. Please try again later.
        /// </summary>
        public static SystemMessage NOT_WORKING_PLEASE_TRY_AGAIN_LATER = new SystemMessage(213);

        /// <summary>
        /// ID: 214
        /// <para/>
        /// Message: Your title has been changed.
        /// </summary>
        public static SystemMessage TITLE_CHANGED = new SystemMessage(214);

        /// <summary>
        /// ID: 215
        /// <para/>
        /// Message: War with the $s1 clan has begun.
        /// </summary>
        public static SystemMessage WAR_WITH_THE_S1_CLAN_HAS_BEGUN = new SystemMessage(215);

        /// <summary>
        /// ID: 216
        /// <para/>
        /// Message: War with the $s1 clan has ended.
        /// </summary>
        public static SystemMessage WAR_WITH_THE_S1_CLAN_HAS_ENDED = new SystemMessage(216);

        /// <summary>
        /// ID: 217
        /// <para/>
        /// Message: You have won the war over the $s1 clan!
        /// </summary>
        public static SystemMessage YOU_HAVE_WON_THE_WAR_OVER_THE_S1_CLAN = new SystemMessage(217);

        /// <summary>
        /// ID: 218
        /// <para/>
        /// Message: You have surrendered to the $s1 clan.
        /// </summary>
        public static SystemMessage YOU_HAVE_SURRENDERED_TO_THE_S1_CLAN = new SystemMessage(218);

        /// <summary>
        /// ID: 219
        /// <para/>
        /// Message: Your clan leader has died. You have been defeated by the $s1 clan.
        /// </summary>
        public static SystemMessage YOU_WERE_DEFEATED_BY_S1_CLAN = new SystemMessage(219);

        /// <summary>
        /// ID: 220
        /// <para/>
        /// Message: You have $s1 minutes left until the clan war ends.
        /// </summary>
        public static SystemMessage S1_MINUTES_LEFT_UNTIL_CLAN_WAR_ENDS = new SystemMessage(220);

        /// <summary>
        /// ID: 221
        /// <para/>
        /// Message: The time limit for the clan war is up. War with the $s1 clan is over.
        /// </summary>
        public static SystemMessage CLAN_WAR_WITH_S1_CLAN_HAS_ENDED = new SystemMessage(221);

        /// <summary>
        /// ID: 222
        /// <para/>
        /// Message: $s1 has joined the clan.
        /// </summary>
        public static SystemMessage S1_HAS_JOINED_CLAN = new SystemMessage(222);

        /// <summary>
        /// ID: 223
        /// <para/>
        /// Message: $s1 has withdrawn from the clan.
        /// </summary>
        public static SystemMessage S1_HAS_WITHDRAWN_FROM_THE_CLAN = new SystemMessage(223);

        /// <summary>
        /// ID: 224
        /// <para/>
        /// Message: $s1 did not respond: Invitation to the clan has been cancelled.
        /// </summary>
        public static SystemMessage S1_DID_NOT_RESPOND_TO_CLAN_INVITATION = new SystemMessage(224);

        /// <summary>
        /// ID: 225
        /// <para/>
        /// Message: You didn't respond to $s1's invitation: joining has been cancelled.
        /// </summary>
        public static SystemMessage YOU_DID_NOT_RESPOND_TO_S1_CLAN_INVITATION = new SystemMessage(225);

        /// <summary>
        /// ID: 226
        /// <para/>
        /// Message: The $s1 clan did not respond: war proclamation has been refused.
        /// </summary>
        public static SystemMessage S1_CLAN_DID_NOT_RESPOND = new SystemMessage(226);

        /// <summary>
        /// ID: 227
        /// <para/>
        /// Message: Clan war has been refused because you did not respond to $s1 clan's war proclamation.
        /// </summary>
        public static SystemMessage CLAN_WAR_REFUSED_YOU_DID_NOT_RESPOND_TO_S1 = new SystemMessage(227);

        /// <summary>
        /// ID: 228
        /// <para/>
        /// Message: Request to end war has been denied.
        /// </summary>
        public static SystemMessage REQUEST_TO_END_WAR_HAS_BEEN_DENIED = new SystemMessage(228);

        /// <summary>
        /// ID: 229
        /// <para/>
        /// Message: You do not meet the criteria in order to create a clan.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_CLAN = new SystemMessage(229);

        /// <summary>
        /// ID: 230
        /// <para/>
        /// Message: You must wait 10 days before creating a new clan.
        /// </summary>
        public static SystemMessage YOU_MUST_WAIT_XX_DAYS_BEFORE_CREATING_A_NEW_CLAN = new SystemMessage(230);

        /// <summary>
        /// ID: 231
        /// <para/>
        /// Message: After a clan member is dismissed from a clan, the clan must wait at least a day before accepting a new member.
        /// </summary>
        public static SystemMessage YOU_MUST_WAIT_BEFORE_ACCEPTING_A_NEW_MEMBER = new SystemMessage(231);

        /// <summary>
        /// ID: 232
        /// <para/>
        /// Message: After leaving or having been dismissed from a clan, you must wait at least a day before joining another clan.
        /// </summary>
        public static SystemMessage YOU_MUST_WAIT_BEFORE_JOINING_ANOTHER_CLAN = new SystemMessage(232);

        /// <summary>
        /// ID: 233
        /// <para/>
        /// Message: The Academy/Royal Guard/Order of Knights is full and cannot accept new members at this time.
        /// </summary>
        public static SystemMessage SUBCLAN_IS_FULL = new SystemMessage(233);

        /// <summary>
        /// ID: 234
        /// <para/>
        /// Message: The target must be a clan member.
        /// </summary>
        public static SystemMessage TARGET_MUST_BE_IN_CLAN = new SystemMessage(234);

        /// <summary>
        /// ID: 235
        /// <para/>
        /// Message: You are not authorized to bestow these rights.
        /// </summary>
        public static SystemMessage NOT_AUTHORIZED_TO_BESTOW_RIGHTS = new SystemMessage(235);

        /// <summary>
        /// ID: 236
        /// <para/>
        /// Message: Only the clan leader is enabled.
        /// </summary>
        public static SystemMessage ONLY_THE_CLAN_LEADER_IS_ENABLED = new SystemMessage(236);

        /// <summary>
        /// ID: 237
        /// <para/>
        /// Message: The clan leader could not be found.
        /// </summary>
        public static SystemMessage CLAN_LEADER_NOT_FOUND = new SystemMessage(237);

        /// <summary>
        /// ID: 238
        /// <para/>
        /// Message: Not joined in any clan.
        /// </summary>
        public static SystemMessage NOT_JOINED_IN_ANY_CLAN = new SystemMessage(238);

        /// <summary>
        /// ID: 239
        /// <para/>
        /// Message: The clan leader cannot withdraw.
        /// </summary>
        public static SystemMessage CLAN_LEADER_CANNOT_WITHDRAW = new SystemMessage(239);

        /// <summary>
        /// ID: 240
        /// <para/>
        /// Message: Currently involved in clan war.
        /// </summary>
        public static SystemMessage CURRENTLY_INVOLVED_IN_CLAN_WAR = new SystemMessage(240);

        /// <summary>
        /// ID: 241
        /// <para/>
        /// Message: Leader of the $s1 Clan is not logged in.
        /// </summary>
        public static SystemMessage LEADER_OF_S1_CLAN_NOT_FOUND = new SystemMessage(241);

        /// <summary>
        /// ID: 242
        /// <para/>
        /// Message: Select target.
        /// </summary>
        public static SystemMessage SELECT_TARGET = new SystemMessage(242);

        /// <summary>
        /// ID: 243
        /// <para/>
        /// Message: You cannot declare war on an allied clan.
        /// </summary>
        public static SystemMessage CANNOT_DECLARE_WAR_ON_ALLIED_CLAN = new SystemMessage(243);

        /// <summary>
        /// ID: 244
        /// <para/>
        /// Message: You are not allowed to issue this challenge.
        /// </summary>
        public static SystemMessage NOT_ALLOWED_TO_CHALLENGE = new SystemMessage(244);

        /// <summary>
        /// ID: 245
        /// <para/>
        /// Message: 5 days has not passed since you were refused war. Do you wish to continue?
        /// </summary>
        public static SystemMessage FIVE_DAYS_NOT_PASSED_SINCE_REFUSED_WAR = new SystemMessage(245);

        /// <summary>
        /// ID: 246
        /// <para/>
        /// Message: That clan is currently at war.
        /// </summary>
        public static SystemMessage CLAN_CURRENTLY_AT_WAR = new SystemMessage(246);

        /// <summary>
        /// ID: 247
        /// <para/>
        /// Message: You have already been at war with the $s1 clan: 5 days must pass before you can challenge this clan again
        /// </summary>
        public static SystemMessage FIVE_DAYS_MUST_PASS_BEFORE_CHALLENGE_S1_AGAIN = new SystemMessage(247);

        /// <summary>
        /// ID: 248
        /// <para/>
        /// Message: You cannot proclaim war: the $s1 clan does not have enough members.
        /// </summary>
        public static SystemMessage S1_CLAN_NOT_ENOUGH_MEMBERS_FOR_WAR = new SystemMessage(248);

        /// <summary>
        /// ID: 249
        /// <para/>
        /// Message: Do you wish to surrender to the $s1 clan?
        /// </summary>
        public static SystemMessage WISH_SURRENDER_TO_S1_CLAN = new SystemMessage(249);

        /// <summary>
        /// ID: 250
        /// <para/>
        /// Message: You have personally surrendered to the $s1 clan. You are no longer participating in this clan war.
        /// </summary>
        public static SystemMessage YOU_HAVE_PERSONALLY_SURRENDERED_TO_THE_S1_CLAN = new SystemMessage(250);

        /// <summary>
        /// ID: 251
        /// <para/>
        /// Message: You cannot proclaim war: you are at war with another clan.
        /// </summary>
        public static SystemMessage ALREADY_AT_WAR_WITH_ANOTHER_CLAN = new SystemMessage(251);

        /// <summary>
        /// ID: 252
        /// <para/>
        /// Message: Enter the clan name to surrender to.
        /// </summary>
        public static SystemMessage ENTER_CLAN_NAME_TO_SURRENDER_TO = new SystemMessage(252);

        /// <summary>
        /// ID: 253
        /// <para/>
        /// Message: Enter the name of the clan you wish to end the war with.
        /// </summary>
        public static SystemMessage ENTER_CLAN_NAME_TO_END_WAR = new SystemMessage(253);

        /// <summary>
        /// ID: 254
        /// <para/>
        /// Message: A clan leader cannot personally surrender.
        /// </summary>
        public static SystemMessage LEADER_CANT_PERSONALLY_SURRENDER = new SystemMessage(254);

        /// <summary>
        /// ID: 255
        /// <para/>
        /// Message: The $s1 clan has requested to end war. Do you agree?
        /// </summary>
        public static SystemMessage S1_CLAN_REQUESTED_END_WAR = new SystemMessage(255);

        /// <summary>
        /// ID: 256
        /// <para/>
        /// Message: Enter title
        /// </summary>
        public static SystemMessage ENTER_TITLE = new SystemMessage(256);

        /// <summary>
        /// ID: 257
        /// <para/>
        /// Message: Do you offer the $s1 clan a proposal to end the war?
        /// </summary>
        public static SystemMessage DO_YOU_OFFER_S1_CLAN_END_WAR = new SystemMessage(257);

        /// <summary>
        /// ID: 258
        /// <para/>
        /// Message: You are not involved in a clan war.
        /// </summary>
        public static SystemMessage NOT_INVOLVED_CLAN_WAR = new SystemMessage(258);

        /// <summary>
        /// ID: 259
        /// <para/>
        /// Message: Select clan members from list.
        /// </summary>
        public static SystemMessage SELECT_MEMBERS_FROM_LIST = new SystemMessage(259);

        /// <summary>
        /// ID: 260
        /// <para/>
        /// Message: Fame level has decreased: 5 days have not passed since you were refused war
        /// </summary>
        public static SystemMessage FIVE_DAYS_NOT_PASSED_SINCE_YOU_WERE_REFUSED_WAR = new SystemMessage(260);

        /// <summary>
        /// ID: 261
        /// <para/>
        /// Message: Clan name is invalid.
        /// </summary>
        public static SystemMessage CLAN_NAME_INVALID = new SystemMessage(261);

        /// <summary>
        /// ID: 262
        /// <para/>
        /// Message: Clan name's length is incorrect.
        /// </summary>
        public static SystemMessage CLAN_NAME_LENGTH_INCORRECT = new SystemMessage(262);

        /// <summary>
        /// ID: 263
        /// <para/>
        /// Message: You have already requested the dissolution of your clan.
        /// </summary>
        public static SystemMessage DISSOLUTION_IN_PROGRESS = new SystemMessage(263);

        /// <summary>
        /// ID: 264
        /// <para/>
        /// Message: You cannot dissolve a clan while engaged in a war.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_WHILE_IN_WAR = new SystemMessage(264);

        /// <summary>
        /// ID: 265
        /// <para/>
        /// Message: You cannot dissolve a clan during a siege or while protecting a castle.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_WHILE_IN_SIEGE = new SystemMessage(265);

        /// <summary>
        /// ID: 266
        /// <para/>
        /// Message: You cannot dissolve a clan while owning a clan hall or castle.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_WHILE_OWNING_CLAN_HALL_OR_CASTLE = new SystemMessage(266);

        /// <summary>
        /// ID: 267
        /// <para/>
        /// Message: There are no requests to disperse.
        /// </summary>
        public static SystemMessage NO_REQUESTS_TO_DISPERSE = new SystemMessage(267);

        /// <summary>
        /// ID: 268
        /// <para/>
        /// Message: That player already belongs to another clan.
        /// </summary>
        public static SystemMessage PLAYER_ALREADY_ANOTHER_CLAN = new SystemMessage(268);

        /// <summary>
        /// ID: 269
        /// <para/>
        /// Message: You cannot dismiss yourself.
        /// </summary>
        public static SystemMessage YOU_CANNOT_DISMISS_YOURSELF = new SystemMessage(269);

        /// <summary>
        /// ID: 270
        /// <para/>
        /// Message: You have already surrendered.
        /// </summary>
        public static SystemMessage YOU_HAVE_ALREADY_SURRENDERED = new SystemMessage(270);

        /// <summary>
        /// ID: 271
        /// <para/>
        /// Message: A player can only be granted a title if the clan is level 3 or above
        /// </summary>
        public static SystemMessage CLAN_LVL_3_NEEDED_TO_ENDOWE_TITLE = new SystemMessage(271);

        /// <summary>
        /// ID: 272
        /// <para/>
        /// Message: A clan crest can only be registered when the clan's skill level is 3 or above.
        /// </summary>
        public static SystemMessage CLAN_LVL_3_NEEDED_TO_SET_CREST = new SystemMessage(272);

        /// <summary>
        /// ID: 273
        /// <para/>
        /// Message: A clan war can only be declared when a clan's skill level is 3 or above.
        /// </summary>
        public static SystemMessage CLAN_LVL_3_NEEDED_TO_DECLARE_WAR = new SystemMessage(273);

        /// <summary>
        /// ID: 274
        /// <para/>
        /// Message: Your clan's skill level has increased.
        /// </summary>
        public static SystemMessage CLAN_LEVEL_INCREASED = new SystemMessage(274);

        /// <summary>
        /// ID: 275
        /// <para/>
        /// Message: Clan has failed to increase skill level.
        /// </summary>
        public static SystemMessage CLAN_LEVEL_INCREASE_FAILED = new SystemMessage(275);

        /// <summary>
        /// ID: 276
        /// <para/>
        /// Message: You do not have the necessary materials or prerequisites to learn this skill.
        /// </summary>
        public static SystemMessage ITEM_MISSING_TO_LEARN_SKILL = new SystemMessage(276);

        /// <summary>
        /// ID: 277
        /// <para/>
        /// Message: You have earned $s1.
        /// </summary>
        public static SystemMessage LEARNED_SKILL_S1 = new SystemMessage(277);

        /// <summary>
        /// ID: 278
        /// <para/>
        /// Message: You do not have enough SP to learn this skill.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SP_TO_LEARN_SKILL = new SystemMessage(278);

        /// <summary>
        /// ID: 279
        /// <para/>
        /// Message: You do not have enough adena.
        /// </summary>
        public static SystemMessage YOU_NOT_ENOUGH_ADENA = new SystemMessage(279);

        /// <summary>
        /// ID: 280
        /// <para/>
        /// Message: You do not have any items to sell.
        /// </summary>
        public static SystemMessage NO_ITEMS_TO_SELL = new SystemMessage(280);

        /// <summary>
        /// ID: 281
        /// <para/>
        /// Message: You do not have enough adena to pay the fee.
        /// </summary>
        public static SystemMessage YOU_NOT_ENOUGH_ADENA_PAY_FEE = new SystemMessage(281);

        /// <summary>
        /// ID: 282
        /// <para/>
        /// Message: You have not deposited any items in your warehouse.
        /// </summary>
        public static SystemMessage NO_ITEM_DEPOSITED_IN_WH = new SystemMessage(282);

        /// <summary>
        /// ID: 283
        /// <para/>
        /// Message: You have entered a combat zone.
        /// </summary>
        public static SystemMessage ENTERED_COMBAT_ZONE = new SystemMessage(283);

        /// <summary>
        /// ID: 284
        /// <para/>
        /// Message: You have left a combat zone.
        /// </summary>
        public static SystemMessage LEFT_COMBAT_ZONE = new SystemMessage(284);

        /// <summary>
        /// ID: 285
        /// <para/>
        /// Message: Clan $s1 has succeeded in engraving the ruler!
        /// </summary>
        public static SystemMessage CLAN_S1_ENGRAVED_RULER = new SystemMessage(285);

        /// <summary>
        /// ID: 286
        /// <para/>
        /// Message: Your base is being attacked.
        /// </summary>
        public static SystemMessage BASE_UNDER_ATTACK = new SystemMessage(286);

        /// <summary>
        /// ID: 287
        /// <para/>
        /// Message: The opposing clan has stared to engrave to monument!
        /// </summary>
        public static SystemMessage OPPONENT_STARTED_ENGRAVING = new SystemMessage(287);

        /// <summary>
        /// ID: 288
        /// <para/>
        /// Message: The castle gate has been broken down.
        /// </summary>
        public static SystemMessage CASTLE_GATE_BROKEN_DOWN = new SystemMessage(288);

        /// <summary>
        /// ID: 289
        /// <para/>
        /// Message: An outpost or headquarters cannot be built because at least one already exists.
        /// </summary>
        public static SystemMessage NOT_ANOTHER_HEADQUARTERS = new SystemMessage(289);

        /// <summary>
        /// ID: 290
        /// <para/>
        /// Message: You cannot set up a base here.
        /// </summary>
        public static SystemMessage NOT_SET_UP_BASE_HERE = new SystemMessage(290);

        /// <summary>
        /// ID: 291
        /// <para/>
        /// Message: Clan $s1 is victorious over $s2's castle siege!
        /// </summary>
        public static SystemMessage CLAN_S1_VICTORIOUS_OVER_S2_S_SIEGE = new SystemMessage(291);

        /// <summary>
        /// ID: 292
        /// <para/>
        /// Message: $s1 has announced the castle siege time.
        /// </summary>
        public static SystemMessage S1_ANNOUNCED_SIEGE_TIME = new SystemMessage(292);

        /// <summary>
        /// ID: 293
        /// <para/>
        /// Message: The registration term for $s1 has ended.
        /// </summary>
        public static SystemMessage REGISTRATION_TERM_FOR_S1_ENDED = new SystemMessage(293);

        /// <summary>
        /// ID: 294
        /// <para/>
        /// Message: Because your clan is not currently on the offensive in a Clan Hall siege war, it cannot summon its base camp.
        /// </summary>
        public static SystemMessage BECAUSE_YOUR_CLAN_IS_NOT_CURRENTLY_ON_THE_OFFENSIVE_IN_A_CLAN_HALL_SIEGE_WAR_IT_CANNOT_SUMMON_ITS_BASE_CAMP = new SystemMessage(294);

        /// <summary>
        /// ID: 295
        /// <para/>
        /// Message: $s1's siege was canceled because there were no clans that participated.
        /// </summary>
        public static SystemMessage S1_SIEGE_WAS_CANCELED_BECAUSE_NO_CLANS_PARTICIPATED = new SystemMessage(295);

        /// <summary>
        /// ID: 296
        /// <para/>
        /// Message: You received $s1 damage from taking a high fall.
        /// </summary>
        public static SystemMessage FALL_DAMAGE_S1 = new SystemMessage(296);

        /// <summary>
        /// ID: 297
        /// <para/>
        /// Message: You have taken $s1 damage because you were unable to breathe.
        /// </summary>
        public static SystemMessage DROWN_DAMAGE_S1 = new SystemMessage(297);

        /// <summary>
        /// ID: 298
        /// <para/>
        /// Message: You have dropped $s1.
        /// </summary>
        public static SystemMessage YOU_DROPPED_S1 = new SystemMessage(298);

        /// <summary>
        /// ID: 299
        /// <para/>
        /// Message: $s1 has obtained $s3 $s2.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S3_S2 = new SystemMessage(299);

        /// <summary>
        /// ID: 300
        /// <para/>
        /// Message: $s1 has obtained $s2.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2 = new SystemMessage(300);

        /// <summary>
        /// ID: 301
        /// <para/>
        /// Message: $s2 $s1 has disappeared.
        /// </summary>
        public static SystemMessage S2_S1_DISAPPEARED = new SystemMessage(301);

        /// <summary>
        /// ID: 302
        /// <para/>
        /// Message: $s1 has disappeared.
        /// </summary>
        public static SystemMessage S1_DISAPPEARED = new SystemMessage(302);

        /// <summary>
        /// ID: 303
        /// <para/>
        /// Message: Select item to enchant.
        /// </summary>
        public static SystemMessage SELECT_ITEM_TO_ENCHANT = new SystemMessage(303);

        /// <summary>
        /// ID: 304
        /// <para/>
        /// Message: Clan member $s1 has logged into game.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_LOGGED_IN = new SystemMessage(304);

        /// <summary>
        /// ID: 305
        /// <para/>
        /// Message: The player declined to join your party.
        /// </summary>
        public static SystemMessage PLAYER_DECLINED = new SystemMessage(305);

        /// <summary>
        /// ID: 309
        /// <para/>
        /// Message: You have succeeded in expelling the clan member.
        /// </summary>
        public static SystemMessage YOU_HAVE_SUCCEEDED_IN_EXPELLING_CLAN_MEMBER = new SystemMessage(309);

        /// <summary>
        /// ID: 311
        /// <para/>
        /// Message: The clan war declaration has been accepted.
        /// </summary>
        public static SystemMessage CLAN_WAR_DECLARATION_ACCEPTED = new SystemMessage(311);

        /// <summary>
        /// ID: 312
        /// <para/>
        /// Message: The clan war declaration has been refused.
        /// </summary>
        public static SystemMessage CLAN_WAR_DECLARATION_REFUSED = new SystemMessage(312);

        /// <summary>
        /// ID: 313
        /// <para/>
        /// Message: The cease war request has been accepted.
        /// </summary>
        public static SystemMessage CEASE_WAR_REQUEST_ACCEPTED = new SystemMessage(313);

        /// <summary>
        /// ID: 314
        /// <para/>
        /// Message: You have failed to surrender.
        /// </summary>
        public static SystemMessage FAILED_TO_SURRENDER = new SystemMessage(314);

        /// <summary>
        /// ID: 315
        /// <para/>
        /// Message: You have failed to personally surrender.
        /// </summary>
        public static SystemMessage FAILED_TO_PERSONALLY_SURRENDER = new SystemMessage(315);

        /// <summary>
        /// ID: 316
        /// <para/>
        /// Message: You have failed to withdraw from the party.
        /// </summary>
        public static SystemMessage FAILED_TO_WITHDRAW_FROM_THE_PARTY = new SystemMessage(316);

        /// <summary>
        /// ID: 317
        /// <para/>
        /// Message: You have failed to expel the party member.
        /// </summary>
        public static SystemMessage FAILED_TO_EXPEL_THE_PARTY_MEMBER = new SystemMessage(317);

        /// <summary>
        /// ID: 318
        /// <para/>
        /// Message: You have failed to disperse the party.
        /// </summary>
        public static SystemMessage FAILED_TO_DISPERSE_THE_PARTY = new SystemMessage(318);

        /// <summary>
        /// ID: 319
        /// <para/>
        /// Message: This door cannot be unlocked.
        /// </summary>
        public static SystemMessage UNABLE_TO_UNLOCK_DOOR = new SystemMessage(319);

        /// <summary>
        /// ID: 320
        /// <para/>
        /// Message: You have failed to unlock the door.
        /// </summary>
        public static SystemMessage FAILED_TO_UNLOCK_DOOR = new SystemMessage(320);

        /// <summary>
        /// ID: 321
        /// <para/>
        /// Message: It is not locked.
        /// </summary>
        public static SystemMessage ITS_NOT_LOCKED = new SystemMessage(321);

        /// <summary>
        /// ID: 322
        /// <para/>
        /// Message: Please decide on the sales price.
        /// </summary>
        public static SystemMessage DECIDE_SALES_PRICE = new SystemMessage(322);

        /// <summary>
        /// ID: 323
        /// <para/>
        /// Message: Your force has increased to $s1 level.
        /// </summary>
        public static SystemMessage FORCE_INCREASED_TO_S1 = new SystemMessage(323);

        /// <summary>
        /// ID: 324
        /// <para/>
        /// Message: Your force has reached maximum capacity.
        /// </summary>
        public static SystemMessage FORCE_MAXLEVEL_REACHED = new SystemMessage(324);

        /// <summary>
        /// ID: 325
        /// <para/>
        /// Message: The corpse has already disappeared.
        /// </summary>
        public static SystemMessage CORPSE_ALREADY_DISAPPEARED = new SystemMessage(325);

        /// <summary>
        /// ID: 326
        /// <para/>
        /// Message: Select target from list.
        /// </summary>
        public static SystemMessage SELECT_TARGET_FROM_LIST = new SystemMessage(326);

        /// <summary>
        /// ID: 327
        /// <para/>
        /// Message: You cannot exceed 80 characters.
        /// </summary>
        public static SystemMessage CANNOT_EXCEED_80_CHARACTERS = new SystemMessage(327);

        /// <summary>
        /// ID: 328
        /// <para/>
        /// Message: Please input title using less than 128 characters.
        /// </summary>
        public static SystemMessage PLEASE_INPUT_TITLE_LESS_128_CHARACTERS = new SystemMessage(328);

        /// <summary>
        /// ID: 329
        /// <para/>
        /// Message: Please input content using less than 3000 characters.
        /// </summary>
        public static SystemMessage PLEASE_INPUT_CONTENT_LESS_3000_CHARACTERS = new SystemMessage(329);

        /// <summary>
        /// ID: 330
        /// <para/>
        /// Message: A one-line response may not exceed 128 characters.
        /// </summary>
        public static SystemMessage ONE_LINE_RESPONSE_NOT_EXCEED_128_CHARACTERS = new SystemMessage(330);

        /// <summary>
        /// ID: 331
        /// <para/>
        /// Message: You have acquired $s1 SP.
        /// </summary>
        public static SystemMessage ACQUIRED_S1_SP = new SystemMessage(331);

        /// <summary>
        /// ID: 332
        /// <para/>
        /// Message: Do you want to be restored?
        /// </summary>
        public static SystemMessage DO_YOU_WANT_TO_BE_RESTORED = new SystemMessage(332);

        /// <summary>
        /// ID: 333
        /// <para/>
        /// Message: You have received $s1 damage by Core's barrier.
        /// </summary>
        public static SystemMessage S1_DAMAGE_BY_CORE_BARRIER = new SystemMessage(333);

        /// <summary>
        /// ID: 334
        /// <para/>
        /// Message: Please enter your private store display message.
        /// </summary>
        public static SystemMessage ENTER_PRIVATE_STORE_MESSAGE = new SystemMessage(334);

        /// <summary>
        /// ID: 335
        /// <para/>
        /// Message: $s1 has been aborted.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_ABORTED = new SystemMessage(335);

        /// <summary>
        /// ID: 336
        /// <para/>
        /// Message: You are attempting to crystallize $s1. Do you wish to continue?
        /// </summary>
        public static SystemMessage WISH_TO_CRYSTALLIZE_S1 = new SystemMessage(336);

        /// <summary>
        /// ID: 337
        /// <para/>
        /// Message: The soulshot you are attempting to use does not match the grade of your equipped weapon.
        /// </summary>
        public static SystemMessage SOULSHOTS_GRADE_MISMATCH = new SystemMessage(337);

        /// <summary>
        /// ID: 338
        /// <para/>
        /// Message: You do not have enough soulshots for that.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SOULSHOTS = new SystemMessage(338);

        /// <summary>
        /// ID: 339
        /// <para/>
        /// Message: Cannot use soulshots.
        /// </summary>
        public static SystemMessage CANNOT_USE_SOULSHOTS = new SystemMessage(339);

        /// <summary>
        /// ID: 340
        /// <para/>
        /// Message: Your private store is now open for business.
        /// </summary>
        public static SystemMessage PRIVATE_STORE_UNDER_WAY = new SystemMessage(340);

        /// <summary>
        /// ID: 341
        /// <para/>
        /// Message: You do not have enough materials to perform that action.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_MATERIALS = new SystemMessage(341);

        /// <summary>
        /// ID: 342
        /// <para/>
        /// Message: Power of the spirits enabled.
        /// </summary>
        public static SystemMessage ENABLED_SOULSHOT = new SystemMessage(342);

        /// <summary>
        /// ID: 343
        /// <para/>
        /// Message: Sweeper failed, target not spoiled.
        /// </summary>
        public static SystemMessage SWEEPER_FAILED_TARGET_NOT_SPOILED = new SystemMessage(343);

        /// <summary>
        /// ID: 344
        /// <para/>
        /// Message: Power of the spirits disabled.
        /// </summary>
        public static SystemMessage SOULSHOTS_DISABLED = new SystemMessage(344);

        /// <summary>
        /// ID: 345
        /// <para/>
        /// Message: Chat enabled.
        /// </summary>
        public static SystemMessage CHAT_ENABLED = new SystemMessage(345);

        /// <summary>
        /// ID: 346
        /// <para/>
        /// Message: Chat disabled.
        /// </summary>
        public static SystemMessage CHAT_DISABLED = new SystemMessage(346);

        /// <summary>
        /// ID: 347
        /// <para/>
        /// Message: Incorrect item count.
        /// </summary>
        public static SystemMessage INCORRECT_ITEM_COUNT = new SystemMessage(347);

        /// <summary>
        /// ID: 348
        /// <para/>
        /// Message: Incorrect item price.
        /// </summary>
        public static SystemMessage INCORRECT_ITEM_PRICE = new SystemMessage(348);

        /// <summary>
        /// ID: 349
        /// <para/>
        /// Message: Private store already closed.
        /// </summary>
        public static SystemMessage PRIVATE_STORE_ALREADY_CLOSED = new SystemMessage(349);

        /// <summary>
        /// ID: 350
        /// <para/>
        /// Message: Item out of stock.
        /// </summary>
        public static SystemMessage ITEM_OUT_OF_STOCK = new SystemMessage(350);

        /// <summary>
        /// ID: 351
        /// <para/>
        /// Message: Incorrect item count.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_ITEMS = new SystemMessage(351);

        /// <summary>
        /// ID: 354
        /// <para/>
        /// Message: Cancel enchant.
        /// </summary>
        public static SystemMessage CANCEL_ENCHANT = new SystemMessage(354);

        /// <summary>
        /// ID: 355
        /// <para/>
        /// Message: Inappropriate enchant conditions.
        /// </summary>
        public static SystemMessage INAPPROPRIATE_ENCHANT_CONDITION = new SystemMessage(355);

        /// <summary>
        /// ID: 356
        /// <para/>
        /// Message: Reject resurrection.
        /// </summary>
        public static SystemMessage REJECT_RESURRECTION = new SystemMessage(356);

        /// <summary>
        /// ID: 357
        /// <para/>
        /// Message: It has already been spoiled.
        /// </summary>
        public static SystemMessage ALREADY_SPOILED = new SystemMessage(357);

        /// <summary>
        /// ID: 358
        /// <para/>
        /// Message: $s1 hour(s) until catle siege conclusion.
        /// </summary>
        public static SystemMessage S1_HOURS_UNTIL_SIEGE_CONCLUSION = new SystemMessage(358);

        /// <summary>
        /// ID: 359
        /// <para/>
        /// Message: $s1 minute(s) until catle siege conclusion.
        /// </summary>
        public static SystemMessage S1_MINUTES_UNTIL_SIEGE_CONCLUSION = new SystemMessage(359);

        /// <summary>
        /// ID: 360
        /// <para/>
        /// Message: Castle siege $s1 second(s) left!
        /// </summary>
        public static SystemMessage CASTLE_SIEGE_S1_SECONDS_LEFT = new SystemMessage(360);

        /// <summary>
        /// ID: 361
        /// <para/>
        /// Message: Over-hit!
        /// </summary>
        public static SystemMessage OVER_HIT = new SystemMessage(361);

        /// <summary>
        /// ID: 362
        /// <para/>
        /// Message: You have acquired $s1 bonus experience from a successful over-hit.
        /// </summary>
        public static SystemMessage ACQUIRED_BONUS_EXPERIENCE_THROUGH_OVER_HIT = new SystemMessage(362);

        /// <summary>
        /// ID: 363
        /// <para/>
        /// Message: Chat available time: $s1 minute.
        /// </summary>
        public static SystemMessage CHAT_AVAILABLE_S1_MINUTE = new SystemMessage(363);

        /// <summary>
        /// ID: 364
        /// <para/>
        /// Message: Enter user's name to search
        /// </summary>
        public static SystemMessage ENTER_USER_NAME_TO_SEARCH = new SystemMessage(364);

        /// <summary>
        /// ID: 365
        /// <para/>
        /// Message: Are you sure?
        /// </summary>
        public static SystemMessage ARE_YOU_SURE = new SystemMessage(365);

        /// <summary>
        /// ID: 366
        /// <para/>
        /// Message: Please select your hair color.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_HAIR_COLOR = new SystemMessage(366);

        /// <summary>
        /// ID: 367
        /// <para/>
        /// Message: You cannot remove that clan character at this time.
        /// </summary>
        public static SystemMessage CANNOT_REMOVE_CLAN_CHARACTER = new SystemMessage(367);

        /// <summary>
        /// ID: 368
        /// <para/>
        /// Message: Equipped +$s1 $s2.
        /// </summary>
        public static SystemMessage S1_S2_EQUIPPED = new SystemMessage(368);

        /// <summary>
        /// ID: 369
        /// <para/>
        /// Message: You have obtained a +$s1 $s2.
        /// </summary>
        public static SystemMessage YOU_PICKED_UP_A_S1_S2 = new SystemMessage(369);

        /// <summary>
        /// ID: 370
        /// <para/>
        /// Message: Failed to pickup $s1.
        /// </summary>
        public static SystemMessage FAILED_PICKUP_S1 = new SystemMessage(370);

        /// <summary>
        /// ID: 371
        /// <para/>
        /// Message: Acquired +$s1 $s2.
        /// </summary>
        public static SystemMessage ACQUIRED_S1_S2 = new SystemMessage(371);

        /// <summary>
        /// ID: 372
        /// <para/>
        /// Message: Failed to earn $s1.
        /// </summary>
        public static SystemMessage FAILED_EARN_S1 = new SystemMessage(372);

        /// <summary>
        /// ID: 373
        /// <para/>
        /// Message: You are trying to destroy +$s1 $s2. Do you wish to continue?
        /// </summary>
        public static SystemMessage WISH_DESTROY_S1_S2 = new SystemMessage(373);

        /// <summary>
        /// ID: 374
        /// <para/>
        /// Message: You are attempting to crystallize +$s1 $s2. Do you wish to continue?
        /// </summary>
        public static SystemMessage WISH_CRYSTALLIZE_S1_S2 = new SystemMessage(374);

        /// <summary>
        /// ID: 375
        /// <para/>
        /// Message: You have dropped +$s1 $s2 .
        /// </summary>
        public static SystemMessage DROPPED_S1_S2 = new SystemMessage(375);

        /// <summary>
        /// ID: 376
        /// <para/>
        /// Message: $s1 has obtained +$s2$s3.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2_S3 = new SystemMessage(376);

        /// <summary>
        /// ID: 377
        /// <para/>
        /// Message: $S1 $S2 disappeared.
        /// </summary>
        public static SystemMessage S1_S2_DISAPPEARED = new SystemMessage(377);

        /// <summary>
        /// ID: 378
        /// <para/>
        /// Message: $s1 purchased $s2.
        /// </summary>
        public static SystemMessage S1_PURCHASED_S2 = new SystemMessage(378);

        /// <summary>
        /// ID: 379
        /// <para/>
        /// Message: $s1 purchased +$s2$s3.
        /// </summary>
        public static SystemMessage S1_PURCHASED_S2_S3 = new SystemMessage(379);

        /// <summary>
        /// ID: 380
        /// <para/>
        /// Message: $s1 purchased $s3 $s2(s).
        /// </summary>
        public static SystemMessage S1_PURCHASED_S3_S2_S = new SystemMessage(380);

        /// <summary>
        /// ID: 381
        /// <para/>
        /// Message: The game client encountered an error and was unable to connect to the petition server.
        /// </summary>
        public static SystemMessage GAME_CLIENT_UNABLE_TO_CONNECT_TO_PETITION_SERVER = new SystemMessage(381);

        /// <summary>
        /// ID: 382
        /// <para/>
        /// Message: Currently there are no users that have checked out a GM ID.
        /// </summary>
        public static SystemMessage NO_USERS_CHECKED_OUT_GM_ID = new SystemMessage(382);

        /// <summary>
        /// ID: 383
        /// <para/>
        /// Message: Request confirmed to end consultation at petition server.
        /// </summary>
        public static SystemMessage REQUEST_CONFIRMED_TO_END_CONSULTATION = new SystemMessage(383);

        /// <summary>
        /// ID: 384
        /// <para/>
        /// Message: The client is not logged onto the game server.
        /// </summary>
        public static SystemMessage CLIENT_NOT_LOGGED_ONTO_GAME_SERVER = new SystemMessage(384);

        /// <summary>
        /// ID: 385
        /// <para/>
        /// Message: Request confirmed to begin consultation at petition server.
        /// </summary>
        public static SystemMessage REQUEST_CONFIRMED_TO_BEGIN_CONSULTATION = new SystemMessage(385);

        /// <summary>
        /// ID: 386
        /// <para/>
        /// Message: The body of your petition must be more than five characters in length.
        /// </summary>
        public static SystemMessage PETITION_MORE_THAN_FIVE_CHARACTERS = new SystemMessage(386);

        /// <summary>
        /// ID: 387
        /// <para/>
        /// Message: This ends the GM petition consultation. Please take a moment to provide feedback about this service.
        /// </summary>
        public static SystemMessage THIS_END_THE_PETITION_PLEASE_PROVIDE_FEEDBACK = new SystemMessage(387);

        /// <summary>
        /// ID: 388
        /// <para/>
        /// Message: Not under petition consultation.
        /// </summary>
        public static SystemMessage NOT_UNDER_PETITION_CONSULTATION = new SystemMessage(388);

        /// <summary>
        /// ID: 389
        /// <para/>
        /// Message: our petition application has been accepted. - Receipt No. is $s1.
        /// </summary>
        public static SystemMessage PETITION_ACCEPTED_RECENT_NO_S1 = new SystemMessage(389);

        /// <summary>
        /// ID: 390
        /// <para/>
        /// Message: You may only submit one petition (active) at a time.
        /// </summary>
        public static SystemMessage ONLY_ONE_ACTIVE_PETITION_AT_TIME = new SystemMessage(390);

        /// <summary>
        /// ID: 391
        /// <para/>
        /// Message: Receipt No. $s1, petition cancelled.
        /// </summary>
        public static SystemMessage RECENT_NO_S1_CANCELED = new SystemMessage(391);

        /// <summary>
        /// ID: 392
        /// <para/>
        /// Message: Under petition advice.
        /// </summary>
        public static SystemMessage UNDER_PETITION_ADVICE = new SystemMessage(392);

        /// <summary>
        /// ID: 393
        /// <para/>
        /// Message: Failed to cancel petition. Please try again later.
        /// </summary>
        public static SystemMessage FAILED_CANCEL_PETITION_TRY_LATER = new SystemMessage(393);

        /// <summary>
        /// ID: 394
        /// <para/>
        /// Message: Petition consultation with $s1, under way.
        /// </summary>
        public static SystemMessage PETITION_WITH_S1_UNDER_WAY = new SystemMessage(394);

        /// <summary>
        /// ID: 395
        /// <para/>
        /// Message: Ending petition consultation with $s1.
        /// </summary>
        public static SystemMessage PETITION_ENDED_WITH_S1 = new SystemMessage(395);

        /// <summary>
        /// ID: 396
        /// <para/>
        /// Message: Please login after changing your temporary password.
        /// </summary>
        public static SystemMessage TRY_AGAIN_AFTER_CHANGING_PASSWORD = new SystemMessage(396);

        /// <summary>
        /// ID: 397
        /// <para/>
        /// Message: Not a paid account.
        /// </summary>
        public static SystemMessage NO_PAID_ACCOUNT = new SystemMessage(397);

        /// <summary>
        /// ID: 398
        /// <para/>
        /// Message: There is no time left on this account.
        /// </summary>
        public static SystemMessage NO_TIME_LEFT_ON_ACCOUNT = new SystemMessage(398);

        /// <summary>
        /// ID: 400
        /// <para/>
        /// Message: You are attempting to drop $s1. Dou you wish to continue?
        /// </summary>
        public static SystemMessage WISH_TO_DROP_S1 = new SystemMessage(400);

        /// <summary>
        /// ID: 401
        /// <para/>
        /// Message: You have to many ongoing quests.
        /// </summary>
        public static SystemMessage TOO_MANY_QUESTS = new SystemMessage(401);

        /// <summary>
        /// ID: 402
        /// <para/>
        /// Message: You do not possess the correct ticket to board the boat.
        /// </summary>
        public static SystemMessage NOT_CORRECT_BOAT_TICKET = new SystemMessage(402);

        /// <summary>
        /// ID: 403
        /// <para/>
        /// Message: You have exceeded your out-of-pocket adena limit.
        /// </summary>
        public static SystemMessage EXCEECED_POCKET_ADENA_LIMIT = new SystemMessage(403);

        /// <summary>
        /// ID: 404
        /// <para/>
        /// Message: Your Create Item level is too low to register this recipe.
        /// </summary>
        public static SystemMessage CREATE_LVL_TOO_LOW_TO_REGISTER = new SystemMessage(404);

        /// <summary>
        /// ID: 405
        /// <para/>
        /// Message: The total price of the product is too high.
        /// </summary>
        public static SystemMessage TOTAL_PRICE_TOO_HIGH = new SystemMessage(405);

        /// <summary>
        /// ID: 406
        /// <para/>
        /// Message: Petition application accepted.
        /// </summary>
        public static SystemMessage PETITION_APP_ACCEPTED = new SystemMessage(406);

        /// <summary>
        /// ID: 407
        /// <para/>
        /// Message: Petition under process.
        /// </summary>
        public static SystemMessage PETITION_UNDER_PROCESS = new SystemMessage(407);

        /// <summary>
        /// ID: 408
        /// <para/>
        /// Message: Set Period
        /// </summary>
        public static SystemMessage SET_PERIOD = new SystemMessage(408);

        /// <summary>
        /// ID: 409
        /// <para/>
        /// Message: Set Time-$s1:$s2:$s3
        /// </summary>
        public static SystemMessage SET_TIME_S1_S2_S3 = new SystemMessage(409);

        /// <summary>
        /// ID: 410
        /// <para/>
        /// Message: Registration Period
        /// </summary>
        public static SystemMessage REGISTRATION_PERIOD = new SystemMessage(410);

        /// <summary>
        /// ID: 411
        /// <para/>
        /// Message: Registration Time-$s1:$s2:$s3
        /// </summary>
        public static SystemMessage REGISTRATION_TIME_S1_S2_S3 = new SystemMessage(411);

        /// <summary>
        /// ID: 412
        /// <para/>
        /// Message: Battle begins in $s1:$s2:$s3
        /// </summary>
        public static SystemMessage BATTLE_BEGINS_S1_S2_S3 = new SystemMessage(412);

        /// <summary>
        /// ID: 413
        /// <para/>
        /// Message: Battle ends in $s1:$s2:$s3
        /// </summary>
        public static SystemMessage BATTLE_ENDS_S1_S2_S3 = new SystemMessage(413);

        /// <summary>
        /// ID: 414
        /// <para/>
        /// Message: Standby
        /// </summary>
        public static SystemMessage STANDBY = new SystemMessage(414);

        /// <summary>
        /// ID: 415
        /// <para/>
        /// Message: Under Siege
        /// </summary>
        public static SystemMessage UNDER_SIEGE = new SystemMessage(415);

        /// <summary>
        /// ID: 416
        /// <para/>
        /// Message: This item cannot be exchanged.
        /// </summary>
        public static SystemMessage ITEM_CANNOT_EXCHANGE = new SystemMessage(416);

        /// <summary>
        /// ID: 417
        /// <para/>
        /// Message: $s1 has been disarmed.
        /// </summary>
        public static SystemMessage S1_DISARMED = new SystemMessage(417);

        /// <summary>
        /// ID: 419
        /// <para/>
        /// Message: $s1 minute(s) of usage time left.
        /// </summary>
        public static SystemMessage S1_MINUTES_USAGE_LEFT = new SystemMessage(419);

        /// <summary>
        /// ID: 420
        /// <para/>
        /// Message: Time expired.
        /// </summary>
        public static SystemMessage TIME_EXPIRED = new SystemMessage(420);

        /// <summary>
        /// ID: 421
        /// <para/>
        /// Message: Another person has logged in with the same account.
        /// </summary>
        public static SystemMessage ANOTHER_LOGIN_WITH_ACCOUNT = new SystemMessage(421);

        /// <summary>
        /// ID: 422
        /// <para/>
        /// Message: You have exceeded the weight limit.
        /// </summary>
        public static SystemMessage WEIGHT_LIMIT_EXCEEDED = new SystemMessage(422);

        /// <summary>
        /// ID: 423
        /// <para/>
        /// Message: You have cancelled the enchanting process.
        /// </summary>
        public static SystemMessage ENCHANT_SCROLL_CANCELLED = new SystemMessage(423);

        /// <summary>
        /// ID: 424
        /// <para/>
        /// Message: Does not fit strengthening conditions of the scroll.
        /// </summary>
        public static SystemMessage DOES_NOT_FIT_SCROLL_CONDITIONS = new SystemMessage(424);

        /// <summary>
        /// ID: 425
        /// <para/>
        /// Message: Your Create Item level is too low to register this recipe.
        /// </summary>
        public static SystemMessage CREATE_LVL_TOO_LOW_TO_REGISTER2 = new SystemMessage(425);

        /// <summary>
        /// ID: 445
        /// <para/>
        /// Message: (Reference Number Regarding Membership Withdrawal Request: $s1)
        /// </summary>
        public static SystemMessage REFERENCE_MEMBERSHIP_WITHDRAWAL_S1 = new SystemMessage(445);

        /// <summary>
        /// ID: 447
        /// <para/>
        /// Message: .
        /// </summary>
        public static SystemMessage DOT = new SystemMessage(447);

        /// <summary>
        /// ID: 448
        /// <para/>
        /// Message: There is a system error. Please log in again later.
        /// </summary>
        public static SystemMessage SYSTEM_ERROR_LOGIN_LATER = new SystemMessage(448);

        /// <summary>
        /// ID: 449
        /// <para/>
        /// Message: The password you have entered is incorrect.
        /// </summary>
        public static SystemMessage PASSWORD_ENTERED_INCORRECT1 = new SystemMessage(449);

        /// <summary>
        /// ID: 450
        /// <para/>
        /// Message: Confirm your account information and log in later.
        /// </summary>
        public static SystemMessage CONFIRM_ACCOUNT_LOGIN_LATER = new SystemMessage(450);

        /// <summary>
        /// ID: 451
        /// <para/>
        /// Message: The password you have entered is incorrect.
        /// </summary>
        public static SystemMessage PASSWORD_ENTERED_INCORRECT2 = new SystemMessage(451);

        /// <summary>
        /// ID: 452
        /// <para/>
        /// Message: Please confirm your account information and try logging in later.
        /// </summary>
        public static SystemMessage PLEASE_CONFIRM_ACCOUNT_LOGIN_LATER = new SystemMessage(452);

        /// <summary>
        /// ID: 453
        /// <para/>
        /// Message: Your account information is incorrect.
        /// </summary>
        public static SystemMessage ACCOUNT_INFORMATION_INCORRECT = new SystemMessage(453);

        /// <summary>
        /// ID: 455
        /// <para/>
        /// Message: Account is already in use. Unable to log in.
        /// </summary>
        public static SystemMessage ACCOUNT_IN_USE = new SystemMessage(455);

        /// <summary>
        /// ID: 456
        /// <para/>
        /// Message: Lineage II game services may be used by individuals 15 years of age or older except for PvP servers,which may only be used by adults 18 years of age and older (Korea Only)
        /// </summary>
        public static SystemMessage LINAGE_MINIMUM_AGE = new SystemMessage(456);

        /// <summary>
        /// ID: 457
        /// <para/>
        /// Message: Currently undergoing game server maintenance. Please log in again later.
        /// </summary>
        public static SystemMessage SERVER_MAINTENANCE = new SystemMessage(457);

        /// <summary>
        /// ID: 458
        /// <para/>
        /// Message: Your usage term has expired.
        /// </summary>
        public static SystemMessage USAGE_TERM_EXPIRED = new SystemMessage(458);

        /// <summary>
        /// ID: 460
        /// <para/>
        /// Message: to reactivate your account.
        /// </summary>
        public static SystemMessage TO_REACTIVATE_YOUR_ACCOUNT = new SystemMessage(460);

        /// <summary>
        /// ID: 461
        /// <para/>
        /// Message: Access failed.
        /// </summary>
        public static SystemMessage ACCESS_FAILED = new SystemMessage(461);

        /// <summary>
        /// ID: 461
        /// <para/>
        /// Message: Please try again later.
        /// </summary>
        public static SystemMessage PLEASE_TRY_AGAIN_LATER = new SystemMessage(461);

        /// <summary>
        /// ID: 464
        /// <para/>
        /// Message: This feature is only available alliance leaders.
        /// </summary>
        public static SystemMessage FEATURE_ONLY_FOR_ALLIANCE_LEADER = new SystemMessage(464);

        /// <summary>
        /// ID: 465
        /// <para/>
        /// Message: You are not currently allied with any clans.
        /// </summary>
        public static SystemMessage NO_CURRENT_ALLIANCES = new SystemMessage(465);

        /// <summary>
        /// ID: 466
        /// <para/>
        /// Message: You have exceeded the limit.
        /// </summary>
        public static SystemMessage YOU_HAVE_EXCEEDED_THE_LIMIT = new SystemMessage(466);

        /// <summary>
        /// ID: 467
        /// <para/>
        /// Message: You may not accept any clan within a day after expelling another clan.
        /// </summary>
        public static SystemMessage CANT_INVITE_CLAN_WITHIN_1_DAY = new SystemMessage(467);

        /// <summary>
        /// ID: 468
        /// <para/>
        /// Message: A clan that has withdrawn or been expelled cannot enter into an alliance within one day of withdrawal or expulsion.
        /// </summary>
        public static SystemMessage CANT_ENTER_ALLIANCE_WITHIN_1_DAY = new SystemMessage(468);

        /// <summary>
        /// ID: 469
        /// <para/>
        /// Message: You may not ally with a clan you are currently at war with. That would be diabolical and treacherous.
        /// </summary>
        public static SystemMessage MAY_NOT_ALLY_CLAN_BATTLE = new SystemMessage(469);

        /// <summary>
        /// ID: 470
        /// <para/>
        /// Message: Only the clan leader may apply for withdrawal from the alliance.
        /// </summary>
        public static SystemMessage ONLY_CLAN_LEADER_WITHDRAW_ALLY = new SystemMessage(470);

        /// <summary>
        /// ID: 471
        /// <para/>
        /// Message: Alliance leaders cannot withdraw.
        /// </summary>
        public static SystemMessage ALLIANCE_LEADER_CANT_WITHDRAW = new SystemMessage(471);

        /// <summary>
        /// ID: 472
        /// <para/>
        /// Message: You cannot expel yourself from the clan.
        /// </summary>
        public static SystemMessage CANNOT_EXPEL_YOURSELF = new SystemMessage(472);

        /// <summary>
        /// ID: 473
        /// <para/>
        /// Message: Different alliance.
        /// </summary>
        public static SystemMessage DIFFERENT_ALLIANCE = new SystemMessage(473);

        /// <summary>
        /// ID: 474
        /// <para/>
        /// Message: That clan does not exist.
        /// </summary>
        public static SystemMessage CLAN_DOESNT_EXISTS = new SystemMessage(474);

        /// <summary>
        /// ID: 475
        /// <para/>
        /// Message: Different alliance.
        /// </summary>
        public static SystemMessage DIFFERENT_ALLIANCE2 = new SystemMessage(475);

        /// <summary>
        /// ID: 476
        /// <para/>
        /// Message: Please adjust the image size to 8x12.
        /// </summary>
        public static SystemMessage ADJUST_IMAGE_8_12 = new SystemMessage(476);

        /// <summary>
        /// ID: 477
        /// <para/>
        /// Message: No response. Invitation to join an alliance has been cancelled.
        /// </summary>
        public static SystemMessage NO_RESPONSE_TO_ALLY_INVITATION = new SystemMessage(477);

        /// <summary>
        /// ID: 478
        /// <para/>
        /// Message: No response. Your entrance to the alliance has been cancelled.
        /// </summary>
        public static SystemMessage YOU_DID_NOT_RESPOND_TO_ALLY_INVITATION = new SystemMessage(478);

        /// <summary>
        /// ID: 479
        /// <para/>
        /// Message: $s1 has joined as a friend.
        /// </summary>
        public static SystemMessage S1_JOINED_AS_FRIEND = new SystemMessage(479);

        /// <summary>
        /// ID: 480
        /// <para/>
        /// Message: Please check your friend list.
        /// </summary>
        public static SystemMessage PLEASE_CHECK_YOUR_FRIENDS_LIST = new SystemMessage(480);

        /// <summary>
        /// ID: 481
        /// <para/>
        /// Message: $s1 has been deleted from your friends list.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_DELETED_FROM_YOUR_FRIENDS_LIST = new SystemMessage(481);

        /// <summary>
        /// ID: 482
        /// <para/>
        /// Message: You cannot add yourself to your own friend list.
        /// </summary>
        public static SystemMessage YOU_CANNOT_ADD_YOURSELF_TO_YOUR_OWN_FRIENDS_LIST = new SystemMessage(482);

        /// <summary>
        /// ID: 483
        /// <para/>
        /// Message: This function is inaccessible right now. Please try again later.
        /// </summary>
        public static SystemMessage FUNCTION_INACCESSIBLE_NOW = new SystemMessage(483);

        /// <summary>
        /// ID: 484
        /// <para/>
        /// Message: This player is already registered in your friends list.
        /// </summary>
        public static SystemMessage S1_ALREADY_IN_FRIENDS_LIST = new SystemMessage(484);

        /// <summary>
        /// ID: 485
        /// <para/>
        /// Message: No new friend invitations may be accepted.
        /// </summary>
        public static SystemMessage NO_NEW_INVITATIONS_ACCEPTED = new SystemMessage(485);

        /// <summary>
        /// ID: 486
        /// <para/>
        /// Message: The following user is not in your friends list.
        /// </summary>
        public static SystemMessage THE_USER_NOT_IN_FRIENDS_LIST = new SystemMessage(486);

        /// <summary>
        /// ID: 487
        /// <para/>
        /// Message: ======<Friends List>======
        /// </summary>
        public static SystemMessage FRIEND_LIST_HEADER = new SystemMessage(487);

        /// <summary>
        /// ID: 488
        /// <para/>
        /// Message: $s1 (Currently: Online)
        /// </summary>
        public static SystemMessage S1_ONLINE = new SystemMessage(488);

        /// <summary>
        /// ID: 489
        /// <para/>
        /// Message: $s1 (Currently: Offline)
        /// </summary>
        public static SystemMessage S1_OFFLINE = new SystemMessage(489);

        /// <summary>
        /// ID: 490
        /// <para/>
        /// Message: ========================
        /// </summary>
        public static SystemMessage FRIEND_LIST_FOOTER = new SystemMessage(490);

        /// <summary>
        /// ID: 491
        /// <para/>
        /// Message: =======<Alliance Information>=======
        /// </summary>
        public static SystemMessage ALLIANCE_INFO_HEAD = new SystemMessage(491);

        /// <summary>
        /// ID: 492
        /// <para/>
        /// Message: Alliance Name: $s1
        /// </summary>
        public static SystemMessage ALLIANCE_NAME_S1 = new SystemMessage(492);

        /// <summary>
        /// ID: 493
        /// <para/>
        /// Message: Connection: $s1 / Total $s2
        /// </summary>
        public static SystemMessage CONNECTION_S1_TOTAL_S2 = new SystemMessage(493);

        /// <summary>
        /// ID: 494
        /// <para/>
        /// Message: Alliance Leader: $s2 of $s1
        /// </summary>
        public static SystemMessage ALLIANCE_LEADER_S2_OF_S1 = new SystemMessage(494);

        /// <summary>
        /// ID: 495
        /// <para/>
        /// Message: Affiliated clans: Total $s1 clan(s)
        /// </summary>
        public static SystemMessage ALLIANCE_CLAN_TOTAL_S1 = new SystemMessage(495);

        /// <summary>
        /// ID: 496
        /// <para/>
        /// Message: =====<Clan Information>=====
        /// </summary>
        public static SystemMessage CLAN_INFO_HEAD = new SystemMessage(496);

        /// <summary>
        /// ID: 497
        /// <para/>
        /// Message: Clan Name: $s1
        /// </summary>
        public static SystemMessage CLAN_INFO_NAME_S1 = new SystemMessage(497);

        /// <summary>
        /// ID: 498
        /// <para/>
        /// Message: Clan Leader: $s1
        /// </summary>
        public static SystemMessage CLAN_INFO_LEADER_S1 = new SystemMessage(498);

        /// <summary>
        /// ID: 499
        /// <para/>
        /// Message: Clan Level: $s1
        /// </summary>
        public static SystemMessage CLAN_INFO_LEVEL_S1 = new SystemMessage(499);

        /// <summary>
        /// ID: 500
        /// <para/>
        /// Message: ------------------------
        /// </summary>
        public static SystemMessage CLAN_INFO_SEPARATOR = new SystemMessage(500);

        /// <summary>
        /// ID: 501
        /// <para/>
        /// Message: ========================
        /// </summary>
        public static SystemMessage CLAN_INFO_FOOT = new SystemMessage(501);

        /// <summary>
        /// ID: 502
        /// <para/>
        /// Message: You already belong to another alliance.
        /// </summary>
        public static SystemMessage ALREADY_JOINED_ALLIANCE = new SystemMessage(502);

        /// <summary>
        /// ID: 503
        /// <para/>
        /// Message: $s1 (Friend) has logged in.
        /// </summary>
        public static SystemMessage FRIEND_S1_HAS_LOGGED_IN = new SystemMessage(503);

        /// <summary>
        /// ID: 504
        /// <para/>
        /// Message: Only clan leaders may create alliances.
        /// </summary>
        public static SystemMessage ONLY_CLAN_LEADER_CREATE_ALLIANCE = new SystemMessage(504);

        /// <summary>
        /// ID: 505
        /// <para/>
        /// Message: You cannot create a new alliance within 10 days after dissolution.
        /// </summary>
        public static SystemMessage CANT_CREATE_ALLIANCE_10_DAYS_DISOLUTION = new SystemMessage(505);

        /// <summary>
        /// ID: 506
        /// <para/>
        /// Message: Incorrect alliance name. Please try again.
        /// </summary>
        public static SystemMessage INCORRECT_ALLIANCE_NAME = new SystemMessage(506);

        /// <summary>
        /// ID: 507
        /// <para/>
        /// Message: Incorrect length for an alliance name.
        /// </summary>
        public static SystemMessage INCORRECT_ALLIANCE_NAME_LENGTH = new SystemMessage(507);

        /// <summary>
        /// ID: 508
        /// <para/>
        /// Message: This alliance name already exists.
        /// </summary>
        public static SystemMessage ALLIANCE_ALREADY_EXISTS = new SystemMessage(508);

        /// <summary>
        /// ID: 509
        /// <para/>
        /// Message: Cannot accept. clan ally is registered as an enemy during siege battle.
        /// </summary>
        public static SystemMessage CANT_ACCEPT_ALLY_ENEMY_FOR_SIEGE = new SystemMessage(509);

        /// <summary>
        /// ID: 510
        /// <para/>
        /// Message: You have invited someone to your alliance.
        /// </summary>
        public static SystemMessage YOU_INVITED_FOR_ALLIANCE = new SystemMessage(510);

        /// <summary>
        /// ID: 511
        /// <para/>
        /// Message: You must first select a user to invite.
        /// </summary>
        public static SystemMessage SELECT_USER_TO_INVITE = new SystemMessage(511);

        /// <summary>
        /// ID: 512
        /// <para/>
        /// Message: Do you really wish to withdraw from the alliance?
        /// </summary>
        public static SystemMessage DO_YOU_WISH_TO_WITHDRW = new SystemMessage(512);

        /// <summary>
        /// ID: 513
        /// <para/>
        /// Message: Enter the name of the clan you wish to expel.
        /// </summary>
        public static SystemMessage ENTER_NAME_CLAN_TO_EXPEL = new SystemMessage(513);

        /// <summary>
        /// ID: 514
        /// <para/>
        /// Message: Do you really wish to dissolve the alliance?
        /// </summary>
        public static SystemMessage DO_YOU_WISH_TO_DISOLVE = new SystemMessage(514);

        /// <summary>
        /// ID: 516
        /// <para/>
        /// Message: $s1 has invited you to be their friend.
        /// </summary>
        public static SystemMessage SI_INVITED_YOU_AS_FRIEND = new SystemMessage(516);

        /// <summary>
        /// ID: 517
        /// <para/>
        /// Message: You have accepted the alliance.
        /// </summary>
        public static SystemMessage YOU_ACCEPTED_ALLIANCE = new SystemMessage(517);

        /// <summary>
        /// ID: 518
        /// <para/>
        /// Message: You have failed to invite a clan into the alliance.
        /// </summary>
        public static SystemMessage FAILED_TO_INVITE_CLAN_IN_ALLIANCE = new SystemMessage(518);

        /// <summary>
        /// ID: 519
        /// <para/>
        /// Message: You have withdrawn from the alliance.
        /// </summary>
        public static SystemMessage YOU_HAVE_WITHDRAWN_FROM_ALLIANCE = new SystemMessage(519);

        /// <summary>
        /// ID: 520
        /// <para/>
        /// Message: You have failed to withdraw from the alliance.
        /// </summary>
        public static SystemMessage YOU_HAVE_FAILED_TO_WITHDRAWN_FROM_ALLIANCE = new SystemMessage(520);

        /// <summary>
        /// ID: 521
        /// <para/>
        /// Message: You have succeeded in expelling a clan.
        /// </summary>
        public static SystemMessage YOU_HAVE_EXPELED_A_CLAN = new SystemMessage(521);

        /// <summary>
        /// ID: 522
        /// <para/>
        /// Message: You have failed to expel a clan.
        /// </summary>
        public static SystemMessage FAILED_TO_EXPELED_A_CLAN = new SystemMessage(522);

        /// <summary>
        /// ID: 523
        /// <para/>
        /// Message: The alliance has been dissolved.
        /// </summary>
        public static SystemMessage ALLIANCE_DISOLVED = new SystemMessage(523);

        /// <summary>
        /// ID: 524
        /// <para/>
        /// Message: You have failed to dissolve the alliance.
        /// </summary>
        public static SystemMessage FAILED_TO_DISOLVE_ALLIANCE = new SystemMessage(524);

        /// <summary>
        /// ID: 525
        /// <para/>
        /// Message: You have succeeded in inviting a friend to your friends list.
        /// </summary>
        public static SystemMessage YOU_HAVE_SUCCEEDED_INVITING_FRIEND = new SystemMessage(525);

        /// <summary>
        /// ID: 526
        /// <para/>
        /// Message: You have failed to add a friend to your friends list.
        /// </summary>
        public static SystemMessage FAILED_TO_INVITE_A_FRIEND = new SystemMessage(526);

        /// <summary>
        /// ID: 527
        /// <para/>
        /// Message: $s1 leader, $s2, has requested an alliance.
        /// </summary>
        public static SystemMessage S2_ALLIANCE_LEADER_OF_S1_REQUESTED_ALLIANCE = new SystemMessage(527);

        /// <summary>
        /// ID: 530
        /// <para/>
        /// Message: The Spiritshot does not match the weapon's grade.
        /// </summary>
        public static SystemMessage SPIRITSHOTS_GRADE_MISMATCH = new SystemMessage(530);

        /// <summary>
        /// ID: 531
        /// <para/>
        /// Message: You do not have enough Spiritshots for that.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SPIRITSHOTS = new SystemMessage(531);

        /// <summary>
        /// ID: 532
        /// <para/>
        /// Message: You may not use Spiritshots.
        /// </summary>
        public static SystemMessage CANNOT_USE_SPIRITSHOTS = new SystemMessage(532);

        /// <summary>
        /// ID: 533
        /// <para/>
        /// Message: Power of Mana enabled.
        /// </summary>
        public static SystemMessage ENABLED_SPIRITSHOT = new SystemMessage(533);

        /// <summary>
        /// ID: 534
        /// <para/>
        /// Message: Power of Mana disabled.
        /// </summary>
        public static SystemMessage DISABLED_SPIRITSHOT = new SystemMessage(534);

        /// <summary>
        /// ID: 536
        /// <para/>
        /// Message: How much adena do you wish to transfer to your Inventory?
        /// </summary>
        public static SystemMessage HOW_MUCH_ADENA_TRANSFER = new SystemMessage(536);

        /// <summary>
        /// ID: 537
        /// <para/>
        /// Message: How much will you transfer?
        /// </summary>
        public static SystemMessage HOW_MUCH_TRANSFER = new SystemMessage(537);

        /// <summary>
        /// ID: 538
        /// <para/>
        /// Message: Your SP has decreased by $s1.
        /// </summary>
        public static SystemMessage SP_DECREASED_S1 = new SystemMessage(538);

        /// <summary>
        /// ID: 539
        /// <para/>
        /// Message: Your Experience has decreased by $s1.
        /// </summary>
        public static SystemMessage EXP_DECREASED_BY_S1 = new SystemMessage(539);

        /// <summary>
        /// ID: 540
        /// <para/>
        /// Message: Clan leaders may not be deleted. Dissolve the clan first and try again.
        /// </summary>
        public static SystemMessage CLAN_LEADERS_MAY_NOT_BE_DELETED = new SystemMessage(540);

        /// <summary>
        /// ID: 541
        /// <para/>
        /// Message: You may not delete a clan member. Withdraw from the clan first and try again.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_MAY_NOT_BE_DELETED = new SystemMessage(541);

        /// <summary>
        /// ID: 542
        /// <para/>
        /// Message: The NPC server is currently down. Pets and servitors cannot be summoned at this time.
        /// </summary>
        public static SystemMessage THE_NPC_SERVER_IS_CURRENTLY_DOWN = new SystemMessage(542);

        /// <summary>
        /// ID: 543
        /// <para/>
        /// Message: You already have a pet.
        /// </summary>
        public static SystemMessage YOU_ALREADY_HAVE_A_PET = new SystemMessage(543);

        /// <summary>
        /// ID: 544
        /// <para/>
        /// Message: Your pet cannot carry this item.
        /// </summary>
        public static SystemMessage ITEM_NOT_FOR_PETS = new SystemMessage(544);

        /// <summary>
        /// ID: 545
        /// <para/>
        /// Message: Your pet cannot carry any more items. Remove some, then try again.
        /// </summary>
        public static SystemMessage YOUR_PET_CANNOT_CARRY_ANY_MORE_ITEMS = new SystemMessage(545);

        /// <summary>
        /// ID: 546
        /// <para/>
        /// Message: Unable to place item, your pet is too encumbered.
        /// </summary>
        public static SystemMessage UNABLE_TO_PLACE_ITEM_YOUR_PET_IS_TOO_ENCUMBERED = new SystemMessage(546);

        /// <summary>
        /// ID: 547
        /// <para/>
        /// Message: Summoning your pet.
        /// </summary>
        public static SystemMessage SUMMON_A_PET = new SystemMessage(547);

        /// <summary>
        /// ID: 548
        /// <para/>
        /// Message: Your pet's name can be up to 8 characters in length.
        /// </summary>
        public static SystemMessage NAMING_PETNAME_UP_TO_8CHARS = new SystemMessage(548);

        /// <summary>
        /// ID: 549
        /// <para/>
        /// Message: To create an alliance, your clan must be Level 5 or higher.
        /// </summary>
        public static SystemMessage TO_CREATE_AN_ALLY_YOU_CLAN_MUST_BE_LEVEL_5_OR_HIGHER = new SystemMessage(549);

        /// <summary>
        /// ID: 550
        /// <para/>
        /// Message: You may not create an alliance during the term of dissolution postponement.
        /// </summary>
        public static SystemMessage YOU_MAY_NOT_CREATE_ALLY_WHILE_DISSOLVING = new SystemMessage(550);

        /// <summary>
        /// ID: 551
        /// <para/>
        /// Message: You cannot raise your clan level during the term of dispersion postponement.
        /// </summary>
        public static SystemMessage CANNOT_RISE_LEVEL_WHILE_DISSOLUTION_IN_PROGRESS = new SystemMessage(551);

        /// <summary>
        /// ID: 552
        /// <para/>
        /// Message: During the grace period for dissolving a clan, the registration or deletion of a clan's crest is not allowed.
        /// </summary>
        public static SystemMessage CANNOT_SET_CREST_WHILE_DISSOLUTION_IN_PROGRESS = new SystemMessage(552);

        /// <summary>
        /// ID: 553
        /// <para/>
        /// Message: The opposing clan has applied for dispersion.
        /// </summary>
        public static SystemMessage OPPOSING_CLAN_APPLIED_DISPERSION = new SystemMessage(553);

        /// <summary>
        /// ID: 554
        /// <para/>
        /// Message: You cannot disperse the clans in your alliance.
        /// </summary>
        public static SystemMessage CANNOT_DISPERSE_THE_CLANS_IN_ALLY = new SystemMessage(554);

        /// <summary>
        /// ID: 555
        /// <para/>
        /// Message: You cannot move - you are too encumbered
        /// </summary>
        public static SystemMessage CANT_MOVE_TOO_ENCUMBERED = new SystemMessage(555);

        /// <summary>
        /// ID: 556
        /// <para/>
        /// Message: You cannot move in this state
        /// </summary>
        public static SystemMessage CANT_MOVE_IN_THIS_STATE = new SystemMessage(556);

        /// <summary>
        /// ID: 557
        /// <para/>
        /// Message: Your pet has been summoned and may not be destroyed
        /// </summary>
        public static SystemMessage PET_SUMMONED_MAY_NOT_DESTROYED = new SystemMessage(557);

        /// <summary>
        /// ID: 558
        /// <para/>
        /// Message: Your pet has been summoned and may not be let go.
        /// </summary>
        public static SystemMessage PET_SUMMONED_MAY_NOT_LET_GO = new SystemMessage(558);

        /// <summary>
        /// ID: 559
        /// <para/>
        /// Message: You have purchased $s2 from $s1.
        /// </summary>
        public static SystemMessage PURCHASED_S2_FROM_S1 = new SystemMessage(559);

        /// <summary>
        /// ID: 560
        /// <para/>
        /// Message: You have purchased +$s2 $s3 from $s1.
        /// </summary>
        public static SystemMessage PURCHASED_S2_S3_FROM_S1 = new SystemMessage(560);

        /// <summary>
        /// ID: 561
        /// <para/>
        /// Message: You have purchased $s3 $s2(s) from $s1.
        /// </summary>
        public static SystemMessage PURCHASED_S3_S2_S_FROM_S1 = new SystemMessage(561);

        /// <summary>
        /// ID: 562
        /// <para/>
        /// Message: You may not crystallize this item. Your crystallization skill level is too low.
        /// </summary>
        public static SystemMessage CRYSTALLIZE_LEVEL_TOO_LOW = new SystemMessage(562);

        /// <summary>
        /// ID: 563
        /// <para/>
        /// Message: Failed to disable attack target.
        /// </summary>
        public static SystemMessage FAILED_DISABLE_TARGET = new SystemMessage(563);

        /// <summary>
        /// ID: 564
        /// <para/>
        /// Message: Failed to change attack target.
        /// </summary>
        public static SystemMessage FAILED_CHANGE_TARGET = new SystemMessage(564);

        /// <summary>
        /// ID: 565
        /// <para/>
        /// Message: Not enough luck.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_LUCK = new SystemMessage(565);

        /// <summary>
        /// ID: 566
        /// <para/>
        /// Message: Your confusion spell failed.
        /// </summary>
        public static SystemMessage CONFUSION_FAILED = new SystemMessage(566);

        /// <summary>
        /// ID: 567
        /// <para/>
        /// Message: Your fear spell failed.
        /// </summary>
        public static SystemMessage FEAR_FAILED = new SystemMessage(567);

        /// <summary>
        /// ID: 568
        /// <para/>
        /// Message: Cubic Summoning failed.
        /// </summary>
        public static SystemMessage CUBIC_SUMMONING_FAILED = new SystemMessage(568);

        /// <summary>
        /// ID: 572
        /// <para/>
        /// Message: Do you accept $s1's party invitation? (Item Distribution: Finders Keepers.)
        /// </summary>
        public static SystemMessage S1_INVITED_YOU_TO_PARTY_FINDERS_KEEPERS = new SystemMessage(572);

        /// <summary>
        /// ID: 573
        /// <para/>
        /// Message: Do you accept $s1's party invitation? (Item Distribution: Random.)
        /// </summary>
        public static SystemMessage S1_INVITED_YOU_TO_PARTY_RANDOM = new SystemMessage(573);

        /// <summary>
        /// ID: 574
        /// <para/>
        /// Message: Pets and Servitors are not available at this time.
        /// </summary>
        public static SystemMessage PETS_ARE_NOT_AVAILABLE_AT_THIS_TIME = new SystemMessage(574);

        /// <summary>
        /// ID: 575
        /// <para/>
        /// Message: How much adena do you wish to transfer to your pet?
        /// </summary>
        public static SystemMessage HOW_MUCH_ADENA_TRANSFER_TO_PET = new SystemMessage(575);

        /// <summary>
        /// ID: 576
        /// <para/>
        /// Message: How much do you wish to transfer?
        /// </summary>
        public static SystemMessage HOW_MUCH_TRANSFER2 = new SystemMessage(576);

        /// <summary>
        /// ID: 577
        /// <para/>
        /// Message: You cannot summon during a trade or while using the private shops.
        /// </summary>
        public static SystemMessage CANNOT_SUMMON_DURING_TRADE_SHOP = new SystemMessage(577);

        /// <summary>
        /// ID: 578
        /// <para/>
        /// Message: You cannot summon during combat.
        /// </summary>
        public static SystemMessage YOU_CANNOT_SUMMON_IN_COMBAT = new SystemMessage(578);

        /// <summary>
        /// ID: 579
        /// <para/>
        /// Message: A pet cannot be sent back during battle.
        /// </summary>
        public static SystemMessage PET_CANNOT_SENT_BACK_DURING_BATTLE = new SystemMessage(579);

        /// <summary>
        /// ID: 580
        /// <para/>
        /// Message: You may not use multiple pets or servitors at the same time.
        /// </summary>
        public static SystemMessage SUMMON_ONLY_ONE = new SystemMessage(580);

        /// <summary>
        /// ID: 581
        /// <para/>
        /// Message: There is a space in the name.
        /// </summary>
        public static SystemMessage NAMING_THERE_IS_A_SPACE = new SystemMessage(581);

        /// <summary>
        /// ID: 582
        /// <para/>
        /// Message: Inappropriate character name.
        /// </summary>
        public static SystemMessage NAMING_INAPPROPRIATE_CHARACTER_NAME = new SystemMessage(582);

        /// <summary>
        /// ID: 583
        /// <para/>
        /// Message: Name includes forbidden words.
        /// </summary>
        public static SystemMessage NAMING_INCLUDES_FORBIDDEN_WORDS = new SystemMessage(583);

        /// <summary>
        /// ID: 584
        /// <para/>
        /// Message: This is already in use by another pet.
        /// </summary>
        public static SystemMessage NAMING_ALREADY_IN_USE_BY_ANOTHER_PET = new SystemMessage(584);

        /// <summary>
        /// ID: 585
        /// <para/>
        /// Message: Please decide on the price.
        /// </summary>
        public static SystemMessage DECIDE_ON_PRICE = new SystemMessage(585);

        /// <summary>
        /// ID: 586
        /// <para/>
        /// Message: Pet items cannot be registered as shortcuts.
        /// </summary>
        public static SystemMessage PET_NO_SHORTCUT = new SystemMessage(586);

        /// <summary>
        /// ID: 588
        /// <para/>
        /// Message: Your pet's inventory is full.
        /// </summary>
        public static SystemMessage PET_INVENTORY_FULL = new SystemMessage(588);

        /// <summary>
        /// ID: 589
        /// <para/>
        /// Message: A dead pet cannot be sent back.
        /// </summary>
        public static SystemMessage DEAD_PET_CANNOT_BE_RETURNED = new SystemMessage(589);

        /// <summary>
        /// ID: 590
        /// <para/>
        /// Message: Your pet is motionless and any attempt you make to give it something goes unrecognized.
        /// </summary>
        public static SystemMessage CANNOT_GIVE_ITEMS_TO_DEAD_PET = new SystemMessage(590);

        /// <summary>
        /// ID: 591
        /// <para/>
        /// Message: An invalid character is included in the pet's name.
        /// </summary>
        public static SystemMessage NAMING_PETNAME_CONTAINS_INVALID_CHARS = new SystemMessage(591);

        /// <summary>
        /// ID: 592
        /// <para/>
        /// Message: Do you wish to dismiss your pet? Dismissing your pet will cause the pet necklace to disappear
        /// </summary>
        public static SystemMessage WISH_TO_DISMISS_PET = new SystemMessage(592);

        /// <summary>
        /// ID: 593
        /// <para/>
        /// Message: Starving, grumpy and fed up, your pet has left.
        /// </summary>
        public static SystemMessage STARVING_GRUMPY_AND_FED_UP_YOUR_PET_HAS_LEFT = new SystemMessage(593);

        /// <summary>
        /// ID: 594
        /// <para/>
        /// Message: You may not restore a hungry pet.
        /// </summary>
        public static SystemMessage YOU_CANNOT_RESTORE_HUNGRY_PETS = new SystemMessage(594);

        /// <summary>
        /// ID: 595
        /// <para/>
        /// Message: Your pet is very hungry.
        /// </summary>
        public static SystemMessage YOUR_PET_IS_VERY_HUNGRY = new SystemMessage(595);

        /// <summary>
        /// ID: 596
        /// <para/>
        /// Message: Your pet ate a little, but is still hungry.
        /// </summary>
        public static SystemMessage YOUR_PET_ATE_A_LITTLE_BUT_IS_STILL_HUNGRY = new SystemMessage(596);

        /// <summary>
        /// ID: 597
        /// <para/>
        /// Message: Your pet is very hungry. Please be careful.
        /// </summary>
        public static SystemMessage YOUR_PET_IS_VERY_HUNGRY_PLEASE_BE_CAREFUL = new SystemMessage(597);

        /// <summary>
        /// ID: 598
        /// <para/>
        /// Message: You may not chat while you are invisible.
        /// </summary>
        public static SystemMessage NOT_CHAT_WHILE_INVISIBLE = new SystemMessage(598);

        /// <summary>
        /// ID: 599
        /// <para/>
        /// Message: The GM has an important notice. Chat has been temporarily disabled.
        /// </summary>
        public static SystemMessage GM_NOTICE_CHAT_DISABLED = new SystemMessage(599);

        /// <summary>
        /// ID: 600
        /// <para/>
        /// Message: You may not equip a pet item.
        /// </summary>
        public static SystemMessage CANNOT_EQUIP_PET_ITEM = new SystemMessage(600);

        /// <summary>
        /// ID: 601
        /// <para/>
        /// Message: There are $S1 petitions currently on the waiting list.
        /// </summary>
        public static SystemMessage S1_PETITION_ON_WAITING_LIST = new SystemMessage(601);

        /// <summary>
        /// ID: 602
        /// <para/>
        /// Message: The petition system is currently unavailable. Please try again later.
        /// </summary>
        public static SystemMessage PETITION_SYSTEM_CURRENT_UNAVAILABLE = new SystemMessage(602);

        /// <summary>
        /// ID: 603
        /// <para/>
        /// Message: That item cannot be discarded or exchanged.
        /// </summary>
        public static SystemMessage CANNOT_DISCARD_EXCHANGE_ITEM = new SystemMessage(603);

        /// <summary>
        /// ID: 604
        /// <para/>
        /// Message: You may not call forth a pet or summoned creature from this location
        /// </summary>
        public static SystemMessage NOT_CALL_PET_FROM_THIS_LOCATION = new SystemMessage(604);

        /// <summary>
        /// ID: 605
        /// <para/>
        /// Message: You may register up to 64 people on your list.
        /// </summary>
        public static SystemMessage MAY_REGISTER_UP_TO_64_PEOPLE = new SystemMessage(605);

        /// <summary>
        /// ID: 606
        /// <para/>
        /// Message: You cannot be registered because the other person has already registered 64 people on his/her list.
        /// </summary>
        public static SystemMessage OTHER_PERSON_ALREADY_64_PEOPLE = new SystemMessage(606);

        /// <summary>
        /// ID: 607
        /// <para/>
        /// Message: You do not have any further skills to learn. Come back when you have reached Level $s1.
        /// </summary>
        public static SystemMessage DO_NOT_HAVE_FURTHER_SKILLS_TO_LEARN_S1 = new SystemMessage(607);

        /// <summary>
        /// ID: 608
        /// <para/>
        /// Message: $s1 has obtained $s3 $s2 by using Sweeper.
        /// </summary>
        public static SystemMessage S1_SWEEPED_UP_S3_S2 = new SystemMessage(608);

        /// <summary>
        /// ID: 609
        /// <para/>
        /// Message: $s1 has obtained $s2 by using Sweeper.
        /// </summary>
        public static SystemMessage S1_SWEEPED_UP_S2 = new SystemMessage(609);

        /// <summary>
        /// ID: 610
        /// <para/>
        /// Message: Your skill has been canceled due to lack of HP.
        /// </summary>
        public static SystemMessage SKILL_REMOVED_DUE_LACK_HP = new SystemMessage(610);

        /// <summary>
        /// ID: 611
        /// <para/>
        /// Message: You have succeeded in Confusing the enemy.
        /// </summary>
        public static SystemMessage CONFUSING_SUCCEEDED = new SystemMessage(611);

        /// <summary>
        /// ID: 612
        /// <para/>
        /// Message: The Spoil condition has been activated.
        /// </summary>
        public static SystemMessage SPOIL_SUCCESS = new SystemMessage(612);

        /// <summary>
        /// ID: 613
        /// <para/>
        /// Message: ======<Ignore List>======
        /// </summary>
        public static SystemMessage BLOCK_LIST_HEADER = new SystemMessage(613);

        /// <summary>
        /// ID: 614
        /// <para/>
        /// Message: $s1 : $s2
        /// </summary>
        public static SystemMessage S1_S2 = new SystemMessage(614);

        /// <summary>
        /// ID: 615
        /// <para/>
        /// Message: You have failed to register the user to your Ignore List.
        /// </summary>
        public static SystemMessage FAILED_TO_REGISTER_TO_IGNORE_LIST = new SystemMessage(615);

        /// <summary>
        /// ID: 616
        /// <para/>
        /// Message: You have failed to delete the character.
        /// </summary>
        public static SystemMessage FAILED_TO_DELETE_CHARACTER = new SystemMessage(616);

        /// <summary>
        /// ID: 617
        /// <para/>
        /// Message: $s1 has been added to your Ignore List.
        /// </summary>
        public static SystemMessage S1_WAS_ADDED_TO_YOUR_IGNORE_LIST = new SystemMessage(617);

        /// <summary>
        /// ID: 618
        /// <para/>
        /// Message: $s1 has been removed from your Ignore List.
        /// </summary>
        public static SystemMessage S1_WAS_REMOVED_FROM_YOUR_IGNORE_LIST = new SystemMessage(618);

        /// <summary>
        /// ID: 619
        /// <para/>
        /// Message: $s1 has placed you on his/her Ignore List.
        /// </summary>
        public static SystemMessage S1_HAS_ADDED_YOU_TO_IGNORE_LIST = new SystemMessage(619);

        /// <summary>
        /// ID: 620
        /// <para/>
        /// Message: $s1 has placed you on his/her Ignore List.
        /// </summary>
        public static SystemMessage S1_HAS_ADDED_YOU_TO_IGNORE_LIST2 = new SystemMessage(620);

        /// <summary>
        /// ID: 621
        /// <para/>
        /// Message: Game connection attempted through a restricted IP.
        /// </summary>
        public static SystemMessage CONNECTION_RESTRICTED_IP = new SystemMessage(621);

        /// <summary>
        /// ID: 622
        /// <para/>
        /// Message: You may not make a declaration of war during an alliance battle.
        /// </summary>
        public static SystemMessage NO_WAR_DURING_ALLY_BATTLE = new SystemMessage(622);

        /// <summary>
        /// ID: 623
        /// <para/>
        /// Message: Your opponent has exceeded the number of simultaneous alliance battles alllowed.
        /// </summary>
        public static SystemMessage OPPONENT_TOO_MUCH_ALLY_BATTLES1 = new SystemMessage(623);

        /// <summary>
        /// ID: 624
        /// <para/>
        /// Message: $s1 Clan leader is not currently connected to the game server.
        /// </summary>
        public static SystemMessage S1_LEADER_NOT_CONNECTED = new SystemMessage(624);

        /// <summary>
        /// ID: 625
        /// <para/>
        /// Message: Your request for Alliance Battle truce has been denied.
        /// </summary>
        public static SystemMessage ALLY_BATTLE_TRUCE_DENIED = new SystemMessage(625);

        /// <summary>
        /// ID: 626
        /// <para/>
        /// Message: The $s1 clan did not respond: war proclamation has been refused.
        /// </summary>
        public static SystemMessage WAR_PROCLAMATION_HAS_BEEN_REFUSED = new SystemMessage(626);

        /// <summary>
        /// ID: 627
        /// <para/>
        /// Message: Clan battle has been refused because you did not respond to $s1 clan's war proclamation.
        /// </summary>
        public static SystemMessage YOU_REFUSED_CLAN_WAR_PROCLAMATION = new SystemMessage(627);

        /// <summary>
        /// ID: 628
        /// <para/>
        /// Message: You have already been at war with the $s1 clan: 5 days must pass before you can declare war again.
        /// </summary>
        public static SystemMessage ALREADY_AT_WAR_WITH_S1_WAIT_5_DAYS = new SystemMessage(628);

        /// <summary>
        /// ID: 629
        /// <para/>
        /// Message: Your opponent has exceeded the number of simultaneous alliance battles alllowed.
        /// </summary>
        public static SystemMessage OPPONENT_TOO_MUCH_ALLY_BATTLES2 = new SystemMessage(629);

        /// <summary>
        /// ID: 630
        /// <para/>
        /// Message: War with the clan has begun.
        /// </summary>
        public static SystemMessage WAR_WITH_CLAN_BEGUN = new SystemMessage(630);

        /// <summary>
        /// ID: 631
        /// <para/>
        /// Message: War with the clan is over.
        /// </summary>
        public static SystemMessage WAR_WITH_CLAN_ENDED = new SystemMessage(631);

        /// <summary>
        /// ID: 632
        /// <para/>
        /// Message: You have won the war over the clan!
        /// </summary>
        public static SystemMessage WON_WAR_OVER_CLAN = new SystemMessage(632);

        /// <summary>
        /// ID: 633
        /// <para/>
        /// Message: You have surrendered to the clan.
        /// </summary>
        public static SystemMessage SURRENDERED_TO_CLAN = new SystemMessage(633);

        /// <summary>
        /// ID: 634
        /// <para/>
        /// Message: Your alliance leader has been slain. You have been defeated by the clan.
        /// </summary>
        public static SystemMessage DEFEATED_BY_CLAN = new SystemMessage(634);

        /// <summary>
        /// ID: 635
        /// <para/>
        /// Message: The time limit for the clan war has been exceeded. War with the clan is over.
        /// </summary>
        public static SystemMessage TIME_UP_WAR_OVER = new SystemMessage(635);

        /// <summary>
        /// ID: 636
        /// <para/>
        /// Message: You are not involved in a clan war.
        /// </summary>
        public static SystemMessage NOT_INVOLVED_IN_WAR = new SystemMessage(636);

        /// <summary>
        /// ID: 637
        /// <para/>
        /// Message: A clan ally has registered itself to the opponent.
        /// </summary>
        public static SystemMessage ALLY_REGISTERED_SELF_TO_OPPONENT = new SystemMessage(637);

        /// <summary>
        /// ID: 638
        /// <para/>
        /// Message: You have already requested a Siege Battle.
        /// </summary>
        public static SystemMessage ALREADY_REQUESTED_SIEGE_BATTLE = new SystemMessage(638);

        /// <summary>
        /// ID: 639
        /// <para/>
        /// Message: Your application has been denied because you have already submitted a request for another Siege Battle.
        /// </summary>
        public static SystemMessage APPLICATION_DENIED_BECAUSE_ALREADY_SUBMITTED_A_REQUEST_FOR_ANOTHER_SIEGE_BATTLE = new SystemMessage(639);

        /// <summary>
        /// ID: 642
        /// <para/>
        /// Message: You are already registered to the attacker side and must not cancel your registration before submitting your request
        /// </summary>
        public static SystemMessage ALREADY_ATTACKER_NOT_CANCEL = new SystemMessage(642);

        /// <summary>
        /// ID: 643
        /// <para/>
        /// Message: You are already registered to the defender side and must not cancel your registration before submitting your request
        /// </summary>
        public static SystemMessage ALREADY_DEFENDER_NOT_CANCEL = new SystemMessage(643);

        /// <summary>
        /// ID: 644
        /// <para/>
        /// Message: You are not yet registered for the castle siege.
        /// </summary>
        public static SystemMessage NOT_REGISTERED_FOR_SIEGE = new SystemMessage(644);

        /// <summary>
        /// ID: 645
        /// <para/>
        /// Message: Only clans of level 4 or higher may register for a castle siege.
        /// </summary>
        public static SystemMessage ONLY_CLAN_LEVEL_4_ABOVE_MAY_SIEGE = new SystemMessage(645);

        /// <summary>
        /// ID: 648
        /// <para/>
        /// Message: No more registrations may be accepted for the attacker side.
        /// </summary>
        public static SystemMessage ATTACKER_SIDE_FULL = new SystemMessage(648);

        /// <summary>
        /// ID: 649
        /// <para/>
        /// Message: No more registrations may be accepted for the defender side.
        /// </summary>
        public static SystemMessage DEFENDER_SIDE_FULL = new SystemMessage(649);

        /// <summary>
        /// ID: 650
        /// <para/>
        /// Message: You may not summon from your current location.
        /// </summary>
        public static SystemMessage YOU_MAY_NOT_SUMMON_FROM_YOUR_CURRENT_LOCATION = new SystemMessage(650);

        /// <summary>
        /// ID: 651
        /// <para/>
        /// Message: Place $s1 in the current location and direction. Do you wish to continue?
        /// </summary>
        public static SystemMessage PLACE_S1_IN_CURRENT_LOCATION_AND_DIRECTION = new SystemMessage(651);

        /// <summary>
        /// ID: 652
        /// <para/>
        /// Message: The target of the summoned monster is wrong.
        /// </summary>
        public static SystemMessage TARGET_OF_SUMMON_WRONG = new SystemMessage(652);

        /// <summary>
        /// ID: 653
        /// <para/>
        /// Message: You do not have the authority to position mercenaries.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_HAVE_AUTHORITY_TO_POSITION_MERCENARIES = new SystemMessage(653);

        /// <summary>
        /// ID: 654
        /// <para/>
        /// Message: You do not have the authority to cancel mercenary positioning.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_HAVE_AUTHORITY_TO_CANCEL_MERCENARY_POSITIONING = new SystemMessage(654);

        /// <summary>
        /// ID: 655
        /// <para/>
        /// Message: Mercenaries cannot be positioned here.
        /// </summary>
        public static SystemMessage MERCENARIES_CANNOT_BE_POSITIONED_HERE = new SystemMessage(655);

        /// <summary>
        /// ID: 656
        /// <para/>
        /// Message: This mercenary cannot be positioned anymore.
        /// </summary>
        public static SystemMessage THIS_MERCENARY_CANNOT_BE_POSITIONED_ANYMORE = new SystemMessage(656);

        /// <summary>
        /// ID: 657
        /// <para/>
        /// Message: Positioning cannot be done here because the distance between mercenaries is too short.
        /// </summary>
        public static SystemMessage POSITIONING_CANNOT_BE_DONE_BECAUSE_DISTANCE_BETWEEN_MERCENARIES_TOO_SHORT = new SystemMessage(657);

        /// <summary>
        /// ID: 658
        /// <para/>
        /// Message: This is not a mercenary of a castle that you own and so you cannot cancel its positioning.
        /// </summary>
        public static SystemMessage THIS_IS_NOT_A_MERCENARY_OF_A_CASTLE_THAT_YOU_OWN_AND_SO_CANNOT_CANCEL_POSITIONING = new SystemMessage(658);

        /// <summary>
        /// ID: 659
        /// <para/>
        /// Message: This is not the time for siege registration and so registrations cannot be accepted or rejected.
        /// </summary>
        public static SystemMessage NOT_SIEGE_REGISTRATION_TIME1 = new SystemMessage(659);

        /// <summary>
        /// ID: 659
        /// <para/>
        /// Message: This is not the time for siege registration and so registration and cancellation cannot be done.
        /// </summary>
        public static SystemMessage NOT_SIEGE_REGISTRATION_TIME2 = new SystemMessage(659);

        /// <summary>
        /// ID: 661
        /// <para/>
        /// Message: This character cannot be spoiled.
        /// </summary>
        public static SystemMessage SPOIL_CANNOT_USE = new SystemMessage(661);

        /// <summary>
        /// ID: 662
        /// <para/>
        /// Message: The other player is rejecting friend invitations.
        /// </summary>
        public static SystemMessage THE_PLAYER_IS_REJECTING_FRIEND_INVITATIONS = new SystemMessage(662);

        /// <summary>
        /// ID: 664
        /// <para/>
        /// Message: Please choose a person to receive.
        /// </summary>
        public static SystemMessage CHOOSE_PERSON_TO_RECEIVE = new SystemMessage(664);

        /// <summary>
        /// ID: 665
        /// <para/>
        /// Message: of alliance is applying for alliance war. Do you want to accept the challenge?
        /// </summary>
        public static SystemMessage APPLYING_ALLIANCE_WAR = new SystemMessage(665);

        /// <summary>
        /// ID: 666
        /// <para/>
        /// Message: A request for ceasefire has been received from alliance. Do you agree?
        /// </summary>
        public static SystemMessage REQUEST_FOR_CEASEFIRE = new SystemMessage(666);

        /// <summary>
        /// ID: 667
        /// <para/>
        /// Message: You are registering on the attacking side of the siege. Do you want to continue?
        /// </summary>
        public static SystemMessage REGISTERING_ON_ATTACKING_SIDE = new SystemMessage(667);

        /// <summary>
        /// ID: 668
        /// <para/>
        /// Message: You are registering on the defending side of the siege. Do you want to continue?
        /// </summary>
        public static SystemMessage REGISTERING_ON_DEFENDING_SIDE = new SystemMessage(668);

        /// <summary>
        /// ID: 669
        /// <para/>
        /// Message: You are canceling your application to participate in the siege battle. Do you want to continue?
        /// </summary>
        public static SystemMessage CANCELING_REGISTRATION = new SystemMessage(669);

        /// <summary>
        /// ID: 670
        /// <para/>
        /// Message: You are refusing the registration of clan on the defending side. Do you want to continue?
        /// </summary>
        public static SystemMessage REFUSING_REGISTRATION = new SystemMessage(670);

        /// <summary>
        /// ID: 671
        /// <para/>
        /// Message: You are agreeing to the registration of clan on the defending side. Do you want to continue?
        /// </summary>
        public static SystemMessage AGREEING_REGISTRATION = new SystemMessage(671);

        /// <summary>
        /// ID: 672
        /// <para/>
        /// Message: $s1 adena disappeared.
        /// </summary>
        public static SystemMessage S1_DISAPPEARED_ADENA = new SystemMessage(672);

        /// <summary>
        /// ID: 673
        /// <para/>
        /// Message: Only a clan leader whose clan is of level 2 or higher is allowed to participate in a clan hall auction.
        /// </summary>
        public static SystemMessage AUCTION_ONLY_CLAN_LEVEL_2_HIGHER = new SystemMessage(673);

        /// <summary>
        /// ID: 674
        /// <para/>
        /// Message: I has not yet been seven days since canceling an auction.
        /// </summary>
        public static SystemMessage NOT_SEVEN_DAYS_SINCE_CANCELING_AUCTION = new SystemMessage(674);

        /// <summary>
        /// ID: 675
        /// <para/>
        /// Message: There are no clan halls up for auction.
        /// </summary>
        public static SystemMessage NO_CLAN_HALLS_UP_FOR_AUCTION = new SystemMessage(675);

        /// <summary>
        /// ID: 676
        /// <para/>
        /// Message: Since you have already submitted a bid, you are not allowed to participate in another auction at this time.
        /// </summary>
        public static SystemMessage ALREADY_SUBMITTED_BID = new SystemMessage(676);

        /// <summary>
        /// ID: 677
        /// <para/>
        /// Message: Your bid price must be higher than the minimum price that can be bid.
        /// </summary>
        public static SystemMessage BID_PRICE_MUST_BE_HIGHER = new SystemMessage(677);

        /// <summary>
        /// ID: 678
        /// <para/>
        /// Message: You have submitted a bid for the auction of $s1.
        /// </summary>
        public static SystemMessage SUBMITTED_A_BID = new SystemMessage(678);

        /// <summary>
        /// ID: 679
        /// <para/>
        /// Message: You have canceled your bid.
        /// </summary>
        public static SystemMessage CANCELED_BID = new SystemMessage(679);

        /// <summary>
        /// ID: 680
        /// <para/>
        /// You cannot participate in an auction.
        /// </summary>
        public static SystemMessage CANNOT_PARTICIPATE_IN_AUCTION = new SystemMessage(680);

        /// <summary>
        /// ID: 681
        /// <para/>
        /// Message: The clan does not own a clan hall.
        /// </summary>
        // CLAN_HAS_NO_CLAN_HALL(681) // Doesn't exist in Hellbound anymore = new SystemMessage(681);

        /// <summary>
        /// ID: 683
        /// <para/>
        /// Message: There are no priority rights on a sweeper.
        /// </summary>
        public static SystemMessage SWEEP_NOT_ALLOWED = new SystemMessage(683);

        /// <summary>
        /// ID: 684
        /// <para/>
        /// Message: You cannot position mercenaries during a siege.
        /// </summary>
        public static SystemMessage CANNOT_POSITION_MERCS_DURING_SIEGE = new SystemMessage(684);

        /// <summary>
        /// ID: 685
        /// <para/>
        /// Message: You cannot apply for clan war with a clan that belongs to the same alliance
        /// </summary>
        public static SystemMessage CANNOT_DECLARE_WAR_ON_ALLY = new SystemMessage(685);

        /// <summary>
        /// ID: 686
        /// <para/>
        /// Message: You have received $s1 damage from the fire of magic.
        /// </summary>
        public static SystemMessage S1_DAMAGE_FROM_FIRE_MAGIC = new SystemMessage(686);

        /// <summary>
        /// ID: 687
        /// <para/>
        /// Message: You cannot move while frozen. Please wait.
        /// </summary>
        public static SystemMessage CANNOT_MOVE_FROZEN = new SystemMessage(687);

        /// <summary>
        /// ID: 688
        /// <para/>
        /// Message: The clan that owns the castle is automatically registered on the defending side.
        /// </summary>
        public static SystemMessage CLAN_THAT_OWNS_CASTLE_IS_AUTOMATICALLY_REGISTERED_DEFENDING = new SystemMessage(688);

        /// <summary>
        /// ID: 689
        /// <para/>
        /// Message: A clan that owns a castle cannot participate in another siege.
        /// </summary>
        public static SystemMessage CLAN_THAT_OWNS_CASTLE_CANNOT_PARTICIPATE_OTHER_SIEGE = new SystemMessage(689);

        /// <summary>
        /// ID: 690
        /// <para/>
        /// Message: You cannot register on the attacking side because you are part of an alliance with the clan that owns the castle.
        /// </summary>
        public static SystemMessage CANNOT_ATTACK_ALLIANCE_CASTLE = new SystemMessage(690);

        /// <summary>
        /// ID: 691
        /// <para/>
        /// Message: $s1 clan is already a member of $s2 alliance.
        /// </summary>
        public static SystemMessage S1_CLAN_ALREADY_MEMBER_OF_S2_ALLIANCE = new SystemMessage(691);

        /// <summary>
        /// ID: 692
        /// <para/>
        /// Message: The other party is frozen. Please wait a moment.
        /// </summary>
        public static SystemMessage OTHER_PARTY_IS_FROZEN = new SystemMessage(692);

        /// <summary>
        /// ID: 693
        /// <para/>
        /// Message: The package that arrived is in another warehouse.
        /// </summary>
        public static SystemMessage PACKAGE_IN_ANOTHER_WAREHOUSE = new SystemMessage(693);

        /// <summary>
        /// ID: 694
        /// <para/>
        /// Message: No packages have arrived.
        /// </summary>
        public static SystemMessage NO_PACKAGES_ARRIVED = new SystemMessage(694);

        /// <summary>
        /// ID: 695
        /// <para/>
        /// Message: You cannot set the name of the pet.
        /// </summary>
        public static SystemMessage NAMING_YOU_CANNOT_SET_NAME_OF_THE_PET = new SystemMessage(695);

        /// <summary>
        /// ID: 697
        /// <para/>
        /// Message: The item enchant value is strange
        /// </summary>
        public static SystemMessage ITEM_ENCHANT_VALUE_STRANGE = new SystemMessage(697);

        /// <summary>
        /// ID: 698
        /// <para/>
        /// Message: The price is different than the same item on the sales list.
        /// </summary>
        public static SystemMessage PRICE_DIFFERENT_FROM_SALES_LIST = new SystemMessage(698);

        /// <summary>
        /// ID: 699
        /// <para/>
        /// Message: Currently not purchasing.
        /// </summary>
        public static SystemMessage CURRENTLY_NOT_PURCHASING = new SystemMessage(699);

        /// <summary>
        /// ID: 700
        /// <para/>
        /// Message: The purchase is complete.
        /// </summary>
        public static SystemMessage THE_PURCHASE_IS_COMPLETE = new SystemMessage(700);

        /// <summary>
        /// ID: 701
        /// <para/>
        /// Message: You do not have enough required items.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_REQUIRED_ITEMS = new SystemMessage(701);

        /// <summary>
        /// ID: 702 
        /// <para/>
        /// Message: There are no GMs currently visible in the public list as they may be performing other functions at the moment.
        /// </summary>
        public static SystemMessage NO_GM_PROVIDING_SERVICE_NOW = new SystemMessage(702);

        /// <summary>
        /// ID: 703
        /// <para/>
        /// Message: ======<GM List>======
        /// </summary>
        public static SystemMessage GM_LIST = new SystemMessage(703);

        /// <summary>
        /// ID: 704
        /// <para/>
        /// Message: GM : $s1
        /// </summary>
        public static SystemMessage GM_S1 = new SystemMessage(704);

        /// <summary>
        /// ID: 705
        /// <para/>
        /// Message: You cannot exclude yourself.
        /// </summary>
        public static SystemMessage CANNOT_EXCLUDE_SELF = new SystemMessage(705);

        /// <summary>
        /// ID: 706
        /// <para/>
        /// Message: You can only register up to 64 names on your exclude list.
        /// </summary>
        public static SystemMessage ONLY_64_NAMES_ON_EXCLUDE_LIST = new SystemMessage(706);

        /// <summary>
        /// ID: 707
        /// <para/>
        /// Message: You cannot teleport to a village that is in a siege.
        /// </summary>
        public static SystemMessage NO_PORT_THAT_IS_IN_SIGE = new SystemMessage(707);

        /// <summary>
        /// ID: 708
        /// <para/>
        /// Message: You do not have the right to use the castle warehouse.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_HAVE_THE_RIGHT_TO_USE_CASTLE_WAREHOUSE = new SystemMessage(708);

        /// <summary>
        /// ID: 709
        /// <para/>
        /// Message: You do not have the right to use the clan warehouse.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_HAVE_THE_RIGHT_TO_USE_CLAN_WAREHOUSE = new SystemMessage(709);

        /// <summary>
        /// ID: 710
        /// <para/>
        /// Message: Only clans of clan level 1 or higher can use a clan warehouse.
        /// </summary>
        public static SystemMessage ONLY_LEVEL_1_CLAN_OR_HIGHER_CAN_USE_WAREHOUSE = new SystemMessage(710);

        /// <summary>
        /// ID: 711
        /// <para/>
        /// Message: The siege of $s1 has started.
        /// </summary>
        public static SystemMessage SIEGE_OF_S1_HAS_STARTED = new SystemMessage(711);

        /// <summary>
        /// ID: 712
        /// <para/>
        /// Message: The siege of $s1 has finished.
        /// </summary>
        public static SystemMessage SIEGE_OF_S1_HAS_ENDED = new SystemMessage(712);

        /// <summary>
        /// ID: 713
        /// <para/>
        /// Message: $s1/$s2/$s3 :
        /// </summary>
        public static SystemMessage S1_S2_S3_D = new SystemMessage(713);

        /// <summary>
        /// ID: 714
        /// <para/>
        /// Message: A trap device has been tripped.
        /// </summary>
        public static SystemMessage A_TRAP_DEVICE_HAS_BEEN_TRIPPED = new SystemMessage(714);

        /// <summary>
        /// ID: 715
        /// <para/>
        /// Message: A trap device has been stopped.
        /// </summary>
        public static SystemMessage A_TRAP_DEVICE_HAS_BEEN_STOPPED = new SystemMessage(715);

        /// <summary>
        /// ID: 716
        /// <para/>
        /// Message: If a base camp does not exist, resurrection is not possible.
        /// </summary>
        public static SystemMessage NO_RESURRECTION_WITHOUT_BASE_CAMP = new SystemMessage(716);

        /// <summary>
        /// ID: 717
        /// <para/>
        /// Message: The guardian tower has been destroyed and resurrection is not possible
        /// </summary>
        public static SystemMessage TOWER_DESTROYED_NO_RESURRECTION = new SystemMessage(717);

        /// <summary>
        /// ID: 718
        /// <para/>
        /// Message: The castle gates cannot be opened and closed during a siege.
        /// </summary>
        public static SystemMessage GATES_NOT_OPENED_CLOSED_DURING_SIEGE = new SystemMessage(718);

        /// <summary>
        /// ID: 719
        /// <para/>
        /// Message: You failed at mixing the item.
        /// </summary>
        public static SystemMessage ITEM_MIXING_FAILED = new SystemMessage(719);

        /// <summary>
        /// ID: 720
        /// <para/>
        /// Message: The purchase price is higher than the amount of money that you have and so you cannot open a personal store.
        /// </summary>
        public static SystemMessage THE_PURCHASE_PRICE_IS_HIGHER_THAN_MONEY = new SystemMessage(720);

        /// <summary>
        /// ID: 721
        /// <para/>
        /// Message: You cannot create an alliance while participating in a siege.
        /// </summary>
        public static SystemMessage NO_ALLY_CREATION_WHILE_SIEGE = new SystemMessage(721);

        /// <summary>
        /// ID: 722
        /// <para/>
        /// Message: You cannot dissolve an alliance while an affiliated clan is participating in a siege battle.
        /// </summary>
        public static SystemMessage CANNOT_DISSOLVE_ALLY_WHILE_IN_SIEGE = new SystemMessage(722);

        /// <summary>
        /// ID: 723
        /// <para/>
        /// Message: The opposing clan is participating in a siege battle.
        /// </summary>
        public static SystemMessage OPPOSING_CLAN_IS_PARTICIPATING_IN_SIEGE = new SystemMessage(723);

        /// <summary>
        /// ID: 724
        /// <para/>
        /// Message: You cannot leave while participating in a siege battle.
        /// </summary>
        public static SystemMessage CANNOT_LEAVE_WHILE_SIEGE = new SystemMessage(724);

        /// <summary>
        /// ID: 725
        /// <para/>
        /// Message: You cannot banish a clan from an alliance while the clan is participating in a siege
        /// </summary>
        public static SystemMessage CANNOT_DISMISS_WHILE_SIEGE = new SystemMessage(725);

        /// <summary>
        /// ID: 726
        /// <para/>
        /// Message: Frozen condition has started. Please wait a moment.
        /// </summary>
        public static SystemMessage FROZEN_CONDITION_STARTED = new SystemMessage(726);

        /// <summary>
        /// ID: 727
        /// <para/>
        /// Message: The frozen condition was removed.
        /// </summary>
        public static SystemMessage FROZEN_CONDITION_REMOVED = new SystemMessage(727);

        /// <summary>
        /// ID: 728
        /// <para/>
        /// Message: You cannot apply for dissolution again within seven days after a previous application for dissolution.
        /// </summary>
        public static SystemMessage CANNOT_APPLY_DISSOLUTION_AGAIN = new SystemMessage(728);

        /// <summary>
        /// ID: 729
        /// <para/>
        /// Message: That item cannot be discarded.
        /// </summary>
        public static SystemMessage ITEM_NOT_DISCARDED = new SystemMessage(729);

        /// <summary>
        /// ID: 730
        /// <para/>
        /// Message: - You have submitted your $s1th petition. - You may submit $s2 more petition(s) today.
        /// </summary>
        public static SystemMessage SUBMITTED_YOU_S1_TH_PETITION_S2_LEFT = new SystemMessage(730);

        /// <summary>
        /// ID: 731
        /// <para/>
        /// Message: A petition has been received by the GM on behalf of $s1. The petition code is $s2.
        /// </summary>
        public static SystemMessage PETITION_S1_RECEIVED_CODE_IS_S2 = new SystemMessage(731);

        /// <summary>
        /// ID: 732
        /// <para/>
        /// Message: $s1 has received a request for a consultation with the GM.
        /// </summary>
        public static SystemMessage S1_RECEIVED_CONSULTATION_REQUEST = new SystemMessage(732);

        /// <summary>
        /// ID: 733
        /// <para/>
        /// Message: We have received $s1 petitions from you today and that is the maximum that you can submit in one day. You cannot submit any more petitions.
        /// </summary>
        public static SystemMessage WE_HAVE_RECEIVED_S1_PETITIONS_TODAY = new SystemMessage(733);

        /// <summary>
        /// ID: 734
        /// <para/>
        /// Message: You have failed at submitting a petition on behalf of someone else. $s1 already submitted a petition.
        /// </summary>
        public static SystemMessage PETITION_FAILED_S1_ALREADY_SUBMITTED = new SystemMessage(734);

        /// <summary>
        /// ID: 735
        /// <para/>
        /// Message: You have failed at submitting a petition on behalf of $s1. The error number is $s2.
        /// </summary>
        public static SystemMessage PETITION_FAILED_FOR_S1_ERROR_NUMBER_S2 = new SystemMessage(735);

        /// <summary>
        /// ID: 736
        /// <para/>
        /// Message: The petition was canceled. You may submit $s1 more petition(s) today.
        /// </summary>
        public static SystemMessage PETITION_CANCELED_SUBMIT_S1_MORE_TODAY = new SystemMessage(736);

        /// <summary>
        /// ID: 737
        /// <para/>
        /// Message: You have cancelled submitting a petition on behalf of $s1.
        /// </summary>
        public static SystemMessage CANCELED_PETITION_ON_S1 = new SystemMessage(737);

        /// <summary>
        /// ID: 738
        /// <para/>
        /// Message: You have not submitted a petition.
        /// </summary>
        public static SystemMessage PETITION_NOT_SUBMITTED = new SystemMessage(738);

        /// <summary>
        /// ID: 739
        /// <para/>
        /// Message: You have failed at cancelling a petition on behalf of $s1. The error number is $s2.
        /// </summary>
        public static SystemMessage PETITION_CANCEL_FAILED_FOR_S1_ERROR_NUMBER_S2 = new SystemMessage(739);

        /// <summary>
        /// ID: 740
        /// <para/>
        /// Message: $s1 participated in a petition chat at the request of the GM.
        /// </summary>
        public static SystemMessage S1_PARTICIPATE_PETITION = new SystemMessage(740);

        /// <summary>
        /// ID: 741
        /// <para/>
        /// Message: You have failed at adding $s1 to the petition chat. Petition has already been submitted.
        /// </summary>
        public static SystemMessage FAILED_ADDING_S1_TO_PETITION = new SystemMessage(741);

        /// <summary>
        /// ID: 742
        /// <para/>
        /// Message: You have failed at adding $s1 to the petition chat. The error code is $s2.
        /// </summary>
        public static SystemMessage PETITION_ADDING_S1_FAILED_ERROR_NUMBER_S2 = new SystemMessage(742);

        /// <summary>
        /// ID: 743
        /// <para/>
        /// Message: $s1 left the petition chat.
        /// </summary>
        public static SystemMessage S1_LEFT_PETITION_CHAT = new SystemMessage(743);

        /// <summary>
        /// ID: 744
        /// <para/>
        /// Message: You have failed at removing $s1 from the petition chat. The error code is $s2.
        /// </summary>
        public static SystemMessage PETITION_REMOVING_S1_FAILED_ERROR_NUMBER_S2 = new SystemMessage(744);

        /// <summary>
        /// ID: 745
        /// <para/>
        /// Message: You are currently not in a petition chat.
        /// </summary>
        public static SystemMessage YOU_ARE_NOT_IN_PETITION_CHAT = new SystemMessage(745);

        /// <summary>
        /// ID: 746
        /// <para/>
        /// Message: It is not currently a petition.
        /// </summary>
        public static SystemMessage CURRENTLY_NO_PETITION = new SystemMessage(746);

        /// <summary>
        /// ID: 748
        /// <para/>
        /// Message: The distance is too far and so the casting has been stopped.
        /// </summary>
        public static SystemMessage DIST_TOO_FAR_CASTING_STOPPED = new SystemMessage(748);

        /// <summary>
        /// ID: 749
        /// <para/>
        /// Message: The effect of $s1 has been removed.
        /// </summary>
        public static SystemMessage EFFECT_S1_DISAPPEARED = new SystemMessage(749);

        /// <summary>
        /// ID: 750
        /// <para/>
        /// Message: There are no other skills to learn.
        /// </summary>
        public static SystemMessage NO_MORE_SKILLS_TO_LEARN = new SystemMessage(750);

        /// <summary>
        /// ID: 751
        /// <para/>
        /// Message: As there is a conflict in the siege relationship with a clan in the alliance, you cannot invite that clan to the alliance.
        /// </summary>
        public static SystemMessage CANNOT_INVITE_CONFLICT_CLAN = new SystemMessage(751);

        /// <summary>
        /// ID: 752
        /// <para/>
        /// Message: That name cannot be used.
        /// </summary>
        public static SystemMessage CANNOT_USE_NAME = new SystemMessage(752);

        /// <summary>
        /// ID: 753
        /// <para/>
        /// Message: You cannot position mercenaries here.
        /// </summary>
        public static SystemMessage NO_MERCS_HERE = new SystemMessage(753);

        /// <summary>
        /// ID: 754
        /// <para/>
        /// Message: There are $s1 hours and $s2 minutes left in this week's usage time.
        /// </summary>
        public static SystemMessage S1_HOURS_S2_MINUTES_LEFT_THIS_WEEK = new SystemMessage(754);

        /// <summary>
        /// ID: 755
        /// <para/>
        /// Message: There are $s1 minutes left in this week's usage time.
        /// </summary>
        public static SystemMessage S1_MINUTES_LEFT_THIS_WEEK = new SystemMessage(755);

        /// <summary>
        /// ID: 756
        /// <para/>
        /// Message: This week's usage time has finished.
        /// </summary>
        public static SystemMessage WEEKS_USAGE_TIME_FINISHED = new SystemMessage(756);

        /// <summary>
        /// ID: 757
        /// <para/>
        /// Message: There are $s1 hours and $s2 minutes left in the fixed use time.
        /// </summary>
        public static SystemMessage S1_HOURS_S2_MINUTES_LEFT_IN_TIME = new SystemMessage(757);

        /// <summary>
        /// ID: 758
        /// <para/>
        /// Message: There are $s1 hours and $s2 minutes left in this week's play time.
        /// </summary>
        public static SystemMessage S1_HOURS_S2_MINUTES_LEFT_THIS_WEEKS_PLAY_TIME = new SystemMessage(758);

        /// <summary>
        /// ID: 759
        /// <para/>
        /// Message: There are $s1 minutes left in this week's play time.
        /// </summary>
        public static SystemMessage S1_MINUTES_LEFT_THIS_WEEKS_PLAY_TIME = new SystemMessage(759);

        /// <summary>
        /// ID: 760
        /// <para/>
        /// Message: $s1 cannot join the clan because one day has not yet passed since he/she left another clan.
        /// </summary>
        public static SystemMessage S1_MUST_WAIT_BEFORE_JOINING_ANOTHER_CLAN = new SystemMessage(760);

        /// <summary>
        /// ID: 761
        /// <para/>
        /// Message: $s1 clan cannot join the alliance because one day has not yet passed since it left another alliance.
        /// </summary>
        public static SystemMessage S1_CANT_ENTER_ALLIANCE_WITHIN_1_DAY = new SystemMessage(761);

        /// <summary>
        /// ID: 762
        /// <para/>
        /// Message: $s1 rolled $s2 and $s3's eye came out.
        /// </summary>
        public static SystemMessage S1_ROLLED_S2_S3_EYE_CAME_OUT = new SystemMessage(762);

        /// <summary>
        /// ID: 763
        /// <para/>
        /// Message: You failed at sending the package because you are too far from the warehouse.
        /// </summary>
        public static SystemMessage FAILED_SENDING_PACKAGE_TOO_FAR = new SystemMessage(763);

        /// <summary>
        /// ID: 764
        /// <para/>
        /// Message: You have been playing for an extended period of time. Please consider taking a break.
        /// </summary>
        public static SystemMessage PLAYING_FOR_LONG_TIME = new SystemMessage(764);

        /// <summary>
        /// ID: 769
        /// <para/>
        /// Message: A hacking tool has been discovered. Please try again after closing unnecessary programs.
        /// </summary>
        public static SystemMessage HACKING_TOOL = new SystemMessage(769);

        /// <summary>
        /// ID: 774
        /// <para/>
        /// Message: Play time is no longer accumulating.
        /// </summary>
        public static SystemMessage PLAY_TIME_NO_LONGER_ACCUMULATING = new SystemMessage(774);

        /// <summary>
        /// ID: 775
        /// <para/>
        /// Message: From here on, play time will be expended.
        /// </summary>
        public static SystemMessage PLAY_TIME_EXPENDED = new SystemMessage(775);

        /// <summary>
        /// ID: 776
        /// <para/>
        /// Message: The clan hall which was put up for auction has been awarded to clan s1.
        /// </summary>
        public static SystemMessage CLANHALL_AWARDED_TO_CLAN_S1 = new SystemMessage(776);

        /// <summary>
        /// ID: 777
        /// <para/>
        /// Message: The clan hall which was put up for auction was not sold and therefore has been re-listed.
        /// </summary>
        public static SystemMessage CLANHALL_NOT_SOLD = new SystemMessage(777);

        /// <summary>
        /// ID: 778
        /// <para/>
        /// Message: You may not log out from this location.
        /// </summary>
        public static SystemMessage NO_LOGOUT_HERE = new SystemMessage(778);

        /// <summary>
        /// ID: 779
        /// <para/>
        /// Message: You may not restart in this location.
        /// </summary>
        public static SystemMessage NO_RESTART_HERE = new SystemMessage(779);

        /// <summary>
        /// ID: 780
        /// <para/>
        /// Message: Observation is only possible during a siege.
        /// </summary>
        public static SystemMessage ONLY_VIEW_SIEGE = new SystemMessage(780);

        /// <summary>
        /// ID: 781
        /// <para/>
        /// Message: Observers cannot participate.
        /// </summary>
        public static SystemMessage OBSERVERS_CANNOT_PARTICIPATE = new SystemMessage(781);

        /// <summary>
        /// ID: 782
        /// <para/>
        /// Message: You may not observe a siege with a pet or servitor summoned.
        /// </summary>
        public static SystemMessage NO_OBSERVE_WITH_PET = new SystemMessage(782);

        /// <summary>
        /// ID: 783
        /// <para/>
        /// Message: Lottery ticket sales have been temporarily suspended.
        /// </summary>
        public static SystemMessage LOTTERY_TICKET_SALES_TEMP_SUSPENDED = new SystemMessage(783);

        /// <summary>
        /// ID: 784
        /// <para/>
        /// Message: Tickets for the current lottery are no longer available.
        /// </summary>
        public static SystemMessage NO_LOTTERY_TICKETS_AVAILABLE = new SystemMessage(784);

        /// <summary>
        /// ID: 785
        /// <para/>
        /// Message: The results of lottery number $s1 have not yet been published.
        /// </summary>
        public static SystemMessage LOTTERY_S1_RESULT_NOT_PUBLISHED = new SystemMessage(785);

        /// <summary>
        /// ID: 786
        /// <para/>
        /// Message: Incorrect syntax.
        /// </summary>
        public static SystemMessage INCORRECT_SYNTAX = new SystemMessage(786);

        /// <summary>
        /// ID: 787
        /// <para/>
        /// Message: The tryouts are finished.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_TRYOUTS_FINISHED = new SystemMessage(787);

        /// <summary>
        /// ID: 788
        /// <para/>
        /// Message: The finals are finished.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_FINALS_FINISHED = new SystemMessage(788);

        /// <summary>
        /// ID: 789
        /// <para/>
        /// Message: The tryouts have begun.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_TRYOUTS_BEGUN = new SystemMessage(789);

        /// <summary>
        /// ID: 790
        /// <para/>
        /// Message: The finals are finished.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_FINALS_BEGUN = new SystemMessage(790);

        /// <summary>
        /// ID: 791
        /// <para/>
        /// Message: The final match is about to begin. Line up!
        /// </summary>
        public static SystemMessage FINAL_MATCH_BEGIN = new SystemMessage(791);

        /// <summary>
        /// ID: 792
        /// <para/>
        /// Message: The siege of the clan hall is finished.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_ENDED = new SystemMessage(792);

        /// <summary>
        /// ID: 793
        /// <para/>
        /// Message: The siege of the clan hall has begun.
        /// </summary>
        public static SystemMessage CLANHALL_SIEGE_BEGUN = new SystemMessage(793);

        /// <summary>
        /// ID: 794
        /// <para/>
        /// Message: You are not authorized to do that.
        /// </summary>
        public static SystemMessage YOU_ARE_NOT_AUTHORIZED_TO_DO_THAT = new SystemMessage(794);

        /// <summary>
        /// ID: 795
        /// <para/>
        /// Message: Only clan leaders are authorized to set rights.
        /// </summary>
        public static SystemMessage ONLY_LEADERS_CAN_SET_RIGHTS = new SystemMessage(795);

        /// <summary>
        /// ID: 796
        /// <para/>
        /// Message: Your remaining observation time is minutes.
        /// </summary>
        public static SystemMessage REMAINING_OBSERVATION_TIME = new SystemMessage(796);

        /// <summary>
        /// ID: 797
        /// <para/>
        /// Message: You may create up to 24 macros.
        /// </summary>
        public static SystemMessage YOU_MAY_CREATE_UP_TO_24_MACROS = new SystemMessage(797);

        /// <summary>
        /// ID: 798
        /// <para/>
        /// Message: Item registration is irreversible. Do you wish to continue?
        /// </summary>
        public static SystemMessage ITEM_REGISTRATION_IRREVERSIBLE = new SystemMessage(798);

        /// <summary>
        /// ID: 799
        /// <para/>
        /// Message: The observation time has expired.
        /// </summary>
        public static SystemMessage OBSERVATION_TIME_EXPIRED = new SystemMessage(799);

        /// <summary>
        /// ID: 800
        /// <para/>
        /// Message: You are too late. The registration period is over.
        /// </summary>
        public static SystemMessage REGISTRATION_PERIOD_OVER = new SystemMessage(800);

        /// <summary>
        /// ID: 801
        /// <para/>
        /// Message: Registration for the clan hall siege is closed.
        /// </summary>
        public static SystemMessage REGISTRATION_CLOSED = new SystemMessage(801);

        /// <summary>
        /// ID: 802
        /// <para/>
        /// Message: Petitions are not being accepted at this time. You may submit your petition after a.m./p.m.
        /// </summary>
        public static SystemMessage PETITION_NOT_ACCEPTED_NOW = new SystemMessage(802);

        /// <summary>
        /// ID: 803
        /// <para/>
        /// Message: Enter the specifics of your petition.
        /// </summary>
        public static SystemMessage PETITION_NOT_SPECIFIED = new SystemMessage(803);

        /// <summary>
        /// ID: 804
        /// <para/>
        /// Message: Select a type.
        /// </summary>
        public static SystemMessage SELECT_TYPE = new SystemMessage(804);

        /// <summary>
        /// ID: 805
        /// <para/>
        /// Message: Petitions are not being accepted at this time. You may submit your petition after $s1 a.m./p.m.
        /// </summary>
        public static SystemMessage PETITION_NOT_ACCEPTED_SUBMIT_AT_S1 = new SystemMessage(805);

        /// <summary>
        /// ID: 806
        /// <para/>
        /// Message: If you are trapped, try typing "/unstuck".
        /// </summary>
        public static SystemMessage TRY_UNSTUCK_WHEN_TRAPPED = new SystemMessage(806);

        /// <summary>
        /// ID: 807
        /// <para/>
        /// Message: This terrain is navigable. Prepare for transport to the nearest village.
        /// </summary>
        public static SystemMessage STUCK_PREPARE_FOR_TRANSPORT = new SystemMessage(807);

        /// <summary>
        /// ID: 808
        /// <para/>
        /// Message: You are stuck. You may submit a petition by typing "/gm".
        /// </summary>
        public static SystemMessage STUCK_SUBMIT_PETITION = new SystemMessage(808);

        /// <summary>
        /// ID: 809
        /// <para/>
        /// Message: You are stuck. You will be transported to the nearest village in five minutes.
        /// </summary>
        public static SystemMessage STUCK_TRANSPORT_IN_FIVE_MINUTES = new SystemMessage(809);

        /// <summary>
        /// ID: 810
        /// <para/>
        /// Message: Invalid macro. Refer to the Help file for instructions.
        /// </summary>
        public static SystemMessage INVALID_MACRO = new SystemMessage(810);

        /// <summary>
        /// ID: 811
        /// <para/>
        /// Message: You will be moved to (). Do you wish to continue?
        /// </summary>
        public static SystemMessage WILL_BE_MOVED = new SystemMessage(811);

        /// <summary>
        /// ID: 812
        /// <para/>
        /// Message: The secret trap has inflicted $s1 damage on you.
        /// </summary>
        public static SystemMessage TRAP_DID_S1_DAMAGE = new SystemMessage(812);

        /// <summary>
        /// ID: 813
        /// <para/>
        /// Message: You have been poisoned by a Secret Trap.
        /// </summary>
        public static SystemMessage POISONED_BY_TRAP = new SystemMessage(813);

        /// <summary>
        /// ID: 814
        /// <para/>
        /// Message: Your speed has been decreased by a Secret Trap.
        /// </summary>
        public static SystemMessage SLOWED_BY_TRAP = new SystemMessage(814);

        /// <summary>
        /// ID: 815
        /// <para/>
        /// Message: The tryouts are about to begin. Line up!
        /// </summary>
        public static SystemMessage TRYOUTS_ABOUT_TO_BEGIN = new SystemMessage(815);

        /// <summary>
        /// ID: 816
        /// <para/>
        /// Message: Tickets are now available for Monster Race $s1!
        /// </summary>
        public static SystemMessage MONSRACE_TICKETS_AVAILABLE_FOR_S1_RACE = new SystemMessage(816);

        /// <summary>
        /// ID: 817
        /// <para/>
        /// Message: Now selling tickets for Monster Race $s1!
        /// </summary>
        public static SystemMessage MONSRACE_TICKETS_NOW_AVAILABLE_FOR_S1_RACE = new SystemMessage(817);

        /// <summary>
        /// ID: 818
        /// <para/>
        /// Message: Ticket sales for the Monster Race will end in $s1 minute(s).
        /// </summary>
        public static SystemMessage MONSRACE_TICKETS_STOP_IN_S1_MINUTES = new SystemMessage(818);

        /// <summary>
        /// ID: 819
        /// <para/>
        /// Message: Tickets sales are closed for Monster Race $s1. Odds are posted.
        /// </summary>
        public static SystemMessage MONSRACE_S1_TICKET_SALES_CLOSED = new SystemMessage(819);

        /// <summary>
        /// ID: 820
        /// <para/>
        /// Message: Monster Race $s2 will begin in $s1 minute(s)!
        /// </summary>
        public static SystemMessage MONSRACE_S2_BEGINS_IN_S1_MINUTES = new SystemMessage(820);

        /// <summary>
        /// ID: 821
        /// <para/>
        /// Message: Monster Race $s1 will begin in 30 seconds!
        /// </summary>
        public static SystemMessage MONSRACE_S1_BEGINS_IN_30_SECONDS = new SystemMessage(821);

        /// <summary>
        /// ID: 822
        /// <para/>
        /// Message: Monster Race $s1 is about to begin! Countdown in five seconds!
        /// </summary>
        public static SystemMessage MONSRACE_S1_COUNTDOWN_IN_FIVE_SECONDS = new SystemMessage(822);

        /// <summary>
        /// ID: 823
        /// <para/>
        /// Message: The race will begin in $s1 second(s)!
        /// </summary>
        public static SystemMessage MONSRACE_BEGINS_IN_S1_SECONDS = new SystemMessage(823);

        /// <summary>
        /// ID: 824
        /// <para/>
        /// Message: They're off!
        /// </summary>
        public static SystemMessage MONSRACE_RACE_START = new SystemMessage(824);

        /// <summary>
        /// ID: 825
        /// <para/>
        /// Message: Monster Race $s1 is finished!
        /// </summary>
        public static SystemMessage MONSRACE_S1_RACE_END = new SystemMessage(825);

        /// <summary>
        /// ID: 826
        /// <para/>
        /// Message: First prize goes to the player in lane $s1. Second prize goes to the player in lane $s2.
        /// </summary>
        public static SystemMessage MONSRACE_FIRST_PLACE_S1_SECOND_S2 = new SystemMessage(826);

        /// <summary>
        /// ID: 827
        /// <para/>
        /// Message: You may not impose a block on a GM.
        /// </summary>
        public static SystemMessage YOU_MAY_NOT_IMPOSE_A_BLOCK_ON_GM = new SystemMessage(827);

        /// <summary>
        /// ID: 828
        /// <para/>
        /// Message: Are you sure you wish to delete the $s1 macro?
        /// </summary>
        public static SystemMessage WISH_TO_DELETE_S1_MACRO = new SystemMessage(828);

        /// <summary>
        /// ID: 829
        /// <para/>
        /// Message: You cannot recommend yourself.
        /// </summary>
        public static SystemMessage YOU_CANNOT_RECOMMEND_YOURSELF = new SystemMessage(829);

        /// <summary>
        /// ID: 830
        /// <para/>
        /// Message: You have recommended $s1. You have $s2 recommendations left.
        /// </summary>
        public static SystemMessage YOU_HAVE_RECOMMENDED_S1_YOU_HAVE_S2_RECOMMENDATIONS_LEFT = new SystemMessage(830);

        /// <summary>
        /// ID: 831
        /// <para/>
        /// Message: You have been recommended by $s1.
        /// </summary>
        public static SystemMessage YOU_HAVE_BEEN_RECOMMENDED_BY_S1 = new SystemMessage(831);

        /// <summary>
        /// ID: 832
        /// <para/>
        /// Message: That character has already been recommended.
        /// </summary>
        public static SystemMessage THAT_CHARACTER_IS_RECOMMENDED = new SystemMessage(832);

        /// <summary>
        /// ID: 833
        /// <para/>
        /// Message: You are not authorized to make further recommendations at this time. You will receive more recommendation credits each day at 1 p.m.
        /// </summary>
        public static SystemMessage NO_MORE_RECOMMENDATIONS_TO_HAVE = new SystemMessage(833);

        /// <summary>
        /// ID: 834
        /// <para/>
        /// Message: $s1 has rolled $s2.
        /// </summary>
        public static SystemMessage S1_ROLLED_S2 = new SystemMessage(834);

        /// <summary>
        /// ID: 835
        /// <para/>
        /// Message: You may not throw the dice at this time. Try again later.
        /// </summary>
        public static SystemMessage YOU_MAY_NOT_THROW_THE_DICE_AT_THIS_TIME_TRY_AGAIN_LATER = new SystemMessage(835);

        /// <summary>
        /// ID: 836
        /// <para/>
        /// Message: You have exceeded your inventory volume limit and cannot take this item.
        /// </summary>
        public static SystemMessage YOU_HAVE_EXCEEDED_YOUR_INVENTORY_VOLUME_LIMIT_AND_CANNOT_TAKE_THIS_ITEM = new SystemMessage(836);

        /// <summary>
        /// ID: 837
        /// <para/>
        /// Message: Macro descriptions may contain up to 32 characters.
        /// </summary>
        public static SystemMessage MACRO_DESCRIPTION_MAX_32_CHARS = new SystemMessage(837);

        /// <summary>
        /// ID: 838
        /// <para/>
        /// Message: Enter the name of the macro.
        /// </summary>
        public static SystemMessage ENTER_THE_MACRO_NAME = new SystemMessage(838);

        /// <summary>
        /// ID: 839
        /// <para/>
        /// Message: That name is already assigned to another macro.
        /// </summary>
        public static SystemMessage MACRO_NAME_ALREADY_USED = new SystemMessage(839);

        /// <summary>
        /// ID: 840
        /// <para/>
        /// Message: That recipe is already registered.
        /// </summary>
        public static SystemMessage RECIPE_ALREADY_REGISTERED = new SystemMessage(840);

        /// <summary>
        /// ID: 841
        /// <para/>
        /// Message: No further recipes may be registered.
        /// </summary>
        public static SystemMessage NO_FUTHER_RECIPES_CAN_BE_ADDED = new SystemMessage(841);

        /// <summary>
        /// ID: 842
        /// <para/>
        /// Message: You are not authorized to register a recipe.
        /// </summary>
        public static SystemMessage NOT_AUTHORIZED_REGISTER_RECIPE = new SystemMessage(842);

        /// <summary>
        /// ID: 843
        /// <para/>
        /// Message: The siege of $s1 is finished.
        /// </summary>
        public static SystemMessage SIEGE_OF_S1_FINISHED = new SystemMessage(843);

        /// <summary>
        /// ID: 844
        /// <para/>
        /// Message: The siege to conquer $s1 has begun.
        /// </summary>
        public static SystemMessage SIEGE_OF_S1_BEGUN = new SystemMessage(844);

        /// <summary>
        /// ID: 845
        /// <para/>
        /// Message: The deadlineto register for the siege of $s1 has passed.
        /// </summary>
        public static SystemMessage DEADLINE_FOR_SIEGE_S1_PASSED = new SystemMessage(845);

        /// <summary>
        /// ID: 846
        /// <para/>
        /// Message: The siege of $s1 has been canceled due to lack of interest.
        /// </summary>
        public static SystemMessage SIEGE_OF_S1_HAS_BEEN_CANCELED_DUE_TO_LACK_OF_INTEREST = new SystemMessage(846);

        /// <summary>
        /// ID: 847
        /// <para/>
        /// Message: A clan that owns a clan hall may not participate in a clan hall siege.
        /// </summary>
        public static SystemMessage CLAN_OWNING_CLANHALL_MAY_NOT_SIEGE_CLANHALL = new SystemMessage(847);

        /// <summary>
        /// ID: 848
        /// <para/>
        /// Message: $s1 has been deleted.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_DELETED = new SystemMessage(848);

        /// <summary>
        /// ID: 849
        /// <para/>
        /// Message: $s1 cannot be found.
        /// </summary>
        public static SystemMessage S1_NOT_FOUND = new SystemMessage(849);

        /// <summary>
        /// ID: 850
        /// <para/>
        /// Message: $s1 already exists.
        /// </summary>
        public static SystemMessage S1_ALREADY_EXISTS2 = new SystemMessage(850);

        /// <summary>
        /// ID: 851
        /// <para/>
        /// Message: $s1 has been added.
        /// </summary>
        public static SystemMessage S1_ADDED = new SystemMessage(851);

        /// <summary>
        /// ID: 852
        /// <para/>
        /// Message: The recipe is incorrect.
        /// </summary>
        public static SystemMessage RECIPE_INCORRECT = new SystemMessage(852);

        /// <summary>
        /// ID: 853
        /// <para/>
        /// Message: You may not alter your recipe book while engaged in manufacturing.
        /// </summary>
        public static SystemMessage CANT_ALTER_RECIPEBOOK_WHILE_CRAFTING = new SystemMessage(853);

        /// <summary>
        /// ID: 854
        /// <para/>
        /// Message: You are missing $s2 $s1 required to create that.
        /// </summary>
        public static SystemMessage MISSING_S2_S1_TO_CREATE = new SystemMessage(854);

        /// <summary>
        /// ID: 855
        /// <para/>
        /// Message: $s1 clan has defeated $s2.
        /// </summary>
        public static SystemMessage S1_CLAN_DEFEATED_S2 = new SystemMessage(855);

        /// <summary>
        /// ID: 856
        /// <para/>
        /// Message: The siege of $s1 has ended in a draw.
        /// </summary>
        public static SystemMessage SIEGE_S1_DRAW = new SystemMessage(856);

        /// <summary>
        /// ID: 857
        /// <para/>
        /// Message: $s1 clan has won in the preliminary match of $s2.
        /// </summary>
        public static SystemMessage S1_CLAN_WON_MATCH_S2 = new SystemMessage(857);

        /// <summary>
        /// ID: 858
        /// <para/>
        /// Message: The preliminary match of $s1 has ended in a draw.
        /// </summary>
        public static SystemMessage MATCH_OF_S1_DRAW = new SystemMessage(858);

        /// <summary>
        /// ID: 859
        /// <para/>
        /// Message: Please register a recipe.
        /// </summary>
        public static SystemMessage PLEASE_REGISTER_RECIPE = new SystemMessage(859);

        /// <summary>
        /// ID: 860
        /// <para/>
        /// Message: You may not buld your headquarters in close proximity to another headquarters.
        /// </summary>
        public static SystemMessage HEADQUARTERS_TOO_CLOSE = new SystemMessage(860);

        /// <summary>
        /// ID: 861
        /// <para/>
        /// Message: You have exceeded the maximum number of memos.
        /// </summary>
        public static SystemMessage TOO_MANY_MEMOS = new SystemMessage(861);

        /// <summary>
        /// ID: 862
        /// <para/>
        /// Message: Odds are not posted until ticket sales have closed.
        /// </summary>
        public static SystemMessage ODDS_NOT_POSTED = new SystemMessage(862);

        /// <summary>
        /// ID: 863
        /// <para/>
        /// Message: You feel the energy of fire.
        /// </summary>
        public static SystemMessage FEEL_ENERGY_FIRE = new SystemMessage(863);

        /// <summary>
        /// ID: 864
        /// <para/>
        /// Message: You feel the energy of water.
        /// </summary>
        public static SystemMessage FEEL_ENERGY_WATER = new SystemMessage(864);

        /// <summary>
        /// ID: 865
        /// <para/>
        /// Message: You feel the energy of wind.
        /// </summary>
        public static SystemMessage FEEL_ENERGY_WIND = new SystemMessage(865);

        /// <summary>
        /// ID: 866
        /// <para/>
        /// Message: You may no longer gather energy.
        /// </summary>
        public static SystemMessage NO_LONGER_ENERGY = new SystemMessage(866);

        /// <summary>
        /// ID: 867
        /// <para/>
        /// Message: The energy is depleted.
        /// </summary>
        public static SystemMessage ENERGY_DEPLETED = new SystemMessage(867);

        /// <summary>
        /// ID: 868
        /// <para/>
        /// Message: The energy of fire has been delivered.
        /// </summary>
        public static SystemMessage ENERGY_FIRE_DELIVERED = new SystemMessage(868);

        /// <summary>
        /// ID: 869
        /// <para/>
        /// Message: The energy of water has been delivered.
        /// </summary>
        public static SystemMessage ENERGY_WATER_DELIVERED = new SystemMessage(869);

        /// <summary>
        /// ID: 870
        /// <para/>
        /// Message: The energy of wind has been delivered.
        /// </summary>
        public static SystemMessage ENERGY_WIND_DELIVERED = new SystemMessage(870);

        /// <summary>
        /// ID: 871
        /// <para/>
        /// Message: The seed has been sown.
        /// </summary>
        public static SystemMessage THE_SEED_HAS_BEEN_SOWN = new SystemMessage(871);

        /// <summary>
        /// ID: 872
        /// <para/>
        /// Message: This seed may not be sown here.
        /// </summary>
        public static SystemMessage THIS_SEED_MAY_NOT_BE_SOWN_HERE = new SystemMessage(872);

        /// <summary>
        /// ID: 873
        /// <para/>
        /// Message: That character does not exist.
        /// </summary>
        public static SystemMessage CHARACTER_DOES_NOT_EXIST = new SystemMessage(873);

        /// <summary>
        /// ID: 874
        /// <para/>
        /// Message: The capacity of the warehouse has been exceeded.
        /// </summary>
        public static SystemMessage WAREHOUSE_CAPACITY_EXCEEDED = new SystemMessage(874);

        /// <summary>
        /// ID: 875
        /// <para/>
        /// Message: The transport of the cargo has been canceled.
        /// </summary>
        public static SystemMessage CARGO_CANCELED = new SystemMessage(875);

        /// <summary>
        /// ID: 876
        /// <para/>
        /// Message: The cargo was not delivered.
        /// </summary>
        public static SystemMessage CARGO_NOT_DELIVERED = new SystemMessage(876);

        /// <summary>
        /// ID: 877
        /// <para/>
        /// Message: The symbol has been added.
        /// </summary>
        public static SystemMessage SYMBOL_ADDED = new SystemMessage(877);

        /// <summary>
        /// ID: 878
        /// <para/>
        /// Message: The symbol has been deleted.
        /// </summary>
        public static SystemMessage SYMBOL_DELETED = new SystemMessage(878);

        /// <summary>
        /// ID: 879
        /// <para/>
        /// Message: The manor system is currently under maintenance.
        /// </summary>
        public static SystemMessage THE_MANOR_SYSTEM_IS_CURRENTLY_UNDER_MAINTENANCE = new SystemMessage(879);

        /// <summary>
        /// ID: 880
        /// <para/>
        /// Message: The transaction is complete.
        /// </summary>
        public static SystemMessage THE_TRANSACTION_IS_COMPLETE = new SystemMessage(880);

        /// <summary>
        /// ID: 881
        /// <para/>
        /// Message: There is a discrepancy on the invoice.
        /// </summary>
        public static SystemMessage THERE_IS_A_DISCREPANCY_ON_THE_INVOICE = new SystemMessage(881);

        /// <summary>
        /// ID: 882
        /// <para/>
        /// Message: The seed quantity is incorrect.
        /// </summary>
        public static SystemMessage THE_SEED_QUANTITY_IS_INCORRECT = new SystemMessage(882);

        /// <summary>
        /// ID: 883
        /// <para/>
        /// Message: The seed information is incorrect.
        /// </summary>
        public static SystemMessage THE_SEED_INFORMATION_IS_INCORRECT = new SystemMessage(883);

        /// <summary>
        /// ID: 884
        /// <para/>
        /// Message: The manor information has been updated.
        /// </summary>
        public static SystemMessage THE_MANOR_INFORMATION_HAS_BEEN_UPDATED = new SystemMessage(884);

        /// <summary>
        /// ID: 885
        /// <para/>
        /// Message: The number of crops is incorrect.
        /// </summary>
        public static SystemMessage THE_NUMBER_OF_CROPS_IS_INCORRECT = new SystemMessage(885);

        /// <summary>
        /// ID: 886
        /// <para/>
        /// Message: The crops are priced incorrectly.
        /// </summary>
        public static SystemMessage THE_CROPS_ARE_PRICED_INCORRECTLY = new SystemMessage(886);

        /// <summary>
        /// ID: 887
        /// <para/>
        /// Message: The type is incorrect.
        /// </summary>
        public static SystemMessage THE_TYPE_IS_INCORRECT = new SystemMessage(887);

        /// <summary>
        /// ID: 888
        /// <para/>
        /// Message: No crops can be purchased at this time.
        /// </summary>
        public static SystemMessage NO_CROPS_CAN_BE_PURCHASED_AT_THIS_TIME = new SystemMessage(888);

        /// <summary>
        /// ID: 889
        /// <para/>
        /// Message: The seed was successfully sown.
        /// </summary>
        public static SystemMessage THE_SEED_WAS_SUCCESSFULLY_SOWN = new SystemMessage(889);

        /// <summary>
        /// ID: 890
        /// <para/>
        /// Message: The seed was not sown.
        /// </summary>
        public static SystemMessage THE_SEED_WAS_NOT_SOWN = new SystemMessage(890);

        /// <summary>
        /// ID: 891
        /// <para/>
        /// Message: You are not authorized to harvest.
        /// </summary>
        public static SystemMessage YOU_ARE_NOT_AUTHORIZED_TO_HARVEST = new SystemMessage(891);

        /// <summary>
        /// ID: 892
        /// <para/>
        /// Message: The harvest has failed.
        /// </summary>
        public static SystemMessage THE_HARVEST_HAS_FAILED = new SystemMessage(892);

        /// <summary>
        /// ID: 893
        /// <para/>
        /// Message: The harvest failed because the seed was not sown.
        /// </summary>
        public static SystemMessage THE_HARVEST_FAILED_BECAUSE_THE_SEED_WAS_NOT_SOWN = new SystemMessage(893);

        /// <summary>
        /// ID: 894
        /// <para/>
        /// Message: Up to $s1 recipes can be registered.
        /// </summary>
        public static SystemMessage UP_TO_S1_RECIPES_CAN_REGISTER = new SystemMessage(894);

        /// <summary>
        /// ID: 895
        /// <para/>
        /// Message: No recipes have been registered.
        /// </summary>
        public static SystemMessage NO_RECIPES_REGISTERED = new SystemMessage(895);

        /// <summary>
        /// ID: 896
        /// <para/>
        /// Message:The ferry has arrived at Gludin Harbor.
        /// </summary>
        public static SystemMessage FERRY_AT_GLUDIN = new SystemMessage(896);

        /// <summary>
        /// ID: 897
        /// <para/>
        /// Message:The ferry will leave for Talking Island Harbor after anchoring for ten minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_TALKING = new SystemMessage(897);

        /// <summary>
        /// ID: 898
        /// <para/>
        /// Message: Only characters of level 10 or above are authorized to make recommendations.
        /// </summary>
        public static SystemMessage ONLY_LEVEL_SUP_10_CAN_RECOMMEND = new SystemMessage(898);

        /// <summary>
        /// ID: 899
        /// <para/>
        /// Message: The symbol cannot be drawn.
        /// </summary>
        public static SystemMessage CANT_DRAW_SYMBOL = new SystemMessage(899);

        /// <summary>
        /// ID: 900
        /// <para/>
        /// Message: No slot exists to draw the symbol
        /// </summary>
        public static SystemMessage SYMBOLS_FULL = new SystemMessage(900);

        /// <summary>
        /// ID: 901
        /// <para/>
        /// Message: The symbol information cannot be found.
        /// </summary>
        public static SystemMessage SYMBOL_NOT_FOUND = new SystemMessage(901);

        /// <summary>
        /// ID: 902
        /// <para/>
        /// Message: The number of items is incorrect.
        /// </summary>
        public static SystemMessage NUMBER_INCORRECT = new SystemMessage(902);

        /// <summary>
        /// ID: 903
        /// <para/>
        /// Message: You may not submit a petition while frozen. Be patient.
        /// </summary>
        public static SystemMessage NO_PETITION_WHILE_FROZEN = new SystemMessage(903);

        /// <summary>
        /// ID: 904
        /// <para/>
        /// Message: Items cannot be discarded while in private store status.
        /// </summary>
        public static SystemMessage NO_DISCARD_WHILE_PRIVATE_STORE = new SystemMessage(904);

        /// <summary>
        /// ID: 905
        /// <para/>
        /// Message: The current score for the Humans is $s1.
        /// </summary>
        public static SystemMessage HUMAN_SCORE_S1 = new SystemMessage(905);

        /// <summary>
        /// ID: 906
        /// <para/>
        /// Message: The current score for the Elves is $s1.
        /// </summary>
        public static SystemMessage ELVES_SCORE_S1 = new SystemMessage(906);

        /// <summary>
        /// ID: 907
        /// <para/>
        /// Message: The current score for the Dark Elves is $s1.
        /// </summary>
        public static SystemMessage DARK_ELVES_SCORE_S1 = new SystemMessage(907);

        /// <summary>
        /// ID: 908
        /// <para/>
        /// Message: The current score for the Orcs is $s1.
        /// </summary>
        public static SystemMessage ORCS_SCORE_S1 = new SystemMessage(908);

        /// <summary>
        /// ID: 909
        /// <para/>
        /// Message: The current score for the Dwarves is $s1.
        /// </summary>
        public static SystemMessage DWARVEN_SCORE_S1 = new SystemMessage(909);

        /// <summary>
        /// ID: 910
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Talking Island Village)
        /// </summary>
        public static SystemMessage LOC_TI_S1_S2_S3 = new SystemMessage(910);

        /// <summary>
        /// ID: 911
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Gludin Village)
        /// </summary>
        public static SystemMessage LOC_GLUDIN_S1_S2_S3 = new SystemMessage(911);

        /// <summary>
        /// ID: 912
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Town of Gludio)
        /// </summary>
        public static SystemMessage LOC_GLUDIO_S1_S2_S3 = new SystemMessage(912);

        /// <summary>
        /// ID: 913
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Neutral Zone)
        /// </summary>
        public static SystemMessage LOC_NEUTRAL_ZONE_S1_S2_S3 = new SystemMessage(913);

        /// <summary>
        /// ID: 914
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Elven Village)
        /// </summary>
        public static SystemMessage LOC_ELVEN_S1_S2_S3 = new SystemMessage(914);

        /// <summary>
        /// ID: 915
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Dark Elf Village)
        /// </summary>
        public static SystemMessage LOC_DARK_ELVEN_S1_S2_S3 = new SystemMessage(915);

        /// <summary>
        /// ID: 916
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Town of Dion)
        /// </summary>
        public static SystemMessage LOC_DION_S1_S2_S3 = new SystemMessage(916);

        /// <summary>
        /// ID: 917
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Floran Village)
        /// </summary>
        public static SystemMessage LOC_FLORAN_S1_S2_S3 = new SystemMessage(917);

        /// <summary>
        /// ID: 918
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Town of Giran)
        /// </summary>
        public static SystemMessage LOC_GIRAN_S1_S2_S3 = new SystemMessage(918);

        /// <summary>
        /// ID: 919
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Giran Harbor)
        /// </summary>
        public static SystemMessage LOC_GIRAN_HARBOR_S1_S2_S3 = new SystemMessage(919);

        /// <summary>
        /// ID: 920
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Orc Village)
        /// </summary>
        public static SystemMessage LOC_ORC_S1_S2_S3 = new SystemMessage(920);

        /// <summary>
        /// ID: 921
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Dwarven Village)
        /// </summary>
        public static SystemMessage LOC_DWARVEN_S1_S2_S3 = new SystemMessage(921);

        /// <summary>
        /// ID: 922
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Town of Oren)
        /// </summary>
        public static SystemMessage LOC_OREN_S1_S2_S3 = new SystemMessage(922);

        /// <summary>
        /// ID: 923
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Hunters Village)
        /// </summary>
        public static SystemMessage LOC_HUNTER_S1_S2_S3 = new SystemMessage(923);

        /// <summary>
        /// ID: 924
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Aden Castle Town)
        /// </summary>
        public static SystemMessage LOC_ADEN_S1_S2_S3 = new SystemMessage(924);

        /// <summary>
        /// ID: 925
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near the Coliseum)
        /// </summary>
        public static SystemMessage LOC_COLISEUM_S1_S2_S3 = new SystemMessage(925);

        /// <summary>
        /// ID: 926
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (Near Heine)
        /// </summary>
        public static SystemMessage LOC_HEINE_S1_S2_S3 = new SystemMessage(926);

        /// <summary>
        /// ID: 927
        /// <para/>
        /// Message: The current time is $s1:$s2.
        /// </summary>
        public static SystemMessage TIME_S1_S2_IN_THE_DAY = new SystemMessage(927);

        /// <summary>
        /// ID: 928
        /// <para/>
        /// Message: The current time is $s1:$s2.
        /// </summary>
        public static SystemMessage TIME_S1_S2_IN_THE_NIGHT = new SystemMessage(928);

        /// <summary>
        /// ID: 929
        /// <para/>
        /// Message: No compensation was given for the farm products.
        /// </summary>
        public static SystemMessage NO_COMPENSATION_FOR_FARM_PRODUCTS = new SystemMessage(929);

        /// <summary>
        /// ID: 930
        /// <para/>
        /// Message: Lottery tickets are not currently being sold.
        /// </summary>
        public static SystemMessage NO_LOTTERY_TICKETS_CURRENT_SOLD = new SystemMessage(930);

        /// <summary>
        /// ID: 931
        /// <para/>
        /// Message: The winning lottery ticket numbers has not yet been anonunced.
        /// </summary>
        public static SystemMessage LOTTERY_WINNERS_NOT_ANNOUNCED_YET = new SystemMessage(931);

        /// <summary>
        /// ID: 932
        /// <para/>
        /// Message: You cannot chat locally while observing.
        /// </summary>
        public static SystemMessage NO_ALLCHAT_WHILE_OBSERVING = new SystemMessage(932);

        /// <summary>
        /// ID: 933
        /// <para/>
        /// Message: The seed pricing greatly differs from standard seed prices.
        /// </summary>
        public static SystemMessage THE_SEED_PRICING_GREATLY_DIFFERS_FROM_STANDARD_SEED_PRICES = new SystemMessage(933);

        /// <summary>
        /// ID: 934
        /// <para/>
        /// Message: It is a deleted recipe.
        /// </summary>
        public static SystemMessage A_DELETED_RECIPE = new SystemMessage(934);

        /// <summary>
        /// ID: 935
        /// <para/>
        /// Message: The amount is not sufficient and so the manor is not in operation.
        /// </summary>
        public static SystemMessage THE_AMOUNT_IS_NOT_SUFFICIENT_AND_SO_THE_MANOR_IS_NOT_IN_OPERATION = new SystemMessage(935);

        /// <summary>
        /// ID: 936
        /// <para/>
        /// Message: Use $s1.
        /// </summary>
        public static SystemMessage USE_S1_ = new SystemMessage(936);

        /// <summary>
        /// ID: 937
        /// <para/>
        /// Message: Currently preparing for private workshop.
        /// </summary>
        public static SystemMessage PREPARING_PRIVATE_WORKSHOP = new SystemMessage(937);

        /// <summary>
        /// ID: 938
        /// <para/>
        /// Message: The community server is currently offline.
        /// </summary>
        public static SystemMessage CB_OFFLINE = new SystemMessage(938);

        /// <summary>
        /// ID: 939
        /// <para/>
        /// Message: You cannot exchange while blocking everything.
        /// </summary>
        public static SystemMessage NO_EXCHANGE_WHILE_BLOCKING = new SystemMessage(939);

        /// <summary>
        /// ID: 940
        /// <para/>
        /// Message: $s1 is blocked everything.
        /// </summary>
        public static SystemMessage S1_BLOCKED_EVERYTHING = new SystemMessage(940);

        /// <summary>
        /// ID: 941
        /// <para/>
        /// Message: Restart at Talking Island Village.
        /// </summary>
        public static SystemMessage RESTART_AT_TI = new SystemMessage(941);

        /// <summary>
        /// ID: 942
        /// <para/>
        /// Message: Restart at Gludin Village.
        /// </summary>
        public static SystemMessage RESTART_AT_GLUDIN = new SystemMessage(942);

        /// <summary>
        /// ID: 943
        /// <para/>
        /// Message: Restart at the Town of Gludin. || guess should be Gludio ;)
        /// </summary>
        public static SystemMessage RESTART_AT_GLUDIO = new SystemMessage(943);

        /// <summary>
        /// ID: 944
        /// <para/>
        /// Message: Restart at the Neutral Zone.
        /// </summary>
        public static SystemMessage RESTART_AT_NEUTRAL_ZONE = new SystemMessage(944);

        /// <summary>
        /// ID: 945
        /// <para/>
        /// Message: Restart at the Elven Village.
        /// </summary>
        public static SystemMessage RESTART_AT_ELFEN_VILLAGE = new SystemMessage(945);

        /// <summary>
        /// ID: 946
        /// <para/>
        /// Message: Restart at the Dark Elf Village.
        /// </summary>
        public static SystemMessage RESTART_AT_DARKELF_VILLAGE = new SystemMessage(946);

        /// <summary>
        /// ID: 947
        /// <para/>
        /// Message: Restart at the Town of Dion.
        /// </summary>
        public static SystemMessage RESTART_AT_DION = new SystemMessage(947);

        /// <summary>
        /// ID: 948
        /// <para/>
        /// Message: Restart at Floran Village.
        /// </summary>
        public static SystemMessage RESTART_AT_FLORAN = new SystemMessage(948);

        /// <summary>
        /// ID: 949
        /// <para/>
        /// Message: Restart at the Town of Giran.
        /// </summary>
        public static SystemMessage RESTART_AT_GIRAN = new SystemMessage(949);

        /// <summary>
        /// ID: 950
        /// <para/>
        /// Message: Restart at Giran Harbor.
        /// </summary>
        public static SystemMessage RESTART_AT_GIRAN_HARBOR = new SystemMessage(950);

        /// <summary>
        /// ID: 951
        /// <para/>
        /// Message: Restart at the Orc Village.
        /// </summary>
        public static SystemMessage RESTART_AT_ORC_VILLAGE = new SystemMessage(951);

        /// <summary>
        /// ID: 952
        /// <para/>
        /// Message: Restart at the Dwarven Village.
        /// </summary>
        public static SystemMessage RESTART_AT_DWARFEN_VILLAGE = new SystemMessage(952);

        /// <summary>
        /// ID: 953
        /// <para/>
        /// Message: Restart at the Town of Oren.
        /// </summary>
        public static SystemMessage RESTART_AT_OREN = new SystemMessage(953);

        /// <summary>
        /// ID: 954
        /// <para/>
        /// Message: Restart at Hunters Village.
        /// </summary>
        public static SystemMessage RESTART_AT_HUNTERS_VILLAGE = new SystemMessage(954);

        /// <summary>
        /// ID: 955
        /// <para/>
        /// Message: Restart at the Town of Aden.
        /// </summary>
        public static SystemMessage RESTART_AT_ADEN = new SystemMessage(955);

        /// <summary>
        /// ID: 956
        /// <para/>
        /// Message: Restart at the Coliseum.
        /// </summary>
        public static SystemMessage RESTART_AT_COLISEUM = new SystemMessage(956);

        /// <summary>
        /// ID: 957
        /// <para/>
        /// Message: Restart at Heine.
        /// </summary>
        public static SystemMessage RESTART_AT_HEINE = new SystemMessage(957);

        /// <summary>
        /// ID: 958
        /// <para/>
        /// Message: Items cannot be discarded or destroyed while operating a private store or workshop.
        /// </summary>
        public static SystemMessage ITEMS_CANNOT_BE_DISCARDED_OR_DESTROYED_WHILE_OPERATING_PRIVATE_STORE_OR_WORKSHOP = new SystemMessage(958);

        /// <summary>
        /// ID: 959
        /// <para/>
        /// Message: $s1 (*$s2) manufactured successfully.
        /// </summary>
        public static SystemMessage S1_S2_MANUFACTURED_SUCCESSFULLY = new SystemMessage(959);

        /// <summary>
        /// ID: 960
        /// <para/>
        /// Message: $s1 manufacturing failure.
        /// </summary>
        public static SystemMessage S1_MANUFACTURE_FAILURE = new SystemMessage(960);

        /// <summary>
        /// ID: 961
        /// <para/>
        /// Message: You are now blocking everything.
        /// </summary>
        public static SystemMessage BLOCKING_ALL = new SystemMessage(961);

        /// <summary>
        /// ID: 962
        /// <para/>
        /// Message: You are no longer blocking everything.
        /// </summary>
        public static SystemMessage NOT_BLOCKING_ALL = new SystemMessage(962);

        /// <summary>
        /// ID: 963
        /// <para/>
        /// Message: Please determine the manufacturing price.
        /// </summary>
        public static SystemMessage DETERMINE_MANUFACTURE_PRICE = new SystemMessage(963);

        /// <summary>
        /// ID: 964
        /// <para/>
        /// Message: Chatting is prohibited for one minute.
        /// </summary>
        public static SystemMessage CHATBAN_FOR_1_MINUTE = new SystemMessage(964);

        /// <summary>
        /// ID: 965
        /// <para/>
        /// Message: The chatting prohibition has been removed.
        /// </summary>
        public static SystemMessage CHATBAN_REMOVED = new SystemMessage(965);

        /// <summary>
        /// ID: 966
        /// <para/>
        /// Message: Chatting is currently prohibited. If you try to chat before the prohibition is removed, the prohibition time will become even longer.
        /// </summary>
        public static SystemMessage CHATTING_IS_CURRENTLY_PROHIBITED = new SystemMessage(966);

        /// <summary>
        /// ID: 967
        /// <para/>
        /// Message: Do you accept $s1's party invitation? (Item Distribution: Random including spoil.)
        /// </summary>
        public static SystemMessage S1_PARTY_INVITE_RANDOM_INCLUDING_SPOIL = new SystemMessage(967);

        /// <summary>
        /// ID: 968
        /// <para/>
        /// Message: Do you accept $s1's party invitation? (Item Distribution: By Turn.)
        /// </summary>
        public static SystemMessage S1_PARTY_INVITE_BY_TURN = new SystemMessage(968);

        /// <summary>
        /// ID: 969
        /// <para/>
        /// Message: Do you accept $s1's party invitation? (Item Distribution: By Turn including spoil.)
        /// </summary>
        public static SystemMessage S1_PARTY_INVITE_BY_TURN_INCLUDING_SPOIL = new SystemMessage(969);

        /// <summary>
        /// ID: 970
        /// <para/>
        /// Message: $s2's MP has been drained by $s1.
        /// </summary>
        public static SystemMessage S2_MP_HAS_BEEN_DRAINED_BY_S1 = new SystemMessage(970);

        /// <summary>
        /// ID: 971
        /// <para/>
        /// Message: Petitions cannot exceed 255 characters.
        /// </summary>
        public static SystemMessage PETITION_MAX_CHARS_255 = new SystemMessage(971);

        /// <summary>
        /// ID: 972
        /// <para/>
        /// Message: This pet cannot use this item.
        /// </summary>
        public static SystemMessage PET_CANNOT_USE_ITEM = new SystemMessage(972);

        /// <summary>
        /// ID: 973
        /// <para/>
        /// Message: Please input no more than the number you have.
        /// </summary>
        public static SystemMessage INPUT_NO_MORE_YOU_HAVE = new SystemMessage(973);

        /// <summary>
        /// ID: 974
        /// <para/>
        /// Message: The soul crystal succeeded in absorbing a soul.
        /// </summary>
        public static SystemMessage SOUL_CRYSTAL_ABSORBING_SUCCEEDED = new SystemMessage(974);

        /// <summary>
        /// ID: 975
        /// <para/>
        /// Message: The soul crystal was not able to absorb a soul.
        /// </summary>
        public static SystemMessage SOUL_CRYSTAL_ABSORBING_FAILED = new SystemMessage(975);

        /// <summary>
        /// ID: 976
        /// <para/>
        /// Message: The soul crystal broke because it was not able to endure the soul energy.
        /// </summary>
        public static SystemMessage SOUL_CRYSTAL_BROKE = new SystemMessage(976);

        /// <summary>
        /// ID: 977
        /// <para/>
        /// Message: The soul crystals caused resonation and failed at absorbing a soul.
        /// </summary>
        public static SystemMessage SOUL_CRYSTAL_ABSORBING_FAILED_RESONATION = new SystemMessage(977);

        /// <summary>
        /// ID: 978
        /// <para/>
        /// Message: The soul crystal is refusing to absorb a soul.
        /// </summary>
        public static SystemMessage SOUL_CRYSTAL_ABSORBING_REFUSED = new SystemMessage(978);

        /// <summary>
        /// ID: 979
        /// <para/>
        /// Message: The ferry arrived at Talking Island Harbor.
        /// </summary>
        public static SystemMessage FERRY_ARRIVED_AT_TALKING = new SystemMessage(979);

        /// <summary>
        /// ID: 980
        /// <para/>
        /// Message: The ferry will leave for Gludin Harbor after anchoring for ten minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GLUDIN_AFTER_10_MINUTES = new SystemMessage(980);

        /// <summary>
        /// ID: 981
        /// <para/>
        /// Message: The ferry will leave for Gludin Harbor in five minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GLUDIN_IN_5_MINUTES = new SystemMessage(981);

        /// <summary>
        /// ID: 982
        /// <para/>
        /// Message: The ferry will leave for Gludin Harbor in one minute.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GLUDIN_IN_1_MINUTE = new SystemMessage(982);

        /// <summary>
        /// ID: 983
        /// <para/>
        /// Message: Those wishing to ride should make haste to get on.
        /// </summary>
        public static SystemMessage MAKE_HASTE_GET_ON_BOAT = new SystemMessage(983);

        /// <summary>
        /// ID: 984
        /// <para/>
        /// Message: The ferry will be leaving soon for Gludin Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_SOON_FOR_GLUDIN = new SystemMessage(984);

        /// <summary>
        /// ID: 985
        /// <para/>
        /// Message: The ferry is leaving for Gludin Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_FOR_GLUDIN = new SystemMessage(985);

        /// <summary>
        /// ID: 986
        /// <para/>
        /// Message: The ferry has arrived at Gludin Harbor.
        /// </summary>
        public static SystemMessage FERRY_ARRIVED_AT_GLUDIN = new SystemMessage(986);

        /// <summary>
        /// ID: 987
        /// <para/>
        /// Message: The ferry will leave for Talking Island Harbor after anchoring for ten minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_TALKING_AFTER_10_MINUTES = new SystemMessage(987);

        /// <summary>
        /// ID: 988
        /// <para/>
        /// Message: The ferry will leave for Talking Island Harbor in five minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_TALKING_IN_5_MINUTES = new SystemMessage(988);

        /// <summary>
        /// ID: 989
        /// <para/>
        /// Message: The ferry will leave for Talking Island Harbor in one minute.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_TALKING_IN_1_MINUTE = new SystemMessage(989);

        /// <summary>
        /// ID: 990
        /// <para/>
        /// Message: The ferry will be leaving soon for Talking Island Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_SOON_FOR_TALKING = new SystemMessage(990);

        /// <summary>
        /// ID: 991
        /// <para/>
        /// Message: The ferry is leaving for Talking Island Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_FOR_TALKING = new SystemMessage(991);

        /// <summary>
        /// ID: 992
        /// <para/>
        /// Message: The ferry has arrived at Giran Harbor.
        /// </summary>
        public static SystemMessage FERRY_ARRIVED_AT_GIRAN = new SystemMessage(992);

        /// <summary>
        /// ID: 993
        /// <para/>
        /// Message: The ferry will leave for Giran Harbor after anchoring for ten minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GIRAN_AFTER_10_MINUTES = new SystemMessage(993);

        /// <summary>
        /// ID: 994
        /// <para/>
        /// Message: The ferry will leave for Giran Harbor in five minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GIRAN_IN_5_MINUTES = new SystemMessage(994);

        /// <summary>
        /// ID: 995
        /// <para/>
        /// Message: The ferry will leave for Giran Harbor in one minute.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_FOR_GIRAN_IN_1_MINUTE = new SystemMessage(995);

        /// <summary>
        /// ID: 996
        /// <para/>
        /// Message: The ferry will be leaving soon for Giran Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVE_SOON_FOR_GIRAN = new SystemMessage(996);

        /// <summary>
        /// ID: 997
        /// <para/>
        /// Message: The ferry is leaving for Giran Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_FOR_GIRAN = new SystemMessage(997);

        /// <summary>
        /// ID: 998
        /// <para/>
        /// Message: The Innadril pleasure boat has arrived. It will anchor for ten minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ANCHOR_10_MINUTES = new SystemMessage(998);

        /// <summary>
        /// ID: 999
        /// <para/>
        /// Message: The Innadril pleasure boat will leave in five minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_LEAVE_IN_5_MINUTES = new SystemMessage(999);

        /// <summary>
        /// ID: 1000
        /// <para/>
        /// Message: The Innadril pleasure boat will leave in one minute.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_LEAVE_IN_1_MINUTE = new SystemMessage(1000);

        /// <summary>
        /// ID: 1001
        /// <para/>
        /// Message: The Innadril pleasure boat will be leaving soon.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_LEAVE_SOON = new SystemMessage(1001);

        /// <summary>
        /// ID: 1002
        /// <para/>
        /// Message: The Innadril pleasure boat is leaving.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_LEAVING = new SystemMessage(1002);

        /// <summary>
        /// ID: 1003
        /// <para/>
        /// Message: Cannot possess a monster race ticket.
        /// </summary>
        public static SystemMessage CANNOT_POSSES_MONS_TICKET = new SystemMessage(1003);

        /// <summary>
        /// ID: 1004
        /// <para/>
        /// Message: You have registered for a clan hall auction.
        /// </summary>
        public static SystemMessage REGISTERED_FOR_CLANHALL = new SystemMessage(1004);

        /// <summary>
        /// ID: 1005
        /// <para/>
        /// Message: There is not enough adena in the clan hall warehouse.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_ADENA_IN_CWH = new SystemMessage(1005);

        /// <summary>
        /// ID: 1006
        /// <para/>
        /// Message: You have bid in a clan hall auction.
        /// </summary>
        public static SystemMessage BID_IN_CLANHALL_AUCTION = new SystemMessage(1006);

        /// <summary>
        /// ID: 1007
        /// <para/>
        /// Message: The preliminary match registration of $s1 has finished.
        /// </summary>
        public static SystemMessage PRELIMINARY_REGISTRATION_OF_S1_FINISHED = new SystemMessage(1007);

        /// <summary>
        /// ID: 1008
        /// <para/>
        /// Message: A hungry strider cannot be mounted or dismounted.
        /// </summary>
        public static SystemMessage HUNGRY_STRIDER_NOT_MOUNT = new SystemMessage(1008);

        /// <summary>
        /// ID: 1009
        /// <para/>
        /// Message: A strider cannot be ridden when dead.
        /// </summary>
        public static SystemMessage STRIDER_CANT_BE_RIDDEN_WHILE_DEAD = new SystemMessage(1009);

        /// <summary>
        /// ID: 1010
        /// <para/>
        /// Message: A dead strider cannot be ridden.
        /// </summary>
        public static SystemMessage DEAD_STRIDER_CANT_BE_RIDDEN = new SystemMessage(1010);

        /// <summary>
        /// ID: 1011
        /// <para/>
        /// Message: A strider in battle cannot be ridden.
        /// </summary>
        public static SystemMessage STRIDER_IN_BATLLE_CANT_BE_RIDDEN = new SystemMessage(1011);

        /// <summary>
        /// ID: 1012
        /// <para/>
        /// Message: A strider cannot be ridden while in battle.
        /// </summary>
        public static SystemMessage STRIDER_CANT_BE_RIDDEN_WHILE_IN_BATTLE = new SystemMessage(1012);

        /// <summary>
        /// ID: 1013
        /// <para/>
        /// Message: A strider can be ridden only when standing.
        /// </summary>
        public static SystemMessage STRIDER_CAN_BE_RIDDEN_ONLY_WHILE_STANDING = new SystemMessage(1013);

        /// <summary>
        /// ID: 1014
        /// <para/>
        /// Message: Your pet gained $s1 experience points.
        /// </summary>
        public static SystemMessage PET_EARNED_S1_EXP = new SystemMessage(1014);

        /// <summary>
        /// ID: 1015
        /// <para/>
        /// Message: Your pet hit for $s1 damage.
        /// </summary>
        public static SystemMessage PET_HIT_FOR_S1_DAMAGE = new SystemMessage(1015);

        /// <summary>
        /// ID: 1016
        /// <para/>
        /// Message: Pet received $s2 damage by $s1.
        /// </summary>
        public static SystemMessage PET_RECEIVED_S2_DAMAGE_BY_S1 = new SystemMessage(1016);

        /// <summary>
        /// ID: 1017
        /// <para/>
        /// Message: Pet's critical hit!
        /// </summary>
        public static SystemMessage CRITICAL_HIT_BY_PET = new SystemMessage(1017);

        /// <summary>
        /// ID: 1018
        /// <para/>
        /// Message: Your pet uses $s1.
        /// </summary>
        public static SystemMessage PET_USES_S1 = new SystemMessage(1018);

        /// <summary>
        /// ID: 1019
        /// <para/>
        /// Message: Your pet uses $s1.
        /// </summary>
        public static SystemMessage PET_USES_S1_ = new SystemMessage(1019);

        /// <summary>
        /// ID: 1020
        /// <para/>
        /// Message: Your pet picked up $s1.
        /// </summary>
        public static SystemMessage PET_PICKED_S1 = new SystemMessage(1020);

        /// <summary>
        /// ID: 1021
        /// <para/>
        /// Message: Your pet picked up $s2 $s1(s).
        /// </summary>
        public static SystemMessage PET_PICKED_S2_S1_S = new SystemMessage(1021);

        /// <summary>
        /// ID: 1022
        /// <para/>
        /// Message: Your pet picked up +$s1 $s2.
        /// </summary>
        public static SystemMessage PET_PICKED_S1_S2 = new SystemMessage(1022);

        /// <summary>
        /// ID: 1023
        /// <para/>
        /// Message: Your pet picked up $s1 adena.
        /// </summary>
        public static SystemMessage PET_PICKED_S1_ADENA = new SystemMessage(1023);

        /// <summary>
        /// ID: 1024
        /// <para/>
        /// Message: Your pet put on $s1.
        /// </summary>
        public static SystemMessage PET_PUT_ON_S1 = new SystemMessage(1024);

        /// <summary>
        /// ID: 1025
        /// <para/>
        /// Message: Your pet took off $s1.
        /// </summary>
        public static SystemMessage PET_TOOK_OFF_S1 = new SystemMessage(1025);

        /// <summary>
        /// ID: 1026
        /// <para/>
        /// Message: The summoned monster gave damage of $s1
        /// </summary>
        public static SystemMessage SUMMON_GAVE_DAMAGE_S1 = new SystemMessage(1026);

        /// <summary>
        /// ID: 1027
        /// <para/>
        /// Message: Servitor received $s2 damage caused by $s1.
        /// </summary>
        public static SystemMessage SUMMON_RECEIVED_DAMAGE_S2_BY_S1 = new SystemMessage(1027);

        /// <summary>
        /// ID: 1028
        /// <para/>
        /// Message: Summoned monster's critical hit!
        /// </summary>
        public static SystemMessage CRITICAL_HIT_BY_SUMMONED_MOB = new SystemMessage(1028);

        /// <summary>
        /// ID: 1029
        /// <para/>
        /// Message: Summoned monster uses $s1.
        /// </summary>
        public static SystemMessage SUMMONED_MOB_USES_S1 = new SystemMessage(1029);

        /// <summary>
        /// ID: 1030
        /// <para/>
        /// Message: <Party Information>
        /// </summary>
        public static SystemMessage PARTY_INFORMATION = new SystemMessage(1030);

        /// <summary>
        /// ID: 1031
        /// <para/>
        /// Message: Looting method: Finders keepers
        /// </summary>
        public static SystemMessage LOOTING_FINDERS_KEEPERS = new SystemMessage(1031);

        /// <summary>
        /// ID: 1032
        /// <para/>
        /// Message: Looting method: Random
        /// </summary>
        public static SystemMessage LOOTING_RANDOM = new SystemMessage(1032);

        /// <summary>
        /// ID: 1033
        /// <para/>
        /// Message: Looting method: Random including spoil
        /// </summary>
        public static SystemMessage LOOTING_RANDOM_INCLUDE_SPOIL = new SystemMessage(1033);

        /// <summary>
        /// ID: 1034
        /// <para/>
        /// Message: Looting method: By turn
        /// </summary>
        public static SystemMessage LOOTING_BY_TURN = new SystemMessage(1034);

        /// <summary>
        /// ID: 1035
        /// <para/>
        /// Message: Looting method: By turn including spoil
        /// </summary>
        public static SystemMessage LOOTING_BY_TURN_INCLUDE_SPOIL = new SystemMessage(1035);

        /// <summary>
        /// ID: 1036
        /// <para/>
        /// Message: You have exceeded the quantity that can be inputted.
        /// </summary>
        public static SystemMessage YOU_HAVE_EXCEEDED_QUANTITY_THAT_CAN_BE_INPUTTED = new SystemMessage(1036);

        /// <summary>
        /// ID: 1037
        /// <para/>
        /// Message: $s1 manufactured $s2.
        /// </summary>
        public static SystemMessage S1_MANUFACTURED_S2 = new SystemMessage(1037);

        /// <summary>
        /// ID: 1038
        /// <para/>
        /// Message: $s1 manufactured $s3 $s2(s).
        /// </summary>
        public static SystemMessage S1_MANUFACTURED_S3_S2_S = new SystemMessage(1038);

        /// <summary>
        /// ID: 1039
        /// <para/>
        /// Message: Items left at the clan hall warehouse can only be retrieved by the clan leader. Do you want to continue?
        /// </summary>
        public static SystemMessage ONLY_CLAN_LEADER_CAN_RETRIEVE_ITEMS_FROM_CLAN_WAREHOUSE = new SystemMessage(1039);

        /// <summary>
        /// ID: 1040
        /// <para/>
        /// Message: Items sent by freight can be picked up from any Warehouse location. Do you want to continue?
        /// </summary>
        public static SystemMessage ITEMS_SENT_BY_FREIGHT_PICKED_UP_FROM_ANYWHERE = new SystemMessage(1040);

        /// <summary>
        /// ID: 1041
        /// <para/>
        /// Message: The next seed purchase price is $s1 adena.
        /// </summary>
        public static SystemMessage THE_NEXT_SEED_PURCHASE_PRICE_IS_S1_ADENA = new SystemMessage(1041);

        /// <summary>
        /// ID: 1042
        /// <para/>
        /// Message: The next farm goods purchase price is $s1 adena.
        /// </summary>
        public static SystemMessage THE_NEXT_FARM_GOODS_PURCHASE_PRICE_IS_S1_ADENA = new SystemMessage(1042);

        /// <summary>
        /// ID: 1043
        /// <para/>
        /// Message: At the current time, the "/unstuck" command cannot be used. Please send in a petition.
        /// </summary>
        public static SystemMessage NO_UNSTUCK_PLEASE_SEND_PETITION = new SystemMessage(1043);

        /// <summary>
        /// ID: 1044
        /// <para/>
        /// Message: Monster race payout information is not available while tickets are being sold.
        /// </summary>
        public static SystemMessage MONSRACE_NO_PAYOUT_INFO = new SystemMessage(1044);

        /// <summary>
        /// ID: 1046
        /// <para/>
        /// Message: Monster race tickets are no longer available.
        /// </summary>
        public static SystemMessage MONSRACE_TICKETS_NOT_AVAILABLE = new SystemMessage(1046);

        /// <summary>
        /// ID: 1047
        /// <para/>
        /// Message: We did not succeed in producing $s1 item.
        /// </summary>
        public static SystemMessage NOT_SUCCEED_PRODUCING_S1 = new SystemMessage(1047);

        /// <summary>
        /// ID: 1048
        /// <para/>
        /// Message: When "blocking" everything, whispering is not possible.
        /// </summary>
        public static SystemMessage NO_WHISPER_WHEN_BLOCKING = new SystemMessage(1048);

        /// <summary>
        /// ID: 1049
        /// <para/>
        /// Message: When "blocking" everything, it is not possible to send invitations for organizing parties.
        /// </summary>
        public static SystemMessage NO_PARTY_WHEN_BLOCKING = new SystemMessage(1049);

        /// <summary>
        /// ID: 1050
        /// <para/>
        /// Message: There are no communities in my clan. Clan communities are allowed for clans with skill levels of 2 and higher.
        /// </summary>
        public static SystemMessage NO_CB_IN_MY_CLAN = new SystemMessage(1050);

        /// <summary>
        /// ID: 1051 
        /// <para/>
        /// Message: Payment for your clan hall has not been made please make payment tomorrow.
        /// </summary>
        public static SystemMessage PAYMENT_FOR_YOUR_CLAN_HALL_HAS_NOT_BEEN_MADE_PLEASE_MAKE_PAYMENT_TO_YOUR_CLAN_WAREHOUSE_BY_S1_TOMORROW = new SystemMessage(1051);

        /// <summary>
        /// ID: 1052 
        /// <para/>
        /// Message: Payment of Clan Hall is overdue the owner loose Clan Hall.
        /// </summary>
        public static SystemMessage THE_CLAN_HALL_FEE_IS_ONE_WEEK_OVERDUE_THEREFORE_THE_CLAN_HALL_OWNERSHIP_HAS_BEEN_REVOKED = new SystemMessage(1052);

        /// <summary>
        /// ID: 1053
        /// <para/>
        /// Message: It is not possible to resurrect in battlefields where a siege war is taking place.
        /// </summary>
        public static SystemMessage CANNOT_BE_RESURRECTED_DURING_SIEGE = new SystemMessage(1053);

        /// <summary>
        /// ID: 1054
        /// <para/>
        /// Message: You have entered a mystical land.
        /// </summary>
        public static SystemMessage ENTERED_MYSTICAL_LAND = new SystemMessage(1054);

        /// <summary>
        /// ID: 1055
        /// <para/>
        /// Message: You have left a mystical land.
        /// </summary>
        public static SystemMessage EXITED_MYSTICAL_LAND = new SystemMessage(1055);

        /// <summary>
        /// ID: 1056
        /// <para/>
        /// Message: You have exceeded the storage capacity of the castle's vault.
        /// </summary>
        public static SystemMessage VAULT_CAPACITY_EXCEEDED = new SystemMessage(1056);

        /// <summary>
        /// ID: 1057
        /// <para/>
        /// Message: This command can only be used in the relax server.
        /// </summary>
        public static SystemMessage RELAX_SERVER_ONLY = new SystemMessage(1057);

        /// <summary>
        /// ID: 1058
        /// <para/>
        /// Message: The sales price for seeds is $s1 adena.
        /// </summary>
        public static SystemMessage THE_SALES_PRICE_FOR_SEEDS_IS_S1_ADENA = new SystemMessage(1058);

        /// <summary>
        /// ID: 1059
        /// <para/>
        /// Message: The remaining purchasing amount is $s1 adena.
        /// </summary>
        public static SystemMessage THE_REMAINING_PURCHASING_IS_S1_ADENA = new SystemMessage(1059);

        /// <summary>
        /// ID: 1060
        /// <para/>
        /// Message: The remainder after selling the seeds is $s1.
        /// </summary>
        public static SystemMessage THE_REMAINDER_AFTER_SELLING_THE_SEEDS_IS_S1 = new SystemMessage(1060);

        /// <summary>
        /// ID: 1061
        /// <para/>
        /// Message: The recipe cannot be registered. You do not have the ability to create items.
        /// </summary>
        public static SystemMessage CANT_REGISTER_NO_ABILITY_TO_CRAFT = new SystemMessage(1061);

        /// <summary>
        /// ID: 1062
        /// <para/>
        /// Message: Writing something new is possible after level 10.
        /// </summary>
        public static SystemMessage WRITING_SOMETHING_NEW_POSSIBLE_AFTER_LEVEL_10 = new SystemMessage(1062);

        /// <summary>
        /// ID: 1063
        /// <para/>
        /// if you become trapped or unable to move, please use the '/unstuck' command.
        /// </summary>
        public static SystemMessage PETITION_UNAVAILABLE = new SystemMessage(1063);

        /// <summary>
        /// ID: 1064
        /// <para/>
        /// Message: The equipment, +$s1 $s2, has been removed.
        /// </summary>
        public static SystemMessage EQUIPMENT_S1_S2_REMOVED = new SystemMessage(1064);

        /// <summary>
        /// ID: 1065
        /// <para/>
        /// Message: While operating a private store or workshop, you cannot discard, destroy, or trade an item.
        /// </summary>
        public static SystemMessage CANNOT_TRADE_DISCARD_DROP_ITEM_WHILE_IN_SHOPMODE = new SystemMessage(1065);

        /// <summary>
        /// ID: 1066
        /// <para/>
        /// Message: $s1 HP has been restored.
        /// </summary>
        public static SystemMessage S1_HP_RESTORED = new SystemMessage(1066);

        /// <summary>
        /// ID: 1067
        /// <para/>
        /// Message: $s2 HP has been restored by $s1
        /// </summary>
        public static SystemMessage S2_HP_RESTORED_BY_S1 = new SystemMessage(1067);

        /// <summary>
        /// ID: 1068
        /// <para/>
        /// Message: $s1 MP has been restored.
        /// </summary>
        public static SystemMessage S1_MP_RESTORED = new SystemMessage(1068);

        /// <summary>
        /// ID: 1069
        /// <para/>
        /// Message: $s2 MP has been restored by $s1.
        /// </summary>
        public static SystemMessage S2_MP_RESTORED_BY_S1 = new SystemMessage(1069);

        /// <summary>
        /// ID: 1070
        /// <para/>
        /// Message: You do not have 'read' permission.
        /// </summary>
        public static SystemMessage NO_READ_PERMISSION = new SystemMessage(1070);

        /// <summary>
        /// ID: 1071
        /// <para/>
        /// Message: You do not have 'write' permission.
        /// </summary>
        public static SystemMessage NO_WRITE_PERMISSION = new SystemMessage(1071);

        /// <summary>
        /// ID: 1072
        /// <para/>
        /// Message: You have obtained a ticket for the Monster Race #$s1 - Single
        /// </summary>
        public static SystemMessage OBTAINED_TICKET_FOR_MONS_RACE_S1_SINGLE = new SystemMessage(1072);

        /// <summary>
        /// ID: 1073
        /// <para/>
        /// Message: You have obtained a ticket for the Monster Race #$s1 - Single
        /// </summary>
        public static SystemMessage OBTAINED_TICKET_FOR_MONS_RACE_S1_SINGLE_ = new SystemMessage(1073);

        /// <summary>
        /// ID: 1074
        /// <para/>
        /// Message: You do not meet the age requirement to purchase a Monster Race Ticket.
        /// </summary>
        public static SystemMessage NOT_MEET_AGE_REQUIREMENT_FOR_MONS_RACE = new SystemMessage(1074);

        /// <summary>
        /// ID: 1075
        /// <para/>
        /// Message: The bid amount must be higher than the previous bid.
        /// </summary>
        public static SystemMessage BID_AMOUNT_HIGHER_THAN_PREVIOUS_BID = new SystemMessage(1075);

        /// <summary>
        /// ID: 1076
        /// <para/>
        /// Message: The game cannot be terminated at this time.
        /// </summary>
        public static SystemMessage GAME_CANNOT_TERMINATE_NOW = new SystemMessage(1076);

        /// <summary>
        /// ID: 1077
        /// <para/>
        /// Message: A GameGuard Execution error has occurred. Please send the *.erl file(s) located in the GameGuard folder to game@inca.co.kr
        /// </summary>
        public static SystemMessage GG_EXECUTION_ERROR = new SystemMessage(1077);

        /// <summary>
        /// ID: 1078
        /// <para/>
        /// Message: When a user's keyboard input exceeds a certain cumulative score a chat ban will be applied. This is done to discourage spamming. Please avoid posting the same message multiple times during a short period.
        /// </summary>
        public static SystemMessage DONT_SPAM = new SystemMessage(1078);

        /// <summary>
        /// ID: 1079
        /// <para/>
        /// Message: The target is currently banend from chatting.
        /// </summary>
        public static SystemMessage TARGET_IS_CHAT_BANNED = new SystemMessage(1079);

        /// <summary>
        /// ID: 1080
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the facelift potion - Type A?
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_A = new SystemMessage(1080);

        /// <summary>
        /// ID: 1081
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the hair dye potion - Type A?
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_A = new SystemMessage(1081);

        /// <summary>
        /// ID: 1082
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type A? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_A = new SystemMessage(1082);

        /// <summary>
        /// ID: 1083
        /// <para/>
        /// Message: Facelift potion - Type A is being applied.
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_A_APPLIED = new SystemMessage(1083);

        /// <summary>
        /// ID: 1084
        /// <para/>
        /// Message: Hair dye potion - Type A is being applied.
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_A_APPLIED = new SystemMessage(1084);

        /// <summary>
        /// ID: 1085
        /// <para/>
        /// Message: The hair style chance potion - Type A is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_A_USED = new SystemMessage(1085);

        /// <summary>
        /// ID: 1086
        /// <para/>
        /// Message: Your facial appearance has been changed.
        /// </summary>
        public static SystemMessage FACE_APPEARANCE_CHANGED = new SystemMessage(1086);

        /// <summary>
        /// ID: 1087
        /// <para/>
        /// Message: Your hair color has changed.
        /// </summary>
        public static SystemMessage HAIR_COLOR_CHANGED = new SystemMessage(1087);

        /// <summary>
        /// ID: 1088
        /// <para/>
        /// Message: Your hair style has been changed.
        /// </summary>
        public static SystemMessage HAIR_STYLE_CHANGED = new SystemMessage(1088);

        /// <summary>
        /// ID: 1089
        /// <para/>
        /// Message: $s1 has obtained a first anniversary commemorative item.
        /// </summary>
        public static SystemMessage S1_OBTAINED_ANNIVERSARY_ITEM = new SystemMessage(1089);

        /// <summary>
        /// ID: 1090
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the facelift potion - Type B?
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_B = new SystemMessage(1090);

        /// <summary>
        /// ID: 1091
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the facelift potion - Type C?
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_C = new SystemMessage(1091);

        /// <summary>
        /// ID: 1092
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the hair dye potion - Type B?
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_B = new SystemMessage(1092);

        /// <summary>
        /// ID: 1093
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the hair dye potion - Type C?
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_C = new SystemMessage(1093);

        /// <summary>
        /// ID: 1094
        /// <para/>
        /// Message: Being permanent, are you sure you wish to use the hair dye potion - Type D?
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_D = new SystemMessage(1094);

        /// <summary>
        /// ID: 1095
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type B? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_B = new SystemMessage(1095);

        /// <summary>
        /// ID: 1096
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type C? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_C = new SystemMessage(1096);

        /// <summary>
        /// ID: 1097
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type D? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_D = new SystemMessage(1097);

        /// <summary>
        /// ID: 1098
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type E? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_E = new SystemMessage(1098);

        /// <summary>
        /// ID: 1099
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type F? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_F = new SystemMessage(1099);

        /// <summary>
        /// ID: 1100
        /// <para/>
        /// Message: Do you wish to use the hair style change potion - Type G? It is permanent.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_G = new SystemMessage(1100);

        /// <summary>
        /// ID: 1101
        /// <para/>
        /// Message: Facelift potion - Type B is being applied.
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_B_APPLIED = new SystemMessage(1101);

        /// <summary>
        /// ID: 1102
        /// <para/>
        /// Message: Facelift potion - Type C is being applied.
        /// </summary>
        public static SystemMessage FACELIFT_POTION_TYPE_C_APPLIED = new SystemMessage(1102);

        /// <summary>
        /// ID: 1103
        /// <para/>
        /// Message: Hair dye potion - Type B is being applied.
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_B_APPLIED = new SystemMessage(1103);

        /// <summary>
        /// ID: 1104
        /// <para/>
        /// Message: Hair dye potion - Type C is being applied.
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_C_APPLIED = new SystemMessage(1104);

        /// <summary>
        /// ID: 1105
        /// <para/>
        /// Message: Hair dye potion - Type D is being applied.
        /// </summary>
        public static SystemMessage HAIRDYE_POTION_TYPE_D_APPLIED = new SystemMessage(1105);

        /// <summary>
        /// ID: 1106
        /// <para/>
        /// Message: The hair style chance potion - Type B is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_B_USED = new SystemMessage(1106);

        /// <summary>
        /// ID: 1107
        /// <para/>
        /// Message: The hair style chance potion - Type C is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_C_USED = new SystemMessage(1107);

        /// <summary>
        /// ID: 1108
        /// <para/>
        /// Message: The hair style chance potion - Type D is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_D_USED = new SystemMessage(1108);

        /// <summary>
        /// ID: 1109
        /// <para/>
        /// Message: The hair style chance potion - Type E is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_E_USED = new SystemMessage(1109);

        /// <summary>
        /// ID: 1110
        /// <para/>
        /// Message: The hair style chance potion - Type F is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_F_USED = new SystemMessage(1110);

        /// <summary>
        /// ID: 1111
        /// <para/>
        /// Message: The hair style chance potion - Type G is being used.
        /// </summary>
        public static SystemMessage HAIRSTYLE_POTION_TYPE_G_USED = new SystemMessage(1111);

        /// <summary>
        /// ID: 1112
        /// <para/>
        /// Message: The prize amount for the winner of Lottery #$s1 is $s2 adena. We have $s3 first prize winners.
        /// </summary>
        public static SystemMessage AMOUNT_FOR_WINNER_S1_IS_S2_ADENA_WE_HAVE_S3_PRIZE_WINNER = new SystemMessage(1112);

        /// <summary>
        /// ID: 1113
        /// <para/>
        /// Message: The prize amount for Lucky Lottery #$s1 is $s2 adena. There was no first prize winner in this drawing, therefore the jackpot will be added to the next drawing.
        /// </summary>
        public static SystemMessage AMOUNT_FOR_LOTTERY_S1_IS_S2_ADENA_NO_WINNER = new SystemMessage(1113);

        /// <summary>
        /// ID: 1114
        /// <para/>
        /// Message: Your clan may not register to participate in a siege while under a grace period of the clan's dissolution.
        /// </summary>
        public static SystemMessage CANT_PARTICIPATE_IN_SIEGE_WHILE_DISSOLUTION_IN_PROGRESS = new SystemMessage(1114);

        /// <summary>
        /// ID: 1115
        /// <para/>
        /// Message: Individuals may not surrender during combat.
        /// </summary>
        public static SystemMessage INDIVIDUALS_NOT_SURRENDER_DURING_COMBAT = new SystemMessage(1115);

        /// <summary>
        /// ID: 1116
        /// <para/>
        /// Message: One cannot leave one's clan during combat.
        /// </summary>
        public static SystemMessage YOU_CANNOT_LEAVE_DURING_COMBAT = new SystemMessage(1116);

        /// <summary>
        /// ID: 1117
        /// <para/>
        /// Message: A clan member may not be dismissed during combat.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_CANNOT_BE_DISMISSED_DURING_COMBAT = new SystemMessage(1117);

        /// <summary>
        /// ID: 1118
        /// <para/>
        /// Message: Progress in a quest is possible only when your inventory's weight and volume are less than 80 percent of capacity.
        /// </summary>
        public static SystemMessage INVENTORY_LESS_THAN_80_PERCENT = new SystemMessage(1118);

        /// <summary>
        /// ID: 1119
        /// <para/>
        /// Message: Quest was automatically canceled when you attempted to settle the accounts of your quest while your inventory exceeded 80 percent of capacity.
        /// </summary>
        public static SystemMessage QUEST_CANCELED_INVENTORY_EXCEEDS_80_PERCENT = new SystemMessage(1119);

        /// <summary>
        /// ID: 1120
        /// <para/>
        /// Message: You are still a member of the clan.
        /// </summary>
        public static SystemMessage STILL_CLAN_MEMBER = new SystemMessage(1120);

        /// <summary>
        /// ID: 1121
        /// <para/>
        /// Message: You do not have the right to vote.
        /// </summary>
        public static SystemMessage NO_RIGHT_TO_VOTE = new SystemMessage(1121);

        /// <summary>
        /// ID: 1122
        /// <para/>
        /// Message: There is no candidate.
        /// </summary>
        public static SystemMessage NO_CANDIDATE = new SystemMessage(1122);

        /// <summary>
        /// ID: 1123
        /// <para/>
        /// Message: Weight and volume limit has been exceeded. That skill is currently unavailable.
        /// </summary>
        public static SystemMessage WEIGHT_EXCEEDED_SKILL_UNAVAILABLE = new SystemMessage(1123);

        /// <summary>
        /// ID: 1124
        /// <para/>
        /// Message: Your recipe book may not be accessed while using a skill.
        /// </summary>
        public static SystemMessage NO_RECIPE_BOOK_WHILE_CASTING = new SystemMessage(1124);

        /// <summary>
        /// ID: 1125
        /// <para/>
        /// Message: An item may not be created while engaged in trading.
        /// </summary>
        public static SystemMessage CANNOT_CREATED_WHILE_ENGAGED_IN_TRADING = new SystemMessage(1125);

        /// <summary>
        /// ID: 1126
        /// <para/>
        /// Message: You cannot enter a negative number.
        /// </summary>
        public static SystemMessage NO_NEGATIVE_NUMBER = new SystemMessage(1126);

        /// <summary>
        /// ID: 1127
        /// <para/>
        /// Message: The reward must be less than 10 times the standard price.
        /// </summary>
        public static SystemMessage REWARD_LESS_THAN_10_TIMES_STANDARD_PRICE = new SystemMessage(1127);

        /// <summary>
        /// ID: 1128
        /// <para/>
        /// Message: A private store may not be opened while using a skill.
        /// </summary>
        public static SystemMessage PRIVATE_STORE_NOT_WHILE_CASTING = new SystemMessage(1128);

        /// <summary>
        /// ID: 1129
        /// <para/>
        /// Message: This is not allowed while riding a ferry or boat.
        /// </summary>
        public static SystemMessage NOT_ALLOWED_ON_BOAT = new SystemMessage(1129);

        /// <summary>
        /// ID: 1130
        /// <para/>
        /// Message: You have given $s1 damage to your target and $s2 damage to the servitor.
        /// </summary>
        public static SystemMessage GIVEN_S1_DAMAGE_TO_YOUR_TARGET_AND_S2_DAMAGE_TO_SERVITOR = new SystemMessage(1130);

        /// <summary>
        /// ID: 1131
        /// <para/>
        /// Message: It is now midnight and the effect of $s1 can be felt.
        /// </summary>
        public static SystemMessage NIGHT_S1_EFFECT_APPLIES = new SystemMessage(1131);

        /// <summary>
        /// ID: 1132
        /// <para/>
        /// Message: It is now dawn and the effect of $s1 will now disappear.
        /// </summary>
        public static SystemMessage DAY_S1_EFFECT_DISAPPEARS = new SystemMessage(1132);

        /// <summary>
        /// ID: 1133
        /// <para/>
        /// Message: Since HP has decreased, the effect of $s1 can be felt.
        /// </summary>
        public static SystemMessage HP_DECREASED_EFFECT_APPLIES = new SystemMessage(1133);

        /// <summary>
        /// ID: 1134
        /// <para/>
        /// Message: Since HP has increased, the effect of $s1 will disappear.
        /// </summary>
        public static SystemMessage HP_INCREASED_EFFECT_DISAPPEARS = new SystemMessage(1134);

        /// <summary>
        /// ID: 1135
        /// <para/>
        /// Message: While you are engaged in combat, you cannot operate a private store or private workshop.
        /// </summary>
        public static SystemMessage CANT_OPERATE_PRIVATE_STORE_DURING_COMBAT = new SystemMessage(1135);

        /// <summary>
        /// ID: 1136
        /// <para/>
        /// Message: Since there was an account that used this IP and attempted to log in illegally, this account is not allowed to connect to the game server for $s1 minutes. Please use another game server.
        /// </summary>
        public static SystemMessage ACCOUNT_NOT_ALLOWED_TO_CONNECT = new SystemMessage(1136);

        /// <summary>
        /// ID: 1137
        /// <para/>
        /// Message: $s1 harvested $s3 $s2(s).
        /// </summary>
        public static SystemMessage S1_HARVESTED_S3_S2S = new SystemMessage(1137);

        /// <summary>
        /// ID: 1138
        /// <para/>
        /// Message: $s1 harvested $s2(s).
        /// </summary>
        public static SystemMessage S1_HARVESTED_S2S = new SystemMessage(1138);

        /// <summary>
        /// ID: 1139
        /// <para/>
        /// Message: The weight and volume limit of your inventory must not be exceeded.
        /// </summary>
        public static SystemMessage INVENTORY_LIMIT_MUST_NOT_BE_EXCEEDED = new SystemMessage(1139);

        /// <summary>
        /// ID: 1140
        /// <para/>
        /// Message: Would you like to open the gate?
        /// </summary>
        public static SystemMessage WOULD_YOU_LIKE_TO_OPEN_THE_GATE = new SystemMessage(1140);

        /// <summary>
        /// ID: 1141
        /// <para/>
        /// Message: Would you like to close the gate?
        /// </summary>
        public static SystemMessage WOULD_YOU_LIKE_TO_CLOSE_THE_GATE = new SystemMessage(1141);

        /// <summary>
        /// ID: 1142
        /// <para/>
        /// Message: Since $s1 already exists nearby, you cannot summon it again.
        /// </summary>
        public static SystemMessage CANNOT_SUMMON_S1_AGAIN = new SystemMessage(1142);

        /// <summary>
        /// ID: 1143
        /// <para/>
        /// Message: Since you do not have enough items to maintain the servitor's stay, the servitor will disappear.
        /// </summary>
        public static SystemMessage SERVITOR_DISAPPEARED_NOT_ENOUGH_ITEMS = new SystemMessage(1143);

        /// <summary>
        /// ID: 1144
        /// <para/>
        /// Message: Currently, you don't have anybody to chat with in the game.
        /// </summary>
        public static SystemMessage NOBODY_IN_GAME_TO_CHAT = new SystemMessage(1144);

        /// <summary>
        /// ID: 1145
        /// <para/>
        /// Message: $s2 has been created for $s1 after the payment of $s3 adena is received.
        /// </summary>
        public static SystemMessage S2_CREATED_FOR_S1_FOR_S3_ADENA = new SystemMessage(1145);

        /// <summary>
        /// ID: 1146
        /// <para/>
        /// Message: $s1 created $s2 after receiving $s3 adena.
        /// </summary>
        public static SystemMessage S1_CREATED_S2_FOR_S3_ADENA = new SystemMessage(1146);

        /// <summary>
        /// ID: 1147
        /// <para/>
        /// Message: $s2 $s3 have been created for $s1 at the price of $s4 adena.
        /// </summary>
        public static SystemMessage S2_S3_S_CREATED_FOR_S1_FOR_S4_ADENA = new SystemMessage(1147);

        /// <summary>
        /// ID: 1148
        /// <para/>
        /// Message: $s1 created $s2 $s3 at the price of $s4 adena.
        /// </summary>
        public static SystemMessage S1_CREATED_S2_S3_S_FOR_S4_ADENA = new SystemMessage(1148);

        /// <summary>
        /// ID: 1149
        /// <para/>
        /// Message: Your attempt to create $s2 for $s1 at the price of $s3 adena has failed.
        /// </summary>
        public static SystemMessage CREATION_OF_S2_FOR_S1_AT_S3_ADENA_FAILED = new SystemMessage(1149);

        /// <summary>
        /// ID: 1150
        /// <para/>
        /// Message: $s1 has failed to create $s2 at the price of $s3 adena.
        /// </summary>
        public static SystemMessage S1_FAILED_TO_CREATE_S2_FOR_S3_ADENA = new SystemMessage(1150);

        /// <summary>
        /// ID: 1151
        /// <para/>
        /// Message: $s2 is sold to $s1 at the price of $s3 adena.
        /// </summary>
        public static SystemMessage S2_SOLD_TO_S1_FOR_S3_ADENA = new SystemMessage(1151);

        /// <summary>
        /// ID: 1152
        /// <para/>
        /// Message: $s2 $s3 have been sold to $s1 for $s4 adena.
        /// </summary>
        public static SystemMessage S3_S2_S_SOLD_TO_S1_FOR_S4_ADENA = new SystemMessage(1152);

        /// <summary>
        /// ID: 1153
        /// <para/>
        /// Message: $s2 has been purchased from $s1 at the price of $s3 adena.
        /// </summary>
        public static SystemMessage S2_PURCHASED_FROM_S1_FOR_S3_ADENA = new SystemMessage(1153);

        /// <summary>
        /// ID: 1154
        /// <para/>
        /// Message: $s3 $s2 has been purchased from $s1 for $s4 adena.
        /// </summary>
        public static SystemMessage S3_S2_S_PURCHASED_FROM_S1_FOR_S4_ADENA = new SystemMessage(1154);

        /// <summary>
        /// ID: 1155
        /// <para/>
        /// Message: +$s2 $s3 have been sold to $s1 for $s4 adena.
        /// </summary>
        public static SystemMessage S3_S2_SOLD_TO_S1_FOR_S4_ADENA = new SystemMessage(1155);

        /// <summary>
        /// ID: 1156
        /// <para/>
        /// Message: +$s2 $s3 has been purchased from $s1 for $s4 adena.
        /// </summary>
        public static SystemMessage S2_S3_PURCHASED_FROM_S1_FOR_S4_ADENA = new SystemMessage(1156);

        /// <summary>
        /// ID: 1157
        /// <para/>
        /// Message: Trying on state lasts for only 5 seconds. When a character's state changes, it can be cancelled.
        /// </summary>
        public static SystemMessage TRYING_ON_STATE = new SystemMessage(1157);

        /// <summary>
        /// ID: 1158
        /// <para/>
        /// Message: You cannot dismount from this elevation.
        /// </summary>
        public static SystemMessage CANNOT_DISMOUNT_FROM_ELEVATION = new SystemMessage(1158);

        /// <summary>
        /// ID: 1159
        /// <para/>
        /// Message: The ferry from Talking Island will arrive at Gludin Harbor in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_10_MINUTES = new SystemMessage(1159);

        /// <summary>
        /// ID: 1160
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Gludin Harbor in approximately 5 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_5_MINUTES = new SystemMessage(1160);

        /// <summary>
        /// ID: 1161
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Gludin Harbor in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GLUDIN_1_MINUTE = new SystemMessage(1161);

        /// <summary>
        /// ID: 1162
        /// <para/>
        /// Message: The ferry from Giran Harbor will be arriving at Talking Island in approximately 15 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_15_MINUTES = new SystemMessage(1162);

        /// <summary>
        /// ID: 1163
        /// <para/>
        /// Message: The ferry from Giran Harbor will be arriving at Talking Island in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_10_MINUTES = new SystemMessage(1163);

        /// <summary>
        /// ID: 1164
        /// <para/>
        /// Message: The ferry from Giran Harbor will be arriving at Talking Island in approximately 5 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_5_MINUTES = new SystemMessage(1164);

        /// <summary>
        /// ID: 1165
        /// <para/>
        /// Message: The ferry from Giran Harbor will be arriving at Talking Island in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_GIRAN_ARRIVE_AT_TALKING_1_MINUTE = new SystemMessage(1165);

        /// <summary>
        /// ID: 1166
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_20_MINUTES = new SystemMessage(1166);

        /// <summary>
        /// ID: 1167
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_15_MINUTES = new SystemMessage(1167);

        /// <summary>
        /// ID: 1168
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_10_MINUTES = new SystemMessage(1168);

        /// <summary>
        /// ID: 1169
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Giran Harbor in approximately 20 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_5_MINUTES = new SystemMessage(1169);

        /// <summary>
        /// ID: 1170
        /// <para/>
        /// Message: The ferry from Talking Island will be arriving at Giran Harbor in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_TALKING_ARRIVE_AT_GIRAN_1_MINUTE = new SystemMessage(1170);

        /// <summary>
        /// ID: 1171
        /// <para/>
        /// Message: The Innadril pleasure boat will arrive in approximately 20 minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ARRIVE_20_MINUTES = new SystemMessage(1171);

        /// <summary>
        /// ID: 1172
        /// <para/>
        /// Message: The Innadril pleasure boat will arrive in approximately 15 minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ARRIVE_15_MINUTES = new SystemMessage(1172);

        /// <summary>
        /// ID: 1173
        /// <para/>
        /// Message: The Innadril pleasure boat will arrive in approximately 10 minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ARRIVE_10_MINUTES = new SystemMessage(1173);

        /// <summary>
        /// ID: 1174
        /// <para/>
        /// Message: The Innadril pleasure boat will arrive in approximately 5 minutes.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ARRIVE_5_MINUTES = new SystemMessage(1174);

        /// <summary>
        /// ID: 1175
        /// <para/>
        /// Message: The Innadril pleasure boat will arrive in approximately 1 minute.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_ARRIVE_1_MINUTE = new SystemMessage(1175);

        /// <summary>
        /// ID: 1176
        /// <para/>
        /// Message: This is a quest event period.
        /// </summary>
        public static SystemMessage QUEST_EVENT_PERIOD = new SystemMessage(1176);

        /// <summary>
        /// ID: 1177
        /// <para/>
        /// Message: This is the seal validation period.
        /// </summary>
        public static SystemMessage VALIDATION_PERIOD = new SystemMessage(1177);

        /// <summary>
        /// ID: 1178
        /// <para/>
        /// <Seal of Avarice description>
        /// </summary>
        public static SystemMessage AVARICE_DESCRIPTION = new SystemMessage(1178);

        /// <summary>
        /// ID: 1179
        /// <para/>
        /// <Seal of Gnosis description>
        /// </summary>
        public static SystemMessage GNOSIS_DESCRIPTION = new SystemMessage(1179);

        /// <summary>
        /// ID: 1180
        /// <para/>
        /// <Seal of Strife description>
        /// </summary>
        public static SystemMessage STRIFE_DESCRIPTION = new SystemMessage(1180);

        /// <summary>
        /// ID: 1181
        /// <para/>
        /// Message: Do you really wish to change the title?
        /// </summary>
        public static SystemMessage CHANGE_TITLE_CONFIRM = new SystemMessage(1181);

        /// <summary>
        /// ID: 1182
        /// <para/>
        /// Message: Are you sure you wish to delete the clan crest?
        /// </summary>
        public static SystemMessage CREST_DELETE_CONFIRM = new SystemMessage(1182);

        /// <summary>
        /// ID: 1183
        /// <para/>
        /// Message: This is the initial period.
        /// </summary>
        public static SystemMessage INITIAL_PERIOD = new SystemMessage(1183);

        /// <summary>
        /// ID: 1184
        /// <para/>
        /// Message: This is a period of calculating statistics in the server.
        /// </summary>
        public static SystemMessage RESULTS_PERIOD = new SystemMessage(1184);

        /// <summary>
        /// ID: 1185
        /// <para/>
        /// Message: days left until deletion.
        /// </summary>
        public static SystemMessage DAYS_LEFT_UNTIL_DELETION = new SystemMessage(1185);

        /// <summary>
        /// ID: 1186
        /// <para/>
        /// Message: To create a new account, please visit the PlayNC website (http://www.plaync.com/us/support/)
        /// </summary>
        public static SystemMessage TO_CREATE_ACCOUNT_VISIT_WEBSITE = new SystemMessage(1186);

        /// <summary>
        /// ID: 1187
        /// <para/>
        /// Message: If you forgotten your account information or password, please visit the Support Center on the PlayNC website(http://www.plaync.com/us/support/)
        /// </summary>
        public static SystemMessage ACCOUNT_INFORMATION_FORGOTTON_VISIT_WEBSITE = new SystemMessage(1187);

        /// <summary>
        /// ID: 1188
        /// <para/>
        /// Message: Your selected target can no longer receive a recommendation.
        /// </summary>
        public static SystemMessage YOUR_TARGET_NO_LONGER_RECEIVE_A_RECOMMENDATION = new SystemMessage(1188);

        /// <summary>
        /// ID: 1189
        /// <para/>
        /// Message: This temporary alliance of the Castle Attacker team is in effect. It will be dissolved when the Castle Lord is replaced.
        /// </summary>
        public static SystemMessage TEMPORARY_ALLIANCE = new SystemMessage(1189);

        /// <summary>
        /// ID: 1189
        /// <para/>
        /// Message: This temporary alliance of the Castle Attacker team has been dissolved.
        /// </summary>
        public static SystemMessage TEMPORARY_ALLIANCE_DISSOLVED = new SystemMessage(1189);

        /// <summary>
        /// ID: 1191
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Talking Island in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_10_MINUTES = new SystemMessage(1191);

        /// <summary>
        /// ID: 1192
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Talking Island in approximately 5 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_5_MINUTES = new SystemMessage(1192);

        /// <summary>
        /// ID: 1193
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Talking Island in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_ARRIVE_AT_TALKING_1_MINUTE = new SystemMessage(1193);

        /// <summary>
        /// ID: 1194
        /// <para/>
        /// Message: A mercenary can be assigned to a position from the beginning of the Seal Validatio period until the time when a siege starts.
        /// </summary>
        public static SystemMessage MERC_CAN_BE_ASSIGNED = new SystemMessage(1194);

        /// <summary>
        /// ID: 1195
        /// <para/>
        /// Message: This mercenary cannot be assigned to a position by using the Seal of Strife.
        /// </summary>
        public static SystemMessage MERC_CANT_BE_ASSIGNED_USING_STRIFE = new SystemMessage(1195);

        /// <summary>
        /// ID: 1196
        /// <para/>
        /// Message: Your force has reached maximum capacity.
        /// </summary>
        public static SystemMessage FORCE_MAXIMUM = new SystemMessage(1196);

        /// <summary>
        /// ID: 1197
        /// <para/>
        /// Message: Summoning a servitor costs $s2 $s1.
        /// </summary>
        public static SystemMessage SUMMONING_SERVITOR_COSTS_S2_S1 = new SystemMessage(1197);

        /// <summary>
        /// ID: 1198
        /// <para/>
        /// Message: The item has been successfully crystallized.
        /// </summary>
        public static SystemMessage CRYSTALLIZATION_SUCCESSFUL = new SystemMessage(1198);

        /// <summary>
        /// ID: 1199
        /// <para/>
        /// Message: =======<Clan War Target>=======
        /// </summary>
        public static SystemMessage CLAN_WAR_HEADER = new SystemMessage(1199);

        /// <summary>
        /// ID: 1200
        /// <para/>
        /// Message:($s1 ($s2 Alliance)
        /// </summary>
        public static SystemMessage S1_S2_ALLIANCE = new SystemMessage(1200);

        /// <summary>
        /// ID: 1201
        /// <para/>
        /// Message: Please select the quest you wish to abort.
        /// </summary>
        public static SystemMessage SELECT_QUEST_TO_ABOR = new SystemMessage(1201);

        /// <summary>
        /// ID: 1202
        /// <para/>
        /// Message:($s1 (No alliance exists)
        /// </summary>
        public static SystemMessage S1_NO_ALLI_EXISTS = new SystemMessage(1202);

        /// <summary>
        /// ID: 1203
        /// <para/>
        /// Message: There is no clan war in progress.
        /// </summary>
        public static SystemMessage NO_WAR_IN_PROGRESS = new SystemMessage(1203);

        /// <summary>
        /// ID: 1204
        /// <para/>
        /// Message: The screenshot has been saved. ($s1 $s2x$s3)
        /// </summary>
        public static SystemMessage SCREENSHOT = new SystemMessage(1204);

        /// <summary>
        /// ID: 1205
        /// <para/>
        /// Message: Your mailbox is full. There is a 100 message limit.
        /// </summary>
        public static SystemMessage MAILBOX_FULL = new SystemMessage(1205);

        /// <summary>
        /// ID: 1206
        /// <para/>
        /// Message: The memo box is full. There is a 100 memo limit.
        /// </summary>
        public static SystemMessage MEMOBOX_FULL = new SystemMessage(1206);

        /// <summary>
        /// ID: 1207
        /// <para/>
        /// Message: Please make an entry in the field.
        /// </summary>
        public static SystemMessage MAKE_AN_ENTRY = new SystemMessage(1207);

        /// <summary>
        /// ID: 1208
        /// <para/>
        /// Message: $s1 died and dropped $s3 $s2.
        /// </summary>
        public static SystemMessage S1_DIED_DROPPED_S3_S2 = new SystemMessage(1208);

        /// <summary>
        /// ID: 1209
        /// <para/>
        /// Message: Congratulations. Your raid was successful.
        /// </summary>
        public static SystemMessage RAID_WAS_SUCCESSFUL = new SystemMessage(1209);

        /// <summary>
        /// ID: 1210
        /// <para/>
        /// Message: Seven Signs: The quest event period has begun. Visit a Priest of Dawn or Priestess of Dusk to participate in the event.
        /// </summary>
        public static SystemMessage QUEST_EVENT_PERIOD_BEGUN = new SystemMessage(1210);

        /// <summary>
        /// ID: 1211
        /// <para/>
        /// Message: Seven Signs: The quest event period has ended. The next quest event will start in one week.
        /// </summary>
        public static SystemMessage QUEST_EVENT_PERIOD_ENDED = new SystemMessage(1211);

        /// <summary>
        /// ID: 1212
        /// <para/>
        /// Message: Seven Signs: The Lords of Dawn have obtained the Seal of Avarice.
        /// </summary>
        public static SystemMessage DAWN_OBTAINED_AVARICE = new SystemMessage(1212);

        /// <summary>
        /// ID: 1213
        /// <para/>
        /// Message: Seven Signs: The Lords of Dawn have obtained the Seal of Gnosis.
        /// </summary>
        public static SystemMessage DAWN_OBTAINED_GNOSIS = new SystemMessage(1213);

        /// <summary>
        /// ID: 1214
        /// <para/>
        /// Message: Seven Signs: The Lords of Dawn have obtained the Seal of Strife.
        /// </summary>
        public static SystemMessage DAWN_OBTAINED_STRIFE = new SystemMessage(1214);

        /// <summary>
        /// ID: 1215
        /// <para/>
        /// Message: Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Avarice.
        /// </summary>
        public static SystemMessage DUSK_OBTAINED_AVARICE = new SystemMessage(1215);

        /// <summary>
        /// ID: 1216
        /// <para/>
        /// Message: Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Gnosis.
        /// </summary>
        public static SystemMessage DUSK_OBTAINED_GNOSIS = new SystemMessage(1216);

        /// <summary>
        /// ID: 1217
        /// <para/>
        /// Message: Seven Signs: The Revolutionaries of Dusk have obtained the Seal of Strife.
        /// </summary>
        public static SystemMessage DUSK_OBTAINED_STRIFE = new SystemMessage(1217);

        /// <summary>
        /// ID: 1218
        /// <para/>
        /// Message: Seven Signs: The Seal Validation period has begun.
        /// </summary>
        public static SystemMessage SEAL_VALIDATION_PERIOD_BEGUN = new SystemMessage(1218);

        /// <summary>
        /// ID: 1219
        /// <para/>
        /// Message: Seven Signs: The Seal Validation period has ended.
        /// </summary>
        public static SystemMessage SEAL_VALIDATION_PERIOD_ENDED = new SystemMessage(1219);

        /// <summary>
        /// ID: 1220
        /// <para/>
        /// Message: Are you sure you wish to summon it?
        /// </summary>
        public static SystemMessage SUMMON_CONFIRM = new SystemMessage(1220);

        /// <summary>
        /// ID: 1221
        /// <para/>
        /// Message: Are you sure you wish to return it?
        /// </summary>
        public static SystemMessage RETURN_CONFIRM = new SystemMessage(1221);

        /// <summary>
        /// ID: 1222
        /// <para/>
        /// Message: Current location : $s1, $s2, $s3 (GM Consultation Service)
        /// </summary>
        public static SystemMessage LOC_GM_CONSULATION_SERVICE_S1_S2_S3 = new SystemMessage(1222);

        /// <summary>
        /// ID: 1223
        /// <para/>
        /// Message: We depart for Talking Island in five minutes.
        /// </summary>
        public static SystemMessage DEPART_FOR_TALKING_5_MINUTES = new SystemMessage(1223);

        /// <summary>
        /// ID: 1224
        /// <para/>
        /// Message: We depart for Talking Island in one minute.
        /// </summary>
        public static SystemMessage DEPART_FOR_TALKING_1_MINUTE = new SystemMessage(1224);

        /// <summary>
        /// ID: 1225
        /// <para/>
        /// Message: All aboard for Talking Island
        /// </summary>
        public static SystemMessage DEPART_FOR_TALKING = new SystemMessage(1225);

        /// <summary>
        /// ID: 1226
        /// <para/>
        /// Message: We are now leaving for Talking Island.
        /// </summary>
        public static SystemMessage LEAVING_FOR_TALKING = new SystemMessage(1226);

        /// <summary>
        /// ID: 1227
        /// <para/>
        /// Message: You have $s1 unread messages.
        /// </summary>
        public static SystemMessage S1_UNREAD_MESSAGES = new SystemMessage(1227);

        /// <summary>
        /// ID: 1228
        /// <para/>
        /// Message: $s1 has blocked you. You cannot send mail to $s1.
        /// </summary>
        public static SystemMessage S1_BLOCKED_YOU_CANNOT_MAIL = new SystemMessage(1228);

        /// <summary>
        /// ID: 1229
        /// <para/>
        /// Message: No more messages may be sent at this time. Each account is allowed 10 messages per day.
        /// </summary>
        public static SystemMessage NO_MORE_MESSAGES_TODAY = new SystemMessage(1229);

        /// <summary>
        /// ID: 1230
        /// <para/>
        /// Message: You are limited to five recipients at a time.
        /// </summary>
        public static SystemMessage ONLY_FIVE_RECIPIENTS = new SystemMessage(1230);

        /// <summary>
        /// ID: 1231
        /// <para/>
        /// Message: You've sent mail.
        /// </summary>
        public static SystemMessage SENT_MAIL = new SystemMessage(1231);

        /// <summary>
        /// ID: 1232
        /// <para/>
        /// Message: The message was not sent.
        /// </summary>
        public static SystemMessage MESSAGE_NOT_SENT = new SystemMessage(1232);

        /// <summary>
        /// ID: 1233
        /// <para/>
        /// Message: You've got mail.
        /// </summary>
        public static SystemMessage NEW_MAIL = new SystemMessage(1233);

        /// <summary>
        /// ID: 1234
        /// <para/>
        /// Message: The mail has been stored in your temporary mailbox.
        /// </summary>
        public static SystemMessage MAIL_STORED_IN_MAILBOX = new SystemMessage(1234);

        /// <summary>
        /// ID: 1235
        /// <para/>
        /// Message: Do you wish to delete all your friends?
        /// </summary>
        public static SystemMessage ALL_FRIENDS_DELETE_CONFIRM = new SystemMessage(1235);

        /// <summary>
        /// ID: 1236
        /// <para/>
        /// Message: Please enter security card number.
        /// </summary>
        public static SystemMessage ENTER_SECURITY_CARD_NUMBER = new SystemMessage(1236);

        /// <summary>
        /// ID: 1237
        /// <para/>
        /// Message: Please enter the card number for number $s1.
        /// </summary>
        public static SystemMessage ENTER_CARD_NUMBER_FOR_S1 = new SystemMessage(1237);

        /// <summary>
        /// ID: 1238
        /// <para/>
        /// Message: Your temporary mailbox is full. No more mail can be stored; you have reached the 10 message limit.
        /// </summary>
        public static SystemMessage TEMP_MAILBOX_FULL = new SystemMessage(1238);

        /// <summary>
        /// ID: 1239
        /// <para/>
        /// Message: The keyboard security module has failed to load. Please exit the game and try again.
        /// </summary>
        public static SystemMessage KEYBOARD_MODULE_FAILED_LOAD = new SystemMessage(1239);

        /// <summary>
        /// ID: 1240
        /// <para/>
        /// Message: Seven Signs: The Revolutionaries of Dusk have won.
        /// </summary>
        public static SystemMessage DUSK_WON = new SystemMessage(1240);

        /// <summary>
        /// ID: 1241
        /// <para/>
        /// Message: Seven Signs: The Lords of Dawn have won.
        /// </summary>
        public static SystemMessage DAWN_WON = new SystemMessage(1241);

        /// <summary>
        /// ID: 1242
        /// <para/>
        /// Message: Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m.
        /// </summary>
        public static SystemMessage NOT_VERIFIED_AGE_NO_LOGIN = new SystemMessage(1242);

        /// <summary>
        /// ID: 1243
        /// <para/>
        /// Message: The security card number is invalid.
        /// </summary>
        public static SystemMessage SECURITY_CARD_NUMBER_INVALID = new SystemMessage(1243);

        /// <summary>
        /// ID: 1244
        /// <para/>
        /// Message: Users who have not verified their age may not log in between the hours if 10:00 p.m. and 6:00 a.m. Logging off now
        /// </summary>
        public static SystemMessage NOT_VERIFIED_AGE_LOG_OFF = new SystemMessage(1244);

        /// <summary>
        /// ID: 1245
        /// <para/>
        /// Message: You will be loged out in $s1 minutes.
        /// </summary>
        public static SystemMessage LOGOUT_IN_S1_MINUTES = new SystemMessage(1245);

        /// <summary>
        /// ID: 1246
        /// <para/>
        /// Message: $s1 died and has dropped $s2 adena.
        /// </summary>
        public static SystemMessage S1_DIED_DROPPED_S2_ADENA = new SystemMessage(1246);

        /// <summary>
        /// ID: 1247
        /// <para/>
        /// Message: The corpse is too old. The skill cannot be used.
        /// </summary>
        public static SystemMessage CORPSE_TOO_OLD_SKILL_NOT_USED = new SystemMessage(1247);

        /// <summary>
        /// ID: 1248
        /// <para/>
        /// Message: You are out of feed. Mount status canceled.
        /// </summary>
        public static SystemMessage OUT_OF_FEED_MOUNT_CANCELED = new SystemMessage(1248);

        /// <summary>
        /// ID: 1249
        /// <para/>
        /// Message: You may only ride a wyvern while you're riding a strider.
        /// </summary>
        public static SystemMessage YOU_MAY_ONLY_RIDE_WYVERN_WHILE_RIDING_STRIDER = new SystemMessage(1249);

        /// <summary>
        /// ID: 1250
        /// <para/>
        /// Message: Do you really want to surrender? If you surrender during an alliance war, your Exp will drop the same as if you were to die once.
        /// </summary>
        public static SystemMessage SURRENDER_ALLY_WAR_CONFIRM = new SystemMessage(1250);

        /// <summary>
        /// ID: 1251
        /// <para/>
        /// you will not be able to accept another clan to your alliance for one day.
        /// </summary>
        public static SystemMessage DISMISS_ALLY_CONFIRM = new SystemMessage(1251);

        /// <summary>
        /// ID: 1252
        /// <para/>
        /// Message: Are you sure you want to surrender? Exp penalty will be the same as death.
        /// </summary>
        public static SystemMessage SURRENDER_CONFIRM1 = new SystemMessage(1252);

        /// <summary>
        /// ID: 1253
        /// <para/>
        /// Message: Are you sure you want to surrender? Exp penalty will be the same as death and you will not be allowed to participate in clan war.
        /// </summary>
        public static SystemMessage SURRENDER_CONFIRM2 = new SystemMessage(1253);

        /// <summary>
        /// ID: 1254
        /// <para/>
        /// Message: Thank you for submitting feedback.
        /// </summary>
        public static SystemMessage THANKS_FOR_FEEDBACK = new SystemMessage(1254);

        /// <summary>
        /// ID: 1255
        /// <para/>
        /// Message: GM consultation has begun.
        /// </summary>
        public static SystemMessage GM_CONSULTATION_BEGUN = new SystemMessage(1255);

        /// <summary>
        /// ID: 1256
        /// <para/>
        /// Message: Please write the name after the command.
        /// </summary>
        public static SystemMessage PLEASE_WRITE_NAME_AFTER_COMMAND = new SystemMessage(1256);

        /// <summary>
        /// ID: 1257
        /// <para/>
        /// Message: The special skill of a servitor or pet cannot be registerd as a macro.
        /// </summary>
        public static SystemMessage PET_SKILL_NOT_AS_MACRO = new SystemMessage(1257);

        /// <summary>
        /// ID: 1258
        /// <para/>
        /// Message: $s1 has been crystallized
        /// </summary>
        public static SystemMessage S1_CRYSTALLIZED = new SystemMessage(1258);

        /// <summary>
        /// ID: 1259
        /// <para/>
        /// Message: =======<Alliance Target>=======
        /// </summary>
        public static SystemMessage ALLIANCE_TARGET_HEADER = new SystemMessage(1259);

        /// <summary>
        /// ID: 1260
        /// <para/>
        /// Message: Seven Signs: Preparations have begun for the next quest event.
        /// </summary>
        public static SystemMessage PREPARATIONS_PERIOD_BEGUN = new SystemMessage(1260);

        /// <summary>
        /// ID: 1261
        /// <para/>
        /// Message: Seven Signs: The quest event period has begun. Speak with a Priest of Dawn or Dusk Priestess if you wish to participate in the event.
        /// </summary>
        public static SystemMessage COMPETITION_PERIOD_BEGUN = new SystemMessage(1261);

        /// <summary>
        /// ID: 1262
        /// <para/>
        /// Message: Seven Signs: Quest event has ended. Results are being tallied.
        /// </summary>
        public static SystemMessage RESULTS_PERIOD_BEGUN = new SystemMessage(1262);

        /// <summary>
        /// ID: 1263
        /// <para/>
        /// Message: Seven Signs: This is the seal validation period. A new quest event period begins next Monday.
        /// </summary>
        public static SystemMessage VALIDATION_PERIOD_BEGUN = new SystemMessage(1263);

        /// <summary>
        /// ID: 1264
        /// <para/>
        /// Message: This soul stone cannot currently absorb souls. Absorption has failed.
        /// </summary>
        public static SystemMessage STONE_CANNOT_ABSORB = new SystemMessage(1264);

        /// <summary>
        /// ID: 1265
        /// <para/>
        /// Message: You can't absorb souls without a soul stone.
        /// </summary>
        public static SystemMessage CANT_ABSORB_WITHOUT_STONE = new SystemMessage(1265);

        /// <summary>
        /// ID: 1266
        /// <para/>
        /// Message: The exchange has ended.
        /// </summary>
        public static SystemMessage EXCHANGE_HAS_ENDED = new SystemMessage(1266);

        /// <summary>
        /// ID: 1267
        /// <para/>
        /// Message: Your contribution score is increased by $s1.
        /// </summary>
        public static SystemMessage CONTRIB_SCORE_INCREASED_S1 = new SystemMessage(1267);

        /// <summary>
        /// ID: 1268
        /// <para/>
        /// Message: Do you wish to add class as your sub class?
        /// </summary>
        public static SystemMessage ADD_SUBCLASS_CONFIRM = new SystemMessage(1268);

        /// <summary>
        /// ID: 1269
        /// <para/>
        /// Message: The new sub class has been added.
        /// </summary>
        public static SystemMessage ADD_NEW_SUBCLASS = new SystemMessage(1269);

        /// <summary>
        /// ID: 1270
        /// <para/>
        /// Message: The transfer of sub class has been completed.
        /// </summary>
        public static SystemMessage SUBCLASS_TRANSFER_COMPLETED = new SystemMessage(1270);

        /// <summary>
        /// ID: 1271
        /// <para/>
        /// Message: Do you wish to participate? Until the next seal validation period, you are a member of the Lords of Dawn.
        /// </summary>
        public static SystemMessage DAWN_CONFIRM = new SystemMessage(1271);

        /// <summary>
        /// ID: 1271
        /// <para/>
        /// Message: Do you wish to participate? Until the next seal validation period, you are a member of the Revolutionaries of Dusk.
        /// </summary>
        public static SystemMessage DUSK_CONFIRM = new SystemMessage(1271);

        /// <summary>
        /// ID: 1273
        /// <para/>
        /// Message: You will participate in the Seven Signs as a member of the Lords of Dawn.
        /// </summary>
        public static SystemMessage SEVENSIGNS_PARTECIPATION_DAWN = new SystemMessage(1273);

        /// <summary>
        /// ID: 1274
        /// <para/>
        /// Message: You will participate in the Seven Signs as a member of the Revolutionaries of Dusk.
        /// </summary>
        public static SystemMessage SEVENSIGNS_PARTECIPATION_DUSK = new SystemMessage(1274);

        /// <summary>
        /// ID: 1275
        /// <para/>
        /// Message: You've chosen to fight for the Seal of Avarice during this quest event period.
        /// </summary>
        public static SystemMessage FIGHT_FOR_AVARICE = new SystemMessage(1275);

        /// <summary>
        /// ID: 1276
        /// <para/>
        /// Message: You've chosen to fight for the Seal of Gnosis during this quest event period.
        /// </summary>
        public static SystemMessage FIGHT_FOR_GNOSIS = new SystemMessage(1276);

        /// <summary>
        /// ID: 1277
        /// <para/>
        /// Message: You've chosen to fight for the Seal of Strife during this quest event period.
        /// </summary>
        public static SystemMessage FIGHT_FOR_STRIFE = new SystemMessage(1277);

        /// <summary>
        /// ID: 1278
        /// <para/>
        /// Message: The NPC server is not operating at this time.
        /// </summary>
        public static SystemMessage NPC_SERVER_NOT_OPERATING = new SystemMessage(1278);

        /// <summary>
        /// ID: 1279
        /// <para/>
        /// Message: Contribution level has exceeded the limit. You may not continue.
        /// </summary>
        public static SystemMessage CONTRIB_SCORE_EXCEEDED = new SystemMessage(1279);

        /// <summary>
        /// ID: 1280
        /// <para/>
        /// Message: Magic Critical Hit!
        /// </summary>
        public static SystemMessage CRITICAL_HIT_MAGIC = new SystemMessage(1280);

        /// <summary>
        /// ID: 1281
        /// <para/>
        /// Message: Your excellent shield defense was a success!
        /// </summary>
        public static SystemMessage YOUR_EXCELLENT_SHIELD_DEFENSE_WAS_A_SUCCESS = new SystemMessage(1281);

        /// <summary>
        /// ID: 1282
        /// <para/>
        /// Message: Your Karma has been changed to $s1
        /// </summary>
        public static SystemMessage YOUR_KARMA_HAS_BEEN_CHANGED_TO_S1 = new SystemMessage(1282);

        /// <summary>
        /// ID: 1283
        /// <para/>
        /// Message: The minimum frame option has been activated.
        /// </summary>
        public static SystemMessage MINIMUM_FRAME_ACTIVATED = new SystemMessage(1283);

        /// <summary>
        /// ID: 1284
        /// <para/>
        /// Message: The minimum frame option has been deactivated.
        /// </summary>
        public static SystemMessage MINIMUM_FRAME_DEACTIVATED = new SystemMessage(1284);

        /// <summary>
        /// ID: 1285
        /// <para/>
        /// Message: No inventory exists: You cannot purchase an item.
        /// </summary>
        public static SystemMessage NO_INVENTORY_CANNOT_PURCHASE = new SystemMessage(1285);

        /// <summary>
        /// ID: 1286
        /// <para/>
        /// Message: (Until next Monday at 6:00 p.m.)
        /// </summary>
        public static SystemMessage UNTIL_MONDAY_6PM = new SystemMessage(1286);

        /// <summary>
        /// ID: 1287
        /// <para/>
        /// Message: (Until today at 6:00 p.m.)
        /// </summary>
        public static SystemMessage UNTIL_TODAY_6PM = new SystemMessage(1287);

        /// <summary>
        /// ID: 1288
        /// <para/>
        /// Message: If trends continue, $s1 will win and the seal will belong to:
        /// </summary>
        public static SystemMessage S1_WILL_WIN_COMPETITION = new SystemMessage(1288);

        /// <summary>
        /// ID: 1289
        /// <para/>
        /// Message: (Until next Monday at 6:00 p.m.)
        /// </summary>
        public static SystemMessage SEAL_OWNED_10_MORE_VOTED = new SystemMessage(1289);

        /// <summary>
        /// ID: 1290
        /// <para/>
        /// Message: Although the seal was not owned, since 35 percent or more people have voted.
        /// </summary>
        public static SystemMessage SEAL_NOT_OWNED_35_MORE_VOTED = new SystemMessage(1290);

        /// <summary>
        /// ID: 1291
        /// <para/>
        /// because less than 10 percent of people have voted.
        /// </summary>
        public static SystemMessage SEAL_OWNED_10_LESS_VOTED = new SystemMessage(1291);

        /// <summary>
        /// ID: 1292
        /// <para/>
        /// and since less than 35 percent of people have voted.
        /// </summary>
        public static SystemMessage SEAL_NOT_OWNED_35_LESS_VOTED = new SystemMessage(1292);

        /// <summary>
        /// ID: 1293
        /// <para/>
        /// Message: If current trends continue, it will end in a tie.
        /// </summary>
        public static SystemMessage COMPETITION_WILL_TIE = new SystemMessage(1293);

        /// <summary>
        /// ID: 1294
        /// <para/>
        /// Message: The competition has ended in a tie. Therefore, nobody has been awarded the seal.
        /// </summary>
        public static SystemMessage COMPETITION_TIE_SEAL_NOT_AWARDED = new SystemMessage(1294);

        /// <summary>
        /// ID: 1295
        /// <para/>
        /// Message: Sub classes may not be created or changed while a skill is in use.
        /// </summary>
        public static SystemMessage SUBCLASS_NO_CHANGE_OR_CREATE_WHILE_SKILL_IN_USE = new SystemMessage(1295);

        /// <summary>
        /// ID: 1296
        /// <para/>
        /// Message: You cannot open a Private Store here.
        /// </summary>
        public static SystemMessage NO_PRIVATE_STORE_HERE = new SystemMessage(1296);

        /// <summary>
        /// ID: 1297
        /// <para/>
        /// Message: You cannot open a Private Workshop here.
        /// </summary>
        public static SystemMessage NO_PRIVATE_WORKSHOP_HERE = new SystemMessage(1297);

        /// <summary>
        /// ID: 1298
        /// <para/>
        /// Message: Please confirm that you would like to exit the Monster Race Track.
        /// </summary>
        public static SystemMessage MONS_EXIT_CONFIRM = new SystemMessage(1298);

        /// <summary>
        /// ID: 1299
        /// <para/>
        /// Message: $s1's casting has been interrupted.
        /// </summary>
        public static SystemMessage S1_CASTING_INTERRUPTED = new SystemMessage(1299);

        /// <summary>
        /// ID: 1300
        /// <para/>
        /// Message: You are no longer trying on equipment.
        /// </summary>
        public static SystemMessage WEAR_ITEMS_STOPPED = new SystemMessage(1300);

        /// <summary>
        /// ID: 1301
        /// <para/>
        /// Message: Only a Lord of Dawn may use this.
        /// </summary>
        public static SystemMessage CAN_BE_USED_BY_DAWN = new SystemMessage(1301);

        /// <summary>
        /// ID: 1302
        /// <para/>
        /// Message: Only a Revolutionary of Dusk may use this.
        /// </summary>
        public static SystemMessage CAN_BE_USED_BY_DUSK = new SystemMessage(1302);

        /// <summary>
        /// ID: 1303
        /// <para/>
        /// Message: This may only be used during the quest event period.
        /// </summary>
        public static SystemMessage CAN_BE_USED_DURING_QUEST_EVENT_PERIOD = new SystemMessage(1303);

        /// <summary>
        /// ID: 1304
        /// <para/>
        /// except for an Alliance with a castle owning clan.
        /// </summary>
        public static SystemMessage STRIFE_CANCELED_DEFENSIVE_REGISTRATION = new SystemMessage(1304);

        /// <summary>
        /// ID: 1305
        /// <para/>
        /// Message: Seal Stones may only be transferred during the quest event period.
        /// </summary>
        public static SystemMessage SEAL_STONES_ONLY_WHILE_QUEST = new SystemMessage(1305);

        /// <summary>
        /// ID: 1306
        /// <para/>
        /// Message: You are no longer trying on equipment.
        /// </summary>
        public static SystemMessage NO_LONGER_TRYING_ON = new SystemMessage(1306);

        /// <summary>
        /// ID: 1307
        /// <para/>
        /// Message: Only during the seal validation period may you settle your account.
        /// </summary>
        public static SystemMessage SETTLE_ACCOUNT_ONLY_IN_SEAL_VALIDATION = new SystemMessage(1307);

        /// <summary>
        /// ID: 1308
        /// <para/>
        /// Message: Congratulations - You've completed a class transfer!
        /// </summary>
        public static SystemMessage CLASS_TRANSFER = new SystemMessage(1308);

        /// <summary>
        /// ID: 1309
        /// <para/>
        /// Message:To use this option, you must have the lastest version of MSN Messenger installed on your computer.
        /// </summary>
        public static SystemMessage LATEST_MSN_REQUIRED = new SystemMessage(1309);

        /// <summary>
        /// ID: 1310
        /// <para/>
        /// Message: For full functionality, the latest version of MSN Messenger must be installed on your computer.
        /// </summary>
        public static SystemMessage LATEST_MSN_RECOMMENDED = new SystemMessage(1310);

        /// <summary>
        /// ID: 1311
        /// <para/>
        /// Message: Previous versions of MSN Messenger only provide the basic features for in-game MSN Messenger Chat. Add/Delete Contacts and other MSN Messenger options are not available
        /// </summary>
        public static SystemMessage MSN_ONLY_BASIC = new SystemMessage(1311);

        /// <summary>
        /// ID: 1312
        /// <para/>
        /// Message: The latest version of MSN Messenger may be obtained from the MSN web site (http://messenger.msn.com).
        /// </summary>
        public static SystemMessage MSN_OBTAINED_FROM = new SystemMessage(1312);

        /// <summary>
        /// ID: 1313
        /// <para/>
        /// Message: $s1, to better serve our customers, all chat histories [...]
        /// </summary>
        public static SystemMessage S1_CHAT_HISTORIES_STORED = new SystemMessage(1313);

        /// <summary>
        /// ID: 1314
        /// <para/>
        /// Message: Please enter the passport ID of the person you wish to add to your contact list.
        /// </summary>
        public static SystemMessage ENTER_PASSPORT_FOR_ADDING = new SystemMessage(1314);

        /// <summary>
        /// ID: 1315
        /// <para/>
        /// Message: Deleting a contact will remove that contact from MSN Messenger as well. The contact can still check your online status and well not be blocked from sending you a message.
        /// </summary>
        public static SystemMessage DELETING_A_CONTACT = new SystemMessage(1315);

        /// <summary>
        /// ID: 1316
        /// <para/>
        /// Message: The contact will be deleted and blocked from your contact list.
        /// </summary>
        public static SystemMessage CONTACT_WILL_DELETED = new SystemMessage(1316);

        /// <summary>
        /// ID: 1317
        /// <para/>
        /// Message: Would you like to delete this contact?
        /// </summary>
        public static SystemMessage CONTACT_DELETE_CONFIRM = new SystemMessage(1317);

        /// <summary>
        /// ID: 1318
        /// <para/>
        /// Message: Please select the contact you want to block or unblock.
        /// </summary>
        public static SystemMessage SELECT_CONTACT_FOR_BLOCK_UNBLOCK = new SystemMessage(1318);

        /// <summary>
        /// ID: 1319
        /// <para/>
        /// Message: Please select the name of the contact you wish to change to another group.
        /// </summary>
        public static SystemMessage SELECT_CONTACT_FOR_CHANGE_GROUP = new SystemMessage(1319);

        /// <summary>
        /// ID: 1320
        /// <para/>
        /// Message: After selecting the group you wish to move your contact to, press the OK button.
        /// </summary>
        public static SystemMessage SELECT_GROUP_PRESS_OK = new SystemMessage(1320);

        /// <summary>
        /// ID: 1321
        /// <para/>
        /// Message: Enter the name of the group you wish to add.
        /// </summary>
        public static SystemMessage ENTER_GROUP_NAME = new SystemMessage(1321);

        /// <summary>
        /// ID: 1322
        /// <para/>
        /// Message: Select the group and enter the new name.
        /// </summary>
        public static SystemMessage SELECT_GROUP_ENTER_NAME = new SystemMessage(1322);

        /// <summary>
        /// ID: 1323
        /// <para/>
        /// Message: Select the group you wish to delete and click the OK button.
        /// </summary>
        public static SystemMessage SELECT_GROUP_TO_DELETE = new SystemMessage(1323);

        /// <summary>
        /// ID: 1324
        /// <para/>
        /// Message: Signing in...
        /// </summary>
        public static SystemMessage SIGNING_IN = new SystemMessage(1324);

        /// <summary>
        /// ID: 1325
        /// <para/>
        /// Message: You've logged into another computer and have been logged out of the .NET Messenger Service on this computer.
        /// </summary>
        public static SystemMessage ANOTHER_COMPUTER_LOGOUT = new SystemMessage(1325);

        /// <summary>
        /// ID: 1326
        /// <para/>
        /// Message: $s1 :
        /// </summary>
        public static SystemMessage S1_D = new SystemMessage(1326);

        /// <summary>
        /// ID: 1327
        /// <para/>
        /// Message: The following message could not be delivered:
        /// </summary>
        public static SystemMessage MESSAGE_NOT_DELIVERED = new SystemMessage(1327);

        /// <summary>
        /// ID: 1328
        /// <para/>
        /// Message: Members of the Revolutionaries of Dusk will not be resurrected.
        /// </summary>
        public static SystemMessage DUSK_NOT_RESURRECTED = new SystemMessage(1328);

        /// <summary>
        /// ID: 1329
        /// <para/>
        /// Message: You are currently blocked from using the Private Store and Private Workshop.
        /// </summary>
        public static SystemMessage BLOCKED_FROM_USING_STORE = new SystemMessage(1329);

        /// <summary>
        /// ID: 1330
        /// <para/>
        /// Message: You may not open a Private Store or Private Workshop for another $s1 minute(s)
        /// </summary>
        public static SystemMessage NO_STORE_FOR_S1_MINUTES = new SystemMessage(1330);

        /// <summary>
        /// ID: 1331
        /// <para/>
        /// Message: You are no longer blocked from using the Private Store and Private Workshop
        /// </summary>
        public static SystemMessage NO_LONGER_BLOCKED_USING_STORE = new SystemMessage(1331);

        /// <summary>
        /// ID: 1332
        /// <para/>
        /// Message: Items may not be used after your character or pet dies.
        /// </summary>
        public static SystemMessage NO_ITEMS_AFTER_DEATH = new SystemMessage(1332);

        /// <summary>
        /// ID: 1333
        /// <para/>
        /// Message: The replay file is not accessible. Please verify that the replay.ini exists in your Linage 2 directory.
        /// </summary>
        public static SystemMessage REPLAY_INACCESSIBLE = new SystemMessage(1333);

        /// <summary>
        /// ID: 1334
        /// <para/>
        /// Message: The new camera data has been stored.
        /// </summary>
        public static SystemMessage NEW_CAMERA_STORED = new SystemMessage(1334);

        /// <summary>
        /// ID: 1335
        /// <para/>
        /// Message: The attempt to store the new camera data has failed.
        /// </summary>
        public static SystemMessage CAMERA_STORING_FAILED = new SystemMessage(1335);

        /// <summary>
        /// ID: 1336
        /// <para/>
        /// Message: The replay file, $s1.$$s2 has been corrupted, please check the fle.
        /// </summary>
        public static SystemMessage REPLAY_S1_S2_CORRUPTED = new SystemMessage(1336);

        /// <summary>
        /// ID: 1337
        /// <para/>
        /// Message: This will terminate the replay. Do you wish to continue?
        /// </summary>
        public static SystemMessage REPLAY_TERMINATE_CONFIRM = new SystemMessage(1337);

        /// <summary>
        /// ID: 1338
        /// <para/>
        /// Message: You have exceeded the maximum amount that may be transferred at one time.
        /// </summary>
        public static SystemMessage EXCEEDED_MAXIMUM_AMOUNT = new SystemMessage(1338);

        /// <summary>
        /// ID: 1339
        /// <para/>
        /// Message: Once a macro is assigned to a shortcut, it cannot be run as a macro again.
        /// </summary>
        public static SystemMessage MACRO_SHORTCUT_NOT_RUN = new SystemMessage(1339);

        /// <summary>
        /// ID: 1340
        /// <para/>
        /// Message: This server cannot be accessed by the coupon you are using.
        /// </summary>
        public static SystemMessage SERVER_NOT_ACCESSED_BY_COUPON = new SystemMessage(1340);

        /// <summary>
        /// ID: 1341
        /// <para/>
        /// Message: Incorrect name and/or email address.
        /// </summary>
        public static SystemMessage INCORRECT_NAME_OR_ADDRESS = new SystemMessage(1341);

        /// <summary>
        /// ID: 1342
        /// <para/>
        /// Message: You are already logged in.
        /// </summary>
        public static SystemMessage ALREADY_LOGGED_IN = new SystemMessage(1342);

        /// <summary>
        /// ID: 1343
        /// <para/>
        /// Message: Incorrect email address and/or password. Your attempt to log into .NET Messenger Service has failed.
        /// </summary>
        public static SystemMessage INCORRECT_ADDRESS_OR_PASSWORD = new SystemMessage(1343);

        /// <summary>
        /// ID: 1344
        /// <para/>
        /// Message: Your request to log into the .NET Messenger service has failed. Please verify that you are currently connected to the internet.
        /// </summary>
        public static SystemMessage NET_LOGIN_FAILED = new SystemMessage(1344);

        /// <summary>
        /// ID: 1345
        /// <para/>
        /// Message: Click the OK button after you have selected a contact name.
        /// </summary>
        public static SystemMessage SELECT_CONTACT_CLICK_OK = new SystemMessage(1345);

        /// <summary>
        /// ID: 1346
        /// <para/>
        /// Message: You are currently entering a chat message.
        /// </summary>
        public static SystemMessage CURRENTLY_ENTERING_CHAT = new SystemMessage(1346);

        /// <summary>
        /// ID: 1347
        /// <para/>
        /// Message: The Linage II messenger could not carry out the task you requested.
        /// </summary>
        public static SystemMessage MESSENGER_FAILED_CARRYING_OUT_TASK = new SystemMessage(1347);

        /// <summary>
        /// ID: 1348
        /// <para/>
        /// Message: $s1 has entered the chat room.
        /// </summary>
        public static SystemMessage S1_ENTERED_CHAT_ROOM = new SystemMessage(1348);

        /// <summary>
        /// ID: 1349
        /// <para/>
        /// Message: $s1 has left the chat room.
        /// </summary>
        public static SystemMessage S1_LEFT_CHAT_ROOM = new SystemMessage(1349);

        /// <summary>
        /// ID: 1350
        /// <para/>
        /// Message: The state will be changed to indicate "off-line." All the chat windows currently opened will be closed.
        /// </summary>
        public static SystemMessage GOING_OFFLINE = new SystemMessage(1350);

        /// <summary>
        /// ID: 1351
        /// <para/>
        /// Message: Click the Delete button after selecting the contact you wish to remove.
        /// </summary>
        public static SystemMessage SELECT_CONTACT_CLICK_REMOVE = new SystemMessage(1351);

        /// <summary>
        /// ID: 1352
        /// <para/>
        /// Message: You have been added to $s1 ($s2)'s contact list.
        /// </summary>
        public static SystemMessage ADDED_TO_S1_S2_CONTACT_LIST = new SystemMessage(1352);

        /// <summary>
        /// ID: 1353
        /// <para/>
        /// Message: You can set the option to show your status as always being off-line to all of your contacts.
        /// </summary>
        public static SystemMessage CAN_SET_OPTION_TO_ALWAYS_SHOW_OFFLINE = new SystemMessage(1353);

        /// <summary>
        /// ID: 1354
        /// <para/>
        /// Message: You are not allowed to chat with a contact while chatting block is imposed.
        /// </summary>
        public static SystemMessage NO_CHAT_WHILE_BLOCKED = new SystemMessage(1354);

        /// <summary>
        /// ID: 1355
        /// <para/>
        /// Message: The contact is currently blocked from chatting.
        /// </summary>
        public static SystemMessage CONTACT_CURRENTLY_BLOCKED = new SystemMessage(1355);

        /// <summary>
        /// ID: 1356
        /// <para/>
        /// Message: The contact is not currently logged in.
        /// </summary>
        public static SystemMessage CONTACT_CURRENTLY_OFFLINE = new SystemMessage(1356);

        /// <summary>
        /// ID: 1357
        /// <para/>
        /// Message: You have been blocked from chatting with that contact.
        /// </summary>
        public static SystemMessage YOU_ARE_BLOCKED = new SystemMessage(1357);

        /// <summary>
        /// ID: 1358
        /// <para/>
        /// Message: You are being logged out...
        /// </summary>
        public static SystemMessage YOU_ARE_LOGGING_OUT = new SystemMessage(1358);

        /// <summary>
        /// ID: 1359
        /// <para/>
        /// Message: $s1 has logged in.
        /// </summary>
        public static SystemMessage S1_LOGGED_IN2 = new SystemMessage(1359);

        /// <summary>
        /// ID: 1360
        /// <para/>
        /// Message: You have received a message from $s1.
        /// </summary>
        public static SystemMessage GOT_MESSAGE_FROM_S1 = new SystemMessage(1360);

        /// <summary>
        /// ID: 1361
        /// <para/>
        /// Message: Due to a system error, you have been logged out of the .NET Messenger Service.
        /// </summary>
        public static SystemMessage LOGGED_OUT_DUE_TO_ERROR = new SystemMessage(1361);

        /// <summary>
        /// ID: 1362
        /// <para/>
        /// click the button next to My Status and then use the Options menu.
        /// </summary>
        public static SystemMessage SELECT_CONTACT_TO_DELETE = new SystemMessage(1362);

        /// <summary>
        /// ID: 1363
        /// <para/>
        /// Message: Your request to participate in the alliance war has been denied.
        /// </summary>
        public static SystemMessage YOUR_REQUEST_ALLIANCE_WAR_DENIED = new SystemMessage(1363);

        /// <summary>
        /// ID: 1364
        /// <para/>
        /// Message: The request for an alliance war has been rejected.
        /// </summary>
        public static SystemMessage REQUEST_ALLIANCE_WAR_REJECTED = new SystemMessage(1364);

        /// <summary>
        /// ID: 1365
        /// <para/>
        /// Message: $s2 of $s1 clan has surrendered as an individual.
        /// </summary>
        public static SystemMessage S2_OF_S1_SURRENDERED_AS_INDIVIDUAL = new SystemMessage(1365);

        /// <summary>
        /// ID: 1366
        /// <para/>
        /// Message: In order to delete a group, you must not [...]
        /// </summary>
        public static SystemMessage DELTE_GROUP_INSTRUCTION = new SystemMessage(1366);

        /// <summary>
        /// ID: 1367
        /// <para/>
        /// Message: Only members of the group are allowed to add records.
        /// </summary>
        public static SystemMessage ONLY_GROUP_CAN_ADD_RECORDS = new SystemMessage(1367);

        /// <summary>
        /// ID: 1368
        /// <para/>
        /// Message: You can not try those items on at the same time.
        /// </summary>
        public static SystemMessage YOU_CAN_NOT_TRY_THOSE_ITEMS_ON_AT_THE_SAME_TIME = new SystemMessage(1368);

        /// <summary>
        /// ID: 1369
        /// <para/>
        /// Message: You've exceeded the maximum.
        /// </summary>
        public static SystemMessage EXCEEDED_THE_MAXIMUM = new SystemMessage(1369);

        /// <summary>
        /// ID: 1370
        /// <para/>
        /// Message: Your message to $s1 did not reach its recipient. You cannot send mail to the GM staff.
        /// </summary>
        public static SystemMessage CANNOT_MAIL_GM_S1 = new SystemMessage(1370);

        /// <summary>
        /// ID: 1371
        /// <para/>
        /// Message: It has been determined that you're not engaged in normal gameplay and a restriction has been imposed upon you. You may not move for $s1 minutes.
        /// </summary>
        public static SystemMessage GAMEPLAY_RESTRICTION_PENALTY_S1 = new SystemMessage(1371);

        /// <summary>
        /// ID: 1372
        /// <para/>
        /// Message: Your punishment will continue for $s1 minutes.
        /// </summary>
        public static SystemMessage PUNISHMENT_CONTINUE_S1_MINUTES = new SystemMessage(1372);

        /// <summary>
        /// ID: 1373
        /// <para/>
        /// Message: $s1 has picked up $s2 that was dropped by a Raid Boss.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2_FROM_RAIDBOSS = new SystemMessage(1373);

        /// <summary>
        /// ID: 1374
        /// <para/>
        /// Message: $s1 has picked up $s3 $s2(s) that was dropped by a Raid Boss.
        /// </summary>
        public static SystemMessage S1_PICKED_UP_S3_S2_S_FROM_RAIDBOSS = new SystemMessage(1374);

        /// <summary>
        /// ID: 1375
        /// <para/>
        /// Message: $s1 has picked up $s2 adena that was dropped by a Raid Boss.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2_ADENA_FROM_RAIDBOSS = new SystemMessage(1375);

        /// <summary>
        /// ID: 1376
        /// <para/>
        /// Message: $s1 has picked up $s2 that was dropped by another character.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2_FROM_ANOTHER_CHARACTER = new SystemMessage(1376);

        /// <summary>
        /// ID: 1377
        /// <para/>
        /// Message: $s1 has picked up $s3 $s2(s) that was dropped by a another character.
        /// </summary>
        public static SystemMessage S1_PICKED_UP_S3_S2_S_FROM_ANOTHER_CHARACTER = new SystemMessage(1377);

        /// <summary>
        /// ID: 1378
        /// <para/>
        /// Message: $s1 has picked up +$s3 $s2 that was dropped by a another character.
        /// </summary>
        public static SystemMessage S1_PICKED_UP_S3_S2_FROM_ANOTHER_CHARACTER = new SystemMessage(1378);

        /// <summary>
        /// ID: 1379
        /// <para/>
        /// Message: $s1 has obtained $s2 adena.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S2_ADENA = new SystemMessage(1379);

        /// <summary>
        /// ID: 1380
        /// <para/>
        /// Message: You can't summon a $s1 while on the battleground.
        /// </summary>
        public static SystemMessage CANT_SUMMON_S1_ON_BATTLEGROUND = new SystemMessage(1380);

        /// <summary>
        /// ID: 1381
        /// <para/>
        /// Message: The party leader has obtained $s2 of $s1.
        /// </summary>
        public static SystemMessage LEADER_OBTAINED_S2_OF_S1 = new SystemMessage(1381);

        /// <summary>
        /// ID: 1382
        /// <para/>
        /// Message: To fulfill the quest, you must bring the chosen weapon. Are you sure you want to choose this weapon?
        /// </summary>
        public static SystemMessage CHOOSE_WEAPON_CONFIRM = new SystemMessage(1382);

        /// <summary>
        /// ID: 1383
        /// <para/>
        /// Message: Are you sure you want to exchange?
        /// </summary>
        public static SystemMessage EXCHANGE_CONFIRM = new SystemMessage(1383);

        /// <summary>
        /// ID: 1384
        /// <para/>
        /// Message: $s1 has become the party leader.
        /// </summary>
        public static SystemMessage S1_HAS_BECOME_A_PARTY_LEADER = new SystemMessage(1384);

        /// <summary>
        /// ID: 1385
        /// <para/>
        /// Message: You are not allowed to dismount at this location.
        /// </summary>
        public static SystemMessage NO_DISMOUNT_HERE = new SystemMessage(1385);

        /// <summary>
        /// ID: 1386
        /// <para/>
        /// Message: You are no longer held in place.
        /// </summary>
        public static SystemMessage NO_LONGER_HELD_IN_PLACE = new SystemMessage(1386);

        /// <summary>
        /// ID: 1387
        /// <para/>
        /// Message: Please select the item you would like to try on.
        /// </summary>
        public static SystemMessage SELECT_ITEM_TO_TRY_ON = new SystemMessage(1387);

        /// <summary>
        /// ID: 1388
        /// <para/>
        /// Message: A party room has been created.
        /// </summary>
        public static SystemMessage PARTY_ROOM_CREATED = new SystemMessage(1388);

        /// <summary>
        /// ID: 1389
        /// <para/>
        /// Message: The party room's information has been revised.
        /// </summary>
        public static SystemMessage PARTY_ROOM_REVISED = new SystemMessage(1389);

        /// <summary>
        /// ID: 1390
        /// <para/>
        /// Message: You are not allowed to enter the party room.
        /// </summary>
        public static SystemMessage PARTY_ROOM_FORBIDDEN = new SystemMessage(1390);

        /// <summary>
        /// ID: 1391
        /// <para/>
        /// Message: You have exited from the party room.
        /// </summary>
        public static SystemMessage PARTY_ROOM_EXITED = new SystemMessage(1391);

        /// <summary>
        /// ID: 1392
        /// <para/>
        /// Message: $s1 has left the party room.
        /// </summary>
        public static SystemMessage S1_LEFT_PARTY_ROOM = new SystemMessage(1392);

        /// <summary>
        /// ID: 1393
        /// <para/>
        /// Message: You have been ousted from the party room.
        /// </summary>
        public static SystemMessage OUSTED_FROM_PARTY_ROOM = new SystemMessage(1393);

        /// <summary>
        /// ID: 1394
        /// <para/>
        /// Message: $s1 has been kicked from the party room.
        /// </summary>
        public static SystemMessage S1_KICKED_FROM_PARTY_ROOM = new SystemMessage(1394);

        /// <summary>
        /// ID: 1395
        /// <para/>
        /// Message: The party room has been disbanded.
        /// </summary>
        public static SystemMessage PARTY_ROOM_DISBANDED = new SystemMessage(1395);

        /// <summary>
        /// ID: 1396
        /// <para/>
        /// Message: The list of party rooms can only be viewed by a person who has not joined a party or who is currently the leader of a party.
        /// </summary>
        public static SystemMessage CANT_VIEW_PARTY_ROOMS = new SystemMessage(1396);

        /// <summary>
        /// ID: 1397
        /// <para/>
        /// Message: The leader of the party room has changed.
        /// </summary>
        public static SystemMessage PARTY_ROOM_LEADER_CHANGED = new SystemMessage(1397);

        /// <summary>
        /// ID: 1398
        /// <para/>
        /// Message: We are recruiting party members.
        /// </summary>
        public static SystemMessage RECRUITING_PARTY_MEMBERS = new SystemMessage(1398);

        /// <summary>
        /// ID: 1399
        /// <para/>
        /// Message: Only the leader of the party can transfer party leadership to another player.
        /// </summary>
        public static SystemMessage ONLY_A_PARTY_LEADER_CAN_TRANSFER_ONES_RIGHTS_TO_ANOTHER_PLAYER = new SystemMessage(1399);

        /// <summary>
        /// ID: 1400
        /// <para/>
        /// Message: Please select the person you wish to make the party leader.
        /// </summary>
        public static SystemMessage PLEASE_SELECT_THE_PERSON_TO_WHOM_YOU_WOULD_LIKE_TO_TRANSFER_THE_RIGHTS_OF_A_PARTY_LEADER = new SystemMessage(1400);

        /// <summary>
        /// ID: 1401
        /// <para/>
        /// Message: Slow down.you are already the party leader.
        /// </summary>
        public static SystemMessage YOU_CANNOT_TRANSFER_RIGHTS_TO_YOURSELF = new SystemMessage(1401);

        /// <summary>
        /// ID: 1402
        /// <para/>
        /// Message: You may only transfer party leadership to another member of the party.
        /// </summary>
        public static SystemMessage YOU_CAN_TRANSFER_RIGHTS_ONLY_TO_ANOTHER_PARTY_MEMBER = new SystemMessage(1402);

        /// <summary>
        /// ID: 1403
        /// <para/>
        /// Message: You have failed to transfer the party leadership.
        /// </summary>
        public static SystemMessage YOU_HAVE_FAILED_TO_TRANSFER_THE_PARTY_LEADER_RIGHTS = new SystemMessage(1403);

        /// <summary>
        /// ID: 1404
        /// <para/>
        /// Message: The owner of the private manufacturing store has changed the price for creating this item. Please check the new price before trying again.
        /// </summary>
        public static SystemMessage MANUFACTURE_PRICE_HAS_CHANGED = new SystemMessage(1404);

        /// <summary>
        /// ID: 1405
        /// <para/>
        /// Message: $s1 CPs have been restored.
        /// </summary>
        public static SystemMessage S1_CP_WILL_BE_RESTORED = new SystemMessage(1405);

        /// <summary>
        /// ID: 1406
        /// <para/>
        /// Message: $s2 CPs has been restored by $s1.
        /// </summary>
        public static SystemMessage S2_CP_WILL_BE_RESTORED_BY_S1 = new SystemMessage(1406);

        /// <summary>
        /// ID: 1407
        /// <para/>
        /// Message: You are using a computer that does not allow you to log in with two accounts at the same time.
        /// </summary>
        public static SystemMessage NO_LOGIN_WITH_TWO_ACCOUNTS = new SystemMessage(1407);

        /// <summary>
        /// ID: 1408
        /// <para/>
        /// Message: Your prepaid remaining usage time is $s1 hours and $s2 minutes. You have $s3 paid reservations left.
        /// </summary>
        public static SystemMessage PREPAID_LEFT_S1_S2_S3 = new SystemMessage(1408);

        /// <summary>
        /// ID: 1409
        /// <para/>
        /// Message: Your prepaid usage time has expired. Your new prepaid reservation will be used. The remaining usage time is $s1 hours and $s2 minutes.
        /// </summary>
        public static SystemMessage PREPAID_EXPIRED_S1_S2 = new SystemMessage(1409);

        /// <summary>
        /// ID: 1410
        /// <para/>
        /// Message: Your prepaid usage time has expired. You do not have any more prepaid reservations left.
        /// </summary>
        public static SystemMessage PREPAID_EXPIRED = new SystemMessage(1410);

        /// <summary>
        /// ID: 1411
        /// <para/>
        /// Message: The number of your prepaid reservations has changed.
        /// </summary>
        public static SystemMessage PREPAID_CHANGED = new SystemMessage(1411);

        /// <summary>
        /// ID: 1412
        /// <para/>
        /// Message: Your prepaid usage time has $s1 minutes left.
        /// </summary>
        public static SystemMessage PREPAID_LEFT_S1 = new SystemMessage(1412);

        /// <summary>
        /// ID: 1413
        /// <para/>
        /// Message: You do not meet the requirements to enter that party room.
        /// </summary>
        public static SystemMessage CANT_ENTER_PARTY_ROOM = new SystemMessage(1413);

        /// <summary>
        /// ID: 1414
        /// <para/>
        /// Message: The width and length should be 100 or more grids and less than 5000 grids respectively.
        /// </summary>
        public static SystemMessage WRONG_GRID_COUNT = new SystemMessage(1414);

        /// <summary>
        /// ID: 1415
        /// <para/>
        /// Message: The command file is not sent.
        /// </summary>
        public static SystemMessage COMMAND_FILE_NOT_SENT = new SystemMessage(1415);

        /// <summary>
        /// ID: 1416
        /// <para/>
        /// Message: The representative of Team 1 has not been selected.
        /// </summary>
        public static SystemMessage TEAM_1_NO_REPRESENTATIVE = new SystemMessage(1416);

        /// <summary>
        /// ID: 1417
        /// <para/>
        /// Message: The representative of Team 2 has not been selected.
        /// </summary>
        public static SystemMessage TEAM_2_NO_REPRESENTATIVE = new SystemMessage(1417);

        /// <summary>
        /// ID: 1418
        /// <para/>
        /// Message: The name of Team 1 has not yet been chosen.
        /// </summary>
        public static SystemMessage TEAM_1_NO_NAME = new SystemMessage(1418);

        /// <summary>
        /// ID: 1419
        /// <para/>
        /// Message: The name of Team 2 has not yet been chosen.
        /// </summary>
        public static SystemMessage TEAM_2_NO_NAME = new SystemMessage(1419);

        /// <summary>
        /// ID: 1420
        /// <para/>
        /// Message: The name of Team 1 and the name of Team 2 are identical.
        /// </summary>
        public static SystemMessage TEAM_NAME_IDENTICAL = new SystemMessage(1420);

        /// <summary>
        /// ID: 1421
        /// <para/>
        /// Message: The race setup file has not been designated.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE1 = new SystemMessage(1421);

        /// <summary>
        /// ID: 1422
        /// <para/>
        /// Message: Race setup file error - BuffCnt is not specified
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE2 = new SystemMessage(1422);

        /// <summary>
        /// ID: 1423
        /// <para/>
        /// Message: Race setup file error - BuffID$s1 is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE3 = new SystemMessage(1423);

        /// <summary>
        /// ID: 1424
        /// <para/>
        /// Message: Race setup file error - BuffLv$s1 is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE4 = new SystemMessage(1424);

        /// <summary>
        /// ID: 1425
        /// <para/>
        /// Message: Race setup file error - DefaultAllow is not specified
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE5 = new SystemMessage(1425);

        /// <summary>
        /// ID: 1426
        /// <para/>
        /// Message: Race setup file error - ExpSkillCnt is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE6 = new SystemMessage(1426);

        /// <summary>
        /// ID: 1427
        /// <para/>
        /// Message: Race setup file error - ExpSkillID$s1 is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE7 = new SystemMessage(1427);

        /// <summary>
        /// ID: 1428
        /// <para/>
        /// Message: Race setup file error - ExpItemCnt is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE8 = new SystemMessage(1428);

        /// <summary>
        /// ID: 1429
        /// <para/>
        /// Message: Race setup file error - ExpItemID$s1 is not specified.
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE9 = new SystemMessage(1429);

        /// <summary>
        /// ID: 1430
        /// <para/>
        /// Message: Race setup file error - TeleportDelay is not specified
        /// </summary>
        public static SystemMessage RACE_SETUP_FILE10 = new SystemMessage(1430);

        /// <summary>
        /// ID: 1431
        /// <para/>
        /// Message: The race will be stopped temporarily.
        /// </summary>
        public static SystemMessage RACE_STOPPED_TEMPORARILY = new SystemMessage(1431);

        /// <summary>
        /// ID: 1432
        /// <para/>
        /// Message: Your opponent is currently in a petrified state.
        /// </summary>
        public static SystemMessage OPPONENT_PETRIFIED = new SystemMessage(1432);

        /// <summary>
        /// ID: 1433
        /// <para/>
        /// Message: You will now automatically apply $s1 to your target.
        /// </summary>
        public static SystemMessage USE_OF_S1_WILL_BE_AUTO = new SystemMessage(1433);

        /// <summary>
        /// ID: 1434
        /// <para/>
        /// Message: You will no longer automatically apply $s1 to your weapon.
        /// </summary>
        public static SystemMessage AUTO_USE_OF_S1_CANCELLED = new SystemMessage(1434);

        /// <summary>
        /// ID: 1435
        /// <para/>
        /// Message: Due to insufficient $s1, the automatic use function has been deactivated.
        /// </summary>
        public static SystemMessage AUTO_USE_CANCELLED_LACK_OF_S1 = new SystemMessage(1435);

        /// <summary>
        /// ID: 1436
        /// <para/>
        /// Message: Due to insufficient $s1, the automatic use function cannot be activated.
        /// </summary>
        public static SystemMessage CANNOT_AUTO_USE_LACK_OF_S1 = new SystemMessage(1436);

        /// <summary>
        /// ID: 1437
        /// <para/>
        /// Message: Players are no longer allowed to play dice. Dice can no longer be purchased from a village store. However, you can still sell them to any village store.
        /// </summary>
        public static SystemMessage DICE_NO_LONGER_ALLOWED = new SystemMessage(1437);

        /// <summary>
        /// ID: 1438
        /// <para/>
        /// Message: There is no skill that enables enchant.
        /// </summary>
        public static SystemMessage THERE_IS_NO_SKILL_THAT_ENABLES_ENCHANT = new SystemMessage(1438);

        /// <summary>
        /// ID: 1439
        /// <para/>
        /// Message: You do not have all of the items needed to enchant that skill.
        /// </summary>
        public static SystemMessage YOU_DONT_HAVE_ALL_OF_THE_ITEMS_NEEDED_TO_ENCHANT_THAT_SKILL = new SystemMessage(1439);

        /// <summary>
        /// ID: 1440
        /// <para/>
        /// Message: You have succeeded in enchanting the skill $s1.
        /// </summary>
        public static SystemMessage YOU_HAVE_SUCCEEDED_IN_ENCHANTING_THE_SKILL_S1 = new SystemMessage(1440);

        /// <summary>
        /// ID: 1441
        /// <para/>
        /// Message: Skill enchant failed. The skill will be initialized.
        /// </summary>
        public static SystemMessage YOU_HAVE_FAILED_TO_ENCHANT_THE_SKILL_S1 = new SystemMessage(1441);

        /// <summary>
        /// ID: 1443
        /// <para/>
        /// Message: You do not have enough SP to enchant that skill.
        /// </summary>
        public static SystemMessage YOU_DONT_HAVE_ENOUGH_SP_TO_ENCHANT_THAT_SKILL = new SystemMessage(1443);

        /// <summary>
        /// ID: 1444
        /// <para/>
        /// Message: You do not have enough experience (Exp) to enchant that skill.
        /// </summary>
        public static SystemMessage YOU_DONT_HAVE_ENOUGH_EXP_TO_ENCHANT_THAT_SKILL = new SystemMessage(1444);

        /// <summary>
        /// ID: 1445
        /// <para/>
        /// Message: Your previous subclass will be removed and replaced with the new subclass at level 40. Do you wish to continue?
        /// </summary>
        public static SystemMessage REPLACE_SUBCLASS_CONFIRM = new SystemMessage(1445);

        /// <summary>
        /// ID: 1446
        /// <para/>
        /// Message: The ferry from $s1 to $s2 has been delayed.
        /// </summary>
        public static SystemMessage FERRY_FROM_S1_TO_S2_DELAYED = new SystemMessage(1446);

        /// <summary>
        /// ID: 1447
        /// <para/>
        /// Message: You cannot do that while fishing.
        /// </summary>
        public static SystemMessage CANNOT_DO_WHILE_FISHING_1 = new SystemMessage(1447);

        /// <summary>
        /// ID: 1448
        /// <para/>
        /// Message: Only fishing skills may be used at this time.
        /// </summary>
        public static SystemMessage ONLY_FISHING_SKILLS_NOW = new SystemMessage(1448);

        /// <summary>
        /// ID: 1449
        /// <para/>
        /// Message: You've got a bite!
        /// </summary>
        public static SystemMessage GOT_A_BITE = new SystemMessage(1449);

        /// <summary>
        /// ID: 1450
        /// <para/>
        /// Message: That fish is more determined than you are - it spit the hook!
        /// </summary>
        public static SystemMessage FISH_SPIT_THE_HOOK = new SystemMessage(1450);

        /// <summary>
        /// ID: 1451
        /// <para/>
        /// Message: Your bait was stolen by that fish!
        /// </summary>
        public static SystemMessage BAIT_STOLEN_BY_FISH = new SystemMessage(1451);

        /// <summary>
        /// ID: 1452
        /// <para/>
        /// Message: Baits have been lost because the fish got away.
        /// </summary>
        public static SystemMessage BAIT_LOST_FISH_GOT_AWAY = new SystemMessage(1452);

        /// <summary>
        /// ID: 1453
        /// <para/>
        /// Message: You do not have a fishing pole equipped.
        /// </summary>
        public static SystemMessage FISHING_POLE_NOT_EQUIPPED = new SystemMessage(1453);

        /// <summary>
        /// ID: 1454
        /// <para/>
        /// Message: You must put bait on your hook before you can fish.
        /// </summary>
        public static SystemMessage BAIT_ON_HOOK_BEFORE_FISHING = new SystemMessage(1454);

        /// <summary>
        /// ID: 1455
        /// <para/>
        /// Message: You cannot fish while under water.
        /// </summary>
        public static SystemMessage CANNOT_FISH_UNDER_WATER = new SystemMessage(1455);

        /// <summary>
        /// ID: 1456
        /// <para/>
        /// Message: You cannot fish while riding as a passenger of a boat - it's against the rules.
        /// </summary>
        public static SystemMessage CANNOT_FISH_ON_BOAT = new SystemMessage(1456);

        /// <summary>
        /// ID: 1457
        /// <para/>
        /// Message: You can't fish here.
        /// </summary>
        public static SystemMessage CANNOT_FISH_HERE = new SystemMessage(1457);

        /// <summary>
        /// ID: 1458
        /// <para/>
        /// Message: Your attempt at fishing has been cancelled.
        /// </summary>
        public static SystemMessage FISHING_ATTEMPT_CANCELLED = new SystemMessage(1458);

        /// <summary>
        /// ID: 1459
        /// <para/>
        /// Message: You do not have enough bait.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_BAIT = new SystemMessage(1459);

        /// <summary>
        /// ID: 1460
        /// <para/>
        /// Message: You reel your line in and stop fishing.
        /// </summary>
        public static SystemMessage REEL_LINE_AND_STOP_FISHING = new SystemMessage(1460);

        /// <summary>
        /// ID: 1461
        /// <para/>
        /// Message: You cast your line and start to fish.
        /// </summary>
        public static SystemMessage CAST_LINE_AND_START_FISHING = new SystemMessage(1461);

        /// <summary>
        /// ID: 1462
        /// <para/>
        /// Message: You may only use the Pumping skill while you are fishing.
        /// </summary>
        public static SystemMessage CAN_USE_PUMPING_ONLY_WHILE_FISHING = new SystemMessage(1462);

        /// <summary>
        /// ID: 1463
        /// <para/>
        /// Message: You may only use the Reeling skill while you are fishing.
        /// </summary>
        public static SystemMessage CAN_USE_REELING_ONLY_WHILE_FISHING = new SystemMessage(1463);

        /// <summary>
        /// ID: 1464
        /// <para/>
        /// Message: The fish has resisted your attempt to bring it in.
        /// </summary>
        public static SystemMessage FISH_RESISTED_ATTEMPT_TO_BRING_IT_IN = new SystemMessage(1464);

        /// <summary>
        /// ID: 1465
        /// <para/>
        /// Message: Your pumping is successful, causing $s1 damage.
        /// </summary>
        public static SystemMessage PUMPING_SUCCESFUL_S1_DAMAGE = new SystemMessage(1465);

        /// <summary>
        /// ID: 1466
        /// <para/>
        /// Message: You failed to do anything with the fish and it regains $s1 HP.
        /// </summary>
        public static SystemMessage FISH_RESISTED_PUMPING_S1_HP_REGAINED = new SystemMessage(1466);

        /// <summary>
        /// ID: 1467
        /// <para/>
        /// Message: You reel that fish in closer and cause $s1 damage.
        /// </summary>
        public static SystemMessage REELING_SUCCESFUL_S1_DAMAGE = new SystemMessage(1467);

        /// <summary>
        /// ID: 1468
        /// <para/>
        /// Message: You failed to reel that fish in further and it regains $s1 HP.
        /// </summary>
        public static SystemMessage FISH_RESISTED_REELING_S1_HP_REGAINED = new SystemMessage(1468);

        /// <summary>
        /// ID: 1469
        /// <para/>
        /// Message: You caught something!
        /// </summary>
        public static SystemMessage YOU_CAUGHT_SOMETHING = new SystemMessage(1469);

        /// <summary>
        /// ID: 1470
        /// <para/>
        /// Message: You cannot do that while fishing.
        /// </summary>
        public static SystemMessage CANNOT_DO_WHILE_FISHING_2 = new SystemMessage(1470);

        /// <summary>
        /// ID: 1471
        /// <para/>
        /// Message: You cannot do that while fishing.
        /// </summary>
        public static SystemMessage CANNOT_DO_WHILE_FISHING_3 = new SystemMessage(1471);

        /// <summary>
        /// ID: 1472
        /// <para/>
        /// Message: You look oddly at the fishing pole in disbelief and realize that you can't attack anything with this.
        /// </summary>
        public static SystemMessage CANNOT_ATTACK_WITH_FISHING_POLE = new SystemMessage(1472);

        /// <summary>
        /// ID: 1473
        /// <para/>
        /// Message: $s1 is not sufficient.
        /// </summary>
        public static SystemMessage S1_NOT_SUFFICIENT = new SystemMessage(1473);

        /// <summary>
        /// ID: 1474
        /// <para/>
        /// Message: $s1 is not available.
        /// </summary>
        public static SystemMessage S1_NOT_AVAILABLE = new SystemMessage(1474);

        /// <summary>
        /// ID: 1475
        /// <para/>
        /// Message: Pet has dropped $s1.
        /// </summary>
        public static SystemMessage PET_DROPPED_S1 = new SystemMessage(1475);

        /// <summary>
        /// ID: 1476
        /// <para/>
        /// Message: Pet has dropped +$s1 $s2.
        /// </summary>
        public static SystemMessage PET_DROPPED_S1_S2 = new SystemMessage(1476);

        /// <summary>
        /// ID: 1477
        /// <para/>
        /// Message: Pet has dropped $s2 of $s1.
        /// </summary>
        public static SystemMessage PET_DROPPED_S2_S1_S = new SystemMessage(1477);

        /// <summary>
        /// ID: 1478
        /// <para/>
        /// Message: You may only register a 64 x 64 pixel, 256-color BMP.
        /// </summary>
        public static SystemMessage ONLY_64_PIXEL_256_COLOR_BMP = new SystemMessage(1478);

        /// <summary>
        /// ID: 1479
        /// <para/>
        /// Message: That is the wrong grade of soulshot for that fishing pole.
        /// </summary>
        public static SystemMessage WRONG_FISHINGSHOT_GRADE = new SystemMessage(1479);

        /// <summary>
        /// ID: 1480
        /// <para/>
        /// Message: Are you sure you want to remove yourself from the Grand Olympiad Games waiting list?
        /// </summary>
        public static SystemMessage OLYMPIAD_REMOVE_CONFIRM = new SystemMessage(1480);

        /// <summary>
        /// ID: 1481
        /// <para/>
        /// Message: You have selected a class irrelevant individual match. Do you wish to participate?
        /// </summary>
        public static SystemMessage OLYMPIAD_NON_CLASS_CONFIRM = new SystemMessage(1481);

        /// <summary>
        /// ID: 1482
        /// <para/>
        /// Message: You've selected to join a class specific game. Continue?
        /// </summary>
        public static SystemMessage OLYMPIAD_CLASS_CONFIRM = new SystemMessage(1482);

        /// <summary>
        /// ID: 1483
        /// <para/>
        /// Message: Are you ready to be a Hero?
        /// </summary>
        public static SystemMessage HERO_CONFIRM = new SystemMessage(1483);

        /// <summary>
        /// ID: 1484
        /// <para/>
        /// Message: Are you sure this is the Hero weapon you wish to use? Kamael race cannot use this.
        /// </summary>
        public static SystemMessage HERO_WEAPON_CONFIRM = new SystemMessage(1484);

        /// <summary>
        /// ID: 1485
        /// <para/>
        /// Message: The ferry from Talking Island to Gludin Harbor has been delayed.
        /// </summary>
        public static SystemMessage FERRY_TALKING_GLUDIN_DELAYED = new SystemMessage(1485);

        /// <summary>
        /// ID: 1486
        /// <para/>
        /// Message: The ferry from Gludin Harbor to Talking Island has been delayed.
        /// </summary>
        public static SystemMessage FERRY_GLUDIN_TALKING_DELAYED = new SystemMessage(1486);

        /// <summary>
        /// ID: 1487
        /// <para/>
        /// Message: The ferry from Giran Harbor to Talking Island has been delayed.
        /// </summary>
        public static SystemMessage FERRY_GIRAN_TALKING_DELAYED = new SystemMessage(1487);

        /// <summary>
        /// ID: 1488
        /// <para/>
        /// Message: The ferry from Talking Island to Giran Harbor has been delayed.
        /// </summary>
        public static SystemMessage FERRY_TALKING_GIRAN_DELAYED = new SystemMessage(1488);

        /// <summary>
        /// ID: 1489
        /// <para/>
        /// Message: Innadril cruise service has been delayed.
        /// </summary>
        public static SystemMessage INNADRIL_BOAT_DELAYED = new SystemMessage(1489);

        /// <summary>
        /// ID: 1490
        /// <para/>
        /// Message: Traded $s2 of crop $s1.
        /// </summary>
        public static SystemMessage TRADED_S2_OF_CROP_S1 = new SystemMessage(1490);

        /// <summary>
        /// ID: 1491
        /// <para/>
        /// Message: Failed in trading $s2 of crop $s1.
        /// </summary>
        public static SystemMessage FAILED_IN_TRADING_S2_OF_CROP_S1 = new SystemMessage(1491);

        /// <summary>
        /// ID: 1492
        /// <para/>
        /// Message: You will be moved to the Olympiad Stadium in $s1 second(s).
        /// </summary>
        public static SystemMessage YOU_WILL_ENTER_THE_OLYMPIAD_STADIUM_IN_S1_SECOND_S = new SystemMessage(1492);

        /// <summary>
        /// ID: 1493
        /// <para/>
        /// Message: Your opponent made haste with their tail between their legs, the match has been cancelled.
        /// </summary>
        public static SystemMessage THE_GAME_HAS_BEEN_CANCELLED_BECAUSE_THE_OTHER_PARTY_ENDS_THE_GAME = new SystemMessage(1493);

        /// <summary>
        /// ID: 1494
        /// <para/>
        /// Message: Your opponent does not meet the requirements to do battle, the match has been cancelled.
        /// </summary>
        public static SystemMessage THE_GAME_HAS_BEEN_CANCELLED_BECAUSE_THE_OTHER_PARTY_DOES_NOT_MEET_THE_REQUIREMENTS_FOR_JOINING_THE_GAME = new SystemMessage(1494);

        /// <summary>
        /// ID: 1495
        /// <para/>
        /// Message: The match will start in $s1 second(s).
        /// </summary>
        public static SystemMessage THE_GAME_WILL_START_IN_S1_SECOND_S = new SystemMessage(1495);

        /// <summary>
        /// ID: 1496
        /// <para/>
        /// Message: The match has started, fight!
        /// </summary>
        public static SystemMessage STARTS_THE_GAME = new SystemMessage(1496);

        /// <summary>
        /// ID: 1497
        /// <para/>
        /// Message: Congratulations, $s1! You win the match!
        /// </summary>
        public static SystemMessage S1_HAS_WON_THE_GAME = new SystemMessage(1497);

        /// <summary>
        /// ID: 1498
        /// <para/>
        /// Message: There is no victor, the match ends in a tie.
        /// </summary>
        public static SystemMessage THE_GAME_ENDED_IN_A_TIE = new SystemMessage(1498);

        /// <summary>
        /// ID: 1499
        /// <para/>
        /// Message: You will be moved back to town in $s1 second(s).
        /// </summary>
        public static SystemMessage YOU_WILL_BE_MOVED_TO_TOWN_IN_S1_SECONDS = new SystemMessage(1499);

        /// <summary>
        /// ID: 1500
        /// <para/>
        /// Message: You cannot participate in the Grand Olympiad Games with a character in their subclass.
        /// </summary>
        public static SystemMessage YOU_CANT_JOIN_THE_OLYMPIAD_WITH_A_SUB_JOB_CHARACTER = new SystemMessage(1500);

        /// <summary>
        /// ID: 1501
        /// <para/>
        /// Message: Only Noblesse can participate in the Olympiad.
        /// </summary>
        public static SystemMessage ONLY_NOBLESS_CAN_PARTICIPATE_IN_THE_OLYMPIAD = new SystemMessage(1501);

        /// <summary>
        /// ID: 1502
        /// <para/>
        /// Message: You have already been registered in a waiting list of an event.
        /// </summary>
        public static SystemMessage YOU_HAVE_ALREADY_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_AN_EVENT = new SystemMessage(1502);

        /// <summary>
        /// ID: 1503
        /// <para/>
        /// Message: You have been registered in the Grand Olympiad Games waiting list for a class specific match.
        /// </summary>
        public static SystemMessage YOU_HAVE_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_CLASSIFIED_GAMES = new SystemMessage(1503);

        /// <summary>
        /// ID: 1504
        /// <para/>
        /// Message: You have registered on the waiting list for the non-class-limited individual match event.
        /// </summary>
        public static SystemMessage YOU_HAVE_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_NO_CLASS_GAMES = new SystemMessage(1504);

        /// <summary>
        /// ID: 1505
        /// <para/>
        /// Message: You have been removed from the Grand Olympiad Games waiting list.
        /// </summary>
        public static SystemMessage YOU_HAVE_BEEN_DELETED_FROM_THE_WAITING_LIST_OF_A_GAME = new SystemMessage(1505);

        /// <summary>
        /// ID: 1506
        /// <para/>
        /// Message: You are not currently registered on any Grand Olympiad Games waiting list.
        /// </summary>
        public static SystemMessage YOU_HAVE_NOT_BEEN_REGISTERED_IN_A_WAITING_LIST_OF_A_GAME = new SystemMessage(1506);

        /// <summary>
        /// ID: 1507
        /// <para/>
        /// Message: You cannot equip that item in a Grand Olympiad Games match.
        /// </summary>
        public static SystemMessage THIS_ITEM_CANT_BE_EQUIPPED_FOR_THE_OLYMPIAD_EVENT = new SystemMessage(1507);

        /// <summary>
        /// ID: 1508
        /// <para/>
        /// Message: You cannot use that item in a Grand Olympiad Games match.
        /// </summary>
        public static SystemMessage THIS_ITEM_IS_NOT_AVAILABLE_FOR_THE_OLYMPIAD_EVENT = new SystemMessage(1508);

        /// <summary>
        /// ID: 1509
        /// <para/>
        /// Message: You cannot use that skill in a Grand Olympiad Games match.
        /// </summary>
        public static SystemMessage THIS_SKILL_IS_NOT_AVAILABLE_FOR_THE_OLYMPIAD_EVENT = new SystemMessage(1509);

        /// <summary>
        /// ID: 1510
        /// <para/>
        /// Message: $s1 is making an attempt at resurrection. Do you want to continue with this resurrection?
        /// </summary>
        public static SystemMessage RESSURECTION_REQUEST_BY_S1 = new SystemMessage(1510);

        /// <summary>
        /// ID: 1511
        /// <para/>
        /// Message: While a pet is attempting to resurrect, it cannot help in resurrecting its master.
        /// </summary>
        public static SystemMessage MASTER_CANNOT_RES = new SystemMessage(1511);

        /// <summary>
        /// ID: 1512
        /// <para/>
        /// Message: You cannot resurrect a pet while their owner is being resurrected.
        /// </summary>
        public static SystemMessage CANNOT_RES_PET = new SystemMessage(1512);

        /// <summary>
        /// ID: 1513
        /// <para/>
        /// Message: Resurrection has already been proposed.
        /// </summary>
        public static SystemMessage RES_HAS_ALREADY_BEEN_PROPOSED = new SystemMessage(1513);

        /// <summary>
        /// ID: 1514
        /// <para/>
        /// Message: You cannot the owner of a pet while their pet is being resurrected
        /// </summary>
        public static SystemMessage CANNOT_RES_MASTER = new SystemMessage(1514);

        /// <summary>
        /// ID: 1515
        /// <para/>
        /// Message: A pet cannot be resurrected while it's owner is in the process of resurrecting.
        /// </summary>
        public static SystemMessage CANNOT_RES_PET2 = new SystemMessage(1515);

        /// <summary>
        /// ID: 1516
        /// <para/>
        /// Message: The target is unavailable for seeding.
        /// </summary>
        public static SystemMessage THE_TARGET_IS_UNAVAILABLE_FOR_SEEDING = new SystemMessage(1516);

        /// <summary>
        /// ID: 1517
        /// <para/>
        /// Message: Failed in Blessed Enchant. The enchant value of the item became 0.
        /// </summary>
        public static SystemMessage BLESSED_ENCHANT_FAILED = new SystemMessage(1517);

        /// <summary>
        /// ID: 1518
        /// <para/>
        /// Message: You do not meet the required condition to equip that item.
        /// </summary>
        public static SystemMessage CANNOT_EQUIP_ITEM_DUE_TO_BAD_CONDITION = new SystemMessage(1518);

        /// <summary>
        /// ID: 1519
        /// <para/>
        /// Message: Your pet has been killed! Make sure you resurrect your pet within 20 minutes or your pet and all of it's items will disappear forever!
        /// </summary>
        public static SystemMessage MAKE_SURE_YOU_RESSURECT_YOUR_PET_WITHIN_20_MINUTES = new SystemMessage(1519);

        /// <summary>
        /// ID: 1520
        /// <para/>
        /// Message: Servitor passed away.
        /// </summary>
        public static SystemMessage SERVITOR_PASSED_AWAY = new SystemMessage(1520);

        /// <summary>
        /// ID: 1521
        /// <para/>
        /// Message: Your servitor has vanished! You'll need to summon a new one.
        /// </summary>
        public static SystemMessage YOUR_SERVITOR_HAS_VANISHED = new SystemMessage(1521);

        /// <summary>
        /// ID: 1522
        /// <para/>
        /// Message: Your pet's corpse has decayed!
        /// </summary>
        public static SystemMessage YOUR_PETS_CORPSE_HAS_DECAYED = new SystemMessage(1522);

        /// <summary>
        /// ID: 1523
        /// <para/>
        /// Message: You should release your pet or servitor so that it does not fall off of the boat and drown!
        /// </summary>
        public static SystemMessage RELEASE_PET_ON_BOAT = new SystemMessage(1523);

        /// <summary>
        /// ID: 1524
        /// <para/>
        /// Message: $s1's pet gained $s2.
        /// </summary>
        public static SystemMessage S1_PET_GAINED_S2 = new SystemMessage(1524);

        /// <summary>
        /// ID: 1525
        /// <para/>
        /// Message: $s1's pet gained $s3 of $s2.
        /// </summary>
        public static SystemMessage S1_PET_GAINED_S3_S2_S = new SystemMessage(1525);

        /// <summary>
        /// ID: 1526
        /// <para/>
        /// Message: $s1's pet gained +$s2$s3.
        /// </summary>
        public static SystemMessage S1_PET_GAINED_S2_S3 = new SystemMessage(1526);

        /// <summary>
        /// ID: 1527
        /// <para/>
        /// Message: Your pet was hungry so it ate $s1.
        /// </summary>
        public static SystemMessage PET_TOOK_S1_BECAUSE_HE_WAS_HUNGRY = new SystemMessage(1527);

        /// <summary>
        /// ID: 1528
        /// <para/>
        /// Message: You've sent a petition to the GM staff.
        /// </summary>
        public static SystemMessage SENT_PETITION_TO_GM = new SystemMessage(1528);

        /// <summary>
        /// ID: 1529
        /// <para/>
        /// Message: $s1 is inviting you to the command channel. Do you want accept?
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_CONFIRM_FROM_S1 = new SystemMessage(1529);

        /// <summary>
        /// ID: 1530
        /// <para/>
        /// Message: Select a target or enter the name.
        /// </summary>
        public static SystemMessage SELECT_TARGET_OR_ENTER_NAME = new SystemMessage(1530);

        /// <summary>
        /// ID: 1531
        /// <para/>
        /// Message: Enter the name of the clan that you wish to declare war on.
        /// </summary>
        public static SystemMessage ENTER_CLAN_NAME_TO_DECLARE_WAR2 = new SystemMessage(1531);

        /// <summary>
        /// ID: 1532
        /// <para/>
        /// Message: Enter the name of the clan that you wish to have a cease-fire with.
        /// </summary>
        public static SystemMessage ENTER_CLAN_NAME_TO_CEASE_FIRE = new SystemMessage(1532);

        /// <summary>
        /// ID: 1533
        /// <para/>
        /// Message: Attention: $s1 has picked up $s2.
        /// </summary>
        public static SystemMessage ATTENTION_S1_PICKED_UP_S2 = new SystemMessage(1533);

        /// <summary>
        /// ID: 1534
        /// <para/>
        /// Message: Attention: $s1 has picked up +$s2$s3.
        /// </summary>
        public static SystemMessage ATTENTION_S1_PICKED_UP_S2_S3 = new SystemMessage(1534);

        /// <summary>
        /// ID: 1535
        /// <para/>
        /// Message: Attention: $s1's pet has picked up $s2.
        /// </summary>
        public static SystemMessage ATTENTION_S1_PET_PICKED_UP_S2 = new SystemMessage(1535);

        /// <summary>
        /// ID: 1536
        /// <para/>
        /// Message: Attention: $s1's pet has picked up +$s2$s3.
        /// </summary>
        public static SystemMessage ATTENTION_S1_PET_PICKED_UP_S2_S3 = new SystemMessage(1536);

        /// <summary>
        /// ID: 1537
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (near Rune Village)
        /// </summary>
        public static SystemMessage LOC_RUNE_S1_S2_S3 = new SystemMessage(1537);

        /// <summary>
        /// ID: 1538
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (near the Town of Goddard)
        /// </summary>
        public static SystemMessage LOC_GODDARD_S1_S2_S3 = new SystemMessage(1538);

        /// <summary>
        /// ID: 1539
        /// <para/>
        /// Message: Cargo has arrived at Talking Island Village.
        /// </summary>
        public static SystemMessage CARGO_AT_TALKING_VILLAGE = new SystemMessage(1539);

        /// <summary>
        /// ID: 1540
        /// <para/>
        /// Message: Cargo has arrived at the Dark Elf Village.
        /// </summary>
        public static SystemMessage CARGO_AT_DARKELF_VILLAGE = new SystemMessage(1540);

        /// <summary>
        /// ID: 1541
        /// <para/>
        /// Message: Cargo has arrived at Elven Village.
        /// </summary>
        public static SystemMessage CARGO_AT_ELVEN_VILLAGE = new SystemMessage(1541);

        /// <summary>
        /// ID: 1542
        /// <para/>
        /// Message: Cargo has arrived at Orc Village.
        /// </summary>
        public static SystemMessage CARGO_AT_ORC_VILLAGE = new SystemMessage(1542);

        /// <summary>
        /// ID: 1543
        /// <para/>
        /// Message: Cargo has arrived at Dwarfen Village.
        /// </summary>
        public static SystemMessage CARGO_AT_DWARVEN_VILLAGE = new SystemMessage(1543);

        /// <summary>
        /// ID: 1544
        /// <para/>
        /// Message: Cargo has arrived at Aden Castle Town.
        /// </summary>
        public static SystemMessage CARGO_AT_ADEN = new SystemMessage(1544);

        /// <summary>
        /// ID: 1545
        /// <para/>
        /// Message: Cargo has arrived at Town of Oren.
        /// </summary>
        public static SystemMessage CARGO_AT_OREN = new SystemMessage(1545);

        /// <summary>
        /// ID: 1546
        /// <para/>
        /// Message: Cargo has arrived at Hunters Village.
        /// </summary>
        public static SystemMessage CARGO_AT_HUNTERS = new SystemMessage(1546);

        /// <summary>
        /// ID: 1547
        /// <para/>
        /// Message: Cargo has arrived at the Town of Dion.
        /// </summary>
        public static SystemMessage CARGO_AT_DION = new SystemMessage(1547);

        /// <summary>
        /// ID: 1548
        /// <para/>
        /// Message: Cargo has arrived at Floran Village.
        /// </summary>
        public static SystemMessage CARGO_AT_FLORAN = new SystemMessage(1548);

        /// <summary>
        /// ID: 1549
        /// <para/>
        /// Message: Cargo has arrived at Gludin Village.
        /// </summary>
        public static SystemMessage CARGO_AT_GLUDIN = new SystemMessage(1549);

        /// <summary>
        /// ID: 1550
        /// <para/>
        /// Message: Cargo has arrived at the Town of Gludio.
        /// </summary>
        public static SystemMessage CARGO_AT_GLUDIO = new SystemMessage(1550);

        /// <summary>
        /// ID: 1551
        /// <para/>
        /// Message: Cargo has arrived at Giran Castle Town.
        /// </summary>
        public static SystemMessage CARGO_AT_GIRAN = new SystemMessage(1551);

        /// <summary>
        /// ID: 1552
        /// <para/>
        /// Message: Cargo has arrived at Heine.
        /// </summary>
        public static SystemMessage CARGO_AT_HEINE = new SystemMessage(1552);

        /// <summary>
        /// ID: 1553
        /// <para/>
        /// Message: Cargo has arrived at Rune Village.
        /// </summary>
        public static SystemMessage CARGO_AT_RUNE = new SystemMessage(1553);

        /// <summary>
        /// ID: 1554
        /// <para/>
        /// Message: Cargo has arrived at the Town of Goddard.
        /// </summary>
        public static SystemMessage CARGO_AT_GODDARD = new SystemMessage(1554);

        /// <summary>
        /// ID: 1555
        /// <para/>
        /// Message: Do you want to cancel character deletion?
        /// </summary>
        public static SystemMessage CANCEL_CHARACTER_DELETION_CONFIRM = new SystemMessage(1555);

        /// <summary>
        /// ID: 1556
        /// <para/>
        /// Message: Your clan notice has been saved.
        /// </summary>
        public static SystemMessage CLAN_NOTICE_SAVED = new SystemMessage(1556);

        /// <summary>
        /// ID: 1557
        /// <para/>
        /// Message: Seed price should be more than $s1 and less than $s2.
        /// </summary>
        public static SystemMessage SEED_PRICE_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = new SystemMessage(1557);

        /// <summary>
        /// ID: 1558
        /// <para/>
        /// Message: The quantity of seed should be more than $s1 and less than $s2.
        /// </summary>
        public static SystemMessage THE_QUANTITY_OF_SEED_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = new SystemMessage(1558);

        /// <summary>
        /// ID: 1559
        /// <para/>
        /// Message: Crop price should be more than $s1 and less than $s2.
        /// </summary>
        public static SystemMessage CROP_PRICE_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = new SystemMessage(1559);

        /// <summary>
        /// ID: 1560
        /// <para/>
        /// Message: The quantity of crop should be more than $s1 and less than $s2
        /// </summary>
        public static SystemMessage THE_QUANTITY_OF_CROP_SHOULD_BE_MORE_THAN_S1_AND_LESS_THAN_S2 = new SystemMessage(1560);

        /// <summary>
        /// ID: 1561
        /// <para/>
        /// Message: The clan, $s1, has declared a Clan War.
        /// </summary>
        public static SystemMessage CLAN_S1_DECLARED_WAR = new SystemMessage(1561);

        /// <summary>
        /// ID: 1562
        /// <para/>
        /// Message: A Clan War has been declared against the clan, $s1. you will only lose a quarter of the normal experience from death.
        /// </summary>
        public static SystemMessage CLAN_WAR_DECLARED_AGAINST_S1_IF_KILLED_LOSE_LOW_EXP = new SystemMessage(1562);

        /// <summary>
        /// ID: 1563
        /// <para/>
        /// Message: The clan, $s1, cannot declare a Clan War because their clan is less than level three, and or they do not have enough members.
        /// </summary>
        public static SystemMessage S1_CLAN_CANNOT_DECLARE_WAR_TOO_LOW_LEVEL_OR_NOT_ENOUGH_MEMBERS = new SystemMessage(1563);

        /// <summary>
        /// ID: 1564
        /// <para/>
        /// Message: A Clan War can be declared only if the clan is level three or above, and the number of clan members is fifteen or greater.
        /// </summary>
        public static SystemMessage CLAN_WAR_DECLARED_IF_CLAN_LVL3_OR_15_MEMBER = new SystemMessage(1564);

        /// <summary>
        /// ID: 1565
        /// <para/>
        /// Message: A Clan War cannot be declared against a clan that does not exist!
        /// </summary>
        public static SystemMessage CLAN_WAR_CANNOT_DECLARED_CLAN_NOT_EXIST = new SystemMessage(1565);

        /// <summary>
        /// ID: 1566
        /// <para/>
        /// Message: The clan, $s1, has decided to stop the war.
        /// </summary>
        public static SystemMessage CLAN_S1_HAS_DECIDED_TO_STOP = new SystemMessage(1566);

        /// <summary>
        /// ID: 1567
        /// <para/>
        /// Message: The war against $s1 Clan has been stopped.
        /// </summary>
        public static SystemMessage WAR_AGAINST_S1_HAS_STOPPED = new SystemMessage(1567);

        /// <summary>
        /// ID: 1568
        /// <para/>
        /// Message: The target for declaration is wrong.
        /// </summary>
        public static SystemMessage WRONG_DECLARATION_TARGET = new SystemMessage(1568);

        /// <summary>
        /// ID: 1569
        /// <para/>
        /// Message: A declaration of Clan War against an allied clan can't be made.
        /// </summary>
        public static SystemMessage CLAN_WAR_AGAINST_A_ALLIED_CLAN_NOT_WORK = new SystemMessage(1569);

        /// <summary>
        /// ID: 1570
        /// <para/>
        /// Message: A declaration of war against more than 30 Clans can't be made at the same time
        /// </summary>
        public static SystemMessage TOO_MANY_CLAN_WARS = new SystemMessage(1570);

        /// <summary>
        /// ID: 1571
        /// <para/>
        /// Message: ======<Clans You've Declared War On>======
        /// </summary>
        public static SystemMessage CLANS_YOU_DECLARED_WAR_ON = new SystemMessage(1571);

        /// <summary>
        /// ID: 1572
        /// <para/>
        /// Message: ======<Clans That Have Declared War On You>======
        /// </summary>
        public static SystemMessage CLANS_THAT_HAVE_DECLARED_WAR_ON_YOU = new SystemMessage(1572);

        /// <summary>
        /// ID: 1573
        /// <para/>
        /// Message: There are no clans that your clan has declared war against.
        /// </summary>
        public static SystemMessage YOU_ARENT_IN_CLAN_WARS = new SystemMessage(1573);

        /// <summary>
        /// ID: 1574
        /// <para/>
        /// Message: All is well. There are no clans that have declared war against your clan.
        /// </summary>
        public static SystemMessage NO_CLAN_WARS_VS_YOU = new SystemMessage(1574);

        /// <summary>
        /// ID: 1575
        /// <para/>
        /// Message: Command Channels can only be formed by a party leader who is also the leader of a level 5 clan.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_ONLY_BY_LEVEL_5_CLAN_LEADER_PARTY_LEADER = new SystemMessage(1575);

        /// <summary>
        /// ID: 1576
        /// <para/>
        /// Message: Pet uses the power of spirit.
        /// </summary>
        public static SystemMessage PET_USE_THE_POWER_OF_SPIRIT = new SystemMessage(1576);

        /// <summary>
        /// ID: 1577
        /// <para/>
        /// Message: Servitor uses the power of spirit.
        /// </summary>
        public static SystemMessage SERVITOR_USE_THE_POWER_OF_SPIRIT = new SystemMessage(1577);

        /// <summary>
        /// ID: 1578
        /// <para/>
        /// Message: Items are not available for a private store or a private manufacture.
        /// </summary>
        public static SystemMessage ITEMS_UNAVAILABLE_FOR_STORE_MANUFACTURE = new SystemMessage(1578);

        /// <summary>
        /// ID: 1579
        /// <para/>
        /// Message: $s1's pet gained $s2 adena.
        /// </summary>
        public static SystemMessage S1_PET_GAINED_S2_ADENA = new SystemMessage(1579);

        /// <summary>
        /// ID: 1580
        /// <para/>
        /// Message: The Command Channel has been formed.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_FORMED = new SystemMessage(1580);

        /// <summary>
        /// ID: 1581
        /// <para/>
        /// Message: The Command Channel has been disbanded.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_DISBANDED = new SystemMessage(1581);

        /// <summary>
        /// ID: 1582
        /// <para/>
        /// Message: You have joined the Command Channel.
        /// </summary>
        public static SystemMessage JOINED_COMMAND_CHANNEL = new SystemMessage(1582);

        /// <summary>
        /// ID: 1583
        /// <para/>
        /// Message: You were dismissed from the Command Channel.
        /// </summary>
        public static SystemMessage DISMISSED_FROM_COMMAND_CHANNEL = new SystemMessage(1583);

        /// <summary>
        /// ID: 1584
        /// <para/>
        /// Message: $s1's party has been dismissed from the Command Channel.
        /// </summary>
        public static SystemMessage S1_PARTY_DISMISSED_FROM_COMMAND_CHANNEL = new SystemMessage(1584);

        /// <summary>
        /// ID: 1585
        /// <para/>
        /// Message: The Command Channel has been disbanded.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_DISBANDED2 = new SystemMessage(1585);

        /// <summary>
        /// ID: 1586
        /// <para/>
        /// Message: You have quit the Command Channel.
        /// </summary>
        public static SystemMessage LEFT_COMMAND_CHANNEL = new SystemMessage(1586);

        /// <summary>
        /// ID: 1587
        /// <para/>
        /// Message: $s1's party has left the Command Channel.
        /// </summary>
        public static SystemMessage S1_PARTY_LEFT_COMMAND_CHANNEL = new SystemMessage(1587);

        /// <summary>
        /// ID: 1588
        /// <para/>
        /// Message: The Command Channel is activated only when there are at least 5 parties participating.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_ONLY_AT_LEAST_5_PARTIES = new SystemMessage(1588);

        /// <summary>
        /// ID: 1589
        /// <para/>
        /// Message: Command Channel authority has been transferred to $s1.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_LEADER_NOW_S1 = new SystemMessage(1589);

        /// <summary>
        /// ID: 1590
        /// <para/>
        /// Message: ===<Guild Info (Total Parties: $s1)>===
        /// </summary>
        public static SystemMessage GUILD_INFO_HEADER = new SystemMessage(1590);

        /// <summary>
        /// ID: 1591
        /// <para/>
        /// Message: No user has been invited to the Command Channel.
        /// </summary>
        public static SystemMessage NO_USER_INVITED_TO_COMMAND_CHANNEL = new SystemMessage(1591);

        /// <summary>
        /// ID: 1592
        /// <para/>
        /// Message: You can no longer set up a Command Channel.
        /// </summary>
        public static SystemMessage CANNOT_LONGER_SETUP_COMMAND_CHANNEL = new SystemMessage(1592);

        /// <summary>
        /// ID: 1593
        /// <para/>
        /// Message: You do not have authority to invite someone to the Command Channel.
        /// </summary>
        public static SystemMessage CANNOT_INVITE_TO_COMMAND_CHANNEL = new SystemMessage(1593);

        /// <summary>
        /// ID: 1594
        /// <para/>
        /// Message: $s1's party is already a member of the Command Channel.
        /// </summary>
        public static SystemMessage S1_ALREADY_MEMBER_OF_COMMAND_CHANNEL = new SystemMessage(1594);

        /// <summary>
        /// ID: 1595
        /// <para/>
        /// Message: $s1 has succeeded.
        /// </summary>
        public static SystemMessage S1_SUCCEEDED = new SystemMessage(1595);

        /// <summary>
        /// ID: 1596
        /// <para/>
        /// Message: You were hit by $s1!
        /// </summary>
        public static SystemMessage HIT_BY_S1 = new SystemMessage(1596);

        /// <summary>
        /// ID: 1597
        /// <para/>
        /// Message: $s1 has failed.
        /// </summary>
        public static SystemMessage S1_FAILED = new SystemMessage(1597);

        /// <summary>
        /// ID: 1598
        /// <para/>
        /// Message: Soulshots and spiritshots are not available for a dead pet or servitor. Sad, isn't it?
        /// </summary>
        public static SystemMessage SOULSHOTS_AND_SPIRITSHOTS_ARE_NOT_AVAILABLE_FOR_A_DEAD_PET = new SystemMessage(1598);

        /// <summary>
        /// ID: 1599
        /// <para/>
        /// Message: You cannot observe while you are in combat!
        /// </summary>
        public static SystemMessage CANNOT_OBSERVE_IN_COMBAT = new SystemMessage(1599);

        /// <summary>
        /// ID: 1600
        /// <para/>
        /// Message: Tomorrow's items will ALL be set to 0. Do you wish to continue?
        /// </summary>
        public static SystemMessage TOMORROW_ITEM_ZERO_CONFIRM = new SystemMessage(1600);

        /// <summary>
        /// ID: 1601
        /// <para/>
        /// Message: Tomorrow's items will all be set to the same value as today's items. Do you wish to continue?
        /// </summary>
        public static SystemMessage TOMORROW_ITEM_SAME_CONFIRM = new SystemMessage(1601);

        /// <summary>
        /// ID: 1602
        /// <para/>
        /// Message: Only a party leader can access the Command Channel.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_ONLY_FOR_PARTY_LEADER = new SystemMessage(1602);

        /// <summary>
        /// ID: 1603
        /// <para/>
        /// Message: Only channel operator can give All Command.
        /// </summary>
        public static SystemMessage ONLY_COMMANDER_GIVE_COMMAND = new SystemMessage(1603);

        /// <summary>
        /// ID: 1604
        /// <para/>
        /// Message: While dressed in formal wear, you can't use items that require all skills and casting operations.
        /// </summary>
        public static SystemMessage CANNOT_USE_ITEMS_SKILLS_WITH_FORMALWEAR = new SystemMessage(1604);

        /// <summary>
        /// ID: 1605
        /// <para/>
        /// Message: * Here, you can buy only seeds of $s1 Manor.
        /// </summary>
        public static SystemMessage HERE_YOU_CAN_BUY_ONLY_SEEDS_OF_S1_MANOR = new SystemMessage(1605);

        /// <summary>
        /// ID: 1606
        /// <para/>
        /// Message: Congratulations - You've completed the third-class transfer quest!
        /// </summary>
        public static SystemMessage THIRD_CLASS_TRANSFER = new SystemMessage(1606);

        /// <summary>
        /// ID: 1607
        /// <para/>
        /// Message: $s1 adena has been withdrawn to pay for purchasing fees.
        /// </summary>
        public static SystemMessage S1_ADENA_HAS_BEEN_WITHDRAWN_TO_PAY_FOR_PURCHASING_FEES = new SystemMessage(1607);

        /// <summary>
        /// ID: 1608
        /// <para/>
        /// Message: Due to insufficient adena you cannot buy another castle.
        /// </summary>
        public static SystemMessage INSUFFICIENT_ADENA_TO_BUY_CASTLE = new SystemMessage(1608);

        /// <summary>
        /// ID: 1609
        /// <para/>
        /// Message: War has already been declared against that clan... but I'll make note that you really don't like them.
        /// </summary>
        public static SystemMessage WAR_ALREADY_DECLARED = new SystemMessage(1609);

        /// <summary>
        /// ID: 1610
        /// <para/>
        /// Message: Fool! You cannot declare war against your own clan!
        /// </summary>
        public static SystemMessage CANNOT_DECLARE_AGAINST_OWN_CLAN = new SystemMessage(1610);

        /// <summary>
        /// ID: 1611
        /// <para/>
        /// Message: Leader: $s1
        /// </summary>
        public static SystemMessage PARTY_LEADER_S1 = new SystemMessage(1611);

        /// <summary>
        /// ID: 1612
        /// <para/>
        /// Message: =====<War List>=====
        /// </summary>
        public static SystemMessage WAR_LIST = new SystemMessage(1612);

        /// <summary>
        /// ID: 1613
        /// <para/>
        /// Message: There is no clan listed on War List.
        /// </summary>
        public static SystemMessage NO_CLAN_ON_WAR_LIST = new SystemMessage(1613);

        /// <summary>
        /// ID: 1614
        /// <para/>
        /// Message: You have joined a channel that was already open.
        /// </summary>
        public static SystemMessage JOINED_CHANNEL_ALREADY_OPEN = new SystemMessage(1614);

        /// <summary>
        /// ID: 1615
        /// <para/>
        /// Message: The number of remaining parties is $s1 until a channel is activated
        /// </summary>
        public static SystemMessage S1_PARTIES_REMAINING_UNTIL_CHANNEL = new SystemMessage(1615);

        /// <summary>
        /// ID: 1616
        /// <para/>
        /// Message: The Command Channel has been activated.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_ACTIVATED = new SystemMessage(1616);

        /// <summary>
        /// ID: 1617
        /// <para/>
        /// Message: You do not have the authority to use the Command Channel.
        /// </summary>
        public static SystemMessage CANT_USE_COMMAND_CHANNEL = new SystemMessage(1617);

        /// <summary>
        /// ID: 1618
        /// <para/>
        /// Message: The ferry from Rune Harbor to Gludin Harbor has been delayed.
        /// </summary>
        public static SystemMessage FERRY_RUNE_GLUDIN_DELAYED = new SystemMessage(1618);

        /// <summary>
        /// ID: 1619
        /// <para/>
        /// Message: The ferry from Gludin Harbor to Rune Harbor has been delayed.
        /// </summary>
        public static SystemMessage FERRY_GLUDIN_RUNE_DELAYED = new SystemMessage(1619);

        /// <summary>
        /// ID: 1620
        /// <para/>
        /// Message: Arrived at Rune Harbor.
        /// </summary>
        public static SystemMessage ARRIVED_AT_RUNE = new SystemMessage(1620);

        /// <summary>
        /// ID: 1621
        /// <para/>
        /// Message: Departure for Gludin Harbor will take place in five minutes!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_GLUDIN_5_MINUTES = new SystemMessage(1621);

        /// <summary>
        /// ID: 1622
        /// <para/>
        /// Message: Departure for Gludin Harbor will take place in one minute!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_GLUDIN_1_MINUTE = new SystemMessage(1622);

        /// <summary>
        /// ID: 1623
        /// <para/>
        /// Message: Make haste! We will be departing for Gludin Harbor shortly...
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_GLUDIN_SHORTLY = new SystemMessage(1623);

        /// <summary>
        /// ID: 1624
        /// <para/>
        /// Message: We are now departing for Gludin Harbor Hold on and enjoy the ride!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_GLUDIN_NOW = new SystemMessage(1624);

        /// <summary>
        /// ID: 1625
        /// <para/>
        /// Message: Departure for Rune Harbor will take place after anchoring for ten minutes.
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_RUNE_10_MINUTES = new SystemMessage(1625);

        /// <summary>
        /// ID: 1626
        /// <para/>
        /// Message: Departure for Rune Harbor will take place in five minutes!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_RUNE_5_MINUTES = new SystemMessage(1626);

        /// <summary>
        /// ID: 1627
        /// <para/>
        /// Message: Departure for Rune Harbor will take place in one minute!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_RUNE_1_MINUTE = new SystemMessage(1627);

        /// <summary>
        /// ID: 1628
        /// <para/>
        /// Message: Make haste! We will be departing for Gludin Harbor shortly...
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_GLUDIN_SHORTLY2 = new SystemMessage(1628);

        /// <summary>
        /// ID: 1629
        /// <para/>
        /// Message: We are now departing for Rune Harbor Hold on and enjoy the ride!
        /// </summary>
        public static SystemMessage DEPARTURE_FOR_RUNE_NOW = new SystemMessage(1629);

        /// <summary>
        /// ID: 1630
        /// <para/>
        /// Message: The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 15 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_RUNE_AT_GLUDIN_15_MINUTES = new SystemMessage(1630);

        /// <summary>
        /// ID: 1631
        /// <para/>
        /// Message: The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_RUNE_AT_GLUDIN_10_MINUTES = new SystemMessage(1631);

        /// <summary>
        /// ID: 1632
        /// <para/>
        /// Message: The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_RUNE_AT_GLUDIN_5_MINUTES = new SystemMessage(1632);

        /// <summary>
        /// ID: 1633
        /// <para/>
        /// Message: The ferry from Rune Harbor will be arriving at Gludin Harbor in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_RUNE_AT_GLUDIN_1_MINUTE = new SystemMessage(1633);

        /// <summary>
        /// ID: 1634
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 15 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_AT_RUNE_15_MINUTES = new SystemMessage(1634);

        /// <summary>
        /// ID: 1635
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Rune harbor in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_AT_RUNE_10_MINUTES = new SystemMessage(1635);

        /// <summary>
        /// ID: 1636
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 10 minutes.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_AT_RUNE_5_MINUTES = new SystemMessage(1636);

        /// <summary>
        /// ID: 1637
        /// <para/>
        /// Message: The ferry from Gludin Harbor will be arriving at Rune Harbor in approximately 1 minute.
        /// </summary>
        public static SystemMessage FERRY_FROM_GLUDIN_AT_RUNE_1_MINUTE = new SystemMessage(1637);

        /// <summary>
        /// ID: 1638
        /// <para/>
        /// Message: You cannot fish while using a recipe book, private manufacture or private store.
        /// </summary>
        public static SystemMessage CANNOT_FISH_WHILE_USING_RECIPE_BOOK = new SystemMessage(1638);

        /// <summary>
        /// ID: 1639
        /// <para/>
        /// Message: Period $s1 of the Grand Olympiad Games has started!
        /// </summary>
        public static SystemMessage OLYMPIAD_PERIOD_S1_HAS_STARTED = new SystemMessage(1639);

        /// <summary>
        /// ID: 1640
        /// <para/>
        /// Message: Period $s1 of the Grand Olympiad Games has now ended.
        /// </summary>
        public static SystemMessage OLYMPIAD_PERIOD_S1_HAS_ENDED = new SystemMessage(1640);

        /// <summary>
        /// ID: 1641
        /// <para/>
        /// and make haste to a Grand Olympiad Manager! Battles in the Grand Olympiad Games are now taking place!
        /// </summary>
        public static SystemMessage THE_OLYMPIAD_GAME_HAS_STARTED = new SystemMessage(1641);

        /// <summary>
        /// ID: 1642
        /// <para/>
        /// Message: Much carnage has been left for the cleanup crew of the Olympiad Stadium. Battles in the Grand Olympiad Games are now over!
        /// </summary>
        public static SystemMessage THE_OLYMPIAD_GAME_HAS_ENDED = new SystemMessage(1642);

        /// <summary>
        /// ID: 1643
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (Dimensional Gap)
        /// </summary>
        public static SystemMessage LOC_DIMENSIONAL_GAP_S1_S2_S3 = new SystemMessage(1643);

        /// <summary>
        /// ID: 1649
        /// <para/>
        /// Message: Play time is now accumulating.
        /// </summary>
        public static SystemMessage PLAY_TIME_NOW_ACCUMULATING = new SystemMessage(1649);

        /// <summary>
        /// ID: 1650
        /// <para/>
        /// Message: Due to high server traffic, your login attempt has failed. Please try again soon.
        /// </summary>
        public static SystemMessage TRY_LOGIN_LATER = new SystemMessage(1650);

        /// <summary>
        /// ID: 1651
        /// <para/>
        /// Message: The Grand Olympiad Games are not currently in progress.
        /// </summary>
        public static SystemMessage THE_OLYMPIAD_GAME_IS_NOT_CURRENTLY_IN_PROGRESS = new SystemMessage(1651);

        /// <summary>
        /// ID: 1652
        /// <para/>
        /// Message: You are now recording gameplay.
        /// </summary>
        public static SystemMessage RECORDING_GAMEPLAY_START = new SystemMessage(1652);

        /// <summary>
        /// ID: 1653
        /// <para/>
        /// Message: Your recording has been successfully stored. ($s1)
        /// </summary>
        public static SystemMessage RECORDING_GAMEPLAY_STOP_S1 = new SystemMessage(1653);

        /// <summary>
        /// ID: 1654
        /// <para/>
        /// Message: Your attempt to record the replay file has failed.
        /// </summary>
        public static SystemMessage RECORDING_GAMEPLAY_FAILED = new SystemMessage(1654);

        /// <summary>
        /// ID: 1655
        /// <para/>
        /// Message: You caught something smelly and scary, maybe you should throw it back!?
        /// </summary>
        public static SystemMessage YOU_CAUGHT_SOMETHING_SMELLY_THROW_IT_BACK = new SystemMessage(1655);

        /// <summary>
        /// ID: 1656
        /// <para/>
        /// Message: You have successfully traded the item with the NPC.
        /// </summary>
        public static SystemMessage SUCCESSFULLY_TRADED_WITH_NPC = new SystemMessage(1656);

        /// <summary>
        /// ID: 1657
        /// <para/>
        /// Message: $s1 has earned $s2 points in the Grand Olympiad Games.
        /// </summary>
        public static SystemMessage S1_HAS_GAINED_S2_OLYMPIAD_POINTS = new SystemMessage(1657);

        /// <summary>
        /// ID: 1658
        /// <para/>
        /// Message: $s1 has lost $s2 points in the Grand Olympiad Games.
        /// </summary>
        public static SystemMessage S1_HAS_LOST_S2_OLYMPIAD_POINTS = new SystemMessage(1658);

        /// <summary>
        /// ID: 1659
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (Cemetery of the Empire)
        /// </summary>
        public static SystemMessage LOC_CEMETARY_OF_THE_EMPIRE_S1_S2_S3 = new SystemMessage(1659);

        /// <summary>
        /// ID: 1660
        /// <para/>
        /// Message: Channel Creator: $s1.
        /// </summary>
        public static SystemMessage CHANNEL_CREATOR_S1 = new SystemMessage(1660);

        /// <summary>
        /// ID: 1661
        /// <para/>
        /// Message: $s1 has obtained $s3 $s2s.
        /// </summary>
        public static SystemMessage S1_OBTAINED_S3_S2_S = new SystemMessage(1661);

        /// <summary>
        /// ID: 1662
        /// <para/>
        /// Message: The fish are no longer biting here because you've caught too many! Try fishing in another location.
        /// </summary>
        public static SystemMessage FISH_NO_MORE_BITING_TRY_OTHER_LOCATION = new SystemMessage(1662);

        /// <summary>
        /// ID: 1663
        /// <para/>
        /// Message: The clan crest was successfully registered. Remember, only a clan that owns a clan hall or castle can have their crest displayed.
        /// </summary>
        public static SystemMessage CLAN_EMBLEM_WAS_SUCCESSFULLY_REGISTERED = new SystemMessage(1663);

        /// <summary>
        /// ID: 1664
        /// <para/>
        /// Message: The fish is resisting your efforts to haul it in! Look at that bobber go!
        /// </summary>
        public static SystemMessage FISH_RESISTING_LOOK_BOBBLER = new SystemMessage(1664);

        /// <summary>
        /// ID: 1665
        /// <para/>
        /// Message: You've worn that fish out! It can't even pull the bobber under the water!
        /// </summary>
        public static SystemMessage YOU_WORN_FISH_OUT = new SystemMessage(1665);

        /// <summary>
        /// ID: 1666
        /// <para/>
        /// Message: You have obtained +$s1 $s2.
        /// </summary>
        public static SystemMessage OBTAINED_S1_S2 = new SystemMessage(1666);

        /// <summary>
        /// ID: 1667
        /// <para/>
        /// Message: Lethal Strike!
        /// </summary>
        public static SystemMessage LETHAL_STRIKE = new SystemMessage(1667);

        /// <summary>
        /// ID: 1668
        /// <para/>
        /// Message: Your lethal strike was successful!
        /// </summary>
        public static SystemMessage LETHAL_STRIKE_SUCCESSFUL = new SystemMessage(1668);

        /// <summary>
        /// ID: 1669
        /// <para/>
        /// Message: There was nothing found inside of that.
        /// </summary>
        public static SystemMessage NOTHING_INSIDE_THAT = new SystemMessage(1669);

        /// <summary>
        /// ID: 1670
        /// <para/>
        /// Message: Due to your Reeling and/or Pumping skill being three or more levels higher than your Fishing skill, a 50 damage penalty will be applied.
        /// </summary>
        public static SystemMessage REELING_PUMPING_3_LEVELS_HIGHER_THAN_FISHING_PENALTY = new SystemMessage(1670);

        /// <summary>
        /// ID: 1671
        /// <para/>
        /// Message: Your reeling was successful! (Mastery Penalty:$s1 )
        /// </summary>
        public static SystemMessage REELING_SUCCESSFUL_PENALTY_S1 = new SystemMessage(1671);

        /// <summary>
        /// ID: 1672
        /// <para/>
        /// Message: Your pumping was successful! (Mastery Penalty:$s1 )
        /// </summary>
        public static SystemMessage PUMPING_SUCCESSFUL_PENALTY_S1 = new SystemMessage(1672);

        /// <summary>
        /// ID: 1673
        /// <para/>
        /// Message: Your current record for this Grand Olympiad is $s1 match(es), $s2 win(s) and $s3 defeat(s). You have earned $s4 Olympiad Point(s).
        /// </summary>
        public static SystemMessage THE_CURRENT_RECORD_FOR_THIS_OLYMPIAD_SESSION_IS_S1_MATCHES_S2_WINS_S3_DEFEATS_YOU_HAVE_EARNED_S4_OLYMPIAD_POINTS = new SystemMessage(1673);

        /// <summary>
        /// ID: 1674
        /// <para/>
        /// Message: This command can only be used by a Noblesse.
        /// </summary>
        public static SystemMessage NOBLESSE_ONLY = new SystemMessage(1674);

        /// <summary>
        /// ID: 1675
        /// <para/>
        /// Message: A manor cannot be set up between 6 a.m. and 8 p.m.
        /// </summary>
        public static SystemMessage A_MANOR_CANNOT_BE_SET_UP_BETWEEN_6_AM_AND_8_PM = new SystemMessage(1675);

        /// <summary>
        /// ID: 1676
        /// <para/>
        /// Message: You do not have a servitor or pet and therefore cannot use the automatic-use function.
        /// </summary>
        public static SystemMessage NO_SERVITOR_CANNOT_AUTOMATE_USE = new SystemMessage(1676);

        /// <summary>
        /// ID: 1677
        /// <para/>
        /// Message: A cease-fire during a Clan War can not be called while members of your clan are engaged in battle.
        /// </summary>
        public static SystemMessage CANT_STOP_CLAN_WAR_WHILE_IN_COMBAT = new SystemMessage(1677);

        /// <summary>
        /// ID: 1678
        /// <para/>
        /// Message: You have not declared a Clan War against the clan $s1.
        /// </summary>
        public static SystemMessage NO_CLAN_WAR_AGAINST_CLAN_S1 = new SystemMessage(1678);

        /// <summary>
        /// ID: 1679
        /// <para/>
        /// Message: Only the creator of a channel can issue a global command.
        /// </summary>
        public static SystemMessage ONLY_CHANNEL_CREATOR_CAN_GLOBAL_COMMAND = new SystemMessage(1679);

        /// <summary>
        /// ID: 1680
        /// <para/>
        /// Message: $s1 has declined the channel invitation.
        /// </summary>
        public static SystemMessage S1_DECLINED_CHANNEL_INVITATION = new SystemMessage(1680);

        /// <summary>
        /// ID: 1681
        /// <para/>
        /// Message: Since $s1 did not respond, your channel invitation has failed.
        /// </summary>
        public static SystemMessage S1_DID_NOT_RESPOND_CHANNEL_INVITATION_FAILED = new SystemMessage(1681);

        /// <summary>
        /// ID: 1682
        /// <para/>
        /// Message: Only the creator of a channel can use the channel dismiss command.
        /// </summary>
        public static SystemMessage ONLY_CHANNEL_CREATOR_CAN_DISMISS = new SystemMessage(1682);

        /// <summary>
        /// ID: 1683
        /// <para/>
        /// Message: Only a party leader can choose the option to leave a channel.
        /// </summary>
        public static SystemMessage ONLY_PARTY_LEADER_CAN_LEAVE_CHANNEL = new SystemMessage(1683);

        /// <summary>
        /// ID: 1684
        /// <para/>
        /// Message: A Clan War can not be declared against a clan that is being dissolved.
        /// </summary>
        public static SystemMessage NO_CLAN_WAR_AGAINST_DISSOLVING_CLAN = new SystemMessage(1684);

        /// <summary>
        /// ID: 1685
        /// <para/>
        /// Message: You are unable to equip this item when your PK count is greater or equal to one.
        /// </summary>
        public static SystemMessage YOU_ARE_UNABLE_TO_EQUIP_THIS_ITEM_WHEN_YOUR_PK_COUNT_IS_GREATER_THAN_OR_EQUAL_TO_ONE = new SystemMessage(1685);

        /// <summary>
        /// ID: 1686
        /// <para/>
        /// Message: Stones and mortar tumble to the earth - the castle wall has taken damage!
        /// </summary>
        public static SystemMessage CASTLE_WALL_DAMAGED = new SystemMessage(1686);

        /// <summary>
        /// ID: 1687
        /// <para/>
        /// Message: This area cannot be entered while mounted atop of a Wyvern. You will be dismounted from your Wyvern if you do not leave!
        /// </summary>
        public static SystemMessage AREA_CANNOT_BE_ENTERED_WHILE_MOUNTED_WYVERN = new SystemMessage(1687);

        /// <summary>
        /// ID: 1688
        /// <para/>
        /// Message: You cannot enchant while operating a Private Store or Private Workshop.
        /// </summary>
        public static SystemMessage CANNOT_ENCHANT_WHILE_STORE = new SystemMessage(1688);

        /// <summary>
        /// ID: 1689
        /// <para/>
        /// Message: You have already joined the waiting list for a class specific match.
        /// </summary>
        public static SystemMessage YOU_ARE_ALREADY_ON_THE_WAITING_LIST_TO_PARTICIPATE_IN_THE_GAME_FOR_YOUR_CLASS = new SystemMessage(1689);

        /// <summary>
        /// ID: 1690
        /// <para/>
        /// Message: You have already joined the waiting list for a non-class specific match.
        /// </summary>
        public static SystemMessage YOU_ARE_ALREADY_ON_THE_WAITING_LIST_FOR_ALL_CLASSES_WAITING_TO_PARTICIPATE_IN_THE_GAME = new SystemMessage(1690);

        /// <summary>
        /// ID: 1691
        /// <para/>
        /// Message: You can't join a Grand Olympiad Game match with that much stuff on you! Reduce your weight to below 80 percent full and request to join again!
        /// </summary>
        public static SystemMessage SINCE_80_PERCENT_OR_MORE_OF_YOUR_INVENTORY_SLOTS_ARE_FULL_YOU_CANNOT_PARTICIPATE_IN_THE_OLYMPIAD = new SystemMessage(1691);

        /// <summary>
        /// ID: 1692
        /// <para/>
        /// Message: You have changed from your main class to a subclass and therefore are removed from the Grand Olympiad Games waiting list.
        /// </summary>
        public static SystemMessage SINCE_YOU_HAVE_CHANGED_YOUR_CLASS_INTO_A_SUB_JOB_YOU_CANNOT_PARTICIPATE_IN_THE_OLYMPIAD = new SystemMessage(1692);

        /// <summary>
        /// ID: 1693
        /// <para/>
        /// Message: You may not observe a Grand Olympiad Games match while you are on the waiting list.
        /// </summary>
        public static SystemMessage WHILE_YOU_ARE_ON_THE_WAITING_LIST_YOU_ARE_NOT_ALLOWED_TO_WATCH_THE_GAME = new SystemMessage(1693);

        /// <summary>
        /// ID: 1694
        /// <para/>
        /// Message: Only a clan leader that is a Noblesse can view the Siege War Status window during a siege war.
        /// </summary>
        public static SystemMessage ONLY_NOBLESSE_LEADER_CAN_VIEW_SIEGE_STATUS_WINDOW = new SystemMessage(1694);

        /// <summary>
        /// ID: 1695
        /// <para/>
        /// Message: You can only use that during a Siege War!
        /// </summary>
        public static SystemMessage ONLY_DURING_SIEGE = new SystemMessage(1695);

        /// <summary>
        /// ID: 1696
        /// <para/>
        /// Message: Your accumulated play time is $s1.
        /// </summary>
        public static SystemMessage ACCUMULATED_PLAY_TIME_IS_S1 = new SystemMessage(1696);

        /// <summary>
        /// ID: 1697
        /// <para/>
        /// Message: Your accumulated play time has reached Fatigue level, so you will receive experience or item drops at only 50 percent [...]
        /// </summary>
        public static SystemMessage ACCUMULATED_PLAY_TIME_WARNING1 = new SystemMessage(1697);

        /// <summary>
        /// ID: 1698
        /// <para/>
        /// Message: Your accumulated play time has reached Ill-health level, so you will no longer gain experience or item drops. [...}
        /// </summary>
        public static SystemMessage ACCUMULATED_PLAY_TIME_WARNING2 = new SystemMessage(1698);

        /// <summary>
        /// ID: 1699
        /// <para/>
        /// Message: You cannot dismiss a party member by force.
        /// </summary>
        public static SystemMessage CANNOT_DISMISS_PARTY_MEMBER = new SystemMessage(1699);

        /// <summary>
        /// ID: 1700
        /// <para/>
        /// Message: You don't have enough spiritshots needed for a pet/servitor.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SPIRITSHOTS_FOR_PET = new SystemMessage(1700);

        /// <summary>
        /// ID: 1701
        /// <para/>
        /// Message: You don't have enough soulshots needed for a pet/servitor.
        /// </summary>
        public static SystemMessage NOT_ENOUGH_SOULSHOTS_FOR_PET = new SystemMessage(1701);

        /// <summary>
        /// ID: 1702
        /// <para/>
        /// Message: $s1 is using a third party program.
        /// </summary>
        public static SystemMessage S1_USING_THIRD_PARTY_PROGRAM = new SystemMessage(1702);

        /// <summary>
        /// ID: 1703
        /// <para/>
        /// Message: The previous investigated user is not using a third party program
        /// </summary>
        public static SystemMessage NOT_USING_THIRD_PARTY_PROGRAM = new SystemMessage(1703);

        /// <summary>
        /// ID: 1704
        /// <para/>
        /// Message: Please close the setup window for your private manufacturing store or private store, and try again.
        /// </summary>
        public static SystemMessage CLOSE_STORE_WINDOW_AND_TRY_AGAIN = new SystemMessage(1704);

        /// <summary>
        /// ID: 1705
        /// <para/>
        /// Message: PC Bang Points acquisition period. Points acquisition period left $s1 hour.
        /// </summary>
        public static SystemMessage PCPOINT_ACQUISITION_PERIOD = new SystemMessage(1705);

        /// <summary>
        /// ID: 1706
        /// <para/>
        /// Message: PC Bang Points use period. Points acquisition period left $s1 hour.
        /// </summary>
        public static SystemMessage PCPOINT_USE_PERIOD = new SystemMessage(1706);

        /// <summary>
        /// ID: 1707
        /// <para/>
        /// Message: You acquired $s1 PC Bang Point.
        /// </summary>
        public static SystemMessage ACQUIRED_S1_PCPOINT = new SystemMessage(1707);

        /// <summary>
        /// ID: 1708
        /// <para/>
        /// Message: Double points! You acquired $s1 PC Bang Point.
        /// </summary>
        public static SystemMessage ACQUIRED_S1_PCPOINT_DOUBLE = new SystemMessage(1708);

        /// <summary>
        /// ID: 1709
        /// <para/>
        /// Message: You are using $s1 point.
        /// </summary>
        public static SystemMessage USING_S1_PCPOINT = new SystemMessage(1709);

        /// <summary>
        /// ID: 1710
        /// <para/>
        /// Message: You are short of accumulated points.
        /// </summary>
        public static SystemMessage SHORT_OF_ACCUMULATED_POINTS = new SystemMessage(1710);

        /// <summary>
        /// ID: 1711
        /// <para/>
        /// Message: PC Bang Points use period has expired.
        /// </summary>
        public static SystemMessage PCPOINT_USE_PERIOD_EXPIRED = new SystemMessage(1711);

        /// <summary>
        /// ID: 1712
        /// <para/>
        /// Message: The PC Bang Points accumulation period has expired.
        /// </summary>
        public static SystemMessage PCPOINT_ACCUMULATION_PERIOD_EXPIRED = new SystemMessage(1712);

        /// <summary>
        /// ID: 1713
        /// <para/>
        /// Message: The games may be delayed due to an insufficient number of players waiting.
        /// </summary>
        public static SystemMessage GAMES_DELAYED = new SystemMessage(1713);

        /// <summary>
        /// ID: 1714
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (Near the Town of Schuttgart)
        /// </summary>
        public static SystemMessage LOC_SCHUTTGART_S1_S2_S3 = new SystemMessage(1714);

        /// <summary>
        /// ID: 1715
        /// <para/>
        /// Message: This is a Peaceful Zone
        /// </summary>
        public static SystemMessage PEACEFUL_ZONE = new SystemMessage(1715);

        /// <summary>
        /// ID: 1716
        /// <para/>
        /// Message: Altered Zone
        /// </summary>
        public static SystemMessage ALTERED_ZONE = new SystemMessage(1716);

        /// <summary>
        /// ID: 1717
        /// <para/>
        /// Message: Siege War Zone
        /// </summary>
        public static SystemMessage SIEGE_ZONE = new SystemMessage(1717);

        /// <summary>
        /// ID: 1718
        /// <para/>
        /// Message: General Field
        /// </summary>
        public static SystemMessage GENERAL_ZONE = new SystemMessage(1718);

        /// <summary>
        /// ID: 1719
        /// <para/>
        /// Message: Seven Signs Zone
        /// </summary>
        public static SystemMessage SEVENSIGNS_ZONE = new SystemMessage(1719);

        /// <summary>
        /// ID: 1720
        /// <para/>
        /// Message: ---
        /// </summary>
        public static SystemMessage UNKNOWN1 = new SystemMessage(1720);

        /// <summary>
        /// ID: 1721
        /// <para/>
        /// Message: Combat Zone
        /// </summary>
        public static SystemMessage COMBAT_ZONE = new SystemMessage(1721);

        /// <summary>
        /// ID: 1722
        /// <para/>
        /// Message: Please enter the name of the item you wish to search for.
        /// </summary>
        public static SystemMessage ENTER_ITEM_NAME_SEARCH = new SystemMessage(1722);

        /// <summary>
        /// ID: 1723
        /// <para/>
        /// Message: Please take a moment to provide feedback about the petition service.
        /// </summary>
        public static SystemMessage PLEASE_PROVIDE_PETITION_FEEDBACK = new SystemMessage(1723);

        /// <summary>
        /// ID: 1724
        /// <para/>
        /// Message: A servitor whom is engaged in battle cannot be de-activated.
        /// </summary>
        public static SystemMessage SERVITOR_NOT_RETURN_IN_BATTLE = new SystemMessage(1724);

        /// <summary>
        /// ID: 1725
        /// <para/>
        /// Message: You have earned $s1 raid point(s).
        /// </summary>
        public static SystemMessage EARNED_S1_RAID_POINTS = new SystemMessage(1725);

        /// <summary>
        /// ID: 1726
        /// <para/>
        /// Message: $s1 has disappeared because its time period has expired.
        /// </summary>
        public static SystemMessage S1_PERIOD_EXPIRED_DISAPPEARED = new SystemMessage(1726);

        /// <summary>
        /// ID: 1727
        /// <para/>
        /// Message: $s1 has invited you to a party room. Do you accept?
        /// </summary>
        public static SystemMessage S1_INVITED_YOU_TO_PARTY_ROOM_CONFIRM = new SystemMessage(1727);

        /// <summary>
        /// ID: 1728
        /// <para/>
        /// Message: The recipient of your invitation did not accept the party matching invitation.
        /// </summary>
        public static SystemMessage PARTY_MATCHING_REQUEST_NO_RESPONSE = new SystemMessage(1728);

        /// <summary>
        /// ID: 1729
        /// <para/>
        /// Message: You cannot join a Command Channel while teleporting.
        /// </summary>
        public static SystemMessage NOT_JOIN_CHANNEL_WHILE_TELEPORTING = new SystemMessage(1729);

        /// <summary>
        /// ID: 1730
        /// <para/>
        /// Message: To establish a Clan Academy, your clan must be Level 5 or higher.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_CLAN_ACADEMY = new SystemMessage(1730);

        /// <summary>
        /// ID: 1731
        /// <para/>
        /// Message: Only the leader can create a Clan Academy.
        /// </summary>
        public static SystemMessage ONLY_LEADER_CAN_CREATE_ACADEMY = new SystemMessage(1731);

        /// <summary>
        /// ID: 1732
        /// <para/>
        /// Message: To create a Clan Academy, a Blood Mark is needed.
        /// </summary>
        public static SystemMessage NEED_BLOODMARK_FOR_ACADEMY = new SystemMessage(1732);

        /// <summary>
        /// ID: 1733
        /// <para/>
        /// Message: You do not have enough adena to create a Clan Academy.
        /// </summary>
        public static SystemMessage NEED_ADENA_FOR_ACADEMY = new SystemMessage(1733);

        /// <summary>
        /// ID: 1734
        /// <para/>
        /// not belong another clan and not yet completed their 2nd class transfer.
        /// </summary>
        public static SystemMessage ACADEMY_REQUIREMENTS = new SystemMessage(1734);

        /// <summary>
        /// ID: 1735
        /// <para/>
        /// Message: $s1 does not meet the requirements to join a Clan Academy.
        /// </summary>
        public static SystemMessage S1_DOESNOT_MEET_REQUIREMENTS_TO_JOIN_ACADEMY = new SystemMessage(1735);

        /// <summary>
        /// ID: 1736
        /// <para/>
        /// Message: The Clan Academy has reached its maximum enrollment.
        /// </summary>
        public static SystemMessage ACADEMY_MAXIMUM = new SystemMessage(1736);

        /// <summary>
        /// ID: 1737
        /// <para/>
        /// Message: Your clan has not established a Clan Academy but is eligible to do so.
        /// </summary>
        public static SystemMessage CLAN_CAN_CREATE_ACADEMY = new SystemMessage(1737);

        /// <summary>
        /// ID: 1738
        /// <para/>
        /// Message: Your clan has already established a Clan Academy.
        /// </summary>
        public static SystemMessage CLAN_HAS_ALREADY_ESTABLISHED_A_CLAN_ACADEMY = new SystemMessage(1738);

        /// <summary>
        /// ID: 1739
        /// <para/>
        /// Message: Would you like to create a Clan Academy?
        /// </summary>
        public static SystemMessage CLAN_ACADEMY_CREATE_CONFIRM = new SystemMessage(1739);

        /// <summary>
        /// ID: 1740
        /// <para/>
        /// Message: Please enter the name of the Clan Academy.
        /// </summary>
        public static SystemMessage ACADEMY_CREATE_ENTER_NAME = new SystemMessage(1740);

        /// <summary>
        /// ID: 1741
        /// <para/>
        /// Message: Congratulations! The $s1's Clan Academy has been created.
        /// </summary>
        public static SystemMessage THE_S1S_CLAN_ACADEMY_HAS_BEEN_CREATED = new SystemMessage(1741);

        /// <summary>
        /// ID: 1742
        /// <para/>
        /// Message: A message inviting $s1 to join the Clan Academy is being sent.
        /// </summary>
        public static SystemMessage ACADEMY_INVITATION_SENT_TO_S1 = new SystemMessage(1742);

        /// <summary>
        /// ID: 1743
        /// <para/>
        /// Message: To open a Clan Academy, the leader of a Level 5 clan or above must pay XX Proofs of Blood or a certain amount of adena.
        /// </summary>
        public static SystemMessage OPEN_ACADEMY_CONDITIONS = new SystemMessage(1743);

        /// <summary>
        /// ID: 1744
        /// <para/>
        /// Message: There was no response to your invitation to join the Clan Academy, so the invitation has been rescinded.
        /// </summary>
        public static SystemMessage ACADEMY_JOIN_NO_RESPONSE = new SystemMessage(1744);

        /// <summary>
        /// ID: 1745
        /// <para/>
        /// Message: The recipient of your invitation to join the Clan Academy has declined.
        /// </summary>
        public static SystemMessage ACADEMY_JOIN_DECLINE = new SystemMessage(1745);

        /// <summary>
        /// ID: 1746
        /// <para/>
        /// Message: You have already joined a Clan Academy.
        /// </summary>
        public static SystemMessage ALREADY_JOINED_ACADEMY = new SystemMessage(1746);

        /// <summary>
        /// ID: 1747
        /// <para/>
        /// Message: $s1 has sent you an invitation to join the Clan Academy belonging to the $s2 clan. Do you accept?
        /// </summary>
        public static SystemMessage JOIN_ACADEMY_REQUEST_BY_S1_FOR_CLAN_S2 = new SystemMessage(1747);

        /// <summary>
        /// ID: 1748
        /// <para/>
        /// Message: Clan Academy member $s1 has successfully completed the 2nd class transfer and obtained $s2 Clan Reputation points.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_GRADUATED_FROM_ACADEMY = new SystemMessage(1748);

        /// <summary>
        /// ID: 1749
        /// <para/>
        /// Message: Congratulations! You will now graduate from the Clan Academy and leave your current clan. As a graduate of the academy, you can immediately join a clan as a regular member without being subject to any penalties.
        /// </summary>
        public static SystemMessage ACADEMY_MEMBERSHIP_TERMINATED = new SystemMessage(1749);

        /// <summary>
        /// ID: 1750
        /// <para/>
        /// Message: If you possess $s1, you cannot participate in the Olympiad.
        /// </summary>
        public static SystemMessage CANNOT_JOIN_OLYMPIAD_POSSESSING_S1 = new SystemMessage(1750);

        /// <summary>
        /// ID: 1751
        /// <para/>
        /// Message: The Grand Master has given you a commemorative item.
        /// </summary>
        public static SystemMessage GRAND_MASTER_COMMEMORATIVE_ITEM = new SystemMessage(1751);

        /// <summary>
        /// ID: 1752
        /// <para/>
        /// Message: Since the clan has received a graduate of the Clan Academy, it has earned $s1 points towards its reputation score.
        /// </summary>
        public static SystemMessage MEMBER_GRADUATED_EARNED_S1_REPU = new SystemMessage(1752);

        /// <summary>
        /// ID: 1753
        /// <para/>
        /// Message: The clan leader has decreed that that particular privilege cannot be granted to a Clan Academy member.
        /// </summary>
        public static SystemMessage CANT_TRANSFER_PRIVILEGE_TO_ACADEMY_MEMBER = new SystemMessage(1753);

        /// <summary>
        /// ID: 1754
        /// <para/>
        /// Message: That privilege cannot be granted to a Clan Academy member.
        /// </summary>
        public static SystemMessage RIGHT_CANT_TRANSFERRED_TO_ACADEMY_MEMBER = new SystemMessage(1754);

        /// <summary>
        /// ID: 1755
        /// <para/>
        /// Message: $s2 has been designated as the apprentice of clan member $s1.
        /// </summary>
        public static SystemMessage S2_HAS_BEEN_DESIGNATED_AS_APPRENTICE_OF_CLAN_MEMBER_S1 = new SystemMessage(1755);

        /// <summary>
        /// ID: 1756
        /// <para/>
        /// Message: Your apprentice, $s1, has logged in.
        /// </summary>
        public static SystemMessage YOUR_APPRENTICE_S1_HAS_LOGGED_IN = new SystemMessage(1756);

        /// <summary>
        /// ID: 1757
        /// <para/>
        /// Message: Your apprentice, $s1, has logged out.
        /// </summary>
        public static SystemMessage YOUR_APPRENTICE_S1_HAS_LOGGED_OUT = new SystemMessage(1757);

        /// <summary>
        /// ID: 1758
        /// <para/>
        /// Message: Your sponsor, $s1, has logged in.
        /// </summary>
        public static SystemMessage YOUR_SPONSOR_S1_HAS_LOGGED_IN = new SystemMessage(1758);

        /// <summary>
        /// ID: 1759
        /// <para/>
        /// Message: Your sponsor, $s1, has logged out.
        /// </summary>
        public static SystemMessage YOUR_SPONSOR_S1_HAS_LOGGED_OUT = new SystemMessage(1759);

        /// <summary>
        /// ID: 1760
        /// <para/>
        /// Message: Clan member $s1's name title has been changed to $2.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_TITLE_CHANGED_TO_S2 = new SystemMessage(1760);

        /// <summary>
        /// ID: 1761
        /// <para/>
        /// Message: Clan member $s1's privilege level has been changed to $s2.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_PRIVILEGE_CHANGED_TO_S2 = new SystemMessage(1761);

        /// <summary>
        /// ID: 1762
        /// <para/>
        /// Message: You do not have the right to dismiss an apprentice.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_HAVE_THE_RIGHT_TO_DISMISS_AN_APPRENTICE = new SystemMessage(1762);

        /// <summary>
        /// ID: 1763
        /// <para/>
        /// Message: $s2, clan member $s1's apprentice, has been removed.
        /// </summary>
        public static SystemMessage S2_CLAN_MEMBER_S1_APPRENTICE_HAS_BEEN_REMOVED = new SystemMessage(1763);

        /// <summary>
        /// ID: 1764
        /// <para/>
        /// Message: This item can only be worn by a member of the Clan Academy.
        /// </summary>
        public static SystemMessage EQUIP_ONLY_FOR_ACADEMY = new SystemMessage(1764);

        /// <summary>
        /// ID: 1765
        /// <para/>
        /// Message: As a graduate of the Clan Academy, you can no longer wear this item.
        /// </summary>
        public static SystemMessage EQUIP_NOT_FOR_GRADUATES = new SystemMessage(1765);

        /// <summary>
        /// ID: 1766
        /// <para/>
        /// Message: An application to join the clan has been sent to $s1 in $s2.
        /// </summary>
        public static SystemMessage CLAN_JOIN_APPLICATION_SENT_TO_S1_IN_S2 = new SystemMessage(1766);

        /// <summary>
        /// ID: 1767
        /// <para/>
        /// Message: An application to join the clan Academy has been sent to $s1.
        /// </summary>
        public static SystemMessage ACADEMY_JOIN_APPLICATION_SENT_TO_S1 = new SystemMessage(1767);

        /// <summary>
        /// ID: 1768
        /// <para/>
        /// Message: $s1 has invited you to join the Clan Academy of $s2 clan. Would you like to join?
        /// </summary>
        public static SystemMessage JOIN_REQUEST_BY_S1_TO_CLAN_S2_ACADEMY = new SystemMessage(1768);

        /// <summary>
        /// ID: 1769
        /// <para/>
        /// Message: $s1 has sent you an invitation to join the $s3 Order of Knights under the $s2 clan. Would you like to join?
        /// </summary>
        public static SystemMessage JOIN_REQUEST_BY_S1_TO_ORDER_OF_KNIGHTS_S3_UNDER_CLAN_S2 = new SystemMessage(1769);

        /// <summary>
        /// ID: 1770
        /// <para/>
        /// Message: The clan's reputation score has dropped below 0. The clan may face certain penalties as a result.
        /// </summary>
        public static SystemMessage CLAN_REPU_0_MAY_FACE_PENALTIES = new SystemMessage(1770);

        /// <summary>
        /// ID: 1771
        /// <para/>
        /// Message: Now that your clan level is above Level 5, it can accumulate clan reputation points.
        /// </summary>
        public static SystemMessage CLAN_CAN_ACCUMULATE_CLAN_REPUTATION_POINTS = new SystemMessage(1771);

        /// <summary>
        /// ID: 1772
        /// <para/>
        /// Message: Since your clan was defeated in a siege, $s1 points have been deducted from your clan's reputation score and given to the opposing clan.
        /// </summary>
        public static SystemMessage CLAN_WAS_DEFEATED_IN_SIEGE_AND_LOST_S1_REPUTATION_POINTS = new SystemMessage(1772);

        /// <summary>
        /// ID: 1773
        /// <para/>
        /// Message: Since your clan emerged victorious from the siege, $s1 points have been added to your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_VICTORIOUS_IN_SIEGE_AND_GAINED_S1_REPUTATION_POINTS = new SystemMessage(1773);

        /// <summary>
        /// ID: 1774
        /// <para/>
        /// Message: Your clan's newly acquired contested clan hall has added $s1 points to your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_ACQUIRED_CONTESTED_CLAN_HALL_AND_S1_REPUTATION_POINTS = new SystemMessage(1774);

        /// <summary>
        /// ID: 1775
        /// <para/>
        /// Message: Clan member $s1 was an active member of the highest-ranked party in the Festival of Darkness. $s2 points have been added to your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_WAS_IN_HIGHEST_RANKED_PARTY_IN_FESTIVAL_OF_DARKNESS_AND_GAINED_S2_REPUTATION = new SystemMessage(1775);

        /// <summary>
        /// ID: 1776
        /// <para/>
        /// Message: Clan member $s1 was named a hero. $2s points have been added to your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_BECAME_HERO_AND_GAINED_S2_REPUTATION_POINTS = new SystemMessage(1776);

        /// <summary>
        /// ID: 1777
        /// <para/>
        /// Message: You have successfully completed a clan quest. $s1 points have been added to your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_QUEST_COMPLETED_AND_S1_POINTS_GAINED = new SystemMessage(1777);

        /// <summary>
        /// ID: 1778
        /// <para/>
        /// Message: An opposing clan has captured your clan's contested clan hall. $s1 points have been deducted from your clan's reputation score.
        /// </summary>
        public static SystemMessage OPPOSING_CLAN_CAPTURED_CLAN_HALL_AND_YOUR_CLAN_LOSES_S1_POINTS = new SystemMessage(1778);

        /// <summary>
        /// ID: 1779
        /// <para/>
        /// Message: After losing the contested clan hall, 300 points have been deducted from your clan's reputation score.
        /// </summary>
        public static SystemMessage CLAN_LOST_CONTESTED_CLAN_HALL_AND_300_POINTS = new SystemMessage(1779);

        /// <summary>
        /// ID: 1780
        /// <para/>
        /// Message: Your clan has captured your opponent's contested clan hall. $s1 points have been deducted from your opponent's clan reputation score.
        /// </summary>
        public static SystemMessage CLAN_CAPTURED_CONTESTED_CLAN_HALL_AND_S1_POINTS_DEDUCTED_FROM_OPPONENT = new SystemMessage(1780);

        /// <summary>
        /// ID: 1781
        /// <para/>
        /// Message: Your clan has added $1s points to its clan reputation score.
        /// </summary>
        public static SystemMessage CLAN_ADDED_S1S_POINTS_TO_REPUTATION_SCORE = new SystemMessage(1781);

        /// <summary>
        /// ID: 1782
        /// <para/>
        /// Message: Your clan member $s1 was killed. $s2 points have been deducted from your clan's reputation score and added to your opponent's clan reputation score.
        /// </summary>
        public static SystemMessage CLAN_MEMBER_S1_WAS_KILLED_AND_S2_POINTS_DEDUCTED_FROM_REPUTATION = new SystemMessage(1782);

        /// <summary>
        /// ID: 1783
        /// <para/>
        /// Message: For killing an opposing clan member, $s1 points have been deducted from your opponents' clan reputation score.
        /// </summary>
        public static SystemMessage FOR_KILLING_OPPOSING_MEMBER_S1_POINTS_WERE_DEDUCTED_FROM_OPPONENTS = new SystemMessage(1783);

        /// <summary>
        /// ID: 1784
        /// <para/>
        /// Message: Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score and added to your opponents'.
        /// </summary>
        public static SystemMessage YOUR_CLAN_FAILED_TO_DEFEND_CASTLE_AND_S1_POINTS_LOST_AND_ADDED_TO_OPPONENT = new SystemMessage(1784);

        /// <summary>
        /// ID: 1785
        /// <para/>
        /// Message: The clan you belong to has been initialized. $s1 points have been deducted from your clan reputation score.
        /// </summary>
        public static SystemMessage YOUR_CLAN_HAS_BEEN_INITIALIZED_AND_S1_POINTS_LOST = new SystemMessage(1785);

        /// <summary>
        /// ID: 1786
        /// <para/>
        /// Message: Your clan has failed to defend the castle. $s1 points have been deducted from your clan's reputation score.
        /// </summary>
        public static SystemMessage YOUR_CLAN_FAILED_TO_DEFEND_CASTLE_AND_S1_POINTS_LOST = new SystemMessage(1786);

        /// <summary>
        /// ID: 1787
        /// <para/>
        /// Message: $s1 points have been deducted from the clan's reputation score.
        /// </summary>
        public static SystemMessage S1_DEDUCTED_FROM_CLAN_REP = new SystemMessage(1787);

        /// <summary>
        /// ID: 1788
        /// <para/>
        /// Message: The clan skill $s1 has been added.
        /// </summary>
        public static SystemMessage CLAN_SKILL_S1_ADDED = new SystemMessage(1788);

        /// <summary>
        /// ID: 1789
        /// <para/>
        /// Message: Since the Clan Reputation Score has dropped to 0 or lower, your clan skill(s) will be de-activated.
        /// </summary>
        public static SystemMessage REPUTATION_POINTS_0_OR_LOWER_CLAN_SKILLS_DEACTIVATED = new SystemMessage(1789);

        /// <summary>
        /// ID: 1790
        /// <para/>
        /// Message: The conditions necessary to increase the clan's level have not been met.
        /// </summary>
        public static SystemMessage FAILED_TO_INCREASE_CLAN_LEVEL = new SystemMessage(1790);

        /// <summary>
        /// ID: 1791
        /// <para/>
        /// Message: The conditions necessary to create a military unit have not been met.
        /// </summary>
        public static SystemMessage YOU_DO_NOT_MEET_CRITERIA_IN_ORDER_TO_CREATE_A_MILITARY_UNIT = new SystemMessage(1791);

        /// <summary>
        /// ID: 1792
        /// <para/>
        /// Message: Please assign a manager for your new Order of Knights.
        /// </summary>
        public static SystemMessage ASSIGN_MANAGER_FOR_ORDER_OF_KNIGHTS = new SystemMessage(1792);

        /// <summary>
        /// ID: 1793
        /// <para/>
        /// Message: $s1 has been selected as the captain of $s2.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_SELECTED_AS_CAPTAIN_OF_S2 = new SystemMessage(1793);

        /// <summary>
        /// ID: 1794
        /// <para/>
        /// Message: The Knights of $s1 have been created.
        /// </summary>
        public static SystemMessage THE_KNIGHTS_OF_S1_HAVE_BEEN_CREATED = new SystemMessage(1794);

        /// <summary>
        /// ID: 1795
        /// <para/>
        /// Message: The Royal Guard of $s1 have been created.
        /// </summary>
        public static SystemMessage THE_ROYAL_GUARD_OF_S1_HAVE_BEEN_CREATED = new SystemMessage(1795);

        /// <summary>
        /// ID: 1796
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE17 = new SystemMessage(1796);

        /// <summary>
        /// ID: 1797
        /// <para/>
        /// Message: $s1 has been promoted to $s2.
        /// </summary>
        public static SystemMessage S1_PROMOTED_TO_S2 = new SystemMessage(1797);

        /// <summary>
        /// ID: 1798
        /// <para/>
        /// Message: Clan lord privileges have been transferred to $s1.
        /// </summary>
        public static SystemMessage CLAN_LEADER_PRIVILEGES_HAVE_BEEN_TRANSFERRED_TO_S1 = new SystemMessage(1798);

        /// <summary>
        /// ID: 1799
        /// <para/>
        /// Message: We are searching for BOT users. Please try again later.
        /// </summary>
        public static SystemMessage SEARCHING_FOR_BOT_USERS_TRY_AGAIN_LATER = new SystemMessage(1799);

        /// <summary>
        /// ID: 1800
        /// <para/>
        /// Message: User $s1 has a history of using BOT.
        /// </summary>
        public static SystemMessage S1_HISTORY_USING_BOT = new SystemMessage(1800);

        /// <summary>
        /// ID: 1801
        /// <para/>
        /// Message: The attempt to sell has failed.
        /// </summary>
        public static SystemMessage SELL_ATTEMPT_FAILED = new SystemMessage(1801);

        /// <summary>
        /// ID: 1802
        /// <para/>
        /// Message: The attempt to trade has failed.
        /// </summary>
        public static SystemMessage TRADE_ATTEMPT_FAILED = new SystemMessage(1802);

        /// <summary>
        /// ID: 1803
        /// <para/>
        /// Message: The request to participate in the game cannot be made starting from 10 minutes before the end of the game.
        /// </summary>
        public static SystemMessage GAME_REQUEST_CANNOT_BE_MADE = new SystemMessage(1803);

        /// <summary>
        /// ID: 1804
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE18 = new SystemMessage(1804);

        /// <summary>
        /// ID: 1805
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE19 = new SystemMessage(1805);

        /// <summary>
        /// ID: 1806
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE20 = new SystemMessage(1806);

        /// <summary>
        /// ID: 1807
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE21 = new SystemMessage(1807);

        /// <summary>
        /// ID: 1808
        /// <para/>
        /// Message: Your account has been suspended ...
        /// </summary>
        public static SystemMessage ILLEGAL_USE22 = new SystemMessage(1808);

        /// <summary>
        /// ID: 1809
        /// <para/>
        /// please visit the PlayNC website (http://www.plaync.com/us/support/)
        /// </summary>
        public static SystemMessage ACCOUNT_MUST_VERIFIED = new SystemMessage(1809);

        /// <summary>
        /// ID: 1810
        /// <para/>
        /// Message: The refuse invitation state has been activated.
        /// </summary>
        public static SystemMessage REFUSE_INVITATION_ACTIVATED = new SystemMessage(1810);

        /// <summary>
        /// ID: 1812
        /// <para/>
        /// Message: Since the refuse invitation state is currently activated, no invitation can be made
        /// </summary>
        public static SystemMessage REFUSE_INVITATION_CURRENTLY_ACTIVE = new SystemMessage(1812);

        /// <summary>
        /// ID: 1813
        /// <para/>
        /// Message: $s1 has $s2 hour(s) of usage time remaining.
        /// </summary>
        public static SystemMessage S2_HOUR_OF_USAGE_TIME_ARE_LEFT_FOR_S1 = new SystemMessage(1813);

        /// <summary>
        /// ID: 1814
        /// <para/>
        /// Message: $s1 has $s2 minute(s) of usage time remaining.
        /// </summary>
        public static SystemMessage S2_MINUTE_OF_USAGE_TIME_ARE_LEFT_FOR_S1 = new SystemMessage(1814);

        /// <summary>
        /// ID: 1815
        /// <para/>
        /// Message: $s2 was dropped in the $s1 region.
        /// </summary>
        public static SystemMessage S2_WAS_DROPPED_IN_THE_S1_REGION = new SystemMessage(1815);

        /// <summary>
        /// ID: 1816
        /// <para/>
        /// Message: The owner of $s2 has appeared in the $s1 region.
        /// </summary>
        public static SystemMessage THE_OWNER_OF_S2_HAS_APPEARED_IN_THE_S1_REGION = new SystemMessage(1816);

        /// <summary>
        /// ID: 1817
        /// <para/>
        /// Message: $s2's owner has logged into the $s1 region.
        /// </summary>
        public static SystemMessage S2_OWNER_HAS_LOGGED_INTO_THE_S1_REGION = new SystemMessage(1817);

        /// <summary>
        /// ID: 1818
        /// <para/>
        /// Message: $s1 has disappeared.
        /// </summary>
        public static SystemMessage S1_HAS_DISAPPEARED = new SystemMessage(1818);

        /// <summary>
        /// ID: 1819
        /// <para/>
        /// Message: An evil is pulsating from $s2 in $s1.
        /// </summary>
        public static SystemMessage EVIL_FROM_S2_IN_S1 = new SystemMessage(1819);

        /// <summary>
        /// ID: 1820
        /// <para/>
        /// Message: $s1 is currently asleep.
        /// </summary>
        public static SystemMessage S1_CURRENTLY_SLEEP = new SystemMessage(1820);

        /// <summary>
        /// ID: 1821
        /// <para/>
        /// Message: $s2's evil presence is felt in $s1.
        /// </summary>
        public static SystemMessage S2_EVIL_PRESENCE_FELT_IN_S1 = new SystemMessage(1821);

        /// <summary>
        /// ID: 1822
        /// <para/>
        /// Message: $s1 has been sealed.
        /// </summary>
        public static SystemMessage S1_SEALED = new SystemMessage(1822);

        /// <summary>
        /// ID: 1823
        /// <para/>
        /// Message: The registration period for a clan hall war has ended.
        /// </summary>
        public static SystemMessage CLANHALL_WAR_REGISTRATION_PERIOD_ENDED = new SystemMessage(1823);

        /// <summary>
        /// ID: 1824
        /// <para/>
        /// Message: You have been registered for a clan hall war. Please move to the left side of the clan hall's arena and get ready.
        /// </summary>
        public static SystemMessage REGISTERED_FOR_CLANHALL_WAR = new SystemMessage(1824);

        /// <summary>
        /// ID: 1825
        /// <para/>
        /// Message: You have failed in your attempt to register for the clan hall war. Please try again.
        /// </summary>
        public static SystemMessage CLANHALL_WAR_REGISTRATION_FAILED = new SystemMessage(1825);

        /// <summary>
        /// ID: 1826
        /// <para/>
        /// Message: In $s1 minute(s), the game will begin. All players must hurry and move to the left side of the clan hall's arena.
        /// </summary>
        public static SystemMessage CLANHALL_WAR_BEGINS_IN_S1_MINUTES = new SystemMessage(1826);

        /// <summary>
        /// ID: 1827
        /// <para/>
        /// Message: In $s1 minute(s), the game will begin. All players must, please enter the arena now
        /// </summary>
        public static SystemMessage CLANHALL_WAR_BEGINS_IN_S1_MINUTES_ENTER_NOW = new SystemMessage(1827);

        /// <summary>
        /// ID: 1828
        /// <para/>
        /// Message: In $s1 seconds(s), the game will begin.
        /// </summary>
        public static SystemMessage CLANHALL_WAR_BEGINS_IN_S1_SECONDS = new SystemMessage(1828);

        /// <summary>
        /// ID: 1829
        /// <para/>
        /// Message: The Command Channel is full.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_FULL = new SystemMessage(1829);

        /// <summary>
        /// ID: 1830
        /// <para/>
        /// Message: $s1 is not allowed to use the party room invite command. Please update the waiting list.
        /// </summary>
        public static SystemMessage S1_NOT_ALLOWED_INVITE_TO_PARTY_ROOM = new SystemMessage(1830);

        /// <summary>
        /// ID: 1831
        /// <para/>
        /// Message: $s1 does not meet the conditions of the party room. Please update the waiting list.
        /// </summary>
        public static SystemMessage S1_NOT_MEET_CONDITIONS_FOR_PARTY_ROOM = new SystemMessage(1831);

        /// <summary>
        /// ID: 1832
        /// <para/>
        /// Message: Only a room leader may invite others to a party room.
        /// </summary>
        public static SystemMessage ONLY_ROOM_LEADER_CAN_INVITE = new SystemMessage(1832);

        /// <summary>
        /// ID: 1833
        /// <para/>
        /// Message: All of $s1 will be dropped. Would you like to continue?
        /// </summary>
        public static SystemMessage CONFIRM_DROP_ALL_OF_S1 = new SystemMessage(1833);

        /// <summary>
        /// ID: 1834
        /// <para/>
        /// Message: The party room is full. No more characters can be invitet in
        /// </summary>
        public static SystemMessage PARTY_ROOM_FULL = new SystemMessage(1834);

        /// <summary>
        /// ID: 1835
        /// <para/>
        /// Message: $s1 is full and cannot accept additional clan members at this time.
        /// </summary>
        public static SystemMessage S1_CLAN_IS_FULL = new SystemMessage(1835);

        /// <summary>
        /// ID: 1836
        /// <para/>
        /// Message: You cannot join a Clan Academy because you have successfully completed your 2nd class transfer.
        /// </summary>
        public static SystemMessage CANNOT_JOIN_ACADEMY_AFTER_2ND_OCCUPATION = new SystemMessage(1836);

        /// <summary>
        /// ID: 1837
        /// <para/>
        /// Message: $s1 has sent you an invitation to join the $s3 Royal Guard under the $s2 clan. Would you like to join?
        /// </summary>
        public static SystemMessage S1_SENT_INVITATION_TO_ROYAL_GUARD_S3_OF_CLAN_S2 = new SystemMessage(1837);

        /// <summary>
        /// ID: 1838
        /// <para/>
        /// Message: 1. The coupon an be used once per character.
        /// </summary>
        public static SystemMessage COUPON_ONCE_PER_CHARACTER = new SystemMessage(1838);

        /// <summary>
        /// ID: 1839
        /// <para/>
        /// Message: 2. A used serial number may not be used again.
        /// </summary>
        public static SystemMessage SERIAL_MAY_USED_ONCE = new SystemMessage(1839);

        /// <summary>
        /// ID: 1840
        /// <para/>
        /// Message: 3. If you enter the incorrect serial number more than 5 times, ...
        /// </summary>
        public static SystemMessage SERIAL_INPUT_INCORRECT = new SystemMessage(1840);

        /// <summary>
        /// ID: 1841
        /// <para/>
        /// Message: The clan hall war has been cancelled. Not enough clans have registered.
        /// </summary>
        public static SystemMessage CLANHALL_WAR_CANCELLED = new SystemMessage(1841);

        /// <summary>
        /// ID: 1842
        /// <para/>
        /// Message: $s1 wishes to summon you from $s2. Do you accept?
        /// </summary>
        public static SystemMessage S1_WISHES_TO_SUMMON_YOU_FROM_S2_DO_YOU_ACCEPT = new SystemMessage(1842);

        /// <summary>
        /// ID: 1843
        /// <para/>
        /// Message: $s1 is engaged in combat and cannot be summoned.
        /// </summary>
        public static SystemMessage S1_IS_ENGAGED_IN_COMBAT_AND_CANNOT_BE_SUMMONED = new SystemMessage(1843);

        /// <summary>
        /// ID: 1844
        /// <para/>
        /// Message: $s1 is dead at the moment and cannot be summoned.
        /// </summary>
        public static SystemMessage S1_IS_DEAD_AT_THE_MOMENT_AND_CANNOT_BE_SUMMONED = new SystemMessage(1844);

        /// <summary>
        /// ID: 1845
        /// <para/>
        /// Message: Hero weapons cannot be destroyed.
        /// </summary>
        public static SystemMessage HERO_WEAPONS_CANT_DESTROYED = new SystemMessage(1845);

        /// <summary>
        /// ID: 1846
        /// <para/>
        /// Message: You are too far away from the Strider to mount it.
        /// </summary>
        public static SystemMessage TOO_FAR_AWAY_FROM_STRIDER_TO_MOUNT = new SystemMessage(1846);

        /// <summary>
        /// ID: 1847
        /// <para/>
        /// Message: You caught a fish $s1 in length.
        /// </summary>
        public static SystemMessage CAUGHT_FISH_S1_LENGTH = new SystemMessage(1847);

        /// <summary>
        /// ID: 1848
        /// <para/>
        /// Message: Because of the size of fish caught, you will be registered in the ranking
        /// </summary>
        public static SystemMessage REGISTERED_IN_FISH_SIZE_RANKING = new SystemMessage(1848);

        /// <summary>
        /// ID: 1849
        /// <para/>
        /// Message: All of $s1 will be discarded. Would you like to continue?
        /// </summary>
        public static SystemMessage CONFIRM_DISCARD_ALL_OF_S1 = new SystemMessage(1849);

        /// <summary>
        /// ID: 1850
        /// <para/>
        /// Message: The Captain of the Order of Knights cannot be appointed.
        /// </summary>
        public static SystemMessage CAPTAIN_OF_ORDER_OF_KNIGHTS_CANNOT_BE_APPOINTED = new SystemMessage(1850);

        /// <summary>
        /// ID: 1851
        /// <para/>
        /// Message: The Captain of the Royal Guard cannot be appointed.
        /// </summary>
        public static SystemMessage CAPTAIN_OF_ROYAL_GUARD_CANNOT_BE_APPOINTED = new SystemMessage(1851);

        /// <summary>
        /// ID: 1852
        /// <para/>
        /// Message: The attempt to acquire the skill has failed because of an insufficient Clan Reputation Score.
        /// </summary>
        public static SystemMessage ACQUIRE_SKILL_FAILED_BAD_CLAN_REP_SCORE = new SystemMessage(1852);

        /// <summary>
        /// ID: 1853
        /// <para/>
        /// Message: Quantity items of the same type cannot be exchanged at the same time
        /// </summary>
        public static SystemMessage CANT_EXCHANGE_QUANTITY_ITEMS_OF_SAME_TYPE = new SystemMessage(1853);

        /// <summary>
        /// ID: 1854
        /// <para/>
        /// Message: The item was converted successfully.
        /// </summary>
        public static SystemMessage ITEM_CONVERTED_SUCCESSFULLY = new SystemMessage(1854);

        /// <summary>
        /// ID: 1855
        /// <para/>
        /// Message: Another military unit is already using that name. Please enter a different name.
        /// </summary>
        public static SystemMessage ANOTHER_MILITARY_UNIT_IS_ALREADY_USING_THAT_NAME = new SystemMessage(1855);

        /// <summary>
        /// ID: 1856
        /// <para/>
        /// Message: Since your opponent is now the owner of $s1, the Olympiad has been cancelled.
        /// </summary>
        public static SystemMessage OPPONENT_POSSESSES_S1_OLYMPIAD_CANCELLED = new SystemMessage(1856);

        /// <summary>
        /// ID: 1857
        /// <para/>
        /// Message: $s1 is the owner of $s2 and cannot participate in the Olympiad.
        /// </summary>
        public static SystemMessage S1_OWNS_S2_AND_CANNOT_PARTICIPATE_IN_OLYMPIAD = new SystemMessage(1857);

        /// <summary>
        /// ID: 1858
        /// <para/>
        /// Message: You cannot participate in the Olympiad while dead.
        /// </summary>
        public static SystemMessage CANNOT_PARTICIPATE_OLYMPIAD_WHILE_DEAD = new SystemMessage(1858);

        /// <summary>
        /// ID: 1859
        /// <para/>
        /// Message: You exceeded the quantity that can be moved at one time.
        /// </summary>
        public static SystemMessage EXCEEDED_QUANTITY_FOR_MOVED = new SystemMessage(1859);

        /// <summary>
        /// ID: 1860
        /// <para/>
        /// Message: The Clan Reputation Score is too low.
        /// </summary>
        public static SystemMessage THE_CLAN_REPUTATION_SCORE_IS_TOO_LOW = new SystemMessage(1860);

        /// <summary>
        /// ID: 1861
        /// <para/>
        /// Message: The clan's crest has been deleted.
        /// </summary>
        public static SystemMessage CLAN_CREST_HAS_BEEN_DELETED = new SystemMessage(1861);

        /// <summary>
        /// ID: 1862
        /// <para/>
        /// Message: Clan skills will now be activated since the clan's reputation score is 0 or higher.
        /// </summary>
        public static SystemMessage CLAN_SKILLS_WILL_BE_ACTIVATED_SINCE_REPUTATION_IS_0_OR_HIGHER = new SystemMessage(1862);

        /// <summary>
        /// ID: 1863
        /// <para/>
        /// Message: $s1 purchased a clan item, reducing the Clan Reputation by $s2 points.
        /// </summary>
        public static SystemMessage S1_PURCHASED_CLAN_ITEM_REDUCING_S2_REPU_POINTS = new SystemMessage(1863);

        /// <summary>
        /// ID: 1864
        /// <para/>
        /// Message: Your pet/servitor is unresponsive and will not obey any orders.
        /// </summary>
        public static SystemMessage PET_REFUSING_ORDER = new SystemMessage(1864);

        /// <summary>
        /// ID: 1865
        /// <para/>
        /// Message: Your pet/servitor is currently in a state of distress.
        /// </summary>
        public static SystemMessage PET_IN_STATE_OF_DISTRESS = new SystemMessage(1865);

        /// <summary>
        /// ID: 1866
        /// <para/>
        /// Message: MP was reduced by $s1.
        /// </summary>
        public static SystemMessage MP_REDUCED_BY_S1 = new SystemMessage(1866);

        /// <summary>
        /// ID: 1867
        /// <para/>
        /// Message: Your opponent's MP was reduced by $s1.
        /// </summary>
        public static SystemMessage YOUR_OPPONENTS_MP_WAS_REDUCED_BY_S1 = new SystemMessage(1867);

        /// <summary>
        /// ID: 1868
        /// <para/>
        /// Message: You cannot exchange an item while it is being used.
        /// </summary>
        public static SystemMessage CANNOT_EXCHANCE_USED_ITEM = new SystemMessage(1868);

        /// <summary>
        /// ID: 1869
        /// <para/>
        /// Message: $s1 has granted the Command Channel's master party the privilege of item looting.
        /// </summary>
        public static SystemMessage S1_GRANTED_MASTER_PARTY_LOOTING_RIGHTS = new SystemMessage(1869);

        /// <summary>
        /// ID: 1870
        /// <para/>
        /// Message: A Command Channel with looting rights already exists.
        /// </summary>
        public static SystemMessage COMMAND_CHANNEL_WITH_LOOTING_RIGHTS_EXISTS = new SystemMessage(1870);

        /// <summary>
        /// ID: 1871
        /// <para/>
        /// Message: Do you want to dismiss $s1 from the clan?
        /// </summary>
        public static SystemMessage CONFIRM_DISMISS_S1_FROM_CLAN = new SystemMessage(1871);

        /// <summary>
        /// ID: 1872
        /// <para/>
        /// Message: You have $s1 hour(s) and $s2 minute(s) left.
        /// </summary>
        public static SystemMessage S1_HOURS_S2_MINUTES_LEFT = new SystemMessage(1872);

        /// <summary>
        /// ID: 1873
        /// <para/>
        /// Message: There are $s1 hour(s) and $s2 minute(s) left in the fixed use time for this PC Cafe.
        /// </summary>
        public static SystemMessage S1_HOURS_S2_MINUTES_LEFT_FOR_THIS_PCCAFE = new SystemMessage(1873);

        /// <summary>
        /// ID: 1874
        /// <para/>
        /// Message: There are $s1 minute(s) left for this individual user.
        /// </summary>
        public static SystemMessage S1_MINUTES_LEFT_FOR_THIS_USER = new SystemMessage(1874);

        /// <summary>
        /// ID: 1875
        /// <para/>
        /// Message: There are $s1 minute(s) left in the fixed use time for this PC Cafe.
        /// </summary>
        public static SystemMessage S1_MINUTES_LEFT_FOR_THIS_PCCAFE = new SystemMessage(1875);

        /// <summary>
        /// ID: 1876
        /// <para/>
        /// Message: Do you want to leave $s1 clan?
        /// </summary>
        public static SystemMessage CONFIRM_LEAVE_S1_CLAN = new SystemMessage(1876);

        /// <summary>
        /// ID: 1877
        /// <para/>
        /// Message: The game will end in $s1 minutes.
        /// </summary>
        public static SystemMessage GAME_WILL_END_IN_S1_MINUTES = new SystemMessage(1877);

        /// <summary>
        /// ID: 1878
        /// <para/>
        /// Message: The game will end in $s1 seconds.
        /// </summary>
        public static SystemMessage GAME_WILL_END_IN_S1_SECONDS = new SystemMessage(1878);

        /// <summary>
        /// ID: 1879
        /// <para/>
        /// Message: In $s1 minute(s), you will be teleported outside of the game arena.
        /// </summary>
        public static SystemMessage IN_S1_MINUTES_TELEPORTED_OUTSIDE_OF_GAME_ARENA = new SystemMessage(1879);

        /// <summary>
        /// ID: 1880
        /// <para/>
        /// Message: In $s1 seconds(s), you will be teleported outside of the game arena.
        /// </summary>
        public static SystemMessage IN_S1_SECONDS_TELEPORTED_OUTSIDE_OF_GAME_ARENA = new SystemMessage(1880);

        /// <summary>
        /// ID: 1881
        /// <para/>
        /// Message: The preliminary match will begin in $s1 second(s). Prepare yourself.
        /// </summary>
        public static SystemMessage PRELIMINARY_MATCH_BEGIN_IN_S1_SECONDS = new SystemMessage(1881);

        /// <summary>
        /// ID: 1882
        /// <para/>
        /// Message: Characters cannot be created from this server.
        /// </summary>
        public static SystemMessage CHARACTERS_NOT_CREATED_FROM_THIS_SERVER = new SystemMessage(1882);

        /// <summary>
        /// ID: 1883
        /// <para/>
        /// Message: There are no offerings I own or I made a bid for.
        /// </summary>
        public static SystemMessage NO_OFFERINGS_OWN_OR_MADE_BID_FOR = new SystemMessage(1883);

        /// <summary>
        /// ID: 1884
        /// <para/>
        /// Message: Enter the PC Room coupon serial number.
        /// </summary>
        public static SystemMessage ENTER_PCROOM_SERIAL_NUMBER = new SystemMessage(1884);

        /// <summary>
        /// ID: 1885
        /// <para/>
        /// Message: This serial number cannot be entered. Please try again in minute(s).
        /// </summary>
        public static SystemMessage SERIAL_NUMBER_CANT_ENTERED = new SystemMessage(1885);

        /// <summary>
        /// ID: 1886
        /// <para/>
        /// Message: This serial has already been used.
        /// </summary>
        public static SystemMessage SERIAL_NUMBER_ALREADY_USED = new SystemMessage(1886);

        /// <summary>
        /// ID: 1887
        /// <para/>
        /// Message: Invalid serial number. Your attempt to enter the number has failed time(s). You will be allowed to make more attempt(s).
        /// </summary>
        public static SystemMessage SERIAL_NUMBER_ENTERING_FAILED = new SystemMessage(1887);

        /// <summary>
        /// ID: 1888
        /// <para/>
        /// Message: Invalid serial number. Your attempt to enter the number has failed 5 time(s). Please try again in 4 hours.
        /// </summary>
        public static SystemMessage SERIAL_NUMBER_ENTERING_FAILED_5_TIMES = new SystemMessage(1888);

        /// <summary>
        /// ID: 1889
        /// <para/>
        /// Message: Congratulations! You have received $s1.
        /// </summary>
        public static SystemMessage CONGRATULATIONS_RECEIVED_S1 = new SystemMessage(1889);

        /// <summary>
        /// ID: 1890
        /// <para/>
        /// Message: Since you have already used this coupon, you may not use this serial number.
        /// </summary>
        public static SystemMessage ALREADY_USED_COUPON_NOT_USE_SERIAL_NUMBER = new SystemMessage(1890);

        /// <summary>
        /// ID: 1891
        /// <para/>
        /// Message: You may not use items in a private store or private work shop.
        /// </summary>
        public static SystemMessage NOT_USE_ITEMS_IN_PRIVATE_STORE = new SystemMessage(1891);

        /// <summary>
        /// ID: 1892
        /// <para/>
        /// Message: The replay file for the previous version cannot be played.
        /// </summary>
        public static SystemMessage REPLAY_FILE_PREVIOUS_VERSION_CANT_PLAYED = new SystemMessage(1892);

        /// <summary>
        /// ID: 1893
        /// <para/>
        /// Message: This file cannot be replayed.
        /// </summary>
        public static SystemMessage FILE_CANT_REPLAYED = new SystemMessage(1893);

        /// <summary>
        /// ID: 1894
        /// <para/>
        /// Message: A sub-class cannot be created or changed while you are over your weight limit.
        /// </summary>
        public static SystemMessage NOT_SUBCLASS_WHILE_OVERWEIGHT = new SystemMessage(1894);

        /// <summary>
        /// ID: 1895
        /// <para/>
        /// Message: $s1 is in an area which blocks summoning.
        /// </summary>
        public static SystemMessage S1_IN_SUMMON_BLOCKING_AREA = new SystemMessage(1895);

        /// <summary>
        /// ID: 1896
        /// <para/>
        /// Message: $s1 has already been summoned.
        /// </summary>
        public static SystemMessage S1_ALREADY_SUMMONED = new SystemMessage(1896);

        /// <summary>
        /// ID: 1897
        /// <para/>
        /// Message: $s1 is required for summoning.
        /// </summary>
        public static SystemMessage S1_REQUIRED_FOR_SUMMONING = new SystemMessage(1897);

        /// <summary>
        /// ID: 1898
        /// <para/>
        /// Message: $s1 is currently trading or operating a private store and cannot be summoned.
        /// </summary>
        public static SystemMessage S1_CURRENTLY_TRADING_OR_OPERATING_PRIVATE_STORE_AND_CANNOT_BE_SUMMONED = new SystemMessage(1898);

        /// <summary>
        /// ID: 1899
        /// <para/>
        /// Message: Your target is in an area which blocks summoning.
        /// </summary>
        public static SystemMessage YOUR_TARGET_IS_IN_AN_AREA_WHICH_BLOCKS_SUMMONING = new SystemMessage(1899);

        /// <summary>
        /// ID: 1900
        /// <para/>
        /// Message: $s1 has entered the party room.
        /// </summary>
        public static SystemMessage S1_ENTERED_PARTY_ROOM = new SystemMessage(1900);

        /// <summary>
        /// ID: 1901
        /// <para/>
        /// Message: $s1 has invited you to enter the party room.
        /// </summary>
        public static SystemMessage S1_INVITED_YOU_TO_PARTY_ROOM = new SystemMessage(1901);

        /// <summary>
        /// ID: 1902
        /// <para/>
        /// Message: Incompatible item grade. This item cannot be used.
        /// </summary>
        public static SystemMessage INCOMPATIBLE_ITEM_GRADE = new SystemMessage(1902);

        /// <summary>
        /// ID: 1903
        /// <para/>
        /// Message: Those of you who have requested NCOTP should run NCOTP by using your cell phone [...]
        /// </summary>
        public static SystemMessage NCOTP = new SystemMessage(1903);

        /// <summary>
        /// ID: 1904
        /// <para/>
        /// Message: A sub-class may not be created or changed while a servitor or pet is summoned.
        /// </summary>
        public static SystemMessage CANT_SUBCLASS_WITH_SUMMONED_SERVITOR = new SystemMessage(1904);

        /// <summary>
        /// ID: 1905
        /// <para/>
        /// Message: $s2 of $s1 will be replaced with $s4 of $s3.
        /// </summary>
        public static SystemMessage S2_OF_S1_WILL_REPLACED_WITH_S4_OF_S3 = new SystemMessage(1905);

        /// <summary>
        /// ID: 1906
        /// <para/>
        /// Message: Select the combat unit
        /// </summary>
        public static SystemMessage SELECT_COMBAT_UNIT = new SystemMessage(1906);

        /// <summary>
        /// ID: 1907
        /// <para/>
        /// Message: Select the character who will [...]
        /// </summary>
        public static SystemMessage SELECT_CHARACTER_WHO_WILL = new SystemMessage(1907);

        /// <summary>
        /// ID: 1908
        /// <para/>
        /// Message: $s1 in a state which prevents summoning.
        /// </summary>
        public static SystemMessage S1_STATE_FORBIDS_SUMMONING = new SystemMessage(1908);

        /// <summary>
        /// ID: 1909
        /// <para/>
        /// Message: ==< List of Academy Graduates During the Past Week >==
        /// </summary>
        public static SystemMessage ACADEMY_LIST_HEADER = new SystemMessage(1909);

        /// <summary>
        /// ID: 1910
        /// <para/>
        /// Message: Graduates: $s1.
        /// </summary>
        public static SystemMessage GRADUATES_S1 = new SystemMessage(1910);

        /// <summary>
        /// ID: 1911
        /// <para/>
        /// Message: You cannot summon players who are currently participating in the Grand Olympiad.
        /// </summary>
        public static SystemMessage YOU_CANNOT_SUMMON_PLAYERS_WHO_ARE_IN_OLYMPIAD = new SystemMessage(1911);

        /// <summary>
        /// ID: 1912
        /// <para/>
        /// Message: Only those requesting NCOTP should make an entry into this field.
        /// </summary>
        public static SystemMessage NCOTP2 = new SystemMessage(1912);

        /// <summary>
        /// ID: 1913
        /// <para/>
        /// Message: The remaining recycle time for $s1 is $s2 minute(s).
        /// </summary>
        public static SystemMessage TIME_FOR_S1_IS_S2_MINUTES_REMAINING = new SystemMessage(1913);

        /// <summary>
        /// ID: 1914
        /// <para/>
        /// Message: The remaining recycle time for $s1 is $s2 seconds(s).
        /// </summary>
        public static SystemMessage TIME_FOR_S1_IS_S2_SECONDS_REMAINING = new SystemMessage(1914);

        /// <summary>
        /// ID: 1915
        /// <para/>
        /// Message: The game will end in $s1 second(s).
        /// </summary>
        public static SystemMessage GAME_ENDS_IN_S1_SECONDS = new SystemMessage(1915);

        /// <summary>
        /// ID: 1916
        /// <para/>
        /// Message: Your Death Penalty is now level $s1.
        /// </summary>
        public static SystemMessage DEATH_PENALTY_LEVEL_S1_ADDED = new SystemMessage(1916);

        /// <summary>
        /// ID: 1917
        /// <para/>
        /// Message: Your Death Penalty has been lifted.
        /// </summary>
        public static SystemMessage DEATH_PENALTY_LIFTED = new SystemMessage(1917);

        /// <summary>
        /// ID: 1918
        /// <para/>
        /// Message: Your pet is too high level to control.
        /// </summary>
        public static SystemMessage PET_TOO_HIGH_TO_CONTROL = new SystemMessage(1918);

        /// <summary>
        /// ID: 1919
        /// <para/>
        /// Message: The Grand Olympiad registration period has ended.
        /// </summary>
        public static SystemMessage OLYMPIAD_REGISTRATION_PERIOD_ENDED = new SystemMessage(1919);

        /// <summary>
        /// ID: 1920
        /// <para/>
        /// Message: Your account is currently inactive because you have not logged into the game for some time. You may reactivate your account by visiting the PlayNC website (http://www.plaync.com/us/support/).
        /// </summary>
        public static SystemMessage ACCOUNT_INACTIVITY = new SystemMessage(1920);

        /// <summary>
        /// ID: 1921
        /// <para/>
        /// Message: $s2 hour(s) and $s3 minute(s) have passed since $s1 has killed.
        /// </summary>
        public static SystemMessage S2_HOURS_S3_MINUTES_SINCE_S1_KILLED = new SystemMessage(1921);

        /// <summary>
        /// ID: 1922
        /// <para/>
        /// Message: Because $s1 has failed to kill for one full day, it has expired.
        /// </summary>
        public static SystemMessage S1_FAILED_KILLING_EXPIRED = new SystemMessage(1922);

        /// <summary>
        /// ID: 1923
        /// <para/>
        /// Message: Court Magician: The portal has been created!
        /// </summary>
        public static SystemMessage COURT_MAGICIAN_CREATED_PORTAL = new SystemMessage(1923);

        /// <summary>
        /// ID: 1924
        /// <para/>
        /// Message: Current Location: $s1, $s2, $s3 (Near the Primeval Isle)
        /// </summary>
        public static SystemMessage LOC_PRIMEVAL_ISLE_S1_S2_S3 = new SystemMessage(1924);

        /// <summary>
        /// ID: 1925
        /// <para/>
        /// Message: Due to the affects of the Seal of Strife, it is not possible to summon at this time.
        /// </summary>
        public static SystemMessage SEAL_OF_STRIFE_FORBIDS_SUMMONING = new SystemMessage(1925);

        /// <summary>
        /// ID: 1926
        /// <para/>
        /// Message: There is no opponent to receive your challenge for a duel.
        /// </summary>
        public static SystemMessage THERE_IS_NO_OPPONENT_TO_RECEIVE_YOUR_CHALLENGE_FOR_A_DUEL = new SystemMessage(1926);

        /// <summary>
        /// ID: 1927
        /// <para/>
        /// Message: $s1 has been challenged to a duel.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_CHALLENGED_TO_A_DUEL = new SystemMessage(1927);

        /// <summary>
        /// ID: 1928
        /// <para/>
        /// Message: $s1's party has been challenged to a duel.
        /// </summary>
        public static SystemMessage S1_PARTY_HAS_BEEN_CHALLENGED_TO_A_DUEL = new SystemMessage(1928);

        /// <summary>
        /// ID: 1929
        /// <para/>
        /// Message: $s1 has accepted your challenge to a duel. The duel will begin in a few moments.
        /// </summary>
        public static SystemMessage S1_HAS_ACCEPTED_YOUR_CHALLENGE_TO_A_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = new SystemMessage(1929);

        /// <summary>
        /// ID: 1930
        /// <para/>
        /// Message: You have accepted $s1's challenge to a duel. The duel will begin in a few moments.
        /// </summary>
        public static SystemMessage YOU_HAVE_ACCEPTED_S1_CHALLENGE_TO_A_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = new SystemMessage(1930);

        /// <summary>
        /// ID: 1931
        /// <para/>
        /// Message: $s1 has declined your challenge to a duel.
        /// </summary>
        public static SystemMessage S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL = new SystemMessage(1931);

        /// <summary>
        /// ID: 1932
        /// <para/>
        /// Message: $s1 has declined your challenge to a duel.
        /// </summary>
        public static SystemMessage S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL2 = new SystemMessage(1932);

        /// <summary>
        /// ID: 1933
        /// <para/>
        /// Message: You have accepted $s1's challenge to a party duel. The duel will begin in a few moments.
        /// </summary>
        public static SystemMessage YOU_HAVE_ACCEPTED_S1_CHALLENGE_TO_A_PARTY_DUEL_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = new SystemMessage(1933);

        /// <summary>
        /// ID: 1934
        /// <para/>
        /// Message: $s1 has accepted your challenge to duel against their party. The duel will begin in a few moments.
        /// </summary>
        public static SystemMessage S1_HAS_ACCEPTED_YOUR_CHALLENGE_TO_DUEL_AGAINST_THEIR_PARTY_THE_DUEL_WILL_BEGIN_IN_A_FEW_MOMENTS = new SystemMessage(1934);

        /// <summary>
        /// ID: 1935
        /// <para/>
        /// Message: $s1 has declined your challenge to a party duel.
        /// </summary>
        public static SystemMessage S1_HAS_DECLINED_YOUR_CHALLENGE_TO_A_PARTY_DUEL = new SystemMessage(1935);

        /// <summary>
        /// ID: 1936
        /// <para/>
        /// Message: The opposing party has declined your challenge to a duel.
        /// </summary>
        public static SystemMessage THE_OPPOSING_PARTY_HAS_DECLINED_YOUR_CHALLENGE_TO_A_DUEL = new SystemMessage(1936);

        /// <summary>
        /// ID: 1937
        /// <para/>
        /// Message: Since the person you challenged is not currently in a party, they cannot duel against your party.
        /// </summary>
        public static SystemMessage SINCE_THE_PERSON_YOU_CHALLENGED_IS_NOT_CURRENTLY_IN_A_PARTY_THEY_CANNOT_DUEL_AGAINST_YOUR_PARTY = new SystemMessage(1937);

        /// <summary>
        /// ID: 1938
        /// <para/>
        /// Message: $s1 has challenged you to a duel.
        /// </summary>
        public static SystemMessage S1_HAS_CHALLENGED_YOU_TO_A_DUEL = new SystemMessage(1938);

        /// <summary>
        /// ID: 1939
        /// <para/>
        /// Message: $s1's party has challenged your party to a duel.
        /// </summary>
        public static SystemMessage S1_PARTY_HAS_CHALLENGED_YOUR_PARTY_TO_A_DUEL = new SystemMessage(1939);

        /// <summary>
        /// ID: 1940
        /// <para/>
        /// Message: You are unable to request a duel at this time.
        /// </summary>
        public static SystemMessage YOU_ARE_UNABLE_TO_REQUEST_A_DUEL_AT_THIS_TIME = new SystemMessage(1940);

        /// <summary>
        /// ID: 1941
        /// <para/>
        /// Message: This is no suitable place to challenge anyone or party to a duel.
        /// </summary>
        public static SystemMessage NO_PLACE_FOR_DUEL = new SystemMessage(1941);

        /// <summary>
        /// ID: 1942
        /// <para/>
        /// Message: The opposing party is currently unable to accept a challenge to a duel.
        /// </summary>
        public static SystemMessage THE_OPPOSING_PARTY_IS_CURRENTLY_UNABLE_TO_ACCEPT_A_CHALLENGE_TO_A_DUEL = new SystemMessage(1942);

        /// <summary>
        /// ID: 1943
        /// <para/>
        /// Message: The opposing party is currently not in a suitable location for a duel.
        /// </summary>
        public static SystemMessage THE_OPPOSING_PARTY_IS_AT_BAD_LOCATION_FOR_A_DUEL = new SystemMessage(1943);

        /// <summary>
        /// ID: 1944
        /// <para/>
        /// Message: In a moment, you will be transported to the site where the duel will take place.
        /// </summary>
        public static SystemMessage IN_A_MOMENT_YOU_WILL_BE_TRANSPORTED_TO_THE_SITE_WHERE_THE_DUEL_WILL_TAKE_PLACE = new SystemMessage(1944);

        /// <summary>
        /// ID: 1945
        /// <para/>
        /// Message: The duel will begin in $s1 second(s).
        /// </summary>
        public static SystemMessage THE_DUEL_WILL_BEGIN_IN_S1_SECONDS = new SystemMessage(1945);

        /// <summary>
        /// ID: 1946
        /// <para/>
        /// Message: $s1 has challenged you to a duel. Will you accept?
        /// </summary>
        public static SystemMessage S1_CHALLENGED_YOU_TO_A_DUEL = new SystemMessage(1946);

        /// <summary>
        /// ID: 1947
        /// <para/>
        /// Message: $s1's party has challenged your party to a duel. Will you accept?
        /// </summary>
        public static SystemMessage S1_CHALLENGED_YOU_TO_A_PARTY_DUEL = new SystemMessage(1947);

        /// <summary>
        /// ID: 1948
        /// <para/>
        /// Message: The duel will begin in $s1 second(s).
        /// </summary>
        public static SystemMessage THE_DUEL_WILL_BEGIN_IN_S1_SECONDS2 = new SystemMessage(1948);

        /// <summary>
        /// ID: 1949
        /// <para/>
        /// Message: Let the duel begin!
        /// </summary>
        public static SystemMessage LET_THE_DUEL_BEGIN = new SystemMessage(1949);

        /// <summary>
        /// ID: 1950
        /// <para/>
        /// Message: $s1 has won the duel.
        /// </summary>
        public static SystemMessage S1_HAS_WON_THE_DUEL = new SystemMessage(1950);

        /// <summary>
        /// ID: 1951
        /// <para/>
        /// Message: $s1's party has won the duel.
        /// </summary>
        public static SystemMessage S1_PARTY_HAS_WON_THE_DUEL = new SystemMessage(1951);

        /// <summary>
        /// ID: 1952
        /// <para/>
        /// Message: The duel has ended in a tie.
        /// </summary>
        public static SystemMessage THE_DUEL_HAS_ENDED_IN_A_TIE = new SystemMessage(1952);

        /// <summary>
        /// ID: 1953
        /// <para/>
        /// Message: Since $s1 was disqualified, $s2 has won.
        /// </summary>
        public static SystemMessage SINCE_S1_WAS_DISQUALIFIED_S2_HAS_WON = new SystemMessage(1953);

        /// <summary>
        /// ID: 1954
        /// <para/>
        /// Message: Since $s1's party was disqualified, $s2's party has won.
        /// </summary>
        public static SystemMessage SINCE_S1_PARTY_WAS_DISQUALIFIED_S2_PARTY_HAS_WON = new SystemMessage(1954);

        /// <summary>
        /// ID: 1955
        /// <para/>
        /// Message: Since $s1 withdrew from the duel, $s2 has won.
        /// </summary>
        public static SystemMessage SINCE_S1_WITHDREW_FROM_THE_DUEL_S2_HAS_WON = new SystemMessage(1955);

        /// <summary>
        /// ID: 1956
        /// <para/>
        /// Message: Since $s1's party withdrew from the duel, $s2's party has won.
        /// </summary>
        public static SystemMessage SINCE_S1_PARTY_WITHDREW_FROM_THE_DUEL_S2_PARTY_HAS_WON = new SystemMessage(1956);

        /// <summary>
        /// ID: 1957
        /// <para/>
        /// Message: Select the item to be augmented.
        /// </summary>
        public static SystemMessage SELECT_THE_ITEM_TO_BE_AUGMENTED = new SystemMessage(1957);

        /// <summary>
        /// ID: 1958
        /// <para/>
        /// Message: Select the catalyst for augmentation.
        /// </summary>
        public static SystemMessage SELECT_THE_CATALYST_FOR_AUGMENTATION = new SystemMessage(1958);

        /// <summary>
        /// ID: 1959
        /// <para/>
        /// Message: Requires $s1 $s2.
        /// </summary>
        public static SystemMessage REQUIRES_S1_S2 = new SystemMessage(1959);

        /// <summary>
        /// ID: 1960
        /// <para/>
        /// Message: This is not a suitable item.
        /// </summary>
        public static SystemMessage THIS_IS_NOT_A_SUITABLE_ITEM = new SystemMessage(1960);

        /// <summary>
        /// ID: 1961
        /// <para/>
        /// Message: Gemstone quantity is incorrect.
        /// </summary>
        public static SystemMessage GEMSTONE_QUANTITY_IS_INCORRECT = new SystemMessage(1961);

        /// <summary>
        /// ID: 1962
        /// <para/>
        /// Message: The item was successfully augmented!
        /// </summary>
        public static SystemMessage THE_ITEM_WAS_SUCCESSFULLY_AUGMENTED = new SystemMessage(1962);

        /// <summary>
        /// ID : 1963
        /// <para/>
        /// Message: Select the item from which you wish to remove augmentation.
        /// </summary>
        public static SystemMessage SELECT_THE_ITEM_FROM_WHICH_YOU_WISH_TO_REMOVE_AUGMENTATION = new SystemMessage(1963);

        /// <summary>
        /// ID: 1964
        /// <para/>
        /// Message: Augmentation removal can only be done on an augmented item.
        /// </summary>
        public static SystemMessage AUGMENTATION_REMOVAL_CAN_ONLY_BE_DONE_ON_AN_AUGMENTED_ITEM = new SystemMessage(1964);

        /// <summary>
        /// ID: 1965
        /// <para/>
        /// Message: Augmentation has been successfully removed from your $s1.
        /// </summary>
        public static SystemMessage AUGMENTATION_HAS_BEEN_SUCCESSFULLY_REMOVED_FROM_YOUR_S1 = new SystemMessage(1965);

        /// <summary>
        /// ID: 1966
        /// <para/>
        /// Message: Only the clan leader may issue commands.
        /// </summary>
        public static SystemMessage ONLY_CLAN_LEADER_CAN_ISSUE_COMMANDS = new SystemMessage(1966);

        /// <summary>
        /// ID: 1967
        /// <para/>
        /// Message: The gate is firmly locked. Please try again later.
        /// </summary>
        public static SystemMessage GATE_LOCKED_TRY_AGAIN_LATER = new SystemMessage(1967);

        /// <summary>
        /// ID: 1968
        /// <para/>
        /// Message: $s1's owner.
        /// </summary>
        public static SystemMessage S1_OWNER = new SystemMessage(1968);

        /// <summary>
        /// ID: 1968
        /// <para/>
        /// Message: Area where $s1 appears.
        /// </summary>
        public static SystemMessage AREA_S1_APPEARS = new SystemMessage(1968);

        /// <summary>
        /// ID: 1970
        /// <para/>
        /// Message: Once an item is augmented, it cannot be augmented again.
        /// </summary>
        public static SystemMessage ONCE_AN_ITEM_IS_AUGMENTED_IT_CANNOT_BE_AUGMENTED_AGAIN = new SystemMessage(1970);

        /// <summary>
        /// ID: 1971
        /// <para/>
        /// Message: The level of the hardener is too high to be used.
        /// </summary>
        public static SystemMessage HARDENER_LEVEL_TOO_HIGH = new SystemMessage(1971);

        /// <summary>
        /// ID: 1972
        /// <para/>
        /// Message: You cannot augment items while a private store or private workshop is in operation.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_A_PRIVATE_STORE_OR_PRIVATE_WORKSHOP_IS_IN_OPERATION = new SystemMessage(1972);

        /// <summary>
        /// ID: 1973
        /// <para/>
        /// Message: You cannot augment items while frozen.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_FROZEN = new SystemMessage(1973);

        /// <summary>
        /// ID: 1974
        /// <para/>
        /// Message: You cannot augment items while dead.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_DEAD = new SystemMessage(1974);

        /// <summary>
        /// ID: 1975
        /// <para/>
        /// Message: You cannot augment items while engaged in trade activities.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_TRADING = new SystemMessage(1975);

        /// <summary>
        /// ID: 1976
        /// <para/>
        /// Message: You cannot augment items while paralyzed.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_PARALYZED = new SystemMessage(1976);

        /// <summary>
        /// ID: 1977
        /// <para/>
        /// Message: You cannot augment items while fishing.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_FISHING = new SystemMessage(1977);

        /// <summary>
        /// ID: 1978
        /// <para/>
        /// Message: You cannot augment items while sitting down.
        /// </summary>
        public static SystemMessage YOU_CANNOT_AUGMENT_ITEMS_WHILE_SITTING_DOWN = new SystemMessage(1978);

        /// <summary>
        /// ID: 1979
        /// <para/>
        /// Message: $s1's remaining Mana is now 10.
        /// </summary>
        public static SystemMessage S1S_REMAINING_MANA_IS_NOW_10 = new SystemMessage(1979);

        /// <summary>
        /// ID: 1980
        /// <para/>
        /// Message: $s1's remaining Mana is now 5.
        /// </summary>
        public static SystemMessage S1S_REMAINING_MANA_IS_NOW_5 = new SystemMessage(1980);

        /// <summary>
        /// ID: 1981
        /// <para/>
        /// Message: $s1's remaining Mana is now 1. It will disappear soon.
        /// </summary>
        public static SystemMessage S1S_REMAINING_MANA_IS_NOW_1 = new SystemMessage(1981);

        /// <summary>
        /// ID: 1982
        /// <para/>
        /// Message: $s1's remaining Mana is now 0, and the item has disappeared.
        /// </summary>
        public static SystemMessage S1S_REMAINING_MANA_IS_NOW_0 = new SystemMessage(1982);

        /// <summary>
        /// ID: 1984
        /// <para/>
        /// Message: Press the Augment button to begin.
        /// </summary>
        public static SystemMessage PRESS_THE_AUGMENT_BUTTON_TO_BEGIN = new SystemMessage(1984);

        /// <summary>
        /// ID: 1985
        /// <para/>
        /// Message: $s1's drop area ($s2)
        /// </summary>
        public static SystemMessage S1_DROP_AREA_S2 = new SystemMessage(1985);

        /// <summary>
        /// ID: 1986
        /// <para/>
        /// Message: $s1's owner ($s2)
        /// </summary>
        public static SystemMessage S1_OWNER_S2 = new SystemMessage(1986);

        /// <summary>
        /// ID: 1987
        /// <para/>
        /// Message: $s1
        /// </summary>
        public static SystemMessage S1 = new SystemMessage(1987);

        /// <summary>
        /// ID: 1988
        /// <para/>
        /// Message: The ferry has arrived at Primeval Isle.
        /// </summary>
        public static SystemMessage FERRY_ARRIVED_AT_PRIMEVAL = new SystemMessage(1988);

        /// <summary>
        /// ID: 1989
        /// <para/>
        /// Message: The ferry will leave for Rune Harbor after anchoring for three minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_FOR_RUNE_3_MINUTES = new SystemMessage(1989);

        /// <summary>
        /// ID: 1990
        /// <para/>
        /// Message: The ferry is now departing Primeval Isle for Rune Harbor.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_PRIMEVAL_FOR_RUNE_NOW = new SystemMessage(1990);

        /// <summary>
        /// ID: 1991
        /// <para/>
        /// Message: The ferry will leave for Primeval Isle after anchoring for three minutes.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_FOR_PRIMEVAL_3_MINUTES = new SystemMessage(1991);

        /// <summary>
        /// ID: 1992
        /// <para/>
        /// Message: The ferry is now departing Rune Harbor for Primeval Isle.
        /// </summary>
        public static SystemMessage FERRY_LEAVING_RUNE_FOR_PRIMEVAL_NOW = new SystemMessage(1992);

        /// <summary>
        /// ID: 1993
        /// <para/>
        /// Message: The ferry from Primeval Isle to Rune Harbor has been delayed.
        /// </summary>
        public static SystemMessage FERRY_FROM_PRIMEVAL_TO_RUNE_DELAYED = new SystemMessage(1993);

        /// <summary>
        /// ID: 1994
        /// <para/>
        /// Message: The ferry from Rune Harbor to Primeval Isle has been delayed.
        /// </summary>
        public static SystemMessage FERRY_FROM_RUNE_TO_PRIMEVAL_DELAYED = new SystemMessage(1994);

        /// <summary>
        /// ID: 1995
        /// <para/>
        /// Message: $s1 channel filtering option
        /// </summary>
        public static SystemMessage S1_CHANNEL_FILTER_OPTION = new SystemMessage(1995);

        /// <summary>
        /// ID: 1996
        /// <para/>
        /// Message: The attack has been blocked.
        /// </summary>
        public static SystemMessage ATTACK_WAS_BLOCKED = new SystemMessage(1996);

        /// <summary>
        /// ID: 1997
        /// <para/>
        /// Message: $s1 is performing a counterattack.
        /// </summary>
        public static SystemMessage S1_PERFORMING_COUNTERATTACK = new SystemMessage(1997);

        /// <summary>
        /// ID: 1998
        /// <para/>
        /// Message: You countered $s1's attack.
        /// </summary>
        public static SystemMessage COUNTERED_S1_ATTACK = new SystemMessage(1998);

        /// <summary>
        /// ID: 1999
        /// <para/>
        /// Message: $s1 dodges the attack.
        /// </summary>
        public static SystemMessage S1_DODGES_ATTACK = new SystemMessage(1999);

        /// <summary>
        /// ID: 2000
        /// <para/>
        /// Message: You have avoided $s1's attack.
        /// </summary>
        public static SystemMessage AVOIDED_S1_ATTACK2 = new SystemMessage(2000);

        /// <summary>
        /// ID: 2001
        /// <para/>
        /// Message: Augmentation failed due to inappropriate conditions.
        /// </summary>
        public static SystemMessage AUGMENTATION_FAILED_DUE_TO_INAPPROPRIATE_CONDITIONS = new SystemMessage(2001);

        /// <summary>
        /// ID: 2002
        /// <para/>
        /// Message: Trap failed.
        /// </summary>
        public static SystemMessage TRAP_FAILED = new SystemMessage(2002);

        /// <summary>
        /// ID: 2003
        /// <para/>
        /// Message: You obtained an ordinary material.
        /// </summary>
        public static SystemMessage OBTAINED_ORDINARY_MATERIAL = new SystemMessage(2003);

        /// <summary>
        /// ID: 2004
        /// <para/>
        /// Message: You obtained a rare material.
        /// </summary>
        public static SystemMessage OBTAINED_RATE_MATERIAL = new SystemMessage(2004);

        /// <summary>
        /// ID: 2005
        /// <para/>
        /// Message: You obtained a unique material.
        /// </summary>
        public static SystemMessage OBTAINED_UNIQUE_MATERIAL = new SystemMessage(2005);

        /// <summary>
        /// ID: 2006
        /// <para/>
        /// Message: You obtained the only material of this kind.
        /// </summary>
        public static SystemMessage OBTAINED_ONLY_MATERIAL = new SystemMessage(2006);

        /// <summary>
        /// ID: 2007
        /// <para/>
        /// Message: Please enter the recipient's name.
        /// </summary>
        public static SystemMessage ENTER_RECIPIENTS_NAME = new SystemMessage(2007);

        /// <summary>
        /// ID: 2008
        /// <para/>
        /// Message: Please enter the text.
        /// </summary>
        public static SystemMessage ENTER_TEXT = new SystemMessage(2008);

        /// <summary>
        /// ID: 2009
        /// <para/>
        /// Message: You cannot exceed 1500 characters.
        /// </summary>
        public static SystemMessage CANT_EXCEED_1500_CHARACTERS = new SystemMessage(2009);

        /// <summary>
        /// ID: 2009
        /// <para/>
        /// Message: $s2 $s1
        /// </summary>
        public static SystemMessage S2_S1 = new SystemMessage(2009);

        /// <summary>
        /// ID: 2011
        /// <para/>
        /// Message: The augmented item cannot be discarded.
        /// </summary>
        public static SystemMessage AUGMENTED_ITEM_CANNOT_BE_DISCARDED = new SystemMessage(2011);

        /// <summary>
        /// ID: 2012
        /// <para/>
        /// Message: $s1 has been activated.
        /// </summary>
        public static SystemMessage S1_HAS_BEEN_ACTIVATED = new SystemMessage(2012);

        /// <summary>
        /// ID: 2013
        /// <para/>
        /// Message: Your seed or remaining purchase amount is inadequate.
        /// </summary>
        public static SystemMessage YOUR_SEED_OR_REMAINING_PURCHASE_AMOUNT_IS_INADEQUATE = new SystemMessage(2013);

        /// <summary>
        /// ID: 2014
        /// <para/>
        /// Message: You cannot proceed because the manor cannot accept any more crops. All crops have been returned and no adena withdrawn.
        /// </summary>
        public static SystemMessage MANOR_CANT_ACCEPT_MORE_CROPS = new SystemMessage(2014);

        /// <summary>
        /// ID: 2015
        /// <para/>
        /// Message: A skill is ready to be used again.
        /// </summary>
        public static SystemMessage SKILL_READY_TO_USE_AGAIN = new SystemMessage(2015);

        /// <summary>
        /// ID: 2016
        /// <para/>
        /// Message: A skill is ready to be used again but its re-use counter time has increased.
        /// </summary>
        public static SystemMessage SKILL_READY_TO_USE_AGAIN_BUT_TIME_INCREASED = new SystemMessage(2016);

        /// <summary>
        /// ID: 2017
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is currently engaged in a private store or manufacture.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_ENGAGED_IN_A_PRIVATE_STORE_OR_MANUFACTURE = new SystemMessage(2017);

        /// <summary>
        /// ID: 2018
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is currently fishing.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_FISHING = new SystemMessage(2018);

        /// <summary>
        /// ID: 2019
        /// <para/>
        /// Message: $s1 cannot duel because $s1's HP or MP is below 50%.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_HP_OR_MP_IS_BELOW_50_PERCENT = new SystemMessage(2019);

        /// <summary>
        /// ID: 2020
        /// <para/>
        /// Message: $s1 cannot make a challenge to a duel because $s1 is currently in a duel-prohibited area (Peaceful Zone / Seven Signs Zone / Near Water / Restart Prohibited Area).
        /// </summary>
        public static SystemMessage S1_CANNOT_MAKE_A_CHALLANGE_TO_A_DUEL_BECAUSE_S1_IS_CURRENTLY_IN_A_DUEL_PROHIBITED_AREA = new SystemMessage(2020);

        /// <summary>
        /// ID: 2021
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is currently engaged in battle.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_ENGAGED_IN_BATTLE = new SystemMessage(2021);

        /// <summary>
        /// ID: 2022
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is already engaged in a duel.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_ALREADY_ENGAGED_IN_A_DUEL = new SystemMessage(2022);

        /// <summary>
        /// ID: 2023
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is in a chaotic state.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_IN_A_CHAOTIC_STATE = new SystemMessage(2023);

        /// <summary>
        /// ID: 2024
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is participating in the Olympiad.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_THE_OLYMPIAD = new SystemMessage(2024);

        /// <summary>
        /// ID: 2025
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is participating in a clan hall war.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_A_CLAN_HALL_WAR = new SystemMessage(2025);

        /// <summary>
        /// ID: 2026
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is participating in a siege war.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_PARTICIPATING_IN_A_SIEGE_WAR = new SystemMessage(2026);

        /// <summary>
        /// ID: 2027
        /// <para/>
        /// Message: $s1 cannot duel because $s1 is currently riding a boat or strider.
        /// </summary>
        public static SystemMessage S1_CANNOT_DUEL_BECAUSE_S1_IS_CURRENTLY_RIDING_A_BOAT_WYVERN_OR_STRIDER = new SystemMessage(2027);

        /// <summary>
        /// ID: 2028
        /// <para/>
        /// Message: $s1 cannot receive a duel challenge because $s1 is too far away.
        /// </summary>
        public static SystemMessage S1_CANNOT_RECEIVE_A_DUEL_CHALLENGE_BECAUSE_S1_IS_TOO_FAR_AWAY = new SystemMessage(2028);

        /// <summary>
        /// ID: 2029
        /// <para/>
        /// Message: $s1 is currently teleporting and cannot participate in the Olympiad.
        /// </summary>
        public static SystemMessage S1_CANNOT_PARTICIPATE_IN_OLYMPIAD_DURING_TELEPORT = new SystemMessage(2029);

        /// <summary>
        /// ID: 2030
        /// <para/>
        /// Message: You are currently logging in.
        /// </summary>
        public static SystemMessage CURRENTLY_LOGGING_IN = new SystemMessage(2030);

        /// <summary>
        /// ID: 2031
        /// <para/>
        /// Message: Please wait a moment.
        /// </summary>
        public static SystemMessage PLEASE_WAIT_A_MOMENT = new SystemMessage(2031);
    }
}
