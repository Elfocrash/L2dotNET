﻿using System;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.clientpackets
{
    class RequestAcquireSkill : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _id;
        private readonly int _level;
        private readonly int _skillType;

        public RequestAcquireSkill(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _id = packet.ReadInt();
            _level = packet.ReadInt();
            _skillType = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
        }
    }
}