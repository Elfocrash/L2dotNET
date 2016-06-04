namespace L2dotNET.GameService.network.l2send
{
    class CharDeleteFail : GameServerNetworkPacket
    {
        public enum CharDeleteFailReason
        {   
            ///<summary>You have failed to delete the character.</summary>
            DELETION_FAILED = 1,
            ///<summary>You may not delete a clan member. Withdraw from the clan first and try again..</summary>
			YOU_MAY_NOT_DELETE_CLAN_MEMBER = 2,
            ///<summary>Clan leaders may not be deleted. Dissolve the clan first and try again.</summary>
			CLAN_LEADERS_MAY_NOT_BE_DELETED = 3
        }

        private CharDeleteFailReason reason;

        public CharDeleteFail(CharDeleteFailReason reason)
        {   
            this.reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x24);
            writeD((int)reason);
        }
    }
}
