using System;
using System.Threading.Tasks;
using L2dotNET.Models;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestRecordInfo : PacketBase
    {
        private readonly GameClient _client;

        public RequestRecordInfo(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                player.SendPacketAsync(new UserInfo(player));
                player.SendPacketAsync(new ExBrExtraUserInfo(player.CharacterId, player.AbnormalBitMaskEvent));

                foreach (L2Object obj in player.KnownObjects.Values)
                    player.OnAddObject(obj, null, $"Player {player.Name} recording replay with your character.");
            });
        }
    }
}