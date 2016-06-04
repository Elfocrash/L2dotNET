using System;
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using L2Crypt;
using L2dotNET.LoginService.data;

namespace L2dotNET.LoginService
{
    sealed class ClientManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientManager));

        private static volatile ClientManager instance;
        private static object syncRoot = new object();
        private int ScrambleCount = 1;
        private ScrambledKeyPair[] ScrambledPairs;
        private int BlowfishCount = 20;
        private Byte[][] BlowfishKeys;

        private List<LoginClient> _loggedClients = new List<LoginClient>();
        private SortedList<string, LoginClient> _waitingAcc = new SortedList<string, LoginClient>();

        public static ClientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ClientManager();
                        }
                    }
                }

                return instance;
            }
        }

        public ClientManager()
        {

        }

        public void Initialize()
        {
            log.Info("Loading client manager.");

            log.Info("Scrambling keypairs.");

            ScrambledPairs = new ScrambledKeyPair[ScrambleCount];

            for (int i = 0; i < ScrambleCount; i++)
            {
                ScrambledPairs[i] = new ScrambledKeyPair(ScrambledKeyPair.genKeyPair());
            }

            log.Info($"Scrambled { ScrambledPairs.Length } keypairs.");
            log.Info("Randomize blowfish keys.");

            BlowfishKeys = new Byte[BlowfishCount][];

            for (int i = 0; i < BlowfishCount; i++)
            {
                BlowfishKeys[i] = new Byte[16];
                new Random().NextBytes(BlowfishKeys[i]);
            }

            log.Info($"Randomized { BlowfishKeys.Length } blowfish keys.");
        }

        private NetworkBlock banned;
        private SortedList<string, DateTime> flood = new SortedList<string, DateTime>();

        public void addClient(TcpClient client)
        {
            if (banned == null)
                banned = NetworkBlock.Instance;

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            log.Info($"Connected: { ip }");
            if (flood.ContainsKey(ip))
            {
                if (flood[ip].CompareTo(DateTime.Now) == 1)
                {
                    log.Warn($"Active flooder: { ip }");
                    client.Close();
                    return;
                }
                else
                    lock (flood)
                    {
                        flood.Remove(ip);
                    }
            }

            flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!banned.Allowed(ip))
            {
                client.Close();
                log.Error($"NetworkBlock: connection attemp failed. IP: { ip } banned.");
                return;
            }

            LoginClient lc = new LoginClient(client);
            if (_loggedClients.Contains(lc))
                return;

            _loggedClients.Add(lc);
        }

        public ScrambledKeyPair GetScrambledKeyPair()
        {
            return ScrambledPairs[0];
        }

        public byte[] GetBlowfishKey()
        {
            return BlowfishKeys[new Random().Next(BlowfishCount - 1)];
        }

        public void RemoveClient(LoginClient loginClient)
        {
            if (!_loggedClients.Contains(loginClient))
                return;


            _loggedClients.Remove(loginClient);
        }
    }
}
