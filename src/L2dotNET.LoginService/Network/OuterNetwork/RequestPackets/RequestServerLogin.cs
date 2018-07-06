using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network.Enums;
using L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets;
using L2dotNET.Network;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.LoginService.Network.OuterNetwork.RequestPackets
{
    class RequestServerLogin : PacketBase
    {
        private readonly LoginClient _client;
        private readonly int _loginOkID1;
        private readonly int _loginOkID2;
        private readonly byte _serverId;

        public RequestServerLogin(IServiceProvider serviceProvider, Packet p, LoginClient client) : base(serviceProvider)
        {
            _client = client;
            _loginOkID1 = p.ReadInt();
            _loginOkID2 = p.ReadInt();
            _serverId = p.ReadByte();
        }

        public override async Task RunImpl()
        {
            if (_client.State != LoginClientState.AuthedLogin)
            {
                await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (_client.Key.LoginOkId1 != _loginOkID1 || _client.Key.LoginOkId2 != _loginOkID2)
            {
                await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            L2Server server = LoginServer.ServiceProvider.GetService<ServerThreadPool>().Get(_serverId);
            if (server == null)
            {
                await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            if (!server.Connected)
            {
                await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonServerMaintenance));
                _client.Close();
                return;
            }

            await server.Thread.SendPlayer(_client);
            _client.SendAsync(PlayOk.ToPacket(_client));
        }
    }
}