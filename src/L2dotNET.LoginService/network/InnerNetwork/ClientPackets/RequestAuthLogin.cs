using System;
using System.Text;
using L2dotNET.DataContracts;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestAuthLogin : PacketBase
    {
        [Inject]
        public IAccountService AccountService => LoginServer.Kernel.Get<IAccountService>();

        protected byte[] Raw;
        private readonly LoginClient _client;

        public RequestAuthLogin(Packet p, LoginClient client)
        {
            _client = client;
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

            CipherParameters key = _client.RsaPair._privateKey;
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

            AccountContract account = AccountService.GetAccountByLogin(username);

            if (account == null)
            {
                if (Config.Config.Instance.ServerConfig.AutoCreate)
                    account = AccountService.CreateAccount(username, L2Security.HashPassword(password));
                else
                {
                    _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }
            }
            else
            {
                if (!AccountService.CheckIfAccountIsCorrect(username, L2Security.HashPassword(password)))
                {
                    _client.Send(LoginFail.ToPacket(LoginFailReason.ReasonUserOrPassWrong));
                    _client.Close();
                    return;
                }

                if (ServerThreadPool.Instance.LoggedAlready(username.ToLower()))
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