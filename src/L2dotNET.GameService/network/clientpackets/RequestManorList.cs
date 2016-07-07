using System.Collections.Generic;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestManorList : GameServerNetworkRequest
    {
        public RequestManorList(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            // do nothing
        }

        public override void Run()
        {
            List<string> manorsName = new List<string>
                                      {
                                          "gludio",
                                          "dion",
                                          "giran",
                                          "oren",
                                          "aden",
                                          "innadril",
                                          "goddard",
                                          "rune",
                                          "schuttgart"
                                      };
            GetClient().SendPacket(new ExSendManorList(manorsName));
        }
    }
}