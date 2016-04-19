
namespace L2dotNET.Game.network.l2recv
{
    class NetPingResponse : GameServerNetworkRequest
    {
        private int request;
        private int msec;
        private int unk2;
        public NetPingResponse(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            request = readD();
            msec = readD();
            unk2 = readD();
        }

        public override void run()
        {
            Client.CurrentPlayer.UpdatePing(request, msec, unk2);
        }
    }
}
