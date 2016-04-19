using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.managers;

namespace L2dotNET.Auth
{
    class Programm
    {
        static void Main(string[] args)
        {
            Programm.getInstance();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static Programm auth = new Programm();

        public static Programm getInstance()
        {
            return auth;
        }

        protected TcpListener LoginListener;

        public Programm()
        {
            Console.Title = "RCS auth";
            Cfg.load();
            ClientManager.getInstance();

            SQLjec.getInstance();

            AccountManager.getInstance();
            ServerThreadPool.getInstance();
            NetworkRedirect.getInstance();

            LoginListener = new TcpListener(IPAddress.Parse(Cfg.SERVER_HOST), Cfg.SERVER_PORT);
            LoginListener.Start();
            CLogger.extra_info("Auth server listening clients at "+Cfg.SERVER_HOST+":"+Cfg.SERVER_PORT);
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
