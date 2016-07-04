namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharCreateFail : GameServerNetworkPacket
    {
        public enum CharCreateFailReason
        {
            ///<summary>Your character creation has failed.</summary>
            CreationFailed = 0,
            ///<summary>You cannot create another character. Please delete the existing character and try again.</summary>
            TooManyCharsOnAccount = 1,
            ///<summary>This name already exists.</summary>
            NameExists = 2,
            ///<summary>Your title cannot exceed 16 characters in length. Please try again.</summary>
            TooLong16Chars = 3,
            ///<summary>Incorrect name. Please try again.</summary>
            IncorrectName = 4,
            ///<summary>Characters cannot be created from this server.</summary>
            CharCreationBlocked = 5,
            ///<summary>Unable to create character. You are unable to create a new character on the selected server. A restriction is in place which restricts users from creating characters on different servers where no previous character exists. Please choose another server.</summary>
            CreationRestriction = 6
        }

        private readonly CharCreateFailReason _reason;

        public CharCreateFail(CharCreateFailReason reason)
        {
            this._reason = reason;
        }

        protected internal override void Write()
        {
            WriteC(0x1a);
            WriteD((int)_reason);
        }
    }
}