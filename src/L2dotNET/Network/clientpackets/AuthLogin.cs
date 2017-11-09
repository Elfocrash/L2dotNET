using System.Collections.Generic;
using System.Linq;
using L2dotNET.model.player;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.Network.clientpackets
{
    class AuthLogin : PacketBase
    {
        [Inject]
        public IAccountService AccountService { get; set; } = GameServer.Kernel.Get<IAccountService>();

        [Inject]
        public IPlayerService PlayerService { get; set; } = GameServer.Kernel.Get<IPlayerService>();

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
                foreach (L2Player p in players.Select(id => PlayerService.RestorePlayer(id, _client)))
                {
                    //TODO: Make delete on startup server or timer listener
                    // See if the char must be deleted
                    if (p.CharDeleteTimeExpired())
                    {
                        PlayerService.DeleteCharByObjId(p.ObjId);
                        continue;
                    }

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