using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2Crypt;
using L2dotNET.LoginService.Network;
using NLog;

namespace L2dotNET.LoginService.Managers
{
    public sealed class ClientManager : IInitialisable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public bool Initialised { get; private set; }

        private const int ScrambleCount = 1;
        private const int BlowfishCount = 20;

        private ScrambledKeyPair[] _scrambledPairs;
        private byte[][] _blowfishKeys;

        private readonly PacketHandler _packetHandler;
        private readonly ICollection<LoginClient> _loggedClients;

        private readonly ConcurrentDictionary<string, DateTime> _flood = new ConcurrentDictionary<string, DateTime>();

        public ClientManager(PacketHandler packetHandler)
        {
            _packetHandler = packetHandler;
            _loggedClients = new List<LoginClient>();
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            Log.Info("Loading client manager.");

            GenerateScrambledKeys();
            GenerateBlowfishKeys();
            
            Initialised = true;
        }

        public void AddClient(TcpClient client)
        {
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

                    DateTime oldDate;
                    _flood.TryRemove(ip, out oldDate);
                }

            _flood.AddOrUpdate(ip, DateTime.UtcNow.AddMilliseconds(3000), (a,b) => DateTime.UtcNow.AddMilliseconds(3000));

            if (!NetworkBlock.Instance.Allowed(ip))
            {
                client.Close();
                Log.Error($"NetworkBlock: connection attemp failed. IP: {ip} banned.");
                return;
            }

            LoginClient loginClient = new LoginClient(client, this, _packetHandler);

            if (_loggedClients.Contains(loginClient))
            {
                return;
            }

            _loggedClients.Add(loginClient);
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

        private void GenerateScrambledKeys()
        {
            Log.Info("Scrambling keypairs.");

            _scrambledPairs = new ScrambledKeyPair[ScrambleCount];

            for (int i = 0; i < ScrambleCount; i++)
            {
                _scrambledPairs[i] = new ScrambledKeyPair(ScrambledKeyPair.genKeyPair());
            }

            Log.Info($"Scrambled {_scrambledPairs.Length} keypairs.");
        }

        private void GenerateBlowfishKeys()
        {
            Log.Info("Randomize blowfish keys.");

            _blowfishKeys = new byte[BlowfishCount][];

            for (int i = 0; i < BlowfishCount; i++)
            {
                _blowfishKeys[i] = new byte[16];
                new Random().NextBytes(_blowfishKeys[i]);
            }

            Log.Info($"Randomized {_blowfishKeys.Length} blowfish keys.");
        }
    }
}