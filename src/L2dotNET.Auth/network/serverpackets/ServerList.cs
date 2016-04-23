using System.Collections.Generic;
using L2dotNET.Auth.gscommunication;

namespace L2dotNET.Auth.serverpackets
{
    public class ServerList : SendBasePacket
    {
        List<L2Server> servers;
        public ServerList(LoginClient Client)
        {
            base.makeme(Client);
            servers = ServerThreadPool.getInstance().servers;
        }

        protected internal override void write()
        {
            writeC(0x04);
            writeC((byte)servers.Count);
            writeC((byte)lc.activeAccount.LastServer);

            foreach (L2Server server in servers)
            {
                writeC(server.id);
                writeB(server.GetIP(lc));
                writeD(server.Port);
                writeC(0);
                writeC(1);// pvp?
                writeH(server.CurPlayers);
                writeH(server.MaxPlayers);

                writeC(server.connected);// status

                int bits = 0x40;
                if (server.testMode)
                    bits |= 0x04;

                writeD(bits);
                writeC(0); //brackets
            }
        }
    }
}
