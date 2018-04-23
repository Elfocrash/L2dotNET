using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using L2dotNET.Controllers;
using L2dotNET.Handlers;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Network;
using L2dotNET.Network.loginauth;
using L2dotNET.Tables;
using L2dotNET.Utility;
using L2dotNET.World;
using log4net.Config;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET
{
    public class GameServer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GameServer));

        private TcpListener _listener;

        private readonly IServiceProvider _serviceProvider;

        public GameServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository,new FileInfo("log4net.config"));

            Config.Config.Instance.Initialize();

            _serviceProvider.GetService<PreReqValidation>().Initialize();

            CharTemplateTable.Instance.Initialize();

            NetworkBlock.Instance.Initialize();
            GameTime.Instance.Initialize();

            _serviceProvider.GetService<IdFactory>().Initialize();

            L2World.Instance.Initialize();

            MapRegionTable.Instance.Initialize();
            ZoneTable.Instance.Initialize();

            _serviceProvider.GetService<ItemTable>().Initialize();
            ItemHandler.Instance.Initialize();

            NpcTable.Instance.Initialize();
            Capsule.Instance.Initialize();
            
            BlowFishKeygen.GenerateKeys();

            _serviceProvider.GetService<IAdminCommandHandler>().Initialize();

            _serviceProvider.GetService<AnnouncementManager>().Initialize();

            StaticObjTable.Instance.Initialize();
            _serviceProvider.GetService<SpawnTable>().Initialize();

            HtmCache.Instance.Initialize();

            // PluginManager.Instance.Initialize(this);

            _serviceProvider.GetService<AuthThread>().Initialize();

            _listener = new TcpListener(IPAddress.Any, Config.Config.Instance.ServerConfig.Port);

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

            Log.Info($"Listening Gameservers on port {Config.Config.Instance.ServerConfig.Port}");

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
            _serviceProvider.GetService<ClientManager>().AddClient(clientSocket);
        }
    }
}