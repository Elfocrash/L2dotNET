using System.Linq;
using log4net;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestLoginAuth : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestLoginAuth));

        private readonly short _port;
        private readonly string _host;
        private readonly string _info;
        private readonly string _code;
        private int _curp;
        private readonly short _maxp;
        private readonly byte _gmonly;
        private readonly byte _test;
        private readonly ServerThread _thread;

        public RequestLoginAuth(Packet p, ServerThread server)
        {
            _thread = server;
            _port = p.ReadShort();
            _host = p.ReadString().ToLower();
            _info = p.ReadString().ToLower();
            _code = p.ReadString().ToLower();
            _curp = p.ReadInt();
            _maxp = p.ReadShort();
            _gmonly = p.ReadByte();
            _test = p.ReadByte();
        }

        public override void RunImpl()
        {
            L2Server server = null;
            foreach (L2Server srv in ServerThreadPool.Instance.Servers.Where(srv => srv.Code == _code))
            {
                srv.Thread = _thread;
                server = srv;
                break;
            }

            if (server == null)
            {
                Log.Error($"Code '{_code}' for server was not found. Closing");
                _thread.Close(ServerLoginFail.ToPacket("Code Error"));
                return;
            }

            _thread.Id = server.Id;
            _thread.Info = _info;
            _thread.Wan = _host;
            _thread.Port = _port;
            _thread.Maxp = _maxp;
            _thread.GmOnly = _gmonly == 1;
            _thread.TestMode = _test == 1;
            _thread.Connected = true;

            _thread.Send(ServerLoginOk.ToPacket());
            Log.Info($"AuthThread: Server #{server.Id} connected");
        }
    }
}