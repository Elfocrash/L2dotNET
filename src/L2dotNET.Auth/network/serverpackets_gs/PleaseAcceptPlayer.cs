using L2dotNET.Auth.basetemplate;
using L2dotNET.Models;

namespace L2dotNET.Auth.network.serverpackets_gs
{
    class PleaseAcceptPlayer : SendServerPacket
    {
        private AccountModel account;
        private string time;
        public PleaseAcceptPlayer(AccountModel account, string time)
        {
            this.account = account;
            this.time = time;
        }

        protected internal override void write()
        {
            //writeC(0xA7);
            //writeS(account..ToLower());
            //writeS(account.type.ToString());
            //writeS(account.timeend);
            //writeC(account.premium ? 1 : 0);
            //writeQ(account.points);
            //writeS(time);
        }
    }
}
