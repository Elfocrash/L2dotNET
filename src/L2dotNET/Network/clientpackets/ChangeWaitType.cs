using L2dotNET.Models.player;

namespace L2dotNET.Network.clientpackets
{
    class ChangeWaitType : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _standType;

        public ChangeWaitType(Packet packet, GameClient client)
        {
            _client = client;
            _standType = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            //TODO: Chair/Mount Logic

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            //Do nothing if already attempting to sit or moving
            if (!player.IsSittingInProgress() || !player.IsMoving())
            {
                switch (_standType)
                {
                    case 0:
                        if (!player.IsSitting())
                            player.Sit();
                        break;
                    case 1:
                        if (player.IsSitting())
                            player.Stand();
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
            
        }
    }
}

