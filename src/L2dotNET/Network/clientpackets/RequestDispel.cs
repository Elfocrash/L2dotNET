using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestDispel : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _ownerId;
        private readonly int _skillId;
        private readonly int _skillLv;

        public RequestDispel(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            packet.MoveOffset(2);
            _client = client;
            _ownerId = packet.ReadInt();
            _skillId = packet.ReadInt();
            _skillLv = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (_ownerId != player.ObjId)
                {
                    player.SendActionFailedAsync();
                    return;
                }
            });
        }
    }
}