namespace L2dotNET.GameService.network.l2send
{
    class CharCreateFail : GameServerNetworkPacket
    {
        public enum CharCreateFailReason
        {
            ///<summary>Your character creation has failed.</summary>
            CREATION_FAILED = 0,
            ///<summary>You cannot create another character. Please delete the existing character and try again.</summary>
            TOO_MANY_CHARS_ON_ACCOUNT = 1,
            ///<summary>This name already exists.</summary>
            NAME_EXISTS = 2,
            ///<summary>Your title cannot exceed 16 characters in length. Please try again.</summary>
            TOO_LONG_16_CHARS = 3,
            ///<summary>Incorrect name. Please try again.</summary>
            INCORRECT_NAME = 4,
            ///<summary>Characters cannot be created from this server.</summary>
            CHAR_CREATION_BLOCKED = 5,
            ///<summary>Unable to create character. You are unable to create a new character on the selected server. A restriction is in place which restricts users from creating characters on different servers where no previous character exists. Please choose another server.</summary>
            CREATION_RESTRICTION = 6
        }

        CharCreateFailReason reason;

        public CharCreateFail(CharCreateFailReason reason)
        {   
            this.reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x1a);
            writeD((int)reason);
        }
    }
}
