using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class CharacterSelected : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _charSlot;
        private readonly int _unk1; // new in C4
        private readonly int _unk2; // new in C4
        private readonly int _unk3; // new in C4
        private readonly int _unk4; // new in C4

        public CharacterSelected(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _charSlot = packet.ReadInt();
            _unk1 = packet.ReadShort();
            _unk2 = packet.ReadInt();
            _unk3 = packet.ReadInt();
            _unk4 = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            if (_client.CurrentPlayer != null)
            {
                return;
            }

            L2Player player = _client.AccountCharacters.FirstOrDefault(x => x.CharacterSlot == _charSlot);

            if (player == null)
            {
                Log.Error("Selected character slot is invalid");
                return;
            }

            player.SetOnline(_client);
            _client.AccountCharacters = null;
            _client.SendPacketAsync(new serverpackets.CharacterSelected(player, _client.SessionKey.PlayOkId1));
        }
    }
}