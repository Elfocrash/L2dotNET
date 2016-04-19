using L2dotNET.Game.network.l2send;
using System;
using L2dotNET.Game.model.player;
using L2dotNET.Game.managers;

namespace L2dotNET.Game.network.l2recv
{
    class RequestPartyMatchList : GameServerNetworkRequest
    {
        public RequestPartyMatchList(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }


        private int _status;
        public override void read()
        {
            _status = readD();
        }

        public override void run()
        {
            Console.WriteLine("party "+_status);
            
        }
    }
}
