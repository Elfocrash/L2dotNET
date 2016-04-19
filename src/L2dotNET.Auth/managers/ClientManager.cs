using System;
using System.Collections.Generic;
using System.Net.Sockets;
using L2dotNET.Auth.data;
using L2Crypt;

namespace L2dotNET.Auth
{
    class ClientManager
    {
        private static ClientManager ab = new ClientManager();
        public static ClientManager getInstance()
        {
            return ab;
        }

        private int ScrambleCount = 1;
        private ScrambledKeyPair[] ScrambledPairs;
        private int BlowfishCount = 20;
        private Byte[][] BlowfishKeys;

        private List<LoginClient> _loggedClients = new List<LoginClient>();
        private SortedList<string, LoginClient> _waitingAcc = new SortedList<string, LoginClient>();

        public ClientManager()
        {
            CLogger.info("Loading client manager.");

            CLogger.info("Scrambling keypairs.");

            ScrambledPairs = new ScrambledKeyPair[ScrambleCount];

            for (int i = 0; i < ScrambleCount; i++)
            {
                ScrambledPairs[i] = new ScrambledKeyPair(ScrambledKeyPair.genKeyPair());
            }

            CLogger.info("Scrambled " + ScrambledPairs.Length + " keypairs.");
            CLogger.info("Randomize blowfish keys.");

            BlowfishKeys = new Byte[BlowfishCount][];
            
            for (int i = 0; i < BlowfishCount; i++)
            {
                BlowfishKeys[i] = new Byte[16];
                new Random().NextBytes(BlowfishKeys[i]);
            }

            CLogger.info("Randomized " + BlowfishKeys.Length + " blowfish keys.");
        }

        protected NetworkBlock _banned;
        protected SortedList<string, DateTime> _flood = new SortedList<string, DateTime>();

        public void addClient(TcpClient client)
        {
            if (_banned == null)
                _banned = NetworkBlock.getInstance();

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            Console.WriteLine("connected "+ip);
            if (_flood.ContainsKey(ip))
            {
                if (_flood[ip].CompareTo(DateTime.Now) == 1)
                {
                    CLogger.warning("active flooder " + ip);
                    client.Close();
                    return;
                }
                else
                    lock (_flood)
                    {
                        _flood.Remove(ip);
                    }
            }

            _flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!_banned.allowed(ip))
            {
                client.Close();
                CLogger.error("NetworkBlock: connection attemp failed. " + ip + " banned.");
                return;
            }

            LoginClient lc = new LoginClient(client);
            if (_loggedClients.Contains(lc))
                return;

            _loggedClients.Add(lc);
        }

        public ScrambledKeyPair getScrambledKeyPair()
        {
            return ScrambledPairs[0];
        }

        public byte[] getBlowfishKey()
        {
            return BlowfishKeys[new Random().Next(BlowfishCount -1)];
        }

        public void removeClient(LoginClient loginClient)
        {
            if (!_loggedClients.Contains(loginClient))
                return;


            _loggedClients.Remove(loginClient);
        }
    }
}
