using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestUnEquipItem : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _slotBitType;

        public RequestUnEquipItem(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _slotBitType = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (player.PBlockAct == 1)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                //int dollId = player.Inventory.getPaperdollIdByMask(slotBitType);

                //player.setPaperdoll(dollId, null, true);
                player.BroadcastUserInfo();
            });
        }
    }
}