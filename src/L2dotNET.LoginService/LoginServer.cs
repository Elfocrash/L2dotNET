using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Managers;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.LoginService
{
    class LoginServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static IServiceProvider ServiceProvider;
        private TcpListener _listener;

        public LoginServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async void Start()
        {
            Config.Config config = ServiceProvider.GetService<Config.Config>();
            ServerThreadPool serverThreadPool = ServiceProvider.GetService<ServerThreadPool>();

            await config.Initialise();
            await ServiceProvider.GetService<PreReqValidation>().Initialise();
            await ServiceProvider.GetService<Managers.ClientManager>().Initialise();
            await serverThreadPool.Initialize();

            NetworkRedirect.Instance.Initialize();

            _listener = new TcpListener(IPAddress.Parse(config.ServerConfig.Host), config.ServerConfig.LoginPort);

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

            Log.Info($"Auth server listening clients at {config.ServerConfig.Host}:{config.ServerConfig.LoginPort}");

            Task.Factory.StartNew(serverThreadPool.Start);

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

        private void AcceptClient(TcpClient client)
        {
            Log.Debug($"Received connection request from: {client.Client.RemoteEndPoint}");

            ServiceProvider.GetService<Managers.ClientManager>().AddClient(client);
        }
    }
}