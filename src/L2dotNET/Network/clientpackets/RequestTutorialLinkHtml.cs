using System;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets
{
    class RequestTutorialLinkHtml : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _link;

        public RequestTutorialLinkHtml(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _link = packet.ReadString();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                if (_link.Contains(":"))
                {
                    string[] link = _link.Split(':');
                    player.SendPacketAsync(new TutorialShowHtml(player, link[0], link[1], player.ViewingAdminPage > 0));
                }
                else
                {
                    if (_link.StartsWithIgnoreCase("tutorial_close_"))
                        player.SendPacketAsync(new TutorialCloseHtml());
                    else
                    {
                        if (_link.EqualsIgnoreCase("admin_close"))
                        {
                            player.SendPacketAsync(new TutorialCloseHtml());
                            player.ViewingAdminPage = 0;
                            player.ViewingAdminTeleportGroup = -1;
                        }
                        else
                            player.SendPacketAsync(new TutorialShowHtml(player, _link, player.ViewingAdminPage > 0));
                    }
                }
            });
        }
    }
}