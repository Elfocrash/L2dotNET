using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class AuthLogin : GameServerNetworkRequest
    {
        [Inject]
        public IAccountService AccountService
        {
            get { return GameServer.Kernel.Get<IAccountService>(); }
        }

        public AuthLogin(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private string _loginName;
        private int _playKey1;
        private int _playKey2;
        private int _loginKey1;
        private int _loginKey2;

        public override void Read()
        {
            _loginName = ReadS();
            _playKey2 = ReadD();
            _playKey1 = ReadD();
            _loginKey1 = ReadD();
            _loginKey2 = ReadD();
        }

        public override void Run()
        {
            if (GetClient().AccountName == null)
            {
                GetClient().AccountName = _loginName;

                List<int> players = AccountService.GetPlayerIdsListByAccountName(_loginName);

                int slot = 0;
                foreach (L2Player p in players.Select(id => new L2Player().RestorePlayer(id, GetClient())))
                {
                    p.CharSlot = slot;
                    slot++;
                    Client.AccountChars.Add(p);
                }

                GetClient().SendPacket(new CharacterSelectionInfo(GetClient().AccountName, GetClient().AccountChars, GetClient().SessionId));
                AuthThread.Instance.SetInGameAccount(GetClient().AccountName, true);
            }
            else
                GetClient().Termination();
        }
    }
}