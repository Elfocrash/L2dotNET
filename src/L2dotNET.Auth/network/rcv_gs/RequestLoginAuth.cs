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
            foreach (L2Server srv in ServerThreadPool.Instance.servers)
            {
                if (srv.Code == code)
                {
                    srv.Thread = thread;
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

            thread.Id = server.Id;
            thread.Info = info;
            thread.Wan = host;
            thread.Port = port;
            thread.Maxp = maxp;
            thread.GmOnly = gmonly == 1;
            thread.TestMode = test == 1;
            thread.Connected = true;

            thread.SendPacket(new ServerLoginOk());
            CLogger.extra_info($"AuthThread: Server #{server.Id} connected");
        }
    }
}
