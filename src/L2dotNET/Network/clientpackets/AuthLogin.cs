using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Models.Player;
using L2dotNET.Network.loginauth;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class AuthLogin : PacketBase
    {
        private readonly ICharacterService _characterService;
        private readonly AuthThread _authThread;

        private readonly GameClient _client;
        private readonly string _loginName;
        private readonly SessionKey _key;

        public AuthLogin(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _authThread = serviceProvider.GetService<AuthThread>();
            _characterService = serviceProvider.GetService<ICharacterService>();

            _loginName = packet.ReadString();

            int _playKey2 = packet.ReadInt();
            int _playKey1 = packet.ReadInt();
            int _loginKey1 = packet.ReadInt();
            int _loginKey2 = packet.ReadInt();

            _key = new SessionKey(_loginKey1, _loginKey2, _playKey1, _playKey2);
        }

        public override async Task RunImpl()
        {
            if (_client.Account != null)
            {
                _client.CloseConnection();
                return;
            }

            Tuple<AccountContract, SessionKey, DateTime> accountTuple = _authThread.GetAwaitingAccount(_loginName);

            if (accountTuple == null)
            {
                Log.Error($"Account is not awaited. Disconnecting. Login: {_loginName}");
                _client.CloseConnection();
                return;
            }

            AccountContract account = accountTuple.Item1;
            SessionKey accountKey = accountTuple.Item2;
            DateTime waitStarTime = accountTuple.Item3;

            // TODO: move 5s to config
            if ((DateTime.UtcNow - waitStarTime).TotalMilliseconds > 5000)
            {
                Log.Error($"Account login timeout. AccountId: {account.AccountId}");
                _client.CloseConnection();
                return;
            }

            if (accountKey != _key)
            {
                Log.Error($"Invalid SessionKey. AccountId: {account.AccountId}");
                _client.CloseConnection();
                return;
            }

            _client.SessionKey = accountKey;

            _client.Account = account;

            await _client.FetchAccountCharacters();

            _client.SendPacketAsync(new CharList(_client, _client.SessionKey.PlayOkId1));
            _authThread.SetInGameAccount(_client.Account.Login, true);

        }
    }
}