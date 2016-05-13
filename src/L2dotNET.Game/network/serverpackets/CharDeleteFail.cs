namespace L2dotNET.GameService.network.l2send
{
    class CharDeleteFail : GameServerNetworkPacket
    {
        public enum CharDeleteFailReason
        {
            DELETION_FAILED = 1,
			YOU_MAY_NOT_DELETE_CLAN_MEMBER = 2,
			CLAN_LEADERS_MAY_NOT_BE_DELETED = 3
        }

        CharDeleteFailReason _reason;

        public CharDeleteFail(CharDeleteFailReason reason)
        {   
            _reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x24);
            writeD((int)_reason);
        }
    }
}
