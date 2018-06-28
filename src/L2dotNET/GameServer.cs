﻿using System;
using System.Net;
using System.Net.Sockets;

using L2dotNET.Controllers;
using L2dotNET.Handlers;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Network;
using L2dotNET.Network.loginauth;
using L2dotNET.Tables;
using L2dotNET.Utility;
using L2dotNET.World;
using L2dotNET.Logging.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET
{
    public class GameServer
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private TcpListener _listener;

        public static IServiceProvider ServiceProvider { get; private set; }

        public GameServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Start()
        {
            var config = ServiceProvider.GetService<Config.Config>();
            config.Initialise();

            ServiceProvider.GetService<PreReqValidation>().Initialise();

            CharTemplateTable.Instance.Initialize();

            NetworkBlock.Instance.Initialize();
            GameTime.Instance.Initialize();

            ServiceProvider.GetService<IdFactory>().Initialise();

            L2World.Instance.Initialize();

            MapRegionTable.Instance.Initialize();
            ZoneTable.Instance.Initialize();

            ServiceProvider.GetService<ItemTable>().Initialise();
            ItemHandler.Instance.Initialize();

            NpcTable.Instance.Initialize();
            Capsule.Instance.Initialize();
            
            BlowFishKeygen.GenerateKeys();

            ServiceProvider.GetService<IAdminCommandHandler>().Initialise();

            ServiceProvider.GetService<AnnouncementManager>().Initialise();

            StaticObjTable.Instance.Initialize();
            ServiceProvider.GetService<SpawnTable>().Initialise();

            ServiceProvider.GetService<HtmCache>().Initialise();

            // PluginManager.Instance.Initialize(this);

            ServiceProvider.GetService<AuthThread>().Initialise();

            _listener = new TcpListener(IPAddress.Any, config.ServerConfig.Port);

            try
            {
                _listener.Start();
            }
            catch (SocketException ex)
            {
                Log.Error($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
                Log.Info("Press ENTER to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            Log.Info($"Listening Gameservers on port {config.ServerConfig.Port}");

            WaitForClients();
        }

        private void WaitForClients()
        {
            _listener.BeginAcceptTcpClient(OnClientConnected, null);
        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            TcpClient clientSocket = _listener.EndAcceptTcpClient(asyncResult);

            Log.Info($"Received connection request from: {clientSocket.Client.RemoteEndPoint}");

            AcceptClient(clientSocket);

            WaitForClients();
        }

        /// <summary>Handle Client Request</summary>
        private void AcceptClient(TcpClient clientSocket)
        {
            ServiceProvider.GetService<ClientManager>().AddClient(clientSocket);
        }
    }
}