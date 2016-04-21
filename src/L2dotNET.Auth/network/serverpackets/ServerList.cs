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

        

        /* NA freya release
         * 0x04 0x02 0x00 0x0f 0x40 0x19 0x25 0x84 0x61 0x1e 0x00 0x00 0x00 0x01 0xaf 0x05 0x70 0x17 0x01 0x00 0x00 0x00 0x00 0x00 0x10 0xce 0x7f 0x91 0xa2 0x61 0x1e 0x00 0x00 0x00 0x01 0x33 0x05 0x70 0x17 0x01 0x00 0x00 0x00 0x00 0x00 0x16 0x53 0xd6 0x8e 0x5f 0x93 0xdd 0x5b 0xde 0xc0 0x93 0x37 0x5b 0xfc 0xb5 0xa0 0x2c 0x2d 0x3f
         * 
         * 0x04
         * 0x02
         * 0x00
         * {
         *   0x0f id
         *   0x40 0x19 0x25 0x84 address
         *   0x61 0x1e 0x00 0x00 port
         *   0x00
         *   0x01
         *   0xaf 0x05 curp
         *   0x70 0x17 maxp
         *   0x01 online
         *   0x00 0x00 0x00 0x00 bits
         *   0x00
         * }
         * */
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
                writeC(1);
                writeH(server.CurPlayers);
                writeH(server.MaxPlayers);

                writeC(server.connected);

                int bits = 0x40;
                if (server.testMode)
                    bits |= 0x04;

                writeD(bits);
                writeC(0);
            }
        }
    }
}
