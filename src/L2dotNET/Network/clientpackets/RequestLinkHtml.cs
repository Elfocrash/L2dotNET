using System;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestLinkHtml : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _link;

        public RequestLinkHtml(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _link = packet.ReadString();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            // log.Info($"link to '{ _link }'");

            string file;
            int id = 0;
            if (_link.Contains("#"))
            {
                file = _link.Split('#')[0];
                id = int.Parse(_link.Split('#')[1]);
            }
            else
                file = _link;

            int idx = player.Target?.ObjId ?? player.ObjId;

            player.SendPacket(new NpcHtmlMessage(player, file, idx, id));
        }
    }
}