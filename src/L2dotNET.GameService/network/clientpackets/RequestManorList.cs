using System.Collections.Generic;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestManorList : GameServerNetworkRequest
    {
        public RequestManorList(GameClient client, byte[] data)
        {
            makeme(client, data, 2);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            List<string> manorsName = new List<string>();
            manorsName.Add("gludio");
            manorsName.Add("dion");
            manorsName.Add("giran");
            manorsName.Add("oren");
            manorsName.Add("aden");
            manorsName.Add("innadril");
            manorsName.Add("goddard");
            manorsName.Add("rune");
            manorsName.Add("schuttgart");
            getClient().SendPacket(new ExSendManorList(manorsName));
        }
    }
}