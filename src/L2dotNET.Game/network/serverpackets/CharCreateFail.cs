
namespace L2dotNET.GameService.network.l2send
{
    class CharCreateFail : GameServerNetworkPacket
    {
        public enum CharCreateFailReason
        {
            JUST_FAILED = 0,
            TOO_MANY_CHARS_ON_ACCOUNT = 1,
            NAME_EXISTS = 2,
            TOO_LONG_16_CHARS = 3,
            INCORRECT_NAME = 4,
            CHAR_CREATION_BLOCKED = 5,
            CREATION_RESTRICTION = 6
        }

        /*
            0 your character creation has failed
            1 You cannot create another character. Please delete the existing character and try again.
            2 This name already exists.
            3 Your title cannot exceed 16 characters in length.  Please try again.
            4 Incorrect name. please try again
            5 characters cannot be created from this server
            6 Unable to create character. You are unable to create a new character on the selected server. A restriction is in place which restricts users from creating characters on different servers where no previous character exists. Please choose another server.
         */

        CharCreateFailReason _reason;
        public CharCreateFail(GameClient client, CharCreateFailReason reason)
        {
            _reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x1a);
            writeD((int)_reason);
        }
    }
}
