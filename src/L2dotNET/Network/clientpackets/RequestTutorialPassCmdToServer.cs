using L2dotNET.model.player;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets
{
    class RequestTutorialPassCmdToServer : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _alias;

        public RequestTutorialPassCmdToServer(Packet packet, GameClient client)
        {
            _client = client;
            _alias = packet.ReadString();
            if (_alias.Contains("\n"))
                _alias = _alias.Replace("\n", string.Empty);
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            if (_alias.StartsWithIgnoreCase("menu_select?"))
            {
                //_alias = _alias.Replace(" ", string.Empty);
                //string x1 = _alias.Split('?')[1];
                //string[] x2 = x1.Split('&');
                //int ask = int.Parse(x2[0].Substring(4));
                //int reply = int.Parse(x2[1].Substring(6));

                //  npc.onDialog(player, ask, reply);
            }
            else
            {
                if (!_alias.StartsWithIgnoreCase("admin?"))
                    return;

                if (player.ViewingAdminPage == 0)
                {
                    player.SendActionFailed();
                    return;
                }

                if (_alias.Contains("tp"))
                {
                    string[] coord = _alias.Split(' ');
                    int x,
                        y,
                        z;
                    if (!int.TryParse(coord[1], out x) || !int.TryParse(coord[2], out y) || !int.TryParse(coord[3], out z))
                    {
                        player.SendMessage("Only numbers allowed in box.");
                        return;
                    }


                }
                else
                {
                    string x1 = _alias.Split('?')[1];
                    string[] x2 = x1.Split('&');
                    int ask = int.Parse(x2[0].Substring(4));
                    int reply = int.Parse(x2[1].Substring(6));
                }
            }
        }
    }
}