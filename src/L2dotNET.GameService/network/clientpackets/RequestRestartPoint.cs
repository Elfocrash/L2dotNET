using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestRestartPoint : GameServerNetworkRequest
    {
        private int _type;
        private int _keyItem = -1;

        public RequestRestartPoint(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        public override void Read()
        {
            _type = ReadD();

            if (_type == 22)
            {
                _keyItem = ReadD();
            }
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

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