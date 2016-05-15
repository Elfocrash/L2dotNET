using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.network.loginauth;
using L2dotNET.Services.Contracts;
using Ninject;
using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2recv
{
    class AuthLogin : GameServerNetworkRequest
    {
        [Inject]
        public IAccountService accountService { get { return GameServer.Kernel.Get<IAccountService>(); } }

        public AuthLogin(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _loginName;
        private int _playKey1;
        private int _playKey2;
        private int _loginKey1;
        private int _loginKey2;

        public override void read()
        {
            _loginName = readS();
            _playKey2 = readD();
            _playKey1 = readD();
            _loginKey1 = readD();
            _loginKey2 = readD();
        }

        public override void run()
        {
            if (getClient().AccountName == null)
            {
                getClient().AccountName = _loginName;

                List<int> players = accountService.GetPlayerIdsListByAccountName(_loginName);

                int slot = 0;
                foreach (int id in players)
                {
                    L2Player p = new L2Player().RestorePlayer(id, getClient());
                    p.CharSlot = slot; slot++;
                    Client.AccountChars.Add(p);
                }

                getClient().sendPacket(new CharacterSelectionInfo(getClient().AccountName, getClient().AccountChars, getClient().SessionId));
                AuthThread.Instance.setInGameAccount(getClient().AccountName, true);
            }
            else
                getClient().termination();
        }
    }
}
