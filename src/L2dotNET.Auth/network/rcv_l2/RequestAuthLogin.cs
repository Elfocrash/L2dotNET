using System;
using System.Text;
using L2dotNET.Auth.basetemplate;
using L2dotNET.Auth.data;
using L2dotNET.Auth.gscommunication;
using L2dotNET.Auth.serverpackets;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace L2dotNET.Auth.rcv_l2
{
    class RequestAuthLogin : ReceiveBasePacket
    {
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

            AccountManager am = AccountManager.getInstance();
            L2Account account = am.get(username);

            if (account == null)
            {
                if (Cfg.AUTO_ACCOUNTS)
                    account = am.createAccount(username, password, getClient()._address.ToString());
                else
                {
                    getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.USER_OR_PASS_WRONG));
                    getClient().close();
                    return;
                }
            }
            else
            {
                if (!account.validatePassword(password))
                {
                    getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.USER_OR_PASS_WRONG));
                    getClient().close();
                    return;
                }

                if(ServerThreadPool.getInstance().LoggedAlready(username.ToLower()))
                {
                    getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.ACCOUNT_IN_USE));
                    getClient().close();
                    return;
                }
            }

            if (!account.type.Equals("free"))
            {
                DateTime time; int res = -3;
                switch (account.type)
                {
                    case AccountType.trial:
                        time = DateTime.Parse(account.timeend);
                        res = time.CompareTo(DateTime.Now);
                        break;
                    case AccountType.limited:
                        time = DateTime.Parse(account.timeend);
                        res = time.CompareTo(DateTime.Now);
                        break;
                }

                if (res == -1)
                {
                    getClient().sendPacket(new SM_LOGIN_FAIL(getClient(), SM_LOGIN_FAIL.LoginFailReason.NO_TIME_LEFT));
                    getClient().close();
                    return;
                }
            }


            Random rnd = new Random();

            getClient().activeAccount = account;
            getClient().setLoginPair(rnd.Next(), rnd.Next());
            getClient().setPlayPair(rnd.Next(), rnd.Next());

            getClient().sendPacket(new SM_LOGIN_OK(getClient()));
        }
    }
}
