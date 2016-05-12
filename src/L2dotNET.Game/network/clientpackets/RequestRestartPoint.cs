
namespace L2dotNET.GameService.network.l2recv
{
    class RequestRestartPoint : GameServerNetworkRequest
    {
        private int type;
        private int keyItem = -1;
        public RequestRestartPoint(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            type = readD();

            if(type == 22)
                keyItem = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            switch (type)
            {
                case 0: //village
                    break;
                case 1://ch
                    break;
                case 2://castle
                    break;
                case 3://fortress
                    break;
                case 4://outpost
                    break;
                case 5://feather
                    break;

                //20 RPT_BRANCH_START
                case 21://agathion
                    break;
                case 22://item resurrection, RPT_NPC?
                    break;
            }

            player.Revive(100);
        }
    }
}
