
namespace L2dotNET.LoginService.Network.OuterNetwork
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
