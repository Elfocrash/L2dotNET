
namespace L2dotNET.Auth.network.serverpackets_gs
{
    class PleaseKickAccount : SendServerPacket
    {
        private string account;
        public PleaseKickAccount(string account)
        {
            this.account = account;
        }

        protected internal override void write()
        {
            writeC(0xA8);
            writeS(account.ToLower());
        }
    }
}
