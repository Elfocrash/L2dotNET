using System;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.Enums;
using L2dotNET.LoginService.Network.OuterNetwork.ResponsePackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.LoginService.Network.OuterNetwork.RequestPackets
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

        public override async Task RunImpl()
        {
            if (_client.State != LoginClientState.AuthedGG)
            {
                await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccessFailed));
                _client.Close();
                return;
            }

            byte[] decrypt = DecryptPacket();

            string username = Encoding.ASCII.GetString(decrypt, 0x5e, 14).Replace("\0", string.Empty);
            string password = Encoding.ASCII.GetString(decrypt, 0x6c, 16).Replace("\0", string.Empty);

            AccountContract account = await _accountService.GetAccountByLogin(username);

            if (account == null)
            {
                if (_config.ServerConfig.AutoCreate)
                {
                    account = await _accountService.CreateAccount(username, password);
                }
                else
                {
                    await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }
            }
            else
            {
                if (!await _accountService.CheckIfAccountIsCorrect(username, password))
                {
                    await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }

                if (LoginServer.ServiceProvider.GetService<ServerThreadPool>().LoggedAlready(account.AccountId))
                {
                    await _client.SendAsync(LoginFail.ToPacket(LoginFailReason.ReasonAccountInUse));
                    _client.Close();
                    return;
                }
            }

            _client.ActiveAccount = account;

            _client.State = LoginClientState.AuthedLogin;
            _client.SendAsync(LoginOk.ToPacket(_client));
        }

        private byte[] DecryptPacket()
        {
            AsymmetricKeyParameter key = _client.RsaPair._privateKey;
            RSAEngine rsa = new RSAEngine();
            rsa.init(false, key);

            byte[] decrypt = rsa.processBlock(Raw, 0, 128);

            if (decrypt.Length < 128)
            {
                byte[] temp = new byte[128];
                Array.Copy(decrypt, 0, temp, 128 - decrypt.Length, decrypt.Length);
                return temp;
            }

            return decrypt;
        }
    }
}