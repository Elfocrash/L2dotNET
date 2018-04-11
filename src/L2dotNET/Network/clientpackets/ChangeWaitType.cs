using L2dotNET.Models.player;
using log4net;

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
            switch (_standType)
            {
                case 0:
                    player.Sit();
                    break;
                case 1:
                    player.Stand();
                    break;
                case 2:
                    //Fake Death
                    break;
                case 3:
                    //Stop Fake Death
                    break;
            }
            
        }
    }
}

