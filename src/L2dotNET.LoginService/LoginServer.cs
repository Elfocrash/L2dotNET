using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Managers;
using Ninject;

namespace L2dotNET.LoginService
{
    class LoginServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginServer));

        private TcpListener LoginServerListener;

        public static IKernel Kernel { get; set; }

        public LoginServer() { }

        public void Start()
        {
            Config.Config.Instance.Initialize();
            PreReqValidation.Instance.Initialize();
            ClientManager.Instance.Initialize();
            ServerThreadPool.Instance.Initialize();
            NetworkRedirect.Instance.Initialize();

            LoginServerListener = new TcpListener(IPAddress.Parse(Config.Config.Instance.serverConfig.Host), Config.Config.Instance.serverConfig.LoginPort);

            try
            {
                LoginServerListener.Start();
            }
            catch (SocketException ex)
            {
                log.Error($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
                log.Info($"Press ENTER to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            log.Info($"Auth server listening clients at {Config.Config.Instance.serverConfig.Host}:{Config.Config.Instance.serverConfig.LoginPort}");
            new Thread(ServerThreadPool.Instance.Start).Start();

            TcpClient clientSocket;
            while (true)
            {
                clientSocket = LoginServerListener.AcceptTcpClient();
                AcceptClient(clientSocket);
            }
        }

        private void AcceptClient(TcpClient client)
        {
            ClientManager.Instance.addClient(client);
        }
    }
}