using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2Crypt;
using L2dotNET.DataContracts;
using L2dotNET.LoginService.Network.Crypt;
using L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets;
using L2dotNET.Network;
using L2dotNET.Utility;
using NLog;

namespace L2dotNET.LoginService.Network
{
    public class LoginClient
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public LoginClientState State { get; set; }
        public EndPoint Address { get; }
        public SessionKey Key { get; }
        public int SessionId { get; }
        public AccountContract ActiveAccount { get; set; }

        // Crypt
        private LoginCrypt _loginCrypt;
        public ScrambledKeyPair RsaPair;
        public byte[] BlowfishKey;

        private readonly PacketHandler _packetHandler;
        private readonly Managers.ClientManager _clientManager;
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _networkStream;

        public LoginClient(TcpClient tcpClient, Managers.ClientManager clientManager, PacketHandler packetHandler)
        {
            _tcpClient = tcpClient;
            _clientManager = clientManager;
            _packetHandler = packetHandler;
            _networkStream = tcpClient.GetStream();
            Address = tcpClient.Client.RemoteEndPoint;
            SessionId = RandomThreadSafe.Instance.Next();
            Key = new SessionKey(RandomThreadSafe.Instance.Next(), RandomThreadSafe.Instance.Next(), RandomThreadSafe.Instance.Next(),
                RandomThreadSafe.Instance.Next());
            State = LoginClientState.Connected;
            InitializeNetwork();
        }

        public void InitializeNetwork()
        {
            RsaPair = _clientManager.GetScrambledKeyPair();
            BlowfishKey = _clientManager.GetBlowfishKey();

            _loginCrypt = new LoginCrypt();
            _loginCrypt.UpdateKey(BlowfishKey);

            Task.Factory.StartNew(ReadAsync);
            Task.Factory.StartNew(SendInit);
        }

        public async Task SendInit()
        {
            await SendAsync(Init.ToPacket(this));
        }

        public async Task SendAsync(Packet p)
        {
            byte[] data = p.GetBuffer();
            data = _loginCrypt.Encrypt(data, 0, data.Length);

            byte[] lengthBytes = BitConverter.GetBytes((short)(data.Length + 2));
            byte[] message = new byte[data.Length + 2];

            lengthBytes.CopyTo(message, 0);
            data.CopyTo(message, 2);

            await _networkStream.WriteAsync(message, 0, message.Length);
            await _networkStream.FlushAsync();
        }

        public async Task ReadAsync()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2];
                    int bytesRead = await _networkStream.ReadAsync(buffer, 0, 2);

                    if (bytesRead == 0)
                    {
                        Log.Debug("Client closed connection");
                        Close();
                        return;
                    }

                    if (bytesRead != 2)
                    {
                        throw new Exception("Wrong package structure");
                    }

                    short length = BitConverter.ToInt16(buffer, 0);

                    buffer = new byte[length - 2];
                    bytesRead = await _networkStream.ReadAsync(buffer, 0, length - 2);

                    if (bytesRead != length - 2)
                    {
                        throw new Exception("Wrong package structure");
                    }

                    if (!_loginCrypt.Decrypt(ref buffer, 0, buffer.Length))
                    {
                        throw new Exception($"Blowfish failed on {Address}. Please restart auth server.");
                    }

#pragma warning disable 4014
                    Task.Factory.StartNew(() => _packetHandler.Handle(new Packet(1, buffer), this));
#pragma warning restore 4014
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                Close();
            }
        }

        public void Close()
        {
            _clientManager.RemoveClient(this);
        }
    }
}