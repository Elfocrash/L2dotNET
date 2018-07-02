using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.Controllers;
using L2dotNET.Handlers;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Network;
using L2dotNET.Network.loginauth;
using L2dotNET.Tables;
using L2dotNET.Utility;
using L2dotNET.World;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET
{
    public class GameServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private TcpListener _listener;

        public static IServiceProvider ServiceProvider { get; private set; }

        public GameServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async void Start()
        {
            Config.Config config = ServiceProvider.GetService<Config.Config>();
            await config.Initialise();

            await ServiceProvider.GetService<PreReqValidation>().Initialise();

            CharTemplateTable.Initialize();

            // TODO: refactor NetworkBlock
            NetworkBlock.Instance.Initialize();
            GameTime.Initialize();

            await ServiceProvider.GetService<IdFactory>().Initialise();

            L2World.Initialize();

            MapRegionTable.Initialize();
            ZoneTable.Initialize();

            await ServiceProvider.GetService<ItemTable>().Initialise();
            ItemHandler.Instance.Initialize();

            NpcTable.Instance.Initialize();
            Capsule.Instance.Initialize();
            
            BlowFishKeygen.GenerateKeys();

            await ServiceProvider.GetService<IAdminCommandHandler>().Initialise();

            await ServiceProvider.GetService<AnnouncementManager>().Initialise();

            StaticObjTable.Instance.Initialize();
            await ServiceProvider.GetService<SpawnTable>().Initialise();

            await ServiceProvider.GetService<HtmCache>().Initialise();

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

        private async void WaitForClients()
        {
            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
#pragma warning disable 4014
                Task.Factory.StartNew(() => AcceptClient(client));
#pragma warning restore 4014
            }
        }

        private void AcceptClient(TcpClient clientSocket)
        {
            Log.Debug($"Received connection request from: {clientSocket.Client.RemoteEndPoint}");

            ServiceProvider.GetService<ClientManager>().AddClient(clientSocket);
        }
    }
}