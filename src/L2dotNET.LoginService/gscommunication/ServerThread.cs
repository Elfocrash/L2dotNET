using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using L2dotNET.LoginService.Network;
using L2dotNET.LoginService.Network.InnerNetwork.ResponsePackets;
using L2dotNET.Network;
using L2dotNET.Utility;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.LoginService.GSCommunication
{
    public class ServerThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        private NetworkStream _nstream;
        private TcpClient _client;
        private readonly PacketHandler _packetHandler;
        private readonly ICollection<int> _accountsInGame;


        public string Wan { get; set; }
        public short Port { get; set; }
        public short CurrentPlayers { get; set; }
        public short MaxPlayers { get; set; } = 1000;
        public string Info { get; set; }
        public bool Connected { get; set; }
        public bool TestMode { get; set; }
        public bool GmOnly { get; set; }
        public byte ServerId { get; set; }

        public ServerThread(PacketHandler packetHandler)
        {
            _packetHandler = packetHandler;
            _accountsInGame = new List<int>();
        }

        public async void ReadData(TcpClient tcpClient, ServerThreadPool cn)
        {
            _nstream = tcpClient.GetStream();
            _client = tcpClient;

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2];
                    int bytesRead = await _nstream.ReadAsync(buffer, 0, 2);

                    if (bytesRead != 2)
                    {
                        throw new Exception("Wrong packet");
                    }

                    short length = BitConverter.ToInt16(buffer, 0);

                    buffer = new byte[length];
                    bytesRead = await _nstream.ReadAsync(buffer, 0, length);

                    if (bytesRead != length)
                    {
                        throw new Exception("Wrong packet");
                    }

                    Task.Factory.StartNew(() => _packetHandler.Handle(buffer.ToPacket(), this));
                }
            }
            catch (Exception e)
            {
                Log.Error($"ServerThread: {e.Message}");
                Termination();
            }
        }

        private void Termination()
        {
            LoginServer.ServiceProvider.GetService<ServerThreadPool>().Shutdown(ServerId);
        }

        public async void Send(Packet pk)
        {
            byte[] buffer = pk.GetBuffer();

            byte[] lengthInBytes = BitConverter.GetBytes((short)buffer.Length);

            byte[] message = new byte[buffer.Length + 2];

            lengthInBytes.CopyTo(message, 0);
            buffer.CopyTo(message, 2);

            await _nstream.WriteAsync(message, 0, message.Length);
            await _nstream.FlushAsync();
        }

        public void Close(Packet packet)
        {
            Send(packet);
            LoginServer.ServiceProvider.GetService<ServerThreadPool>().Shutdown(ServerId);
        }

        public void Stop()
        {
            try
            {
                _nstream.Close();
                _client.Close();
            }
            catch (Exception e)
            {
                Log.Error($"ServerThread: {e.Message}");
            }

            _accountsInGame.Clear();
        }

        public void AccountInGame(int accountId, byte status)
        {
            if (status == 1)
            {
                if (!_accountsInGame.Contains(accountId))
                    _accountsInGame.Add(accountId);
            }
            else
            {
                if (_accountsInGame.Contains(accountId))
                    _accountsInGame.Remove(accountId);
            }
        }

        public bool LoggedAlready(int accountId)
        {
            return _accountsInGame.Contains(accountId);
        }

        public void KickAccount(int accountId)
        {
            _accountsInGame.Remove(accountId);
            Send(PleaseKickAccount.ToPacket(accountId));
        }

        public void SendPlayer(LoginClient loginClient)
        {
            Send(PleaseAcceptPlayer.ToPacket(loginClient.ActiveAccount, loginClient.Key));
        }
    }
}