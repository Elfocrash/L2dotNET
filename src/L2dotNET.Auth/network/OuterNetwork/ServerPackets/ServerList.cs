using System.Collections.Generic;
using L2dotNET.LoginService.gscommunication;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    public class ServerList : SendBasePacket
    {
        List<L2Server> servers;
        public ServerList(LoginClient Client)
        {
            base.makeme(Client);
            servers = ServerThreadPool.Instance.servers;
        }

        protected internal override void write()
        {
            writeC(0x04);
            writeC((byte)servers.Count);
            writeC((byte)lc.ActiveAccount.LastServer);

            foreach (L2Server server in servers)
            {
                writeC(server.Id);
                writeB(server.GetIP(lc));
                writeD(server.Port);
                writeC(0);
                writeC(1);// pvp?
                writeH(server.CurrentPlayers);
                writeH(server.MaxPlayers);

                writeC(server.Connected);// status

                int bits = 0x40;
                if (server.TestMode)
                    bits |= 0x04;

                writeD(bits);
                writeC(0); //brackets
            }
        }
    }
}
