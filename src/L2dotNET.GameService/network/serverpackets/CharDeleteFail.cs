using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharDeleteFail : GameserverPacket
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
            _reason = reason;
        }

        public override void Write()
        {
            WriteByte(0x24);
            WriteInt((int)_reason);
        }
    }
}