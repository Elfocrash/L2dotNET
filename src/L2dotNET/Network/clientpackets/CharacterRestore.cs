using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Services.Contracts;
using L2dotNET.Utility;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace L2dotNET.Network.clientpackets
{
    class CharacterRestore : PacketBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ICrudService<CharacterContract> _characterCrudService;
        private readonly GameClient _client;
        private readonly int _charSlot;

        public CharacterRestore(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _characterCrudService = serviceProvider.GetService<ICrudService<CharacterContract>>();
            _charSlot = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            ValidateAndRestore();

            _client.SendPacketAsync(new CharList(_client.Account.Login, _client.AccountCharacters, _client.SessionKey.PlayOkId1));
        }

        private void ValidateAndRestore()
        {
            L2Player player = _client.AccountCharacters.FirstOrDefault(filter => filter.CharacterSlot == _charSlot);
            
            if (player == null)
            {
                Log.Warn($"{_client.Address} tried to restore Character in slot {_charSlot} but no characters exits at that slot.");
                return;
            }

            player.DeleteTime = null;
            _characterCrudService.Update(player.ToContract());
        }
    }
}