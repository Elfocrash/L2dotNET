using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestLoginAuth : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ServerThread _thread;
        private readonly short _port;
        private readonly string _host;
        private readonly string _info;
        private readonly string _code;
        private readonly int _curp;
        private readonly short _maxp;
        private readonly byte _gmonly;
        private readonly byte _test;

        public RequestLoginAuth(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                // TODO: Fix _code
                L2Server server = LoginServer.ServiceProvider.GetService<ServerThreadPool>().Servers.FirstOrDefault();

                if (server == null)
                {
                    Log.Error($"Code '{_code}' for server was not found. Closing");
                    _thread.Close(ServerLoginFail.ToPacket("Code Error"));
                    return;
                }

                server.Thread = _thread;
                _thread.Id = server.Id;
                _thread.Info = _info;
                _thread.Wan = _host;
                _thread.Port = _port;
                _thread.Maxp = _maxp;
                _thread.GmOnly = _gmonly == 1;
                _thread.TestMode = _test == 1;
                _thread.Connected = true;
                _thread.Send(ServerLoginOk.ToPacket());

                Log.Info($"Server #{server.Id} connected");
            });
        }
    }
}