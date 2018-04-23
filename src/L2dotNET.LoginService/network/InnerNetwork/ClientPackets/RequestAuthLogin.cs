using System;
using System.Text;
using L2dotNET.DataContracts;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestAuthLogin : PacketBase
    {
        private readonly IAccountService _accountService;
        private readonly Config.Config _config;
        protected byte[] Raw;
        private readonly LoginClient _client;

        public RequestAuthLogin(IServiceProvider serviceProvider, Packet p, LoginClient client) : base(serviceProvider)
        {
            _accountService = serviceProvider.GetService<IAccountService>();
            _client = client;
            _config = serviceProvider.GetService<Config.Config>();
            Raw = p.ReadByteArrayAlt(128);
        }

        public override void RunImpl()
        {
            if (_client.State != LoginClientState.AuthedGG)
            {
                _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }
            
            var key = _client.RsaPair._privateKey;
            RSAEngine rsa = new RSAEngine();
            rsa.init(false, key);

            byte[] decrypt = rsa.processBlock(Raw, 0, 128);

            if (decrypt.Length < 128)
            {
                byte[] temp = new byte[128];
                Array.Copy(decrypt, 0, temp, 128 - decrypt.Length, decrypt.Length);
                decrypt = temp;
            }

            string username = Encoding.ASCII.GetString(decrypt, 0x5e, 14).Replace("\0", string.Empty);
            string password = Encoding.ASCII.GetString(decrypt, 0x6c, 16).Replace("\0", string.Empty);

            AccountContract account = _accountService.GetAccountByLogin(username);

            if (account == null)
            {
                if (_config.ServerConfig.AutoCreate)
                    account = _accountService.CreateAccount(username, L2Security.HashPassword(password));
                else
                {
                    _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }
            }
            else
            {
                if (!_accountService.CheckIfAccountIsCorrect(username, L2Security.HashPassword(password)))
                {
                    _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }

                if (LoginServer.ServiceProvider.GetService<ServerThreadPool>().LoggedAlready(username.ToLower()))
                {
                    _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonAccountInUse));
                    _client.Close();
                    return;
                }
            }

            _client.ActiveAccount = account;

            _client.State = LoginClientState.AuthedLogin;
            _client.Send(LoginOk.ToPacket(_client));
        }
    }
}