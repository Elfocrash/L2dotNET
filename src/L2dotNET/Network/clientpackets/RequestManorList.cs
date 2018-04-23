﻿using System;
using System.Collections.Generic;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestManorList : PacketBase
    {
        private readonly GameClient _client;

        public RequestManorList(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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