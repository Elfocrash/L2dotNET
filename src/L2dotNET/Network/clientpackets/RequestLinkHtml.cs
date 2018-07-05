using System;
using System.Threading.Tasks;
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                // Log.Info($"link to '{ _link }'");

                string file;
                int id = 0;
                if (_link.Contains("#"))
                {
                    file = _link.Split('#')[0];
                    id = int.Parse(_link.Split('#')[1]);
                }
                else
                    file = _link;

                int idx = player.Target?.CharacterId ?? player.CharacterId;

                player.SendPacketAsync(new NpcHtmlMessage(player, file, idx, id));
            });
        }
    }
}