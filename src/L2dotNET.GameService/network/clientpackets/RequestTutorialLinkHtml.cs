using System;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTutorialLinkHtml : GameServerNetworkRequest
    {
        public RequestTutorialLinkHtml(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private string _link;

        public override void read()
        {
            _link = readS();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (_link.Contains(":"))
            {
                string[] link = _link.Split(':');
                player.sendPacket(new TutorialShowHtml(player, link[0], link[1], player.ViewingAdminPage > 0));
            }
            else if (_link.StartsWith("tutorial_close_", StringComparison.InvariantCultureIgnoreCase))
                player.sendPacket(new TutorialCloseHtml());
            else if (_link.Equals("admin_close"))
            {
                player.sendPacket(new TutorialCloseHtml());
                player.ViewingAdminPage = 0;
                player.ViewingAdminTeleportGroup = -1;
            }
            else
            {
                player.sendPacket(new TutorialShowHtml(player, _link, player.ViewingAdminPage > 0));
            }
        }
    }
}