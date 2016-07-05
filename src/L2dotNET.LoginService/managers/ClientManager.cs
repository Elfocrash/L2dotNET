using System;
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using L2Crypt;
using L2dotNET.LoginService.Network;

namespace L2dotNET.LoginService.Managers
{
    sealed class ClientManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientManager));

        private static volatile ClientManager _instance;
        private static readonly object SyncRoot = new object();
        private const int ScrambleCount = 1;
        private ScrambledKeyPair[] _scrambledPairs;
        private const int BlowfishCount = 20;
        private byte[][] _blowfishKeys;

        private readonly List<LoginClient> _loggedClients = new List<LoginClient>();
        private SortedList<string, LoginClient> _waitingAcc = new SortedList<string, LoginClient>();

        public static ClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ClientManager();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            Log.Info("Loading client manager.");

            Log.Info("Scrambling keypairs.");

            _scrambledPairs = new ScrambledKeyPair[ScrambleCount];

            for (int i = 0; i < ScrambleCount; i++)
            {
                _scrambledPairs[i] = new ScrambledKeyPair(ScrambledKeyPair.genKeyPair());
            }

            Log.Info($"Scrambled {_scrambledPairs.Length} keypairs.");
            Log.Info("Randomize blowfish keys.");

            _blowfishKeys = new byte[BlowfishCount][];

            for (int i = 0; i < BlowfishCount; i++)
            {
                _blowfishKeys[i] = new byte[16];
                new Random().NextBytes(_blowfishKeys[i]);
            }

            Log.Info($"Randomized {_blowfishKeys.Length} blowfish keys.");
        }

        private NetworkBlock _banned;
        private readonly SortedList<string, DateTime> _flood = new SortedList<string, DateTime>();

        public void AddClient(TcpClient client)
        {
            if (_banned == null)
            {
                _banned = NetworkBlock.Instance;
            }

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            Log.Info($"Connected: {ip}");
            if (_flood.ContainsKey(ip))
            {
                if (_flood[ip].CompareTo(DateTime.Now) == 1)
                {
                    Log.Warn($"Active flooder: {ip}");
                    client.Close();
                    return;
                }

                lock (_flood)
                {
                    _flood.Remove(ip);
                }
            }

            _flood.Add(ip, DateTime.Now.AddMilliseconds(3000));

            if (!_banned.Allowed(ip))
            {
                client.Close();
                Log.Error($"NetworkBlock: connection attemp failed. IP: {ip} banned.");
                return;
            }

            LoginClient lc = new LoginClient(client);
            if (_loggedClients.Contains(lc))
            {
                return;
            }

            _loggedClients.Add(lc);
        }

        public ScrambledKeyPair GetScrambledKeyPair()
        {
            return _scrambledPairs[0];
        }

        public byte[] GetBlowfishKey()
        {
            return _blowfishKeys[new Random().Next(BlowfishCount - 1)];
        }

        public void RemoveClient(LoginClient loginClient)
        {
            if (!_loggedClients.Contains(loginClient))
            {
                return;
            }

            _loggedClients.Remove(loginClient);
        }
    }
}