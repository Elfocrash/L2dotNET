using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestTutorialLinkHtml : GameServerNetworkRequest
    {
        public RequestTutorialLinkHtml(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private string _link;

        public override void Read()
        {
            _link = ReadS();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (_link.Contains(":"))
            {
                string[] link = _link.Split(':');
                player.SendPacket(new TutorialShowHtml(player, link[0], link[1], player.ViewingAdminPage > 0));
            }
            else
            {
                if (_link.StartsWithIgnoreCase("tutorial_close_"))
                    player.SendPacket(new TutorialCloseHtml());
                else
                {
                    if (_link.EqualsIgnoreCase("admin_close"))
                    {
                        player.SendPacket(new TutorialCloseHtml());
                        player.ViewingAdminPage = 0;
                        player.ViewingAdminTeleportGroup = -1;
                    }
                    else
                        player.SendPacket(new TutorialShowHtml(player, _link, player.ViewingAdminPage > 0));
                }
            }
        }
    }
}