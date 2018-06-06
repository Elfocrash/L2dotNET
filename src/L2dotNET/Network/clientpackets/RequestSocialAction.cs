using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestSocialAction : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _actionId;

        public RequestSocialAction(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _actionId = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;
                if (player == null)
                    return;

                if ((_actionId < 2) || (_actionId > 13))
                    return;

                player.BroadcastPacketAsync(new SocialAction(player.ObjId, _actionId));
            });
        }
    }
}