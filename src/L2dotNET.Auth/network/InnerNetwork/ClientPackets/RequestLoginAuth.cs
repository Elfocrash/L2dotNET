using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;
using L2dotNET.Network;
using log4net;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestLoginAuth
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestLoginAuth));

        private short port;
        private string host;
        private string info;
        private string code;
        private int curp;
        private short maxp;
        private byte gmonly;
        private byte test;
        private ServerThread thread;
        public RequestLoginAuth(Packet p, ServerThread server)
        {
            thread = server;
            port = p.ReadShort();
            host = p.ReadString().ToLower();
            info = p.ReadString().ToLower();
            code = p.ReadString().ToLower();
            curp = p.ReadInt();
            maxp = p.ReadShort();
            gmonly = p.ReadByte();
            test = p.ReadByte();
        }

        public void RunImpl()
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
                log.Error($"Code '{ code }' for server was not found. Closing");
                thread.close(ServerLoginFail.ToPacket("Code Error"));
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

            thread.Send(ServerLoginOk.ToPacket());
            log.Info($"AuthThread: Server #{ server.Id } connected");
        }
    }
}
