using System.Collections.Generic;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestManorList : PacketBase
    {
        private readonly GameClient _client;
        public RequestManorList(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
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
            _client.SendPacket(new ExSendManorList(manorsName));
        }
    }
}