using System.Collections.Generic;
using L2dotNET.GameService.Controllers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class BypassUserCmd : GameServerNetworkRequest
    {
        public BypassUserCmd(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _command;

        public override void read()
        {
            _command = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            switch (_command)
            {
                case 0: // [loc]
                    int regId = 0; //MapRegionTable.getInstance().getRegionSysId(player.X, player.Y);
                    if (regId > 0)
                        player.SendPacket(new SystemMessage((SystemMessage.SystemMessageId)regId).AddNumber(player.X).AddNumber(player.Y).AddNumber(player.Z));
                    else
                        player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.NOT_IMPLEMENTED_YET_2361).AddString("Nowhere"));

                    int x = (player.X >> 15) + 9 + 8;
                    int y = (player.Y >> 15) + 10 + 11;
                    player.SendMessage($"Current loc is X:{player.X} Y:{player.Y} Z:{player.Z}");
                    player.BroadcastUserInfo(); //for debug reasons
                    break;
                case 52: // /unstuck

                    L2WorldRegion worldRegion = L2World.Instance.GetRegion(player.X, player.Y);
                    player.SetRegion(worldRegion);
                    List<L2Player> knowns = player.GetKnownPlayers();
                    //player.SpawnMe();
                    player.SendMessage("Unstuck not implemented yet.");
                    //player.knownObjects;
                    break;
                case 62: // /dismount
                    player.SendMessage("Dismount not implemented yet.");
                    break;
                case 77: // [time]
                    GameTime.Instance.ShowInfo(player);
                    break;
                default:
                    player.SendMessage("cmd alias " + _command);
                    break;
            }
        }
    }
}