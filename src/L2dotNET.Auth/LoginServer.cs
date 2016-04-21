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

namespace L2dotNET.Auth
{
    class LoginServer
    {
        public LoginServer()
        { }
            
        protected TcpListener LoginListener;
        public static IKernel Kernel { get; set; }

        public void Start()
        {
            Console.Title = "RCS auth";
            Cfg.load();
            ClientManager.getInstance();

            SQLjec.getInstance();

            ServerThreadPool.getInstance();
            NetworkRedirect.getInstance();

            LoginListener = new TcpListener(IPAddress.Parse(Cfg.SERVER_HOST), Cfg.SERVER_PORT);
            LoginListener.Start();
            CLogger.extra_info("Auth server listening clients at " + Cfg.SERVER_HOST + ":" + Cfg.SERVER_PORT);
            new System.Threading.Thread(ServerThreadPool.getInstance().start).Start();
            TcpClient clientSocket = default(TcpClient);
            while (true)
            {
                clientSocket = LoginListener.AcceptTcpClient();
                accept(clientSocket);
            }
        }

        private void accept(TcpClient client)
        {
            ClientManager.getInstance().addClient(client);
        }
    }
}
