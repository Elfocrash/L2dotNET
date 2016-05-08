using System;
using System.Text;
using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.serverpackets;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using L2dotNET.Models;
using Ninject;
using L2dotNET.Services.Contracts;
using L2dotNET.Auth.Utils;

namespace L2dotNET.Auth.rcv_l2
{
    class RequestAuthLogin : ReceiveBasePacket
    {
        [Inject]
        public IAccountService accountService { get { return LoginServer.Kernel.Get<IAccountService>(); } }

        public RequestAuthLogin(LoginClient Client, byte[] data)
        {
            base.makeme(Client, data);
        }

        protected byte[] _raw = null;

        public override void read()
        {
            _raw = readB(128);
        }

        public override void run()
        {
            string username, password;

            CipherParameters key = getClient().RsaPair._privateKey;
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
                    getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    getClient().close();
                    return;
                }
            }
            else
            {
                if (!accountService.CheckIfAccountIsCorrect(username, L2Security.HashPassword(password)))
                {
                    getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_USER_OR_PASS_WRONG));
                    getClient().close();
                    return;
                }

                if(ServerThreadPool.Instance.LoggedAlready(username.ToLower()))
                {
                    getClient().sendPacket(new LoginFail(getClient(), LoginFail.LoginFailReason.REASON_ACCOUNT_IN_USE));
                    getClient().close();
                    return;
                }
            }

            Random rnd = new Random();

            getClient().ActiveAccount = account;
            getClient().setLoginPair(rnd.Next(), rnd.Next());
            getClient().setPlayPair(rnd.Next(), rnd.Next());

            getClient().sendPacket(new LoginOk(getClient()));
        }
    }
}
