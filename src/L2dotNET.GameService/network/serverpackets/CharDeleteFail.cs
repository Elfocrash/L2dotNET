namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharDeleteFail : GameServerNetworkPacket
    {
        public enum CharDeleteFailReason
        {
            ///<summary>You have failed to delete the character.</summary>
            DeletionFailed = 1,
            ///<summary>You may not delete a clan member. Withdraw from the clan first and try again.</summary>
            YouMayNotDeleteClanMember = 2,
            ///<summary>Clan leaders may not be deleted. Dissolve the clan first and try again.</summary>
            ClanLeadersMayNotBeDeleted = 3
        }

        private readonly CharDeleteFailReason _reason;

        public CharDeleteFail(CharDeleteFailReason reason)
        {
            this._reason = reason;
        }

        protected internal override void Write()
        {
            WriteC(0x24);
            WriteD((int)_reason);
        }
    }
}