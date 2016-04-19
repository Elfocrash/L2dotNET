using L2dotNET.Auth.basetemplate;

namespace L2dotNET.Auth.network.serverpackets_gs
{
    class PleaseAcceptPlayer : SendServerPacket
    {
        private L2Account account;
        private string time;
        public PleaseAcceptPlayer(L2Account account, string time)
        {
            this.account = account;
            this.time = time;
        }

        protected internal override void write()
        {
            writeC(0xA7);
            writeS(account.name.ToLower());
            writeS(account.type.ToString());
            writeS(account.timeend);
            writeC(account.premium ? 1 : 0);
            writeQ(account.points);
            writeS(time);
        }
    }
}
