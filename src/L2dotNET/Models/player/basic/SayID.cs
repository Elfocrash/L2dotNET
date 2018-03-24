namespace L2dotNET.Models.player.basic
{
    public class SayId
    {
        public static byte MaxId = (byte)SayIDList.CHAT_MPCC_ROOM;

        public static SayIDList getType(byte id)
        {
            switch (id)
            {
                case 1:
                    return SayIDList.CHAT_SHOUT;
                case 2:
                    return SayIDList.CHAT_TELL;
                case 3:
                    return SayIDList.CHAT_PARTY;
                case 4:
                    return SayIDList.CHAT_CLAN;
                case 5:
                    return SayIDList.CHAT_SYSTEM;
                case 6:
                    return SayIDList.CHAT_USER_PET;
                case 7:
                    return SayIDList.CHAT_GM_PET;
                case 8:
                    return SayIDList.CHAT_MARKET;
                case 9:
                    return SayIDList.CHAT_ALLIANCE;
                case 10:
                    return SayIDList.CHAT_ANNOUNCE;
                case 11:
                    return SayIDList.CHAT_CUSTOM;
                case 12:
                    return SayIDList.CHAT_L2_FRIEND;
                case 13:
                    return SayIDList.CHAT_MSN_CHAT;
                case 14:
                    return SayIDList.CHAT_PARTY_ROOM_CHAT;
                case 15:
                    return SayIDList.CHAT_COMMANDER_CHAT;
                case 16:
                    return SayIDList.CHAT_INTER_PARTYMASTER_CHAT;
                case 17:
                    return SayIDList.CHAT_HERO;
                case 18:
                    return SayIDList.CHAT_CRITICAL_ANNOUNCE;
                case 19:
                    return SayIDList.CHAT_SCREEN_ANNOUNCE;
                case 20:
                    return SayIDList.CHAT_DOMINIONWAR;
                case 21:
                    return SayIDList.CHAT_MPCC_ROOM;
            }

            return SayIDList.CHAT_NORMAL;
        }
    }
}