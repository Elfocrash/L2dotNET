using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.LoginService
{
    class LoginServer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginServer));
        public static IServiceProvider ServiceProvider;
        private TcpListener _listener;

        public LoginServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Start()
        {
            // CheckRunningProcesses();
            var config = ServiceProvider.GetService<Config.Config>();
            var serverThreadPool = ServiceProvider.GetService<ServerThreadPool>();

            config.Initialise();
            ServiceProvider.GetService<PreReqValidation>().Initialise();
            ServiceProvider.GetService<Managers.ClientManager>().Initialise();
            serverThreadPool.Initialize();
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
            new Thread(serverThreadPool.Start).Start();

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
            ServiceProvider.GetService<Managers.ClientManager>().AddClient(clientSocket);
        }

        private void CheckRunningProcesses()
        {
            if (Process.GetProcessesByName("L2dotNET.LoginService").Length == 1)
                return;

            Log.Fatal("A L2dotNET.LoginService process is already running!");
            Log.Info("Press ENTER to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}