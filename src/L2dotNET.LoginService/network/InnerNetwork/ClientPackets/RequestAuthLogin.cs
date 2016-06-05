using System;
using System.Text;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.OuterNetwork.ServerPackets;
using L2dotNET.Models;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Ninject;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.LoginService.Network.InnerNetwork.ClientPackets
{
    class RequestAuthLogin
    {
        [Inject]
        public IAccountService accountService
        {
            get { return LoginServer.Kernel.Get<IAccountService>(); }
        }

        protected byte[] _raw = null;
        private readonly LoginClient client;

        public RequestAuthLogin(Packet p, LoginClient client)
        {
            this.client = client;
            _raw = p.ReadByteArrayAlt(128);
        }

        public void RunImpl()
        {
            string username,
                   password;

            CipherParameters key = client.RsaPair._privateKey;
            RSAEngine rsa = new RSAEngine();
            rsa.init(false, key);

            byte[] decrypt = rsa.processBlock(_raw, 0, 128);

            if (decrypt.Length < 128)
            {
                byte[] temp = new byte[128];
                Array.Copy(decrypt, 0, temp, 128 - decrypt.Length, decrypt.Length);
                decrypt = temp;
            }

            username = Encoding.ASCII.GetString(decrypt, 0x5e, 14).Replace("\0", "");
            password = Encoding.ASCII.GetString(decrypt, 0x6c, 16).Replace("\0", "");

            AccountModel account = accountService.GetAccountByLogin(username);

            if (account == null)
            {
                if (Config.Config.Instance.serverConfig.AutoCreate)
                    account = accountService.CreateAccount(username, L2Security.HashPassword(password));
                else
                {
                    client.Send(LoginFail.ToPacket(LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    client.close();
                    return;
                }
            }
            else
            {
                if (!accountService.CheckIfAccountIsCorrect(username, L2Security.HashPassword(password)))
                {
                    client.Send(LoginFail.ToPacket(LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    client.close();
                    return;
                }

                if (ServerThreadPool.Instance.LoggedAlready(username.ToLower()))
                {
                    client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCOUNT_IN_USE));
                    client.close();
                    return;
                }
            }

            Random rnd = new Random();

            client.ActiveAccount = account;
            client.setLoginPair(rnd.Next(), rnd.Next());
            client.setPlayPair(rnd.Next(), rnd.Next());

            client.Send(LoginOk.ToPacket(client));
        }
    }
}