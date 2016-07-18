using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Player;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestRestartPoint : PacketBase
    {
        private int _type;
        private int _keyItem = -1;
        private readonly GameClient _client;

        public RequestRestartPoint(Packet packet, GameClient client)
        {
            _client = client;
            _type = packet.ReadInt();

            if (_type == 22)
            {
                _keyItem = packet.ReadInt();
            }
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            switch (_type)
            {
                case 0: //village
                    break;
                case 1: //ch
                    break;
                case 2: //castle
                    break;
                case 3: //fortress
                    break;
                case 4: //outpost
                    break;
                case 5: //feather
                    break;

                //20 RPT_BRANCH_START
                case 21: //agathion
                    break;
                case 22: //item resurrection, RPT_NPC?
                    break;
            }

            player.Revive(100);
        }
    }
}