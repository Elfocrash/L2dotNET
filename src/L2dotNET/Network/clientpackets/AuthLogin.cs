using System;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Player;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class AuthLogin : PacketBase
    {
        private readonly IAccountService _accountService;
        private readonly AuthThread _authThread;
        private readonly IPlayerService _playerService;

        private readonly GameClient _client;
        private readonly string _loginName;
        private readonly int _playKey1;
        private readonly int _playKey2;
        private readonly int _loginKey1;
        private readonly int _loginKey2;

        public AuthLogin(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _authThread = serviceProvider.GetService<AuthThread>();
            _accountService = serviceProvider.GetService<IAccountService>();
            _playerService = serviceProvider.GetService<IPlayerService>();
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
                _client.SessionKey = new SessionKey(_loginKey1,_loginKey2, _playKey1, _playKey2);

                _client.AccountName = _loginName;

                List<int> players = _accountService.GetPlayerIdsListByAccountName(_loginName);

                int slot = 0;
                foreach (L2Player p in players.Select(id => _playerService.RestorePlayer(id, _client)))
                {
                    //TODO: Make delete on startup server or timer listener
                    // See if the char must be deleted
                    if (p.CharDeleteTimeExpired())
                    {
                        _playerService.DeleteCharByObjId(p.ObjId);
                        continue;
                    }

                    p.CharSlot = slot;
                    slot++;
                    _client.AccountChars.Add(p);
                }

                _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionKey.PlayOkId1));
                _authThread.SetInGameAccount(_client.AccountName, true);
            }
            else
                _client.Termination();
        }
    }
}