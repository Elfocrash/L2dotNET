using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using log4net;

namespace L2dotNET.Auth
{
    class LoginServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginServer));

        public LoginServer()
        { }
            
        protected TcpListener LoginListener;
        public static IKernel Kernel { get; set; }

        public void Start()
        {
            Console.Title = "L2dotNET Loginserver";

            Cfg.load();
            ClientManager.Instance.Initialize();
            ServerThreadPool.Instance.Initialize();
            NetworkRedirect.Instance.Initialize();

            LoginListener = new TcpListener(IPAddress.Parse(Cfg.SERVER_HOST), Cfg.SERVER_PORT);
            LoginListener.Start();
            log.Info("Auth server listening clients at " + Cfg.SERVER_HOST + ":" + Cfg.SERVER_PORT);
            new System.Threading.Thread(ServerThreadPool.Instance.Start).Start();
            TcpClient clientSocket = default(TcpClient);
            while (true)
            {
                clientSocket = LoginListener.AcceptTcpClient();
                AcceptClient(clientSocket);
            }
        }

        private void AcceptClient(TcpClient client)
        {
            ClientManager.Instance.addClient(client);
        }
    }
}
