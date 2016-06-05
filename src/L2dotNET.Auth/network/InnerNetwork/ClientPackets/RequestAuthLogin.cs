using System;
using System.Text;
using L2dotNET.LoginService.gscommunication;
using L2dotNET.LoginService.Network.OuterNetwork;
using L2dotNET.LoginService.Utils;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.LoginService.Network.InnerNetwork
{
    class RequestAuthLogin : ReceiveBasePacket
    {
        [Inject]
        public IAccountService accountService
        {
            get { return LoginServer.Kernel.Get<IAccountService>(); }
        }

        public RequestAuthLogin(LoginClient Client, byte[] data)
        {
            base.CreatePacket(Client, data);
        }

        protected byte[] _raw = null;

        public override void Read()
        {
            _raw = ReadByteArray(128);
        }

        public override void Run()
        {
            string username, password;

            CipherParameters key = Client.RsaPair._privateKey;
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
                if (Config.Instance.serverConfig.AutoCreate)
                    account = accountService.CreateAccount(username, L2Security.HashPassword(password));
                else
                {
                    Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    Client.close();
                    return;
                }
            }
            else
            {
                if (!accountService.CheckIfAccountIsCorrect(username, L2Security.HashPassword(password)))
                {
                    Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    Client.close();
                    return;
                }

                if (ServerThreadPool.Instance.LoggedAlready(username.ToLower()))
                {
                    Client.Send(LoginFail.ToPacket(LoginFailReason.REASON_ACCOUNT_IN_USE));
                    Client.close();
                    return;
                }
            }

            Random rnd = new Random();

            Client.ActiveAccount = account;
            Client.setLoginPair(rnd.Next(), rnd.Next());
            Client.setPlayPair(rnd.Next(), rnd.Next());

            Client.Send(LoginOk.ToPacket(Client));
        }
    }
}