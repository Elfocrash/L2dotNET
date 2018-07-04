﻿using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class CharacterRestore : PacketBase
    {
        public readonly ICharacterService CharacterService;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterRestore(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            CharacterService = serviceProvider.GetService<ICharacterService>();
            _charSlot = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //    return;

            await Task.Run(() =>
            {
                ValidateAndRestore();

                _client.SendPacketAsync(new CharacterSelectionInfo(_client.AccountName, _client.AccountCharacters, _client.SessionKey.PlayOkId1));
            });
        }

        private void ValidateAndRestore()
        {
            L2Player player = _client.AccountCharacters.FirstOrDefault(filter => filter.CharSlot == _charSlot);
            
            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to restore Character in slot {_charSlot} but no characters exits at that slot.");
                return;
            }

            //_playerService.MarkToRestoreChar(player.ObjId);
            player.DeleteTime = null;
        }
    }
}