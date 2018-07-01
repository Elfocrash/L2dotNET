﻿using System;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network.InnerNetwork.RequestPackets
{
    class RequestPlayerInGame : PacketBase
    {
        private readonly ServerThread _thread;
        private readonly string _account;
        private readonly byte _status;

        public RequestPlayerInGame(IServiceProvider serviceProvider, Packet p, ServerThread server) : base(serviceProvider)
        {
            _thread = server;
            _account = p.ReadString();
            _status = p.ReadByte();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                _thread.AccountInGame(_account, _status);
            });
        }
    }
}