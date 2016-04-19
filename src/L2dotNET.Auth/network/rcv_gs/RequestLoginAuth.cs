using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.network.serverpackets_gs;

namespace L2dotNET.Auth.network.rcv_gs
{
    class RequestLoginAuth : ReceiveServerPacket
    {
        private short port;
        private string host;
        private string info;
        private string code;
        private int curp;
        private short maxp;
        private byte gmonly;
        private byte test;

        public RequestLoginAuth(ServerThread server, byte[] data)
        {
            base.makeme(server, data);
        }

        public override void read()
        {
            port = readH();
            host = readS();
            info = readS();
            code = readS();
            curp = readD();
            maxp = readH();
            gmonly = readC();
            test = readC();
        }

        public override void run()
        {
            L2Server server = null;
            foreach (L2Server srv in ServerThreadPool.getInstance().servers)
            {
                if (srv.code == code)
                {
                    srv.thread = thread;
                    server = srv;
                    break;
                }
            }

            if (server == null)
            {
                CLogger.error("code '" + code + "' for server was not found. closing");
                thread.close(new ServerLoginFail("wrong code"));
                return;
            }

            thread.id = server.id;
            thread.info = info;
            thread.wan = host;
            thread.port = port;
            thread.maxp = maxp;
            thread.gmonly = gmonly == 1;
            thread.testMode = test == 1;
            thread.connected = true;

            thread.sendPacket(new ServerLoginOk());
            CLogger.extra_info("AuthThread: Server #"+server.id+" connected");
        }
    }
}
