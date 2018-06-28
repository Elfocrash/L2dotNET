using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2Crypt;
using L2dotNET.Logging.Abstraction;
using L2dotNET.LoginService.Network;

namespace L2dotNET.LoginService.Managers
{
    public sealed class ClientManager : IInitialisable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private const int ScrambleCount = 1;
        private ScrambledKeyPair[] _scrambledPairs;
        private const int BlowfishCount = 20;
        private byte[][] _blowfishKeys;
        private readonly PacketHandler _packetHandler;
        private readonly List<LoginClient> _loggedClients = new List<LoginClient>();
        private SortedList<string, LoginClient> _waitingAcc = new SortedList<string, LoginClient>();
        public bool Initialised { get; private set; }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

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
            Initialised = true;
        }

        private NetworkBlock _banned;
        private readonly SortedList<string, DateTime> _flood = new SortedList<string, DateTime>();

        public ClientManager(PacketHandler packetHandler)
        {
            _packetHandler = packetHandler;
        }

        public void AddClient(TcpClient client)
        {
            if (_banned == null)
                _banned = NetworkBlock.Instance;

            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];
            Log.Info($"Connected: {ip}");

            lock (_flood)
            {
                if (_flood.ContainsKey(ip))
                {
                    if (_flood[ip].CompareTo(DateTime.Now) == 1)
                    {
                        Log.Warn($"Active flooder: {ip}");
                        client.Close();
                        return;
                    }

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

            LoginClient lc = new LoginClient(client, this, _packetHandler);
            if (_loggedClients.Contains(lc))
                return;

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
                return;

            _loggedClients.Remove(loginClient);
        }
    }
}