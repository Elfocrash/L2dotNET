﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Logging.Abstraction;
using L2dotNET.LoginService.Model;
using L2dotNET.LoginService.Network;
using L2dotNET.Services.Contracts;
using Mapster;
using NLog;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThreadPool
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ICollection<L2Server> Servers { get; private set; }

        private readonly IServerService _serverService;
        private readonly Config.Config _config;
        private readonly PacketHandler _packetHandler;
        private TcpListener _listener;

        public ServerThreadPool(IServerService serverService, Config.Config config, PacketHandler packetHandler)
        {
            _serverService = serverService;
            _config = config;
            _packetHandler = packetHandler;
        }

        public async Task Initialize()
        {
            IEnumerable<ServerContract> servers = await _serverService.GetServerList();

            Servers = servers.AsQueryable().ProjectToType<L2Server>().ToList();

            Log.Info($"GameServerThread: loaded {Servers.Count()} servers");
        }

        public L2Server Get(short serverId)
        {
            return Servers.FirstOrDefault(s => s.ServerId == serverId);
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(_config.ServerConfig.Host), _config.ServerConfig.GsPort);

            try
            {
                _listener.Start();
                Log.Info($"Auth server listening gameservers at {_config.ServerConfig.Host}:{_config.ServerConfig.GsPort}");
            }
            catch (SocketException ex)
            {
                Log.Halt($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
            }

            Task.Factory.StartNew(WaitForClients);
        }

        private async void WaitForClients()
        {
            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                Log.Info($"Received connection request from: {client.Client.RemoteEndPoint}");

                ServerThread serverThread = new ServerThread(_packetHandler);

                Task.Factory.StartNew(() => serverThread.ReadData(client, this));
            }
        }

        public void Shutdown(byte id)
        {
            L2Server server = Servers.FirstOrDefault(s => s.ServerId == id);

            if (server == null)
            {
                return;
            }

            server.Thread?.Stop();
            server.Thread = null;
            Log.Warn($"ServerThread: #{id} shutted down");
        }

        public bool LoggedAlready(int accountId)
        {
            foreach (L2Server srv in Servers.Where(srv => srv.Thread != null && srv.Thread.LoggedAlready(accountId)))
            {
                srv.Thread.KickAccount(accountId);
                return true;
            }

            return false;
        }

        public void SendPlayer(byte serverId, LoginClient client)
        {
            L2Server server = Servers.FirstOrDefault(srv => srv.ServerId == serverId && srv.Thread != null);
            server?.Thread.SendPlayer(client);
        }
    }
}