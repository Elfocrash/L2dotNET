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
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginClient));

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

            InitializeNetwork();
        }

        public void InitializeNetwork()
        {
            RsaPair = ClientManager.Instance.GetScrambledKeyPair();
            BlowfishKey = ClientManager.Instance.GetBlowfishKey();
            _loginCrypt = new LoginCrypt();
            _loginCrypt.UpdateKey(BlowfishKey);

            new Thread(Read).Start();
            new Thread(SendInit).Start();
        }

        public void SendInit()
        {
            Send(Init.ToPacket(this));
        }

        public void Send(Packet p)
        {
            byte[] data = p.GetBuffer();
            data = _loginCrypt.Encrypt(data, 0, data.Length);
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes((short)(data.Length + 2)));
            array.AddRange(data);
            NetStream.Write(array.ToArray(), 0, array.Count);
            //Console.WriteLine($"Recieve :\r\n{L2Buffer.ToString(array.ToArray())}");
            NetStream.Flush();
        }

        public void Read()
        {
            try
            {
                _buffer = new byte[2];
                NetStream.BeginRead(_buffer, 0, 2, OnReceiveCallbackStatic, null);
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                Close();
                throw;
            }
        }

        private void OnReceiveCallbackStatic(IAsyncResult result)
        {
            try
            {
                int rs = NetStream.EndRead(result);

                if (rs <= 0)
                    return;

                short length = BitConverter.ToInt16(_buffer, 0);
                _buffer = new byte[length - 2];
                NetStream.BeginRead(_buffer, 0, length - 2, OnReceiveCallback, result.AsyncState);
            }
            catch (Exception s)
            {
                Log.Warn(Address + $" was closed by force. {s}");
                Close();
            }
        }

        public void Close()
        {
            ClientManager.Instance.RemoveClient(this);
        }

        private void OnReceiveCallback(IAsyncResult result)

        {
            NetStream.EndRead(result);

            byte[] buff = new byte[_buffer.Length];
            _buffer.CopyTo(buff, 0);

            if (!_loginCrypt.Decrypt(ref buff, 0, buff.Length))
                Log.Error($"Blowfish failed on {Address}. Please restart auth server.");
            else
            {
                PacketHandler.Handle(new Packet(1, buff), this);
                Read();
            }
        }



        public int Login1,
                   Login2;

        public void SetLoginPair(int key1, int key2)
        {
            Login1 = key1;
            Login2 = key2;
        }

        public int Play1,
                   Play2;

        public void SetPlayPair(int key1, int key2)
        {
            Play1 = key1;
            Play2 = key2;
        }

        public AccountModel ActiveAccount { get; set; }
    }
}