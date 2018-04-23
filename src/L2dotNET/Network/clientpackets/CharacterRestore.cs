using System;
using System.Linq;
using log4net;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class CharacterRestore : PacketBase
    {
        public readonly IPlayerService _playerService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(CharacterDelete));

        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterRestore(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _playerService = serviceProvider.GetService<IPlayerService>();
            _charSlot = packet.ReadInt();
        }

        public override void RunImpl()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //    return;

            ValidateAndRestore();

            _client.SendPacket(new CharacterSelectionInfo(_client.AccountName, _client.AccountChars, _client.SessionKey.PlayOkId1));
        }

        private void ValidateAndRestore()
        {
            L2Player player = _client.AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);
            
            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to restore Character in slot {_charSlot} but no characters exits at that slot.");
                return;
            }

            _playerService.MarkToRestoreChar(player.ObjId);
            player.DeleteTime = 0;
        }
    }
}