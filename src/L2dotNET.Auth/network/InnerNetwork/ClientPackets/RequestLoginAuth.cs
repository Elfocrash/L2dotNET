using log4net;
using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestLoginAuth
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestLoginAuth));

        private readonly short port;
        private readonly string host;
        private readonly string info;
        private readonly string code;
        private int curp;
        private readonly short maxp;
        private readonly byte gmonly;
        private readonly byte test;
        private readonly ServerThread thread;

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
                log.Error($"Code '{code}' for server was not found. Closing");
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
            log.Info($"AuthThread: Server #{server.Id} connected");
        }
    }
}