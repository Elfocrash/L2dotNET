using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class CharacterSelected : PacketBase
    {
        private readonly ICharacterService CharacterService;

        private readonly GameClient _client;
        private readonly int _charSlot;
        private readonly int _unk1; // new in C4
        private readonly int _unk2; // new in C4
        private readonly int _unk3; // new in C4
        private readonly int _unk4; // new in C4

        public CharacterSelected(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            CharacterService = serviceProvider.GetService<ICharacterService>();
            _charSlot = packet.ReadInt();
            _unk1 = packet.ReadShort();
            _unk2 = packet.ReadInt();
            _unk3 = packet.ReadInt();
            _unk4 = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            //if (_client.CurrentPlayer == null)
            {
                L2Player player = await CharacterService.GetPlayerBySlotId(_client.AccountName, _charSlot);

                if (player == null)
                    return;

                player.Online = 1;
                player.Gameclient = _client;
                _client.CurrentPlayer = player;

                _client.SendPacketAsync(new serverpackets.CharacterSelected(player, _client.SessionKey.PlayOkId1));
            }
        }
    }
}