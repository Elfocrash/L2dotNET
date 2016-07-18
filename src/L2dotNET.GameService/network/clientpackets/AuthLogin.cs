using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class AuthLogin : PacketBase
    {
        [Inject]
        public IAccountService AccountService => GameServer.Kernel.Get<IAccountService>();

        private readonly GameClient _client;
        private readonly string _loginName;
        private readonly int _playKey1;
        private readonly int _playKey2;
        private readonly int _loginKey1;
        private readonly int _loginKey2;

        public AuthLogin(Packet packet, GameClient client)
        {
            _client = client;
            _loginName = packet.ReadString();
            _playKey2 = packet.ReadInt();
            _playKey1 = packet.ReadInt();
            _loginKey1 = packet.ReadInt();
            _loginKey2 = packet.ReadInt();
        }

        public override void RunImpl()
        {
            if (_client.AccountName == null)
            {
                _client.AccountName = _loginName;

                List<int> players = AccountService.GetPlayerIdsListByAccountName(_loginName);

                int slot = 0;
                foreach (L2Player p in players.Select(id => new L2Player().RestorePlayer(id, _client)))
                {
                    p.CharSlot = slot;
                    slot++;
                    _client.AccountChars.Add(p);
                }

                _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionId));
                AuthThread.Instance.SetInGameAccount(_client.AccountName, true);
            }
            else
                _client.Termination();
        }
    }
}