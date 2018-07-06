using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets;
using L2dotNET.Network;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.LoginService.Network.InnerNetwork.RequestPackets
{
    class RequestLoginAuth : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ServerThread _thread;
        private readonly short _port;
        private readonly string _host;
        private readonly string _info;
        private readonly string _serverKey;
        private readonly int _currentPlayers;
        private readonly short _maxPlayers;
        private readonly byte _gmonly;
        private readonly byte _test;

        public RequestLoginAuth(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _port = p.ReadShort();
            _host = p.ReadString().ToLower();
            _info = p.ReadString().ToLower();
            _serverKey = p.ReadString();
            _currentPlayers = p.ReadInt();
            _maxPlayers = p.ReadShort();
            _gmonly = p.ReadByte();
            _test = p.ReadByte();
        }

        public override async Task RunImpl()
        {
            L2Server server = LoginServer.ServiceProvider.GetService<ServerThreadPool>()
                .Servers.FirstOrDefault(x => x.ServerKey == _serverKey);

            if (server == null)
            {
                Log.Error($"Server with id '{_serverKey}' was not found. Closing");
                _thread.Close(ServerLoginFail.ToPacket("ServerId Error"));
                return;
            }

            server.Thread = _thread;
            _thread.ServerId = server.ServerId;
            _thread.Info = _info;
            _thread.Wan = _host;
            _thread.Port = _port;
            _thread.MaxPlayers = _maxPlayers;
            _thread.GmOnly = _gmonly == 1;
            _thread.TestMode = _test == 1;
            _thread.Connected = true;
            _thread.Send(ServerLoginOk.ToPacket());

            Log.Info($"Server #{server.ServerId} connected");
        }
    }
}