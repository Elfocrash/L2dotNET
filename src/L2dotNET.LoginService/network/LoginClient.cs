using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using L2Crypt;
using L2dotNET.LoginService.Managers;
using L2dotNET.LoginService.Network.Crypt;
using L2dotNET.LoginService.Network.InnerNetwork.ClientPackets;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Models;
using L2dotNET.Network;
using L2dotNET.Utility;

namespace L2dotNET.LoginService.Network
{
    public class LoginClient
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginClient));

        public int SessionId;
        public EndPoint Address { get; set; }
        public TcpClient Client { get; set; }
        public NetworkStream NetStream { get; set; }
        private byte[] _buffer;
        private LoginCrypt _loginCrypt;
        public byte[] BlowfishKey;
        public ScrambledKeyPair RsaPair;

        public LoginClient(TcpClient tcpClient)
        {
            Client = tcpClient;
            NetStream = tcpClient.GetStream();
            Address = tcpClient.Client.RemoteEndPoint;
            SessionId = new Random().Next(int.MaxValue);

            initializeNetwork();
        }

        public void initializeNetwork()
        {
            RsaPair = ClientManager.Instance.GetScrambledKeyPair();
            BlowfishKey = ClientManager.Instance.GetBlowfishKey();
            _loginCrypt = new LoginCrypt();
            _loginCrypt.updateKey(BlowfishKey);

            new Thread(read).Start();
            new Thread(SendInit).Start();
        }

        public void SendInit()
        {
            Send(Init.ToPacket(this));
        }

        public void Send(Packet p)
        {
            byte[] data = p.GetBuffer();
            data = _loginCrypt.encrypt(data, 0, data.Length);
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes((short)(data.Length + 2)));
            array.AddRange(data);
            NetStream.Write(array.ToArray(), 0, array.Count);
            Console.WriteLine("Recieve :\r\n{0}", L2Buffer.ToString(array.ToArray()));
            NetStream.Flush();
        }

        public void read()
        {
            try
            {
                _buffer = new byte[2];
                NetStream.BeginRead(_buffer, 0, 2, new AsyncCallback(OnReceiveCallbackStatic), null);
            }
            catch (Exception ex)
            {
                close();
                throw ex;
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            int rs = 0;
            try
            {
                rs = NetStream.EndRead(result);

                if (rs > 0)
                {
                    short Length = BitConverter.ToInt16(_buffer, 0);
                    _buffer = new byte[Length - 2];
                    NetStream.BeginRead(_buffer, 0, Length - 2, new AsyncCallback(OnReceiveCallback), result.AsyncState);
                }
            }
            catch (Exception s)
            {
                log.Warn(Address + $" was closed by force. {s}");
                close();
            }
        }

        public void close()
        {
            ClientManager.Instance.RemoveClient(this);
        }

        private void OnReceiveCallback(IAsyncResult result)

        {
            NetStream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);

            if (!_loginCrypt.decrypt(ref buff, 0, buff.Length))
            {
                log.Error($"Blowfish failed on {Address}. Please restart auth server.");
            }
            else
            {
                Handle(new Packet(1, buff));
                new System.Threading.Thread(read).Start();
            }
        }

        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="packet">Incoming packet.</param>
        protected void Handle(Packet packet)
        {
            switch (packet.FirstOpcode)
            {
                case 0x00:
                    new RequestAuthLogin(packet, this).RunImpl();
                    break;
                case 0x02:
                    new RequestServerLogin(packet, this).RunImpl();
                    break;
                case 0x05:
                    new RequestServerList(packet, this).RunImpl();
                    break;
                case 0x07:
                    new AuthGameGuard(packet, this).RunImpl();
                    break;

                default:
                    log.Warn($"LoginClient: received unk request {packet.FirstOpcode}");
                    break;
            }

            //if (msg != null)
            //    new Thread(new ThreadStart(msg.Run)).Start();
        }

        public int login1,
                   login2;

        public void setLoginPair(int key1, int key2)
        {
            login1 = key1;
            login2 = key2;
        }

        public int play1,
                   play2;

        public void setPlayPair(int key1, int key2)
        {
            play1 = key1;
            play2 = key2;
        }

        public AccountModel ActiveAccount { get; set; }
    }
}