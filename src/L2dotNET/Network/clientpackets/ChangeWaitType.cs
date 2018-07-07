using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class ChangeWaitType : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _standType;

        public ChangeWaitType(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _standType = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                //TODO: Chair/Mount Logic

                if (player.PBlockAct == 1)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                //Do nothing if already attempting to sit or moving
                if (!player.IsSittingInProgress() || !player.CharMovement.IsMoving)
                {
                    switch (_standType)
                    {
                        case 0:
                            if (!player.IsSitting())
                                player.SitAsync();
                            break;
                        case 1:
                            if (player.IsSitting())
                                player.StandAsync();
                            break;
                        case 2:
                            //TODO: Fake Death
                            break;
                        case 3:
                            //TODO: Stop Fake Death
                            break;
                        default:
                            //Invalid wait type should log?
                            return;
                    }
                }
            });
        }
    }
}

